using Models.Sales;

namespace Models.DTO.Sales;

public class PixPaymentDTO
{
    public DateTime PaymentDate { get; set; }
    public Pix Pix { get; set; }
}