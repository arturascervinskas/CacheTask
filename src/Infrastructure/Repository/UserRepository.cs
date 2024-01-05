using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Interfaces;

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
                            WHERE id=@Id";

        return await _dbConnection.QuerySingleOrDefaultAsync<UserEntity>(sql, queryArguments);
    }

    public async Task<UserEntity?> Get(string name)
    {
        var queryArguments = new { Name = name };

        string sql = @"SELECT * FROM users
                            WHERE name=@Name";

        return await _dbConnection.QuerySingleOrDefaultAsync<UserEntity>(sql, queryArguments);
    }

    public async Task<Guid> Add(UserEntity user)
    {
        string sql = @"INSERT INTO 
                        users(name, password, role) 
                        VALUES (@Name, @Password ,@Role) 
                        RETURNING id";

        return await _dbConnection.ExecuteScalarAsync<Guid>(sql,user);
    }
}
