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
    public class clsViewListApps
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataApps(Int64 OrgID)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_apps = new HttpClient();
                http_data_apps.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_data_apps.DefaultRequestHeaders.Accept.Clear();
                http_data_apps.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_apps.GetAsync(string.Format($"/userroleselectmappinglistapps/" + OrgID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "OrgID", OrgID.ToString(), "GetDataApps", StartTime, "OK", MyUser.GetUsername(), "/" + OrgID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "OrgID", OrgID.ToString(), "GetDataApps", StartTime, "ERROR", MyUser.GetUsername(), "/" + OrgID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }
    }
}