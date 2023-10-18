using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;



namespace VacancyProAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LieuController
{
    
    private readonly ApplicationContext _context;

    public LieuController(ApplicationContext context)
    {
        _context = context;
    }
    
    
    [HttpGet("AllLieux")]
    public async Task<ActionResult<IEnumerable<Lieux>>> GetAllLieux()
    {
        return null;
    }
    
    

    

    
}