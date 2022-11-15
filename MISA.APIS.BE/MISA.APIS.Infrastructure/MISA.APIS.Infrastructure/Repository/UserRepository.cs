using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using MISA.APIS.Core.Interfaces.Infrastructure;
using MISA.APIS.Core.Entities;
using System.Text.Json;
using Newtonsoft.Json;


namespace MISA.APIS.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User> ,IUserRepository
    {
        

        #region Methods

        /// <summary>
        /// Thực hiện lấy dữ liệu người dùng theo phân trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi/trang</param>
        /// <param name="pageNumber">Vị trí trang hiển thị</param>
        /// <param name="searchKey">Từ khóa tìm kiếm</param>
        /// <returns>Object chứa đầy đủ thông tin trả về</returns>
        /// Author: VQBao - 6/9/2022
        public Object GetUserPaging(int pageSize, int pageNumber, string? searchByText, string? searchByGroup)
        {
            try
            {
                var storeProc = "Proc_GetUserPaging";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_PageSize", pageSize);
                parameters.Add("@v_PageNumber", pageNumber);
                parameters.Add("@v_SearchByText", searchByText);
                parameters.Add("@v_SearchByGroup", searchByGroup);
                parameters.Add("@v_TotalRecords", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_TotalPages", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_RecordStart", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_RecordEnd", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                var data = connection.Query<User>(storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                int totalRecords = parameters.Get<int>("@v_TotalRecords");
                int totalPages = parameters.Get<int>("@v_TotalPages");
                int recordStart = parameters.Get<int>("@v_RecordStart");
                int recordEnd = parameters.Get<int>("@v_RecordEnd");

                var respond = new
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    RecordStart = recordStart,
                    RecordEnd = recordEnd,
                    Data = data,
                };
                return respond;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Lấy người dùng theo id
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>Object chứ thông tin người dùng, danh sách nhóm của người dùng đó</returns>
        /// Author: VQBao - 6/9/2022
        public Object GetById(Guid id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                var procGetUser = "Proc_GetUserById";
                var procGetGroupOfUser = "Proc_GetAllGroupOfUser";
                parameters.Add("@v_UserID", id);
                var userData = connection.QueryFirstOrDefault<User>(procGetUser, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                var groupData = connection.Query<UserGroup>(procGetGroupOfUser, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                var res = new
                {
                    User = userData,
                    UserGroup = groupData,
                };
                return res;
            }
            catch (Exception)
            {
                throw ;
            }
        }

        /// <summary>
        /// Xóa 1 người dùng
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>1-Xóa thành công, 0-Xóa thất bại</returns>
        /// Author: VQBao - 10/8/2022
        public int DeleteUser(Guid id)
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var storeProc = $"Proc_DeleteUser";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@v_UserID", id);
                    var data = connection.Execute(storeProc, param: parameters, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
                    transaction.Commit();
                    return data;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Sửa nhóm của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <param name="groupIds">Mảng id của nhóm thuộc người dùng đó</param>
        /// Author: VQBao - 12/9/2022
        public int UpdateGroupOfUser(Guid userId, List<int>? groupIds)
        {
            var jsonGroupIDs = JsonConvert.SerializeObject(groupIds);
            try
            {
                var storePro = $"Proc_InsertMultiGroupToUser";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserID", userId);
                parameters.Add("v_JsonUserGroupIDs", jsonGroupIDs);
                var data = connection.Execute(storePro, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Lấy tùy chỉnh cột của người dùng
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>Người dùng</returns>
        /// Author: VQBao - 15/9/2022
        public IEnumerable<User> GetUserGrid(Guid id)
        {
            try
            {
                var storeProc = $"Proc_GetUserGrid";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserID", id);
                var data = connection.Query<User>(storeProc, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Thay đổi tùy chỉnh cột của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <param name="userOption">Tùy chỉnh cột mới</param>
        /// <returns>1-Thành công, 0-Lỗi</returns>
        /// Author: VQBao - 15/9/2022
        public int UpdateUserGrid(Guid userId, List<UserOption> userOption)
        {
            try
            {
                var storeProc = $"Proc_UpdateUserGrid";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserID", userId);
                parameters.Add("v_UserOption", userOption);
                var data = connection.Execute(storeProc, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Thêm người dùng vào database
        /// </summary>
        /// <param name="users">Người dùng</param>
        /// <returns>Số bản ghi đã thêm thành công</returns>
        /// Author: VQBao - 20/9/2022
        public int InsertUser(List<User> users)
        {
            var jsonUsers = JsonConvert.SerializeObject(users);
            try
            {
                var storeInsertUser = $"Proc_InsertUser";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_JsonTable", jsonUsers);
                var data = connection.Execute(storeInsertUser, parameters, commandType: System.Data.CommandType.StoredProcedure);

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
