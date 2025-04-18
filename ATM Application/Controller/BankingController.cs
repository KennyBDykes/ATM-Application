using Microsoft.AspNetCore.Mvc;

[Route("api/v1/[controller]")]
[ApiController]
public class BankingController : ControllerBase
{
    private  IBankingService _bankingService;
    public BankingController(IBankingService bankingService)
    {
      _bankingService = bankingService;
    }
    [HttpGet("balance/{type}")]
    public ActionResult GetAccountBalance([FromRoute] string type)
    {
      try
      {
      if (type != "Checking" && type != "Savings")
       {
       return BadRequest("Invalid account type. Must be 'Checking' or 'Savings'.");
       }
      var getBalance = _bankingService.GetAccountBalance(type);

      return Ok(getBalance);
      }
      catch(ArgumentException e)
      {
        return NotFound(e.Message);
      }
    }
    [HttpGet("transactions/{accountType}")]
    public ActionResult GetTransactionHistory([FromRoute]string accountType)
    {
    try
    {
    if (accountType != "Checking" && accountType != "Savings")
    {
        return BadRequest("Invalid account type. Must be 'Checking' or 'Savings'.");
    }
     var  transactionHistory = _bankingService.GetTransactionHistory(accountType);
     return Ok(transactionHistory);
    }  
    catch(ArgumentException e)
      {
         return NotFound(e.Message);
      }
    }
    [HttpPut("deposit")]
    public ActionResult Deposit([FromBody] RequestDto dto)
    {
        try
        {
         _bankingService.Deposit(dto);
         return NoContent();
        }    
      catch(ArgumentException e)
      {
         return NotFound(e.Message);
      }
    }
    [HttpPut("withdrawal")]
    public ActionResult Withdrawal([FromBody] RequestDto dto)
    {
    try{
        _bankingService.Withdrawal(dto);
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
    public ActionResult Transfer([FromBody] TransferDto dto)
    {
        try
        {
         _bankingService.Transfer(dto);
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