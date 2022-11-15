using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.APIS.Core.Entities;
using MISA.APIS.Core.Interfaces.Services;
using MISA.APIS.Core;

namespace MISA.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : MISABaseController<User>
    {
        #region Constructor

        IUserService service;
        public UsersController(IUserService _service):base(_service)
        {
            service = _service;
        }

        #endregion

        #region Controller

        /// <summary>
        /// Api lấy dữ liệu người dùng phân trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi/trang</param>
        /// <param name="pageNumber">Số thứ tự trang hiển thị</param>
        /// <param name="searchKey">Từ khóa tìm kiếm</param>
        /// <returns>Object chứa các thông tin trả về</returns>
        /// Author: VQBao - 6/9/2022
        [HttpGet("Filter")]
        public Object GetUserPaging(int pageSize, int pageNumber, string? searchByText, string? searchByGroup)
        {
            try
            {
                var data = service.GetUserPaging(pageSize, pageNumber, searchByText, searchByGroup);
                return Ok(data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Lấy người dùng theo id
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>Object chứ thông tin người dùng, danh sách nhóm của người dùng đó</returns>
        /// Author: VQBao - 6/9/2022
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var data = service.GetById(id);
                return Ok(data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Xóa 1 người dùng
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>Mã 201-Xóa thành công</returns>
        /// Author: VQBao - 10/8/2022
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            try
            {
                var data = service.DeleteUser(id);
                return StatusCode(201, data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Chỉnh sửa nhóm của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <param name="groupIds">Chuỗi id của nhóm thuộc người dùng đó</param>
        /// <returns>Mã 201: thành công</returns>
        /// Author: VQBao - 12/9/2022
        [HttpPut("UpdateGroupOfUser")]
        public IActionResult UpdateGroupOfUser(Guid userId, List<int> groupIds)
        {
            try
            {
                var data = service.UpdateGroupOfUser(userId, groupIds);
                return StatusCode(201, data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Xuất khẩu dữ liệu người dùng ra file excel
        /// </summary>
        /// <returns>File download</returns>
        /// Author: VQBao - 12/9/2022
        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> GetExcelFile()
        {   
            
            try
            {
                var stream = service.ExportToExcel();
                string excelName = "Danh_sach_nguoi_dung.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Lấy dữ liệu tùy chỉnh cột của người dùng theo id
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>Mã 200: thành công</returns>
        /// Author: VQBao - 15/9/2022
        [HttpGet("GetUserGrid/{id}")]
        public IActionResult GetUserGrid(Guid id)
        {

            try
            {
                var data = service.GetUserGrid(id);
                return Ok(data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Lưu tùy chỉnh cột của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <param name="userOption"></param>
        /// <returns>Mã 201</returns>
        /// Author: VQBao - 15/9/2022
        [HttpPut("SaveUserOption/{userId}")]
        public IActionResult SaveUserOption(Guid userId, List<UserOption> userOption)
        {

            try
            {
                var data = service.UpdateUserGrid(userId, userOption);
                return StatusCode(201, data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Nhập khẩu vào database những người dùng hợp lệ
        /// </summary>
        /// <param name="validUsers">Mảng người dùng</param>
        /// <returns>Mã 201</returns>
        /// Author: VQBao - 20/9/2022
        [HttpPost("Import")]
        public IActionResult ImportUser(List<User> validUsers)
        {

            try
            {
                var data = service.ImportValidUsers(validUsers);
                return StatusCode(201, data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Lấy số lượng bản ghi hợp lệ, không hợp lệ, danh sách người dùng hợp lệ, không hợp lệ từ file excel
        /// </summary>
        /// <param name="fileImport">File excel import</param>
        /// <returns>Object chứa các thông tin số lượng bản ghi hợp lệ, không hợp lệ, danh sách người dùng hợp lệ, không hợp</returns>
        /// Author: VQBao - 20/9/2022
        [HttpPost("GetImportQuantity")]
        public IActionResult GetImportQuantity(IFormFile fileImport)
        {

            try
            {
                var data = service.GetImportQuantity(fileImport);
                return StatusCode(201, data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Xuất khẩu ra những người dùng không hợp lệ kèm theo lỗi
        /// </summary>
        /// <param name="users">Danh sách người dùng không hợp lệ</param>
        /// <returns>File excel chứa danh sách người dùng</returns>
        /// Author: VQBao - 20/9/2022
        [HttpPost("ExportInvalidUser")]
        public async Task<IActionResult> ExportInvalidUser(List<User> users)
        {
            try
            {
                var stream = service.ExportInvalidUser(users);
                string excelName = "Danh_sach_nguoi_dung_loi.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Xuất khẩu ra file excel mẫu để nhập liệu
        /// </summary>
        /// <returns>File excel mẫu</returns>
        /// Author: VQBao - 20/9/2022
        [HttpGet("ExportSampleExcel")]
        public async Task<IActionResult> ExportSampleExcel()
        {
            try
            {
                var stream = service.ExportSampleExcel();
                string excelName = "Danh_sach_mau.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
        #endregion
    }
}
