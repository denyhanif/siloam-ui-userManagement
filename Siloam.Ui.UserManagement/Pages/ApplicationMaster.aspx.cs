using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

using Siloam.Ui.UserManagement.API_Code.Models;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Siloam.Ui.UserManagement.Pages.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Siloam.Ui.UserManagement.Pages
{
    public partial class ApplicationMaster : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (!IsPostBack)
            {
                loadLinkActive();
                getDataApplication();
                HiddenFlagCari.Value = 0.ToString();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Page_Load", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk menampilkan toast via akses javascript
        void ShowToastr(string message, string title, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "toastr_message",
                String.Format("toastr.{0}('{1}', '{2}');", type.ToLower(), message, title), addScriptTags: true);

            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "toastr_message",
            //     String.Format("toastr.{0}('{1}', '{2}'); toastr.options.positionClass = 'toast-bottom-right' ", type.ToLower(), message, title), addScriptTags: true);
        }

        //fungsi untuk memberikan style pada link sidebar yang aktif
        void loadLinkActive()
        {
            //untuk mengakses element yg ada di master site
            HtmlContainerControl myObject;
            myObject = (HtmlContainerControl)Master.FindControl("divBoxApp");

            myObject.Style.Add("Background-color", "#1a2269");
            myObject.Style.Add("border-right", "5px solid #ffd800");
            myObject.Style.Add("color", "#f2c22c");
        }

        //fungsi untuk get data application
        void getDataApplication()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Application> ListAppData = new List<Application>();
                var Get_App = clsApplication.GetDataApplication();
                var Get_DataApp = JsonConvert.DeserializeObject<Result_Data_Application>(Get_App.Result.ToString());

                ListAppData = Get_DataApp.list;

                if (ListAppData.Count() > 0)
                {
                    DataTable dt_app = Helper.ToDataTable(ListAppData);
                    Session["AppData"] = ListAppData;

                    //fungsi sorting datatable
                    dt_app.DefaultView.Sort = "application_name ASC";
                    dt_app = dt_app.DefaultView.ToTable();

                    GridViewApplication.DataSource = dt_app;
                    GridViewApplication.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_App.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataApplication", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataApplication", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                ShowToastr("Please Check Your Connection!", "Error Load Data", "error");
            }
        }

        //fungsi link pagination pada gridview
        protected void GridViewApplication_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewApplication.PageIndex = e.NewPageIndex;
            if (HiddenFlagCari.Value == "0")
            {
                getDataApplication();
            }
            else if (HiddenFlagCari.Value == "1")
            {
                getDataApplicationSearch(Search_masterData.Text);
            }
            
        }

        //fungsi untuk mengecek data yang diiputkan apakah sudah ada di DB atau belum
        public bool getDataAppExist(string appname)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool eksis = false;
            try
            {
                if (appname == "")
                {
                    eksis = true;
                }
                else
                { 
                    List<Application> ListAppData = new List<Application>();
                    var Get_Apps = clsApplication.GetDataApplicationbyAppName(appname);
                    var Get_DataApp = JsonConvert.DeserializeObject<Result_Data_Application>(Get_Apps.Result.ToString());

                    ListAppData = Get_DataApp.list;

                    if (ListAppData.Count() > 0)
                    {
                        eksis = true;
                    }
                    else
                    {
                        eksis = false;
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataAppExist", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataAppExist", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                p_Add.InnerText = "";
            }
            return eksis;
        }

        //fungsi untuk me reset form input dan notifikasi menjadi kosong kembali
        void clearFormAPP()
        {
            Add_TextAppname.Text = "";
            Add_TextUrl.Text = "";

            p_Add.InnerText = "";

            Edit_TextAppname.Text = "";
            Edit_TextUrl.Text = "";

            p_Edit.InnerText = "";
        }

        //fungsi button save edit data
        protected void Edit_ButtonSaveApp_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            Application model_App = new Application();

            model_App.application_id = Guid.Parse(Edit_HiddenAppID.Value.ToString());
            model_App.application_name = Edit_TextAppname.Text.ToString();
            model_App.url = Edit_TextUrl.Text.ToString();
            model_App.is_active = bool.Parse(Edit_HiddenIsActive.Value.ToString());
            model_App.created_by = Edit_HiddenCreateby.Value.ToString();
            model_App.created_date = DateTime.Parse(Edit_HiddenCreatedate.Value.ToString());
            model_App.modified_by = Helper.UserLogin(this.Page);
            model_App.modified_date = DateTime.Now;

            if (getDataAppExist(Edit_TextAppname.Text.ToString()) == false)
            {   
                var hasil = clsApplication.PutDataApplication(model_App.application_id, model_App);

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
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalEditApp').modal('hide');", addScriptTags: true);
                    ShowToastr("Application : data successfully changed", "Save Success", "success");
                    getDataApplication();

                    clearFormAPP();
                }
            }
            else
            {
                if (Edit_TextAppname.Text != Edit_HiddenTextAppname.Value.ToString())
                {
                    p_Edit.Attributes.Remove("style");
                    p_Edit.Attributes.Add("style", "display:block; color:red;");
                    p_Edit.InnerText = "Application Name Already Exist!";

                    Edit_TextAppname.Text = Edit_HiddenTextAppname.Value.ToString();
                }
                else
                {  
                    var hasil = clsApplication.PutDataApplication(model_App.application_id, model_App);

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
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalEditApp').modal('hide');", addScriptTags: true);
                        ShowToastr("Application : data successfully changed", "Save Success", "success");
                        getDataApplication();

                        clearFormAPP();
                    }
                }
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Edit_ButtonSaveApp_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button save add data
        protected void Add_ButtonSaveApp_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            p_Add.InnerText = "";
            Application model_App = new Application();

            model_App.application_id = Guid.NewGuid();
            model_App.application_name = Add_TextAppname.Text.ToString();
            model_App.url = Add_TextUrl.Text.ToString();
            model_App.is_active = true;
            model_App.created_by = Helper.UserLogin(this.Page);
            model_App.created_date = DateTime.Now;
            model_App.modified_by = null;
            model_App.modified_date = DateTime.Now;

            if (getDataAppExist(Add_TextAppname.Text.ToString()) == false)
            {          
                var hasil = clsApplication.PostDataApplication(model_App);

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
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalAddApp').modal('hide');", addScriptTags: true);
                    ShowToastr("Application : data successfully added", "Save Success", "success");
                    getDataApplication();

                    clearFormAPP();
                }
            }
            else
            {
                if (Add_TextAppname.Text.ToString() != "")
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Application Name Already Exist!";

                    Add_TextAppname.Text = "";
                }
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Add_ButtonSaveApp_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button update status active
        protected void ButtonChangeStatus_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            Application model_App = new Application();

            model_App.application_id = Guid.Parse(HideAppID.Value.ToString());
            model_App.application_name = HideAppName.Value.ToString();
            model_App.url = HideUrl.Value.ToString();
            if (bool.Parse(HideAppActive.Value.ToString()) == true)
            {
                model_App.is_active = false;
            }
            else if (bool.Parse(HideAppActive.Value.ToString()) == false)
            {
                model_App.is_active = true;
            }

            model_App.created_by = HideCreatby.Value.ToString();
            model_App.created_date = DateTime.Parse(HideCreatdate.Value.ToString());
            model_App.modified_by = Helper.UserLogin(this.Page);
            model_App.modified_date = DateTime.Now;

            var hasil = clsApplication.PutDataApplication(model_App.application_id, model_App);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {             
                ShowToastr("Application : status successfully updated", "Save Success", "success");
                getDataApplication();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonChangeStatus_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk get data application pencarian
        void getDataApplicationSearch(string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Application> ListAppData = new List<Application>();
                var Get_App = clsApplication.GetDataApplicationbySearch(keyword);
                var Get_DataApp = JsonConvert.DeserializeObject<Result_Data_Application>(Get_App.Result.ToString());

                ListAppData = Get_DataApp.list;

                if (ListAppData.Count() > 0)
                {
                    DataTable dt_app = Helper.ToDataTable(ListAppData);

                    //fungsi sorting datatable
                    dt_app.DefaultView.Sort = "application_name ASC";
                    dt_app = dt_app.DefaultView.ToTable();

                    GridViewApplication.DataSource = dt_app;
                    GridViewApplication.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_App.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, "Data not found", "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataApplicationSearch", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataApplicationSearch", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                ShowToastr("Please Check Your Connection!", "Error Load Data", "error");
            }
        }

        //fungsi klik button pencarian
        protected void ButtonCari_Click(object sender, EventArgs e)
        {
            if (Search_masterData.Text != "")
            {
                getDataApplicationSearch(Search_masterData.Text);
                HiddenFlagCari.Value = 1.ToString();
            }
            else
            {
                getDataApplication();
                HiddenFlagCari.Value = 0.ToString();
            }
        }
    }
}