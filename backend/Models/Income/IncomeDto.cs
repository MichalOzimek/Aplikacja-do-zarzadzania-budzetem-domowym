using ProjectSoftwareWorkshop.Models.Account;
using System;
using System.ComponentModel.DataAnnotations;
namespace ProjectSoftwareWorkshop.Models.Income;

public class IncomeDto
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public AccountDto? Account { get; set; } = default!;

    public int AccountId { get; set; }
}
