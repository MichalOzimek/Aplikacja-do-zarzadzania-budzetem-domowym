using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;

namespace ProjectSoftwareWorkshop.Repositories;

public class AccountsRepository : GenericRepository<Account>, IAccountsRepository
{
    public AccountsRepository(ProjectSoftwareWorkshopDbContext context) : base(context)
    {
    }
}
