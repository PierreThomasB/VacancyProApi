
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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



    private readonly Pusher _pusher;
    private readonly DatabaseContext _context;
    private readonly IUserService _userService;


    public ChatController(DatabaseContext context , ILogger<ChatController> logger, UserManager<User> userManager, IUserService userService)
    {
        this._context = context;
        _userService = userService;

        var options = new PusherOptions
        {
            Cluster = "eu",
            Encrypted = true
        };

        this._pusher = new Pusher(
            "1708130",
            "74f1716b51dbbc6c19ca",
            "c3341eb1f00700d5711a",
            options);

    }



    [HttpPost("NewMessage")]
    public async Task<ActionResult> PostNewMessage([FromBody]Chat chat)
    {
        Check100Message();
        string userId = _userService.GetUserIdFromToken()!;
        var user = await _context.Users.FindAsync(userId);
        chat.User = user;
        
         _context.Messages.Add(chat);
        await _pusher.TriggerAsync(chat.Channel, "my-event", new { Date = chat.Date ,Message =  chat.Message  , User = user});
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetMessage", new { id = chat.Id }, chat);

    }


    private async void Check100Message()
    {
        var messages =  _context.Messages.OrderByDescending(m => m.Date);
        int count = _context.Messages.Count();
        if (count > 100)
        {
            foreach (var message in messages)
            {
                _context.Messages.Remove(message);
                count--;
                if (count < 100)
                {
                    break;
                }
            }
        }
        await  _context.SaveChangesAsync();
    }


    [HttpGet("Message")]
    [Produces("application/json")]

    public async Task<ActionResult> GetMessage(int id)
    {
        var values = await _context.Messages.FindAsync(id);
        return Ok(values);
    }
    
    [HttpGet("AllMessage")]
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Permet de recupérer les messages d'un channel")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Le channel n'est pas valide", typeof(ErrorViewModel))]
    public async Task<ActionResult> GetAllMessage(string channel)
    {
        var values =  _context.Messages.Include(m => m.User).Where(a => a.Channel == channel).OrderBy(m => m.Date);

        return Ok(values);

    }



}