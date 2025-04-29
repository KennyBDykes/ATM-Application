using ATM_Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace ATM_Application.Controller
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class BankingController : ControllerBase
  {
    private  IBankingService _bankingService;
    public BankingController(IBankingService bankingService)
    {
      _bankingService = bankingService;
    }
    [HttpGet("balance/{accountType}")]
    public async Task <ActionResult> GetAccountBalance([FromRoute] AccountType accountType)
    {
      try
      {
      if (accountType != AccountType.Checking && accountType != AccountType.Savings)
       {
       return BadRequest("Invalid account type. Must be 'Checking' or 'Savings'.");
       }
      var getBalance = await _bankingService.GetAccountBalance(accountType);

      return Ok(getBalance);
      }
      catch(ArgumentException e)
      {
        return NotFound(e.Message);
      }
    }
    [HttpGet("transactions/{accountType}")]
    public async Task <ActionResult> GetTransactionHistory([FromRoute]AccountType accountType)
    {
    try
    {
    if (accountType != AccountType.Checking && accountType != AccountType.Savings)
    {
        return BadRequest("Invalid account type. Must be 'Checking' or 'Savings'.");
    }
     var  transactionHistory = await _bankingService.GetTransactionHistory(accountType);
     return Ok(transactionHistory);
    }  
    catch(ArgumentException e)
      {
         return NotFound(e.Message);
      }
    }
    [HttpPut("deposit")]
    public async Task <ActionResult> Deposit([FromBody] RequestDto dto)
    {
        try
        {
         await _bankingService.Deposit(dto);
         return NoContent();
        }    
      catch(ArgumentException e)
      {
         return NotFound(e.Message);
      }
    }
    [HttpPut("withdrawal")]
    public async Task<ActionResult> Withdrawal([FromBody] RequestDto dto)
    {
    try{
        await _bankingService.Withdrawal(dto);
        return NoContent();
        }
     catch(InvalidDataException e)
     {
        return BadRequest(e.Message);
     }
     catch(ArgumentException e)
      {
        return NotFound(e.Message);
      }
    }
    [HttpPut("transfer")]
    public async Task<ActionResult> Transfer([FromBody] TransferDto dto)
    {
        try
        {
         await _bankingService.Transfer(dto);
         return NoContent();
        }  
        catch(InvalidDataException e)
         {
          return BadRequest(e.Message);
        }
         catch(ArgumentException e)
         {
          return NotFound(e.Message);
         }
    }
  }
}