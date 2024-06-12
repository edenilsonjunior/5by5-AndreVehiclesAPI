using Models.Financials;
using Models.Sales;
using Repositories.Sales;
using System.Net.Http.Json;

namespace Repositories.Financials;

public class CarFinancingRepository
{
    public CarFinancingRepository() { }

    public List<CarFinancing> Get()
    {
        var list = new List<CarFinancing>();
        foreach (dynamic row in DapperUtilsRepository<dynamic>.GetAll(CarFinancing.GETALL))
        {
            int id = row.Id;

            Sale sale = new SaleRepository().Get(row.SaleId);

            DateTime financingDate = row.FinancingDate;

            string bankCnpj = row.BankCnpj;
            Bank? bank = GetBank(bankCnpj).Result;

            CarFinancing carFinancing = new()
            {
                Id = id,
                Sale = sale,
                FinancingDate = financingDate,
                Bank = bank,
            };

            list.Add(carFinancing);
        }

        return list;
    }

    public CarFinancing Get(int id)
    {

        dynamic row = DapperUtilsRepository<dynamic>.Get(CarFinancing.GET, new { Id = id });

        Sale sale = new SaleRepository().Get(row.SaleId);

        DateTime financingDate = row.FinancingDate;

        string bankCnpj = row.BankCnpj;
        Bank? bank = GetBank(bankCnpj).Result;

        CarFinancing carFinancing = new()
        {
            Id = id,
            Sale = sale,
            FinancingDate = financingDate,
            Bank = bank
        };

        return carFinancing;
    }

    public int Post(CarFinancing carFinancing)
    {
        return DapperUtilsRepository<CarFinancing>.InsertWithScalar(CarFinancing.POST, new
        {
            SaleId = carFinancing.Sale.Id,
            FinancingDate = carFinancing.FinancingDate,
            BankCnpj = carFinancing.Bank.Cnpj
        });
    }

    private async Task<Bank?> GetBank(string cnpj)
    {
        Bank? bank;
        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:7031");

            HttpResponseMessage response = await client.GetAsync($"/GetBankByCnpj/{cnpj}");

            response.EnsureSuccessStatusCode();
            bank = response.Content.ReadFromJsonAsync<Bank>().Result;

            return bank != null ? bank : null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
