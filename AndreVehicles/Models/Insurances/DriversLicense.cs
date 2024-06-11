using System.ComponentModel.DataAnnotations;

namespace Models.Insurances;

public class DriversLicense
{
    [Key]
    public long License { get; set; }
    public DateTime DueDate { get; set; }
    public string Rg { get; set; }
    public string Cpf { get; set; }
    public string MotherName { get; set; }
    public string FatherName { get; set; }
    public DriversLicenseCategory Category { get; set; }
}
