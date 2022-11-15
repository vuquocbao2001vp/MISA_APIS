using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Enum
{
    public class MISAEnum
    {
        /// <summary>
        /// Trạng thái hoạt động của người dùng
        /// </summary>
        public enum UserStatus
        {
            /// <summary>
            /// Đang hoạt động
            /// </summary>
            Active = 1,

            /// <summary>
            /// Chưa kích hoạt
            /// </summary>
            Inactive = 0,
        }

        /// <summary>
        /// Trạng thái sử dụng của người dùng
        /// </summary>
        public enum UserGroupStatus
        {
            /// <summary>
            /// Đang sử dụng
            /// </summary>
            Active = 1,

            /// <summary>
            /// Không sử dụng
            /// </summary>
            Inactive = 0
        }
    }
}
