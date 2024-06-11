using Models.People;

namespace Models.Financials;

public class TermsOfUseAccept
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
    public TermsOfUse TermsOfUse { get; set; }
    public DateTime AcceptDate { get; set; }
}
