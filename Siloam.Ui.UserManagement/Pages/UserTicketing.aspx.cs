using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

using Siloam.Ui.UserManagement.API_Code.Models;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Siloam.Ui.UserManagement.Pages.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace Siloam.Ui.UserManagement.Pages
{
    public partial class UserTicketing : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (!IsPostBack)
            {
                loadLinkActive();
                getDataOrganizationTicket();
                getDataUserHope();

                HiddenOrgRowSelect.Value = 0.ToString();
                HiddenOrgIdSelect.Value = 0.ToString();
                HiddenOrgNameSelect.Value = ""; 
                HiddenTicketIsreject.Value = "false";
                HiddenTicketIsvalid.Value = "false";
                HiddenFlagCari.Value = 0.ToString();
                HF_ItemSelectedHopeId.Value = 0.ToString();

                initializeData();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Page_Load", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk menampilkan toast via akses javascript
        void ShowToastr(string message, string title, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "toastr_message",
                 String.Format("toastr.{0}('{1}', '{2}');", type.ToLower(), message, title), addScriptTags: true);
        }

        //fungsi untuk memberikan style pada link sidebar yang aktif
        void loadLinkActive()
        {
            //untuk mengakses element yg ada di master site
            HtmlContainerControl myObject;
            myObject = (HtmlContainerControl)Master.FindControl("divBoxTicket");

            myObject.Style.Add("Background-color", "#1a2269");
            myObject.Style.Add("border-right", "5px solid #ffd800");
            myObject.Style.Add("color", "#f2c22c");
        }

        //inisialisasi data pada filter awal ticket
        void initializeData()
        {
            try
            {
                HiddenField orgID = (HiddenField)GridViewOrganization.Rows[0].FindControl("HiddenFieldOrgID");
                Int64 Org_ID = Int64.Parse(orgID.Value.ToString());
                HiddenOrgIdSelect.Value = Org_ID.ToString();

                LinkButton orgNAME = (LinkButton)GridViewOrganization.Rows[0].FindControl("LinkListOrg");
                string Org_NAME = orgNAME.Text.ToString();
                HiddenOrgNameSelect.Value = Org_NAME;

                LabelOrgTitle.Text = Org_NAME;
                GridViewRow rowNew = GridViewOrganization.Rows[0];
                rowNew.BackColor = ColorTranslator.FromHtml("#fbecc0");

                //filter status NEW
                divstatusNew.Attributes.Add("style", "background-color: #f7f7f7; border-right: solid 3px #f2c32e; font-weight: bold;");

                getListTicketByStatus(Org_ID, false, false);              
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
            }
        }

        //fungsi untuk get dapat organization plus counter ticket
        void getDataOrganizationTicket()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<ViewTicketCounter> ListOrgDataTicket = new List<ViewTicketCounter>();
                var Get_OrgTicket = clsUserTicketing.GetDataTicketCount();
                var Get_DataOrgTicket = JsonConvert.DeserializeObject<Result_Data_TicketCount>(Get_OrgTicket.Result.ToString());

                ListOrgDataTicket = Get_DataOrgTicket.list;

                if (ListOrgDataTicket.Count() > 0)
                {
                    DataTable dt_org = Helper.ToDataTable(ListOrgDataTicket);

                    //fungsi sorting datatable
                    //dt_org.DefaultView.Sort = "organization_name DESC";
                    //dt_org = dt_org.DefaultView.ToTable();

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
                        }
                        else
                        {
                            //no data
                        }
                    }

                    GridViewOrganization.DataSource = dt_result;
                    GridViewOrganization.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_OrgTicket.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrganizationTicket", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrganizationTicket", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi untuk get list data ticket by filter status
        void getListTicketByStatus(Int64 OrgID, bool isRejected, bool isValidated)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                DataTable dt_ticket;

                List<ViewTicketDetail> ListDataTicketDetail = new List<ViewTicketDetail>();
                var Get_DetailTicket = clsUserTicketing.GetDataTicketByOrgandStatus(OrgID, isRejected, isValidated);
                var Get_DataDetailTicket = JsonConvert.DeserializeObject<Result_Data_TicketDetail>(Get_DetailTicket.Result.ToString());

                ListDataTicketDetail = Get_DataDetailTicket.list;

                if (ListDataTicketDetail.Count() > 0)
                {
                    dt_ticket = Helper.ToDataTable(ListDataTicketDetail);

                    //fungsi sorting datatable
                    dt_ticket.DefaultView.Sort = "created_date DESC";
                    dt_ticket = dt_ticket.DefaultView.ToTable();

                    GridViewOutstandingTicket.DataSource = dt_ticket;
                    GridViewOutstandingTicket.DataBind();
                }
                else
                {
                    dt_ticket = null;
                    GridViewOutstandingTicket.DataSource = dt_ticket;
                    GridViewOutstandingTicket.DataBind();

                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_DetailTicket.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr("No Ticket Available", "Information", "info");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getListTicketByStatus", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getListTicketByStatus", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi untuk get list data ticket by all status
        void getListTicketAllStatus(Int64 OrgID)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                DataTable dt_ticket;

                List<ViewTicketDetail> ListDataTicketAll = new List<ViewTicketDetail>();
                var Get_AllTicket = clsUserTicketing.GetDataTicketAllStatus(OrgID);
                var Get_DataAllTicket = JsonConvert.DeserializeObject<Result_Data_TicketDetail>(Get_AllTicket.Result.ToString());

                ListDataTicketAll = Get_DataAllTicket.list;

                if (ListDataTicketAll.Count() > 0)
                {
                    dt_ticket = Helper.ToDataTable(ListDataTicketAll);

                    //fungsi sorting datatable
                    dt_ticket.DefaultView.Sort = "created_date DESC";
                    dt_ticket = dt_ticket.DefaultView.ToTable();

                    GridViewOutstandingTicket.DataSource = dt_ticket;
                    GridViewOutstandingTicket.DataBind();
                }
                else
                {
                    dt_ticket = null;
                    GridViewOutstandingTicket.DataSource = dt_ticket;
                    GridViewOutstandingTicket.DataBind();

                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_AllTicket.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr("No Ticket Available", "Information", "info");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getListTicketAllStatus", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getListTicketAllStatus", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //ticket pagination
        protected void GridViewOutstandingTicket_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewOutstandingTicket.PageIndex = e.NewPageIndex;
            if (HiddenTicketStatusName.Value == "All")
            {               
                if (HiddenFlagCari.Value == "0")
                {
                    getListTicketAllStatus(Int64.Parse(HiddenOrgIdSelect.Value.ToString()));
                }
                else if (HiddenFlagCari.Value == "1")
                {
                    getListTicketAllStatusSearch(Int64.Parse(HiddenOrgIdSelect.Value.ToString()), Search_masterData.Text);
                }
            }
            else
            {
                bool rejected = bool.Parse(HiddenTicketIsreject.Value.ToString());
                bool validated = bool.Parse(HiddenTicketIsvalid.Value.ToString());

                getListTicketByStatus(Int64.Parse(HiddenOrgIdSelect.Value.ToString()), rejected, validated);
            }
        }

        //function click on filter hospital
        protected void LinkListOrg_Click(object sender, EventArgs e)
        {
            Int64 Org_ID;

            int RowSelectOld = int.Parse(HiddenOrgRowSelect.Value.ToString());
            GridViewRow rowOld = GridViewOrganization.Rows[RowSelectOld];
            rowOld.BackColor = Color.Transparent;

            int RowSelect = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;
            GridViewRow rowNew = GridViewOrganization.Rows[RowSelect];
            rowNew.BackColor = ColorTranslator.FromHtml("#fbecc0");

            HiddenOrgRowSelect.Value = RowSelect.ToString(); 

            LinkButton orgName = (LinkButton)GridViewOrganization.Rows[RowSelect].FindControl("LinkListOrg");
            Label LblorgName = (Label)GridViewOrganization.Rows[RowSelect].FindControl("LabelListOrg");
            HiddenField orgID = (HiddenField)GridViewOrganization.Rows[RowSelect].FindControl("HiddenFieldOrgID");
            LabelOrgTitle.Text = LblorgName.Text.ToString();
            Org_ID = Int64.Parse(orgID.Value.ToString());

            HiddenOrgIdSelect.Value = Org_ID.ToString(); 
            HiddenOrgNameSelect.Value = LblorgName.Text.ToString();

            getListTicketByStatus(Org_ID, false, false);

            divstatusNew.Attributes.Remove("style");
            divstatusApproved.Attributes.Remove("style");
            divstatusRejected.Attributes.Remove("style");
            divstatusAll.Attributes.Remove("style");
            divstatusNew.Attributes.Add("style", "background-color: #f7f7f7; border-right: solid 3px #f2c32e; font-weight: bold;");
        }

        //function click on filter status
        protected void TK_ButtonFilterStatus_Click(object sender, EventArgs e)
        {
            divstatusNew.Attributes.Remove("style");
            divstatusApproved.Attributes.Remove("style");
            divstatusRejected.Attributes.Remove("style");
            divstatusAll.Attributes.Remove("style");

            if (HiddenTicketStatusName.Value == "All")
            {
                getListTicketAllStatus(Int64.Parse(HiddenOrgIdSelect.Value.ToString()));

                divstatusAll.Attributes.Add("style", "background-color: #f7f7f7; border-right: solid 3px #f2c32e; font-weight: bold;");
            }
            else
            {
                bool rejected = bool.Parse(HiddenTicketIsreject.Value.ToString());
                bool validated = bool.Parse(HiddenTicketIsvalid.Value.ToString());

                getListTicketByStatus(Int64.Parse(HiddenOrgIdSelect.Value.ToString()), rejected, validated);

                if (HiddenTicketIsreject.Value == "false" && HiddenTicketIsvalid.Value == "false")
                {
                    divstatusNew.Attributes.Add("style", "background-color: #f7f7f7; border-right: solid 3px #f2c32e; font-weight: bold;");
                }
                else if (HiddenTicketIsreject.Value == "false" && HiddenTicketIsvalid.Value == "true")
                {
                    divstatusApproved.Attributes.Add("style", "background-color: #f7f7f7; border-right: solid 3px #f2c32e; font-weight: bold;");
                }
                else if (HiddenTicketIsreject.Value == "true" && HiddenTicketIsvalid.Value == "false")
                {
                    divstatusRejected.Attributes.Add("style", "background-color: #f7f7f7; border-right: solid 3px #f2c32e; font-weight: bold;");
                }
            }
        }

        //empty textbox remark
        void clearForm()
        {
            TK_TextboxRemarks.Text = "";

            divstatusNew.Attributes.Remove("style");
            divstatusApproved.Attributes.Remove("style");
            divstatusRejected.Attributes.Remove("style");
            divstatusAll.Attributes.Remove("style");

            divstatusNew.Attributes.Add("style", "background-color: #f7f7f7; border-right: solid 3px #f2c32e; font-weight: bold;");
        }

        //function button approve tiket plus validasi
        protected void TK_ButtonApprove_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            p_Add.InnerText = "";

            try
            {
                string var_UserID = "";
                string var_AppID = HF_appID.Value.ToString();
                string var_OrgID = HF_orgId.Value.ToString();
                string var_RoleID = HF_roleId.Value.ToString();

                DataTable dt_userrole = new DataTable();
                DataTable dt_result_app = new DataTable();
                DataTable dt_result_org = new DataTable();
                DataTable dt_result_role = new DataTable();

                int isExist_App = 0;
                int isExist_Org = 0;
                int isExist_Role = 0;
                int isActive_userRole = 0;

                List<ViewUserRoleExtend> ListUR_Username = new List<ViewUserRoleExtend>();
                var Get_Userrole = clsUserRole.GetDataUserRole_ByUsername(HF_userName.Value.ToString());
                var Get_DataUserrole = JsonConvert.DeserializeObject<Result_Data_URExtend>(Get_Userrole.Result.ToString());

                ListUR_Username = Get_DataUserrole.list;

                //PERLU DITAMBAH PENJAGAAN SAAT BAD NETWORK
                if (ListUR_Username.Count() > 0)
                {
                    dt_userrole = Helper.ToDataTable(ListUR_Username);
                    var_UserID = dt_userrole.Rows[0]["user_id"].ToString();

                    for (int i = 0; i < dt_userrole.Rows.Count; i++)
                    {
                        if (dt_userrole.Rows[i]["application_id"].ToString() == var_AppID)
                        {
                            isExist_App = 1;
                            //next checking
                        }
                    }

                    if (isExist_App == 1)
                    {                        
                        DataRow[] foundApps = dt_userrole.Select("application_id = '" + var_AppID + "'");
                        if (foundApps.Length > 0)
                        {
                            dt_result_app = foundApps.CopyToDataTable();
                        }

                        for (int j = 0; j < dt_result_app.Rows.Count; j++)
                        {
                            if (dt_result_app.Rows[j]["organization_id"].ToString() == var_OrgID)
                            {
                                isExist_Org = 1;
                                //next checking
                            }
                        }
                    }
                    else
                    {
                        goto MAPPING;
                    }

                    if (isExist_Org == 1)
                    {                       
                        DataRow[] foundOrgs = dt_result_app.Select("organization_id = '" + var_OrgID + "'");
                        if (foundOrgs.Length > 0)
                        {
                            dt_result_org = foundOrgs.CopyToDataTable();
                        }

                        for (int k = 0; k < dt_result_org.Rows.Count; k++)
                        {
                            if (dt_result_org.Rows[k]["role_id"].ToString() == var_RoleID)
                            {
                                isExist_Role = 1;
                                //next checking
                            }
                        }
                    }
                    else
                    {
                        goto MAPPING;
                    }

                    if (isExist_Role == 1)
                    {
                        DataRow[] foundRole = dt_result_org.Select("role_id = '" + var_RoleID + "'");
                        if (foundRole.Length > 0)
                        {
                            dt_result_role = foundRole.CopyToDataTable();
                        }

                        for (int m = 0; m < dt_result_role.Rows.Count; m++)
                        {
                            if (dt_result_role.Rows[m]["is_active"].ToString().ToLower() == "true")
                            {
                                isActive_userRole = 1;
                                //next checking
                            }
                        }
                    }
                    else
                    {
                        //notif change role or not 

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "confirmation", "POPmodalChangeRole('" + HF_ticketID.Value.ToString() + "','" + HF_roleName.Value.ToString() + "','" + dt_result_org.Rows[0]["role_name"].ToString() + "','" + dt_result_org.Rows[0]["user_role_id"].ToString() + "','" + dt_result_org.Rows[0]["user_id"].ToString() + "','" + dt_result_org.Rows[0]["created_by"].ToString().Replace(" ", "_").Replace("\\", "+") + "','" + dt_result_org.Rows[0]["created_date"].ToString() + "','" + HF_userName.Value.ToString() + "')", addScriptTags: true);
                        goto FINISH;
                    }

                    if (isActive_userRole == 1)
                    {
                        //notif already exist

                        List<User> ListUserData = new List<User>();
                        var Get_User = clsUser.GetDataUserByUsername(HF_userName.Value.ToString());
                        var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

                        ListUserData = Get_DataUser.list;

                        if (ListUserData.Count() > 0)
                        {
                            if (ListUserData.First().is_active == true)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "confirmation", "POPmodalAlreadyExist('" + HF_ticketID.Value.ToString() + "','" + HF_userName.Value.ToString() + "')", addScriptTags: true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "confirmation", "POPmodalInfoActiveUSer('" + HF_ticketID.Value.ToString() + "','" + HF_userName.Value.ToString() + "')", addScriptTags: true);
                            }
                        }

                        goto FINISH;
                    }
                    else
                    {
                        //notif active userrole

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "confirmation", "POPmodalActivationRole('" + HF_ticketID.Value.ToString() + "','" + dt_result_org.Rows[0]["user_role_id"].ToString() + "','" + dt_result_org.Rows[0]["user_id"].ToString() + "','" + dt_result_org.Rows[0]["created_by"].ToString().Replace(" ", "_").Replace("\\", "+") + "','" + dt_result_org.Rows[0]["created_date"].ToString() + "','" + dt_result_org.Rows[0]["role_id"].ToString() + "','" + HF_userName.Value.ToString() + "')", addScriptTags: true);
                        goto FINISH;
                    }

                MAPPING:
                    UserRole model_UR = new UserRole();

                    model_UR.user_id = Guid.Parse(var_UserID);
                    model_UR.organization_id = Int64.Parse(var_OrgID);
                    model_UR.application_id = Guid.Parse(var_AppID);
                    model_UR.role_id = Guid.Parse(var_RoleID);
                    model_UR.is_active = true;
                    model_UR.created_by = Helper.UserLogin(this.Page);
                    model_UR.created_date = DateTime.Now;
                    model_UR.modified_by = null;
                    model_UR.modified_date = DateTime.Now;

                    var hasil = clsUserRole.PostDataUserRole(model_UR);

                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    if (Status == "Fail")
                    {
                        p_Add.Attributes.Remove("style");
                        p_Add.Attributes.Add("style", "display:block; color:red;");
                        p_Add.InnerText = "Approve Ticket Failed!";
                        ShowToastr(Status + "! " + Message, "Approve Ticket Failed", "error");
                    }
                    else
                    {
                        ShowToastr("The username was successfully registered and mapped", "Approve Ticket Success", "success");
                        ApproveFunction();
                    }

                FINISH:
                    clearForm();

                }
                else
                {
                    //select ke table user
                    List<User> ListUserData = new List<User>();
                    var Get_User = clsUser.GetDataUserByUsername(HF_userName.Value.ToString());
                    var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

                    ListUserData = Get_DataUser.list;

                    if (ListUserData.Count() > 0)
                    {
                        UserRole model_UR = new UserRole();

                        model_UR.user_id = ListUserData.First().user_id;
                        model_UR.organization_id = Int64.Parse(var_OrgID);
                        model_UR.application_id = Guid.Parse(var_AppID);
                        model_UR.role_id = Guid.Parse(var_RoleID);
                        model_UR.is_active = true;
                        model_UR.created_by = Helper.UserLogin(this.Page);
                        model_UR.created_date = DateTime.Now;
                        model_UR.modified_by = null;
                        model_UR.modified_date = DateTime.Now;
 
                        var hasil = clsUserRole.PostDataUserRole(model_UR);

                        var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
                        var Status = Response.Property("status").Value.ToString();
                        var Message = Response.Property("message").Value.ToString();

                        if (Status == "Fail")
                        {
                            p_Add.Attributes.Remove("style");
                            p_Add.Attributes.Add("style", "display:block; color:red;");
                            p_Add.InnerText = "Approve Ticket Failed!";
                            ShowToastr(Status + "! " + Message, "Approve Ticket Failed", "error");
                        }
                        else
                        {
                            //update is_active di table customer
                            User model_user = ListUserData.First();
                            model_user.is_active = true;

                            var hasilUpdate = clsUser.PutDataUser(model_user.user_id, model_user);

                            var ResponseUpdate = (JObject)JsonConvert.DeserializeObject<dynamic>(hasilUpdate.Result);
                            var StatusUpdate = ResponseUpdate.Property("status").Value.ToString();
                            var MessageUpdate = ResponseUpdate.Property("message").Value.ToString();

                            if (StatusUpdate == "Fail")
                            {
                                p_Add.Attributes.Remove("style");
                                p_Add.Attributes.Add("style", "display:block; color:red;");
                                p_Add.InnerText = "Approve Ticket Failed!";
                                ShowToastr(StatusUpdate + "! " + MessageUpdate, "Approve Ticket Failed", "error");
                            }
                            else
                            {
                                ShowToastr("The username was successfully registered and mapped", "Approve Ticket Success", "success");
                                ApproveFunction();
                            }
                        }   
                    }
                    else
                    {

                        //add new feature here
                        //if (TK_LabelHopeID.Text == "0")
                        //{
                        //    HF_ItemSelectedHopeId.Value = "0";
                        //}

                        if (getDataHOPEIDExist(Int64.Parse(HF_ItemSelectedHopeId.Value.ToString())) == true)
                        {
                            p_Add.Attributes.Remove("style");
                            p_Add.Attributes.Add("style", "display:block; color:red;");
                            p_Add.InnerText = "Hope User ID already used by another user!";
                        }
                        else
                        {
                            //p_Add.Attributes.Remove("style");
                            //p_Add.Attributes.Add("style", "display:block; color:green;");
                            //p_Add.InnerText = "BERHASIL";
                            User model_user = new User();
                            List<DoctorSIP> model_sip = new List<DoctorSIP>();
                            ViewUserSIP model = new ViewUserSIP();

                            model_user.user_id = Guid.NewGuid();
                            model_user.user_name = HF_userName.Value.ToString();
                            model_user.password = HF_password.Value.ToString();  //ConfigurationManager.AppSettings["DefaultPassword"].ToString();
                            model_user.full_name = HF_fullname.Value.ToString();
                            //model_user.hope_user_id = Int64.Parse(ConfigurationManager.AppSettings["DefaultHopeID"].ToString()); //0 //Unnamed
                            model_user.hope_user_id = Int64.Parse(HF_ItemSelectedHopeId.Value.ToString());
                            model_user.email = HF_email.Value.ToString();
                            model_user.birthday = DateTime.Parse(ConfigurationManager.AppSettings["DefaultBirthday"].ToString());
                            model_user.handphone = HF_phone.Value.ToString();
                            model_user.lock_counter = 0;
                            model_user.last_login_date = null;
                            model_user.exp_pass_date = DateTime.Now.AddMonths(3);
                            model_user.is_internal = true;
                            model_user.is_ad = false;
                            model_user.is_proint = false;
                            model_user.is_active = true;
                            model_user.created_by = Helper.UserLogin(this.Page);
                            model_user.created_date = DateTime.Now;
                            model_user.modified_by = null;
                            model_user.modified_date = DateTime.Now;

                            model.model_sip = model_sip;
                            model.model_user = model_user;
                            var hasil = clsUser.PostDataUser(model);

                            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
                            var Status = Response.Property("status").Value.ToString();
                            var Message = Response.Property("message").Value.ToString();

                            if (Status == "Fail")
                            {
                                p_Add.Attributes.Remove("style");
                                p_Add.Attributes.Add("style", "display:block; color:red;");
                                p_Add.InnerText = "Save Failed!";
                                ShowToastr(Status + "! " + Message, "Approve Ticket Failed", "error");
                            }
                            else
                            {
                                //select ke table user
                                List<User> ListUserDataMap = new List<User>();
                                var Get_UserMap = clsUser.GetDataUserByUsername(HF_userName.Value.ToString());
                                var Get_DataUserMap = JsonConvert.DeserializeObject<Result_Data_user>(Get_UserMap.Result.ToString());

                                ListUserDataMap = Get_DataUserMap.list;

                                if (ListUserDataMap.Count() > 0)
                                {
                                    UserRole model_UR = new UserRole();

                                    model_UR.user_id = ListUserDataMap.First().user_id;
                                    model_UR.organization_id = Int64.Parse(var_OrgID);
                                    model_UR.application_id = Guid.Parse(var_AppID);
                                    model_UR.role_id = Guid.Parse(var_RoleID);
                                    model_UR.is_active = true;
                                    model_UR.created_by = Helper.UserLogin(this.Page);
                                    model_UR.created_date = DateTime.Now;
                                    model_UR.modified_by = null;
                                    model_UR.modified_date = DateTime.Now;

                                    var hasilUR = clsUserRole.PostDataUserRole(model_UR);

                                    var ResponseUR = (JObject)JsonConvert.DeserializeObject<dynamic>(hasilUR.Result);
                                    var StatusUR = ResponseUR.Property("status").Value.ToString();
                                    var MessageUR = ResponseUR.Property("message").Value.ToString();

                                    if (StatusUR == "Fail")
                                    {
                                        p_Add.Attributes.Remove("style");
                                        p_Add.Attributes.Add("style", "display:block; color:red;");
                                        p_Add.InnerText = "Save Failed!";
                                        ShowToastr(StatusUR + "! " + MessageUR, "Approve Ticket Failed", "error");
                                    }
                                    else
                                    {
                                        ShowToastr("The username was successfully registered and mapped", "Approve Ticket Success", "success");
                                        ApproveFunction();
                                    }
                                }
                                else
                                {
                                    //data user not found
                                }
                            }
                        }
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "TK_ButtonApprove_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "TK_ButtonApprove_Click", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //function update status reject
        void RejectFunction()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            long user_ticketing_id = Int64.Parse(HF_ticketID.Value.ToString());
            string remark = TK_TextboxRemarks.Text.ToString();
            bool is_reject = true;
            string reject_by = Helper.UserLogin(this.Page).Replace("\\", ".");
            bool is_valid = false;
            string valid_by = "system";
 
            var hasil = clsUserTicketing.PutDataUpdateStatus(user_ticketing_id, remark, is_reject, reject_by, is_valid, valid_by);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:red;");
                p_Add.InnerText = "Reject Ticket Failed!";
                ShowToastr(Status + "! " + Message, "Reject Ticket Failed", "error");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalDetailsTicket').modal('hide');", addScriptTags: true);
                ShowToastr("The Ticket was successfully rejected", "Reject Ticket Success", "success");

                getListTicketByStatus(Int64.Parse(HiddenOrgIdSelect.Value.ToString()), false, false);
                getDataOrganizationTicket();
                clearForm();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "RejectFunction", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //function update status approve
        void ApproveFunction()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            long user_ticketing_id = Int64.Parse(HF_ticketID.Value.ToString());
            string remark = "approved";
            bool is_reject = false;
            string reject_by = "system";
            bool is_valid = true;
            string valid_by = Helper.UserLogin(this.Page).Replace("\\", ".");

            var hasil = clsUserTicketing.PutDataUpdateStatus(user_ticketing_id, remark, is_reject, reject_by, is_valid, valid_by);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:red;");
                p_Add.InnerText = "Approve Ticket Failed!";
                ShowToastr(Status + "! " + Message, "Approve Ticket Failed", "error");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalDetailsTicket').modal('hide');", addScriptTags: true);
                ShowToastr("Ticket status updated", "Ticket", "info");
                //ShowToastr("The username was successfully registered and mapped", "Approve Ticket Success", "success");

                getListTicketByStatus(Int64.Parse(HiddenOrgIdSelect.Value.ToString()), false, false);
                getDataOrganizationTicket();
                clearForm();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ApproveFunction", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //button reject click
        protected void TK_ButtonReject_Click(object sender, EventArgs e)
        {
            p_Add.InnerText = "";
            RejectFunction();
        }

        //button change role click
        protected void CT_ButtonChangeRole_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            UserRole model_UR = new UserRole();

            model_UR.user_role_id = Int64.Parse(Hidden_CT_URID.Value.ToString());
            model_UR.user_id = Guid.Parse(Hidden_CT_userID.Value.ToString());
            model_UR.organization_id = Int64.Parse(HF_orgId.Value.ToString());
            model_UR.application_id = Guid.Parse(HF_appID.Value.ToString());
            model_UR.role_id = Guid.Parse(HF_roleId.Value.ToString());
            model_UR.is_active = true;
            model_UR.created_by = Hidden_CT_createBy.Value.ToString();
            model_UR.created_date = DateTime.Parse(Hidden_CT_createDate.Value.ToString());
            model_UR.modified_by = Helper.UserLogin(this.Page);
            model_UR.modified_date = DateTime.Now;

            var hasil = clsUserRole.PutDataUserRole(model_UR.user_role_id, model_UR);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                p_Edit.Attributes.Remove("style");
                p_Edit.Attributes.Add("style", "display:block; color:red;");
                p_Edit.InnerText = "Change Role Failed!";
                ShowToastr(Status + "! " + Message, "Change Role Failed", "error");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalChangeRole').modal('hide');", addScriptTags: true);
                ShowToastr("The role of username was successfully changed", "Approve Ticket Success", "success");
                ApproveFunction();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "CT_ButtonChangeRole_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //button activation user role click
        protected void AR_ButtonActivationRole_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            UserRole model_UR = new UserRole();

            model_UR.user_role_id = Int64.Parse(Hidden_AR_URID.Value.ToString());
            model_UR.user_id = Guid.Parse(Hidden_AR_userID.Value.ToString());
            model_UR.organization_id = Int64.Parse(HF_orgId.Value.ToString());
            model_UR.application_id = Guid.Parse(HF_appID.Value.ToString());
            model_UR.role_id = Guid.Parse(Hidden_AR_roleID.Value.ToString());
            model_UR.is_active = true;
            model_UR.created_by = Hidden_AR_createBy.Value.ToString();
            model_UR.created_date = DateTime.Parse(Hidden_AR_createDate.Value.ToString());
            model_UR.modified_by = Helper.UserLogin(this.Page);
            model_UR.modified_date = DateTime.Now;

            var hasil = clsUserRole.PutDataUserRole(model_UR.user_role_id, model_UR);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                p_active.Attributes.Remove("style");
                p_active.Attributes.Add("style", "display:block; color:red;");
                p_active.InnerText = "Activation User Role Failed!";
                ShowToastr(Status + "! " + Message, "Activation User Role Failed", "error");
            }
            else
            {
                List<User> ListUserData = new List<User>();
                var Get_User = clsUser.GetDataUserByUsername(HF_userName.Value.ToString());
                var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

                ListUserData = Get_DataUser.list;

                if (ListUserData.Count() > 0)
                {
                    User model_user = ListUserData.First();
                    model_user.is_active = true;

                    var hasilUpdate = clsUser.PutDataUser(model_user.user_id, model_user);

                    var ResponseUpdate = (JObject)JsonConvert.DeserializeObject<dynamic>(hasilUpdate.Result);
                    var StatusUpdate = ResponseUpdate.Property("status").Value.ToString();
                    var MessageUpdate = ResponseUpdate.Property("message").Value.ToString();

                    if (StatusUpdate == "Fail")
                    {
                        p_Add.Attributes.Remove("style");
                        p_Add.Attributes.Add("style", "display:block; color:red;");
                        p_Add.InnerText = "Approve Ticket Failed!";
                        ShowToastr(StatusUpdate + "! " + MessageUpdate, "Approve Ticket Failed", "error");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalActivationRole').modal('hide');", addScriptTags: true);
                        ShowToastr("The activation of username was successfully activated", "Approve Ticket Success", "success");
                        ApproveFunction();
                    }
                }
                else
                {
                    ShowToastr("Please try again", "Approve Ticket Failed", "error");
                }
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "AR_ButtonActivationRole_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk get list data ticket by all status pencarian
        void getListTicketAllStatusSearch(Int64 OrgID, string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                DataTable dt_ticket;

                List<ViewTicketDetail> ListDataTicketAll = new List<ViewTicketDetail>();
                var Get_AllTicket = clsUserTicketing.GetDataTicketAllStatusSearch(OrgID, keyword);
                var Get_DataAllTicket = JsonConvert.DeserializeObject<Result_Data_TicketDetail>(Get_AllTicket.Result.ToString());

                ListDataTicketAll = Get_DataAllTicket.list;

                if (ListDataTicketAll.Count() > 0)
                {
                    dt_ticket = Helper.ToDataTable(ListDataTicketAll);

                    //fungsi sorting datatable
                    dt_ticket.DefaultView.Sort = "created_date DESC";
                    dt_ticket = dt_ticket.DefaultView.ToTable();

                    GridViewOutstandingTicket.DataSource = dt_ticket;
                    GridViewOutstandingTicket.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_AllTicket.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr("No Ticket Found", "Information", "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getListTicketAllStatusSearch", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getListTicketAllStatusSearch", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi klik button pencarian
        protected void ButtonCari_Click(object sender, EventArgs e)
        {
            divstatusNew.Attributes.Remove("style");
            divstatusApproved.Attributes.Remove("style");
            divstatusRejected.Attributes.Remove("style");
            divstatusAll.Attributes.Remove("style");

            divstatusAll.Attributes.Add("style", "background-color: #fbecc0; border-right: solid 3px #f2c32e; font-weight: bold;");
            HiddenTicketStatusName.Value = "All";

            if (Search_masterData.Text != "")
            {
                getListTicketAllStatusSearch(Int64.Parse(HiddenOrgIdSelect.Value.ToString()),Search_masterData.Text);
                HiddenFlagCari.Value = 1.ToString();
            }
            else
            {
                getListTicketAllStatus(Int64.Parse(HiddenOrgIdSelect.Value.ToString()));
                HiddenFlagCari.Value = 0.ToString();
            }
        }

        //fungsi untuk get data user HOPE
        public void getDataUserHope()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                if (Session["dataUserHope"] == null || Session["dataUserHope"].ToString() == "")
                {
                    List<XUserHOPE> ListUserDataHope = new List<XUserHOPE>();
                    var Get_UserHope = clsXUserHOPE.GetDataUserHopeID();
                    var Get_DataUserHope = JsonConvert.DeserializeObject<Result_Data_XUserHope>(Get_UserHope.Result.ToString());

                    //ListUserDataHope = Get_DataUserHope.list;
                    ListUserDataHope.Add(new XUserHOPE { userId = 0, userName = "unnamed", name = "" });
                    ListUserDataHope.AddRange(Get_DataUserHope.list);

                    if (ListUserDataHope.Count() > 0)
                    {
                        DataTable dt_userHope = Helper.ToDataTable(ListUserDataHope);

                        //fungsi sorting datatable
                        dt_userHope.DefaultView.Sort = "userName ASC";
                        dt_userHope = dt_userHope.DefaultView.ToTable();

                        //menyimpan data di session untuk nanti tinggal dimanfaatkan dimana-mana
                        Session["dataUserHope"] = dt_userHope;
                    }
                    else
                    {
                        var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_UserHope.Result);
                        var Status = Response.Property("status").Value.ToString();
                        var Message = Response.Property("message").Value.ToString();

                        ShowToastr(Message, Status, "warning");
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUserHope", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUserHope", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi untuk mengecek data yang diiputkan apakah sudah ada di DB atau belum
        public bool getDataHOPEIDExist(Int64 user_hope_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool eksis = false;
            try
            {
                if (user_hope_id == 0)
                {
                    eksis = false;
                }
                else
                {
                    List<User> ListHopeIDData = new List<User>();
                    var Get_UserHopeID = clsUser.GetDataUserByHopeID(user_hope_id);
                    var Get_DataUserHopeID = JsonConvert.DeserializeObject<Result_Data_user>(Get_UserHopeID.Result.ToString());

                    ListHopeIDData = Get_DataUserHopeID.list;

                    if (ListHopeIDData.Count() > 0)
                    {
                        eksis = true;
                    }
                    else
                    {
                        eksis = false;
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataHOPEIDExist", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataHOPEIDExist", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
            return eksis;
        }
    }
}