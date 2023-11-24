using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Models.ViewModels;

namespace VacancyProAPI.Controllers;

/// <summary>
/// Controller qui permet de gérer les périodes de vacances
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]


    public async Task<ActionResult<IEnumerable<Period>>> GetAllVacances()
    {
        return Ok(await _context.Periods.Include(e  => e.Place).ToListAsync());
    }

    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "La période de vacances n'existe pas", typeof(ErrorViewModel))]


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
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]

    public async Task<IActionResult> Post([FromBody] Period p)
    {


        Place place = await _placeController.AddPlace(p.Place);
        var user = await  _context.Users.FindAsync(p.Creator.Id);
       
        
        
        Period vacancesObj = new Period();


        vacancesObj.Name = p.Name;
        vacancesObj.Description = p.Description;
        vacancesObj.BeginDate = p.BeginDate;
        vacancesObj.EndDate = p.EndDate;
        vacancesObj.Creator = user!;
        vacancesObj.Place = place;
        vacancesObj.ListUser = new List<User>();
        vacancesObj.ListUser.Add(user);
        
        _context.Periods.Add(vacancesObj);
        await _context.SaveChangesAsync();


        return CreatedAtAction("GetVacances", new { id = p.Id }, vacancesObj);

    }

   
    [HttpDelete("Delete")]
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    public async Task<IActionResult> DeleteVacances(int id)
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
    
    
    [HttpPut("AddUser")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    public async Task<IActionResult> AddUser(User user, int period)
    {
        Period result = await this._context.Periods.FindAsync(period);
        
        if (result == null)
        {
            return BadRequest("L'id des vacances n'a pas été trouvé");
        }
        else
        {
            result.ListUser.Add(user);
            await _context.SaveChangesAsync();
            return Ok("La personne à bien été ajoutée");
        }

       
    }
}