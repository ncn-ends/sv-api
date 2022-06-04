using System.Net;
using System.Security;
using System.Security.Authentication;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace app.Controllers;

[Route("[controller]")]
[ApiController]
// [ResponseType(typeof(BookDto))]
public class CommentController : ControllerBase
{
    private readonly CommentServices _services;
    private readonly IHostEnvironment _env;

    public CommentController(CommentServices services, IHostEnvironment env)
    {
        _services = services;
        _env = env;
    }


    [HttpPost]
    public async Task<IActionResult> SubmitComment([FromBody] CommentEndpointBody commentBody)
    {
        try
        {
            var comment = await _services.SubmitComment(commentBody);
            return Ok(comment);
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

    
    [HttpPut]
    public async Task<IActionResult> UpdateComment([FromBody] CommentEndpointBody commentBody)
    {
        try
        {
            var updatedComment = await _services.UpdateComment(commentBody);
            return Ok(updatedComment);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(404);
        }
        catch (SecurityException e)
        {
            Console.WriteLine(e.ToString());
            return Forbid();
        }
        catch (CookieException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(406);
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(401);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }
    
    
    [HttpDelete]
    public async Task<IActionResult> DeleteComment([FromQuery] int commentId)
    {
        try
        {
            var updatedComment = await _services.DeleteComment(commentId);
            return Ok(updatedComment);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(404);
        }
        catch (SecurityException e)
        {
            Console.WriteLine(e.ToString());
            return Forbid();
        }
        catch (CookieException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(406);
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(401);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }
}