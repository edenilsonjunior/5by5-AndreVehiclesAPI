using Models.Cars;
using Repositories.Cars;

namespace Services.Cars;

public class CarOperationService
{
    private readonly CarOperationRepository _carOperationRepository;

    public CarOperationService()
    {
        _carOperationRepository = new CarOperationRepository();
    }


    public List<CarOperation> Get(string technology)
    {
        return _carOperationRepository.Get(technology);
    }


    public CarOperation Get(string technology, int id)
    {
        return _carOperationRepository.Get(technology, id);
    }


    public bool Post(string technology, CarOperation carOperation)
    {


        return _carOperationRepository.Post(technology, carOperation);
    }


}
