using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;

using Siloam.Ui.UserManagement.API_Code.Models;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Siloam.Ui.UserManagement.Pages.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace Siloam.Ui.UserManagement.Pages
{
    public partial class UserRoleMapping : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //deklarasi temp table
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (!IsPostBack)
            {
                loadLinkActive();
                getDataApplication();
                getDataOrganization();
                getDataUser();

                HiddenAppRowSelect.Value = 0.ToString(); 
                HiddenAppIdSelect.Value = Guid.Empty.ToString();
                HiddenAppNameSelect.Value = ""; 
                HiddenOrgRowSelect.Value = 0.ToString();
                HiddenOrgIdSelect.Value = 0.ToString();  
                HiddenOrgNameSelect.Value = "";

                //inisialisasi temp table
                dt = new DataTable();
                MakeDataTable_UserTemp();
                BindGrid_UserTemp();
            }
            else
            {
                dt = (DataTable)ViewState["DataTable"];
            }
            ViewState["DataTable"] = dt;
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
            myObject = (HtmlContainerControl)Master.FindControl("divBoxUserMap");

            myObject.Style.Add("Background-color", "#1a2269");
            myObject.Style.Add("border-right", "5px solid #ffd800");
            myObject.Style.Add("color", "#f2c22c");
        }

        //inisialisasi datatable
        private void MakeDataTable_UserTemp()
        {
            dt.Columns.Add("user_id");
            dt.Columns.Add("user_name");
        }

        //adding data to datatable from input user
        private void AddToDataTable_UserTemp(string id, string data)
        {
            DataRow dr = dt.NewRow();
            dr["user_id"] = id;
            dr["user_name"] = data;
            dt.Rows.Add(dr);
        }

        //bind data to gridview
        private void BindGrid_UserTemp()
        {
            Add_GridViewUserMapList.DataSource = dt;
            Add_GridViewUserMapList.DataBind();
        }

        //fungsi untuk get dapat application
        void getDataApplication()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Application> ListAppData = new List<Application>();

                if (Session["AppData"] == null)
                {
                    var Get_App = clsApplication.GetDataApplication();
                    var Get_DataApp = JsonConvert.DeserializeObject<Result_Data_Application>(Get_App.Result.ToString());
                    ListAppData = Get_DataApp.list;
                    Session["AppData"] = ListAppData;

                    if (ListAppData.Count() == 0)
                    {
                        var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_App.Result);
                        var Status = Response.Property("status").Value.ToString();
                        var Message = Response.Property("message").Value.ToString();

                        ShowToastr(Message, Status, "warning");
                    }
                }
                else
                {
                    ListAppData = (List<Application>)Session["AppData"];
                }

                if (ListAppData.Count() > 0)
                {
                    DataTable dt_result = new DataTable();
                    DataTable dt_app = Helper.ToDataTable(ListAppData);

                    //fungsi sorting datatable
                    dt_app.DefaultView.Sort = "application_name ASC";
                    dt_app = dt_app.DefaultView.ToTable();

                    DataRow[] foundRows = dt_app.Select("is_active =" + true);
                    if (foundRows.Length > 0)
                    {
                        dt_result = foundRows.CopyToDataTable();
                    }

                    DataTable dt_login = (DataTable)Session[Helper.Session_DataLogin];
                    if (dt_login == null)
                    {
                        Response.Redirect("~/Pages/Login_page.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }

                    string role_admin = dt_login.Rows[0]["role_id"].ToString();
                    if (role_admin != ConfigurationManager.AppSettings["rolesuperadmin"].ToString()) //role admin cabang
                    {
                        for (int i = 0; i < dt_result.Rows.Count; i++)
                        {
                            DataRow dr_ums = dt_result.Rows[i];
                            if (dr_ums["application_id"].ToString() == ConfigurationManager.AppSettings["ApplicationId"].ToString())
                            {
                                dr_ums.Delete();
                            }
                        }
                        dt_result.AcceptChanges();
                    }

                    GridViewApplication.DataSource = dt_result;
                    GridViewApplication.DataBind();
                }
                //else
                //{
                //    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_App.Result);
                //    var Status = Response.Property("status").Value.ToString();
                //    var Message = Response.Property("message").Value.ToString();

                //    ShowToastr(Message, Status, "warning");
                //}
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataApplication", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataApplication", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
            }
        }

        //fungsi untuk get data organization
        void getDataOrganization()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Organization> ListOrgData = new List<Organization>();

                if (Session["OrgData"] == null)
                {
                    var Get_Org = clsOrganization.GetDataOrganization();
                    var Get_DataOrg = JsonConvert.DeserializeObject<Result_Data_Organization>(Get_Org.Result.ToString());
                    ListOrgData = Get_DataOrg.list;
                    Session["OrgData"] = ListOrgData;

                    if (ListOrgData.Count() == 0)
                    {
                        var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Org.Result);
                        var Status = Response.Property("status").Value.ToString();
                        var Message = Response.Property("message").Value.ToString();

                        ShowToastr(Message, Status, "warning");
                    }
                }
                else
                {
                    ListOrgData = (List<Organization>)Session["OrgData"];
                }

                if (ListOrgData.Count() > 0)
                {
                    DataTable dt_org = new DataTable();
                    DataTable dt_org_temp = Helper.ToDataTable(ListOrgData);

                    DataRow[] foundRowsOrg = dt_org_temp.Select("is_active =" + true);
                    if (foundRowsOrg.Length > 0)
                    {
                        dt_org = foundRowsOrg.CopyToDataTable();
                    }

                    //fungsi sorting datatable
                    dt_org.DefaultView.Sort = "organization_name ASC";
                    dt_org = dt_org.DefaultView.ToTable();

                    DataTable dt_result = new DataTable();
                    DataTable dt_login = (DataTable)Session[Helper.Session_DataLogin];
                    if (dt_login == null)
                    {
                        Response.Redirect("~/Pages/Login_page.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }

                    string listID = "";

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
                //else
                //{
                //    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Org.Result);
                //    var Status = Response.Property("status").Value.ToString();
                //    var Message = Response.Property("message").Value.ToString();

                //    ShowToastr(Message, Status, "warning");
                //}
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrganization", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrganization", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
            }
        }

        //fungsi untuk get data all user
        void getDataUser()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<User> ListUser = new List<User>();
                var Get_User = clsUser.GetDataUser();
                var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

                ListUser = Get_DataUser.list;

                if (ListUser.Count() > 0)
                {
                    Session["dataUserUMS"] = ListUser;
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_User.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUser", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUser", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi untuk get data all user
        //void getDataUserDDL()
        //{
        //    try
        //    {
        //        List<User> ListUserDDL = new List<User>();
        //        var Get_UserDDL = clsUser.GetDataUser();
        //        var Get_DataUserDDL = JsonConvert.DeserializeObject<Result_Data_user>(Get_UserDDL.Result.ToString());

        //        //ListUserDDL = Get_DataUserDDL.list;
        //        //untuk inisialisasi data pertama dari DDL lalu append data range
        //        ListUserDDL.Add(new User { user_id = Guid.Empty, user_name = "~select user~", is_active=true });
        //        ListUserDDL.AddRange(Get_DataUserDDL.list);

        //        if (ListUserDDL.Count() > 0)
        //        {
        //            DataTable dt_result = new DataTable();
        //            DataTable dt_userDDL = Helper.ToDataTable(ListUserDDL);

        //            DataRow[] foundRows = dt_userDDL.Select("is_active =" + true);
        //            if (foundRows.Length > 0)
        //            {
        //                dt_result = foundRows.CopyToDataTable();
        //            }

        //            //fungsi sorting datatable
        //            dt_result.DefaultView.Sort = "user_name ASC";
        //            dt_result = dt_result.DefaultView.ToTable();

        //            Add_DDLUsername.DataTextField = "user_name";
        //            Add_DDLUsername.DataValueField = "user_id";

        //            Add_DDLUsername.DataSource = dt_result;
        //            Add_DDLUsername.DataBind();

        //            Add_DDLUsername.SelectedValue = Guid.Empty.ToString();

        //            Session["dataUserUMS"] = dt_result;
        //        }
        //        else
        //        {
        //            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_UserDDL.Result);
        //            var Status = Response.Property("status").Value.ToString();
        //            var Message = Response.Property("message").Value.ToString();

        //            ShowToastr(Message, Status, "warning");
        //        }
        //    }
        //    catch (Exception exx)
        //    {
        //        ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
        //    }
        //}

        //fungsi untuk get data all user
        void getDataUserDDLfilter()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<User> ListUserDDL = new List<User>();

                if (Session["dataUserUMS"] == null)
                {
                    var Get_UserDDL = clsUser.GetDataUser();
                    var Get_DataUserDDL = JsonConvert.DeserializeObject<Result_Data_user>(Get_UserDDL.Result.ToString());
                    ListUserDDL = Get_DataUserDDL.list;
                    Session["dataUserUMS"] = ListUserDDL;
                }
                else
                {
                    ListUserDDL = (List<User>)Session["dataUserUMS"];
                }

                List<ViewListUser> ListUserDataCek = new List<ViewListUser>();
                var Get_UserCek = clsUserRole.GetDataUser_byOrgApp(Int64.Parse(HiddenOrgIdSelect.Value.ToString()), Guid.Parse(HiddenAppIdSelect.Value.ToString()));
                var Get_DataUserCek = JsonConvert.DeserializeObject<Result_Data_User_List>(Get_UserCek.Result.ToString());
                ListUserDataCek = Get_DataUserCek.list;

                DataTable dt_userDDL = Helper.ToDataTable(ListUserDDL);
                DataTable dt_userDataCek = Helper.ToDataTable(ListUserDataCek);

                if (dt_userDDL.Rows.Count > 0)
                {
                    if (dt_userDataCek.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_userDDL.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt_userDataCek.Rows.Count; j++)
                            {
                                if (dt_userDDL.Rows[i]["user_id"].ToString() == dt_userDataCek.Rows[j]["user_id"].ToString())
                                {
                                    dt_userDDL.Rows[i]["user_name"] = dt_userDDL.Rows[i]["user_name"] + " - (already mapping)";
                                }
                            }
                        }
                    }

                    DataTable dt_result = new DataTable();

                    DataRow[] foundRows = dt_userDDL.Select("is_active =" + true);
                    if (foundRows.Length > 0)
                    {
                        dt_result = foundRows.CopyToDataTable();
                    }

                    //fungsi sorting datatable
                    dt_result.DefaultView.Sort = "user_name ASC";
                    dt_result = dt_result.DefaultView.ToTable();

                    Session["dataUserUMSfiltered"] = dt_result;
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_UserCek.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUserDDLfilter", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUserDDLfilter", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi untuk get data role selected by app id - switch role
        void getDataRoleDDL(Guid application_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<ViewListRole> ListRoleData = new List<ViewListRole>();
                var Get_Role = clsViewListRole.GetDataRole_forPage(application_id);
                var Get_DataRole = JsonConvert.DeserializeObject<Result_Data_Role_List>(Get_Role.Result.ToString());

                //ListRoleData = Get_DataRole.list;
                //untuk inisialisasi data pertama dari DDL lalu append data range
                ListRoleData.Add(new ViewListRole { role_id = Guid.Empty, role_name = "~select role~", is_active=true });
                ListRoleData.AddRange(Get_DataRole.list);

                if (ListRoleData.Count() > 0)
                {
                    DataTable dt_result = new DataTable();
                    DataTable dt_roleDDL = Helper.ToDataTable(ListRoleData);

                    DataRow[] foundRows = dt_roleDDL.Select("is_active =" + true);
                    if (foundRows.Length > 0)
                    {
                        dt_result = foundRows.CopyToDataTable();
                    }

                    //fungsi sorting datatable
                    dt_result.DefaultView.Sort = "role_name ASC";
                    dt_result = dt_result.DefaultView.ToTable();

                    SW_DDLRolename.DataTextField = "role_name";
                    SW_DDLRolename.DataValueField = "role_id";

                    SW_DDLRolename.DataSource = dt_result;
                    SW_DDLRolename.DataBind();

                    SW_DDLRolename.SelectedValue = Guid.Empty.ToString();
                }
                else
                {
                    SW_DDLRolename.Items.Clear();

                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Role.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataRoleDDL", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataRoleDDL", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi untuk mengecek data yang diiputkan apakah sudah ada di DB atau belum
        bool getDataUserRoleExist(Guid user_id, Int64 organization_id, Guid application_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool existt = false;
            try
            {
                List<UserRole> ListURExist = new List<UserRole>();
                var Get_URExist = clsUserRole.GetDataUserRole_Exist(user_id, organization_id, application_id);
                var Get_DataURExist = JsonConvert.DeserializeObject<Result_Data_UserRole>(Get_URExist.Result.ToString());

                ListURExist = Get_DataURExist.list;

                if (ListURExist.Count() > 0)
                {
                    existt = true;
                }
                else
                {
                    existt = false;
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUserRoleExist", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUserRoleExist", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
            return existt;
        }

        //fungsi untuk validasi, bahwa data yg akan diinput sudah ada di DB atau belum
        //protected void Add_DDLUsername_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Guid userID = Guid.Parse(Add_DDLUsername.SelectedValue.ToString());
        //    Int64 orgID = Int64.Parse(HiddenOrgID.Value.ToString());
        //    Guid appID = Guid.Parse(HiddenAppID.Value.ToString());
        //    Guid roleID = Guid.Parse(HiddenRoleID.Value.ToString());

        //    int cekdatasama = 0;
        //    p_Add.InnerText = "";

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        if (row["user_name"].ToString() == Add_DDLUsername.SelectedItem.ToString())
        //        {
        //            cekdatasama = 1;
        //        }
        //    }

        //    if (cekdatasama == 0)
        //    {
        //        if (getDataUserRoleExist(userID, orgID, appID) == true)
        //        {
        //            p_Add.Attributes.Remove("style");
        //            p_Add.Attributes.Add("style", "display:block; color:red;");
        //            p_Add.InnerText = "Username already Mapping in this application!";
        //        }
        //        else
        //        {
        //            AddToDataTable_UserTemp(Add_DDLUsername.SelectedValue.ToString(), Add_DDLUsername.SelectedItem.ToString());
        //            BindGrid_UserTemp();
        //        }
        //    }
        //    else
        //    {
        //        p_Add.Attributes.Remove("style");
        //        p_Add.Attributes.Add("style", "display:block; color:red;");
        //        p_Add.InnerText = "Input Username cannot be the same!";
        //    }
        //    Add_DDLUsername.SelectedValue = Guid.Empty.ToString();
        //    this.Page.SetFocus(Add_DDLUsername.ClientID);
        //}

        //fungsi untuk delete 1 data from temp gridview
        protected void ImgBtn_DeleteRow_Click(object sender, ImageClickEventArgs e)
        {
            int RowSelect = ((GridViewRow)(((ImageButton)sender).Parent.Parent)).RowIndex;
            dt.Rows[RowSelect].Delete();
            BindGrid_UserTemp();

            p_Add.InnerText = "";
        }

        //fungsi untuk mengosongkan temp gridview
        protected void ButtonClearList_Click(object sender, EventArgs e)
        {
            clearFormAdd();
        }

        //fungsi untuk me reset form input dan notifikasi menjadi kosong kembali
        void clearFormAdd()
        {
            int counter = dt.Rows.Count - 1;
            for (int j = counter; j >= 0; j--)
            {
                dt.Rows[j].Delete();
            }
            ViewState["DataTable"] = dt;
            BindGrid_UserTemp();

            p_Add.InnerText = "";
        }

        //fungsi untuk menjalankan aksi saat side application dipilih dan diklik
        protected void LinkButtonApps_Click(object sender, EventArgs e)
        {
            Guid App_ID;

            int RowSelectOld = int.Parse(HiddenAppRowSelect.Value.ToString()); 
            GridViewRow rowOld = GridViewApplication.Rows[RowSelectOld];
            rowOld.BackColor = Color.Transparent;

            int RowSelect = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;
            GridViewRow rowNew = GridViewApplication.Rows[RowSelect];
            rowNew.BackColor = Color.White;

            HiddenAppRowSelect.Value = RowSelect.ToString();  

            LinkButton appName = (LinkButton)GridViewApplication.Rows[RowSelect].FindControl("LinkListApps");
            Label LblappName = (Label)GridViewApplication.Rows[RowSelect].FindControl("LabelListApps");
            HiddenField appID = (HiddenField)GridViewApplication.Rows[RowSelect].FindControl("HiddenFieldAppID");
            LabelAppTitle.Text = LblappName.Text.ToString();
            App_ID = Guid.Parse(appID.Value.ToString());

            getDataRoleDDL(App_ID);

            HiddenAppIdSelect.Value = App_ID.ToString(); 
            HiddenAppNameSelect.Value = LblappName.Text.ToString(); 

            divkonten.Visible = true;
            if (LabelAppTitle.Text != "Application" && LabelOrgTitle.Text != "Organization")
            {
                Int64 Org_ID = Int64.Parse(HiddenOrgIdSelect.Value.ToString());  
                getMappingUserRoleExpand(App_ID, Org_ID);
                divWaiting.Visible = false;

                getDataUserDDLfilter();
            }
            Search_TextApp.Text = "";
            //ScriptManager.RegisterStartupScript(Page, GetType(), "setscroll", "setAppScroll();", true);
        }

        //fungsi untuk menjalankan aksi saat side organization dipilih dan diklik
        protected void LinkListOrg_Click(object sender, EventArgs e)
        {
            Int64 Org_ID;

            int RowSelectOld = int.Parse(HiddenOrgRowSelect.Value.ToString()); 
            GridViewRow rowOld = GridViewOrganization.Rows[RowSelectOld];
            rowOld.BackColor = Color.Transparent;

            int RowSelect = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;
            GridViewRow rowNew = GridViewOrganization.Rows[RowSelect];
            rowNew.BackColor = Color.LightGray;

            HiddenOrgRowSelect.Value = RowSelect.ToString(); 

            LinkButton orgName = (LinkButton)GridViewOrganization.Rows[RowSelect].FindControl("LinkListOrg");
            Label LblorgName = (Label)GridViewOrganization.Rows[RowSelect].FindControl("LabelListOrg");
            HiddenField orgID = (HiddenField)GridViewOrganization.Rows[RowSelect].FindControl("HiddenFieldOrgID");
            LabelOrgTitle.Text = LblorgName.Text.ToString();
            Org_ID = Int64.Parse(orgID.Value.ToString());

            HiddenOrgIdSelect.Value = Org_ID.ToString(); 
            HiddenOrgNameSelect.Value = LblorgName.Text.ToString();  

            if (LabelAppTitle.Text != "Application" && LabelOrgTitle.Text != "Organization")
            {
                Guid App_ID = Guid.Parse(HiddenAppIdSelect.Value.ToString()); 
                getMappingUserRoleExpand(App_ID, Org_ID);
                divWaiting.Visible = false;

                getDataUserDDLfilter();
            }
            Search_TextOrg.Text = "";
        }

        //fungsi untuk build table dengan data mapping yang sudah terrancang
        public void getMappingUserRoleExpand(Guid application_id, Int64 organization_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Guid appid_send = application_id;
            string appname_send = HiddenAppNameSelect.Value.ToString(); 
            Int64 orgid_send = organization_id;
            string orgname_send = HiddenOrgNameSelect.Value.ToString(); 

            divContentMapping.InnerHtml = "";
            StringBuilder resultHTML = new StringBuilder();
            resultHTML.Append("");

            try
            {
                List<ViewListRole> ListRoleData = new List<ViewListRole>();
                var Get_Role = clsViewListRole.GetDataRole_forPage(application_id);
                var Get_DataRole = JsonConvert.DeserializeObject<Result_Data_Role_List>(Get_Role.Result.ToString());

                ListRoleData = Get_DataRole.list;

                if (ListRoleData.Count() > 0)
                {
                    DataTable dt_role = new DataTable();
                    DataTable dt_result = Helper.ToDataTable(ListRoleData);

                    DataRow[] foundRows = dt_result.Select("is_active =" + true);
                    if (foundRows.Length > 0)
                    {
                        dt_role = foundRows.CopyToDataTable();

                        //fungsi sorting datatable
                        dt_role.DefaultView.Sort = "role_name ASC";
                        dt_role = dt_role.DefaultView.ToTable();
                    }

                    if (dt_role.Rows.Count > 0)
                    {
                        //first table - data role
                        resultHTML.Append("<table id=\"tbl_roledetail\" name=\"tbl_roledetail\" border=\"0\" width=\"100%\">");

                        for (int i = 0; i < dt_role.Rows.Count; i++)
                        {
                            resultHTML.Append(
                                            "<tr class=\"headerRole boxShadow\">" +
                                            "<td class=\"headerlabel\" style=\"height:40px; width:80%; cursor:pointer;\"> <b> <i class=\"fa fa-plus-square iconRole\"></i> &nbsp;" + dt_role.Rows[i]["role_name"].ToString() + " </b> </td>" +
                                            "<td> <button type=\"button\" class=\"TombolAddData btn btn-primary TeksNormal text-left\" style=\"color:white;\" onclick=\"AddMappingUser('" + orgid_send.ToString() + "','" + orgname_send.ToString().Replace(" ", "_") + "','" + appid_send.ToString() + "','" + appname_send.ToString().Replace(" ", "_") + "','" + dt_role.Rows[i]["role_id"].ToString() + "','" + dt_role.Rows[i]["role_name"].ToString().Replace(" ", "_") + "')\" ><i class=\"fa fa-plus\"></i>&nbsp; Map User</button> </td>" +
                                            "</tr>");

                            List<ViewListUser> ListUserData = new List<ViewListUser>();
                            var Get_User = clsViewListUser.GetDataUser(organization_id, application_id, Guid.Parse(dt_role.Rows[i]["role_id"].ToString()));
                            var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_User_List>(Get_User.Result.ToString());

                            ListUserData = Get_DataUser.list;
                            string iscek = "";

                            if (ListUserData.Count() > 0)
                            {
                                DataTable dt_userr = Helper.ToDataTable(ListUserData);
                                //fungsi sorting datatable
                                dt_userr.DefaultView.Sort = "user_name ASC";
                                dt_userr = dt_userr.DefaultView.ToTable();

                                if (dt_userr.Rows.Count > 0)
                                {
                                    resultHTML.Append("<tr class=\"header\"> <td colspan=\"2\">");
                                    resultHTML.Append("<table id=\"tbl_userdetail\" name=\"tbl_userdetail\" border=\"0\" width=\"100%\" class=\"table table-striped\">");  //second table - data user

                                    for (int j = 0; j < dt_userr.Rows.Count; j++)
                                    {
                                        if (dt_userr.Rows[j]["is_active"].ToString() == "True")
                                        {
                                            iscek = "checked";
                                        }
                                        else
                                        {
                                            iscek = "unchecked";
                                        }

                                        if (dt_userr.Rows[j]["user_name"].ToString() == Helper.UserLogin(this.Page))
                                        {
                                            resultHTML.Append(
                                                   "<tr style=\"opacity:0.5;\">" +
                                                   "<td style=\"width:15%; border-width:0\"> <div style=\"margin-left:10px;\"><input ID=\"CheckBoxStatus\" disabled class=\"CheckBoxSwitch\" type=\"checkbox\" " + iscek + " data-toggle=\"toggle\" data-on=\"Active\" data-off=\"Inactive\" data-onstyle=\"success\" data-offstyle=\"default\" data-size=\"mini\"  onchange=\"UpdateStatus('" + dt_userr.Rows[j]["user_role_id"].ToString() + "','" + dt_userr.Rows[j]["user_id"].ToString() + "','" + organization_id + "','" + application_id + "','" + dt_role.Rows[i]["role_id"].ToString() + "','" + dt_userr.Rows[j]["created_by"].ToString().Replace(" ", "_").Replace("\\", "+") + "','" + dt_userr.Rows[j]["created_date"].ToString().Replace(" ", "_") + "','" + dt_userr.Rows[j]["is_active"].ToString() + "')\"> </div> </td>" +
                                                   "<td style=\"width:30%; border-width:0\"> " + dt_userr.Rows[j]["user_name"].ToString() + " </td>" +
                                                   "<td style=\"width:50%; border-width:0\"> " + dt_userr.Rows[j]["full_name"].ToString() + " </td>" +
                                                   "<td style=\"width:5%; border-width:0\"> <a style=\"text-decoration: none;\"> <img alt=\"\" class=\"ic_Switch\" src=\"../Assets/Icons/ic_Switch.svg\" /> </a>" + " </td>" +
                                                   "</tr>");
                                        }
                                        //akan digunakan ketika sudah ada balikan data isactive dari shg_user
                                        //else if(dt_userr.Rows[j]["is_active"].ToString().ToLower() == "false")
                                        //{
                                        //    resultHTML.Append(
                                        //           "<tr>" +
                                        //           "<td style=\"width:15%; border-width:0\"> <div style=\"margin-left:10px;\"><input ID=\"CheckBoxStatus\" class=\"CheckBoxSwitch\" type=\"checkbox\" " + iscek + " data-toggle=\"toggle\" data-on=\"Active\" data-off=\"Inactive\" data-onstyle=\"success\" data-offstyle=\"default\" data-size=\"mini\"  onchange=\"UpdateStatus('" + dt_userr.Rows[j]["user_role_id"].ToString() + "','" + dt_userr.Rows[j]["user_id"].ToString() + "','" + organization_id + "','" + application_id + "','" + dt_role.Rows[i]["role_id"].ToString() + "','" + dt_userr.Rows[j]["created_by"].ToString().Replace(" ", "_").Replace("\\", "+") + "','" + dt_userr.Rows[j]["created_date"].ToString().Replace(" ", "_") + "','" + dt_userr.Rows[j]["is_active"].ToString() + "')\"> </div> </td>" +
                                        //           "<td style=\"width:30%; border-width:0; text-decoration: line-through; color: darkgrey;\" title=\"User Inactive\"> " + dt_userr.Rows[j]["user_name"].ToString() + " </td>" +
                                        //           "<td style=\"width:50%; border-width:0\"> " + dt_userr.Rows[j]["full_name"].ToString() + " </td>" +
                                        //           "<td style=\"width:5%; border-width:0\"> <a href=\"#\" style=\"text-decoration: none;\" onclick=\"SwitchTheRole('" + orgid_send.ToString() + "','" + orgname_send.ToString().Replace(" ", "_") + "','" + appid_send.ToString() + "','" + appname_send.ToString().Replace(" ", "_") + "','" + dt_userr.Rows[j]["user_role_id"].ToString() + "','" + dt_userr.Rows[j]["user_id"].ToString() + "','" + dt_userr.Rows[j]["user_name"].ToString().Replace("\\", "+") + "','" + dt_userr.Rows[j]["created_by"].ToString().Replace(" ", "_").Replace("\\", "+") + "','" + dt_userr.Rows[j]["created_date"].ToString().Replace(" ", "_") + "','" + dt_userr.Rows[j]["is_active"].ToString() + "')\"> <img alt=\"\" class=\"ic_Switch\" src=\"../Assets/Icons/ic_Switch.svg\" /> </a>" + " </td>" +
                                        //           "</tr>");
                                        //}
                                        else
                                        {
                                            resultHTML.Append(
                                                   "<tr>" +
                                                   "<td style=\"width:15%; border-width:0\"> <div style=\"margin-left:10px;\"><input ID=\"CheckBoxStatus\" class=\"CheckBoxSwitch\" type=\"checkbox\" " + iscek + " data-toggle=\"toggle\" data-on=\"Active\" data-off=\"Inactive\" data-onstyle=\"success\" data-offstyle=\"default\" data-size=\"mini\"  onchange=\"UpdateStatus('" + dt_userr.Rows[j]["user_role_id"].ToString() + "','" + dt_userr.Rows[j]["user_id"].ToString() + "','" + organization_id + "','" + application_id + "','" + dt_role.Rows[i]["role_id"].ToString() + "','" + dt_userr.Rows[j]["created_by"].ToString().Replace(" ", "_").Replace("\\", "+") + "','" + dt_userr.Rows[j]["created_date"].ToString().Replace(" ", "_") + "','" + dt_userr.Rows[j]["is_active"].ToString() + "')\"> </div> </td>" +
                                                   "<td style=\"width:30%; border-width:0\"> " + dt_userr.Rows[j]["user_name"].ToString() + " </td>" +
                                                   "<td style=\"width:50%; border-width:0\"> " + dt_userr.Rows[j]["full_name"].ToString() + " </td>" +
                                                   "<td style=\"width:5%; border-width:0\"> <a href=\"#\" style=\"text-decoration: none;\" onclick=\"SwitchTheRole('" + orgid_send.ToString() + "','" + orgname_send.ToString().Replace(" ", "_") + "','" + appid_send.ToString() + "','" + appname_send.ToString().Replace(" ", "_") + "','" + dt_userr.Rows[j]["user_role_id"].ToString() + "','" + dt_userr.Rows[j]["user_id"].ToString() + "','" + dt_userr.Rows[j]["user_name"].ToString().Replace("\\","+") + "','" + dt_userr.Rows[j]["created_by"].ToString().Replace(" ", "_").Replace("\\", "+") + "','" + dt_userr.Rows[j]["created_date"].ToString().Replace(" ", "_") + "','" + dt_userr.Rows[j]["is_active"].ToString() + "')\"> <img alt=\"\" class=\"ic_Switch\" src=\"../Assets/Icons/ic_Switch.svg\" /> </a>" + " </td>" +
                                                   "</tr>");
                                        }

                                    }

                                    resultHTML.Append(" </table> ");  //end of second table - data user
                                    resultHTML.Append(" </td> </tr>  ");
                                }
                            }
                            resultHTML.Append("<tr class=\"header\"> <td colspan=\"3\"> <br /> </td> </tr>");
                        }
                        resultHTML.Append("<tr class=\"headerRole\"> <td colspan=\"2\"> <br /> </td> </tr>");
                        resultHTML.Append("</table>");
                        //end of first table - data role

                        divContentMapping.InnerHtml = resultHTML.ToString();
                    }
                    else
                    {
                        resultHTML.Append("<label class=\"boxShadow\" style=\"margin-top:7px; height:26px; padding-left:0px;\">No Data</label>");
                        divContentMapping.InnerHtml = resultHTML.ToString();
                    }
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Role.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    resultHTML.Append("<label class=\"boxShadow\" style=\"margin-top:7px; height:26px; padding-left:0px;\">No Data</label>");
                    divContentMapping.InnerHtml = resultHTML.ToString();
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getMappingUserRoleExpand", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getMappingUserRoleExpand", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi button save add data
        protected void Add_ButtonSaveMapUser_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            UserRole model_UR = new UserRole();
            int isFail = 0;
            int isSuccess = 0;

            model_UR.organization_id = Int64.Parse(HiddenOrgID.Value.ToString());
            model_UR.application_id = Guid.Parse(HiddenAppID.Value.ToString());
            model_UR.role_id = Guid.Parse(HiddenRoleID.Value.ToString());
            model_UR.is_active = true;
            model_UR.created_by = Helper.UserLogin(this.Page);
            model_UR.created_date = DateTime.Now;
            model_UR.modified_by = null;
            model_UR.modified_date = DateTime.Now;

            var Status = "";
            var Message = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                model_UR.user_id = Guid.Parse(dt.Rows[i]["user_id"].ToString());
                
                var hasil = clsUserRole.PostDataUserRole(model_UR);

                var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
                Status = Response.Property("status").Value.ToString();
                Message = Response.Property("message").Value.ToString();

                if (Status == "Fail")
                {
                    isFail = 1;
                }
                else
                {
                    isSuccess = 1;
                }
            }

            if (isSuccess == 1)
            {
                if (isFail == 1)
                {
                    ShowToastr("Mapping User Role : data successfully mapped, but some data may be not mapped. Please check again", "Save Success", "success");
                }
                else
                {
                    ShowToastr("Mapping User Role : data successfully mapped", "Save Success", "success");
                }

                clearFormAdd();

                getMappingUserRoleExpand(model_UR.application_id, model_UR.organization_id);

                //Add_DDLUsername.SelectedValue = Guid.Empty.ToString();
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refresh", "$('.selectpicker').selectpicker('refresh');", addScriptTags: true);
            }
            else if (isFail == 1)
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Add_ButtonSaveMapUser_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button switch change role
        protected void SW_ButtonSwitchRole_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            p_sw.InnerText = "";
            UserRole model_UR = new UserRole();

            model_UR.user_role_id = Int64.Parse(SW_HiddenURID.Value.ToString());
            model_UR.user_id = Guid.Parse(SW_HiddenUserID.Value.ToString());
            model_UR.organization_id = Int64.Parse(SW_HiddenOrgID.Value.ToString());
            model_UR.application_id = Guid.Parse(SW_HiddenAppID.Value.ToString());
            model_UR.role_id = Guid.Parse(SW_DDLRolename.SelectedValue.ToString());
            model_UR.is_active = bool.Parse(SW_HiddenIsActive.Value.ToString());
            model_UR.created_by = SW_HiddenCreateBy.Value.ToString();
            model_UR.created_date = DateTime.Parse(SW_HiddenCreateDate.Value.ToString());
            model_UR.modified_by = Helper.UserLogin(this.Page);
            model_UR.modified_date = DateTime.Now;
            
            var hasil = clsUserRole.PutDataUserRole(model_UR.user_role_id, model_UR);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                ShowToastr("Mapping User Role : data successfully switched", "Save Success", "success");
                getMappingUserRoleExpand(model_UR.application_id, model_UR.organization_id);

                SW_DDLRolename.SelectedValue = Guid.Empty.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refresh", "$('.selectpicker').selectpicker('refresh');", addScriptTags: true);
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "SW_ButtonSwitchRole_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button update status active
        protected void ButtonChangeStatus_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            UserRole model_UR = new UserRole();

            model_UR.user_role_id = Int64.Parse(HideUR_ID.Value.ToString());
            model_UR.user_id = Guid.Parse(HideUR_userID.Value.ToString());
            model_UR.organization_id = Int64.Parse(HideUR_orgID.Value.ToString());
            model_UR.application_id = Guid.Parse(HideUR_appID.Value.ToString());
            model_UR.role_id = Guid.Parse(HideUR_roleID.Value.ToString());
            if (bool.Parse(HideUR_Active.Value.ToString()) == true)
            {
                model_UR.is_active = false;
            }
            else if (bool.Parse(HideUR_Active.Value.ToString()) == false)
            {
                model_UR.is_active = true;
            }
            model_UR.created_by = HideUR_Creatby.Value.ToString();
            model_UR.created_date = DateTime.Parse(HideUR_Creatdate.Value.ToString());
            model_UR.modified_by = Helper.UserLogin(this.Page);
            model_UR.modified_date = DateTime.Now;
           
            var hasil = clsUserRole.PutDataUserRole(model_UR.user_role_id, model_UR);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                ShowToastr("Mapping User Role : status successfully updated", "Save Success", "success");
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonChangeStatus_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk disable item DDL yang sudah termapping
        //protected void ButtonDisableDDL_Click(object sender, EventArgs e)
        //{
        //    getDataUserDDL();

        //    List<ViewListUser> ListUserDataCek = new List<ViewListUser>();
        //    var Get_UserCek = clsUserRole.GetDataUser_byOrgApp(Int64.Parse(HiddenOrgID.Value.ToString()), Guid.Parse(HiddenAppID.Value.ToString()));
        //    var Get_DataUserCek = JsonConvert.DeserializeObject<Result_Data_User_List>(Get_UserCek.Result.ToString());

        //    ListUserDataCek = Get_DataUserCek.list;

        //    if (ListUserDataCek.Count() > 0)
        //    {
        //        foreach (ListItem item in Add_DDLUsername.Items)
        //        {
        //            for (int i = 0; i < ListUserDataCek.Count; i++)
        //            {
        //                if (item.Value.ToString() == ListUserDataCek[i].user_id.ToString())
        //                {
        //                    //item.Attributes.Add("disabled", "disabled");
        //                    //item.Attributes.Add("style", "color:lightgrey;");
        //                    item.Text = item.Text + " - (already mapping)";
        //                }
        //            }
        //        }
        //    }

        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refresh", "$('.selectpicker').selectpicker('refresh');", addScriptTags: true);
        //}

        protected void ButtonAjaxSearchUsername_Click(object sender, EventArgs e)
        {
            Guid userID = Guid.Parse(HiddenUsrID.Value.ToString());
            string userName = HiddenUsrName.Value.ToString();
            Int64 orgID = Int64.Parse(HiddenOrgID.Value.ToString());
            Guid appID = Guid.Parse(HiddenAppID.Value.ToString());
            Guid roleID = Guid.Parse(HiddenRoleID.Value.ToString());

            int cekdatasama = 0;
            p_Add.InnerText = "";

            foreach (DataRow row in dt.Rows)
            {
                if (row["user_name"].ToString() == userName)
                {
                    cekdatasama = 1;
                }
            }

            if (cekdatasama == 0)
            {
                if (getDataUserRoleExist(userID, orgID, appID) == true)
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Username already Mapping in this application!";
                }
                else
                {
                    AddToDataTable_UserTemp(HiddenUsrID.Value.ToString(), userName);
                    BindGrid_UserTemp();
                }
            }
            else
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:red;");
                p_Add.InnerText = "Input Username cannot be the same!";
            }

            txtItemUsername_AC.Text = "";
        }

        //jangan dihapus
        //public void getMappingUserRole(Int64 organization_id)
        //{
        //    divContentMapping.InnerHtml = "";
        //    StringBuilder resultHTML = new StringBuilder();
        //    resultHTML.Append("");

        //    List<ViewListApps> ListAppsData = new List<ViewListApps>();
        //    var Get_Apps = ViewListAppsController.GetDataApps(organization_id);
        //    var Get_DataApps = JsonConvert.DeserializeObject<Result_Data_Apps_List>(Get_Apps.Result.ToString());

        //    ListAppsData = Get_DataApps.list;

        //    if (ListAppsData.Count() > 0)
        //    {
        //        DataTable dt_apps = Helper.ToDataTable(ListAppsData);

        //        if (dt_apps.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt_apps.Rows.Count; i++)
        //            {
        //                resultHTML.Append("Apps : " + dt_apps.Rows[i]["application_name"].ToString() + "<br />");

        //                List<ViewListRole> ListRoleData = new List<ViewListRole>();
        //                var Get_Role = ViewListRoleController.GetDataRole(organization_id, Guid.Parse(dt_apps.Rows[i]["application_id"].ToString()));
        //                var Get_DataRole = JsonConvert.DeserializeObject<Result_Data_Role_List>(Get_Role.Result.ToString());

        //                ListRoleData = Get_DataRole.list;

        //                if (ListRoleData.Count() > 0)
        //                {
        //                    DataTable dt_role = Helper.ToDataTable(ListRoleData);
        //                    for (int j = 0; j < dt_role.Rows.Count; j++)
        //                    {
        //                        resultHTML.Append(" + Role : " + dt_role.Rows[j]["role_name"].ToString() + "<br />");

        //                        List<ViewListUser> ListUserData = new List<ViewListUser>();
        //                        var Get_User = ViewListUserController.GetDataUser(organization_id, Guid.Parse(dt_apps.Rows[i]["application_id"].ToString()), Guid.Parse(dt_role.Rows[j]["role_id"].ToString()));
        //                        var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_User_List>(Get_User.Result.ToString());

        //                        ListUserData = Get_DataUser.list;

        //                        if (ListUserData.Count() > 0)
        //                        {
        //                            DataTable dt_user = Helper.ToDataTable(ListUserData);

        //                            resultHTML.Append("<table border=\"1\">");

        //                            for (int k = 0; k < dt_user.Rows.Count; k++)
        //                            {
        //                                //resultHTML.Append(" - - User : " + dt_user.Rows[k]["user_name"].ToString() + "<br />");
        //                                resultHTML.Append(
        //                                "<tr>" +
        //                                "<td>  - - User : " + dt_user.Rows[k]["user_name"].ToString() + "</td>" +
        //                                "<td> &nbsp; &nbsp; | &nbsp; &nbsp;</td>" + 
        //                                "<td>" + dt_user.Rows[k]["is_active"].ToString() + "</td>" +
        //                                "<td> &nbsp; &nbsp;| &nbsp; &nbsp;</td> " +
        //                                "<td>" + "<button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal\"> Switch </button>" + "</td>" +
        //                                "</tr>");

        //                            }

        //                            resultHTML.Append("</table>");
        //                        }
        //                    }
        //                }
        //                resultHTML.Append("<br />");
        //            }
        //            divContentMapping.InnerHtml = resultHTML.ToString();
        //        }
        //    }
        //    else
        //    {
        //        //no data found
        //    }
        //}

        //public void getMappingUserRoleExpand(Int64 organization_id)
        //{
        //    divContentMapping.InnerHtml = "";
        //    StringBuilder resultHTML = new StringBuilder();
        //    resultHTML.Append("");

        //    List<ViewListApps> ListAppsData = new List<ViewListApps>();
        //    var Get_Apps = ViewListAppsController.GetDataApps(organization_id);
        //    var Get_DataApps = JsonConvert.DeserializeObject<Result_Data_Apps_List>(Get_Apps.Result.ToString());

        //    ListAppsData = Get_DataApps.list;

        //    if (ListAppsData.Count() > 0)
        //    {
        //        DataTable dt_apps = Helper.ToDataTable(ListAppsData);

        //        if (dt_apps.Rows.Count > 0)
        //        {
        //            resultHTML.Append("<table border=\"1\" width=\"100%\">");

        //            for (int i = 0; i < dt_apps.Rows.Count; i++)
        //            {
        //                //resultHTML.Append("Apps : " + dt_apps.Rows[i]["application_name"].ToString() + "<br />");
        //                resultHTML.Append(
        //                                "<tr class=\"headerApp\">" +
        //                                "<td> <b>" + dt_apps.Rows[i]["application_name"].ToString() + " </b> </td>" +
        //                                "<td> &nbsp; </td>" +
        //                                "<td> &nbsp; </td>" +
        //                                "<td> &nbsp; </td>" +
        //                                "<td> &nbsp; </td>" +
        //                                "<td> &nbsp; </td>" +
        //                                "</tr>");

        //                List<ViewListRole> ListRoleData = new List<ViewListRole>();
        //                var Get_Role = ViewListRoleController.GetDataRole(organization_id, Guid.Parse(dt_apps.Rows[i]["application_id"].ToString()));
        //                var Get_DataRole = JsonConvert.DeserializeObject<Result_Data_Role_List>(Get_Role.Result.ToString());

        //                ListRoleData = Get_DataRole.list;

        //                if (ListRoleData.Count() > 0)
        //                {
        //                    DataTable dt_role = Helper.ToDataTable(ListRoleData);
        //                    for (int j = 0; j < dt_role.Rows.Count; j++)
        //                    {
        //                        //resultHTML.Append(" + Role : " + dt_role.Rows[j]["role_name"].ToString() + "<br />");
        //                        resultHTML.Append(
        //                               "<tr class=\"header\">" +
        //                               "<td> <i> >> &nbsp;" + dt_role.Rows[j]["role_name"].ToString() + " </i> </td>" +
        //                               "<td> &nbsp; </td>" +
        //                               "<td> &nbsp; </td>" +
        //                               "<td> &nbsp; </td>" +
        //                               "<td> &nbsp; </td>" +
        //                               "<td> &nbsp; </td>" +
        //                               "</tr>");

        //                        List<ViewListUser> ListUserData = new List<ViewListUser>();
        //                        var Get_User = ViewListUserController.GetDataUser(organization_id, Guid.Parse(dt_apps.Rows[i]["application_id"].ToString()), Guid.Parse(dt_role.Rows[j]["role_id"].ToString()));
        //                        var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_User_List>(Get_User.Result.ToString());

        //                        ListUserData = Get_DataUser.list;

        //                        if (ListUserData.Count() > 0)
        //                        {
        //                            DataTable dt_user = Helper.ToDataTable(ListUserData);



        //                            for (int k = 0; k < dt_user.Rows.Count; k++)
        //                            {
        //                                //resultHTML.Append(" - - User : " + dt_user.Rows[k]["user_name"].ToString() + "<br />");
        //                                resultHTML.Append(
        //                                "<tr>" +
        //                                "<td>" + dt_user.Rows[k]["user_name"].ToString() + "</td>" +
        //                                "<td>" + dt_user.Rows[k]["is_active"].ToString() + "</td>" +
        //                                "<td>" + "<button type=\"button\" class=\"btn btn-danger\" data-toggle=\"modal\" data-target=\"#myModal\"> Switch </button>" + "</td>" +
        //                                "<td> &nbsp; </td>" +
        //                                "<td> &nbsp; </td>" +
        //                                "<td> &nbsp; </td>" +
        //                                "</tr>");

        //                            }
        //                        }
        //                    }
        //                }
        //                resultHTML.Append("<tr class=\"header\"> <td colspan=\"6\"> <br /> </td> </tr>");
        //            }

        //            resultHTML.Append("</table>");

        //            divContentMapping.InnerHtml = resultHTML.ToString();


        //        }
        //    }
        //    else
        //    {
        //        //no data found
        //    }
        //}
    }
}