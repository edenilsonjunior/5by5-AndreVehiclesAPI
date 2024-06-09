namespace Models.Sales;


public class BankSlip
{
    public readonly static string POST = "INSERT INTO BankSlip(Number, DueDate) VALUES(@Number, @DueDate); SELECT CAST (SCOPE_IDENTITY() AS INT);";
    public readonly static string GET = "SELECT Id, Number, DueDate FROM BankSlip WHERE Id = @Id";

    public int Id { get; set; }
    public int Number { get; set; }
    public DateTime DueDate { get; set; }

    public BankSlip() { }

    public BankSlip(int id, int number, DateTime dueDate)
    {
        Id = id;
        Number = number;
        DueDate = dueDate;
    }
}
