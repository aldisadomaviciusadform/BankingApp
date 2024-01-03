using Application.Dto.Accounts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;

    public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
    }

    public async Task<Account> Get(Guid id)
    {
        AccountEntity accountEntity = await _accountRepository.Get(id) ?? throw new NotFoundException("Account not found in DB");

        Account account = new()
        {
            Id = id,
            UserId = accountEntity.UserId,
            Type = accountEntity.Type,
            Balance = accountEntity.Balance,
        };

        return account;
    }

    public async Task<List<Account>> GetUserAccounts(Guid id)
    {
        List<Account> accounts = [];
        IEnumerable<AccountEntity> accountsEntities = await _accountRepository.GetUserAccounts(id);

        if (!accountsEntities.Any())
            return [];

        accounts = accountsEntities.Select(o => new Account()
        {
            Id = o.Id,
            UserId = o.UserId,
            Type = o.Type,
            Balance = o.Balance,
        }).ToList();

        return accounts;
    }

    public async Task<List<Account>> Get()
    {
        List<Account> accounts = [];
        IEnumerable<AccountEntity> accountsEntities = await _accountRepository.Get();

        if (!accountsEntities.Any())
            return [];

        accounts = accountsEntities.Select(o => new Account()
        {
            Id = o.Id,
            UserId = o.UserId,
            Type = o.Type,
            Balance = o.Balance,
        }).ToList();

        return accounts;
    }

    public async Task<Guid> Add(AccountAdd account)
    {
        if (!Enum.IsDefined(typeof(AccountType), account.Type))
            throw new ArgumentException("Invalid account type");

        await _userRepository.Get(account.UserId);

        if ((await GetUserAccounts(account.UserId)).Count >= 2)
            throw new LimitReachedException("Account");

        AccountEntity accountEntity = new()
        {
            UserId = account.UserId,
            Type = account.Type,
        };

        return await _accountRepository.Add(accountEntity);
    }

    public async Task UpdateType(Guid id, AccountTypeEdit account)
    {
        if (!Enum.IsDefined(typeof(AccountType), account.Type))
            throw new ArgumentException("Invalid account type");

        await Get(id);

        AccountEntity accountEntity = new()
        {
            Id = id,
            Type = account.Type,
        };

        int result = await _accountRepository.UpdateType(accountEntity);

        if (result > 1)
            throw new InvalidOperationException("Update was performed on multiple rows");
    }

    public async Task Delete(Guid id)
    {
        await Get(id);

        await _accountRepository.Delete(id);
    }
}
