using Microsoft.EntityFrameworkCore;

namespace VacancyProAPI.Models;

public class ApplicationContext : DbContext
{

    public DbSet<Vacances> Vacances { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
}