using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.APIS.Core;
using MISA.APIS.Core.Interfaces.Services;

namespace MISA.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MISABaseController<Entity> : ControllerBase
    {
        #region Constructor
        IBaseService<Entity> service;
        public MISABaseController(IBaseService<Entity> _service)
        {
            service = _service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Lấy tất cả dữ liệu trong bảng
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi</returns>
        /// Author: VQBao - 6/9/2022
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var data = service.GetAll();
                return Ok(data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// xử lý exception
        /// </summary>
        /// <returns>Mã lỗi, Thông tin lỗi</returns>
        /// Author: VQBao - 5/9/2022
        protected IActionResult HandleException(Exception e)
        {
            var message = new
            {
                userMsg = "Có lỗi xảy ra, vui lòng liên hệ MISA để được hỗ trợ",
                errorMsg = e.Data["validateError"]
            };
            if (e is MISAException)
            {
                return BadRequest(message);
            }
            return StatusCode(500, message);
        }
        #endregion
    }

}
