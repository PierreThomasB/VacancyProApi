using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ActiviteController : ControllerBase
{
    private readonly ApplicationContext _context;

    public ActiviteController(ApplicationContext context)
    {
        _context = context;
    }
    
    
    
    [HttpGet("AllActivite")]
    public async Task<ActionResult<IEnumerable<Vacances>>> GetAllActivite()
    {
        var val = await _context.Activites.ToListAsync();
        return  Ok(val);
    }


    [HttpPost("NewActivite")]
    public async Task<IActionResult> Post([FromBody] Activite activite)
    {
        Activite activiteObj = new Activite()
        {
            Nom = activite.Nom,
            Description = activite.Description,
            Lieux = null,
        };
        _context.Activites.Add(activiteObj);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("GetActivite", new { id = activiteObj.IdActivite }, activiteObj);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Vacances>> GetActivite(int id)
    {
        var result = await this._context.Activites.FindAsync(id);
        if (result == null)
        {
            return NotFound("L'id n'a pas été trouvé");
        }

        return Ok(result);
    }

    
    

    [HttpGet("GetByVacances")]
    public async Task<ActionResult<Activite>> GetActiviteByVacances(int idVacances)
    {
        if (await  _context.Vacances.FindAsync(idVacances) != null)
        {
            Vacances vacances = (await _context.Vacances.FindAsync(idVacances))!;
            return Ok(vacances.Activites);
        }

        return NotFound("L'id de la Vacances n'a pas été trouvé ");
    }
    
    
    
    
    
}