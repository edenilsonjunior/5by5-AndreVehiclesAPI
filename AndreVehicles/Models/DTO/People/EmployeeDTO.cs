using Models.People;

namespace Models.DTO.People;

public class EmployeeDTO
{
    public string Document { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public AddressDTO Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public decimal CommissionValue { get; set; }
    public decimal Commission { get; set; }
}
