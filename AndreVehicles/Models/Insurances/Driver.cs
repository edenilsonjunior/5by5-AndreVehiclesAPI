using Models.People;

namespace Models.Insurances;

public class Driver : Person
{
    public DriversLicense License { get; set; }
}
