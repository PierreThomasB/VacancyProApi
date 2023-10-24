using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Controllers
{
    /// <summary>
    /// Controller qui permet de gérer les périodes de vacances
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PeriodController : ControllerBase
    {
    
        private readonly DatabaseContext _context;
        public PeriodController(DatabaseContext context)
        {
            this._context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Period>>> GetPeriods()
        {
            return  await _context.Periods.ToListAsync();
        }
    }
}


