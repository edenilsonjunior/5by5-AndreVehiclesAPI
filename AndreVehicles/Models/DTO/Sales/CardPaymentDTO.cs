namespace Models.DTO.Sales;

public class CardPaymentDTO
{
    public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public string CardNumber { get; set; }
    public string SecurityCode { get; set; }
    public string ExpirationDate { get; set; }
    public string CardHolderName { get; set; }
}
