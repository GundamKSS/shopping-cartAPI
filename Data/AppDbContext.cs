using Microsoft.EntityFrameworkCore;
using MyAPI_1.Models;

namespace MyAPI_1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<UserProfile> Profiles { get; set; }
    public DbSet<Product> Products { get; set; }
}