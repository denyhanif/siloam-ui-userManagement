using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;
using System.Reflection;

namespace Siloam.Ui.UserManagement.API_Code.Controller
{
    public class clsXUserHOPE
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataUserHopeID()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_data_userHOPE = new HttpClient();
                http_data_userHOPE.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLOrgHOPE"].ToString());

                http_data_userHOPE.DefaultRequestHeaders.Accept.Clear();
                http_data_userHOPE.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_data_userHOPE.GetAsync(string.Format($"/user"));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataUserHopeID", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataUserHopeID", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                return exx.Message;
            }
        }
    }
}