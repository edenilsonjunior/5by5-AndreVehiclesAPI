using Models.People;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Insurances;

public class Driver : Person
{
    public DriversLicense License { get; set; }
}
