using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;

namespace ProjectSoftwareWorkshop.Repositories;

public class PurchasesRepository : GenericRepository<Purchase>, IPurchasesRepository
{
    public PurchasesRepository(ProjectSoftwareWorkshopDbContext context) : base(context)
    {
    }
    
    public override async Task<List<Purchase>> GetAllAsync()
    {
        return await Context.Purchases
            .Include(p => p.Shop)
            .Include(p => p.Category)
            .Include(p => p.Account)
            .ToListAsync();
    }
}