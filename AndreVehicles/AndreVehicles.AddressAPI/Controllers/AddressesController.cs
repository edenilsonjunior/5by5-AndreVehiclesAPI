using AndreVehicles.AddressAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO.People;
using Models.People;
using Services.People;

namespace AndreVehicles.AddressAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly AndreVehiclesAddressAPIContext _context;
    private readonly AddressService _service;

    public AddressesController(AndreVehiclesAddressAPIContext context)
    {
        _context = context;
        _service = new AddressService();
    }


    [HttpGet("{technology}")]
    public async Task<ActionResult<IEnumerable<Address>>> GetAddress(string technology)
    {
        switch (technology)
        {
            case "entity":
                if (_context.Address == null)
                    return NotFound();

                return await _context.Address.ToListAsync();

            case "dapper":
            case "ado":
                var list = _service.Get(technology);
                return list != null ? Ok(list) : NotFound();
            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Address>> GetAddress(string technology, int id)
    {
        switch (technology)
        {
            case "entity":
                if (_context.Address == null)
                    return NotFound();

                return await _context.Address.FindAsync(id);

            case "dapper":
            case "ado":
                var a = _service.Get(technology, id);
                return a != null ? Ok(a) : NotFound();
            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }

    }


    [HttpPost("{technology}")]
    public async Task<ActionResult<Address>> PostAddress(string technology, AddressDTO addressDTO)
    {
        Address address = await _service.GetAddressByPostalCode(addressDTO);

        if (address == null)
            return BadRequest("Failed to get address by postal code.");


        if(new AddressService().PostMongo(address) == null)
            return BadRequest("Failed to save address in MongoDB.");

        switch (technology)
        {
            case "entity":
                if (_context.Address == null)
                    return Problem("Entity set 'AndreVehiclesAddressAPIContext.Address'  is null.");

                _context.Address.Add(address);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAddress", new { technology, id = address.Id }, address);

            case "dapper":
            case "ado":
                address.Id = _service.Post(technology, address);

                return address.Id != -1 ? CreatedAtAction("GetAddress", new { technology, id = address.Id }, address) : BadRequest("Failed to insert Address.");

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }










































    }

    /*
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAddress(int id, Address address)
    {
        if (id != address.Id)
        {
            return BadRequest();
        }

        _context.Entry(address).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AddressExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Addresses/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        if (_context.Address == null)
        {
            return NotFound();
        }
        var address = await _context.Address.FindAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        _context.Address.Remove(address);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    */

    private bool AddressExists(int id)
    {
        return (_context.Address?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
