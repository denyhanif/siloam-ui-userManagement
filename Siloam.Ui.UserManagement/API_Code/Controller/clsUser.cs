using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Siloam.Ui.UserManagement.API_Code.Models;
using System.Text;
using log4net;
using System.Reflection;


namespace Siloam.Ui.UserManagement.API_Code.Controller
{
    public class clsUser
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<string> GetDataUser()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_user = new HttpClient();
                http_data_user.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_user.DefaultRequestHeaders.Accept.Clear();
                http_data_user.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_user.GetAsync(string.Format($"/userselect"));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataUser", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataUser", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataUserByOrg(string umsorgids)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_user = new HttpClient();
                http_data_user.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_user.DefaultRequestHeaders.Accept.Clear();
                http_data_user.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_user.GetAsync(string.Format($"/userselectbyorg/" + umsorgids));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataUser", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataUser", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> PostDataUser(ViewUserSIP model_user)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_user);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_postuser = new HttpClient();
                http_postuser.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_postuser.DefaultRequestHeaders.Accept.Clear();
                http_postuser.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_postuser.PostAsync(string.Format($"/userinsert"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataUser", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataUser", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataUser(Guid user_id, User model_user)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_user);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putuser = new HttpClient();
                http_putuser.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putuser.DefaultRequestHeaders.Accept.Clear();
                http_putuser.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putuser.PutAsync(string.Format($"/userupdate/" + user_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "user_id", user_id.ToString(), "PutDataUser", StartTime, "OK", MyUser.GetUsername(), "/" + user_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "user_id", user_id.ToString(), "PutDataUser", StartTime, "ERROR", MyUser.GetUsername(), "/" + user_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataUserSIP(Guid user_id, ViewUserSIP model_user)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_user);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putuser = new HttpClient();
                http_putuser.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putuser.DefaultRequestHeaders.Accept.Clear();
                http_putuser.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putuser.PutAsync(string.Format($"/usersipupdate/" + user_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "user_id", user_id.ToString(), "PutDataUserSIP", StartTime, "OK", MyUser.GetUsername(), "/" + user_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "user_id", user_id.ToString(), "PutDataUserSIP", StartTime, "ERROR", MyUser.GetUsername(), "/" + user_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataUserByUsername(string username)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            VarUsername param = new VarUsername();
            param.user_name = username;

            var JsonString = JsonConvert.SerializeObject(param);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_data_user = new HttpClient();
                http_data_user.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_user.DefaultRequestHeaders.Accept.Clear();
                http_data_user.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_user.PutAsync(string.Format($"/userselectbyusername"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "GetDataUserByUsername", StartTime, "OK", username, "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "GetDataUserByUsername", StartTime, "ERROR", username, "", JsonString, exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataUserByHopeID(Int64 hopeID)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_userbyHopeiD = new HttpClient();
                http_data_userbyHopeiD.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_userbyHopeiD.DefaultRequestHeaders.Accept.Clear();
                http_data_userbyHopeiD.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_userbyHopeiD.GetAsync(string.Format($"/userselectbyhopeid/" + hopeID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "hopeID", hopeID.ToString(), "GetDataUserByHopeID", StartTime, "OK", MyUser.GetUsername(), "/" + hopeID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "hopeID", hopeID.ToString(), "GetDataUserByHopeID", StartTime, "ERROR", MyUser.GetUsername(), "/" + hopeID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> ChangePasswordUser(string username, string oldpass, string newpass, string modifiedby)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            ParamChangePass param = new ParamChangePass();
            param.user_name = username;
            param.old_pass = oldpass;
            param.new_pass = newpass;
            param.modified_by = modifiedby;

            var JsonString = JsonConvert.SerializeObject(param);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                //string apicentralums = "http://10.85.129.91:8500"; //untuk persiapan ganti uri base address
                HttpClient http_putuser = new HttpClient();
                http_putuser.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putuser.DefaultRequestHeaders.Accept.Clear();
                http_putuser.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putuser.PutAsync(string.Format($"/userupdatechangepassword"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "ChangePasswordUser", StartTime, "OK", username, "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "ChangePasswordUser", StartTime, "ERROR", username, "", JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataUserBySearch(string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_user = new HttpClient();
                http_data_user.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_user.DefaultRequestHeaders.Accept.Clear();
                http_data_user.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_user.GetAsync(string.Format($"/userselectsearchuser/" + keyword));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "keyword", keyword.ToString(), "GetDataUserBySearch", StartTime, "OK", MyUser.GetUsername(), "/" + keyword.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "keyword", keyword.ToString(), "GetDataUserBySearch", StartTime, "ERROR", MyUser.GetUsername(), "/" + keyword.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataUserBySearchByOrg(string umsorgids, string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_user = new HttpClient();
                http_data_user.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_user.DefaultRequestHeaders.Accept.Clear();
                http_data_user.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_user.GetAsync(string.Format($"/userselectsearchuserbyorg/" + umsorgids + "/" + keyword));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "keyword", keyword.ToString(), "GetDataUserBySearch", StartTime, "OK", MyUser.GetUsername(), "/" + keyword.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "keyword", keyword.ToString(), "GetDataUserBySearch", StartTime, "ERROR", MyUser.GetUsername(), "/" + keyword.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataUserForgotPass(string username, string email)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            ParamForgotPass param = new ParamForgotPass();
            param.user_name = username;
            param.email = email;

            var JsonString = JsonConvert.SerializeObject(param);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                //string apicentralums = "http://10.85.129.91:8500"; //untuk persiapan ganti uri base address
                HttpClient http_data_userForgot = new HttpClient();
                http_data_userForgot.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_userForgot.DefaultRequestHeaders.Accept.Clear();
                http_data_userForgot.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_userForgot.PutAsync(string.Format($"/userselectbyforgotpassword"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "GetDataUserForgotPass", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "GetDataUserForgotPass", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> ReleaseUserLock(string username)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            VarUsername param = new VarUsername();
            param.user_name = username;

            var JsonString = JsonConvert.SerializeObject(param);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                //string apicentralums = "http://10.85.129.91:8500"; //untuk persiapan ganti uri base address
                HttpClient http_putuser_release = new HttpClient();
                http_putuser_release.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putuser_release.DefaultRequestHeaders.Accept.Clear();
                http_putuser_release.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putuser_release.PutAsync(string.Format($"/userupdatereleaseuserlock"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "ReleaseUserLock", StartTime, "OK", username, "/" + MyUser.GetUsername(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "ReleaseUserLock", StartTime, "ERROR", username, "/" + MyUser.GetUsername(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataSIPByUserId(Guid UserId)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_org = new HttpClient();
                http_data_org.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_org.DefaultRequestHeaders.Accept.Clear();
                http_data_org.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_org.GetAsync(string.Format($"/sipbyuser/" + UserId));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "UserId", UserId.ToString(), "GetDataSIPByUserId", StartTime, "OK", MyUser.GetUsername(), "/" + MyUser.GetUsername(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "UserId", UserId.ToString(), "GetDataSIPByUserId", StartTime, "ERROR", MyUser.GetUsername(), "/" + MyUser.GetUsername(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> ResetPasswordUserGlobal(string username, string modifby)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            try
            {
                //string apicentralums = "http://10.85.129.91:8500"; //untuk persiapan ganti uri base address
                HttpClient http_resetpass = new HttpClient();
                http_resetpass.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_resetpass.DefaultRequestHeaders.Accept.Clear();
                http_resetpass.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_resetpass.PutAsync(string.Format($"/userupdateresetpassword/" + username + "/" + modifby), null);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "ResetPasswordUserGlobal", StartTime, "OK", MyUser.GetUsername(), "/" + username.ToString() + "/" + modifby, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username.ToString(), "ResetPasswordUserGlobal", StartTime, "ERROR", MyUser.GetUsername(), "/" + username.ToString() + "/" + modifby, "", ex.Message));
                return ex.Message;
            }
        }

        //function dibawah belum terpakai
        public static async Task<string> ResetPasswordUser(string username, string newpass, string modifby, DateTime modifdate)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<User> ListUserData = new List<User>();
            var Get_User = GetDataUserByUsername(username);
            var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

            User model_user = Get_DataUser.list[0];

            var JsonString = JsonConvert.SerializeObject(model_user);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_resetpass = new HttpClient();
                http_resetpass.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_resetpass.DefaultRequestHeaders.Accept.Clear();
                http_resetpass.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_resetpass.PutAsync(string.Format($"/userupdateresetpasswordlocal/" + username + "/" + newpass + "/" + modifby + "/" + modifdate), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username, "ResetPasswordUser", StartTime, "OK", MyUser.GetUsername(), "/" + username.ToString() + "/" + newpass + "/" + modifby + "/" + modifdate.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", username, "ResetPasswordUser", StartTime, "ERROR", MyUser.GetUsername(), "/" + username.ToString() + "/" + newpass + "/" + modifby + "/" + modifdate.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }
    }
}