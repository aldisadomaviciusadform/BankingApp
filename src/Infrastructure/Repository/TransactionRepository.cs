using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDbConnection _dbConnection;

    public TransactionRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<TransactionEntity?> Get(Guid id)
    {
        var queryArguments = new { Id = id };

        string sql = @"SELECT * FROM transactions
                            WHERE id=@Id";

        return await _dbConnection.QuerySingleOrDefaultAsync<TransactionEntity>(sql, queryArguments);
    }

    public async Task<IEnumerable<TransactionEntity>> GetAccountsTransactions(Guid accountId)
    {
        var queryArguments = new { AccountId = accountId };

        string sql = @"SELECT * FROM transactions
                            WHERE account_Id=@AccountId";

        return await _dbConnection.QueryAsync<TransactionEntity>(sql, queryArguments);
    }

    public async Task<IEnumerable<TransactionEntity>> GetUsersTransactions(Guid userId)
    {
        var queryArguments = new { UserId = userId };

        string sql = @"SELECT transactions.* FROM accounts
                            JOIN transactions
                            ON accounts.id = transactions.account_id
                            WHERE accounts.is_deleted=false AND accounts.user_id=@UserId";

        return await _dbConnection.QueryAsync<TransactionEntity>(sql, queryArguments);
    }

    public async Task<IEnumerable<TransactionEntity>> Get()
    {
        string sql = @"SELECT * FROM transactions";

        return await _dbConnection.QueryAsync<TransactionEntity>(sql);
    }

    public async Task<Guid> Add(TransactionEntity transaction, IDbTransaction dbTransaction)
    {
        string sql = @"INSERT INTO transactions
                        (type, amount, account_id)
                        VALUES (@Type, @Amount, @AccountId)
                        RETURNING id";

        return await _dbConnection.ExecuteScalarAsync<Guid>(sql, transaction, dbTransaction);
    }

    public IDbTransaction StartTransaction()
    {
        _dbConnection.Open();
        return _dbConnection.BeginTransaction();
    }

    public void EndTransaction(IDbTransaction _transactionQuery)
    {
        _transactionQuery.Commit();
    }
}
