namespace Models.Cars;


public class CarOperation
{
    public readonly static string POST = "INSERT INTO CarOperation(CarPlate, OperationId) VALUES(@CarPlate, @OperationId)";

    public readonly static string GETALL = @"
    select 
        c.Plate, 
        c.Name, 
        c.YearManufacture, 
        c.YearModel, 
        c.Color, 
        c.Sold,
        o.Description,
        o.Id AS OperationId,
        co.Id AS CarOperationId,
        co.Status AS CarOperationStatus
    from Car c
    JOIN CarOperation co ON c.Plate = co.CarPlate
    JOIN Operation o ON co.OperationId = o.Id";

    public readonly static string GET = GETALL + "WHERE c.Plate = @Plate";

    public int Id { get; set; }
    public Car Car { get; set; }
    public Operation Operation { get; set; }
    public bool Status { get; set; }

    public CarOperation() { }

    public CarOperation(Car car, Operation operation, bool status)
    {
        Car = car;
        Operation = operation;
        Status = status;
    }
}
