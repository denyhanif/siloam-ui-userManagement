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
    public class clsPage
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataPage()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_page = new HttpClient();
                http_data_page.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_page.DefaultRequestHeaders.Accept.Clear();
                http_data_page.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_page.GetAsync(string.Format($"/pageselect"));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataPage", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataPage", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> PostDataPage(Pagee model_page)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_page);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_postpage = new HttpClient();
                http_postpage.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_postpage.DefaultRequestHeaders.Accept.Clear();
                http_postpage.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_postpage.PostAsync(string.Format($"/pageinsert"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataPage", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataPage", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataPage(Guid page_id, Pagee model_page)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_page);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putPage = new HttpClient();
                http_putPage.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putPage.DefaultRequestHeaders.Accept.Clear();
                http_putPage.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putPage.PutAsync(string.Format($"/pageupdate/" + page_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "page_id", page_id.ToString(), "PutDataPage", StartTime, "OK", MyUser.GetUsername(), "/" + page_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "page_id", page_id.ToString(), "PutDataPage", StartTime, "ERROR", MyUser.GetUsername(), "/" + page_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataPage_byAppID(Guid application_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_page = new HttpClient();
                http_data_page.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_page.DefaultRequestHeaders.Accept.Clear();
                http_data_page.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_page.GetAsync(string.Format($"/pageselectbyappid/" + application_id));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataPage_byAppID", StartTime, "OK", MyUser.GetUsername(), "/" + application_id.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataPage_byAppID", StartTime, "ERROR", MyUser.GetUsername(), "/" + application_id.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataPage_byPageName(Guid application_id, string page_name)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_pagename = new HttpClient();
                http_data_pagename.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_pagename.DefaultRequestHeaders.Accept.Clear();
                http_data_pagename.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_pagename.GetAsync(string.Format($"/pageselectbypagename/" + application_id + "/" + page_name));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataPage_byPageName", StartTime, "OK", MyUser.GetUsername(), "/" + application_id.ToString() + "/" + page_name, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataPage_byPageName", StartTime, "ERROR", MyUser.GetUsername(), "/" + application_id.ToString() + "/" + page_name, "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataPage_bySearch(Guid application_id, string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_pageserach = new HttpClient();
                http_data_pageserach.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_pageserach.DefaultRequestHeaders.Accept.Clear();
                http_data_pageserach.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_pageserach.GetAsync(string.Format($"/pageselectbysearch/" + application_id + "/" + keyword));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataPage_bySearch", StartTime, "OK", MyUser.GetUsername(), "/" + application_id.ToString() + "/" + keyword, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "application_id", application_id.ToString(), "GetDataPage_bySearch", StartTime, "ERROR", MyUser.GetUsername(), "/" + application_id.ToString() + "/" + keyword, "", exx.Message));
                return exx.Message;
            }
        }
    }
}