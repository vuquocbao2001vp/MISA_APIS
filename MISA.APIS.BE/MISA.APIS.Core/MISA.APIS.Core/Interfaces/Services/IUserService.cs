using Microsoft.AspNetCore.Http;
using MISA.APIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Interfaces.Services
{
    public interface IUserService: IBaseService<User>
    {
        #region Service

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
        MemoryStream ExportToExcel();

        /// <summary>
        /// Lấy tùy chỉnh cột của người dùng
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>Người dùng</returns>
        /// Author: VQBao - 15/9/2022
        IEnumerable<User> GetUserGrid(Guid id);

        /// <summary>
        /// Thay đổi tùy chỉnh cột của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <param name="userOption">Tùy chỉnh cột mới</param>
        /// <returns>1-Thành công, 0-Lỗi</returns>
        /// Author: VQBao - 15/9/2022
        int UpdateUserGrid(Guid userId, List<UserOption> userOption);

        /// <summary>
        /// Lấy dữ liệu người dùng từ file excel import
        /// </summary>
        /// <param name="formFile">File import</param>
        /// <returns>Mảng danh sách người dùng</returns>
        /// Author: VQBao - 20/9/2022
        List<User> GetUserFromImportFile(IFormFile formFile);

        /// <summary>
        /// Lấy số lượng bản ghi hợp lệ, không hợp lệ, danh sách người dùng hợp lệ, không hợp lệ từ file excel
        /// </summary>
        /// <param name="fileImport">File excel import</param>
        /// <returns>Object chứa các thông tin số lượng bản ghi hợp lệ, không hợp lệ, danh sách người dùng hợp lệ, không hợp</returns>
        /// Author: VQBao - 20/9/2022
        Object GetImportQuantity(IFormFile formFile);

        /// <summary>
        /// Nhập khẩu những người dùng hợp lệ
        /// </summary>
        /// <param name="validUsers">Danh sách người dùng hợp lệ</param>
        /// <returns>Số người dùng đã nhập khẩu</returns>
        /// Author: VQBao - 20/9/2022
        int ImportValidUsers(List<User> validUsers);

        /// <summary>
        /// Xuất khẩu danh sách người dùng không hợp lệ ra file excel
        /// </summary>
        /// <param name="users">Danh sách người dùng không hợp lệ</param>
        /// Author: VQBao - 20/9/2022
        MemoryStream ExportInvalidUser(List<User> users);

        /// <summary>
        /// Xuất khẩu file excel nhập liệu mẫu
        /// </summary>
        /// Author: VQBao - 20/9/2022
        MemoryStream ExportSampleExcel();

        #endregion
    }
}
