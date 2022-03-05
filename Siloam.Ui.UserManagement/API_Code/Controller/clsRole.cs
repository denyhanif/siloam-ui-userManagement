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
    public class clsRole
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataRole()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_role = new HttpClient();
                http_data_role.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_role.DefaultRequestHeaders.Accept.Clear();
                http_data_role.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_role.GetAsync(string.Format($"/roleselect"));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataRole", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataRole", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> PostDataRole(Role model_role)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_role);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_postrole = new HttpClient();
                http_postrole.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_postrole.DefaultRequestHeaders.Accept.Clear();
                http_postrole.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_postrole.PostAsync(string.Format($"/roleinsert"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataRole", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataRole", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataRole(Guid role_id, Role model_role)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_role);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putRole = new HttpClient();
                http_putRole.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putRole.DefaultRequestHeaders.Accept.Clear();
                http_putRole.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putRole.PutAsync(string.Format($"/roleupdate/" + role_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "role_id", role_id.ToString(), "PutDataRole", StartTime, "OK", MyUser.GetUsername(), "/" + role_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "role_id", role_id.ToString(), "PutDataRole", StartTime, "ERROR", MyUser.GetUsername(), "/" + role_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataRole_byAppID(Guid application_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_role = new HttpClient();
                http_data_role.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_role.DefaultRequestHeaders.Accept.Clear();
                http_data_role.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_role.GetAsync(string.Format($"/roleselectbyappid/" + application_id));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataRole_byAppID", StartTime, "OK", MyUser.GetUsername(), "/" + application_id.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataRole_byAppID", StartTime, "ERROR", MyUser.GetUsername(), "/" + application_id.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataRole_byRoleName(Guid application_id, string role_name)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_rolename = new HttpClient();
                http_data_rolename.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_rolename.DefaultRequestHeaders.Accept.Clear();
                http_data_rolename.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_rolename.GetAsync(string.Format($"/roleselectbyrolename/" + application_id + "/" + role_name));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataRole_byRoleName", StartTime, "OK", MyUser.GetUsername(), "/" + application_id.ToString() + "/" + role_name, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataRole_byRoleName", StartTime, "ERROR", MyUser.GetUsername(), "/" + application_id.ToString() + "/" + role_name, "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataRole_bySearch(Guid application_id, string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_rolesearch = new HttpClient();
                http_data_rolesearch.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_rolesearch.DefaultRequestHeaders.Accept.Clear();
                http_data_rolesearch.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_rolesearch.GetAsync(string.Format($"/roleselectbysearch/" + application_id + "/" + keyword));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataRole_bySearch", StartTime, "OK", MyUser.GetUsername(), "/" + application_id.ToString() + "/" + keyword, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataRole_bySearch", StartTime, "ERROR", MyUser.GetUsername(), "/" + application_id.ToString() + "/" + keyword, "", exx.Message));
                return exx.Message;
            }
        }
    }
}