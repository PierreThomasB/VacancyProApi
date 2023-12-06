using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class ActivityController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly PlaceController _placeController;

    public ActivityController(DatabaseContext context)
    {
        _context = context;
        _placeController = new PlaceController(context);
    }
    
    
 


    [HttpPost("NewActivity")]
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]

    public async Task<IActionResult> Post([FromBody] Activity activity)
    {
        string formatedDateBegin = activity.BeginDate.ToString("yyyy-MM-dd HH:mm:ss");
        string formatedDateEnd = activity.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
        Place place  = await _placeController.AddPlace(activity.Place);
        string sqlRaw = "INSERT INTO Activities (Name , Description ,BeginDate,EndDate,PlaceId,PeriodId) VALUES ('" +
                        activity.Name +
                        "','" + activity.Description + "','" + formatedDateBegin + "','" + formatedDateEnd + "','" +
                        place.Id +
                        "'," + activity.Period.Id + ")";
        
         _context.Database.ExecuteSqlRaw(sqlRaw);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
    }

    
    [HttpGet("ActivityByPeriod")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]

    public async Task<ActionResult<Activity>> ActivitiesByPeriodId(int id)
    {
        if (id == null)
        {
            return  BadRequest("L'id ne peux être nul");
        }

        var period = await _context.Periods.FindAsync(id);
        var result =  _context.Activities.Include(p => p.Place).Include(pl => pl.Period).Where(a => a.Period == period);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivity(int id)
    {
        var result = await this._context.Activities.FindAsync(id);
        if (result == null)
        {
            return NotFound("L'id n'a pas été trouvé");
        }

        return Ok(result);
    }
}