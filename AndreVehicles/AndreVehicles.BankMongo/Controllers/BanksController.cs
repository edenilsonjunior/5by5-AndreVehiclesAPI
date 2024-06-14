using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Financials;
using Services.Financials;

namespace AndreVehicles.BankMongo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BanksController : ControllerBase
{
    private BankService _bankService;

    public BanksController(BankService bankService)
    {
        _bankService = bankService;
    }


    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var banks = _bankService.Get();
        return Ok(banks);
    }


    [HttpGet("{id:length(24)}", Name = "GetBank")]
    public async Task<ActionResult> Get(string id)
    {
        var bank = _bankService.Get(id);

        if (bank == null)
            return NotFound();

        return Ok(bank);
    }

    [HttpPost]
    public async Task<ActionResult> Create(Bank bank)
    {
        bank = _bankService.Create(bank);

        if (bank == null)
            return BadRequest("Cant insert bank in mongoDB!");

        return CreatedAtRoute("GetBank", new { id = bank.Cnpj }, bank);
    }
}
