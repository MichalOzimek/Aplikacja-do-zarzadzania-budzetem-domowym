using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSoftwareWorkshop.Data;

[Table("Purchases", Schema = "ProjectSoftwareWorkshop")]
public class Purchase
{
    public int Id { get; set; }

    [ForeignKey(nameof(ShopId))] public int ShopId { get; set; }
    public required Shop Shop { get; set; }

    [Required] public decimal BillCost { get; set; }

    [ForeignKey(nameof(CategoryId))] public int CategoryId { get; set; }
    public required Category Category { get; set; }
    
    public required DateTime Date { get; set; } = DateTime.Now;

    public required string Note { get; set; }

    [ForeignKey(nameof(Account))]
    public int AccountId { get; set; }

    public Account? Account { get; set; }
}