using Dapper;
using Microsoft.AspNetCore.Http;
using MISA.APIS.Core.Entities;
using MISA.APIS.Core.Interfaces.Infrastructure;
using MISA.APIS.Core.Interfaces.Services;
using OfficeOpenXml;
using MISA.APIS.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static MISA.APIS.Core.Enum.MISAEnum;


using MISA.APIS.Core.Resource;

namespace MISA.APIS.Core.Services
{
    public class UserService: BaseService<User> ,IUserService
    {
        #region Constructor

        IUserRepository userRepository;
        IJobPositionRepository jobPositionRepository;
        IOrganizationUnitRepository organizationUnitRepository;
        public UserService(IUserRepository _userRepository, IJobPositionRepository _jobPositionRepository, IOrganizationUnitRepository _organizationUnitRepository) : base(_userRepository)
        {
            userRepository = _userRepository;
            jobPositionRepository = _jobPositionRepository;
            organizationUnitRepository = _organizationUnitRepository;
        }

        List<string> ErrorValidateMsgs = new List<string>();
        #endregion

        #region Methods

        /// <summary>
        /// Lấy dữ liệu người dùng phân trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi/trang</param>
        /// <param name="pageNumber">Số thứ tự trang hiển thị</param>
        /// <param name="searchKey">Từ khóa tìm kiếm</param>
        /// <returns>Object chứa các thông tin trả về</returns>
        /// Author: VQBao - 6/9/2022
        public Object GetUserPaging(int pageSize, int pageNumber, string? searchByText, string? searchByGroup)
        {
            return userRepository.GetUserPaging(pageSize, pageNumber, searchByText, searchByGroup);
        }

        /// <summary>
        /// Lấy người dùng theo id
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>Object chứ thông tin người dùng, danh sách nhóm của người dùng đó</returns>
        /// Author: VQBao - 6/9/2022
        public Object GetById(Guid id)
        {
            return userRepository.GetById(id);
        }

        /// <summary>
        /// Xóa 1 người dùng
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <returns>1-Xóa thành công, 0-Xóa thất bại</returns>
        /// Author: VQBao - 10/8/2022
        public int DeleteUser(Guid id)
        {
            return userRepository.DeleteUser(id);
        }

        /// <summary>
        /// Sửa nhóm của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <param name="groupIds">Mảng id của nhóm thuộc người dùng đó</param>
        /// <returns>1-update thành công</returns>
        /// Author: VQBao - 12/9/2022
        public int UpdateGroupOfUser(Guid userId, List<int> groupIds)
        {
            return userRepository.UpdateGroupOfUser(userId, groupIds);
        }

        /// <summary>
        /// Xuất khẩu dữ liệu người dùng ra file excel
        /// </summary>
        /// <returns>1 luồng chứa file excel danh sách người dùng</returns>
        /// Author: VQBao - 12/9/2022
        public MemoryStream ExportToExcel()
        {
            try
            {
                var listUser = userRepository.GetAll();
                var users = listUser.ToList();
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    // tạo worksheet để hiển thị bảng danh sách giáo viên
                    var workSheet = package.Workbook.Worksheets.Add("MISA_APIS_Excel");

                    // Tạo tiêu đề của bảng
                    workSheet.Cells["A1:I1"].Merge = true;
                    workSheet.Cells["A1"].Value = "DANH SÁCH NGƯỜI DÙNG";
                    workSheet.Cells["A1:I1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    workSheet.Cells["A1:I1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Cells["A1:I1"].Style.Font.Size = 16;
                    workSheet.Row(1).Height = 24;
                    workSheet.Row(2).Height = 24;
                    workSheet.Cells["A1"].Style.Font.Bold = true;
                    workSheet.Cells["A2:I2"].Merge = true;

                    List<string> listHeader = new List<string>()
                    {
                        "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3", "I3"
                    };

                    // Tạo border cho bảng
                    listHeader.ForEach(c =>
                    {
                        workSheet.Cells[c].Style.Font.Bold = true;
                        workSheet.Cells[c].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        workSheet.Cells[c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(211, 211, 211));

                    });

                    // Tạo tiêu đề cho các cột trường thông tin
                    workSheet.Cells[listHeader[0]].Value = ResourceVN.Table_FullName;
                    workSheet.Cells[listHeader[1]].Value = ResourceVN.Table_JobPosition;
                    workSheet.Cells[listHeader[2]].Value = ResourceVN.Table_OrganizationUnit;
                    workSheet.Cells[listHeader[3]].Value = ResourceVN.Table_Organization;
                    workSheet.Cells[listHeader[4]].Value = ResourceVN.Table_Email;
                    workSheet.Cells[listHeader[5]].Value = ResourceVN.Table_UserGroup;
                    workSheet.Cells[listHeader[6]].Value = ResourceVN.Table_Mobile;
                    workSheet.Cells[listHeader[7]].Value = ResourceVN.Table_WorkPhone;
                    workSheet.Cells[listHeader[8]].Value = ResourceVN.Table_Status;

                    for (int i = 0; i < users.Count; i++)
                    {
                        workSheet.Cells[i + 4, 1].Value = users[i].FullName;
                        workSheet.Cells[i + 4, 2].Value = users[i].JobPositionName;
                        if (!string.IsNullOrEmpty(users[i].OrganizationUnitName))
                        {
                            workSheet.Cells[i + 4, 3].Value = users[i].OrganizationUnitName;
                        }
                        else
                        {
                            workSheet.Cells[i + 4, 3].Value = "-";
                        }
                        workSheet.Cells[i + 4, 4].Value = users[i].OrganizationName;
                        if (!string.IsNullOrEmpty(users[i].Email))
                        {
                            workSheet.Cells[i + 4, 5].Value = users[i].Email;
                        }
                        else
                        {
                            workSheet.Cells[i + 4, 5].Value = "-";
                        }
                        if (!string.IsNullOrEmpty(users[i].UserGroupName))
                        {
                            workSheet.Cells[i + 4, 6].Value = users[i].UserGroupName;
                        }
                        else
                        {
                            workSheet.Cells[i + 4, 6].Value = "-";
                        }
                        if (!string.IsNullOrEmpty(users[i].Mobile))
                        {
                            workSheet.Cells[i + 4, 7].Value = users[i].Mobile;
                        }
                        else
                        {
                            workSheet.Cells[i + 4, 7].Value = "-";
                        }
                        if (!string.IsNullOrEmpty(users[i].WorkPhone))
                        {
                            workSheet.Cells[i + 4, 8].Value = users[i].WorkPhone;
                        }
                        else
                        {
                            workSheet.Cells[i + 4, 8].Value = "-";
                        }
                        if (users[i].Status == MISAEnum.UserStatus.Active)
                        {
                            workSheet.Cells[i + 4, 9].Value = ResourceVN.Table_Status_Active;
                        }
                        else
                        {
                            workSheet.Cells[i + 4, 9].Value = ResourceVN.Table_Status_Inactive;
                        }
                    }

                    // Tạo độ rộng hiển thị cho các cột của bảng
                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    workSheet.Column(5).AutoFit();
                    workSheet.Column(6).Width = 80;
                    workSheet.Column(7).AutoFit();
                    workSheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Column(8).AutoFit();
                    workSheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Column(9).AutoFit();
                    package.Save();
                }
                // tạo link download file excel danh sách giáo viên
                stream.Position = 0;
                return stream;
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
            return userRepository.GetUserGrid(id);
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
            return userRepository.UpdateUserGrid(userId, userOption);
        }

        /// <summary>
        /// Lấy dữ liệu người dùng từ file excel import
        /// </summary>
        /// <param name="formFile">File import</param>
        /// <returns>Mảng danh sách người dùng</returns>
        /// Author: VQBao - 20/9/2022
        public List<User> GetUserFromImportFile(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return null;
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var users = new List<User>();
            var listJobPosition = jobPositionRepository.GetAll().ToList();
            var listOrganizationUnit = organizationUnitRepository.GetAll().ToList();

            using (var stream = new MemoryStream())
            {
                formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var fullName = ConvertObjectToString(worksheet.Cells[row, 1].Value);
                        var jobPositionName = ConvertObjectToString(worksheet.Cells[row, 2].Value);
                        var organizationUnitName = ConvertObjectToString(worksheet.Cells[row, 3].Value);
                        var organizationName = ConvertObjectToString(worksheet.Cells[row, 4].Value);
                        var email = ConvertObjectToString(worksheet.Cells[row, 5].Value);
                        var mobile = ConvertObjectToString(worksheet.Cells[row, 6].Value);
                        var workPhone = ConvertObjectToString(worksheet.Cells[row, 7].Value);
                        var status = GetUserStatus(worksheet.Cells[row, 8].Value);
                        var user = new User()
                        {
                            UserID = Guid.NewGuid(),
                            FullName = fullName,
                            JobPositionName = jobPositionName,
                            JobPositionID = GetJobPositionID(listJobPosition, jobPositionName),
                            OrganizationUnitID = GetOrganizationUnitID(listOrganizationUnit, organizationUnitName, organizationName),
                            OrganizationUnitName = organizationUnitName,
                            OrganizationName = organizationName,
                            Email = email,
                            Mobile = mobile,
                            WorkPhone = workPhone,
                            Status = status,
                        };
                        // Làm mới danh sách lỗi
                        ErrorValidateMsgs.Clear();
                        var isValid = ValidateUser(user);
                        if(isValid == false)
                        {
                            user.IsValidToImport = false;
                            user.ListErrorImport.AddRange(ErrorValidateMsgs);
                        }
                        users.Add(user);
                    }
                }
            }
            return users;
        }

        /// <summary>
        /// Lấy số lượng bản ghi hợp lệ, không hợp lệ, danh sách người dùng hợp lệ, không hợp lệ từ file excel
        /// </summary>
        /// <param name="fileImport">File excel import</param>
        /// <returns>Object chứa các thông tin số lượng bản ghi hợp lệ, không hợp lệ, danh sách người dùng hợp lệ, không hợp</returns>
        /// Author: VQBao - 20/9/2022
        public Object GetImportQuantity(IFormFile formFile)
        {
            var users = GetUserFromImportFile(formFile);
            var validUsers = users.Where(e => e.IsValidToImport == true).ToList();
            var invalidUsers = users.Where(e => e.IsValidToImport == false).ToList();
            return new { TotalValid = validUsers.Count, TotalInvalid = invalidUsers.Count, ValidUsers = validUsers, InvalidUsers = invalidUsers };
        }

        /// <summary>
        /// Nhập khẩu những người dùng hợp lệ
        /// </summary>
        /// <param name="validUsers">Danh sách người dùng hợp lệ</param>
        /// <returns>Số người dùng đã nhập khẩu</returns>
        /// Author: VQBao - 20/9/2022
        public int ImportValidUsers(List<User> validUsers)
        {
            var userInserted = 0;
            if (validUsers.Count > 0)
            {
                userInserted = userRepository.InsertUser(validUsers);
            }
            return userInserted;
        }

        /// <summary>
        /// Xuất khẩu danh sách người dùng không hợp lệ ra file excel
        /// </summary>
        /// <param name="users">Danh sách người dùng không hợp lệ</param>
        /// Author: VQBao - 20/9/2022
        public MemoryStream ExportInvalidUser(List<User> users)
        {
            try
            {
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    // tạo worksheet để hiển thị bảng danh sách giáo viên
                    var workSheet = package.Workbook.Worksheets.Add("MISA_APIS_Excel");
                    List<string> listHeader = new List<string>()
                    {
                        "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1", "I1"
                    };

                    // Tạo border cho bảng
                    listHeader.ForEach(c =>
                    {
                        workSheet.Cells[c].Style.Font.Bold = true;
                        workSheet.Cells[c].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        workSheet.Cells[c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(211, 211, 211));

                    });

                    // Tạo tiêu đề cho các cột trường thông tin
                    workSheet.Cells[listHeader[0]].Value = ResourceVN.Table_FullName;
                    workSheet.Cells[listHeader[1]].Value = ResourceVN.Table_JobPosition;
                    workSheet.Cells[listHeader[2]].Value = ResourceVN.Table_OrganizationUnit;
                    workSheet.Cells[listHeader[3]].Value = ResourceVN.Table_Organization;
                    workSheet.Cells[listHeader[4]].Value = ResourceVN.Table_Email;
                    workSheet.Cells[listHeader[5]].Value = ResourceVN.Table_Mobile;
                    workSheet.Cells[listHeader[6]].Value = ResourceVN.Table_WorkPhone;
                    workSheet.Cells[listHeader[7]].Value = ResourceVN.Table_Status;
                    workSheet.Cells[listHeader[8]].Value = ResourceVN.Table_Error;

                    for (int i = 0; i < users.Count; i++)
                    {
                        workSheet.Cells[i + 2, 1].Value = users[i].FullName;
                        workSheet.Cells[i + 2, 2].Value = users[i].JobPositionName;
                        workSheet.Cells[i + 2, 3].Value = users[i].OrganizationUnitName;
                        workSheet.Cells[i + 2, 4].Value = users[i].OrganizationName;
                        workSheet.Cells[i + 2, 5].Value = users[i].Email;
                        workSheet.Cells[i + 2, 6].Value = users[i].Mobile;
                        workSheet.Cells[i + 2, 7].Value = users[i].WorkPhone;
                        if (users[i].Status == MISAEnum.UserStatus.Active)
                        {
                            workSheet.Cells[i + 2, 8].Value = ResourceVN.Table_Status_Active;
                        }
                        else
                        {
                            workSheet.Cells[i + 2, 8].Value = ResourceVN.Table_Status_Inactive;
                        }
                        workSheet.Cells[i + 2, 9].Value = string.Join(", " ,users[i].ListErrorImport);
                    }

                    // Tạo độ rộng hiển thị cho các cột của bảng
                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    workSheet.Column(5).AutoFit();
                    workSheet.Column(6).AutoFit();
                    workSheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Column(7).AutoFit();
                    workSheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Column(8).AutoFit();
                    workSheet.Column(9).Width = 60;
                    package.Save();
                }
                // tạo link download file excel 
                stream.Position = 0;
                return stream;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Xuất khẩu file excel nhập liệu mẫu
        /// </summary>
        /// Author: VQBao - 20/9/2022
        public MemoryStream ExportSampleExcel()
        {
            try
            {
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    // tạo worksheet để hiển thị bảng danh sách giáo viên
                    var workSheet = package.Workbook.Worksheets.Add("MISA_APIS_Excel");
                    List<string> listHeader = new List<string>()
                    {
                        "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1"
                    };

                    // Tạo border cho bảng
                    listHeader.ForEach(c =>
                    {
                        workSheet.Cells[c].Style.Font.Bold = true;
                        workSheet.Cells[c].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        workSheet.Cells[c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(211, 211, 211));

                    });

                    // Tạo tiêu đề cho các cột trường thông tin
                    workSheet.Cells[listHeader[0]].Value = ResourceVN.Table_FullName;
                    workSheet.Cells[listHeader[1]].Value = ResourceVN.Table_JobPosition;
                    workSheet.Cells[listHeader[2]].Value = ResourceVN.Table_OrganizationUnit;
                    workSheet.Cells[listHeader[3]].Value = ResourceVN.Table_Organization;
                    workSheet.Cells[listHeader[4]].Value = ResourceVN.Table_Email;
                    workSheet.Cells[listHeader[5]].Value = ResourceVN.Table_Mobile;
                    workSheet.Cells[listHeader[6]].Value = ResourceVN.Table_WorkPhone;
                    workSheet.Cells[listHeader[7]].Value = ResourceVN.Table_Status;

                    // Tạo độ rộng hiển thị cho các cột của bảng
                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    workSheet.Column(5).AutoFit();
                    workSheet.Column(6).AutoFit();
                    workSheet.Column(6).Style.Numberformat.Format = "@";
                    workSheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Column(7).AutoFit();
                    workSheet.Column(7).Style.Numberformat.Format = "@";
                    workSheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Column(8).AutoFit();
                    package.Save();
                }
                // tạo link download file excel 
                stream.Position = 0;
                return stream;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Chuyển kiểu dữ liệu từ object sang string
        /// </summary>
        /// <param name="value">giá trị của object</param>
        /// <returns>string ứng với object đó</returns>
        /// Author: VQBao - 20/9/2022
        private string? ConvertObjectToString(object? value)
        {
            if(value == null)
            {
                return null;
            } else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Lấy ID của chức vụ
        /// </summary>
        /// <param name="jobPositions">Danh sách các chức vụ</param>
        /// <param name="jobPositionName">Tên chức vụ muốn lấy Id</param>
        /// <returns>Id của chức vụ đó</returns>
        /// Author: VQBao - 20/9/2022
        public int? GetJobPositionID(List<JobPosition> jobPositions, string? jobPositionName)
        {
            foreach (var job in jobPositions)
            {
                if(job.JobPositionName == jobPositionName)
                {
                    return job.JobPositionID;
                }
            }
            return null;
        }

        /// <summary>
        /// Lấy ra ID của phòng ban theo tên phòng ban và tên đơn vị
        /// </summary>
        /// <param name="organizationUnits">Danh sách tất cả phòng ban</param>
        /// <param name="organizationUnitName">Tên phòng ban</param>
        /// <param name="organizationName">Tên đơn vị</param>
        /// <returns>Id của phòng ban</returns>
        /// Author: VQBao - 20/9/2022
        public int? GetOrganizationUnitID(List<OrganizationUnit> organizationUnits, string? organizationUnitName, string? organizationName)
        {
            if (string.IsNullOrEmpty(organizationUnitName) && !string.IsNullOrEmpty(organizationName))
            {
                foreach (var organizationUnit in organizationUnits)
                {
                    if (organizationUnit.OrganizationUnitName == organizationName) return organizationUnit.OrganizationUnitID;
                }
            } 
            else if(!string.IsNullOrEmpty(organizationUnitName) && !string.IsNullOrEmpty(organizationName))
            {
                foreach (var organizationUnit in organizationUnits)
                {
                    if (organizationUnit.OrganizationUnitName == organizationUnitName) return organizationUnit.OrganizationUnitID;
                }
            }
            return null;
        }

        /// <summary>
        /// Lấy trạng thái hoạt động của người dùng
        /// </summary>
        /// <param name="value">object chứa thông tin từ cột trạng thái của file nhập khẩu</param>
        /// <returns>Trạng thái người dùng</returns>
        /// Author: VQBao - 20/9/2022
        public UserStatus? GetUserStatus(object? value)
        {
            if(value == null) return UserStatus.Inactive;
            if (value.ToString() == ResourceVN.Table_Status_Active)
            {
                return UserStatus.Active;
            } else 
            {
                return UserStatus.Inactive;
            }
        }

        /// <summary>
        /// Validate các trường thông tin của người dùng
        /// </summary>
        /// <param name="user">Người dùng</param>
        /// <returns>true: hợp lệ, false: không hợp lệ, danh sách lỗi</returns>
        /// Author: VQBao - 20/9/2022
        private bool ValidateUser(User user)
        {
            bool isValid = true;
            // xử lý nghiệp vụ
            // Họ và tên không được phép để trống
            if (string.IsNullOrEmpty(user.FullName))
            {
                ErrorValidateMsgs.Add(ResourceVN.Error_EmptyFullName);
                isValid = false;
            }
            // Chức vụ không được phép để trống
            if (string.IsNullOrEmpty(user.JobPositionName))
            {
                ErrorValidateMsgs.Add(ResourceVN.Error_EmptyJobPosition);
                isValid = false;
            }
            // Tên chức vụ không chính xác
            if (user.JobPositionID == null && !string.IsNullOrEmpty(user.JobPositionName))
            {
                ErrorValidateMsgs.Add(ResourceVN.Error_NotJobPositionName);
                isValid = false;
            }
            // Đơn vị không được phép để trống
            if (string.IsNullOrEmpty(user.OrganizationName))
            {
                ErrorValidateMsgs.Add(ResourceVN.Error_EmptyOrganization);
                isValid = false;
            }
            // Tên phòng ban, đơn vị không chính xác
            if (user.OrganizationUnitID == null && !string.IsNullOrEmpty(user.OrganizationName))
            {
                ErrorValidateMsgs.Add(ResourceVN.Error_NotOrganizationName);
                isValid = false;
            }
            // Email không đúng định dạng
            if (user.Email != null && user.Email != "" && !IsEmail(user.Email))
            {
                ErrorValidateMsgs.Add(ResourceVN.Error_NotEmail);
                isValid = false;
            }
            // Điện thoại di động không đúng độ dài
            if(user.Mobile != null && (user.Mobile.Length < 9 || user.Mobile.Length > 11))
            {
                ErrorValidateMsgs.Add(ResourceVN.Error_LengthMobile);
                isValid = false;
            }
            // Điện thoại cơ quan không đúng độ dài
            if (user.WorkPhone != null && (user.WorkPhone.Length < 9 || user.WorkPhone.Length > 11))
            {
                ErrorValidateMsgs.Add(ResourceVN.Error_LengthWorkPhone);
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// Hàm kiểm tra email có hợp lệ hay không
        /// </summary>
        /// <param name="email">Tham số đầu vào muốn kiểm tra</param>
        /// <returns>true: là email, false: không là email</returns>
        /// Author: VQBao - 15/9/2020
        public Boolean IsEmail(string email)
        {
            Regex validateEmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (validateEmailRegex.IsMatch(email)) { return true; }
            else return false;
        }
        #endregion
    }
}
