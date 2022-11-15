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
    public class UserGroupService: BaseService<UserGroup>, IUserGroupService
    {
        #region Constructor
        IUserGroupRepository userGroupRepository;
        public UserGroupService(IUserGroupRepository _userGroupRepository):base(_userGroupRepository)
        {
            userGroupRepository = _userGroupRepository;
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
        public Object GetUserGroupPaging(int pageSize, int pageNumber, string? searchKey)
        {
            var data = userGroupRepository.GetUserGroupPaging(pageSize, pageNumber, searchKey);
            return data;
        }
        /// <summary>
        /// Lấy nhóm người dùng theo id
        /// </summary>
        /// <param name="id">Id của nhóm người dùng</param>
        /// <returns>Object chứ thông tin nhóm, danh sách thành viên của nhóm</returns>
        /// Author: VQBao - 6/9/2022
        public UserGroup GetById(int id)
        {
            return userGroupRepository.GetById(id);
        }
        /// <summary>
        /// Phân trang thành viên thuộc nhóm người dùng
        /// </summary>
        /// <param name="userGroupId">Id của nhóm người dùng</param>
        /// <param name="searchKey">Từ khóa tìm kiếm theo tên avf email</param>
        /// <returns>Danh sách người dùng thỏa mãn</returns>
        /// Author: VQBao - 8/9/2022
        public Object GetUserOfGroupPaging(int pageSize, int pageNumber, int? userGroupId, string? searchByText, string? searchByPosition, string? MISACode)
        {
            return userGroupRepository.GetUserOfGroupPaging(pageSize, pageNumber, userGroupId, searchByText, searchByPosition, MISACode);
        }

        /// <summary>
        /// Xóa hàng loạt người dùng khỏi 1 nhóm người dùng
        /// </summary>
        /// <param name="groupId">Id của nhóm</param>
        /// <param name="userIds">Mảng id người dùng</param>
        /// <returns>1-Xóa thành công</returns>
        /// Author: VQBao - 12/9/2022
        public int DeleteMultiUserOfGroup(int groupId, List<Guid> userIds)
        {
            return userGroupRepository.DeleteMultiUserOfGroup(groupId, userIds);
        }

        /// <summary>
        /// Sửa thành viên trong nhóm người dùng
        /// </summary>
        /// <param name="groupId">Id của nhóm người dùng</param>
        /// <param name="userIds">Mảng id của người dùng</param>
        /// Author: VQBao - 12/9/2022
        public int UpdateMemberOfGroup(int groupId, List<Guid>? userIds)
        {
            return userGroupRepository.UpdateMemberOfGroup(groupId, userIds);
        }
        #endregion
    }
}
