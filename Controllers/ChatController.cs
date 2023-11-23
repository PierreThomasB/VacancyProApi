
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PusherServer;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class ChatController : ControllerBase
{



    private readonly Pusher _pusher;
    private readonly DatabaseContext _context;
 

    public ChatController(DatabaseContext context , ILogger<ChatController> logger)
    {
        this._context = context;
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
         _context.Messages.Add(chat);
        await _pusher.TriggerAsync(chat.Channel, "my-event", new { chat.Message });
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetMessage", new { id = chat.Id }, chat);

    }


    [HttpGet("Message")]
    public async Task<ActionResult> GetMessage(int id)
    {
        var values = await _context.Messages.FindAsync(id);
        return Ok(values);
    }
    
    [HttpGet("AllMessage")]
    public async Task<ActionResult> GetAllMessage(string channel)
    {
        var values =  _context.Messages.Where(a => a.Channel == channel);

        return Ok(values);

    }



}