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
    public class clsRoleAccess
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataRoleAcces_Exist(Guid role_id, Guid page_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_roleaccess = new HttpClient();
                http_data_roleaccess.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_roleaccess.DefaultRequestHeaders.Accept.Clear();
                http_data_roleaccess.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_roleaccess.GetAsync(string.Format($"/roleaccesscheckexist/" + role_id + "/" + page_id));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "app_id", role_id.ToString(), "GetDataRoleAcces_Exist", StartTime, "OK", MyUser.GetUsername(), "/" + role_id.ToString() + "/" + page_id.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "app_id", role_id.ToString(), "GetDataRoleAcces_Exist", StartTime, "ERROR", MyUser.GetUsername(), "/" + role_id.ToString() + "/" + page_id.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> PostDataRoleAccess(RoleAccess model_roleaccess)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_roleaccess);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_postroleaccess = new HttpClient();
                http_postroleaccess.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_postroleaccess.DefaultRequestHeaders.Accept.Clear();
                http_postroleaccess.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_postroleaccess.PostAsync(string.Format($"/roleaccessinsert"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataRoleAccess", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataRoleAccess", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataRoleAccess(Int64 roleaccess_id, RoleAccess model_roleaccess)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_roleaccess);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putRoleAccess = new HttpClient();
                http_putRoleAccess.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putRoleAccess.DefaultRequestHeaders.Accept.Clear();
                http_putRoleAccess.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putRoleAccess.PutAsync(string.Format($"/roleaccessupdate/" + roleaccess_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "roleaccess_id", roleaccess_id.ToString(), "PutDataRoleAccess", StartTime, "OK", MyUser.GetUsername(), "/" + roleaccess_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "roleaccess_id", roleaccess_id.ToString(), "PutDataRoleAccess", StartTime, "ERROR", MyUser.GetUsername(), "/" + roleaccess_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }
    }
}