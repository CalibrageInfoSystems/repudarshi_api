using log4net;
using Model;
using Microsoft.AspNet.Identity;  
using Model.Identity;
using Model.Response;
using Newtonsoft.Json;
using Repository.Identity;
using Repository.Interface.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Configuration;

namespace Repository.Settings
{
    public class UsersRepository : IUsers
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        AuthRepository _auth = new AuthRepository();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int CustomerRoleId = Convert.ToInt32(ConfigurationManager.AppSettings["CustomerRoleId"].ToString());
        public ValueDataResponse<GetUserInfoById_Result> AddUpdateUser(Model.Identity.UserModel req)
        {
            ValueDataResponse<GetUserInfoById_Result> response = new ValueDataResponse<GetUserInfoById_Result>();
            try
            {
                //var user = new ApplicationUser { UserName = req.UserName, Email = req.Email, PhoneNumber = req.MobileNumber };

                //var identityResult = await _userManager.CreateAsync(user, req.Password);
                IdentityResult identityResult = new IdentityResult();
                if (req.Id == 0)
                {
                    identityResult = _auth.RegisterUser(req.UserName,req.Password);
                }

                if (identityResult.Succeeded || req.Id != 0)
                {
                    DateTime CreatedDate = req.CreatedDate;
                    CreatedDate = TimeZoneInfo.ConvertTimeToUtc(CreatedDate);
                    CreatedDate = new DateTime(CreatedDate.Ticks, DateTimeKind.Utc);
                    CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                    DateTime UpdatedDate = req.UpdatedDate;
                    UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(UpdatedDate);
                    UpdatedDate = new DateTime(UpdatedDate.Ticks, DateTimeKind.Utc);
                    UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                    //using (var scope = new TransactionScope())
                    //{
                    //    _uInfo = req;
                    //    _uInfo.CreatedDate = CreatedDate;
                    //    _uInfo.UpdatedDate = UpdatedDate;
                    //    _unitOfWork.UserRepository.Insert(_uInfo);
                    //    _unitOfWork.Save();
                    //    scope.Complete();
                    //}

                    ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                    ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));
                    var UserId = "";
                    if (req.Id == 0)
                    {
                        UserId = _context.Users.Where(u => u.UserName == req.UserName).FirstOrDefault().Id;
                    }
                    
                    var result = _context.AddUpdateUserInfo(req.Id,(req.Id==0?UserId: req.UserId), req.FirstName, req.LastName, req.MiddleName,
                        req.ContactNumber, req.Email, req.UserName, req.Password, req.RoleId, req.StoreIds, req.ManagerId, req.Address, req.IsActive, req.CreatedByUserId,
                        CreatedDate, req.UpdatedByUserId, UpdatedDate, statusCode, statusMessage);

                    var sc = Convert.ToInt32(statusCode.Value);

                    if (sc > 0)
                    {
                        response.Result = _context.GetUserInfoById(sc).FirstOrDefault();
                        response.AffectedRecords = 1;
                        response.IsSuccess = true;
                        response.EndUserMessage = statusMessage.Value.ToString();
                    }
                    else
                    {
                        if (req.Id == 0)
                        {
                            var usr = _context.Users.Where(u => u.UserName == req.UserName).FirstOrDefault();
                            _context.Users.Remove(usr);
                            _context.SaveChanges();
                        }
                       
                        response.AffectedRecords = 0;
                        response.IsSuccess = false;
                        response.EndUserMessage = statusMessage.Value.ToString();
                    }
                }
                else
                {
                  
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = identityResult.Errors.FirstOrDefault().ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<GetVendorInfoById_Result> AddUpdatecustomerInfo(Model.Identity.RegisterModel req)
        {
            ValueDataResponse<GetVendorInfoById_Result> response = new ValueDataResponse<GetVendorInfoById_Result>();
            try
            { 
                 
                IdentityResult identityResult = new IdentityResult();
                if (req.Id == 0)
                {
                    identityResult = _auth.RegisterUser(req.UserName, req.Password);
                }

                if (identityResult.Succeeded || req.Id != 0)
                {
                    DateTime CreatedDate = DateTime.UtcNow;
                    
                    DateTime UpdatedDate = DateTime.UtcNow;
                    
                    ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                    ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));

                    var userId = _context.Users.Where(u => u.UserName == req.UserName).FirstOrDefault().Id;
   
                    
                    var result = _context.AddUpdateVendorInfo(req.Id, userId, req.FirstName, req.MiddleName, req.LastName, req.UserName,
                        req.ContactNumber, req.ContactNumber, req.Email, req.BusinessName, req.GSTIN, req.Country, req.State, req.City, req.Password, req.Address1, req.Address2,
                        req.Address3, req.Landmark,req.Pincode, req.Latitude, req.Longitude,(int)Roles.Vendor,req.ServiceTypeId ,(int)VendorStatus.ApprovalforPending,req.CreatedByUserId, CreatedDate, req.UpdatedByUserId, UpdatedDate, statusCode, statusMessage);

                   

                    var sc = Convert.ToInt32(statusCode.Value);

                    if (sc > 0)
                    {
                        response.Result = _context.GetVendorInfoById(sc).FirstOrDefault();
                        response.AffectedRecords = 1;
                        response.IsSuccess = true;
                        response.EndUserMessage = statusMessage.Value.ToString();
                    }
                    else
                    {
                        if (req.Id == 0)
                        {
                            var usr = _context.Users.Where(u => u.UserName == req.UserName).FirstOrDefault();
                            _context.Users.Remove(usr);
                            _context.SaveChanges();
                        }
                        response.AffectedRecords = 0;
                        response.IsSuccess = false;
                        response.EndUserMessage = statusMessage.Value.ToString();
                    }
                }
                else
                {
                   
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = identityResult.Errors.FirstOrDefault().ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ListDataResponse<GetUserInfoById_Result> GetUser(int? Id)
        {
            ListDataResponse<GetUserInfoById_Result> response = new ListDataResponse<GetUserInfoById_Result>();

            try
            {
                //response.Result = _unitOfWork.UserRepository.GetByID(Id);
                response.ListResult = _context.GetUserInfoById(Id).ToList();

                if (response.ListResult != null)
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Get User details successful";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No data found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }

            return response;
        }
        public async Task<ValueDataResponse<dynamic>> Login(LoginRequest req)
        {
             ValueDataResponse<dynamic>  response = new ValueDataResponse<dynamic> ();

            try
            {
                //var tokenServiceUrl = ConfigurationManager.AppSettings["tokenServiceUrl"].ToString();
                var request = HttpContext.Current.Request;
                var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/token";

                using (var client = new HttpClient())
                {
                    var requestParams = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("grant_type", "password"),
                            new KeyValuePair<string, string>("username", req.UserName),
                            new KeyValuePair<string, string>("password", req.Password)
                        };
                    var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    var responseCode = tokenServiceResponse.StatusCode;

                    if (responseCode == HttpStatusCode.OK)
                    {
                        LoginRes res = JsonConvert.DeserializeObject<LoginRes>(responseString);

                        var user = _context.UserInfoes.Where(u => u.UserName == req.UserName && u.Password == req.Password&&u.RoleId != CustomerRoleId&&u.IsActive==true).FirstOrDefault();

                        if (user != null)
                        {
                            //var result = new LoginRes
                            //{
                            //    access_token = res.access_token,
                            //    token_type = res.token_type,
                            //    expires_in = res.expires_in,
                            //    UserInfos = _context.GetUserInfoById(users.FirstOrDefault().Id).FirstOrDefault()
                            //};

                            response.Result = new LoginRes
                            {
                                access_token = res.access_token,
                                token_type = res.token_type,
                                expires_in = res.expires_in,
                                UserInfos = _context.GetUserInfoById(user.Id).FirstOrDefault(),
                                activityRights = _context.GetActivityRightsByRoleId(user.RoleId).ToList()
                            };
                            response.IsSuccess = true;
                            response.AffectedRecords = 1;
                            response.EndUserMessage = "Login Successful";

                        }
                        else
                        {

                            response.IsSuccess = false;
                            response.AffectedRecords = 0;
                            response.EndUserMessage = "Invalid UserName or Password";
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.AffectedRecords = 0;
                        response.EndUserMessage = "Invalid UserName or Password";
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
               
            }
            return response;
        }
        public async Task<ValueDataResponse<dynamic>> CustomerLogin(LoginRequest req)
        {
            ValueDataResponse<dynamic> response = new ValueDataResponse<dynamic>();

            try
            {
               
                //var tokenServiceUrl = ConfigurationManager.AppSettings["tokenServiceUrl"].ToString();
                var request = HttpContext.Current.Request;
                var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/token";

                using (var client = new HttpClient())
                {
                    //var requestParams = new List<KeyValuePair<string, string>>
                    //    {
                    //        new KeyValuePair<string, string>("grant_type", "password"),
                    //        new KeyValuePair<string, string>("username", req.UserName),
                    //        new KeyValuePair<string, string>("password", req.Password)
                    //    };
                    ////var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    ////var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                    ////var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    ////var responseCode = tokenServiceResponse.StatusCode;

                    //var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    //var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                    //var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    //var responseCode = tokenServiceResponse.StatusCode;

                    //if (responseCode == HttpStatusCode.OK)
                    if (true)
                    {
                        //LoginRes res = JsonConvert.DeserializeObject<LoginRes>(responseString);

                        //var users = _context.UserInfoes.Where(u => u.UserName == req.UserName && u.Password == req.Password&&u.RoleId== CustomerRoleId&&u.IsActive==true).FirstOrDefault();
                        var users = _context.VendorInfoes.Where(u => u.UserName == req.UserName && u.Password == req.Password).FirstOrDefault();

                        if (users != null)
                        {
                            //var result = new LoginRes
                            //{
                            //    access_token = res.access_token,
                            //    token_type = res.token_type,
                            //    expires_in = res.expires_in,
                            //    UserInfos = _context.GetUserInfoById(users.FirstOrDefault().Id).FirstOrDefault()
                            //};
                            if(users.StatustypeId == (int)VendorStatus.Approved)
                            {
                                response.Result = new LoginRes
                                {
                                    //access_token = res.access_token,
                                    //token_type = res.token_type,
                                    ////expires_in = res.expires_in,
                                    access_token = null,
                                    token_type = null,
                                    expires_in = 0,
                                    UserInfos = _context.GetUserInfoById(users.Id).FirstOrDefault(),
                                    activityRights = _context.GetActivityRightsByRoleId(users.RoleId).ToList()
                                };
                                response.IsSuccess = true;
                                response.AffectedRecords = 1;
                                response.EndUserMessage = "Login Successful";
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.AffectedRecords = 0;
                                response.EndUserMessage = "Please wait for approval";
                            }
                           

                        }
                        else
                        {

                            response.IsSuccess = false;
                            response.AffectedRecords = 0;
                            response.EndUserMessage = "Invalid UserName or Password";
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.AffectedRecords = 0;
                        response.EndUserMessage = "Invalid UserName or Password";
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;

            }
            return response;
        }
        public async Task<ValueDataResponse<dynamic>> ChangePassword(ChangePassword req)
        {
            ValueDataResponse<dynamic> response = new ValueDataResponse<dynamic>();

            try
            {
                var _user = _context.UserInfoes.Where(u => u.UserId == req.UserId && u.Password == req.OldPassword).FirstOrDefault();

                if (_user != null)
                {
                    IdentityResult result = await _auth.ChangeUserPassword(req);

                    if (result.Succeeded)
                    {
                        _user.Password = req.NewPassword;
                        _context.SaveChanges();
                        response.Result = _user;
                        response.IsSuccess = true;
                        response.AffectedRecords = 1;
                        response.EndUserMessage = "Password changed successfully";
                    }

                    else
                    {
                        response.IsSuccess = false;
                        response.AffectedRecords = 0;
                        response.EndUserMessage = result.Errors.FirstOrDefault() == null ? "Failed" : result.Errors.FirstOrDefault();
                    }
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "Invalid User";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }

            return response;
        }
        public ValueDataResponse<ValidateUser_Result> ValidateUser(string UserName, string Password)
        {
            ValueDataResponse<ValidateUser_Result> response = new ValueDataResponse<ValidateUser_Result>();
            try
            {
                var result = _context.ValidateUser(UserName, Password).FirstOrDefault();
                if (result != null)
                {
                    response.Result = result;
                    response.AffectedRecords = 1;
                    response.IsSuccess = true;
                    response.EndUserMessage = "User Details Found";
                }
                else
                {
                    response.Result = null;
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No User Details Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<UserInfo> ValidateUser(string UserName)
        {
            ValueDataResponse<UserInfo> response = new ValueDataResponse<UserInfo>();
            try
            {
                var result = _context.UserInfoes.Where(x => x.UserName == UserName).FirstOrDefault();
                if (result != null)
                {
                    response.Result = result;
                    response.AffectedRecords = 1;
                    response.IsSuccess = true;
                    response.EndUserMessage = "User Details Found";
                }
                else
                {
                    response.Result = null;
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No User Details Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<GetUserinfoByUserName_Result> GetUserinfoByUserName(string UserName)
        {
            ValueDataResponse<GetUserinfoByUserName_Result> response = new ValueDataResponse<GetUserinfoByUserName_Result>();
            try
            {
                var result = _context.GetUserinfoByUserName(UserName).FirstOrDefault();
                if (result != null)
                {
                    response.Result = result;
                    response.AffectedRecords = 1;
                    response.IsSuccess = true;
                    response.EndUserMessage = "User Details Found";
                }
                else
                {
                    response.Result = null;
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No User Details Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ListDataResponse<GetUsersByRoleId_Result> GetUsersByRoleId(string RoleId)
        {
            ListDataResponse<GetUsersByRoleId_Result> response = new ListDataResponse<GetUsersByRoleId_Result>();

            try
            {
                //response.Result = _unitOfWork.UserRepository.GetByID(Id);
                RoleId = RoleId.ToLower() == "null" ? null : RoleId;
                response.ListResult = _context.GetUsersByRoleId(RoleId).ToList();

                if (response.ListResult != null)
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Get Users details successful";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No data found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }

            return response;
        }


        public ListDataResponse<GetUsersBySearch_Result> GetUsersBySearch(GetUsersBySearchReq req)
        {
            ListDataResponse<GetUsersBySearch_Result> response = new ListDataResponse<GetUsersBySearch_Result>();
            try
            {
               
                var result = _context.GetUsersBySearch(req.Search, req.RoleId).ToList();
                if (result.Count > 0)
                { 
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Users Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = true;
                    response.EndUserMessage = "No User Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }

        public ListDataResponse<dynamic> GetVendorDetailsById(string SearchKey)
        {
            ListDataResponse<dynamic> response = new ListDataResponse<dynamic>();

            try
            {
                SearchKey = string.IsNullOrEmpty(SearchKey) ? null : SearchKey.ToUpper();
                var result = (from f in _context.VendorInfoes
                              //join p in _spContext.Plots on f.Code equals p.FarmerCode
                              //join h in _spContext.FarmerHistories on p.Code equals h.PlotCode
                              //join v in _spContext.Villages on f.VillageId equals v.Id
                              //join m in _spContext.Mandals on f.MandalId equals m.Id
                              where ((string.IsNullOrEmpty(SearchKey) || (f.ContactNumber + (string.IsNullOrEmpty(f.MobileNumber) ? "" : ", " + f.MobileNumber)).Contains(SearchKey)
                                    || (f.FirstName + (string.IsNullOrEmpty(f.MiddleName) ? " " : f.MiddleName + " ") + f.LastName).ToUpper().Contains(SearchKey))) 
                                    
                              select new
                              {
                                  f.Id,
                                  f.FirstName,
                                  f.MiddleName,
                                  f.LastName,
                                  VendorName = f.FirstName + (string.IsNullOrEmpty(f.MiddleName) ? " " : f.MiddleName + " ") + f.LastName,
                                  f.ContactNumber,
                                  f.MobileNumber,
                                  ContactNumbers = f.ContactNumber + (string.IsNullOrEmpty(f.MobileNumber) ? "" : ", " + f.MobileNumber)                                  
                              }).Distinct();

                if (result.Count() > 0)
                {
                    response.ListResult = result;
                    response.AffectedRecords = response.ListResult.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Get Vendor Details Successful";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = true;
                    response.EndUserMessage = "No Vendor Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }

            return response;
        }

        public ListDataResponse<GetVendorInfo_Result> GetVendorInfo(VendorReq req)
        {
            ListDataResponse<GetVendorInfo_Result> response = new ListDataResponse<GetVendorInfo_Result>();
            try
            {
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (req.FromDate != null && req.ToDate != null)
                {
                    FromDate = Convert.ToDateTime(req.FromDate);
                    FromDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(FromDate));
                    FromDate = new DateTime(Convert.ToDateTime(FromDate).Ticks, DateTimeKind.Utc);
                    FromDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(FromDate), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                    ToDate = Convert.ToDateTime(req.ToDate);
                    ToDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(ToDate));
                    ToDate = new DateTime(Convert.ToDateTime(ToDate).Ticks, DateTimeKind.Utc);
                    ToDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(ToDate), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                }
                var result = _context.GetVendorInfo(req.VendorId,req.ServiceTypeId,req.StatusTypeId, FromDate,ToDate).ToList();
                if (result.Count > 0)
                {
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Vendors Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = true;
                    response.EndUserMessage = "No Vendor Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<UpdateVendorStatus> UpdateVendorStatus(UpdateVendorStatus req)
        {
            ValueDataResponse<UpdateVendorStatus> response = new ValueDataResponse<UpdateVendorStatus>();
            try
            {
                DateTime UpdatedDate = Convert.ToDateTime(req.UpdatedDate);
                UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(UpdatedDate));
                UpdatedDate = new DateTime(Convert.ToDateTime(UpdatedDate).Ticks, DateTimeKind.Utc);
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(UpdatedDate), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                ObjectParameter statusCode = new ObjectParameter("statusCode", typeof(int));
                ObjectParameter statusMessage = new ObjectParameter("statusMessage", typeof(string));

                var result = _context.UpdateVendorStatus(req.VendorId,req.StatusTypeId,req.UpdatedById, UpdatedDate, statusCode, statusMessage);
                if (Convert.ToInt32(statusCode.Value) > 0)
                {
                    response.AffectedRecords = 1;
                    response.IsSuccess = true;
                    response.EndUserMessage = statusMessage.Value.ToString();
                }
                else
                {
                    response.Result = null;
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "Vendor Updation Failed";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<UserInfo> UpdateDeviseTokenByUserId(UpdateDeviseToken deviseToken)
        {
            ValueDataResponse<UserInfo> response = new ValueDataResponse<UserInfo>();

            try
            {
                var result = _context.UserInfoes.Where(x => x.Id == deviseToken.UserId).FirstOrDefault();
                if (result != null)
                {
                    result.AccessToken = deviseToken.DeviseToken;
                    _context.SaveChanges();

                }

                if (result != null)
                {
                    response.Result = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Devise Token Updated Succefully";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "User Id Not Found";
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
                //_logger.LogError(ex, "An error occurred while Adding  country");
            }

            return response;
        }

        //public ValueDataResponse<LoginRes> Login(LoginRequest req)
        //{
        //    ValueDataResponse<LoginRes> response = new ValueDataResponse<LoginRes>();

        //    try
        //    {
        //        var users = _context.UserInfoes.Where(u => u.UserName == req.UserName && u.Password == req.Password).FirstOrDefault();

        //        if (users != null)
        //        {
        //            response.Result = new LoginRes
        //            {
        //                //access_token = res.access_token,
        //                //token_type = res.token_type,
        //                //expires_in = res.expires_in,
        //                UserInfos = _context.GetUserInfoById(users.Id).FirstOrDefault(),
        //                activityRights = _context.GetActivityRightsByRoleId(users.RoleId).ToList()
        //            };
        //            response.IsSuccess = true;
        //            response.AffectedRecords = 1;
        //            response.EndUserMessage = "Login Successful";
        //            // return response;
        //        }
        //        else
        //        {
        //            response.IsSuccess = false;
        //            response.AffectedRecords = 0;
        //            response.EndUserMessage = "Invalid UserName or Password";
        //            // return response;
        //        }

        //        // //var tokenServiceUrl = WebConfigurationManager.AppSettings["tokenServiceUrl"].ToString();
        //        //var request = HttpContext.Current.Request;
        //        // var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "token";

        //        // using (var client = new HttpClient())
        //        // {
        //        //     var requestParams = new List<KeyValuePair<string, string>>
        //        //     {
        //        //         new KeyValuePair<string, string>("grant_type", "password"),
        //        //         new KeyValuePair<string, string>("username", req.UserName),
        //        //         new KeyValuePair<string, string>("password", req.Password) 
        //        // //new KeyValuePair<string, string>("client_id", model.client_id),
        //        // //new KeyValuePair<string, string>("client_secret", model.client_secret)
        //        //     };
        //        //     var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
        //        //     var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
        //        //     var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
        //        //     var responseCode = tokenServiceResponse.StatusCode;

        //        //     if (responseCode == HttpStatusCode.OK)
        //        //     {
        //        //         LoginResponse res = JsonConvert.DeserializeObject<LoginResponse>(responseString);
        //        //         dynamic obj = null;
        //        //         dynamic userrole = null;
        //        //         List<GetActivityRightsByRoleId_Result> role = null;

        //        //         var users = _context.UserInfoes.Where(u => u.UserName == req.UserName && u.Password == req.Password).FirstOrDefault();

        //        //         if (users != null)
        //        //         {
        //        //             response.Result = new LoginRes
        //        //             {
        //        //                 //access_token = res.access_token,
        //        //                 //token_type = res.token_type,
        //        //                 //expires_in = res.expires_in,
        //        //                 UserInfos = _context.GetUserInfoById(users.Id).FirstOrDefault(),
        //        //                 role = _context.GetActivityRightsByRoleId(users.RoleId).ToList() 
        //        //         };
        //        //             response.IsSuccess = true;
        //        //             response.AffectedRecords = 1;
        //        //             response.EndUserMessage = "Login Successful";
        //        //         }
        //        //         else
        //        //         {
        //        //             response.IsSuccess = false;
        //        //             response.AffectedRecords = 0;
        //        //             response.EndUserMessage = "Invalid UserName or Password";
        //        //         }
        //        //     }
        //        //     else
        //        //     {
        //        //         response.IsSuccess = false;
        //        //         response.AffectedRecords = 0;
        //        //         response.EndUserMessage = tokenServiceResponse.ReasonPhrase;
        //        //     }
        //        // }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.AffectedRecords = 0;
        //        response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
        //        response.Exception = ex;
        //    }

        //    return response;
        //}
    }
}
