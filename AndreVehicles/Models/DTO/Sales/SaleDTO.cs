namespace Models.DTO.Sales;

public class SaleDTO
{
    public string CustomerDocument { get; set; }
    public string EmployeeDocument { get; set; }
    public string CarPlate { get; set; }
    public int PaymentId { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal SalePrice { get; set; }
}
