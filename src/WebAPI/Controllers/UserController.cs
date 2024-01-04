using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JWTService _jwtService;

    public UserController(UserService userService, IConfiguration config)
    {
        _userService = userService;
        _jwtService = new JWTService(config);
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(UserLogin user)
    {
        await _userService.CheckLoginData(user);

        string token = _jwtService.GenerateToken(user);
        return Ok(new { token });
    }

    [Authorize(Roles ="Admin")]
    [HttpGet("Admin")]
    public async Task<IActionResult> Test1()
    {
        string token = "pass";
        return Ok(new { token });
    }

    [Authorize(Roles = "user")]
    [HttpGet("user")]
    public async Task<IActionResult> Test2()
    {
        string token = "pass";
        return Ok(new { token });
    }

    [Authorize]
    [HttpGet("none")]
    public async Task<IActionResult> Testa3()
    {
        string token = "pass";
        return Ok(new { token });
    }
}