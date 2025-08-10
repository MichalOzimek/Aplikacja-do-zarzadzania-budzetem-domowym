using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Data;
using ProjectSoftwareWorkshop.Models.Summary;

namespace ProjectSoftwareWorkshop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SummaryController : ControllerBase
{
    private readonly ProjectSoftwareWorkshopDbContext _context;

    public SummaryController(ProjectSoftwareWorkshopDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<SummaryDto>> GetSummary()
    {
        var now = DateTime.Now;

        var incomeSum = await _context.Incomes
            .Where(i => i.Date.Month == now.Month && i.Date.Year == now.Year)
            .SumAsync(i => i.Amount);

        var expenseSum = await _context.Purchases
            .Where(p => p.Date.Month == now.Month && p.Date.Year == now.Year)
            .SumAsync(p => p.BillCost);

        var balanceSum = await _context.Accounts.SumAsync(a => a.Balance);

        var result = new SummaryDto
        {
            Incomes =  balanceSum,
            Expenses = expenseSum,
            Balance = balanceSum
        };

        return Ok(result);
    }



    [HttpGet("month/{month}/year/{year}")]
    public async Task<ActionResult<SummaryDto>> GetMonthlySummary(int month, int year)
    {
        var incomes = await _context.Incomes
            .Where(i => i.Date.Month == month && i.Date.Year == year)
            .SumAsync(i => i.Amount);

        var expenses = await _context.Purchases
            .Where(p => p.Date.Month == month && p.Date.Year == year)
            .SumAsync(p => p.BillCost);

        return Ok(new SummaryDto
        {
            Incomes = incomes,
            Expenses = expenses,
            Balance = incomes - expenses
        });
    }

    [HttpGet("balance-history")]
    public async Task<ActionResult<IEnumerable<BalancePointDto>>> GetDailyBalanceHistory()
    {
        var today = DateTime.Today;
        var last7Days = Enumerable.Range(0, 7)
            .Select(i => today.AddDays(-6 + i).Date)
            .ToList();

        var incomeGroups = await _context.Incomes
            .Where(i => i.Date.Date >= last7Days.First() && i.Date.Date <= today)
            .GroupBy(i => i.Date.Date)
            .Select(g => new { Date = g.Key, Incomes = g.Sum(i => i.Amount) })
            .ToDictionaryAsync(x => x.Date, x => x.Incomes);

        var expenseGroups = await _context.Purchases
            .Where(p => p.Date.Date >= last7Days.First() && p.Date.Date <= today)
            .GroupBy(p => p.Date.Date)
            .Select(g => new { Date = g.Key, Expenses = g.Sum(p => p.BillCost) })
            .ToDictionaryAsync(x => x.Date, x => x.Expenses);

        var result = last7Days.Select(date =>
        {
            var income = incomeGroups.TryGetValue(date, out var inc) ? inc : 0;
            var expense = expenseGroups.TryGetValue(date, out var exp) ? exp : 0;

            return new BalancePointDto
            {
                Day = date.Day,
                Month = date.Month,
                Year = date.Year,
                Date = date,
                Incomes = income,
                Expenses = expense,
                Balance = income - expense
            };
        }).ToList();

        return Ok(result);
    }
}
