using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Controllers;

/// <summary>
/// Controller qui permet de gérer les périodes de vacances
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PeriodController : ControllerBase
{

    private readonly DatabaseContext _context;

    public PeriodController(DatabaseContext context)
    {
        _context = context;
    }


    [HttpGet("AllPeriods")]

    public async Task<ActionResult<IEnumerable<Period>>> GetAllVacances()
    {
        return Ok(await _context.Periods.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Period>> GetVacances(int id)
    {
        var result = await this._context.Periods.FindAsync(id);
        if (result == null)
        {
            return NotFound("L'id n'a pas été trouvé");
        }

        return Ok(result);

    }


    [HttpPost("NewVacances")]
    public async Task<IActionResult> Post([FromBody] Period p)
    {

        /*
        Period vacancesObj = new Period(

        {
            Name = p.Name,
            Description = p.Description,
            BeginDate = p.BeginDate,
            EndDate = p.EndDate,
            Creator = p.Creator,
            Place = p.Place,
            ListUser = new HashSet<User>(),
            ListActivity = new List<Activity>(),
        };
        ;
        
        **/


        _context.Periods.Add(p);
        await _context.SaveChangesAsync();


        return CreatedAtAction("GetVacances", new { id = p.Id }, p);

    }



[HttpDelete("Delete")]

    public async Task<IActionResult> DeleteVacances(string id)
    {
        
        
        if (await this._context.Periods.FindAsync(id) == null)
        {
            return BadRequest("L'id des vacances n'a pas été trouvé");
        }
        else
        {
            Period vacances = (await this._context.Periods.FindAsync(id))!;
            _context.Periods.Remove(vacances);
            await _context.SaveChangesAsync();
            return Ok("Les vacances ont bien été supprimé ");
        }

       
    }
}