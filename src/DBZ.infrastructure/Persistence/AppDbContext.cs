using DBZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBZ.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Personagem> DBZ { get; set; } = null!;
}
