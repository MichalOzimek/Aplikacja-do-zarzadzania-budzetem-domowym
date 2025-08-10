using System.ComponentModel.DataAnnotations;

namespace ProjectSoftwareWorkshop.Models.Purchase;

public class PurchaseUpdateDto
{
    public int Id { get; set; }

    [Required]
    public string Note { get; set; } = string.Empty;

    [Required]
    public decimal BillCost { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int AccountId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public int? ShopId { get; set; }
}
