using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Cars;
using Models.DTO.Financials;
using Models.Financials;
using Models.Sales;
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
        Sale? sale = GetSaleById(carFinancingDTO.SaleId).Result;

        Bank? bank = GetBankByCnpj(carFinancingDTO.BankCnpj).Result;

        if (sale == null)
            return NotFound("Can't find Sale!");

        if (bank == null)
            return NotFound("Can't find Bank!");


        CarFinancing carFinancing = new()
        {
            Sale = sale,
            Bank = bank,
            FinancingDate = carFinancingDTO.FinancingDate
        };

        carFinancing.Id = _carFinancingService.Post(carFinancing);

        return carFinancing.Id > 0 ? CreatedAtRoute("GetCarFinancing", new { id = carFinancing.Id }, carFinancing) : BadRequest();
    }


    private async Task<Sale?> GetSaleById(int id)
    {
        Sale? sale;
        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:7237");

            HttpResponseMessage response = await client.GetAsync($"/GetSaleById/dapper/{id}");

            response.EnsureSuccessStatusCode();
            sale = response.Content.ReadFromJsonAsync<Sale>().Result;

            return sale;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task<Bank?> GetBankByCnpj(string cnpj)
    {
        Bank? bank;
        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:7031");

            HttpResponseMessage response = await client.GetAsync($"/GetBankByCnpj/{cnpj}");

            response.EnsureSuccessStatusCode();
            bank = response.Content.ReadFromJsonAsync<Bank>().Result;

            return bank;
        }
        catch (Exception)
        {
            return null;
        }
    }

}
