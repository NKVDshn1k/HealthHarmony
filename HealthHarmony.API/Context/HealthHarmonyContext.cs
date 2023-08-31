using HealthHarmony.API.Entity;
using Microsoft.EntityFrameworkCore;

namespace HealthHarmony.API.Context;

public class HealthHarmonyContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public HealthHarmonyContext(DbContextOptions<HealthHarmonyContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
    }
}
