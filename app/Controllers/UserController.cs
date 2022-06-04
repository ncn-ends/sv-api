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

    public UserController(UserServices services, IHostEnvironment env, BuildDbContext context)
    {
        _services = services;
    }

    [HttpGet]
    public ActionResult<UserProfile> GetUserProfileById([FromQuery] string id)
    {
        try
        {
            var user = _services.GetUserProfileById(id);
            if (user == null) return NoContent();
            return Ok(user);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }

    [HttpGet("me")]
    public ActionResult<UserProfile> GetMyUserProfile()
    {
        try
        {
            var user = _services.GetMyUserProfile();
            return Ok(user);
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine(e.ToString());
            return Unauthorized();
        }
        catch (DataMisalignedException e)
        {
            Console.WriteLine(e.ToString());
            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }

    [HttpDelete]
    public IActionResult DeleteUserProfile()
    {
        try
        {
            _services.DeleteUsersProfile();
            return Ok();
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine(e.ToString());
            return Unauthorized();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return BadRequest();
        }
    }

    [HttpPost("sync/auth")]
    // [Authorize]
    // public async Task<IActionResult> SyncUserProfile([FromBody] UserProfileDto userProfileDto)
    public async Task<IActionResult> SyncUserAuth([FromBody] Auth0UserDataDto auth0UserDataDto)
    {
        try
        {
            var user = await _services.SyncUserAuth(auth0UserDataDto);
            return Ok(user);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return NotFound();
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine(e.ToString());
            return Unauthorized();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }
    
    [HttpPost("sync/profile")]
    [Authorize]
    public async Task<IActionResult> SyncUserProfile([FromBody] UserProfileDto userProfileDto)
    {
        try
        {
            var user = await _services.SyncUserProfile(userProfileDto);
            return Ok(user);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return NotFound();
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine(e.ToString());
            return Unauthorized();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisteringUserDto user)
    {
        try
        {
            var createdUser = await _services.RegisterUser(user);
            return Ok(createdUser);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }
}