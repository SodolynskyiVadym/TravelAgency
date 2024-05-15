using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaceController : ControllerBase
{
    private readonly MyDbContext _context;

    public PlaceController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("test")]
    public IActionResult Test()
    {
        _context.Places.Add(new Place()
        {
            Name = "Paris",
            Description = "Amazing city!!!",
            Country = "France",
            ImageUrl = "test_link"
        });
        _context.SaveChanges();
        return Ok("Test");
    }
}