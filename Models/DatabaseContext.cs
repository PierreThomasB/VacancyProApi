using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Models;

public class DatabaseContext : DbContext
{

    public DbSet<Period> Periods { get; set; } = null!;
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
}