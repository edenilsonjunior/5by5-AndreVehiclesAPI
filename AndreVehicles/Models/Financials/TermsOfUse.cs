namespace Models.Financials;

public class TermsOfUse
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int Version { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool Status { get; set; }
}
