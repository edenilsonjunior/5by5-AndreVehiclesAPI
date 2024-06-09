using Models.People;
using Repositories.People;

namespace Services.People;

public class AddressService
{
    private AddressRepository _addressRepository;


    public AddressService()
    {
        _addressRepository = new AddressRepository();
    }


    public List<Address> Get(string technology)
    {
        return _addressRepository.Get(technology);
    }


    public Address Get(string technology, int id)
    {
        return _addressRepository.Get(technology, id);
    }


    public bool Post(string technology, Address address)
    {
        return _addressRepository.Post(technology, address);
    }


}
