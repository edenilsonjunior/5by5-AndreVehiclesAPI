namespace Models.DTO.Sales;

public class PixPaymentDTO
{
    public int Id { get; set; }
    public string PixKey { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PixTypeName { get; set; }
}