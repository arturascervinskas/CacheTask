using Application.Dto;
using Application.Services;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
//    [Authorize(Roles ="Admin")]
    [HttpPost()]
    public async Task<IActionResult> Add(User user)
    {
        Guid id = await _userService.Add(user);

        return Ok(new { id });
    }
}