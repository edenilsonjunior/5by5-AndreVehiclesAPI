using Models.Cars;
using Models.Insurances;
using Models.People;
using Repositories.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Insurances
{
    public class InsuranceRepository
    {

        public InsuranceRepository() { }

        public List<Insurance> Get()
        {
            List<Insurance> insurances = new List<Insurance>();

            var obj = DapperUtilsRepository<dynamic>.GetAll(Insurance.GETALL);

            foreach(var row in obj)
            {
                Customer customer = new CustomerRepository().Get("dapper", row.CustomerDocument);

                Car car = DapperUtilsRepository<Car>.Get(Car.GET, new { Plate = row.CarPlate });

                Driver? driver = ApiConsume<Driver>.Get("https://localhost:7194/api/Drivers/", $"{row.MainDriverDocument}").Result;

                Insurance insurance = new Insurance
                {
                    Id = row.Id,
                    Customer = customer,
                    Deductible = row.Deductible,
                    Car = car,
                    MainDriver = driver
                };

                insurances.Add(insurance);
            }

            return insurances;
        }

        public Insurance Get(int id)
        {
            /*
             "SELECT Id, CustomerDocument, Deductible, CarPlate, MainDriverDocument FROM Insurance"
             */

            dynamic obj = DapperUtilsRepository<dynamic>.Get(Insurance.GET, new { Id = id });

            Customer customer = new CustomerRepository().Get("dapper", obj.CustomerDocument);

            Car car = DapperUtilsRepository<Car>.Get(Car.GET, new { Plate = obj.CarPlate });

            Driver? driver = ApiConsume<Driver>.Get("https://localhost:7194/api/Drivers/", $"{obj.MainDriverDocument}").Result;

            Insurance insurance = new Insurance
            {
                Id = obj.Id,
                Customer = customer,
                Deductible = obj.Deductible,
                Car = car,
                MainDriver = driver
            };

            return insurance;
        }

        public bool Post(Insurance insurance)
        {
            object obj = new 
            {
                CustomerDocument = insurance.Customer.Document, 
                Deductible =  insurance.Deductible, 
                CarPlate = insurance.Car.Plate,
                MainDriverDocument = insurance.MainDriver.Document 
            };

            return DapperUtilsRepository<Insurance>.Insert(Insurance.POST, obj);
        }
    }
}
