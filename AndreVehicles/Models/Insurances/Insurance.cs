using Models.Cars;
using Models.People;

namespace Models.Insurances;

public class Insurance
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
    public string Deductible { get; set; }
    public Car Car { get; set; }
    public Driver MainDriver { get; set; }
}
