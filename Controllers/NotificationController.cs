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
    private readonly Logger<NotificationController> _logger;


    public NotificationController(DatabaseContext context, IUserService userService , Logger<NotificationController> logger)
    {
        _context = context;
        _userService = userService;
        _logger = logger;
    }
    
    
    
    [HttpGet("NotificationFromUser")]
    public  async Task<ActionResult<List<Notification>>> GetUserNotification()
    {
        string userId = _userService.GetUserIdFromToken()!;
        var user = await _context.Users.FindAsync(userId);
        var result = await _context.Notifications.Where(u => u.User == user).ToListAsync();
        _logger.LogInformation("User {userId} get notifications", userId);
        return Ok(result);

    }
    
    [HttpDelete("Notification")]
    public  async Task<ActionResult> DeleteNotification(int id )
    {
        var notification = await _context.Notifications.FindAsync(id);
        if(notification == null)
        {
            _logger.LogInformation("Notification {notificationId} not found", id);
            return NotFound("Notification not found");
        }
        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Notification {notificationId} deleted", id);
        return Ok("Notification deleted");

    }
}