using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Models.ViewModels;
using VacancyProAPI.Services.UserService;

namespace VacancyProAPI.Controllers;

/// <summary>
/// Controller qui permet de gérer les périodes de vacances
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class PeriodController : ControllerBase
{

    private readonly DatabaseContext _context;
    private readonly PlaceController _placeController;
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<PeriodController> _logger;



    public PeriodController(DatabaseContext context , IUserService userService , UserManager<User> userManager , ILogger<PeriodController> logger )
    {
        _logger = logger;
        _context = context;
        _placeController = new PlaceController(context);
        _userService =  userService;
        _userManager = userManager;
    }


    [HttpGet("PeriodByUser")]
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [SwaggerOperation(Summary = "Permet de récupérer les periodes d'un utilisateur ")]

    public async Task<ActionResult> GetPeriodByUser()
    {
        var userId =  _userService.GetUserIdFromToken()!;
        List<Period> periods = await _context.Periods.Include(a => a.Place).Include(a => a.ListUser).ToListAsync();
        List<Period> result = new(); 
        foreach (var period in periods)
        {
            foreach (var user in period.ListUser)
            {
                if (user.Id == userId)
                {
                    result.Add(period);
                }
            }
        }
        
        return Ok(result);
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
    [SwaggerOperation(Summary = "Permet de créer une nouvelle période ")]


    public async Task<IActionResult> Post([FromBody] Period p)
    {

        if (!ModelState.IsValid)
        {
            _logger.LogError("La période n'est pas valide ");
            return BadRequest("La période n'est pas valide ");
        }

        Place place = await _placeController.AddPlace(p.Place);
        
        Period vacancesObj = new Period();

        var userId =  _userService.GetUserIdFromToken()!;
        var user = await _userManager.FindByIdAsync(userId);


        vacancesObj.Name = p.Name;
        vacancesObj.Description = p.Description;
        vacancesObj.BeginDate = p.BeginDate;
        vacancesObj.EndDate = p.EndDate;
        vacancesObj.ListUser = new () {user};
        vacancesObj.Place = place;

        _context.Periods.Add(vacancesObj);
        await _context.SaveChangesAsync();


        _logger.LogInformation("La période a bien été créée");
        return CreatedAtAction("GetVacances", new { id = p.Id }, vacancesObj);

    }

   
    [HttpDelete("Delete")]
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [SwaggerOperation(Summary = "Permet de supprimer une période  ")]

    public async Task<IActionResult> DeleteVacances(int id)
    {
        string userId =  _userService.GetUserIdFromToken()!;
        var user = _context.Users.Include(u => u.Periods).First(u => u.Id == userId);
       
        if (await this._context.Periods.FindAsync(id) == null)
        {
            _logger.LogError("L'id des vacances n'a pas été trouvé");
            return NotFound("L'id des vacances n'a pas été trouvé");
        }
       
        Period vacances = (await _context.Periods.FindAsync(id))!;
        if (!user.Periods.Contains(vacances))
        {
            _logger.LogError("Vous n'avez pas l'autorisation de supprimer cette periode de vacances");
            return Unauthorized("Vous n'avez pas l'autorisation de supprimer cette periode de vacances");
        }
        _context.Periods.Remove(vacances);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Les vacances ont bien été supprimé");
        return Ok("Les vacances ont bien été supprimé ");
    }
    
    
    [HttpPut("AddUser")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Permet d'ajouter un utilisateur à une période ")]


    public async Task<IActionResult> AddUser(string userId, int period)
    {
        Period? result = (await _context.Periods.FindAsync(period));
        User? user = (await _context.Users.FindAsync(userId));
        
        if (result == null || user == null)
        {
            _logger.LogError("L'id de l'utilisateur ou des vacances n'a pas été trouvé");
            return BadRequest("L'id de l'utilisateur ou des vacances n'a pas été trouvé");
        }
           
        result.ListUser.Add(user);
        AddNotif(user, "Vous avez été ajouté à une nouvelle périodes de vacances ");
        await _context.SaveChangesAsync();
        _logger.LogInformation("La personne à bien été ajoutée");
        return Ok("La personne à bien été ajoutée");
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