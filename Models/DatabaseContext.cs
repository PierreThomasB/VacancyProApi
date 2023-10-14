using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Models;

public class DatabaseContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Period> Periods { get; set; }

    public DbSet<Activity> Activities { get; set; }
    
    public DbSet<ActivityPlace> ActivityPlaces { get; set; }
    public DbSet<PeriodPlace> PeriodPlaces { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
}