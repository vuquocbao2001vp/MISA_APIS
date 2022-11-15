using MISA.APIS.Core.Entities;
using MISA.APIS.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Newtonsoft.Json;

namespace MISA.APIS.Infrastructure.Repository
{
    public class UserGroupRepository: BaseRepository<UserGroup>, IUserGroupRepository
    {
        #region Methods
        /// <summary>
        /// Lấy dữ liệu nhóm nguời dùng phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNumber">Số thứ tự trang hiển thị</param>
        /// <param name="searchKey">Từ khóa tìm kiếm</param>
        /// <returns>Object chứ thông tin nhóm người dùng</returns>
        /// Author: VQBao - 6/9/2022
        public Object GetUserGroupPaging(int pageSize, int pageNumber, string? searchKey)
        {
            try
            {
                var storeProc = "Proc_GetUserGroupPaging";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_PageSize", pageSize);
                parameters.Add("@v_PageNumber", pageNumber);
                parameters.Add("@v_SearchKey", searchKey);
                parameters.Add("@v_TotalRecords", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_TotalPages", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_RecordStart", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_RecordEnd", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                var data = connection.Query<UserGroup>(storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
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
            catch (Exception e)
            {
                return e;
            }
        }

        /// <summary>
        /// Lấy nhóm người dùng theo id
        /// </summary>
        /// <param name="id">Id của nhóm người dùng</param>
        /// <returns>Object chứ thông tin nhóm, danh sách thành viên của nhóm</returns>
        /// Author: VQBao - 6/9/2022
        public UserGroup GetById(int id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                var procGetUserGroup = "Proc_GetUserGroupById";
                parameters.Add("v_UserGroupID", id);
                var groupData = connection.QueryFirstOrDefault<UserGroup>(procGetUserGroup, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                return groupData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Phân trang thành viên thuộc nhóm người dùng
        /// </summary>
        /// <param name="userGroupId">Id của nhóm người dùng</param>
        /// <param name="searchKey">Từ khóa tìm kiếm theo tên avf email</param>
        /// <returns>Danh sách người dùng thỏa mãn</returns>
        /// Author: VQBao - 8/9/2022
        public Object GetUserOfGroupPaging(int pageSize, int pageNumber , int? userGroupId, string? searchByText, string? searchByPosition, string? MISACode)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                var procGetUserOfGroup = "Proc_GetAllUserOfGroup";
                parameters.Add("@v_UserGroupID", userGroupId);
                parameters.Add("@v_SearchByText", searchByText);
                parameters.Add("@v_SearchByPosition", searchByPosition);
                parameters.Add("@v_MISACode", MISACode);
                parameters.Add("@v_PageSize", pageSize);
                parameters.Add("@v_PageNumber", pageNumber);
                parameters.Add("@v_TotalRecords", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_TotalPages", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_RecordStart", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parameters.Add("@v_RecordEnd", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                var userData = connection.Query<User>(procGetUserOfGroup, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                int totalRecords = parameters.Get<int>("@v_TotalRecords");
                int totalPages = parameters.Get<int>("@v_TotalPages");
                int recordStart = parameters.Get<int>("@v_RecordStart");
                int recordEnd = parameters.Get<int>("@v_RecordEnd");
                var obj = new
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    RecordStart = recordStart,
                    RecordEnd = recordEnd,
                    Data = userData,
                };
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Xóa hàng loạt người dùng khỏi 1 nhóm người dùng
        /// </summary>
        /// <param name="groupId">Id của nhóm</param>
        /// <param name="userIds">Mảng id người dùng</param>
        /// <returns>1-Xóa thành công</returns>
        /// Author: VQBao - 12/9/2022
        public int DeleteMultiUserOfGroup(int groupId, List<Guid> userIds)
        {
            try
            {
                string listId = string.Join(",", userIds);
                var storeProc = $"Proc_DeleteMultiUserOfGroup";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserGroupID", groupId);
                parameters.Add("v_UserID", listId);
                var data = connection.Execute(storeProc, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

                                                                                                                                                                                                                                                                                                                                                                                                      /// <summary>
        /// Sửa thành viên trong nhóm người dùng
        /// </summary>
        /// <param name="groupId">Id của nhóm người dùng</param>
        /// <param name="userIds">Mảng id của người dùng</param>
        /// Author: VQBao - 12/9/2022
        public int UpdateMemberOfGroup(int groupId, List<Guid>? userIds)
        {
            try
            {
                var jsonUserIDs = JsonConvert.SerializeObject(userIds);
                var storeProc = $"Proc_InsertMultiUserToGroup";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserGroupID", groupId);
                parameters.Add("v_JsonUserIDs", jsonUserIDs);
                var data = connection.Execute(storeProc, parameters, commandType: System.Data.CommandType.StoredProcedure);
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
