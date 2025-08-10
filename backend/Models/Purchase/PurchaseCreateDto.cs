namespace ProjectSoftwareWorkshop.Models.Purchase;

public class PurchaseCreateDto
{
    public int ShopId { get; set; }
    public int CategoryId { get; set; }
    public decimal BillCost { get; set; }
    public DateTime Date { get; set; }
    public string Note { get; set; } = string.Empty;

    public int AccountId { get; set; }
}