using Models.People;
using Repositories.People;

namespace Services.People;

public class DependentService
{
    private DependentRepository _repository;
    private AddressRepository _addressRepository;

    public DependentService()
    {
        _repository = new();
        _addressRepository = new();
    }

    public List<Dependent> Get() => _repository.Get();

    public Dependent Get(string document) => _repository.Get(document);

    public bool Post(Dependent dependent)
    {
        dependent.Address.Id = _addressRepository.Post("ado", dependent.Address);
        return _repository.Post(dependent);
    }
}
