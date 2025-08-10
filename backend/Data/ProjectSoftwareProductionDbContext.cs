using Microsoft.EntityFrameworkCore;

namespace ProjectSoftwareWorkshop.Data;

public class ProjectSoftwareWorkshopDbContext : DbContext
{
    public ProjectSoftwareWorkshopDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Category> Kategories { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Account> Accounts { get; set; }
}