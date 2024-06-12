using Models.Financials;
using Repositories.Financials;

namespace Services.Financials;

public class CarFinancingService
{
    private CarFinancingRepository _carFinancingRepository;

    public CarFinancingService()
    {
        _carFinancingRepository = new CarFinancingRepository();
    }

    public List<CarFinancing> Get() => _carFinancingRepository.Get();

    public CarFinancing Get(int id) => _carFinancingRepository.Get(id);

    public int Post(CarFinancing carFinancing) => _carFinancingRepository.Post(carFinancing);

}
