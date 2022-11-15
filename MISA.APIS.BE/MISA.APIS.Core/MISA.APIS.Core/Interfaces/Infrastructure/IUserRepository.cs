using MISA.APIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Interfaces.Infrastructure
{
    public interface IUserRepository: IBaseRepository<User>
    {
        #region Interface

        /// <summary>
        /// Thực hiện lấy dữ liệu người dùng theo phân trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi/trang</param>
        /// <param name="pageNumber">Vị trí trang hiển thị</param>
        /// <param name="searchKey">Từ khóa tìm kiếm</param>
        /// <returns>Object chứa đầy đủ thông tin trả về</returns>
        /// Author: VQBao - 6/9/2022
        Object GetUserPaging(int pageSize, int pageNumber, string? searchByText, string? searchByGroup);
        /// <summary>
        /// Lấy người dùng theo id
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>Object chứ thông tin người dùng, danh sách nhóm của người dùng đó</returns>
        /// Author: VQBao - 6/9/2022
        Object GetById(Guid id);
        /// <summary>
        /// Xóa 1 người dùng
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>1-Xóa thành công, 0-Xóa thất bại</returns>
        /// Author: VQBao - 10/8/2022
        int DeleteUser(Guid id);

        /// <summary>
        /// Sửa nhóm của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <param name="groupIds">Mảng id của nhóm thuộc người dùng đó</param>
        /// <returns>1-update thành công</returns>
        /// Author: VQBao - 12/9/2022
        int UpdateGroupOfUser(Guid userId, List<int> groupIds);

        /// <summary>
        /// Xuất khẩu dữ liệu người dùng ra file excel
        /// </summary>
        /// <returns>1 luồng chứa file excel danh sách người dùng</returns>
        /// Author: VQBao - 12/9/2022
        IEnumerable<User> GetUserGrid(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userOption"></param>
        /// <returns></returns>
        int UpdateUserGrid(Guid userId, List<UserOption> userOption);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        int InsertUser(List<User> users);

        #endregion
    }
}
