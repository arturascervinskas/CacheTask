using Application.Dto;
using Application.Services;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

/// <summary>
/// Controller for handling authentication-related operations.
/// </summary>
[ApiController]
[Route("v1/[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly AuthenticateService _authenticateService;

    /// <summary>
    /// Initializes a new instance of the AuthenticateController class.
    /// </summary>
    /// <param name="authenticateService">The service handling authentication.</param>
    public AuthenticateController(AuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    /// <summary>
    /// Authenticates a user based on the provided login information.
    /// </summary>
    /// <param name="user">The user's login credentials.</param>
    /// <returns>A response indicating the authentication status.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate(UserLogin user)
    {
        string token = await _authenticateService.CheckLoginData(user);

        return Ok(new { token });
    }
}