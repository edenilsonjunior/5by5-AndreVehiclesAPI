using Models.Sales;

namespace Models.Financials;

public class CarFinancing
{
    public static readonly string POST = "INSERT INTO CarFinancing (SaleId, FinancingDate, BankCnpj) VALUES (@SaleId, @FinancingDate, @BankCnpj); CAST (SCOPE_IDENTITY() as int)";
    public static readonly string GETALL = "SELECT Id, SaleId, FinancingDate, BankCnpj FROM CarFinancing";
    public static readonly string GET = GETALL + " WHERE Id = @Id";

    public int Id { get; set; }
    public Sale Sale { get; set; }
    public DateTime FinancingDate { get; set; }
    public Bank Bank { get; set; }
}
