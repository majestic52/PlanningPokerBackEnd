using Microsoft.EntityFrameworkCore;
using Poker2.Models;

namespace Poker2.DataAccess;

public class TableDBContext : DbContext
{
    private readonly IConfiguration _configuration;

    public TableDBContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Tables> Tables => Set<Tables>();
    public DbSet<Users> Users => Set<Users>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));   
    }
}