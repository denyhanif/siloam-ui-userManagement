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
    public class clsViewListUser
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataUser(Int64 OrgID, Guid AppID, Guid RoleID)
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
                    return await http_data_user.GetAsync(string.Format($"/userroleselectmappinglistuser/" + OrgID + "/" + AppID + "/" + RoleID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "OrgID", OrgID.ToString(), "GetDataUser", StartTime, "OK", MyUser.GetUsername(), "/" + OrgID.ToString() + "/" + AppID.ToString() + "/" + RoleID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "OrgID", OrgID.ToString(), "GetDataUser", StartTime, "ERROR", MyUser.GetUsername(), "/" + OrgID.ToString() + "/" + AppID.ToString() + "/" + RoleID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }
    }
}