using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class VacancesController : ControllerBase
{
    
    private readonly ApplicationContext _context;
    public VacancesController(ApplicationContext context)
    {
        this._context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vacances>>> GetVacances()
    {
        return  await _context.Vacances.ToListAsync();
    }
}