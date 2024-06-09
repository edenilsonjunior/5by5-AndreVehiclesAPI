using Models.Sales;
using Repositories.Sales;

namespace Services.Sales;

public class PaymentService
{
    private PaymentRepository _paymentRepository;


    public PaymentService()
    {
        _paymentRepository = new PaymentRepository();
    }


    public List<Payment> Get(string technology)
    {
        return _paymentRepository.Get(technology);
    }


    public Payment Get(string technology, int id)
    {
        return _paymentRepository.Get(technology, id);
    }


    public bool Post(string technology, Payment payment)
    {
        return _paymentRepository.Post(technology, payment);
    }


}
