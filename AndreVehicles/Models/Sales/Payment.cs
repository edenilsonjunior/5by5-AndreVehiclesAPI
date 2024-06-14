using System.Net.NetworkInformation;

namespace Models.Sales;


public class Payment
{
    public readonly static string Post = "INSERT INTO Payment(CardNumber, BankSlipId, PixId, PaymentDate) VALUES(@CardNumber, @BankSlipId, @PixId, @PaymentDate);";
    public readonly static string CardInsert = "INSERT INTO Payment(CardNumber, PaymentDate) VALUES(@CardNumber, @PaymentDate);";
    public readonly static string PixInsert = "INSERT INTO Payment(PixId, PaymentDate) VALUES(@PixId, @PaymentDate);";
    public readonly static string BankSlipInsert = "INSERT INTO Payment(BankSlipId, PaymentDate) VALUES(@BankSlipId, @PaymentDate);";


    public readonly static string GETALL = @"
    select  
        py.Id,
        py.PaymentDate,
        c.CardNumber,
        c.SecurityCode,
        c.ExpirationDate,
        c.CardHolderName,
        bs.Id AS BankSlipId,
        bs.Number AS BankSlipNumber,
        bs.DueDate AS BankSlipDueDate,
        p.Id AS PixId,
        p.PixKey,
        pt.Id AS PixTypeId,
        pt.Name AS PixTypeName
    from Payment py
    LEFT JOIN 
        Card c on py.CardNumber = c.CardNumber
    LEFT JOIN 
        BankSlip bs on py.BankSlipId = bs.Id
    LEFT JOIN 
        Pix p on py.PixId = p.Id
    LEFT JOIN 
        PixType pt on p.Type = pt.Id";

    public readonly static string GET = GETALL + " where py.Id = @Id";



    public int Id { get; set; }
    public Card? Card { get; set; }
    public BankSlip? BankSlip { get; set; }
    public Pix? Pix { get; set; }
    public DateTime PaymentDate { get; set; }

    public Payment() { }

    public Payment(int id, Card card, BankSlip bankSlip, Pix pix, DateTime paymentDate)
    {
        Id = id;
        Card = card;
        BankSlip = bankSlip;
        Pix = pix;
        PaymentDate = paymentDate;
    }
}
