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
        var comment = await _services.SubmitComment(commentBody);
        return Ok(comment);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateComment([FromBody] CommentEndpointBody commentBody)
    {
        var updatedComment = await _services.UpdateComment(commentBody);
        return Ok(updatedComment);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteComment([FromQuery] int commentId)
    {
        var updatedComment = await _services.DeleteComment(commentId);
        return Ok(updatedComment);
    }
}