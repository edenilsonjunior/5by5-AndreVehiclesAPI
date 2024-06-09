using System.ComponentModel.DataAnnotations;

namespace Models.Sales;


public class Card
{
    public readonly static string POST = "INSERT INTO Card(CardNumber, SecurityCode, ExpirationDate, CardHolderName) VALUES(@CardNumber, @SecurityCode, @ExpirationDate, @CardHolderName)";
    public readonly static string GET = "SELECT CardNumber, SecurityCode, ExpirationDate, CardHolderName FROM Card WHERE CardNumber = @CardNumber";

    [Key]
    public string CardNumber { get; set; }
    public string SecurityCode { get; set; }
    public string ExpirationDate { get; set; }
    public string CardHolderName { get; set; }

    public Card() { }

    public Card(string cardNumber, string securityCode, string expirationDate, string cardHolderName)
    {
        CardNumber = cardNumber;
        SecurityCode = securityCode;
        ExpirationDate = expirationDate;
        CardHolderName = cardHolderName;
    }
}
