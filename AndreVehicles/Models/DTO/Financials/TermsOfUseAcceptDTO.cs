using Models.Financials;
using Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Financials;

public class TermsOfUseAcceptDTO
{
    public int Id { get; set; }
    public string CustomerDocument { get; set; }
    public string TermsOfUseId { get; set; }
    public DateTime AcceptDate { get; set; }
}
