using Application.Dto;
using Domain.Dto;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security;
using System.Security.Cryptography;

namespace Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Add(User user)
    {
        UserEntity? userEntity = await _userRepository.Get(user.Name);

        if (userEntity is not null)
            throw new FoundException($"User {user.Name}");

        byte[] password = EncryptionService.EncryptStringToBytes_Aes(user.Password) 
                                    ?? throw new ArgumentException("Failed to add encryption");

        userEntity = new()
        {
            Name = user.Name,
            Password = password,
            Role = user.Role
        };

        return await _userRepository.Add(userEntity);
    }
}
