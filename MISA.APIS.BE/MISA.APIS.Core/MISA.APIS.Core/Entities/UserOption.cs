using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Entities
{
    public class UserOption
    {
        #region Property

        /// <summary>
        /// Độ rộng của cột
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Có thể sắp xếp lại vị trí hay không
        /// </summary>
        public Boolean? IsSort { get; set; }

        /// <summary>
        /// Có được hiển thị trong bảng hay không
        /// </summary>
        public Boolean? IsVisible { get; set; }

        /// <summary>
        /// Tên trường thông tin
        /// </summary>
        public String? Caption { get; set; }

        /// <summary>
        /// Trường thông tin
        /// </summary>
        public String? FieldName { get; set; }

        #endregion
    }
}
