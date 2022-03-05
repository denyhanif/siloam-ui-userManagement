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
    public class clsViewListPage
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //this method for page mapping
        public static async Task<string> GetDataPage(Guid RoleId, Guid AppID)
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
                    return await http_data_page.GetAsync(string.Format($"/pageroleselectmappinglistpage/" + RoleId + "/" + AppID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "RoleId", RoleId.ToString(), "GetDataPage", StartTime, "OK", MyUser.GetUsername(), "/" + RoleId.ToString() + "/" + AppID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "RoleId", RoleId.ToString(), "GetDataPage", StartTime, "ERROR", MyUser.GetUsername(), "/" + RoleId.ToString() + "/" + AppID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }
    }
}