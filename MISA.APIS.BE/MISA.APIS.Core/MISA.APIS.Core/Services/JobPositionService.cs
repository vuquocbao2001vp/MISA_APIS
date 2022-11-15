using MISA.APIS.Core.Entities;
using MISA.APIS.Core.Interfaces.Infrastructure;
using MISA.APIS.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Services
{
    public class JobPositionService: BaseService<JobPosition>, IJobPositionService
    {
        IJobPositionRepository jobPositionRepository;
        public JobPositionService(IJobPositionRepository _jobPositionRepository):base(_jobPositionRepository)
        {
            jobPositionRepository = _jobPositionRepository;
        }
    }
}
