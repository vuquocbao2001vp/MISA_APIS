using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.APIS.Core.Entities;
using MISA.APIS.Core.Interfaces.Services;

namespace MISA.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserGroupsController : MISABaseController<UserGroup>
    {
        #region Constructor
        IUserGroupService userGroupService;
        public UserGroupsController(IUserGroupService _userGroupService):base(_userGroupService)
        {
            userGroupService = _userGroupService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy dữ liệu nhóm nguời dùng phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNumber">Số thứ tự trang hiển thị</param>
        /// <param name="searchKey">Từ khóa tìm kiếm</param>
        /// <returns>Object chứ thông tin nhóm người dùng</returns>
        /// Author: VQBao - 6/9/2022
        [HttpGet("Filter")]
        public Object GetUserGroupPaging(int pageSize, int pageNumber, string? searchKey)
        {
            try
            {
                var data = userGroupService.GetUserGroupPaging(pageSize, pageNumber, searchKey);
                return Ok(data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Lấy nhóm người dùng theo id
        /// </summary>
        /// <param name="id">Id của nhóm người dùng</param>
        /// <returns>Object chứ thông tin nhóm, danh sách thành viên của nhóm</returns>
        /// Author: VQBao - 6/9/2022
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var data = userGroupService.GetById(id);
                return Ok(data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Phân trang thành viên thuộc nhóm người dùng
        /// </summary>
        /// <param name="id">Id của nhóm người dùng</param>
        /// <param name="searchKey">Từ khóa tìm kiếm theo tên avf email</param>
        /// <returns>Danh sách người dùng thỏa mãn</returns>
        /// Author: VQBao - 8/9/2022
        [HttpGet("MemberOfGroupPaging")]
        public IActionResult GetMemberOfGroupPaging(int pageSize, int pageNumber, int? userGroupId, string? searchByText, string? searchByPosition, string? MISACode)
        {
            try
            {
                var data = userGroupService.GetUserOfGroupPaging(pageSize, pageNumber, userGroupId, searchByText, searchByPosition, MISACode);
                return Ok(data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Xóa hàng loạt người dùng khỏi 1 nhóm người dùng
        /// </summary>
        /// <param name="groupId">Id của nhóm</param>
        /// <param name="userIds">Mảng id người dùng</param>
        /// <returns>Mã 201-Xóa thành công</returns>
        /// Author: VQBao - 12/9/2022
        [HttpDelete("DeleteMultiUserOfGroup")]
        public IActionResult DeleteMultiUserOfGroup(int groupId, List<Guid> userIds)
        {
            try
            {
                var data = userGroupService.DeleteMultiUserOfGroup(groupId, userIds);
                return StatusCode(201, data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        /// <summary>
        /// Cập nhật thành viên của nhóm người dùng
        /// </summary>
        /// <param name="groupId">Id của nhóm người dùng</param>
        /// <param name="userIds">Mảng id của thành viên nhóm</param>
        /// <returns>Mã 201-Thành công</returns>
        /// Author: VQBao - 20/9/2022
        [HttpPut("UpdateMemberOfGroup/{groupId}")]
        public IActionResult UpdateMemberOfGroup(int groupId, List<Guid> userIds)
        {
            try
            {
                var data = userGroupService.UpdateMemberOfGroup(groupId, userIds);
                return StatusCode(201, data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
        #endregion
    }
}
