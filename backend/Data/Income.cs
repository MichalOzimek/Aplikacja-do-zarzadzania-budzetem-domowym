using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSoftwareWorkshop.Data;

[Table("Incomes", Schema = "ProjectSoftwareWorkshop")]
public class Income
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Source { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [ForeignKey(nameof(Account))]
    public int AccountId { get; set; }

    public Account? Account { get; set; }
}
