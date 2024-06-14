using Models.DTO.Financials;
using Models.People;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Financials;

public class FinancialPending
{
    public readonly static string POST = @"INSERT INTO[DBAndreVehiclesAPI].[dbo].[FinancialPending]
                                                ([Description]
                                                ,[CustomerDocument]
                                                ,[Price]
                                                ,[FinancialPendingDate]
                                                ,[PaymentDate]
                                                ,[Status])
                                            VALUES
                                                (@Description
                                                , @CustomerDocument
                                                , @Price
                                                , @FinancialPendingDate
                                                , @PaymentDate
                                                , @Status)";

    public int Id { get; set; }
    public string Description { get; set; }
    public Customer Customer { get; set; }
    public decimal Price { get; set; }
    public DateTime FinancialPendingDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool Status { get; set; }
}
