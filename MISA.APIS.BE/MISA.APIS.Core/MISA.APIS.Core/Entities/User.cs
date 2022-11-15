using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.APIS.Core.Enum;

namespace MISA.APIS.Core.Entities
{
    public class User: BaseEntity
    {
        #region Property

        /// <summary>
        /// Id người dùng
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Điện thoại cá nhân
        /// </summary>
        public string? Mobile { get; set; }

        /// <summary>
        /// Điện thoại cơ quan
        /// </summary>
        public string? WorkPhone { get; set; }

        /// <summary>
        /// Id chức vụ
        /// </summary>
        public int? JobPositionID { get; set; }

        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public string? JobPositionName { get; set; }

        /// <summary>
        /// Id phòng ban
        /// </summary>
        public int? OrganizationUnitID { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? OrganizationUnitName { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string? OrganizationName { get; set; }
        
        /// <summary>
        /// Tên nhóm người dùng
        /// </summary>
        public string? UserGroupName { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public MISAEnum.UserStatus? Status { get; set; }

        /// <summary>
        /// Tùy chỉnh cột
        /// </summary>
        public List<UserOption> UserOption { get; set; } = new List<UserOption>();

        /// <summary>
        /// Tùy chỉnh cột mặc định
        /// </summary>
        public List<UserOption> OptionDefault { get; set; } = new List<UserOption>();

        /// <summary>
        /// Kiểm tra tính hợp lệ để nhập khẩu
        /// </summary>
        public bool IsValidToImport { get; set; } = true;

        /// <summary>
        /// Danh sách lỗi khi validate thông tin người dùng
        /// </summary>
        public List<string> ListErrorImport { get; set; } = new List<string>();
        #endregion
    }
}
