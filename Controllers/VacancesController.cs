using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class VacancesController : ControllerBase
{
    
    private readonly ApplicationContext _context;
    public VacancesController(ApplicationContext context)
    {
        _context = context;
    }


    [HttpGet("AllVacances")]
  
    public async Task<ActionResult<IEnumerable<Vacances>>> GetAllVacances()
    {
        return  Ok(await _context.Vacances.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Vacances>> GetVacances(int id)
    {
        var result = await this._context.Vacances.FindAsync(id);
        if (result == null)
        {
            return NotFound("L'id n'a pas été trouvé");
        }

        return Ok(result);

    }


    [HttpPost("New")]
    public async  Task<IActionResult> Post([FromBody]Vacances vacances)
    {
        
        Vacances vacancesObj = new Vacances()
        {
            Nom = vacances.Nom,
            Description = vacances.Description,
            DateDebut = vacances.DateDebut,
            DateFin = vacances.DateFin
        };

         _context.Vacances.Add(vacancesObj);
         await _context.SaveChangesAsync();


        return CreatedAtAction("GetVacances", new { id = vacancesObj.IdVacances }, vacancesObj);

    }


    [HttpDelete("Delete")]

    public async Task<IActionResult> DeleteVacances(string id)
    {
        
        
        if (await this._context.Vacances.FindAsync(id) == null)
        {
            return BadRequest("L'id des vacances n'a pas été trouvé");
        }
        else
        {
            Vacances vacances = (await this._context.Vacances.FindAsync(id))!;
            _context.Vacances.Remove(vacances);
            await _context.SaveChangesAsync();
            return Ok("Les vacances ont bien été supprimé ");
        }

       
    }
}