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
    public class clsApplication
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataApplication()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_app = new HttpClient();
                http_data_app.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_app.DefaultRequestHeaders.Accept.Clear();
                http_data_app.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_app.GetAsync(string.Format($"/applicationselect"));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataApplication", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataApplication", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> PostDataApplication(Application model_app)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_app);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_postapp = new HttpClient();
                http_postapp.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_postapp.DefaultRequestHeaders.Accept.Clear();
                http_postapp.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_postapp.PostAsync(string.Format($"/applicationinsert"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataApplication", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataApplication", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataApplication(Guid app_id, Application model_app)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_app);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putApp = new HttpClient();
                http_putApp.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putApp.DefaultRequestHeaders.Accept.Clear();
                http_putApp.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putApp.PutAsync(string.Format($"/applicationupdate/" + app_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "app_id", app_id.ToString(), "PutDataApplication", StartTime, "OK", MyUser.GetUsername(), "/" + app_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "app_id", app_id.ToString(), "PutDataApplication", StartTime, "ERROR", MyUser.GetUsername(), "/" + app_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataApplicationbyAppName(string application_name)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_appbyAppname = new HttpClient();
                http_data_appbyAppname.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_appbyAppname.DefaultRequestHeaders.Accept.Clear();
                http_data_appbyAppname.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_appbyAppname.GetAsync(string.Format($"/applicationselectbyappname/" + application_name));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_name", application_name, "GetDataApplicationbyAppName", StartTime, "OK", MyUser.GetUsername(), "/" + application_name, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_name", application_name, "GetDataApplicationbyAppName", StartTime, "ERROR", MyUser.GetUsername(), "/" + application_name, "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataApplicationbySearch(string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_appbySearch = new HttpClient();
                http_data_appbySearch.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_appbySearch.DefaultRequestHeaders.Accept.Clear();
                http_data_appbySearch.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_appbySearch.GetAsync(string.Format($"/applicationselectbysearch/" + keyword));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "keyword", keyword, "GetDataApplicationbySearch", StartTime, "OK", MyUser.GetUsername(), "/" + keyword, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "keyword", keyword, "GetDataApplicationbySearch", StartTime, "ERROR", MyUser.GetUsername(), "/" + keyword, "", exx.Message));
                return exx.Message;
            }
        }
    }
}