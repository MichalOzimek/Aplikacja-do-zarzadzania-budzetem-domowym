using System.ComponentModel.DataAnnotations;

namespace ProjectSoftwareWorkshop.Models.Shop;

public class ShopDto
{
    public int Id { get; set; }

    [Required] public required string Name { get; set; }
}