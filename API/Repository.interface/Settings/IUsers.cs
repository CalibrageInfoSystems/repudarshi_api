using Model;
using Model.Identity;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace Repository.Interface.Settings
{
   public interface IUsers
{
        ValueDataResponse<GetUserInfoById_Result> AddUpdateUser(Model.Identity.UserModel req);
        ValueDataResponse<GetVendorInfoById_Result> AddUpdatecustomerInfo(Model.Identity.RegisterModel req);
        ListDataResponse<GetUserInfoById_Result> GetUser(int? Id);
        Task<ValueDataResponse<dynamic>>  Login(LoginRequest req);
        Task<ValueDataResponse<dynamic>> CustomerLogin(LoginRequest req);
        Task<ValueDataResponse<dynamic>> ChangePassword(ChangePassword changePassword);
        ValueDataResponse<ValidateUser_Result> ValidateUser(string UserName, string Password);
        ValueDataResponse<UserInfo> ValidateUser(string UserName);
        ValueDataResponse<GetUserinfoByUserName_Result> GetUserinfoByUserName(string UserName);
        ListDataResponse<GetUsersByRoleId_Result> GetUsersByRoleId(string RoleId);
        ListDataResponse<GetUsersBySearch_Result> GetUsersBySearch(GetUsersBySearchReq req); 
        ListDataResponse<GetVendorInfo_Result> GetVendorInfo(VendorReq req);
        ListDataResponse<dynamic> GetVendorDetailsById(string SearchKey);
        ValueDataResponse<UpdateVendorStatus> UpdateVendorStatus(UpdateVendorStatus req);
        ValueDataResponse<UserInfo> UpdateDeviseTokenByUserId(UpdateDeviseToken deviseToken);//
    }
}
