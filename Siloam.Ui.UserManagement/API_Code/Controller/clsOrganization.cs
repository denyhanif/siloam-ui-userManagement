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
    public class clsOrganization
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataOrganization()
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
                    return await http_data_org.GetAsync(string.Format($"/organizationselect"));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataOrganization", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataOrganization", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> PostDataOrganization(Organization model_org)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_org);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_postorg = new HttpClient();
                http_postorg.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_postorg.DefaultRequestHeaders.Accept.Clear();
                http_postorg.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_postorg.PostAsync(string.Format($"/organizationinsert"), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataOrganization", StartTime, "OK", MyUser.GetUsername(), "", JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "PostDataOrganization", StartTime, "ERROR", MyUser.GetUsername(), "", JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataOrganization(Int64 org_id, Organization model_org)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_org);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putOrg = new HttpClient();
                http_putOrg.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putOrg.DefaultRequestHeaders.Accept.Clear();
                http_putOrg.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putOrg.PutAsync(string.Format($"/organizationupdate/" + org_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "org_id", org_id.ToString(), "PutDataOrganization", StartTime, "OK", MyUser.GetUsername(), "/" + org_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "org_id", org_id.ToString(), "PutDataOrganization", StartTime, "ERROR", MyUser.GetUsername(), "/" + org_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataOrganizationbyOrgName(string organization_name)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_orgbyorgname = new HttpClient();
                http_data_orgbyorgname.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_orgbyorgname.DefaultRequestHeaders.Accept.Clear();
                http_data_orgbyorgname.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_orgbyorgname.GetAsync(string.Format($"/organizationselectbyorgname/" + organization_name));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "organization_name", organization_name, "GetDataApplication", StartTime, "OK", MyUser.GetUsername(), "/" + organization_name, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "organization_name", organization_name, "GetDataApplication", StartTime, "ERROR", MyUser.GetUsername(), "/" + organization_name, "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataOrganizationbyHopeID(Int64? hope_org_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_orgbyhopeID = new HttpClient();
                http_data_orgbyhopeID.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_orgbyhopeID.DefaultRequestHeaders.Accept.Clear();
                http_data_orgbyhopeID.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_orgbyhopeID.GetAsync(string.Format($"/organizationselectbyhopeid/" + hope_org_id));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "hope_org_id", hope_org_id != null ? hope_org_id.Value.ToString() : "", "GetDataOrganizationbyHopeID", StartTime, "OK", MyUser.GetUsername(), "/" + hope_org_id != null ? hope_org_id.Value.ToString() : "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "hope_org_id", hope_org_id != null ? hope_org_id.Value.ToString() : "", "GetDataOrganizationbyHopeID", StartTime, "ERROR", MyUser.GetUsername(), "/" + hope_org_id != null ? hope_org_id.Value.ToString() : "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataOrganizationbyMobileID(Guid? mobile_org_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_orgbymobileID = new HttpClient();
                http_data_orgbymobileID.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_orgbymobileID.DefaultRequestHeaders.Accept.Clear();
                http_data_orgbymobileID.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_orgbymobileID.GetAsync(string.Format($"/organizationselectbymobileid/" + mobile_org_id));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "mobile_org_id", mobile_org_id != null?mobile_org_id.Value.ToString():"", "GetDataOrganizationbyMobileID", StartTime, "OK", MyUser.GetUsername(), "/" + mobile_org_id != null ? mobile_org_id.Value.ToString() : "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "mobile_org_id", mobile_org_id != null ? mobile_org_id.Value.ToString() : "", "GetDataOrganizationbyMobileID", StartTime, "ERROR", MyUser.GetUsername(), "/" + mobile_org_id != null ? mobile_org_id.Value.ToString() : "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataOrganizationbyAxID(string ax_org_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_orgbyaxID = new HttpClient();
                http_data_orgbyaxID.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_orgbyaxID.DefaultRequestHeaders.Accept.Clear();
                http_data_orgbyaxID.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_orgbyaxID.GetAsync(string.Format($"/organizationselectbyaxid/" + ax_org_id));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "ax_org_id", ax_org_id, "GetDataOrganizationbyAxID", StartTime, "OK", MyUser.GetUsername(), "/" + ax_org_id, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "ax_org_id", ax_org_id, "GetDataOrganizationbyAxID", StartTime, "ERROR", MyUser.GetUsername(), "/" + ax_org_id, "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataOrganizationbySearch(string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_orgbysearch= new HttpClient();
                http_data_orgbysearch.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_orgbysearch.DefaultRequestHeaders.Accept.Clear();
                http_data_orgbysearch.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_orgbysearch.GetAsync(string.Format($"/organizationselectbysearch/" + keyword));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "keyword", keyword, "GetDataOrganizationbySearch", StartTime, "OK", MyUser.GetUsername(), "/" + keyword, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "keyword", keyword, "GetDataOrganizationbySearch", StartTime, "ERROR", MyUser.GetUsername(), "/" + keyword, "", exx.Message));
                return exx.Message;
            }
        }
    }
}