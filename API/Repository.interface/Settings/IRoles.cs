using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace Repository.Interface.Settings
{
    public interface IRoles
{
        ValueDataResponse<Model.Role> AddUpdateRole(AddUpdateRoleReq req);
        ListDataResponse<GetAllRoles_Result> GetAllRoles(int? Id,bool? IsActive);
        ListDataResponse<GetActivityRightsByRoleId_Result> GetActivityRightsByRoleId(int RoleId);
        ListDataResponse<GetActivityRights_Result> GetActivityRights(int? Id);
        ListDataResponse<GetUsersByRoleIdStoreId_Result> GetUsersByRoleStore(GetUsersByRoleAndStoreReq req);
    }
}
