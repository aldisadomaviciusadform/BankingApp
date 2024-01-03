namespace Application.Dto.Transactions;

public class TransactionSend
{
    public decimal Amount { get; set; }
    public Guid ReceiverAccount { get; set; }
}
