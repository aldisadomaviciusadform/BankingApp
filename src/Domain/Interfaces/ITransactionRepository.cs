using Domain.Entities;
using System.Data;

namespace Domain.Interfaces;

public interface ITransactionRepository
{
    public Task<Guid> Add(TransactionEntity transaction, IDbTransaction dbTransaction);
    public Task<IEnumerable<TransactionEntity>> Get();
    public Task<TransactionEntity?> Get(Guid id);
    public Task<IEnumerable<TransactionEntity>> GetAccountsTransactions(Guid id);
    public Task<IEnumerable<TransactionEntity>> GetUsersTransactions(Guid id);
    public IDbTransaction StartTransaction();
    public void EndTransaction(IDbTransaction _transactionQuery);
}
