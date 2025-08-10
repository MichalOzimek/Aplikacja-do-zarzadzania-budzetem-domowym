using System.ComponentModel.DataAnnotations;

namespace ProjectSoftwareWorkshop.Models.Category;

public class CategoryDto
{
    public int Id { get; set; }

    [Required] public required string Name { get; set; }
}