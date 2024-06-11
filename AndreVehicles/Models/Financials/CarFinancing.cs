using Models.Sales;

namespace Models.Financials;

public class CarFinancing
{
    public int Id { get; set; }
    public Sale Sale { get; set; }
    public DateTime FinancingDate { get; set; }
    public Bank Bank { get; set; }
}
