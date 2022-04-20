using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Model;
using Model.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Repository.Identity
{
    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;
        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public IdentityResult RegisterUser(string UserName,string Password)
        {
            IdentityUser user=new IdentityUser();
            IdentityResult result=new IdentityResult();
            try
            {
                user = new IdentityUser
                {
                    UserName = UserName
                };

                result = _userManager.Create(user, Password);
                
                if (!result.Succeeded)
                {
                    user.Id = null;
                }
                //else
                //{
                //    var authKey = ConfigurationManager.AppSettings["SMS_AuthKey"];
                //    var senderId = ConfigurationManager.AppSettings["SenderId"];
                //    var routeId = ConfigurationManager.AppSettings["RouteId"];
                //    string Message = "Welcome To Water Supply Mangement !Your Id:" + user.UserName + ";Pwd:" + userModel.Password + ".";// Link for Mobile App https://play.google.com/store/apps/details?id=com.calib.fis&hl=en";
                //    string smsResponse = GetPageContent("http://bulksms.bulksmsvalue.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=" + authKey + "&message=" + Message + "&senderId=" + senderId + "&routeId=" + routeId + "&mobileNos=" + userModel.ContactNumber);//x.Phone.Replace("+", ""));
                //    var res = JsonConvert.DeserializeObject<smsResponse>(smsResponse);
                //}
            }
            catch(Exception ex)
            {
                user.Id = null;
            }

            return result;
        }
        private static string GetPageContent(string FullUri)
        {
            HttpWebRequest Request;
            StreamReader ResponseReader;
            Request = ((HttpWebRequest)(WebRequest.Create(FullUri)));
            ResponseReader = new StreamReader(Request.GetResponse().GetResponseStream());
            return ResponseReader.ReadToEnd();
        }
        public async Task<IdentityResult> ChangeUserPassword(ChangePassword changePassword)
        {
            string userId = changePassword.UserId;
            string oldPassword = changePassword.OldPassword;
            string newPassword = changePassword.NewPassword;
            IdentityResult result = await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);

           // IdentityUser user = await _userManager.FindAsync(userName, password);

            return result;
        }

        public async Task<IdentityResult> ResetPassword(ResetPassword resetPassword)
        {
            string userId = resetPassword.UserId;
            string newPassword = resetPassword.NewPassword;

            var provider = new DpapiDataProtectionProvider("ASP.NET IDENTITY");

            _userManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(
                provider.Create("ResetPasswordPurpose"));

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(userId);
            IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(userId, resetToken, newPassword);

            return passwordChangeResult;

        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public Model.Identity.Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(Model.Identity.RefreshToken token)
        {

            var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken.ToString());
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(Model.Identity.RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<Model.Identity.RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<Model.Identity.RefreshToken> GetAllRefreshTokens()
        {
            return _ctx.RefreshTokens.ToList();
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;

        }
        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}
