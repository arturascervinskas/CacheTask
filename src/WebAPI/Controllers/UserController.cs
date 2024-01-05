using Application.Dto;
using Application.Services;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

/// <summary>
/// Controller for managing user-related operations.
/// </summary>
[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    /// <summary>
    /// Initializes a new instance of the UserController class.
    /// </summary>
    /// <param name="userService">The service handling user-related operations.</param>
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="user">The user information to add.</param>
    /// <returns>A response indicating the result of the operation.</returns>
    [AllowAnonymous]
//    [Authorize(Roles ="Admin")]
    [HttpPost()]
    public async Task<IActionResult> Add(User user)
    {
        Guid id = await _userService.Add(user);

        return Ok(new { id });
    }
}