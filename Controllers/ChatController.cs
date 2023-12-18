
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PusherServer;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Models.ViewModels;
using VacancyProAPI.Services.UserService;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class ChatController : ControllerBase
{



    private readonly ILogger<ChatController> _logger;
    private readonly Pusher _pusher;
    private readonly DatabaseContext _context;
    private readonly IUserService _userService;


 
    public ChatController(DatabaseContext context, ILogger<ChatController> logger, UserManager<User> userManager,
        IUserService userService )
    {
         _context = context;
        _userService = userService;
        _logger = logger;

        var options = new PusherOptions
        {
            Cluster = "eu",
            Encrypted = true
        };

       _pusher = new Pusher(
            "1708130",
            "74f1716b51dbbc6c19ca",
            "c3341eb1f00700d5711a",
            options);
    }



    [HttpPost("NewMessage")]
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Permet d'envoyer un message sur un channel spécifique")]
    [SwaggerResponse(StatusCodes.Status200OK, "Message envoyé ")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Les informations sont invalide")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "L'utilisateur n'existe pas")]
    public async Task<ActionResult> PostNewMessage([FromBody]Chat chat)
    {
        if (!ModelState.IsValid)
        {
            _logger.Log(LogLevel.Warning, "Le format du message n'est pas correst ");
            return BadRequest(new ErrorViewModel("Le format du message n'est pas correst "));
        }
        Check100Message();
        string userId = _userService.GetUserIdFromToken()!;
        User? user = await _context.Users.FindAsync(userId);
        if(user == null)
        {
            _logger.Log(LogLevel.Warning, "L'utilisateur n'a pas été trouvé ");
            return NotFound(new ErrorViewModel("L'utilisateur n'a pas été trouvé "));
        }
        chat.UserName = user.UserName;
         _context.Messages.Add(chat);
        await _context.SaveChangesAsync();
        await _pusher.TriggerAsync(chat.Channel, "my-event", new {Id = chat.Id, Date = chat.Date ,Message =  chat.Message , Channel = chat.Channel  , UserName = user.UserName});
        _logger.Log(LogLevel.Information, "Le message a été envoyé ");
        return CreatedAtAction("GetMessage", new { id = chat.Id }, chat);

    }


    private async void Check100Message()
    {
        
        var messages =  _context.Messages.OrderByDescending(m => m.Date);
        int count = _context.Messages.Count();
        if (count > 100)
        {
            int nbr = 0;
            foreach (var message in messages)
            {
                _context.Messages.Remove(message);
                count--;
                nbr++;
                if (count < 100)
                {
                    break;
                }
            }
        }
        await  _context.SaveChangesAsync();
        _logger.Log(LogLevel.Information , "Il y a eu une suppression de  "+ count + " messages" );
    }


    [HttpGet("Message")]
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Permet de recupérer un message avec son id ")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Le channel n'est pas valide")]

    public async Task<ActionResult> GetMessage(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.Log(LogLevel.Warning, "L'id n'est pas correct ");
            return BadRequest("L'id n'est pas correct");
            
        }
        var values = await _context.Messages.FindAsync(id);
        if (values == null)
        {
            _logger.Log(LogLevel.Warning, "Le message n'a pas été trouvé ");
            return NotFound("Le message n'a pas été trouvé ");
        }
        return Ok(values);
    }
    
    [HttpGet("AllMessage")]
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Permet de recupérer les messages d'un channel")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Le channel n'est pas valide")]
    public async Task<ActionResult> GetAllMessage(string channel)
    {
        if (channel.Length == 0 )
        {
            _logger.Log(LogLevel.Warning, "La valeur pour le channel n'est pas correcte ");
            return BadRequest("La valeur pour le channel n'est pas correcte ");
        }
        var values =  _context.Messages.Where(a => a.Channel == channel).OrderBy(m => m.Date);
        if (values.IsNullOrEmpty())
        {
            _logger.Log(LogLevel.Warning, "Le channel n'a pas été trouvé ");
            return NotFound("Le channel n'a pas été trouvé ");
        }
        _logger.Log(LogLevel.Information, "Les messages ont été récupérés ");
        return Ok(values);
    }
}