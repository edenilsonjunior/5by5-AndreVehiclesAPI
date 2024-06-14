using Models.DTO.Insurance;
using Models.Insurances;
using Repositories;
using Repositories.Insurances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Insurances
{
    public class InsuranceService
    {
        private InsuranceRepository _insuranceRepository;

        public List<Insurance> Get()
        {
            return _insuranceRepository.Get();
        }

        public bool Post(Insurance insurance)
        {
            return _insuranceRepository.Post(insurance);
        }
    }
}
