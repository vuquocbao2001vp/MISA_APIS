using MISA.APIS.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Services
{
    public class BaseService<Entity>
    {
        #region Constructor
        IBaseRepository<Entity> repository;
        public BaseService(IBaseRepository<Entity> _repository)
        {
            repository = _repository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả dữ liệu trong bảng
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi</returns>
        /// Author: VQBao - 6/9/2022
        public IEnumerable<Entity> GetAll()
        {
            var data = repository.GetAll();
            return data;
        }
        #endregion
    }
}
