using Models.Cars;
using Models.Insurances;
using Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Insurance
{
    public class InsuranceDTO
    {
        public int Id { get; set; }
        public string CustomerDocument { get; set; }
        public string Deductible { get; set; }
        public string CarPlate { get; set; }
        public long MainDriverDocument { get; set; }
    }
}
