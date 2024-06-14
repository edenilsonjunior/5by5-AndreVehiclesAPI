using Models.Sales;

namespace Models.DTO.Sales;

public class BankSlipPaymentDTO
{
    public DateTime PaymentDate { get; set; }
    public BankSlip BankSlip { get; set; }

}
