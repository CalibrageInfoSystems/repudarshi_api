using Model;
using Model.Response;
using Repository.Interface.Settings;
using Repository.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors; 
namespace API.Controllers
{
    [RoutePrefix("api/Roles")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RoleController : ApiController
    {
        public static RupdarshiEntities _context = new RupdarshiEntities();

        public IRoles _roleRepo = new RolesRepository();

        [HttpPost]
        //[Authorize]
        [Route("AddUpdateRole")]
        public ValueDataResponse<Model.Role> Post(AddUpdateRoleReq role)
        {
            return _roleRepo.AddUpdateRole(role);
        }
        [HttpGet]
        //[Authorize]
        [Route("GetRoles/{Id}")]
        public ListDataResponse<GetAllRoles_Result> Get(int? Id)
        {
            return _roleRepo.GetAllRoles(Id,true);
        }
        [HttpGet]
        //[Authorize]
        [Route("GetAllRoles/{Id}/{IsActive}")]
        public ListDataResponse<GetAllRoles_Result> GetAllRoles(int? Id, bool? IsActive)
        {
            return _roleRepo.GetAllRoles(Id,IsActive);
        }
        /// <summary>
        /// Get ActivityRights By RoleId
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [Route("GetActivityRightsByRoleId/{RoleId}")]
        //[Authorize]
        [HttpGet]
        public ListDataResponse<GetActivityRightsByRoleId_Result> GetActivityRightsByRoleId(int RoleId)
        {
            return _roleRepo.GetActivityRightsByRoleId(RoleId);
        }
        [Route("GetActivityRights/{Id}")]
        //[Authorize]
        [HttpGet]
        public ListDataResponse<GetActivityRights_Result> GetActivityRights(int? Id)
        {
            return _roleRepo.GetActivityRights(Id);
        }
        [Route("GetUsersByRoleAndStore")] 
        [HttpPost]
        public ListDataResponse<GetUsersByRoleIdStoreId_Result> GetUsersByRoleAndStore(GetUsersByRoleAndStoreReq req)
        { 
            return _roleRepo.GetUsersByRoleStore(req);
        }
    }
}
