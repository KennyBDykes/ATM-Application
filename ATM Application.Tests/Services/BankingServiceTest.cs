using Moq;
using Xunit;
using ATM_Application.Services;        
using ATM_Application.Repositories;   
using ATM_Application.Models;          
using System.Threading.Tasks;
using System;
using System.IO;


    public class BankingServiceTest
    {
        private readonly Mock<IAccountRepository> _mockAccountRepo;
        private readonly Mock<ITransactionRepository> _mockTransactionRepo;
        private readonly BankingService _bankingService;

        public BankingServiceTest()
        {
            _mockAccountRepo = new Mock<IAccountRepository>();
            _mockTransactionRepo = new Mock<ITransactionRepository>();
            _bankingService = new BankingService(_mockAccountRepo.Object, _mockTransactionRepo.Object);
        }

        [Fact]
        public async Task Deposit_HappyPath()
        {
               // Arrange
            var depositAmount = 100m;
            var accountType = AccountType.Checking;

            var requestDto = new RequestDto
            {
                AccountType = accountType,
                Amount = depositAmount
            };

            _mockAccountRepo.Setup(x => x.Deposit(It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns(Task.CompletedTask);

            _mockTransactionRepo.Setup(x => x.AddTransaction(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            // Act
            await _bankingService.Deposit(requestDto);

            // Assert
            _mockAccountRepo.Verify(x => x.Deposit("Checking", depositAmount), Times.Once);
            _mockTransactionRepo.Verify(x => x.AddTransaction(It.Is<Transaction>(
                t => t.Amount == depositAmount && t.Type == "Deposit" && t.AccountType == "Checking"
            )), Times.Once);
        }

        [Fact]
        public async Task Withdraw_HappyPath()
        {
          // Arrange
         var withdrawalAmount = 50m;
         var startingBalance = 100m;
         var accountType = AccountType.Savings;

          var requestDto = new RequestDto
          {
           AccountType = accountType,
           Amount = withdrawalAmount
          };

         _mockAccountRepo.Setup(x => x.GetAccountBalance("Savings"))
         .ReturnsAsync(startingBalance);

         _mockAccountRepo.Setup(x => x.Withdrawal("Savings", withdrawalAmount))
         .Returns(Task.CompletedTask);

         _mockTransactionRepo.Setup(x => x.AddTransaction(It.IsAny<Transaction>()))
         .Returns(Task.CompletedTask);

         // Act
         await _bankingService.Withdrawal(requestDto);

         // Assert
         _mockAccountRepo.Verify(x => x.GetAccountBalance("Savings"), Times.Once);
         _mockAccountRepo.Verify(x => x.Withdrawal("Savings", withdrawalAmount), Times.Once);
         _mockTransactionRepo.Verify(x => x.AddTransaction(It.Is<Transaction>(
          t => t.Amount == withdrawalAmount && t.Type == "Withdrawal" && t.AccountType == "Savings"
         )), Times.Once);
        
      }

        [Fact]
       public async Task Withdrawal_ShouldThrow_WhenInsufficientFunds()
        {
             var accountType = AccountType.Checking;
             var dto = new RequestDto { AccountType = accountType, Amount = 200 };

             _mockAccountRepo.Setup(repo => repo.GetAccountBalance("Checking"))
                    .ReturnsAsync(100);

             var exception = await Assert.ThrowsAsync<InvalidDataException>(
             () => _bankingService.Withdrawal(dto)
             );

            Assert.Equal("Not enough money in account to withdraw", exception.Message);
            _mockAccountRepo.Verify(r => r.Withdrawal(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
        }
 
        [Fact]
        public async Task Transfer_HappyPath()
        {
            // Arrange
            var transferAmount = 100m;
            var startingBalance = 200m;

            var transferDto = new TransferDto
              {
                 AccountFrom = AccountType.Checking,
                 AccountTo = AccountType.Savings,
                  Amount = transferAmount
              };

             _mockAccountRepo.Setup(x => x.GetAccountBalance("Checking"))
             .ReturnsAsync(startingBalance);

             _mockAccountRepo.Setup(x => x.Transfer("Checking", "Savings", transferAmount))
            .Returns(Task.CompletedTask);

             _mockTransactionRepo.Setup(x => x.AddTransaction(It.IsAny<Transaction>()))
             .Returns(Task.CompletedTask);

             // Act
            await _bankingService.Transfer(transferDto);

            // Assert
           _mockAccountRepo.Verify(x => x.GetAccountBalance("Checking"), Times.Once);
           _mockAccountRepo.Verify(x => x.Transfer("Checking", "Savings", transferAmount), Times.Once);

           _mockTransactionRepo.Verify(x => x.AddTransaction(It.Is<Transaction>(t =>
           t.AccountType == "Checking" && t.Amount == transferAmount && t.Type == "Transfer"
             )), Times.Once);

           _mockTransactionRepo.Verify(x => x.AddTransaction(It.Is<Transaction>(t =>
           t.AccountType == "Savings" && t.Amount == transferAmount && t.Type == "Transfer"
             )), Times.Once);
        }

        [Fact]
        public async Task Transfer_ShouldThrow_WhenSameAccount()
        {
            var dto = new TransferDto
             {
                AccountFrom = AccountType.Savings,
                AccountTo = AccountType.Savings,
                Amount = 100
             };

             var exception = await Assert.ThrowsAsync<InvalidDataException>(
              () => _bankingService.Transfer(dto)
             );

             Assert.Equal("Attempting to transfer from and to the same account", exception.Message);
        }
        
        [Fact]
        public async Task Transfer_ShouldThrow_WhenSameAccountType()
        {
            var dto = new TransferDto
            {
                AccountFrom = AccountType.Checking,
                AccountTo = AccountType.Checking,
                Amount = 100
            };

            await Assert.ThrowsAsync<InvalidDataException>(() => _bankingService.Transfer(dto));
        }
        
    
}