using Microsoft.AspNetCore.Mvc;
using VacancyProAPI.Models;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AnonymUserController : ControllerBase
{

    private readonly ApplicationContext _context;
    
    public AnonymUserController(ApplicationContext context) 
    {
        _context = context;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Vacances>> Anonym(int id)
    {
        var result = await this._context.AnonymUsers.FindAsync(id);
        if (result == null)
        {
            return NotFound("L'id n'a pas été trouvé");
        }

        return Ok(result);

    }
    
    

    [HttpPost("NewAnonym")]
    public async Task<IActionResult> Post([FromBody] AnonymUser anonym)
    {
        AnonymUser anonymUserObj = new AnonymUser()
        {
            Description = anonym.Description,
            Sujet = anonym.Sujet,
            Email = anonym.Email,

        };
        _context.AnonymUsers.Add(anonymUserObj);
        await _context.SaveChangesAsync();

        return CreatedAtAction("Anonym", new { id = anonymUserObj.IdAnonym }, anonymUserObj);



    }
    
    
    
}