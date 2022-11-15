using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;

namespace MISA.APIS.Infrastructure.Repository
{
    public class BaseRepository<Entity>: IDisposable
    {
        #region Constructor

        protected readonly string connectionString = "";
        protected MySqlConnection connection;
        public BaseRepository()
        {
            // khởi tạo kết nối với dataabse
            connectionString =
                "Host=localhost; " +
                "Port=3306; " +
                "Database=misa.apis; " +
                "User=root; " +
                "Password=vuquocbao2001vp;";
            connection = new MySqlConnection(connectionString);
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        // Đóng kết nối
        public void Dispose()
        {
            connection.Dispose();
            connection.Close();
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
            try
            {
                var entity = typeof(Entity).Name;
                var storeProc = $"Proc_GetAll{entity}";
                var data = connection.Query<Entity>(storeProc, commandType: System.Data.CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
