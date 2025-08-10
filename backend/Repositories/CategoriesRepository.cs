using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;

namespace ProjectSoftwareWorkshop.Repositories;

public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
{
    public CategoriesRepository(ProjectSoftwareWorkshopDbContext context) : base(context)
    {
    }
}