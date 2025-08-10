using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;

namespace ProjectSoftwareWorkshop.Repositories;

public class ShopsRepository : GenericRepository<Shop>, IShopsRepository
{
    public ShopsRepository(ProjectSoftwareWorkshopDbContext context) : base(context)
    {
    }
}