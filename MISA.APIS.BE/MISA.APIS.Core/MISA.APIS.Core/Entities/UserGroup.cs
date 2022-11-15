using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.APIS.Core.Enum.MISAEnum;

namespace MISA.APIS.Core.Entities
{
    public class UserGroup: BaseEntity
    {
        #region Property
        /// <summary>
        /// Id nhóm người dùng
        /// </summary>
        public int UserGroupID { get; set; }

        /// <summary>
        /// Tên nhóm người dùng
        /// </summary>
        public string? UserGroupName { get; set; }

        /// <summary>
        /// Số lượng thành viên
        /// </summary>
        public int? TotalMember { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public UserGroupStatus? Status { get; set; }
        #endregion
    }
}
