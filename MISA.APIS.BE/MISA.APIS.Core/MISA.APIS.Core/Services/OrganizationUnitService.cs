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
    public class OrganizationUnitService: BaseService<OrganizationUnit>, IOrganizationUnitService
    {
        IOrganizationUnitRepository organizationUnitRepository;
        public OrganizationUnitService(IOrganizationUnitRepository _organizationUnitRepository) :base(_organizationUnitRepository)
        {
            organizationUnitRepository = _organizationUnitRepository;
        }
    }
}
