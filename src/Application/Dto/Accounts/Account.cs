using Domain.Entities;

namespace Application.Dto.Accounts;

public class Account
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
}
