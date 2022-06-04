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

    public RatingsController(RatingServices services, IHostEnvironment env)
    {
        _services = services;
        _env = env;
    }


    [HttpPost]
    public async Task<IActionResult> SubmitRating([FromBody] RatingEndpointBody ratingBody)
    {
        var rating = await _services.SubmitRating(ratingBody);
        return Ok(rating);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRating([FromBody] RatingEndpointBody ratingBody)
    {
        var updatedRating = await _services.UpdateRating(ratingBody);
        return Ok(updatedRating);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRating([FromQuery] int listId)
    {
        var updatedRating = await _services.DeleteRating(listId);
        return Ok(updatedRating);
    }
}