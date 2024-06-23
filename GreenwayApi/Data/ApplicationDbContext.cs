using GreenwayApi.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenwayApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Collect> Collects { get; set; }
    
}