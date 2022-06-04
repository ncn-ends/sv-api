using System.Net;
using System.Security.Authentication;
using app.Configs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace app.Controllers;


public class RatingEndpointBody
{
    public bool rating { get; set; }
    public int listId { get; set; }
}

[Route("[controller]")]
[ApiController]
// [ResponseType(typeof(BookDto))]
public class RatingsController : ControllerBase
{
    private readonly RatingServices _services;
    private readonly IHostEnvironment _env;
    private readonly BuildDbContext _dbContext;

    public RatingsController(RatingServices services, IHostEnvironment env, BuildDbContext dbContext)
    {
        _services = services;
        _env = env;
        _dbContext = dbContext;
    }


    [HttpPost]
    public async Task<IActionResult> SubmitRating([FromBody] RatingEndpointBody ratingBody)
    {
        try
        {
            var rating = await _services.SubmitRating(ratingBody);
            return Ok(rating);
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
    public async Task<IActionResult> UpdateRating([FromBody] RatingEndpointBody ratingBody)
    {
        try
        {
            var updatedRating = await _services.UpdateRating(ratingBody);
            return Ok(updatedRating);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(404);
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
    public async Task<IActionResult> DeleteRating([FromQuery] int listId)
    {
        try
        {
            var updatedRating = await _services.DeleteRating(listId);
            return Ok(updatedRating);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode(404);
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