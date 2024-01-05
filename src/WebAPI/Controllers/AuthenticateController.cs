using Application.Dto;
using Application.Services;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly AuthenticateService _authenticateService;

    public AuthenticateController(AuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate(UserLogin user)
    {
        string token = await _authenticateService.CheckLoginData(user);

        return Ok(new { token });
    }
}