using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace Models.Cars;


public class Car
{

    public readonly static string POST = "INSERT INTO Car(Plate, Name, YearManufacture, YearModel, Color, Sold) VALUES(@Plate, @Name, @YearManufacture, @YearModel, @Color, @Sold)";
    public readonly static string GETALL = "SELECT Plate, Name, YearManufacture, YearModel, Color, Sold FROM Car";
    public readonly static string GET = GETALL + " WHERE Plate = @Plate";

    [Key]
    public string Plate { get; set; }
    public string Name { get; set; }
    public int YearManufacture { get; set; }
    public int YearModel { get; set; }
    public string Color { get; set; }
    public bool Sold { get; set; }

    public Car() { }

    public Car(string plate, string name, int yearManufacture, int yearModel, string color, bool sold)
    {
        Plate = plate;
        Name = name;
        YearManufacture = yearManufacture;
        YearModel = yearModel;
        Color = color;
        Sold = sold;
    }

}
