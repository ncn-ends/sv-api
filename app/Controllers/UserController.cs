using System.Security.Authentication;
using app.Configs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserServices _services;

    public UserController(
        UserServices services,
        IHostEnvironment env,
        BuildDbContext context
    )
    {
        _services = services;
    }

    [HttpGet]
    public ActionResult<UserProfile> GetUserProfileById([FromQuery] string id)
    {
        var user = _services.GetUserProfileById(id);
        return Ok(user);
    }

    [HttpGet("me")]
    public ActionResult<UserProfile> GetMyUserProfile()
    {
        var user = _services.GetMyUserProfile();
        return Ok(user);
    }

    [HttpDelete]
    public IActionResult DeleteUserProfile()
    {
        _services.DeleteUsersProfile();
        return Ok();
    }

    [HttpPost("sync/auth")]
    // [Authorize]
    // public async Task<IActionResult> SyncUserProfile([FromBody] UserProfileDto userProfileDto)
    public async Task<IActionResult> SyncUserAuth([FromBody] Auth0UserDataDto auth0UserDataDto)
    {
        var user = await _services.SyncUserAuth(auth0UserDataDto);
        return Ok(user);
    }

    [HttpPost("sync/profile")]
    [Authorize]
    public async Task<IActionResult> SyncUserProfile([FromBody] UserProfileDto userProfileDto)
    {
        var user = await _services.SyncUserProfile(userProfileDto);
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisteringUserDto user)
    {
        var createdUser = await _services.RegisterUser(user);
        return Ok(createdUser);
    }
}