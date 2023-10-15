using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using VacancyProAPI.Models;
using VacancyProAPI.Services.ChatService;

namespace VacancyProAPI.Controllers;



[Route("api/chat")]
[ApiController]
public class ChatController
{

    private readonly IHubContext<ChatHub> _hubContext;
    
    
    public ChatController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [Route("send")]                                         
    [HttpPost]
    public IActionResult SendRequest([FromBody] MessageDto msg)
    {
        _hubContext.Clients.All.SendAsync("ReceiveOne", msg.User, msg.MsgText);
        return null;
    }

}