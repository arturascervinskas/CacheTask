using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repository;

public class ItemRepository : IItemRepository
{
    private readonly IDbConnection _dbConnection;

    public ItemRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<ItemEntity>> Get()
    {
        string sql = @"SELECT * FROM items";

        return await _dbConnection.QueryAsync<ItemEntity>(sql);
    }

    public async Task<ItemEntity?> Get(string key)
    {
        string sql = @"SELECT * FROM items 
                        WHERE key=@Key";

        return await _dbConnection.QuerySingleOrDefaultAsync<ItemEntity>(sql, new { key });
    }

    public async Task<ItemEntity?> Create(ItemEntity itemEntity)
    {

        string sql = @"INSERT INTO items 
                        (key, value, expiration_period, expiration_date) 
                        VALUES (@Key, @Value, @ExpirationPeriod, @ExpirationDate) 
                        RETURNING *";

        return await _dbConnection.QuerySingleOrDefaultAsync<ItemEntity>(sql, itemEntity);
    }

    public async Task<ItemEntity?> Update(ItemEntity itemEntity)
    {
        string sql = @"UPDATE items 
                        SET key=@Key, value=@Value, expiration_period=@ExpirationPeriod
                        WHERE key=@Key 
                        RETURNING *";

        return await _dbConnection.QuerySingleOrDefaultAsync<ItemEntity>(sql, itemEntity); ;
    }

    public async Task Delete(string key)
    {
        string sql = @"DELETE FROM items 
                        WHERE key=@Key";

        await _dbConnection.ExecuteAsync(sql, new { key });
    }
}
