using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.APIS.Core.Enum.MISAEnum;

namespace MISA.APIS.Core.Entities
{
    public class OrganizationUnit: BaseEntity
    {
        #region Property
        /// <summary>
        /// Id phòng ban
        /// </summary>
        public int OrganizationUnitID { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? OrganizationUnitName { get; set; }

        /// <summary>
        /// Mã trong cây đơn vị
        /// </summary>
        public string? MISACode { get; set; }

        /// <summary>
        /// Id cha
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// Có chứa phần tử con hay không
        /// </summary>
        public bool? IsParent { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public UserGroupStatus? Inactive { get; set; }
        #endregion
    }
}
