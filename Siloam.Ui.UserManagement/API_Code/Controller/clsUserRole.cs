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
    public class clsUserRole
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataUserRole_Exist(Guid user_id, Int64 organization_id, Guid application_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_userrole = new HttpClient();
                http_data_userrole.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_userrole.DefaultRequestHeaders.Accept.Clear();
                http_data_userrole.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_userrole.GetAsync(string.Format($"/userrolecheckexist/" + user_id + "/" + organization_id + "/" + application_id));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "user_id", user_id.ToString(), "GetDataUserRole_Exist", StartTime, "OK", MyUser.GetUsername(), "/" + user_id.ToString() + "/" + organization_id.ToString() + "/" + application_id.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "user_id", user_id.ToString(), "GetDataUserRole_Exist", StartTime, "ERROR", MyUser.GetUsername(), "/" + user_id.ToString() + "/" + organization_id.ToString() + "/" + application_id.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> PostDataUserRole(UserRole model_userrole)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_userrole);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_postuserrole = new HttpClient();
                http_postuserrole.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_postuserrole.DefaultRequestHeaders.Accept.Clear();
                http_postuserrole.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_postuserrole.PostAsync(string.Format($"/userroleinsert"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataUserRole", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataUserRole", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataUserRole(Int64 userrole_id, UserRole model_userrole)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_userrole);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putuserRole = new HttpClient();
                http_putuserRole.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putuserRole.DefaultRequestHeaders.Accept.Clear();
                http_putuserRole.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putuserRole.PutAsync(string.Format($"/userroleupdate/" + userrole_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "userrole_id", userrole_id.ToString(), "PutDataUserRole", StartTime, "OK", MyUser.GetUsername(), "/" + userrole_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "userrole_id", userrole_id.ToString(), "PutDataUserRole", StartTime, "ERROR", MyUser.GetUsername(), "/" + userrole_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataUser_byOrgApp(Int64 OrgID, Guid AppID)
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
                    return await http_data_user.GetAsync(string.Format($"/userroleselectlistuserbyorgandapp/" + OrgID + "/" + AppID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "OrgID", OrgID.ToString(), "GetDataUser_byOrgApp", StartTime, "OK", MyUser.GetUsername(), "/" + OrgID.ToString() + "/" + AppID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "OrgID", OrgID.ToString(), "GetDataUser_byOrgApp", StartTime, "ERROR", MyUser.GetUsername(), "/" + OrgID.ToString() + "/" + AppID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataUserRole_ByUsername(string user_name)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            VarUsername param = new VarUsername();
            param.user_name = user_name;

            var JsonString = JsonConvert.SerializeObject(param);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_data_userrole = new HttpClient();
                http_data_userrole.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_userrole.DefaultRequestHeaders.Accept.Clear();
                http_data_userrole.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_userrole.PutAsync(string.Format($"/userroleselectbyusername"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "user_name", user_name.ToString(), "GetDataUserRole_ByUsername", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "user_name", user_name.ToString(), "GetDataUserRole_ByUsername", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataOrg_byuserid(Guid UserID)
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
                    return await http_data_user.GetAsync(string.Format($"/userroleselectgetorg/" + UserID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "UserID", UserID.ToString(), "GetDataOrg_byuserid", StartTime, "OK", MyUser.GetUsername(), "/" + UserID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "UserID", UserID.ToString(), "GetDataOrg_byuserid", StartTime, "ERROR", MyUser.GetUsername(), "/" + UserID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataMap_byuseridorgid(Guid UserID, Int64 OrgID)
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
                    return await http_data_user.GetAsync(string.Format($"/userroleselectgetapprole/" + UserID + "/" + OrgID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "UserID", UserID.ToString(), "GetDataMap_byuseridorgid", StartTime, "OK", MyUser.GetUsername(), "/" + UserID.ToString() + "/" + OrgID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "UserID", UserID.ToString(), "GetDataMap_byuseridorgid", StartTime, "ERROR", MyUser.GetUsername(), "/" + UserID.ToString() + "/" + OrgID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }
    }
}