namespace Models.DTO.People;

public class CustomerDTO
{
    public string Document { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public int AddressId { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public decimal Income { get; set; }
}
