using Microsoft.EntityFrameworkCore;
using WatchCollectionApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Watch> Watches => Set<Watch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Watch>().HasData(
            new Watch
            {
                Id = 1,
                Brand = "Casio",
                Model = "G-Shock",
                Year = 2020
            },
            new Watch
            {
                Id = 2,
                Brand = "Rolex",
                Model = "Submariner",
                Year = 2022
            }
        );
    }
}