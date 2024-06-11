using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.People;
using Models.People;
using Services.People;

namespace AndreVehicles.DependentAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DependentsController : ControllerBase
{
    private DependentService _service;
    private CustomerService _customerService;

    public DependentsController()
    {
        _service = new();
        _customerService = new();
    }



    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dependent>>> GetDependent()
    {
        var list = _service.Get();

        if (list == null)
            return NotFound("Can't load Dependents list!");

        return Ok(list);
    }

    [HttpGet("{document}")]
    public async Task<ActionResult<Dependent>> GetDependent(string document)
    {
        var dependent = _service.Get(document);

        if (dependent == null)
            return NotFound("Dependent not found!");

        return Ok(dependent);
    }

    [HttpPost]
    public async Task<ActionResult<Dependent>> PostDependent(DependetDTO dependentDTO)
    {
        var customer = _customerService.Get("ado", dependentDTO.CustomerDocument);

        if (customer == null)
            return NotFound("Customer not found!");

        Address address = await new AddressService().GetAddressByPostalCode(dependentDTO.Address);

        if (address == null)
            return NotFound("Address not found!");

        var dependent = new Dependent()
        {
            Document = dependentDTO.Document,
            Name = dependentDTO.Name,
            BirthDate = dependentDTO.BirthDate,
            Address = address,
            Phone = dependentDTO.Phone,
            Email = dependentDTO.Email,
            Customer = customer
        };

        bool sucess = _service.Post(dependent);
        return sucess ? CreatedAtAction("GetDependent", new { document = dependent.Document }, dependent) : BadRequest("Can't create Dependent!");
    }
}
