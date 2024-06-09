using Models.Cars;
using Repositories.Cars;

namespace Services.Cars;

public class CarService
{
    private readonly CarRepository _carRepository;

    public CarService()
    {
        _carRepository = new CarRepository();
    }


    public List<Car> Get(string technology)
    {
        return _carRepository.Get(technology);
    }


    public Car Get(string technology, string plate)
    {
        return _carRepository.Get(technology, plate);
    }


    public bool Post(string technology, Car car)
    {
        return _carRepository.Post(technology, car);
    }

}
