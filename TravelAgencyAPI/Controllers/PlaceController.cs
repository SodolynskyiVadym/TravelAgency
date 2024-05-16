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

    [HttpPost]
    [Route("create")]
    public IActionResult CreatePlace(Place place)
    {
        place.Id = 0;
        _context.Places.Add(place);
        _context.SaveChanges();
        return Ok(place);
    }
}