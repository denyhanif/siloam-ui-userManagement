using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

namespace Siloam.Ui.UserManagement.API_Code.Controller
{
    public class clsViewListRole
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //this method for user mapping
        public static async Task<string> GetDataRole(Int64 OrgID, Guid AppID)
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
                    return await http_data_role.GetAsync(string.Format($"/userroleselectmappinglistrole/" + OrgID + "/" + AppID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "OrgID", OrgID.ToString(), "GetDataRole", StartTime, "OK", MyUser.GetUsername(), "/" + OrgID.ToString() + "/" + AppID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "OrgID", OrgID.ToString(), "GetDataRole", StartTime, "ERROR", MyUser.GetUsername(), "/" + OrgID.ToString() + "/" + AppID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        //this method for page mapping
        public static async Task<string> GetDataRole_forPage(Guid AppID)
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
                    return await http_data_role.GetAsync(string.Format($"/pageroleselectmappinglistrole/" + AppID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "AppID", AppID.ToString(), "GetDataRole_forPage", StartTime, "OK", MyUser.GetUsername(), "/" + AppID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "AppID", AppID.ToString(), "GetDataRole_forPage", StartTime, "ERROR", MyUser.GetUsername(), "/" + AppID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }
    }
}