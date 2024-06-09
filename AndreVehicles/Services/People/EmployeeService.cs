using Models.People;
using Repositories.People;

namespace Services.People;

public class EmployeeService
{
    private EmployeeRepository _employeeRepository;


    public EmployeeService()
    {
        _employeeRepository = new EmployeeRepository();
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
        return _employeeRepository.Post(technology, employee);
    }

}
