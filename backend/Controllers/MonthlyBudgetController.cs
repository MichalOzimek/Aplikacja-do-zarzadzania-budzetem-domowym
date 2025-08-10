using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Models.Category;
using ProjectSoftwareWorkshop.Models.Shop;

namespace ProjectSoftwareWorkshop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MonthlyBudgetController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPurchasesRepository _purchasesRepository;

    public MonthlyBudgetController(IMapper mapper, IPurchasesRepository purchasesRepository)
    {
        _mapper = mapper;
        _purchasesRepository = purchasesRepository;
    }

    [HttpGet("monthlyCategoriesBudget/{month}/{year}")]
    public async Task<ActionResult<List<CategoryBudgetDto>>> GetMonthlyCategoriesBudget(int month, int year)
    {
        var purchases = await _purchasesRepository.GetAllAsync();

        var filteredPurchases = purchases
            .Where(p => p.Date.Month == month && p.Date.Year == year)
            .ToList();

        var categoryGroups = filteredPurchases
            .GroupBy(p => p.Category.Id)
            .ToList();

        var result = new List<CategoryBudgetDto>();

        foreach (var group in categoryGroups)
        {
            var category = group.First().Category;
            var categoryDto = _mapper.Map<CategoryDto>(category);
            var total = group.Sum(p => p.BillCost);

            result.Add(new CategoryBudgetDto
            {
                Category = categoryDto,
                Total = total
            });
        }

        return Ok(result);
    }
    
    [HttpGet("monthlyShopsBudget/{month}/{year}")]
    public async Task<ActionResult<List<ShopBudgetDto>>> GetMonthlyShopsBudget(int month, int year)
    {
        var purchases = await _purchasesRepository.GetAllAsync();

        var filteredPurchases = purchases
            .Where(p => p.Date.Month == month && p.Date.Year == year)
            .ToList();

        var shopGroups = filteredPurchases
            .GroupBy(p => p.Shop.Id)
            .ToList();

        var result = new List<ShopBudgetDto>();

        foreach (var group in shopGroups)
        {
            var shop = group.First().Shop;
            var shopDto = _mapper.Map<ShopDto>(shop);
            var total = group.Sum(p => p.BillCost);

            result.Add(new ShopBudgetDto
            {
                Shop = shopDto,
                Total = total
            });
        }

        return Ok(result);
    }
    
}