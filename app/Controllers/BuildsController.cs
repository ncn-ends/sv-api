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
public class BuildsController : ControllerBase
{
    private readonly BuildServices _services;
    private readonly IHostEnvironment _env;

    public BuildsController(BuildServices services, IHostEnvironment env)
    {
        _services = services;
        _env = env;
    }


    [HttpGet("all")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<BuildOrderList>> ListAllBuilds()
    {
        if (!_env.IsDevelopment()) return NotFound();

        return Ok(_services.GetAll());
    }

    [HttpGet("my")]
    [Authorize]
    public ActionResult<IEnumerable<BuildOrderList>> ListMyBuild()
    {
        var userProfile = (TokenUser) HttpContext.Items["UserProfile"]!;

        if (userProfile.profile_id == null) return NotFound();

        if (!_env.IsDevelopment()) return NotFound();

        return Ok(_services.GetAll());
    }

    [HttpPost("new")]
    public async Task<ActionResult<BuildOrderListDto>> SubmitList([FromBody] BuildOrderListDto buildOrderList)
    {
        try
        {
            var added = await _services.Add(buildOrderList);
            return Ok(added);
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine(e.ToString());
            return Unauthorized();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return BadRequest();
        }
    }

    [HttpGet("one")]
    public IActionResult GetBuildById([FromQuery] int id)
    {
        try
        {
            var build = _services.GetById(id);
            return Ok(build);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return NotFound();
        }
    }


    [HttpGet("filtered")]
    public ActionResult<IEnumerable<BuildOrderList>> GetFilteredBuilds([FromQuery] FilteredBuildsQuery query)
    {
        try
        {
            return Ok(_services.GetFiltered(query));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(500);
        }
    }
}