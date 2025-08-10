namespace ProjectSoftwareWorkshop.Models.Shop;

public class ShopBudgetDto
{
    public ShopDto Shop { get; set; } = null!;
    public decimal Total { get; set; }
}