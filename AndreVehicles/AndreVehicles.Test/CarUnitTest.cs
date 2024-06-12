using AndreVehicles.CarAPI.Controllers;
using AndreVehicles.CarAPI.Data;
using Microsoft.EntityFrameworkCore;
using Models.Cars;

namespace AndreVehicles.Test
{
    public class CarUnitTest
    {
        private DbContextOptions<AndreVehiclesCarAPIContext> options;

        private void InitializeDataBase()
        {
            options = new DbContextOptionsBuilder<AndreVehiclesCarAPIContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var context = new AndreVehiclesCarAPIContext(options))
            {
                context.Car.Add(new Models.Cars.Car 
                { 
                    Plate = "ABC1234", 
                    Name = "Polo tsi", 
                    YearManufacture = 2000, 
                    YearModel = 2000, 
                    Color = "blue", 
                    Sold = false 
                });

                context.Car.Add(new Models.Cars.Car
                {
                    Plate = "AAA111",
                    Name = "Polo tsi",
                    YearManufacture = 2000,
                    YearModel = 2000,
                    Color = "blue",
                    Sold = false
                });

                context.Car.Add(new Models.Cars.Car
                {
                    Plate = "BBB222",
                    Name = "Polo tsi",
                    YearManufacture = 2000,
                    YearModel = 2000,
                    Color = "blue",
                    Sold = false
                });

                context.SaveChanges();
            }
        }

        [Fact]
        public void GetAllTest()
        {
            InitializeDataBase();

            using (var context = new AndreVehiclesCarAPIContext(options))
            {
                var controller = new CarsController(context);

                IEnumerable<Car> result =  controller.GetCar("entity").Result.Value;

                Assert.Equal(3, result.Count());
            }
        }

        [Fact]
        public void GetByPlateTest()
        {
            InitializeDataBase();

            using (var context = new AndreVehiclesCarAPIContext(options))
            {
                var controller = new CarsController(context);

                Car result = controller.GetCar("entity", "ABC1234").Result.Value;

                Assert.Equal("ABC1234", result.Plate);
            }
        }
    }
}