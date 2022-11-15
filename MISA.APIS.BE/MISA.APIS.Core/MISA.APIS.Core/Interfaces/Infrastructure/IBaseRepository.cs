using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Interfaces.Infrastructure
{
    public interface IBaseRepository<Entity>
    {
        #region Interface
        /// <summary>
        /// Lấy tất cả dữ liệu trong bảng
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi</returns>
        /// Author: VQBao - 6/9/2022
        IEnumerable<Entity> GetAll();
        #endregion
    }
}
