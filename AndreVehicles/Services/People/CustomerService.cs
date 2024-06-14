using Models.People;
using Repositories.People;

namespace Services.People;

public class CustomerService
{
    private CustomerRepository _customerRepository;
    private AddressRepository _addressRepository;

    public CustomerService()
    {
        _customerRepository = new CustomerRepository();
        _addressRepository = new AddressRepository();
    }


    public List<Customer> Get(string technology)
    {
        return _customerRepository.Get(technology);
    }


    public Customer Get(string technology, string document)
    {
        return _customerRepository.Get(technology, document);
    }


    public bool Post(string technology, Customer customer)
    {
         _addressRepository.Post(technology, customer.Address);

        return _customerRepository.Post(technology, customer);
    }

}
