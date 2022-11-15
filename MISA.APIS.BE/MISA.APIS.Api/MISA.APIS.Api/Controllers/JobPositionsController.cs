using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.APIS.Core.Entities;
using MISA.APIS.Core.Interfaces.Services;

namespace MISA.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JobPositionsController : MISABaseController<JobPosition>
    {
        #region Constructor
        IJobPositionService jobPositionService;
        public JobPositionsController(IJobPositionService _jobPositionService):base(_jobPositionService)
        {
            jobPositionService = _jobPositionService;
        }
        #endregion
    }
}
