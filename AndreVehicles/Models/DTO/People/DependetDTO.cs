using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.People;

public class DependetDTO
{
    public string Document { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public AddressDTO Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string CustomerDocument { get; set; }
}
