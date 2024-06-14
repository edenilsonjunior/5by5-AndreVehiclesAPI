using Models.DTO.People;
using Models.Insurances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Insurance
{
    public class DriverDTO
    {
        public string Document { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public AddressDTO Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DriversLicense DriversLicense { get; set; }
    }
}
