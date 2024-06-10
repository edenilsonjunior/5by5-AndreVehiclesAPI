using Models.People;

namespace Models.DTO.People;

public class AddressDTO
{
    public string PostalCode { get; set; }
    public string AdditionalInfo { get; set; }
    public int Number { get; set; }
    public string StreetType { get; set; }


}
