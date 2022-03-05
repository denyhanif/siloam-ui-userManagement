using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using System.Web.UI.HtmlControls;

using Siloam.Ui.UserManagement.API_Code.Models;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Siloam.Ui.UserManagement.Pages.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Siloam.Ui.UserManagement.Pages
{
    public partial class UserMaster : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (!IsPostBack)
            {
                loadLinkActive();
                getDataUser();
                getDataUserHope();
                initDataUserHope();
                initDataUserHope_Edit();
                GetDataHospitalUnit();

                LabelNewPass.Text = ConfigurationManager.AppSettings["DefaultPassword"].ToString();
                HiddenFlagCari.Value = 0.ToString();

                DataTable dt_login = (DataTable)Session[Helper.Session_DataLogin];
                if (dt_login == null)
                {
                    Response.Redirect("~/Pages/Login_page.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
               
                string role_branchadmin = dt_login.Rows[0]["role_id"].ToString();
                if (role_branchadmin == ConfigurationManager.AppSettings["roleadmincabang"].ToString()) //role admin cabang
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideAddUser(); hideEditUser();", true);
                }
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
            myObject = (HtmlContainerControl)Master.FindControl("divBoxUser");

            myObject.Style.Add("Background-color", "#1a2269");
            myObject.Style.Add("border-right", "5px solid #ffd800");
            myObject.Style.Add("color", "#f2c22c");
        }

        //fungsi untuk get data user
        public void getDataUser()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                string arrumsorgids = generateListUmsOrgIdByLogin();

                List<User> ListUserData = new List<User>();
                //var Get_User = clsUser.GetDataUser();
                var Get_User = clsUser.GetDataUserByOrg(arrumsorgids);
                var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

                ListUserData = Get_DataUser.list;

                if (ListUserData.Count() > 0)
                {
                    DataTable dt_user = Helper.ToDataTable(ListUserData);

                    //fungsi sorting datatable
                    dt_user.DefaultView.Sort = "user_name ASC";
                    dt_user = dt_user.DefaultView.ToTable();

                    GridViewUserSimple.DataSource = dt_user;
                    GridViewUserSimple.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_User.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }

                DataTable dt_login = (DataTable)Session[Helper.Session_DataLogin];
                if (dt_login == null)
                {
                    Response.Redirect("~/Pages/Login_page.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }

                string role_branchadmin = dt_login.Rows[0]["role_id"].ToString();
                if (role_branchadmin == ConfigurationManager.AppSettings["roleadmincabang"].ToString()) //role admin cabang
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideAddUser(); hideEditUser();", true);
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUser", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUser", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi link pagination pada gridview
        protected void GridViewUserSimple_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewUserSimple.PageIndex = e.NewPageIndex;
            if (HiddenFlagCari.Value == "0")
            {
                getDataUser();
            }
            else if (HiddenFlagCari.Value == "1")
            {
                getDataUserSearch(Search_masterData.Text);
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

        //inisialisasi data hope user ID - ADD
        public void initDataUserHope()
        {
            try
            {
                DataTable dt_userHope = (DataTable)Session["dataUserHope"];
                dt_userHope = dt_userHope.Rows.Cast<System.Data.DataRow>().Take(100).CopyToDataTable();

                GridViewUserHope.DataSource = dt_userHope;
                GridViewUserHope.DataBind();
            }
            catch (Exception ex)
            {
                ShowToastr("Please Logout first and Login again.", "Initialize Failed!", "warning");
            }
        }

        //fungsi klik saat memilih hope user ID - ADD
        protected void LinkListUserHope_Click(object sender, EventArgs e)
        {
            Int64 userHope_ID;
            string userHope_name;

            int RowSelect = ((GridViewRow)(((Button)sender).Parent.Parent)).RowIndex;
            GridViewRow rowNew = GridViewUserHope.Rows[RowSelect];

            HiddenRowSelect.Value = RowSelect.ToString();  //--ViewState["rowindex"] = RowSelect;

            Button userNAME = (Button)GridViewUserHope.Rows[RowSelect].FindControl("LinkListUserHope");
            HiddenField userHopeID = (HiddenField)GridViewUserHope.Rows[RowSelect].FindControl("HiddenFieldUserHopeID");
            HiddenField userHopename = (HiddenField)GridViewUserHope.Rows[RowSelect].FindControl("HiddenFieldUserHopename");

            userHope_ID = Int64.Parse(userHopeID.Value.ToString());
            HiddenUserIdSelect.Value = userHope_ID.ToString();
            userHope_name = userHopename.Value.ToString();
            HiddenUsernameSelect.Value = userHope_name.ToString();

            if (getDataHOPEIDExist(userHope_ID) == true)
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:red;");
                p_Add.InnerText = "Hope User ID already used by another user!";

                HiddenUserIdSelect.Value = "0";
                HiddenUsernameSelect.Value = "unnamed";
                TextBoxDDLHope.Text = "unnamed";
                Add_TextFullname.Text = "";
            }
            else
            {
                if (Add_DDLUserType.SelectedValue != "2" || Add_DDLUserType.SelectedValue != "1")
                {
                    Add_TextFullname.Text = userHope_name;
                }
                TextBoxDDLHope.Text = userNAME.Text.ToString();
            }

            Add_TextDDLHopeID.Text = "";
            initDataUserHope();
        }

        //fungsi search data pada dropdown search - ADD
        protected void SearchHopeUser_Click(object sender, EventArgs e)
        {
            if (Add_TextDDLHopeID.Text != "")
            {
                DataTable dt_userHope = (DataTable)Session["dataUserHope"];
                DataTable dt_result = new DataTable();

                DataRow[] foundRows = dt_userHope.Select("userName LIKE '%" + Add_TextDDLHopeID.Text.ToString() + "%'");
                if (foundRows.Length > 0)
                {
                    dt_result = foundRows.CopyToDataTable();
                    dt_result = dt_result.Rows.Cast<System.Data.DataRow>().Take(100).CopyToDataTable();
                }

                GridViewUserHope.DataSource = dt_result;
                GridViewUserHope.DataBind();               
            }
            else
            {
                initDataUserHope();
            }
            this.Page.SetFocus(Add_TextDDLHopeID.ClientID);
        }

        //inisialisasi data hope user ID - EDIT
        public void initDataUserHope_Edit()
        {
            try
            {
                DataTable dt_userHope = (DataTable)Session["dataUserHope"];
                dt_userHope = dt_userHope.Rows.Cast<System.Data.DataRow>().Take(100).CopyToDataTable();

                Edit_GridViewUserHope.DataSource = dt_userHope;
                Edit_GridViewUserHope.DataBind();
            }
            catch (Exception ex)
            {
                ShowToastr("Please Logout first and Login again.", "Initialize Failed!", "warning");
            }
        }
        
        //fungsi klik saat memilih hope user ID - EDIT
        protected void Edit_LinkListUserHope_Click(object sender, EventArgs e)
        {
            Int64 userHope_ID;

            int RowSelect = ((GridViewRow)(((Button)sender).Parent.Parent)).RowIndex;
            GridViewRow rowNew = Edit_GridViewUserHope.Rows[RowSelect];

            Edit_HiddenRowSelect.Value = RowSelect.ToString();  //--ViewState["rowindex"] = RowSelect;

            Button userNAME = (Button)Edit_GridViewUserHope.Rows[RowSelect].FindControl("Edit_LinkListUserHope");
            HiddenField userHopeID = (HiddenField)Edit_GridViewUserHope.Rows[RowSelect].FindControl("Edit_HiddenFieldUserHopeID");

            userHope_ID = Int64.Parse(userHopeID.Value.ToString());

            if (getDataHOPEIDExist(userHope_ID) == true)
            {
                if (userHope_ID != Int64.Parse(Edit_TempUserHopeID.Value.ToString()))
                {
                    p_Edit.Attributes.Remove("style");
                    p_Edit.Attributes.Add("style", "display:block; color:red;");
                    p_Edit.InnerText = "Hope User ID already used by another user!";

                    Edit_HiddenUserIdSelect.Value = userHope_ID.ToString();
                    Edit_TextBoxDDLHope.Text = Edit_TempUserHopeUsername.Value.ToString();
                }
                else
                {
                    p_Edit.InnerText = "";
                    Edit_HiddenUserIdSelect.Value = userHope_ID.ToString();
                    Edit_TextBoxDDLHope.Text = userNAME.Text.ToString();
                }
            }
            else
            {
                p_Edit.InnerText = "";
                Edit_HiddenUserIdSelect.Value = userHope_ID.ToString();
                Edit_TextBoxDDLHope.Text = userNAME.Text.ToString();
            }

            Edit_TextDDLHopeID.Text = "";
            initDataUserHope_Edit();
        }

        //fungsi search data pada dropdown search - EDIT
        protected void Edit_SearchHopeUser_Click(object sender, EventArgs e)
        {
            if (Edit_TextDDLHopeID.Text != "")
            {
                DataTable dt_userHope = (DataTable)Session["dataUserHope"];
                DataTable dt_result = new DataTable();

                DataRow[] foundRows = dt_userHope.Select("userName LIKE '%" + Edit_TextDDLHopeID.Text.ToString() + "%'");
                if (foundRows.Length > 0)
                {
                    dt_result = foundRows.CopyToDataTable();
                    dt_result = dt_result.Rows.Cast<System.Data.DataRow>().Take(100).CopyToDataTable();
                }

                Edit_GridViewUserHope.DataSource = dt_result;
                Edit_GridViewUserHope.DataBind();
            }
            else
            {
                initDataUserHope_Edit();
            }
            this.Page.SetFocus(Edit_TextDDLHopeID.ClientID);
        }

        //fungsi load data hope user id saat akan edit data
        protected void Edit_ButtonLoadHopeID_Click(object sender, EventArgs e)
        {
            DataTable dt_userHope = (DataTable)Session["dataUserHope"];
            DataTable dt_result = new DataTable();

            DataRow[] foundRows = dt_userHope.Select("userId = '" + Edit_HiddenUserIdSelect.Value.ToString() + "'");
            if (foundRows.Length > 0)
            {
                dt_result = foundRows.CopyToDataTable();
                Edit_TextBoxDDLHope.Text = dt_result.Rows[0]["userName"].ToString();

                Edit_TempUserHopeID.Value = dt_result.Rows[0]["userID"].ToString();
                Edit_TempUserHopeUsername.Value = dt_result.Rows[0]["userName"].ToString();
            }

            string temp = Edit_HiddenUserID.Value.ToString();
            List<DoctorSIP> SIP = new List<DoctorSIP>();
            SIP = GetSIPByUserId(Guid.Parse(Edit_HiddenUserID.Value.ToString()));
            DataTable dtSIP = Helper.ToDataTable(SIP);
            gvw_EditUnitSIP.DataSource = dtSIP;
            gvw_EditUnitSIP.DataBind();
        }

        //fungsi untuk mengecek data yang diiputkan apakah sudah ada di DB atau belum
        public bool getDataUserExist(string userName)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool eksis = false;
            try
            {
                List<User> ListUserData = new List<User>();
                var Get_User = clsUser.GetDataUserByUsername(userName);
                var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

                ListUserData = Get_DataUser.list;

                if (ListUserData.Count() > 0)
                {
                    eksis = true;
                }
                else
                {
                    eksis = false;
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", userName, "getDataUserExist", StartTime, "OK", userName, "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", userName, "getDataUserExist", StartTime, "ERROR", userName, "", "", exx.Message));
            }
            return eksis;
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

        //fungsi untuk menyesuaikan form add dengan type user yg dipilih
        protected void Add_DDLUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Add_DDLUserType.SelectedValue == "1") //ProInt
            {
                Add_LabelBintangBday.Visible = true;

                Add_TextFullname.Enabled = false;
                Add_TextEmail.Enabled = true;

                Add_TextFullname.ToolTip = "Fullname will automatically fill from your Proint data.";
            }
            else if (Add_DDLUserType.SelectedValue == "2") //AD
            {
                Add_LabelBintangBday.Visible = false;

                Add_TextFullname.Enabled = false;
                Add_TextEmail.Enabled = false;

                Add_TextFullname.ToolTip = "";
            }
            else
            {
                Add_LabelBintangBday.Visible = false;

                Add_TextFullname.Enabled = true;
                Add_TextEmail.Enabled = true;

                Add_TextFullname.ToolTip = "";
            }
            clearForm();
        }

        //fungsi untuk validasi untuk data yang diiputkan apakah sudah ada di DB atau belum
        protected void Add_TextUsername_TextChanged(object sender, EventArgs e)
        {
            p_Add.InnerText = "";

            if (Add_DDLUserType.SelectedValue == "1")  //Proint
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(Add_TextUsername.Text, "[^0-9]"))
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Please enter only numbers!";
                    Add_TextUsername.Text = "";
                }
                else
                {
                    if (getDataUserExist(Add_TextUsername.Text.ToString()) == true)
                    {
                        p_Add.Attributes.Remove("style");
                        p_Add.Attributes.Add("style", "display:block; color:red;");
                        p_Add.InnerText = "Username Already Exist!";

                        Add_TextFullname.Text = "";
                        Add_TextEmail.Text = "";
                        Add_TextUsername.Text = "";
                    }
                }
            }
            else if (Add_DDLUserType.SelectedValue == "2")  //AD
            {
                string DomainPlusUsername = "";
                string userDomain = "";
                string userName = Add_TextUsername.Text.ToString();

                using (var contextt = new PrincipalContext(ContextType.Domain))
                {
                    //contoh username : "SILOAMHOSPITALS\\agus.wiranata"
                    using (var user = UserPrincipal.FindByIdentity(contextt, userName))
                    {
                        if (user != null)
                        {                         
                            userDomain = getDomainfromAD(user.DistinguishedName);

                            DomainPlusUsername = userDomain + "+" + userName;

                            if (getDataUserExist(DomainPlusUsername) == false)
                            {
                                Add_TextFullname.Text = user.Name;
                                Add_TextEmail.Text = user.UserPrincipalName;
                                Add_TextUsername.Text = userDomain + @"\" + userName;
                            }
                            else
                            {
                                p_Add.Attributes.Remove("style");
                                p_Add.Attributes.Add("style", "display:block; color:red;");
                                p_Add.InnerText = "Username Already Exist!";

                                Add_TextFullname.Text = "";
                                Add_TextEmail.Text = "";
                                Add_TextUsername.Text = "";
                            }
                        }
                        else
                        {
                            p_Add.Attributes.Remove("style");
                            p_Add.Attributes.Add("style", "display:block; color:red;");
                            p_Add.InnerText = "Username Not Found!";

                            Add_TextFullname.Text = "";
                            Add_TextEmail.Text = "";
                            Add_TextUsername.Text = "";
                        }
                    }
                }
            }
            else
            {
                if (getDataUserExist(Add_TextUsername.Text.ToString()) == true)
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Username Already Exist!";

                    Add_TextFullname.Text = "";
                    Add_TextEmail.Text = "";
                    Add_TextUsername.Text = "";
                }
            }
        }

        //get data Domain from Active Directory
        string getDomainfromAD(string DistinguishedName)
        {
            string Domainn = "";
            var x = DistinguishedName;
            var y = x.Split(',');

            for (int i = 0; i < y.Count(); i++)
            {
                var z = y[i].Split('=');
                if (z[0] == "DC" && z[1].Length >= 12)
                {
                    Domainn = z[1].ToString();
                }
            }

            return Domainn;
        }

        //fungsi untuk me reset form input dan notifikasi menjadi kosong kembali
        void clearFormAdd()
        {
            Add_DDLUserType.SelectedValue = "-1";
            Add_TextUsername.Text = "";
            Add_TextFullname.Text = "";
            Add_TextEmail.Text = "";
            Add_TextPhone.Text = "";
            Add_TextBday.Text = "";

            p_Add.InnerText = "";
            p_Edit.InnerText = "";
        }

        //fungsi untuk me reset form input dan notifikasi menjadi kosong kembali - khusus DDL user type
        void clearForm()
        {
            Add_TextUsername.Text = "";
            Add_TextFullname.Text = "";
            Add_TextEmail.Text = "";
            Add_TextPhone.Text = "";
            Add_TextBday.Text = "";

            HiddenUserIdSelect.Value = "0";
            HiddenUsernameSelect.Value = "unnamed";
            TextBoxDDLHope.Text = "unnamed";

            p_Add.InnerText = "";
            p_Edit.InnerText = "";
        }

        //fungsi button save edit data
        protected void Edit_ButtonSaveUser_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            ViewUserSIP model = new ViewUserSIP();
            List<DoctorSIP> model_sip = new List<DoctorSIP>();
            model_sip = GetRowList_DoctorSIP(2);

            User model_user = new User();

            model_user.user_id = Guid.Parse(Edit_HiddenUserID.Value.ToString());
            model_user.user_name = Edit_HiddenUsername.Value.ToString();
            model_user.password = Edit_HiddenPassword.Value.ToString();
            model_user.full_name = Edit_Textfullname.Text.ToString().Trim();
            model_user.hope_user_id = Int64.Parse(Edit_HiddenUserIdSelect.Value.ToString());
            model_user.email = Edit_Textemail.Text.ToString().Trim();

            if (Edit_Textbday.Text == "")
            {
                model_user.birthday = DateTime.Parse(ConfigurationManager.AppSettings["DefaultBirthday"].ToString());
            }
            else
            {
                model_user.birthday = DateTime.Parse(Edit_Textbday.Text.ToString());
            }
            model_user.handphone = Edit_Textphone.Text.ToString().Trim();
            model_user.lock_counter = byte.Parse(Edit_HiddenLcounter.Value.ToString());

            if (Edit_HiddenLastLogin.Value.ToString() == "")
            {
                model_user.last_login_date = null;
            }
            else
            {
                model_user.last_login_date = DateTime.Parse(Edit_HiddenLastLogin.Value.ToString());
            }
            if (Edit_HiddenExpPassDate.Value.ToString() == "")
            {
                model_user.exp_pass_date = null;
            }
            else
            {
                model_user.exp_pass_date = DateTime.Parse(Edit_HiddenExpPassDate.Value.ToString());
            }
            model_user.is_internal = bool.Parse(Edit_HiddenInternal.Value.ToString());
            model_user.is_ad = bool.Parse(Edit_HiddenAD.Value.ToString());
            model_user.is_proint = bool.Parse(Edit_HiddenProint.Value.ToString());
            model_user.is_active = bool.Parse(Edit_HiddenIsActive.Value.ToString());
            model_user.created_by = Edit_HiddenCreateby.Value.ToString();
            model_user.created_date = DateTime.Parse(Edit_HiddenCreatedate.Value.ToString());
            model_user.modified_by = Helper.UserLogin(this.Page);
            model_user.modified_date = DateTime.Now;

            model.model_user = model_user;
            model.model_sip = model_sip;
            var hasil = clsUser.PutDataUserSIP(model_user.user_id, model);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                p_Edit.Attributes.Remove("style");
                p_Edit.Attributes.Add("style", "display:block; color:red;");
                p_Edit.InnerText = "Save Failed!";
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalEditUser').modal('hide');", addScriptTags: true);
                ShowToastr("User : data successfully changed", "Save Success", "success");
                getDataUser();
                clearFormAdd();
                ddlEditUnitSIP.SelectedIndex = 0;
                txtEditUnitSIP.Text = "";
                gvw_EditUnitSIP.DataSource = null;
                gvw_EditUnitSIP.DataBind();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Edit_ButtonSaveUser_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button save add data
        protected void Add_ButtonSaveUser_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            ViewUserSIP model = new ViewUserSIP();
            p_Add.InnerText = "";

            List<DoctorSIP> model_sip = new List<DoctorSIP>();
            model_sip = GetRowList_DoctorSIP(1);

            User model_user = new User();

            model_user.user_id = Guid.NewGuid();
            model_user.user_name = Add_TextUsername.Text.Trim();
            model_user.password = ConfigurationManager.AppSettings["DefaultPassword"].ToString();
            model_user.full_name = Add_TextFullname.Text.Trim();
            model_user.hope_user_id = Int64.Parse(HiddenUserIdSelect.Value.ToString());
            model_user.email = Add_TextEmail.Text.Trim();

            if (Add_TextBday.Text == "")
            {
                model_user.birthday = DateTime.Parse(ConfigurationManager.AppSettings["DefaultBirthday"].ToString());
            }
            else
            {
                model_user.birthday = DateTime.Parse(Add_TextBday.Text.ToString());
            }
            model_user.handphone = Add_TextPhone.Text.Trim();
            model_user.lock_counter = 0;
            model_user.last_login_date = null;
            model_user.exp_pass_date = DateTime.Now.AddMonths(3);

            if (Add_DDLUserType.SelectedValue == "1")  //Proint
            {
                model_user.is_internal = true;
                model_user.is_ad = false;
                model_user.is_proint = true;
            }
            else if (Add_DDLUserType.SelectedValue == "2")  //AD
            {
                model_user.is_internal = true;
                model_user.is_ad = true;
                model_user.is_proint = false;

                model_user.user_name = Add_TextUsername.Text;
            }
            else if (Add_DDLUserType.SelectedValue == "3")  //Internal
            {
                model_user.is_internal = true;
                model_user.is_ad = false;
                model_user.is_proint = false;
            }
            else
            {
                model_user.is_internal = false;
                model_user.is_ad = false;
                model_user.is_proint = false;
            }
            model_user.is_active = true;
            model_user.created_by = Helper.UserLogin(this.Page);
            model_user.created_date = DateTime.Now;
            model_user.modified_by = null;
            model_user.modified_date = DateTime.Now;

            model.model_user = model_user;
            model.model_sip = model_sip;

            var hasil = clsUser.PostDataUser(model);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:red;");
                p_Add.InnerText = "Save Failed!";
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalAddUser').modal('hide');", addScriptTags: true);
                ShowToastr("User : data successfully added", "Save Success", "success");
                getDataUser();
                clearFormAdd();
                ddlAddUnitSIP.SelectedIndex = 0;
                txtAddUnitSIP.Text = "";
                gvw_AddUnitSIP.DataSource = null;
                gvw_AddUnitSIP.DataBind();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Add_ButtonSaveUser_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button update status active
        protected void ButtonChangeStatus_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            User model_user = new User();

            model_user.user_id = Guid.Parse(HideUserID.Value.ToString());
            model_user.user_name = HideUserName.Value.ToString();
            model_user.password = HidePassword.Value.ToString();
            model_user.full_name = HideFUllName.Value.ToString();
            model_user.hope_user_id = Int64.Parse(HideHopeID.Value.ToString());
            model_user.email = HideEmail.Value.ToString();
            
            if (HideBday.Value.ToString() == "")
            {
                model_user.birthday = DateTime.Parse(ConfigurationManager.AppSettings["DefaultBirthday"].ToString());
            }
            else
            {
                model_user.birthday = DateTime.Parse(HideBday.Value.ToString());
            }
            model_user.handphone = HidePhone.Value.ToString();
            model_user.lock_counter = byte.Parse(HideLCounter.Value.ToString());
            if (HideLastLogin.Value.ToString() == "")
            {
                model_user.last_login_date = null;
            }
            else
            {
                model_user.last_login_date = DateTime.Parse(HideLastLogin.Value.ToString());
            }
            if (HideExpPassDate.Value.ToString() == "")
            {
                model_user.exp_pass_date = null;
            }
            else
            {
                model_user.exp_pass_date = DateTime.Parse(HideExpPassDate.Value.ToString());
            }
            model_user.is_internal = bool.Parse(HideInternal.Value.ToString());
            model_user.is_ad = bool.Parse(HideAD.Value.ToString());
            model_user.is_proint = bool.Parse(HideProint.Value.ToString());
            if (bool.Parse(HideUserActive.Value.ToString()) == true)
            {
                model_user.is_active = false;
            }
            else if (bool.Parse(HideUserActive.Value.ToString()) == false)
            {
                model_user.is_active = true;
            }
            model_user.created_by = HideCreatby.Value.ToString();
            model_user.created_date = DateTime.Parse(HideCreatdate.Value.ToString());
            model_user.modified_by = Helper.UserLogin(this.Page);
            model_user.modified_date = DateTime.Now;

            var hasil = clsUser.PutDataUser(model_user.user_id, model_user);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                ShowToastr("User : status successfully updated", "Save Success", "success");
                getDataUser();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonChangeStatus_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk reset password
        protected void ButtonResetPass_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            User model_user = new User();

            model_user.user_id = Guid.Parse(reset_userid.Value.ToString());
            model_user.user_name = reset_username.Value.ToString();
            model_user.password = Helper.Encrypt(ConfigurationManager.AppSettings["DefaultPassword"].ToString());
            model_user.full_name = reset_fullname.Value.ToString();
            model_user.hope_user_id = Int64.Parse(reset_hopeid.Value.ToString());
            model_user.email = reset_email.Value.ToString();
            if (reset_bday.Value.ToString() == "")
            {
                model_user.birthday = DateTime.Parse(ConfigurationManager.AppSettings["DefaultBirthday"].ToString());
            }
            else
            {
                model_user.birthday = DateTime.Parse(reset_bday.Value.ToString());
            }
            model_user.handphone = reset_phone.Value.ToString();
            model_user.lock_counter = byte.Parse(reset_lcounter.Value.ToString());
            if (reset_lastlogin.Value.ToString() == "")
            {
                model_user.last_login_date = null;
            }
            else
            {
                model_user.last_login_date = DateTime.Parse(reset_lastlogin.Value.ToString());
            }
            if (reset_exppassdate.Value.ToString() == "")
            {
                model_user.exp_pass_date = null;
            }
            else
            {
                model_user.exp_pass_date = DateTime.Parse(reset_exppassdate.Value.ToString());
            }
            model_user.is_internal = bool.Parse(reset_internal.Value.ToString());
            model_user.is_ad = bool.Parse(reset_ad.Value.ToString());
            model_user.is_proint = bool.Parse(reset_proint.Value.ToString());
            model_user.is_active = bool.Parse(reset_isactive.Value.ToString());
            model_user.created_by = reset_createdby.Value.ToString();
            model_user.created_date = DateTime.Parse(reset_createddate.Value.ToString());
            model_user.modified_by = Helper.UserLogin(this.Page);
            model_user.modified_date = DateTime.Now;

            var hasil = clsUser.PutDataUser(model_user.user_id, model_user);
            var hasilrelease = clsUser.ReleaseUserLock(model_user.user_name.ToString());

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr("User : Reset Password Failed <br /> Please Try Again...", "Save Failed", "error");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalResetPass').modal('hide'); $('#modalAfterReset').modal('show');", addScriptTags: true);
                getDataUser();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonResetPass_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        protected void ButtonResetPassGlobal_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                string usrmdf = Helper.UserLogin(this.Page).Replace('\\', '+');
                if (usrmdf.Contains("+") == true)
                {
                    usrmdf = usrmdf.Split('+')[1];
                }
                var hasil = clsUser.ResetPasswordUserGlobal(reset_username.Value.ToString(), usrmdf);

                var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
                var Status = Response.Property("status").Value.ToString();
                var Message = Response.Property("message").Value.ToString();

                if (Status == "Fail")
                {
                    ShowToastr("User : Reset Password Failed <br /> Please Try Again...", "Save Failed", "error");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalResetPass').modal('hide'); $('#modalAfterReset').modal('show');", addScriptTags: true);
                    getDataUser();
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonResetPassGlobal_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception ex)
            {
                ShowToastr("User : Reset Password Failed <br />" + ex.Message.ToString(), "Save Failed", "error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonResetPassGlobal_Click", StartTime, "ERROR", MyUser.GetUsername(), "", "", ex.Message));
            }
        }

        //fungsi untuk get data user pencarian
        public void getDataUserSearch(string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                string arrumsorgids = generateListUmsOrgIdByLogin();
                List<User> ListUserData = new List<User>();
                //var Get_User = clsUser.GetDataUserBySearch(keyword);
                var Get_User = clsUser.GetDataUserBySearchByOrg(arrumsorgids, keyword);
                var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

                ListUserData = Get_DataUser.list;

                if (ListUserData.Count() > 0)
                {
                    DataTable dt_user = Helper.ToDataTable(ListUserData);

                    //fungsi sorting datatable
                    dt_user.DefaultView.Sort = "user_name ASC";
                    dt_user = dt_user.DefaultView.ToTable();

                    GridViewUserSimple.DataSource = dt_user;
                    GridViewUserSimple.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_User.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, "Data not found", "warning");
                }

                DataTable dt_login = (DataTable)Session[Helper.Session_DataLogin];
                if (dt_login == null)
                {
                    Response.Redirect("~/Pages/Login_page.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }

                string role_branchadmin = dt_login.Rows[0]["role_id"].ToString();
                if (role_branchadmin == ConfigurationManager.AppSettings["roleadmincabang"].ToString()) //role admin cabang
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideAddUser(); hideEditUser();", true);
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUserSearch", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataUserSearch", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi klik tombol cari
        protected void ButtonCari_Click(object sender, EventArgs e)
        {
            if (Search_masterData.Text != "")
            {
                getDataUserSearch(Search_masterData.Text);
                HiddenFlagCari.Value = 1.ToString();
            }
            else
            {
                getDataUser();
                HiddenFlagCari.Value = 0.ToString();
            }
        }

        protected void Button_Release_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var hasil = clsUser.ReleaseUserLock(HiddenUsernameDetail.Value.ToString());

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr("User : Release Failed <br /> Please Try Again...", "Save Failed", "error");
            }
            else
            {
                Label_Locked.Text = "Unlocked";
                ShowToastr("User : Release Success", "Save Success", "success");
                getDataUser();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Button_Release_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        protected void ButtonPass_Click(object sender, EventArgs e)
        {
            string pass = "";
            pass = Helper.Decrypt(Edit_HiddenPassword.Value.ToString());
            ButtonPass.Text = pass;
        }

        public void getMappingAuthorization(Guid user_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            divSimpleMapping.InnerHtml = "";
            StringBuilder resultHTML = new StringBuilder();
            resultHTML.Append("");

            try
            {
                List<ViewSimpleOrg> ListOrg = new List<ViewSimpleOrg>();
                var Get_Org = clsUserRole.GetDataOrg_byuserid(user_id);
                var Get_DataOrg = JsonConvert.DeserializeObject<Result_Data_SImpleOrg>(Get_Org.Result.ToString());

                ListOrg = Get_DataOrg.list;

                if (ListOrg.Count() > 0)
                {
                    DataTable dt_org = new DataTable();
                    dt_org = Helper.ToDataTable(ListOrg);

                    if (dt_org.Rows.Count > 0)
                    {
                        resultHTML.Append("<table id=\"tbl_orgdetail\" name=\"tbl_orgdetail\" border=\"0\" width=\"100%\">");

                        for (int i = 0; i < dt_org.Rows.Count; i++)
                        {
                            resultHTML.Append(
                                            "<tr>" +
                                            "<td style=\"width:100%; text-align:left; color:#1a2269; font-weight:bold;\"> <i class=\"icon-ic_Organization\"></i> &nbsp; &nbsp;" + dt_org.Rows[i]["organization_name"].ToString() + "</td>" +
                                            "</tr>");

                            List<ViewSimpleMapping> ListMapping = new List<ViewSimpleMapping>();
                            var Get_Mapping = clsUserRole.GetDataMap_byuseridorgid(user_id, Int64.Parse(dt_org.Rows[i]["organization_id"].ToString()));
                            var Get_DataMapping = JsonConvert.DeserializeObject<Result_Data_SImpleMapping>(Get_Mapping.Result.ToString());

                            ListMapping = Get_DataMapping.list;

                            if (ListMapping.Count() > 0)
                            {
                                DataTable dt_map = new DataTable();
                                dt_map = Helper.ToDataTable(ListMapping);

                                if (dt_map.Rows.Count > 0)
                                {
                                    resultHTML.Append("<tr> <td style=\"padding-left:25px;\">");
                                    resultHTML.Append("<table id=\"tbl_mapdetail\" name=\"tbl_mapdetail\" border=\"0\" width=\"100%\">" +
                                        "<tr style=\"border-top:1px solid lightgrey;\">" +
                                            "<td style=\"width:40%; text-align:left; font-weight:bold; color:#eb9a00;\"> Application </td>" +
                                            "<td style=\"width:40%; text-align:left; font-weight:bold; color:#eb9a00;\"> Role </td>" +
                                            "<td style=\"width:20%; text-align:center; font-weight:bold; color:#eb9a00;\"> Status </td>" +
                                            "</tr>"
                                            );

                                    for (int j = 0; j < dt_map.Rows.Count; j++)
                                    {
                                        var stt = "";
                                        if (dt_map.Rows[j]["is_active"].ToString().ToLower() == "true")
                                        {
                                            stt = "<td style =\"width:20%; text-align:center;\"> <span class=\"badge\" style=\"background-color:#5cb85c; width:65px;\"> Active </span> </td>";
                                        }
                                        else
                                        {
                                            stt = "<td style =\"width:20%; text-align:center;\"> <span class=\"badge\" style=\"background-color:#b85c5c; width:65px;\"> Inactive </span> </td>";
                                        }
                                        resultHTML.Append(
                                            "<tr>" +
                                            "<td style=\"width:40%; text-align:left; text-transform: capitalize;\"> " + dt_map.Rows[j]["application_name"].ToString().ToLower() + "</td>" +
                                            "<td style=\"width:40%; text-align:left; text-transform: capitalize;\"> " + dt_map.Rows[j]["role_name"].ToString().ToLower() + "</td>" +
                                            stt +
                                            "</tr>"
                                            );
                                    }

                                    resultHTML.Append(" </table> ");
                                    resultHTML.Append(" </td> </tr>  ");
                                    resultHTML.Append(" <tr> <td> &nbsp; </td> </tr>  ");
                                }
                            }
                        }

                        resultHTML.Append("</table>");

                        divSimpleMapping.InnerHtml = resultHTML.ToString();
                    }
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Org.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();
                }

                DataTable dt_login = (DataTable)Session[Helper.Session_DataLogin];
                string role_branchadmin = dt_login.Rows[0]["role_id"].ToString();
                if (role_branchadmin == ConfigurationManager.AppSettings["roleadmincabang"].ToString()) //role admin cabang
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "hide", "hideAddUser(); hideEditUser();", true);
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getMappingAuthorization", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getMappingAuthorization", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        protected void ButtonViewMapping_Click(object sender, EventArgs e)
        {
            getMappingAuthorization(Guid.Parse(HF_userid_viewmapping.Value.ToString()));
        }

        void GetDataHospitalUnit()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                DataTable dt = new DataTable();
                List<Organization> ListHopsitalUnit = new List<Organization>();
                var GetListUnitAPI = clsOrganization.GetDataOrganization();
                var GetListUnit = JsonConvert.DeserializeObject<Result_Data_Organization>(GetListUnitAPI.Result.ToString());

                ListHopsitalUnit = GetListUnit.list;

                if (ListHopsitalUnit.Count() > 0)
                {
                    dt = Helper.ToDataTable(ListHopsitalUnit);

                    ddlAddUnitSIP.DataSource = dt;
                    ddlAddUnitSIP.DataTextField = "organization_name";
                    ddlAddUnitSIP.DataValueField = "organization_id";
                    ddlAddUnitSIP.DataBind();
                    ddlAddUnitSIP.Items.Insert(0, new ListItem("Select Organization", "-1"));

                    ddlEditUnitSIP.DataSource = dt;
                    ddlEditUnitSIP.DataTextField = "organization_name";
                    ddlEditUnitSIP.DataValueField = "organization_id";
                    ddlEditUnitSIP.DataBind();
                    ddlEditUnitSIP.Items.Insert(0, new ListItem("Select Organization", "-1"));
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataHospitalUnit", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetDataHospitalUnit", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
            }
        }

        protected List<DoctorSIP> GetSIPByUserId (Guid UserId)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<DoctorSIP> ListSIP = new List<DoctorSIP>();
            var GetListSIPAPI = clsUser.GetDataSIPByUserId(UserId);
            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(GetListSIPAPI.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status.ToLower() == "success")
            {
                var GetListSIP = JsonConvert.DeserializeObject<ResultSIP>(GetListSIPAPI.Result.ToString());
                ListSIP = GetListSIP.list;
            }

            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "GetSIPByUserId", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            return ListSIP;
        }

        protected List<DoctorSIP> GetRowList_DoctorSIP(int type)
        {
            List<DoctorSIP> data = new List<DoctorSIP>();
            try
            {
                if (type == 1)
                {
                    foreach (GridViewRow rows in gvw_AddUnitSIP.Rows)
                    {
                        HiddenField doctor_sip_id = (HiddenField)rows.FindControl("doctor_sip_id");
                        HiddenField user_id = (HiddenField)rows.FindControl("user_id");
                        HiddenField organization_id = (HiddenField)rows.FindControl("organization_id");
                        Label organization_name = (Label)rows.FindControl("organization_name");
                        Label sip_no = (Label)rows.FindControl("sip_no");

                        DoctorSIP row = new DoctorSIP();

                        row.doctor_sip_id = Guid.Parse(doctor_sip_id.Value);
                        row.user_id = Guid.Parse(user_id.Value);
                        row.organization_id = long.Parse(organization_id.Value);
                        row.organization_name = organization_name.Text;
                        row.sip_no = sip_no.Text;
                        row.is_delete = 0;
                        data.Add(row);
                    }
                }
                else if (type == 2)
                {
                    foreach (GridViewRow rows in gvw_EditUnitSIP.Rows)
                    {
                        HiddenField doctor_sip_id = (HiddenField)rows.FindControl("doctor_sip_id");
                        HiddenField user_id = (HiddenField)rows.FindControl("user_id");
                        HiddenField organization_id = (HiddenField)rows.FindControl("organization_id");
                        Label organization_name = (Label)rows.FindControl("organization_name");
                        Label sip_no = (Label)rows.FindControl("sip_no");

                        DoctorSIP row = new DoctorSIP();

                        row.doctor_sip_id = Guid.Parse(doctor_sip_id.Value);
                        row.user_id = Guid.Parse(user_id.Value);
                        row.organization_id = long.Parse(organization_id.Value);
                        row.organization_name = organization_name.Text;
                        row.sip_no = sip_no.Text;
                        row.is_delete = 0;
                        data.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }

        protected void btndelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                p_Add.InnerText = "";

                DataTable dt;
                int selRowIndex = ((GridViewRow)(((ImageButton)sender).Parent.Parent)).RowIndex;

                List<DoctorSIP> data = GetRowList_DoctorSIP(1);
                dt = Helper.ToDataTable(data);
                dt.Rows[selRowIndex].SetField("is_delete", 1);
                if (dt.Select("is_delete = 0").Count() > 0)
                {
                    gvw_AddUnitSIP.DataSource = dt.Select("is_delete = 0").CopyToDataTable();
                    gvw_AddUnitSIP.DataBind();
                }
                else
                {
                    gvw_AddUnitSIP.DataSource = null;
                    gvw_AddUnitSIP.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAddUnitSIP_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlAddUnitSIP.SelectedValue == "-1" || txtAddUnitSIP.Text == "")
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Please Select and Fill Data First!";
                }
                else
                {
                    p_Add.InnerText = "";

                    DataTable dt = new DataTable();
                    List<DoctorSIP> data = GetRowList_DoctorSIP(1);

                    if (data == null)
                    {
                        dt.Columns.Add("doctor_sip_id");
                        dt.Columns.Add("user_id");
                        dt.Columns.Add("organization_id");
                        dt.Columns.Add("organization_name");
                        dt.Columns.Add("sip_no");
                        dt.Columns.Add("is_delete");
                        dt.Rows.Add(new Object[] { Guid.Empty, Guid.Empty, ddlAddUnitSIP.SelectedValue, ddlAddUnitSIP.SelectedItem.Text, txtAddUnitSIP.Text, 0 });
                        gvw_AddUnitSIP.DataSource = dt;
                        gvw_AddUnitSIP.DataBind();
                    }
                    else
                    {
                        int flag = 0;
                        foreach (DoctorSIP x in data)
                        {
                            if (x.organization_id.ToString() == ddlAddUnitSIP.SelectedValue)
                            {
                                flag = 1;
                            }
                        }

                        if (flag == 0)
                        {
                            DataTable dtSIP = Helper.ToDataTable(data);
                            dtSIP.Rows.Add(new Object[] { Guid.Empty, Guid.Empty, ddlAddUnitSIP.SelectedValue, ddlAddUnitSIP.SelectedItem.Text, txtAddUnitSIP.Text, 0 });
                            gvw_AddUnitSIP.DataSource = dtSIP;
                            gvw_AddUnitSIP.DataBind();
                        }
                        else
                        {
                            p_Add.Attributes.Remove("style");
                            p_Add.Attributes.Add("style", "display:block; color:red;");
                            p_Add.InnerText = "SIP Data with Same Org ID Already Exist!";
                        }
                    }

                    ddlAddUnitSIP.SelectedIndex = 0;
                    txtAddUnitSIP.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btndeleteedit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                p_Edit.InnerText = "";

                DataTable dt;
                int selRowIndex = ((GridViewRow)(((ImageButton)sender).Parent.Parent)).RowIndex;

                List<DoctorSIP> data = GetRowList_DoctorSIP(2);
                dt = Helper.ToDataTable(data);
                dt.Rows[selRowIndex].SetField("is_delete", 1);
                if (dt.Select("is_delete = 0").Count() > 0)
                {
                    gvw_EditUnitSIP.DataSource = dt.Select("is_delete = 0").CopyToDataTable();
                    gvw_EditUnitSIP.DataBind();
                }
                else
                {
                    gvw_EditUnitSIP.DataSource = null;
                    gvw_EditUnitSIP.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAddEditUnitSIP_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlEditUnitSIP.SelectedValue == "-1" || txtEditUnitSIP.Text == "")
                {
                    p_Edit.Attributes.Remove("style");
                    p_Edit.Attributes.Add("style", "display:block; color:red;");
                    p_Edit.InnerText = "Please Select and Fill Data First!";
                }
                else
                {
                    p_Edit.InnerText = "";

                    DataTable dt = new DataTable();
                    List<DoctorSIP> data = GetRowList_DoctorSIP(2);

                    if (data == null)
                    {
                        dt.Columns.Add("doctor_sip_id");
                        dt.Columns.Add("user_id");
                        dt.Columns.Add("organization_id");
                        dt.Columns.Add("organization_name");
                        dt.Columns.Add("sip_no");
                        dt.Columns.Add("is_delete");
                        dt.Rows.Add(new Object[] { Guid.Empty, Guid.Empty, ddlEditUnitSIP.SelectedValue, ddlEditUnitSIP.SelectedItem.Text, txtEditUnitSIP.Text, 0 });
                        gvw_EditUnitSIP.DataSource = dt;
                        gvw_EditUnitSIP.DataBind();
                    }
                    else
                    {
                        int flag = 0;
                        foreach (DoctorSIP x in data)
                        {
                            if (x.organization_id.ToString() == ddlEditUnitSIP.SelectedValue)
                            {
                                flag = 1;
                            }
                        }

                        if (flag == 0)
                        {
                            DataTable dtSIP = Helper.ToDataTable(data);
                            dtSIP.Rows.Add(new Object[] { Guid.Empty, Guid.Empty, ddlEditUnitSIP.SelectedValue, ddlEditUnitSIP.SelectedItem.Text, txtEditUnitSIP.Text, 0 });
                            gvw_EditUnitSIP.DataSource = dtSIP;
                            gvw_EditUnitSIP.DataBind();
                        }
                        else
                        {
                            p_Edit.Attributes.Remove("style");
                            p_Edit.Attributes.Add("style", "display:block; color:red;");
                            p_Edit.InnerText = "SIP Data with Same Org ID Already Exist!";
                        }
                    }

                    ddlEditUnitSIP.SelectedIndex = 0;
                    txtEditUnitSIP.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string generateListUmsOrgIdByLogin()
        {
            DataTable dtlogin = (DataTable)Session[Helper.Session_DataLogin];
            string arrorg = "";
            for (int i = 0; i < dtlogin.Rows.Count; i++)
            {
                arrorg = arrorg + dtlogin.Rows[i]["organization_id"].ToString() + ";";
            }
            arrorg = arrorg.Remove(arrorg.Length - 1,1);

            return arrorg;
        }
    }
}