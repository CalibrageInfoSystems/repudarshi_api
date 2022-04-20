using Model;
using log4net;
using Model.Response;
using Repository.Interface.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Settings
{ 
      public class RolesRepository : IRoles
    {
        RupdarshiEntities ctx = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ValueDataResponse<Model.Role> AddUpdateRole(AddUpdateRoleReq req)
        {
            ValueDataResponse<Model.Role> response = new ValueDataResponse<Model.Role>();

            try
            {
                DateTime CreatedDate = req.CreatedDate;

                CreatedDate = TimeZoneInfo.ConvertTimeToUtc(CreatedDate);
                CreatedDate = new DateTime(CreatedDate.Ticks, DateTimeKind.Utc);
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                DateTime UpdatedDate = req.UpdatedDate;

                UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(UpdatedDate);
                UpdatedDate = new DateTime(UpdatedDate.Ticks, DateTimeKind.Utc);
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));

                var result = ctx.AddUpdateRole(req.Id, req.Code, req.NAME, req.Desc, req.ParentRoleId,req.ActivityRightIds, req.IsActive, req.CreatedByUserId, CreatedDate, req.UpdatedByUserId, UpdatedDate, statusCode, statusMessage);

                //var result = mr.AddUpdateRole(req.Id, req.Code, req.Name, req.Desc, req.ParentRoleId, req.ActivityRightIds, req.IsActive, req.CreatedByUserId, CreatedDate, req.UpdatedByUserId, UpdatedDate);

                var sc = Convert.ToInt32(statusCode.Value);

                if (sc > 0)
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Role added Succesfully";
                    return response;
                }
                else if (sc == -1)
                {
                    response.IsSuccess = false;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "Bad request";
                    return response;
                }
                else if (sc == 0)
                {
                    response.IsSuccess = false;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = statusMessage.Value.ToString();
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "Internal Server Error";
                    return response;
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return response;
            }
        }

        public ListDataResponse<GetAllRoles_Result> GetAllRoles(int? Id,bool? IsActive)
        {
            ListDataResponse<GetAllRoles_Result> response = new ListDataResponse<GetAllRoles_Result>();

            try
            {
                var result = ctx.GetAllRoles(Id, IsActive).ToList();

                if (result.Count() > 0)
                {
                    response.ListResult = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = result.Count();
                    response.EndUserMessage = "Get all roles successfull";
                    return response;
                }
                else
                {
                    response.ListResult = null;
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No data found";
                    return response;
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return response;
            }
        }
        public ListDataResponse<GetActivityRightsByRoleId_Result> GetActivityRightsByRoleId(int RoleId)
        {
            ListDataResponse<GetActivityRightsByRoleId_Result> response = new ListDataResponse<GetActivityRightsByRoleId_Result>();

            try
            {
                var result = ctx.GetActivityRightsByRoleId(RoleId).ToList();

                if (result.Count() > 0)
                {
                    response.ListResult = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = result.Count();
                    response.EndUserMessage = "Get all Activity rights successfull";
                    return response;
                }
                else
                {
                    response.ListResult = null;
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No data found";
                    return response;
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return response;
            }
        }
        public ListDataResponse<GetActivityRights_Result> GetActivityRights(int? Id)
        {
            ListDataResponse<GetActivityRights_Result> response = new ListDataResponse<GetActivityRights_Result>();

            try
            {
                var result = ctx.GetActivityRights(Id).ToList();

                if (result.Count() > 0)
                {
                    response.ListResult = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = result.Count();
                    response.EndUserMessage = "Get all activity rights successfull";
                    return response;
                }
                else
                {
                    response.ListResult = null;
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No data found";
                    return response;
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return response;
            }
        }
        public ListDataResponse<GetUsersByRoleIdStoreId_Result> GetUsersByRoleStore(GetUsersByRoleAndStoreReq req)
        {
            ListDataResponse<GetUsersByRoleIdStoreId_Result> response = new ListDataResponse<GetUsersByRoleIdStoreId_Result>();

            try
            {
                var result = ctx.GetUsersByRoleIdStoreId(req.RoleId,req.StoreId).ToList();

                if (result.Count() > 0)
                {
                    response.ListResult = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = result.Count();
                    response.EndUserMessage = "Get all Users successfully";
                    return response;
                }
                else
                {
                    response.ListResult = null;
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No data found";
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return response;
            }
        }
    }
}
