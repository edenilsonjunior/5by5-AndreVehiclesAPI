using AndreVehicles.AddressAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO.People;
using Models.People;
using Services.People;
using System.Net;

namespace AndreVehicles.AddressAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly AndreVehiclesAddressAPIContext _context;
    private readonly AddressService _service;
    private readonly string InvalidTechnology = "Invalid technology. Valid values are: entity, dapper, ado";

    public AddressesController(AndreVehiclesAddressAPIContext context, AddressService addressService)
    {
        _context = context;
        _service = addressService;
    }


    [HttpGet("{technology}")]
    public async Task<ActionResult<IEnumerable<Address>>> GetAddress(string technology)
    {
        return technology switch
        {
            "entity" => await GetAllWithEntity(),
            "dapper" or "ado" => await GetAllWithDapperOrAdo(technology),
            _ => BadRequest(InvalidTechnology),
        };
    }



    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Address>> GetAddress(string technology, int id)
    {
        return technology switch
        {
            "entity" => await GetByIdWithEntity(id),
            "dapper" or "ado" => await GetByIdWithDapperOrAdo(technology, id),
            _ => BadRequest(InvalidTechnology),
        };
    }



    [HttpPost("{technology}")]
    public async Task<ActionResult<Address>> PostAddress(string technology, AddressDTO addressDTO)
    {
        Address address = await _service.GetAddressByPostalCode(addressDTO.PostalCode);

        address.PostalCode = addressDTO.PostalCode;
        address.AdditionalInfo = addressDTO.AdditionalInfo;
        address.Number = addressDTO.Number;
        address.StreetType = addressDTO.StreetType;

        if (address == null)
            return BadRequest("Failed to get address by postal code.");

        if (_service.PostMongo(address) == null)
            return BadRequest("Failed to save address in MongoDB.");


        return (technology) switch
        {
            "entity" => await PostWithEntity(technology, address),
            "dapper" or "ado" => await PostWithDapperOrAdo(technology, address),
            _ => BadRequest(InvalidTechnology),
        };
    }


    [HttpGet("/GetAddressByCep/{cep}")]
    public async Task<ActionResult<Address>> GetAddressByCep(string cep)
    {
        Address address = await _service.GetAddressByPostalCode(cep);
        return address != null ? Ok(address) : NotFound();
    }



    private async Task<ActionResult<Address>> GetByIdWithEntity(int id)
    {
        if (_context.Address == null)
            return NotFound();

        var address = await _context.Address.FindAsync(id);
        return address != null ? Ok(address) : NotFound();
    }

    private async Task<ActionResult<Address>> GetByIdWithDapperOrAdo(string technology, int id)
    {
        var address = _service.Get(technology, id);
        return address != null ? Ok(address) : NotFound();
    }



    private async Task<ActionResult<IEnumerable<Address>>> GetAllWithEntity()
    {
        if (_context.Address == null)
            return NotFound();

        return await _context.Address.ToListAsync();
    }

    private async Task<ActionResult<IEnumerable<Address>>> GetAllWithDapperOrAdo(string technology)
    {
        var list = _service.Get(technology);
        return list != null ? Ok(list) : NotFound();
    }



    private async Task<ActionResult<Address>> PostWithEntity(string technology, Address address)
    {
        if (_context.Address == null)
            return Problem("Entity set 'AndreVehiclesAddressAPIContext.Address' is null.");

        _context.Address.Add(address);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAddress", new { technology, id = address.Id }, address);
    }

    private async Task<ActionResult<Address>> PostWithDapperOrAdo(string technology, Address address)
    {
        bool sucess = _service.Post(technology, address);

        if (sucess)
            return CreatedAtAction("GetAddress", new { technology, id = address.Id }, address);
        else
            return BadRequest("Failed to insert Address.");
    }
}
