using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Models.DTOs;
using VacancyProAPI.Models.ViewModels;
using VacancyProAPI.Services.UserService;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class ActivityController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly PlaceController _placeController;
    private readonly ILogger<ActivityController> _logger;
    private readonly IUserService _userService;

    public ActivityController(DatabaseContext context , ILogger<ActivityController> logger , IUserService userService )
    {
        _context = context;
        _placeController = new PlaceController(context);
       _logger = logger;
       _userService = userService;
       
    }
    
    
 


    [HttpPost("NewActivity")]
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [SwaggerOperation(Summary = "Crée une nouvelle activitée ")]

    public async Task<IActionResult> Post([FromBody] Activity activity)
    {
        if (!ModelState.IsValid)
        {
            _logger.Log(LogLevel.Warning , "Erreur dans l'ajout d'une activitée ");
            return BadRequest(new ErrorViewModel("Informations invalide"));
        }
        string formatedDateBegin = activity.BeginDate.ToString("yyyy-MM-dd HH:mm:ss");
        string formatedDateEnd = activity.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
        
        Place place  = await _placeController.AddPlace(activity.Place);
        
        string sqlRaw = "INSERT INTO Activities (Name , Description ,BeginDate,EndDate,PlaceId,PeriodId) VALUES ('" +
                        activity.Name +
                        "','" + activity.Description + "','" + formatedDateBegin + "','" + formatedDateEnd + "','" +
                        place.Id +
                        "'," + activity.Period.Id + ")";
        
         _context.Database.ExecuteSqlRaw(sqlRaw);
         _logger.Log(LogLevel.Information , "Activitée ajouté avec succès");
         await _context.SaveChangesAsync();
        
        return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
    }
    
    [HttpPut("{id:int}")]
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [SwaggerOperation(Summary = "Met à jour une activitée avec les dates concernées ")]
    public async Task<IActionResult> UpdateActivity(int id ,  EditActivityDto activity)
    {
        if (!ModelState.IsValid)
        {
            _logger.Log(LogLevel.Warning, "Erreur dans la mise à jour d'une activitée ");
            return BadRequest(new ErrorViewModel("Informations invalide"));
        }
        try
        {
            var activityInBd = await _context.Activities.FindAsync(id);
            if (activityInBd == null){
                return NotFound($"Employee with Id = {id} not found");
            }
            activityInBd.BeginDate = activity.BeginDate;
            activityInBd.EndDate = activity.EndDate;
            activityInBd.Name = activity.Name;
            activityInBd.Description = activity.Description;
             _context.Activities.Update(activityInBd);
             await _context.SaveChangesAsync();
             return Ok(activityInBd);
        }catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
        }
    }
    
    
    

    
    [HttpGet("ActivityByPeriod")]
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [SwaggerOperation(Summary = "Retourne les activitée par période  ")]

    public async Task<ActionResult<Activity>> ActivitiesByPeriodId(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.Log(LogLevel.Warning , "Erreur dans la récupération de donnée pour une activitée ");
            return BadRequest("Aucune valeur passée pour l'id ");
        }
        var period = await _context.Periods.FindAsync(id);
        if (period == null)
        {
            _logger.Log(LogLevel.Warning , "Erreur dans la récupération de donnée pour une activitée ");
            return NotFound("La période correspondant à l'id n'a pas été trouvé ");
        }
        var result =  _context.Activities.Include(p => p.Place).Include(pl => pl.Period).Where(a => a.Period == period);
        _logger.Log(LogLevel.Information , "Récupération de données éffectué ");
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Retourne l' activitée avec l'id correspondant  ")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    public async Task<ActionResult<Activity>> GetActivity(int id)
    {
        var result = await this._context.Activities.FindAsync(id);
        if (result == null)
        {
            _logger.Log(LogLevel.Warning , "Erreur dans la récupération de donnée pour une activitée ");
            return NotFound("L'id n'a pas été trouvé");
        }

        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Supprime l'activitée avec l'id correspondant  ")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    public async Task<ActionResult<Activity>> DeleteActivity(int id)
    {
        string userId =  _userService.GetUserIdFromToken()!;
        User? user =  _context.Users.Include(p => p.Periods).First( u => u.Id == userId);
        
        Activity? activity =  _context.Activities.Include(a => a.Period).First(a => a.Id == id);
        if (activity == null || user == null)
        {
            _logger.Log(LogLevel.Warning , "Erreur dans la suppression d'une activitée ");
            return NotFound("L'id n'a pas été trouvé");
        }
        int periodId = activity.Period.Id;

        foreach (var period in user.Periods)
        {
            if (period.Id == periodId)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
                _logger.Log(LogLevel.Information , "Activitée supprimé avec succès");
                return Ok(activity);
            }
        }
        _logger.Log(LogLevel.Warning , "Erreur dans la suppression d'une activitée ");
        return BadRequest("Vous n'avez pas les droits pour supprimer cette activitée");
    }
    
    private void AddNotif(User user, string contenu)
    {
        Notification notification = new Notification();
        notification.User = user;
        notification.Contenu = contenu;
        notification.Date = DateTime.Now;
        _context.Notifications.Add(notification);

    }
}