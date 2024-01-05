using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<UserEntity?> Get(Guid id);
    public Task<UserEntity?> Get(string name);
    public Task<Guid> Add(UserEntity user);
}
