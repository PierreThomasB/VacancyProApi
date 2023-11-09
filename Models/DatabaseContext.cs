using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Models;

public class DatabaseContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Period> Periods { get; set; }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<Place> Places { get; set; }
    
    public DbSet<AnonymUser> AnonymUsers { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().HasMany(u => u.Periods);

        builder.Entity<Period>().HasOne(p => p.Creator);
        builder.Entity<Period>().HasMany(p => p.ListUser);
        builder.Entity<Period>().HasMany(p => p.ListActivity);
    }
}