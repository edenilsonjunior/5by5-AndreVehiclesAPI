using Microsoft.AspNetCore.Mvc;
using Models.DTO.Financials;
using Models.Financials;
using Models.Sales;
using Repositories;
using Services.Financials;

namespace AndreVehicles.CarFinancingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarFinancingController : ControllerBase
{
    private readonly CarFinancingService _carFinancingService;

    public CarFinancingController()
    {
        _carFinancingService = new();
    }



    [HttpGet]
    public ActionResult<IEnumerable<CarFinancing>> Get()
    {
        var list = _carFinancingService.Get();

        return list != null ? Ok(list) : NotFound();
    }



    [HttpGet("{id}", Name = "GetCarFinancing")]
    public ActionResult<CarFinancing> Get(int id)
    {
        var carFinancing = _carFinancingService.Get(id);

        return carFinancing != null ? Ok(carFinancing) : NotFound();
    }



    [HttpPost]
    public ActionResult Post(CarFinancingDTO carFinancingDTO)
    {
        var sale = ApiConsume<Sale>.Get("https://localhost:7237/api/Sales/", $"dapper/{carFinancingDTO.SaleId}");
        var bank = ApiConsume<Bank>.Get("https://localhost:7031/api/Banks/", $"{carFinancingDTO.BankCnpj}");

        Task.WaitAll(sale, bank);

        if (sale.Result == null) return NotFound("Can't find Sale!");
        if (bank.Result == null) return NotFound("Can't find Bank!");

        CarFinancing carFinancing = new()
        {
            Sale = sale.Result,
            Bank = bank.Result,
            FinancingDate = carFinancingDTO.FinancingDate
        };

        carFinancing.Id = _carFinancingService.Post(carFinancing);

        return carFinancing.Id > 0 ? CreatedAtRoute("GetCarFinancing", new { id = carFinancing.Id }, carFinancing) : BadRequest();
    }
}
