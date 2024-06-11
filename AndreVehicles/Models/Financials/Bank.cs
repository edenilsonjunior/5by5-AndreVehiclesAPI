using System.ComponentModel.DataAnnotations;

namespace Models.Financials;

public class Bank
{
    [Key]
    public string Cnpj { get; set; }
    public string Name { get; set; }
    public DateTime FoundationDate { get; set; }
}
