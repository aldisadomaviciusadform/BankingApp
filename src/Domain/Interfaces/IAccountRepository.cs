using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IAccountRepository
{
    public Task<AccountEntity?> Get(Guid id);
    public Task<IEnumerable<AccountEntity>> GetUserAccounts(Guid id);
    public Task<IEnumerable<AccountEntity>> Get();
    public Task<Guid> Add(AccountEntity account);
    public Task<int> UpdateType(AccountEntity account);
    public Task Delete(Guid id);
    public Task UpdateAmount(AccountEntity account, IDbTransaction dbTransaction);
    public IDbTransaction StartTransaction();
    public void EndTransaction(IDbTransaction _transactionQuery);
}
