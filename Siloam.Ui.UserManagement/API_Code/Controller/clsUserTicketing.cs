using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Siloam.Ui.UserManagement.API_Code.Models;
using log4net;
using System.Reflection;

namespace Siloam.Ui.UserManagement.API_Code.Controller
{
    public class clsUserTicketing
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static async Task<string> GetDataTicketCount()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_ticket_count = new HttpClient();
                http_ticket_count.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_ticket_count.DefaultRequestHeaders.Accept.Clear();
                http_ticket_count.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_ticket_count.GetAsync(string.Format($"/userticketinggetoutstandingticket"));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataTicketCount", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataTicketCount", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataTicketByOrgandStatus(Int64 orgID, bool isrejected, bool isvalidated)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_ticket_detail = new HttpClient();
                http_ticket_detail.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_ticket_detail.DefaultRequestHeaders.Accept.Clear();
                http_ticket_detail.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_ticket_detail.GetAsync(string.Format($"/userticketinggetoutstandingticketbyorg/" + orgID + "/" + isrejected + "/" + isvalidated));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "app_id", orgID.ToString(), "GetDataTicketByOrgandStatus", StartTime, "OK", MyUser.GetUsername(), "/" + orgID.ToString() + "/" + isrejected.ToString() + "/" + isvalidated.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "app_id", orgID.ToString(), "GetDataTicketByOrgandStatus", StartTime, "ERROR", MyUser.GetUsername(), "/" + orgID.ToString() + "/" + isrejected.ToString() + "/" + isvalidated.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> GetDataTicketAllStatus(Int64 orgID)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_ticket_detail = new HttpClient();
                http_ticket_detail.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_ticket_detail.DefaultRequestHeaders.Accept.Clear();
                http_ticket_detail.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_ticket_detail.GetAsync(string.Format($"/userticketinggetallticketstatus/" + orgID));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "app_id", orgID.ToString(), "GetDataTicketAllStatus", StartTime, "OK", MyUser.GetUsername(), "/" + orgID.ToString(), "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "app_id", orgID.ToString(), "GetDataTicketAllStatus", StartTime, "ERROR", MyUser.GetUsername(), "/" + orgID.ToString(), "", exx.Message));
                return exx.Message;
            }
        }

        public static async Task<string> PutDataUserTicketing(Int64 userTicketing_id, UserTicketing model_ticket)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = JsonConvert.SerializeObject(model_ticket);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putTicket = new HttpClient();
                http_putTicket.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putTicket.DefaultRequestHeaders.Accept.Clear();
                http_putTicket.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putTicket.PutAsync(string.Format($"/userticketingupdate/" + userTicketing_id), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "userTicketing_id", userTicketing_id.ToString(), "PutDataUserTicketing", StartTime, "OK", MyUser.GetUsername(), "/" + userTicketing_id.ToString(), JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "userTicketing_id", userTicketing_id.ToString(), "PutDataUserTicketing", StartTime, "ERROR", MyUser.GetUsername(), "/" + userTicketing_id.ToString(), JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> PutDataUpdateStatus(long userTicketing_id, string remark, bool isreject, string rejectby, bool isvalid, string validby)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var JsonString = "";
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            try
            {
                HttpClient http_putTicketStatus = new HttpClient();
                http_putTicketStatus.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_putTicketStatus.DefaultRequestHeaders.Accept.Clear();
                http_putTicketStatus.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_putTicketStatus.PutAsync(string.Format($"/userticketingupdatestatusticket/" + userTicketing_id + "/" + remark + "/" + isreject  + "/" + rejectby + "/" + isvalid + "/" + validby), content);
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "userTicketing_id", userTicketing_id.ToString(), "PutDataUpdateStatus", StartTime, "OK", MyUser.GetUsername(), "/" + userTicketing_id.ToString() + "/" + remark + "/" + isreject.ToString() + "/" + rejectby + "/" + isvalid.ToString() + "/" + validby, JsonString, ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "userTicketing_id", userTicketing_id.ToString(), "PutDataUpdateStatus", StartTime, "ERROR", MyUser.GetUsername(), "/" + userTicketing_id.ToString() + "/" + remark + "/" + isreject.ToString() + "/" + rejectby + "/" + isvalid.ToString() + "/" + validby, JsonString, ex.Message));
                return ex.Message;
            }
        }

        public static async Task<string> GetDataTicketAllStatusSearch(Int64 orgID, string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                HttpClient http_ticket_search = new HttpClient();
                http_ticket_search.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLUserManagement"].ToString());

                http_ticket_search.DefaultRequestHeaders.Accept.Clear();
                http_ticket_search.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(async () =>
                {
                    return await http_ticket_search.GetAsync(string.Format($"/userticketinggetallticketstatussearch/" + orgID + "/" + keyword));
                });

                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "orgID", orgID.ToString(), "GetDataTicketAllStatusSearch", StartTime, "OK", MyUser.GetUsername(), "/" + orgID.ToString() + "/" + keyword, "", ""));
                return task.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "orgID", orgID.ToString(), "GetDataTicketAllStatusSearch", StartTime, "ERROR", MyUser.GetUsername(), "/" + orgID.ToString() + "/" + keyword, "", exx.Message));
                return exx.Message;
            }
        }
    }
}