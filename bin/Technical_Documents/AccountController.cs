using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using eSales.Service.Utils;
using System.Configuration;

namespace eSales.Service.Controllers
{
    public class AccountController : ApiController
    {

        public class UserInfo
        {
            public string Role { get; set; }
            public string UserName { get; set; }
            public string Token { get; set; }
            public string Name { get; set; }
            public string SlsperID { get; set; }
            public bool IsFirstLogin { get; set; }
            public DateTime LastTimeUpdatePassword { get; set; }
            public List<UserBranch> BranchList { get; set; }
            public List<PPC_GetSetting_Result> SettingList { get; set; }

        }

        public class UserBranch
        {
            public string BranchID { get; set; }
            public string Name { get; set; }
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        [HttpGet]
        [AllowAnonymous]
        public UserInfo Login(string userName, string password, string imei, string localTime)
        {
            return LoginValidate(userName, password, imei, localTime, "vi");
            
        }
        [HttpGet]
        [AllowAnonymous]
        public UserInfo Login(string userName, string password, string imei, string localTime, string langID)
        {
            return LoginValidate(userName, password, imei, localTime, langID);
        }

        private UserInfo LoginValidate(string userName, string password, string imei, string localTime, string langID)
        {
            try
            {
                DateTime localDate = Convert.ToDateTime(localTime);
                DateTime serverDateFrom = DateTime.Now.AddMinutes(-30);
                DateTime serverDateTo = DateTime.Now.AddMinutes(30);
                //Option ValidateTime : dùng để kiểm tra múi giờ SFA và Server
                string validateTime = ConfigurationManager.AppSettings["ValidateTime"] ?? "true";
                if (validateTime.Equals("true") && ( localDate < serverDateFrom || localDate > serverDateTo ) )
                {
                    throw Request.CreateResponse(HttpStatusCode.InternalServerError, "Thời gian trên máy không đúng, thời gian hiện tại trên hệ thống là " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));
                }

                UserInfo info = new UserInfo();
                AppEntities app = DbHelper.CreateObjectContext<AppEntities>(false);

                var user = app.PPC_GetUserInfo(userName).FirstOrDefault();

                if (user == null) throw Request.CreateResponse(HttpStatusCode.Unauthorized, "Sai tài khoản hoặc mật khẩu");
                
                string passDb = user.Password;
                try
                {
                    passDb = Encryption.Decrypt(user.Password, "1210Hq10s081f359t");
                }
                catch
                {
                        
                }
                
                if (passDb != password)
                {
                    throw Request.CreateResponse(HttpStatusCode.Unauthorized, "Sai tài khoản hoặc mật khẩu");
                }

                if (string.IsNullOrEmpty(user.IMEI))
                {
                    throw Request.CreateResponse(HttpStatusCode.InternalServerError, "Tài khoản " + userName + " chưa được cấu hình trên hệ thống vui lòng liên hệ bộ phận kỹ thuật");
                }

                List<PPC_GetSetting_Result> settings = new List<PPC_GetSetting_Result>();

                if (user.Role == "S")
                {
                    settings = app.PPC_GetSetting(userName).ToList();
                }
                else if (user.Role == "U")
                {
                    settings = app.PPC_GetSettingSup(userName).ToList();
                }
                else if (user.Role == "W")
                {
                    settings = app.PPC_GetSettingStock(userName).ToList();
                }
                else if (user.Role == "E")
                {
                    settings = app.PPC_GetSettingCheckCabinet(userName).ToList();
                }
                else if (user.Role == "C")
                {
                    settings = app.PPC_GetSettingCustomer(userName).ToList();
                }
                else if (user.Role == "P")
                {
                    settings = app.PPC_GetSettingPG(userName).ToList();
                }
                else if (user.Role == "D")
                {
                    settings = app.PPC_GetSettingDeliveryV2(userName).ToList();
                }
                else if (user.Role == "DS")
                {
                    settings = app.PPC_GetSettingDistStaff(userName).ToList();
                }
                var checkLicense = settings.FirstOrDefault(p => p.Key.ToUpper() == "CHECK_LICENSE");

                if (checkLicense != null && checkLicense.Value == "1" && user.IMEI != imei)
                {
                    throw Request.CreateResponse(HttpStatusCode.InternalServerError, "Tài khoản " + userName + " không được đăng nhập trên máy này");
                }

                settings.Add(new PPC_GetSetting_Result() { Key = "SERVER_DATE", Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                Token token = new Token(userName, "");
                info.Token = token.TokenID;
                info.Role = user.Role;
                info.UserName = userName;
                info.Name = user.FullName;
                info.SlsperID = user.SlsperID;
                info.IsFirstLogin = (bool)user.IsFirstLogin;
                info.LastTimeUpdatePassword = user.LastTimeUpdatePassword;
                info.BranchList = app.PPC_GetBranch(userName).Select(p => new UserBranch() { BranchID = p.BranchID, Name = p.Name, Lat = p.Lat, Lng = p.Lng }).OrderBy(p => p.BranchID).ToList();
                info.SettingList = settings;
                TokenRepo.AddToken(token);
                return info;
               
               
            }
            catch (Exception ex)
            {
                throw Request.HandleError(ex);
            }

        }

        [HttpPost]
        public bool UpdatePassword(string userName, string branchID, string password)
        {
            try
            {
                UserInfo info = new UserInfo();
                AppEntities app = DbHelper.CreateObjectContext<AppEntities>();
                var user = app.PPC_GetUserInfo(userName).FirstOrDefault();
                List<PPC_GetSetting_Result> settings = new List<PPC_GetSetting_Result>();
                try
                {
                    if (user.Role == "S")
                        settings = app.PPC_GetSetting(userName).ToList();
                    else if (user.Role == "U")
                        settings = app.PPC_GetSettingSup(userName).ToList();
                    else if (user.Role == "W")
                        settings = app.PPC_GetSettingStock(userName).ToList();
                    else if (user.Role == "E")
                        settings = app.PPC_GetSettingCheckCabinet(userName).ToList();
                    else if (user.Role == "C")
                        settings = app.PPC_GetSettingCustomer(userName).ToList();
                    else if (user.Role == "P")
                        settings = app.PPC_GetSettingPG(userName).ToList();
                    else if (user.Role == "D")
                        settings = app.PPC_GetSettingDeliveryV2(userName).ToList();
                    else if (user.Role == "DS")
                        settings = app.PPC_GetSettingDistStaff(userName).ToList();

                    var pass_change_encryption = settings.FirstOrDefault(p => p.Key.ToUpper() == "PASS_CHANGE_ENCRYPTION");

                    if (pass_change_encryption != null && pass_change_encryption.Value == "1" )
                        password = Encryption.Encrypt(password, "1210Hq10s081f359t");
                }
                catch
                {

                }

                if (user != null)
                {
                    if (user.Role == "C")
                        app.PPC_UpdatePasswordCustomer(userName, branchID, password);
                    else
                        app.PPC_UpdatePassword(userName, branchID, password);
                }
                   
                return true;
            }
            catch (Exception ex)
            {
                throw Request.HandleError(ex);
            }
        }
    }
}
