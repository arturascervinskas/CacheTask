using Application.Dto;
using Microsoft.Extensions.Configuration;
using System.Security;

namespace Application.Services;

public class UserService
{

    public UserService()
    {

    }

    public async Task CheckLoginData(UserLogin user)
    {
        if (user.Password != "slaptazodis" || user.Name != "vardas")
            throw new SecurityException("User or password does exist");
    }
}
