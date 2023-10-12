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


    [HttpGet]
    [Route("api/Vacances/All")]
    public async Task<ActionResult<IEnumerable<Vacances>>> GetAllVacances()
    {
        return  Ok(await _context.Vacances.ToListAsync());
    }


    [HttpPost]
    public async  Task<IActionResult> Post(Vacances vacances)
    {
         var  val = await this._context.AddAsync(vacances);
         if (val.State == EntityState.Added)
         {
             return Ok("Vacances ajoutée avec succès");
         }

         return BadRequest("Les valeurs ne sont pas bonnes ");

    }


    [HttpDelete]

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