namespace Domain.Entities;

public enum AccountType
{
    Default,
    Saving,
}

public class AccountEntity : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
}
