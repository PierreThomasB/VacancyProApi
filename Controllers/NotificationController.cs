using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Models.ViewModels;
using VacancyProAPI.Services.UserService;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationController : ControllerBase
{
    
    private readonly DatabaseContext _context;
    private readonly IUserService _userService;



    public NotificationController(DatabaseContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }
    
    
    [HttpPost("NotificationToUser")]
    public async Task<ActionResult> PostNewMessage([FromBody]Notification notification)
    {
        await _context.AddAsync(notification);
        return Ok();
    }
    
    [HttpGet("NotificationfromUser")]
    public  async Task<ActionResult<List<Notification>>> GetUserNotification()
    {
        string userId = _userService.GetUserIdFromToken()!;
        var result =  _context.Users.Include(a => a.Notifications).First(u => u.Id == userId);
        

        return Ok(new NotificationViewModel( result.Notifications.Count , result.Notifications ));

    }
    
    [HttpDelete("Notification")]
    public  async Task<ActionResult> DeleteNotification(int id )
    {
        var notification = await _context.Notifications.FindAsync(id);
        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();

        return Ok();

    }
}