
public class Transaction
{
    public DateTime Date{ get; set; }
    public string Type { get; set; }
    public string AccountType { get; set; }
    public string? AccountTo { get; set; }
    public string? AccountFrom { get; set; }
    public decimal Amount { get; set; }
}