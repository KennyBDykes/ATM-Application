
public class Transaction
{
    public Guid Id{ get; set; } = Guid.NewGuid();
    public DateTime Date{ get; set; }
    public string Type { get; set; }
    public string AccountType { get; set; }
    public string? AccountTo { get; set; }
    public string? AccountFrom { get; set; }
    public decimal Amount { get; set; }
}