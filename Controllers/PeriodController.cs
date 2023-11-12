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
    private readonly PlaceController _placeController;

    public PeriodController(DatabaseContext context )
    {
        _context = context;
        _placeController = new PlaceController(context);
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


        await _placeController.AddPlace(p.Place);
       
        
        
        Period vacancesObj = new Period();


        vacancesObj.Name = p.Name;
        vacancesObj.Description = p.Description;
        vacancesObj.BeginDate = p.BeginDate;
        vacancesObj.EndDate = p.EndDate;
       // vacancesObj.Creator = p.Creator;
        vacancesObj.Place = p.Place;
       // vacancesObj.ListUser = new HashSet<User>();
        //vacancesObj.ListActivity = new List<Activity>();
       
        
        
        _context.Periods.Add(vacancesObj);
        await _context.SaveChangesAsync();


        return CreatedAtAction("GetVacances", new { id = p.Id }, vacancesObj);

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