using Models.Sales;

namespace Models.DTO.Sales;

public class CardPaymentDTO
{
    public DateTime PaymentDate { get; set; }
    public Card Card { get; set; }
}
