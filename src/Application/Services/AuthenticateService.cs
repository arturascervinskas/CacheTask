using Domain.Dto;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class AuthenticateService
{
    private readonly IUserRepository _userRepository;
    private readonly IJWTService _jwtService;

    public AuthenticateService(IUserRepository userRepository, IJWTService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string> CheckLoginData(UserLogin user)
    {
        UserEntity userEntity = await _userRepository.Get(user.Name) 
                                ?? throw new UnauthorizedAccessException();

        string password = EncryptionService.DecryptStringFromBytes_Aes(userEntity.Password) 
                                ?? throw new ArgumentException("Failed to add encryption");

        if (user.Password != password)
            throw new UnauthorizedAccessException();

        return _jwtService.GenerateToken(user);
    }
}
