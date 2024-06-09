using Models.Cars;
using Repositories.Cars;

namespace Services.Cars;

public class OperationService
{
    private readonly OperationRepository _operationRepository;


    public OperationService()
    {
        _operationRepository = new OperationRepository();
    }


    public List<Operation> Get(string technology)
    {
        return _operationRepository.Get(technology);
    }


    public Operation Get(string technology, int id)
    {
        return _operationRepository.Get(technology, id);
    }


    public bool Post(string technology, Operation operation)
    {
        return _operationRepository.Post(technology, operation);
    }

}
