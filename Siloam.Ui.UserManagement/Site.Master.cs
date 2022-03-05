using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Siloam.Ui.UserManagement.Pages.Common;
using Siloam.Ui.UserManagement.API_Code.Models;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Siloam.Ui.UserManagement
{
    public partial class SiteMaster : MasterPage
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string useractive = "SYSTEM";
        string flagnotif = "off";

        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();

            if (!IsPostBack)
            {
                //data didapat dari form login
                DataTable dt_login = (DataTable)Session[Helper.Session_DataLogin];

                if (dt_login != null)
                {
                    lblUsername.Text = dt_login.Rows[0]["user_name"].ToString();
                    HideMenu();
                    ShowMenuLogin(Guid.Parse(dt_login.Rows[0]["user_id"].ToString()), Guid.Parse(dt_login.Rows[0]["role_id"].ToString()), Int64.Parse(dt_login.Rows[0]["organization_id"].ToString()));
                }
                else
                {
                    Response.Redirect("~/Pages/Login_page.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                getDataOrganizationTicket();

                if (HF_flagnotif.Value == "off")
                {
                    divNotif.Visible = false;
                }
            }

            //fungsi untuk menampilkan toast via akses javascript
            void ShowToastr(string message, string title, string type)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "toastr_message",
                     String.Format("toastr.{0}('{1}', '{2}');", type.ToLower(), message, title), addScriptTags: true);
            }

            void HideMenu()
            {
                divBoxApp.Visible = false;
                divBoxPage.Visible = false;
                divBoxRole.Visible = false;
                divBoxRoleMap.Visible = false;
                divBoxOrg.Visible = false;
                divBoxUser.Visible = false;
                divBoxUserMap.Visible = false;
                divBoxTicket.Visible = false;
            }

            void ShowMenuLogin(Guid userID, Guid roleID, Int64 orgID)
            {
                string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                try
                {
                    List<ViewLoginPage> ListPageData = new List<ViewLoginPage>();
                    var Get_PageLogin = clsLoginUser.GetPageLogin(userID, roleID, orgID);
                    var Get_DataPageLogin = JsonConvert.DeserializeObject<Result_Data_LoginPage>(Get_PageLogin.Result.ToString());

                    ListPageData = Get_DataPageLogin.list;

                    if (ListPageData.Count() > 0)
                    {
                        DataTable dt_listPagee = Helper.ToDataTable(ListPageData);
                        HF_flagnotif.Value = "off";

                        for (int i = 0; i < dt_listPagee.Rows.Count; i++)
                        {
                            if (dt_listPagee.Rows[i]["page_id"].ToString() == "b76a2ce8-0f2a-4c8e-bbe7-e12e5b3eb362") //Application
                            {
                                divBoxApp.Visible = true;
                            }
                            else if (dt_listPagee.Rows[i]["page_id"].ToString() == "071a27c5-d6fa-412d-8409-506196e2b74b") //Page
                            {
                                divBoxPage.Visible = true;
                            }
                            else if (dt_listPagee.Rows[i]["page_id"].ToString() == "2163c294-9120-4d10-aaaf-bc033fee082e") //Role
                            {
                                divBoxRole.Visible = true;
                            }
                            else if (dt_listPagee.Rows[i]["page_id"].ToString() == "344869d0-409b-41bd-9160-2ac02280700b") //Mapping Role Access
                            {
                                divBoxRoleMap.Visible = true;
                            }
                            else if (dt_listPagee.Rows[i]["page_id"].ToString() == "55a4b5f7-25fa-44fb-ad98-86080dbf7700") //Organization
                            {
                                divBoxOrg.Visible = true;
                            }
                            else if (dt_listPagee.Rows[i]["page_id"].ToString() == "989d47e2-687f-41a4-bf46-f8804c73c39a") //User
                            {
                                divBoxUser.Visible = true;
                            }
                            else if (dt_listPagee.Rows[i]["page_id"].ToString() == "f36a3438-bde8-48e4-a6e9-365dca0ae639") //Mapping User Role
                            {
                                divBoxUserMap.Visible = true;
                            }
                            else if (dt_listPagee.Rows[i]["page_id"].ToString() == "34e52a1a-4802-49a7-8588-07068a205267") //Ticket Request
                            {
                                divBoxTicket.Visible = true;
                                HF_flagnotif.Value = "on";
                            }
                        }
                    }
                    else
                    {
                        var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_PageLogin.Result);
                        var Status = Response.Property("status").Value.ToString();
                        var Message = Response.Property("message").Value.ToString();

                        ShowToastr(Message, Status, "warning");
                    }
                    Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "userID", userID.ToString(), "ShowMenuLogin", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
                }
                catch (Exception exx)
                {
                    Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "userID", userID.ToString(), "ShowMenuLogin", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                    ShowToastr("Please Check Your Connection!", "Error Load Data", "error");
                }
            }

            //fungsi untuk get dapat organization plus counter ticket
            void getDataOrganizationTicket()
            {
                try
                {
                    List<ViewTicketCounter> ListOrgDataTicket = new List<ViewTicketCounter>();
                    var Get_OrgTicket = clsUserTicketing.GetDataTicketCount();
                    var Get_DataOrgTicket = JsonConvert.DeserializeObject<Result_Data_TicketCount>(Get_OrgTicket.Result.ToString());

                    ListOrgDataTicket = Get_DataOrgTicket.list;

                    if (ListOrgDataTicket.Count() > 0)
                    {
                        DataTable dt_org = Helper.ToDataTable(ListOrgDataTicket);
                        int outstanding = 0;

                        DataTable dt_result = new DataTable();
                        DataTable dt_login = (DataTable)Session[Helper.Session_DataLogin];
                        if (dt_login == null)
                        {
                            Response.Redirect("~/Pages/Login_page.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }

                        string listID = "";

                        //cek apakah yg sedang login hope orgnya di HO atau tidak
                        DataRow[] foundAdmin = dt_login.Select("hope_organization_id = 1");
                        if (foundAdmin.Length > 0)
                        {
                            dt_result = dt_org;

                            for (int i = 0; i < dt_result.Rows.Count; i++)
                            {
                                outstanding = outstanding + int.Parse(dt_result.Rows[i]["counter"].ToString());
                            }

                            if (outstanding == 0)
                            {
                                divNotif.Visible = false;
                            }
                            else
                            {
                                divNotif.Visible = true;
                            }
                        }
                        else
                        {
                            //collect ID ORG from user login
                            for (int i = 0; i < dt_login.Rows.Count; i++)
                            {
                                if (i == (dt_login.Rows.Count - 1))
                                {
                                    listID = listID + dt_login.Rows[i]["organization_id"].ToString();
                                }
                                else
                                {
                                    listID = listID + dt_login.Rows[i]["organization_id"].ToString() + ",";
                                }
                            }

                            DataRow[] foundRows = dt_org.Select("organization_id in (" + listID + ")");
                            if (foundRows.Length > 0)
                            {
                                dt_result = foundRows.CopyToDataTable();

                                for (int i = 0; i < dt_result.Rows.Count; i++)
                                {
                                    outstanding = outstanding + int.Parse(dt_result.Rows[i]["counter"].ToString());
                                }

                                if (outstanding == 0)
                                {
                                    divNotif.Visible = false;
                                }
                                else
                                {
                                    divNotif.Visible = true;
                                }
                            }
                            else
                            {
                                //no data
                            }
                        }
                    }
                    else
                    {
                        var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_OrgTicket.Result);
                        var Status = Response.Property("status").Value.ToString();
                        var Message = Response.Property("message").Value.ToString();

                        ShowToastr(Message, Status, "warning");
                    }
                }
                catch (Exception exx)
                {
                    ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                }
            }
        }

        protected void LinkButtonLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Pages/Login_page.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}