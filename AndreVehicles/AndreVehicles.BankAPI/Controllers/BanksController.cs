using Microsoft.AspNetCore.Mvc;
using Models.Financials;
using Models.People;
using Services.Financials;

namespace AndreVehicles.BankAPI.Controllers;

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
    public ActionResult<List<Bank>> Get()
    {
        var bankList = _bankService.Get();
        if (bankList == null)
            return NotFound();


        return bankList;
    }

    [HttpGet("{cnpj}", Name = "GetBank")]
    public ActionResult<Bank> Get(string cnpj)
    {
        var bank = _bankService.Get(cnpj);
        if (bank == null)
            return NotFound();

        return bank;
    }

    [HttpPost]
    public ActionResult<Bank> Create(Bank bank)
    {
        _bankService.Create(bank);

        return CreatedAtRoute("GetBank", new { cnpj = bank.Cnpj }, bank);
    }


    [HttpGet("/GetBankByCnpj/{cnpj}")]
    public ActionResult<Bank> GetBankByCnpj(string cnpj)
    {
        var bank = _bankService.Get(cnpj);

        return bank != null ? Ok(bank) : NotFound();
    }

}
