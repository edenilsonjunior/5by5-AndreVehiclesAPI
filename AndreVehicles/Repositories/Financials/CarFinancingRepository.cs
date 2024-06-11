using Models.Financials;
using Models.Sales;
using Repositories.Sales;


namespace Repositories.Financials;

public class CarFinancingRepository
{
    public CarFinancingRepository()
    {

    }

    public List<CarFinancing> Get()
    {

        var list = new List<CarFinancing>();
        foreach (dynamic row in DapperUtilsRepository<dynamic>.GetAll(CarFinancing.GETALL))
        {
            int id = row.Id;

            Sale sale = new SaleRepository().Get(row.SaleId);

            DateTime financingDate = row.FinancingDate;


            CarFinancing carFinancing = new()
            {
                Id = id,
                Sale = sale,
                FinancingDate = financingDate,
                // TODO: Bank from mongo

            };

            list.Add(carFinancing);
        }

        return list;
    }

    public CarFinancing Get(int id)
    {

        dynamic row = DapperUtilsRepository<dynamic>.Get(CarFinancing.GET, new { Id = id});

        Sale sale = new SaleRepository().Get(row.SaleId);

        DateTime financingDate = row.FinancingDate;


        CarFinancing carFinancing = new()
        {
            Id = id,
            Sale = sale,
            FinancingDate = financingDate,
            // TODO: Bank from mongo
        };

        return carFinancing;
    }

    public void Post(CarFinancing carFinancing)
    {
        // TODO: Post method

    }

}
