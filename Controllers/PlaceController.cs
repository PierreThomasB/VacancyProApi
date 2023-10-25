using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;


namespace VacancyProAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlaceController : ControllerBase
{
    
    private readonly DatabaseContext _context;

    public PlaceController(DatabaseContext context) 
    {
        _context = context;
    }
    
    
    [HttpGet("AllLieux")]
    public async Task<ActionResult<IEnumerable<Place>>> GetAllLieux()
    {
        return Ok(await _context.Places.ToListAsync());
    }
    
    

    

    
}