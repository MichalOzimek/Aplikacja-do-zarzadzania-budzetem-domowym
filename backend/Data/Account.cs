using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSoftwareWorkshop.Data;

[Table("Accounts", Schema = "ProjectSoftwareWorkshop")]
public class Account
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Balance { get; set; }

    public ICollection<Income>? Incomes { get; set; }
}
