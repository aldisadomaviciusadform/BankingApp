using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<UserEntity?> Get(Guid id)
    {
        var queryArguments = new { Id = id };

        string sql = @"SELECT * FROM users
                            WHERE id=@Id AND is_deleted=false";

        return await _dbConnection.QuerySingleOrDefaultAsync<UserEntity>(sql, queryArguments);
    }

    public async Task<IEnumerable<UserEntity>> Get()
    {
        string sql = @"SELECT * FROM users
                            WHERE is_deleted=false";

        return await _dbConnection.QueryAsync<UserEntity>(sql);
    }

    public async Task<Guid> Add(UserEntity user)
    {
        string sql = @"INSERT INTO users
                            (name, address)
                            VALUES (@Name, @Address)
                            RETURNING id";

        return await _dbConnection.ExecuteScalarAsync<Guid>(sql, user);
    }

    public async Task<int> Update(UserEntity user)
    {
        string sql = @"UPDATE users
                            SET name=@Name, address=@Address
                            WHERE id=@Id AND is_deleted=false";

        return await _dbConnection.ExecuteAsync(sql, user);
    }

    public async Task Delete(Guid id)
    {
        var queryArguments = new { Id = id };

        string sql = @"UPDATE users
                            SET is_deleted=true
                            WHERE id=@Id AND is_Deleted=false";

        await _dbConnection.ExecuteAsync(sql, queryArguments);
    }
}
