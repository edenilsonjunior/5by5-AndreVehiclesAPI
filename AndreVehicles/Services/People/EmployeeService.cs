using Models.People;
using Repositories.People;

namespace Services.People;

public class EmployeeService
{
    private EmployeeRepository _employeeRepository;
    private AddressRepository _addressRepository;


    public EmployeeService()
    {
        _employeeRepository = new EmployeeRepository();
        _addressRepository = new AddressRepository();
    }

    public List<Employee> Get(string technology)
    {
        return _employeeRepository.Get(technology);
    }


    public Employee Get(string technology, string document)
    {
        return _employeeRepository.Get(technology, document);
    }


    public bool Post(string technology, Employee employee)
    {
        _addressRepository.Post(technology, employee.Address);
        return _employeeRepository.Post(technology, employee);
    }
}
