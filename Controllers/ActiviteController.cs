using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ActiviteController : ControllerBase
{
    private readonly ApplicationContext _context;

    public ActiviteController(ApplicationContext context)
    {
        _context = context;
    }
    
    
    
    [HttpGet("AllActivite")]
    public async Task<ActionResult<IEnumerable<Vacances>>> GetAllActivite()
    {
        var val = await _context.Activites.ToListAsync();
        return  Ok(val);
    }
    
    
    
    
    
}