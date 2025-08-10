namespace ProjectSoftwareWorkshop.Models.Category;

public class CategoryBudgetDto
{
    public CategoryDto Category { get; set; } = null!;
    public decimal Total { get; set; }
}