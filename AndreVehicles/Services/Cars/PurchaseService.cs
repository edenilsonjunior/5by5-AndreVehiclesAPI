using Models.Cars;
using Repositories.Cars;

namespace Services.Cars;

public class PurchaseService
{
    private PurchaseRepository _purchaseRepository;


    public PurchaseService()
    {
        _purchaseRepository = new PurchaseRepository();
    }


    public List<Purchase> Get(string technology)
    {
        return _purchaseRepository.Get(technology);
    }


    public Purchase Get(string technology, int id)
    {
        return _purchaseRepository.Get(technology, id);
    }


    public bool Post(string technology,  Purchase purchase)
    {
        return _purchaseRepository.Post(technology, purchase);
    }


}
