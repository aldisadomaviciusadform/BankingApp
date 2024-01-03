using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly IDbConnection _dbConnection;

    public AccountRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<AccountEntity?> Get(Guid id)
    {
        var queryArguments = new { Id = id };

        string sql = @"SELECT * FROM accounts
                            WHERE id=@Id AND is_deleted=false";

        return await _dbConnection.QuerySingleOrDefaultAsync<AccountEntity>(sql, queryArguments);
    }

    public async Task<IEnumerable<AccountEntity>> GetUserAccounts(Guid id)
    {
        var queryArguments = new { Id = id };

        string sql = @"SELECT * FROM accounts
                            WHERE is_deleted=false
                            AND user_id=@Id";

        return await _dbConnection.QueryAsync<AccountEntity>(sql, queryArguments);
    }

    public async Task<IEnumerable<AccountEntity>> Get()
    {
        string sql = @"SELECT * FROM accounts
                            WHERE is_deleted=false";

        return await _dbConnection.QueryAsync<AccountEntity>(sql);
    }

    public async Task<Guid> Add(AccountEntity account)
    {
        string sql = @"INSERT INTO accounts
                            (type, balance, user_id)
                            VALUES (@Type, 0, @UserId)
                            RETURNING id";

        return await _dbConnection.ExecuteScalarAsync<Guid>(sql, account);
    }

    public async Task<int> UpdateType(AccountEntity account)
    {
        string sql = @"UPDATE accounts
                            SET type=@Type
                            WHERE id=@Id AND is_deleted=false";

        return await _dbConnection.ExecuteAsync(sql, account);
    }

    public async Task Delete(Guid id)
    {
        var queryArguments = new { Id = id };

        string sql = @"UPDATE accounts
                            SET is_deleted=true
                            WHERE id=@Id AND is_deleted=false";

        await _dbConnection.ExecuteAsync(sql, queryArguments);
    }

    public async Task UpdateAmount(AccountEntity account, IDbTransaction dbTransaction)
    {
        string sql = @"UPDATE accounts
                            SET balance=@Balance
                            WHERE id=@Id AND is_deleted=false";

        await _dbConnection.ExecuteAsync(sql, account, dbTransaction);
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
