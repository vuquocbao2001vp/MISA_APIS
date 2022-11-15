using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Entities
{
    public class JobPosition: BaseEntity
    {
        #region Property
        /// <summary>
        /// Id chức vụ
        /// </summary>
        public int JobPositionID { get; set; }

        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public string? JobPositionName { get; set; }
        #endregion
    }
}
