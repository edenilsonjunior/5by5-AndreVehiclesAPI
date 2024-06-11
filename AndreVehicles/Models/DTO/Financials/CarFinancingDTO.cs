using Models.Financials;
using Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Financials;

public class CarFinancingDTO
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public DateTime FinancingDate { get; set; }
    public string BankCnpj { get; set; }
}
