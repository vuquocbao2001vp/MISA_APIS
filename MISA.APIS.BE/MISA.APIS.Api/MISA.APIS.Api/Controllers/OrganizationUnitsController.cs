using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.APIS.Core.Entities;
using MISA.APIS.Core.Interfaces.Services;

namespace MISA.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrganizationUnitsController : MISABaseController<OrganizationUnit>
    {
        IOrganizationUnitService organizationUnitService;
        public OrganizationUnitsController(IOrganizationUnitService _organizationUnitService):base(_organizationUnitService)
        {
            organizationUnitService = _organizationUnitService;
        }
    }
}
