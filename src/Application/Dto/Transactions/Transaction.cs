using Domain.Entities;

namespace Application.Dto.Transactions;

public class Transaction
{
    public DateTime Created { get; set; }
    public Guid Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public Guid AccountId { get; set; }
}
