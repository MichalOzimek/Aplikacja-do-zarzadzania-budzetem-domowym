using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Data;
using ProjectSoftwareWorkshop.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectSoftwareWorkshop.Repositories
{
    public class IncomesRepository : GenericRepository<Income>, IIncomesRepository
    {
        public IncomesRepository(ProjectSoftwareWorkshopDbContext context) : base(context) { }

        public override async Task<List<Income>> GetAllAsync()
        {
            return await Context.Incomes
                .Include(i => i.Account)
                .ToListAsync();
        }
    }
}
