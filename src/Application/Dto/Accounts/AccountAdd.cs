using Domain.Entities;

namespace Application.Dto.Accounts;

public class AccountAdd
{
    public Guid UserId { get; set; }
    public AccountType Type { get; set; }
}
