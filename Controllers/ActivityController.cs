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
        this._placeController = new PlaceController(context);
    }
    
    
 


    [HttpPost("NewActivity")]
    public async Task<IActionResult> Post([FromBody] Activity activity)
    {
        Place place  = await _placeController.AddPlace(activity.Place);
        Period? period = (await _context.Periods.FindAsync(activity.Period.Id));
        if (period == null)
        {
            return BadRequest("La période ne peux etre nule ");
        }
        
        
        Activity activiteObj = new Activity();

        activiteObj.Name = activity.Name;
        activiteObj.Description = activity.Description;
        activiteObj.Place = place;
        activiteObj.BeginDate = activity.BeginDate;
        activiteObj.EndDate = activity.EndDate;
        activiteObj.Period = period!;
        
        
        _context.Activities.Add(activiteObj);
        _context.Periods.Update(period);
        
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("GetActivity", new { id = activity.Id }, activiteObj);
    }

    
    [HttpGet("ActivityByPeriod")]
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