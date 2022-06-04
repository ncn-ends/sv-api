using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[Route("[controller]")]
[ApiController]
// [ResponseType(typeof(BookDto))]
public class PingController : ControllerBase
{
    private readonly BuildServices _services;
    private readonly IHostEnvironment _env;

    public PingController(BuildServices services, IHostEnvironment env)
    {
        _services = services;
        _env = env;
    }

    [HttpGet]
    public ActionResult<string> Pong()
    {
        return Ok("pong");
    }

    [HttpPost]
    public IActionResult Post([FromBody] object something)
    {
        return Ok(something);
    }
}