using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IJWTService
{
    string GenerateToken(UserLogin user);
    Guid ValidateJwtToken(string? token);
}
