using Model; 
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors; 
using Model.Response; 
using Repository.Interface.Settings;
using Repository.Settings;
using Model.Identity;
using System.Web;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using log4net;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
         //IUsers _userRepo = new UserRepository();
        //RupdarshiEntities _ctx = new RupdarshiEntities();
        IUsers _repo = new UsersRepository();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        [Route("Login")]
        public async  Task<ValueDataResponse<dynamic>> Login(LoginRequest req)
        {
           
            var response = new ValueDataResponse<dynamic>();

            if (!ModelState.IsValid)
            {
                (from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, ErrorMessage = string.Join(",", modelstate.Value.Errors.Select(e => e.ErrorMessage)) }).ToList().ForEach(x => response.ValidationErrors.Add(new ValidationItem(x.Title, x.ErrorMessage)));

                response.Result = ModelState;
                response.EndUserMessage = "Bad Request";
                response.AffectedRecords = 0;
                response.IsSuccess = false;
            }
            else
            {
                return await _repo.Login(req);
            }

            return response;
        }

        [HttpPost]
        [Route("CustomerLogin")]
        public async Task<ValueDataResponse<dynamic>> CustomerLogin(LoginRequest req)
        {

            var response = new ValueDataResponse<dynamic>();

            if (!ModelState.IsValid)
            {
                (from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, ErrorMessage = string.Join(",", modelstate.Value.Errors.Select(e => e.ErrorMessage)) }).ToList().ForEach(x => response.ValidationErrors.Add(new ValidationItem(x.Title, x.ErrorMessage)));

                response.Result = ModelState;
                response.EndUserMessage = "Bad Request";
                response.AffectedRecords = 0;
                response.IsSuccess = false;
            }
            else
            {
                return await _repo.CustomerLogin(req);
            }

            return response;
        }


        [Route("GetUserDetails/{UserName}/{Password}")]
        [HttpGet]
        public ValueDataResponse<ValidateUser_Result> GetUserDetails(string UserName, string Password)
        {
            return _repo.ValidateUser(UserName, Password);
        }
        [Route("ValidateUser/{UserName}")]
        [HttpGet]
        public ValueDataResponse<UserInfo> ValidateUser(string UserName) {
            return _repo.ValidateUser(UserName);
        }
        [Route("GetUserInfoById/{Id}")]
        //[Authorize]
        [HttpGet]
        public ListDataResponse<GetUserInfoById_Result> GetUser(int? Id)
        {
            return _repo.GetUser(Id);
        }

        [Route("GetUserInfoByUserName/{UserName}")]
        [HttpGet]
        public ValueDataResponse<GetUserinfoByUserName_Result> GetUserDetails(string UserName)
        {
            return _repo.GetUserinfoByUserName(UserName);
        }
        [HttpPost]
        //[Authorize]
        [Route("AddUpdateUserInfo")]
        public ValueDataResponse<GetUserInfoById_Result> AddUpdateUserInfo(Model.Identity.UserModel user)
        {
            return _repo.AddUpdateUser(user);
        }
        [HttpPost] 
        [Route("Register")]
        public ValueDataResponse<GetVendorInfoById_Result> AddUpdateCustomerInfo(Model.Identity.RegisterModel user)
        {
            return _repo.AddUpdatecustomerInfo(user);
        }
        [Route("UpdateUser")]
        [HttpPost]
        public ValueDataResponse<GetUserInfoById_Result> UpdateUser(Model.Identity.UserModel req)
        {
           return _repo.AddUpdateUser(req);
        }
        //POST api/Account/ChangePassword
        [Route("ChangePassword")]
        //[Authorize]
        [HttpPost]
        public async Task<ValueDataResponse<dynamic>> ChangePassword(ChangePassword changePassword)
        {
            var response = new ValueDataResponse<dynamic>();

            if (!ModelState.IsValid)
            {
                (from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key, ErrorMessage = string.Join(",", modelstate.Value.Errors.Select(e => e.ErrorMessage)) }).ToList().ForEach(x => response.ValidationErrors.Add(new ValidationItem(x.Title, x.ErrorMessage)));

                response.Result = ModelState;
                response.EndUserMessage = "Bad Request";
                response.AffectedRecords = 0;
                response.IsSuccess = false;
            }
            else
            {
                response = await _repo.ChangePassword(changePassword);
            }

            return response;
        }

        [Route("GetUsersByRoleId/{RoleId}")]
        [HttpGet]
        public ListDataResponse<GetUsersByRoleId_Result> GetUsersByRoleId(string RoleId)
        {
            return _repo.GetUsersByRoleId(RoleId);
        }
        [Route("GetUsersBySearch")]
        [HttpPost]
        public ListDataResponse<GetUsersBySearch_Result> GetUsersBySearch(GetUsersBySearchReq req)
        {
            return _repo.GetUsersBySearch(req);
        }
        [Route("UpdateDeviseTokenByUserId")]
        [HttpPost]
        public ValueDataResponse<UserInfo> UpdateDeviseTokenByUserId(UpdateDeviseToken deviseToken)
        {
            return _repo.UpdateDeviseTokenByUserId(deviseToken);
        }

        [Route("GetVendorInfo")]
        [HttpPost]
        public ListDataResponse<GetVendorInfo_Result> GetVendorInfo(VendorReq req)
        {
            return _repo.GetVendorInfo(req);
        }

        [Route("GetVendorDetailsById/{SearchKey}")]
        [HttpGet]
        public ListDataResponse<dynamic> GetVendorDetailsById(string SearchKey)
        {
            return _repo.GetVendorDetailsById(SearchKey);
        }
        [Route("UpdateVendorStatus")]
        [HttpPost]
        public ValueDataResponse<UpdateVendorStatus> UpdateVendorStatus(UpdateVendorStatus req)
        {
            return _repo.UpdateVendorStatus(req);
        }

        [Route("ExportVendorData")]
        //[Authorize]
        [HttpPost]
        public HttpResponseMessage ExportUserInfo(List<GetVendorInfo_Result> result)
        {
            Stream stream = null;
            try
            {
                var iRowCnt = 2;
                using (ExcelPackage excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
                {
                    excelPackage.Workbook.Properties.Title = "Vendor Details";

                    excelPackage.Workbook.Worksheets.Add("Vendor Details");
                    var worksheet = excelPackage.Workbook.Worksheets[1];
                    using (var range = worksheet.Cells["A1:K1"])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.Orange);
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Left.Color.SetColor(Color.Black);
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Right.Color.SetColor(Color.Black);
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Bottom.Color.SetColor(Color.Black);
                        range.Style.Font.Color.SetColor(Color.Black);
                        range.Style.Font.SetFromFont(new Font("Calibri", 12));
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.Font.Bold = true;
                    }
                    worksheet.Cells[iRowCnt - 1, 1].Value = "Vendor Name";
                    worksheet.Cells[iRowCnt - 1, 2].Value = "Mobile Number";
                    worksheet.Cells[iRowCnt - 1, 3].Value = "Status";
                    worksheet.Cells[iRowCnt - 1, 4].Value = "Business Name";
                    worksheet.Cells[iRowCnt - 1, 5].Value = "GSTIN";
                    worksheet.Cells[iRowCnt - 1, 6].Value = "Service Type";
                    worksheet.Cells[iRowCnt - 1, 7].Value = "Regstered Date";
                    worksheet.Cells[iRowCnt - 1, 8].Value = "State";
                    worksheet.Cells[iRowCnt - 1, 9].Value = "City";
                    worksheet.Cells[iRowCnt - 1, 10].Value = "Address";
                    worksheet.Cells[iRowCnt - 1, 11].Value = "Pincode";


                    int i;
                    for (i = 0; i <= result.Count - 1; i++)
                    {
                        worksheet.Cells[iRowCnt, 1].Value = result[i].VendorName;
                        worksheet.Cells[iRowCnt, 2].Value = result[i].MobileNumber;
                        worksheet.Cells[iRowCnt, 3].Value = result[i].StatusType;
                        worksheet.Cells[iRowCnt, 4].Value = result[i].BusinessName;
                        worksheet.Cells[iRowCnt, 5].Value = result[i].GSTIN;
                        worksheet.Cells[iRowCnt, 6].Value = result[i].ServiceType;
                        worksheet.Cells[iRowCnt, 7].Value = result[i].CreatedDate;
                        worksheet.Cells[iRowCnt, 7].Style.Numberformat.Format = @"dd-MM-yyyy";
                        worksheet.Cells[iRowCnt, 8].Value = result[i].State;
                        worksheet.Cells[iRowCnt, 9].Value = result[i].City;
                        worksheet.Cells[iRowCnt, 10].Value = result[i].Address;
                        worksheet.Cells[iRowCnt, 11].Value = result[i].Pincode;

                        iRowCnt = iRowCnt + 1;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, excelPackage.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.InnerException.Message);

            }
        }
        #region CommentedCode
        //[HttpPost]
        //[Route("Login")]
        //public  ValueDataResponse<LoginRes>  Login(LoginRequest req)
        //{
        //    return _repo.Login(req);
        //}
        //[HttpPost]
        //[Route("CLogin")]
        //public async Task<HttpResponseMessage> CLogin(LoginRequest req)
        //{
        //    LoginRes response = new LoginRes();

        //    try
        //    {
        //        //var tokenServiceUrl = ConfigurationManager.AppSettings["tokenServiceUrl"].ToString();
        //        var request = HttpContext.Current.Request;
        //        var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/token";

        //        using (var client = new HttpClient())
        //        {
        //            var requestParams = new List<KeyValuePair<string, string>>
        //            {
        //                new KeyValuePair<string, string>("grant_type", "password"),
        //                new KeyValuePair<string, string>("username", req.UserName),
        //                new KeyValuePair<string, string>("password", req.Password)
        //            };
        //            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
        //            var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
        //            var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
        //            var responseCode = tokenServiceResponse.StatusCode;

        //            if (responseCode == HttpStatusCode.OK)
        //            {
        //                LoginResponse res = JsonConvert.DeserializeObject<LoginResponse>(responseString);

        //                var users = _ctx.UserInfoes.Where(u => u.UserName == req.UserName && u.Password == req.Password);

        //                if (users != null)
        //                {
        //                    var result = new LoginRes
        //                    {
        //                        access_token = res.access_token,
        //                        token_type = res.token_type,
        //                        expires_in = res.expires_in,
        //                        UserInfos = _ctx.GetUserInfoById(users.FirstOrDefault().Id).FirstOrDefault()
        //                    };
        //                    return Request.CreateResponse(HttpStatusCode.OK, result);
        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest);
        //                }

        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid UserName & Password");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
        //    }
        //}
        #endregion

    }
}
