using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ActivityController : ControllerBase
{
    private readonly DatabaseContext _context;

    public ActivityController(DatabaseContext context)
    {
        _context = context;
    }
    
    
    
    [HttpGet("AllActivity")]
    public async Task<ActionResult<IEnumerable<Period>>> GetAllActivite()
    {
        var val = await _context.Activities.ToListAsync();
        return  Ok(val);
    }


    [HttpPost("NewActivite")]
    public async Task<IActionResult> Post([FromBody] Activity activity)
    {
        Activity activiteObj = new Activity()
        {
            Name = activity.Name,
            Description = activity.Description,
            Place = activity.Place,
            BeginDate = activity.BeginDate,
            EndDate = activity.EndDate,
            Period = activity.Period,
        };
        _context.Activities.Add(activiteObj);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("GetActivity", new { id = activiteObj.Id }, activiteObj);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Period>> GetActivity(int id)
    {
        var result = await this._context.Activities.FindAsync(id);
        if (result == null)
        {
            return NotFound("L'id n'a pas été trouvé");
        }

        return Ok(result);
    }

    
    

    [HttpGet("GetByVacances")]
    public async Task<ActionResult<Activity>> GetActiviteByVacances(int idVacances)
    {
        if (await  _context.Periods.FindAsync(idVacances) != null)
        {
            Period vacances = (await _context.Periods.FindAsync(idVacances))!;
            return Ok(vacances.ListActivity);
        }

        return NotFound("L'id de la Vacances n'a pas été trouvé ");
    }
    
    
    
    
    
}