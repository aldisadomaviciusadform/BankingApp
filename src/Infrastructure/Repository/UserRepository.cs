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
        var queryArguments = new
        {
            Id = id
        };

        return await _dbConnection.QuerySingleAsync<UserEntity>("SELECT * FROM items" +
                                                " WHERE id=@Id AND \"isDeleted\"=false", queryArguments);
    }

    public async Task<IEnumerable<UserEntity>> Get()
    {
        return await _dbConnection.QueryAsync<UserEntity>("SELECT * FROM items" +
                                                " WHERE \"isDeleted\"=false");
    }

    public async Task<Guid> Add(UserEntity item)
    {
        string sql = $"INSERT INTO items" +
                        " (name, price, \"shopId\")" +
                        " VALUES (@Name, @Price, @ShopId)" +
                        "RETURNING id";

        return await _dbConnection.ExecuteScalarAsync<Guid>(sql, item);
    }

    public async Task<int> Update(UserEntity item)
    {
        return await _dbConnection.ExecuteAsync("UPDATE items" +
                                        " SET name=@Name,price=@Price,\"shopId\"=@ShopId" +
                                        " WHERE id=@Id AND \"isDeleted\"=false", item);
    }

    public async Task Delete(Guid id)
    {
        var queryArguments = new
        {
            Id = id
        };

        await _dbConnection.ExecuteAsync("UPDATE items" +
                                        " SET \"isDeleted\"=true" +
                                        " WHERE id=@Id AND \"isDeleted\"=false", queryArguments);
    }
}
