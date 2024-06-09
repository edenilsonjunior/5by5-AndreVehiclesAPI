using Models.Sales;
using Repositories.Sales;

namespace Services.Sales;

public class SaleService
{
    private SaleRepository _saleRepository;


    public SaleService()
    {
        _saleRepository = new SaleRepository();
    }


    public List<Sale> Get(string technology)
    {
        return _saleRepository.Get(technology);
    }


    public Sale Get(string technology, int id)
    {
        return _saleRepository.Get(technology, id);
    }


    public bool Post(string technology, Sale sale)
    {
        return _saleRepository.Post(technology, sale);
    }

}
