using Models.Cars;
using Models.People;

namespace Models.Insurances;

public class Insurance
{
    public static readonly string POST = @"INSERT INTO Insurance(CustomerDocument, Deductible, CarPlate, MainDriverDocument) 
                                            VALUES (@CustomerDocument, @Deductible, @CarPlate, @MainDriverDocument)";

    public static readonly string GETALL = "SELECT Id, CustomerDocument, Deductible, CarPlate, MainDriverDocument FROM Insurance";
    public static readonly string GET = GETALL + " WHERE Id = @Id";

    public int Id { get; set; }
    public Customer Customer { get; set; }
    public string Deductible { get; set; }
    public Car Car { get; set; }
    public Driver MainDriver { get; set; }
}
