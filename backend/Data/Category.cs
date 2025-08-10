using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSoftwareWorkshop.Data;

[Table("Categories", Schema = "ProjectSoftwareWorkshop")]
public class Category
{
    public int Id { get; set; }

    [MaxLength(256)] [Required] public required string Name { get; set; }
    
}