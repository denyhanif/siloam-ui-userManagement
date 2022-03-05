using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;

using Siloam.Ui.UserManagement.API_Code.Models;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Siloam.Ui.UserManagement.Pages.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Siloam.Ui.UserManagement.Pages
{
    public partial class PageMaster : System.Web.UI.Page
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
                
                HiddenRowSelect.Value = 0.ToString();            
                HiddenAppIdSelect.Value = Guid.Empty.ToString();
                HiddenFlagCari.Value = 0.ToString();

                //inisialisasi temp table
                dt = new DataTable();
                MakeDataTable_PageTemp();
                BindGrid_PageTemp();
            }
            else
            {
                dt = (DataTable)ViewState["DataTablePage"];
            }
            ViewState["DataTablePage"] = dt;
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
            myObject = (HtmlContainerControl)Master.FindControl("divBoxPage");

            myObject.Style.Add("Background-color", "#1a2269");
            myObject.Style.Add("border-right", "5px solid #ffd800");
            myObject.Style.Add("color", "#f2c22c");
        }

        //inisialisasi datatable
        private void MakeDataTable_PageTemp()
        {
            dt.Columns.Add("page_name");
        }

        //adding data to datatable from input user
        private void AddToDataTable_PageTemp()
        {
            DataRow dr = dt.NewRow();
            dr["page_name"] = Add_TextPagename.Text.ToString();
            dt.Rows.Add(dr);
        }
        
        //bind data to gridview
        private void BindGrid_PageTemp()
        {
            Add_GridViewPagenameList.DataSource = dt;
            Add_GridViewPagenameList.DataBind();
        }

        //fungsi untuk add 1 data to temp gridview
        protected void Add_ImgBtnAddPageList_Click(object sender, ImageClickEventArgs e)
        {
            int cekdatasama = 0;
            p_Add.InnerText = "";

            foreach (DataRow row in dt.Rows)
            {
                if (row["page_name"].ToString() == Add_TextPagename.Text.ToString())
                {
                    cekdatasama = 1;
                }
            }

            if (cekdatasama == 0)
            {
                if (getDataPageExist(Guid.Parse(HiddenAppIdSelect.Value.ToString()), Add_TextPagename.Text.ToString()) == true)
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Page Name Already Exist!";
                }
                else
                {
                    AddToDataTable_PageTemp();
                    BindGrid_PageTemp();
                }
            }
            else
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:red;");
                p_Add.InnerText = "Input Page Name cannot be the same!";
            }    
            Add_TextPagename.Text = "";
            this.Page.SetFocus(Add_TextPagename.ClientID);
        }

        //fungsi untuk delete 1 data from temp gridview
        protected void ImgBtn_DeleteRow_Click(object sender, ImageClickEventArgs e)
        {
            int RowSelect = ((GridViewRow)(((ImageButton)sender).Parent.Parent)).RowIndex;
            dt.Rows[RowSelect].Delete();
            BindGrid_PageTemp();

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
            ViewState["DataTablePage"] = dt;
            BindGrid_PageTemp();

            p_Add.InnerText = "";
            p_Edit.InnerText = "";
        }

        //fungsi untuk get dapat application
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

                    GridViewApplication.DataSource = dt_result;
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
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
            }
        }

        //fungsi untuk get data page by application id
        void getDataPage(Guid app_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Pagee> ListPageData = new List<Pagee>();
                var Get_Page = clsPage.GetDataPage_byAppID(app_id);
                var Get_DataPage = JsonConvert.DeserializeObject<Result_Data_Page>(Get_Page.Result.ToString());

                ListPageData = Get_DataPage.list;

                if (ListPageData.Count() > 0)
                {
                    DataTable dt_page = Helper.ToDataTable(ListPageData);

                    //fungsi sorting datatable
                    dt_page.DefaultView.Sort = "page_name ASC";
                    dt_page = dt_page.DefaultView.ToTable();

                    GridViewPage.DataSource = dt_page;
                    GridViewPage.DataBind();
                }
                else
                {
                    DataTable dt_page = null;

                    GridViewPage.DataSource = dt_page;
                    GridViewPage.DataBind();

                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Page.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataPage", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataPage", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi link pagination pada gridview
        protected void GridViewPage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Guid App_ID;
            App_ID = Guid.Parse(HiddenAppIdSelect.Value.ToString());

            GridViewPage.PageIndex = e.NewPageIndex;
            if (HiddenFlagCari.Value == "0")
            {
                getDataPage(App_ID);
            }
            else if (HiddenFlagCari.Value == "1")
            {
                getDataPageSearch(App_ID, Search_masterData.Text);
            }           
        }

        //fungsi untuk mengecek data yang diiputkan apakah sudah ada di DB atau belum
        public bool getDataPageExist(Guid appid, string pagename)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool eksis = false;
            try
            {
                if (pagename == "")
                {
                    eksis = true;
                }
                else
                {
                    List<Pagee> ListPageData = new List<Pagee>();
                    var Get_Pages = clsPage.GetDataPage_byPageName(appid, pagename);
                    var Get_DataPages = JsonConvert.DeserializeObject<Result_Data_Page>(Get_Pages.Result.ToString());

                    ListPageData = Get_DataPages.list;

                    if (ListPageData.Count() > 0)
                    {
                        eksis = true;
                    }
                    else
                    {
                        eksis = false;
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataPageExist", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataPageExist", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
            return eksis;
        }

        //fungsi untuk menjalankan aksi saat side application dipilih dan diklik
        protected void LinkButtonApps_Click(object sender, EventArgs e)
        {
            Guid App_ID;

            int RowSelectOld = int.Parse(HiddenRowSelect.Value.ToString());
            GridViewRow rowOld = GridViewApplication.Rows[RowSelectOld];
            rowOld.BackColor = Color.Transparent;

            int RowSelect = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;
            GridViewRow rowNew = GridViewApplication.Rows[RowSelect];
            rowNew.BackColor = Color.White;

            HiddenRowSelect.Value = RowSelect.ToString();

            LinkButton appName = (LinkButton)GridViewApplication.Rows[RowSelect].FindControl("LinkListApps");
            Label LblappName = (Label)GridViewApplication.Rows[RowSelect].FindControl("LabelListApps");
            HiddenField appID = (HiddenField)GridViewApplication.Rows[RowSelect].FindControl("HiddenFieldAppID");
            LabelAppTitle.Text = LblappName.Text.ToString();
            App_ID = Guid.Parse(appID.Value.ToString());

            HiddenAppIdSelect.Value = App_ID.ToString();

            getDataPage(App_ID);  
            divWaiting.Visible = false;
            divkonten.Visible = true;
            Search_TextApp.Text = "";
        }

        //fungsi button save edit data
        protected void Edit_ButtonSavePage_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Pagee model_Page = new Pagee();

            model_Page.page_id = Guid.Parse(Edit_HiddenPageID.Value.ToString());
            model_Page.application_id = Guid.Parse(Edit_HiddenAppID.Value.ToString());
            model_Page.page_name = Edit_TextPagename.Text.ToString();
            model_Page.is_active = bool.Parse(Edit_HiddenIsActive.Value.ToString());
            model_Page.created_by = Edit_HiddenCreateby.Value.ToString();
            model_Page.created_date = DateTime.Parse(Edit_HiddenCreatedate.Value.ToString());
            model_Page.modified_by = Helper.UserLogin(this.Page);
            model_Page.modified_date = DateTime.Now;

            if (getDataPageExist(model_Page.application_id, Edit_TextPagename.Text) == false)
            {               
                var hasil = clsPage.PutDataPage(model_Page.page_id, model_Page);

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
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalEditPage').modal('hide');", addScriptTags: true);
                    ShowToastr("Page : data successfully changed", "Save Success", "success");
                    getDataPage(model_Page.application_id);

                    clearFormAdd();
                }
            }
            else
            {
                if (Edit_TextPagename.Text != Edit_HiddenTextPagename.Value.ToString())
                {
                    p_Edit.Attributes.Remove("style");
                    p_Edit.Attributes.Add("style", "display:block; color:red;");
                    p_Edit.InnerText = "Page Name Already Exist!";

                    Edit_TextPagename.Text = Edit_HiddenTextPagename.Value.ToString();
                }
                else
                {                   
                    var hasil = clsPage.PutDataPage(model_Page.page_id, model_Page);

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
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalEditPage').modal('hide');", addScriptTags: true);
                        ShowToastr("Page : data successfully changed", "Save Success", "success");
                        getDataPage(model_Page.application_id);

                        clearFormAdd();
                    }
                }
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Edit_ButtonSavePage_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button save add data
        protected void Add_ButtonSavePage_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Pagee model_Page = new Pagee();
            int isFail = 0;
            int isSuccess = 0;

            model_Page.application_id = Guid.Parse(Add_HiddenDDLAppId.Value.ToString()); 
            model_Page.is_active = true;
            model_Page.created_by = Helper.UserLogin(this.Page);
            model_Page.created_date = DateTime.Now;
            model_Page.modified_by = null;
            model_Page.modified_date = DateTime.Now;

            var Status = "";
            var Message = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                model_Page.page_id = Guid.NewGuid();
                model_Page.page_name = dt.Rows[i]["page_name"].ToString();

                var hasil = clsPage.PostDataPage(model_Page);

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
                    ShowToastr("Page : data successfully added, but some data may be not added. Please check again", "Save Success", "success");
                }
                else
                {
                    ShowToastr("Page : data successfully added", "Save Success", "success");
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalAddPage').modal('hide');", addScriptTags: true);

                if (LabelAppTitle.Text != "Application")
                {
                    getDataPage(model_Page.application_id);
                }
                clearFormAdd();
            }
            else if(isFail == 1)
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Add_ButtonSavePage_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button update status active
        protected void ButtonChangeStatus_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Pagee model_Page = new Pagee();

            model_Page.page_id = Guid.Parse(HidePageID.Value.ToString());
            model_Page.application_id = Guid.Parse(HideAppID.Value.ToString());
            model_Page.page_name = HidePageName.Value.ToString();
            if (bool.Parse(HidePageActive.Value.ToString()) == true)
            {
                model_Page.is_active = false;
            }
            else if (bool.Parse(HidePageActive.Value.ToString()) == false)
            {
                model_Page.is_active = true;
            }
            model_Page.created_by = HideCreatby.Value.ToString();
            model_Page.created_date = DateTime.Parse(HideCreatdate.Value.ToString());
            model_Page.modified_by = Helper.UserLogin(this.Page);
            model_Page.modified_date = DateTime.Now;
            
            var hasil = clsPage.PutDataPage(model_Page.page_id, model_Page);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                ShowToastr("Page : status successfully updated", "Save Success", "success");
                getDataPage(model_Page.application_id);
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonChangeStatus_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk get data page by application id pencarian
        void getDataPageSearch(Guid app_id, string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Pagee> ListPageData = new List<Pagee>();
                var Get_Page = clsPage.GetDataPage_bySearch(app_id, keyword);
                var Get_DataPage = JsonConvert.DeserializeObject<Result_Data_Page>(Get_Page.Result.ToString());

                ListPageData = Get_DataPage.list;

                if (ListPageData.Count() > 0)
                {
                    DataTable dt_page = Helper.ToDataTable(ListPageData);

                    //fungsi sorting datatable
                    dt_page.DefaultView.Sort = "page_name ASC";
                    dt_page = dt_page.DefaultView.ToTable();

                    GridViewPage.DataSource = dt_page;
                    GridViewPage.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Page.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, "Data not found", "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataPageSearch", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataPageSearch", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi klik button pencarian
        protected void ButtonCari_Click(object sender, EventArgs e)
        {
            Guid App_ID;
            App_ID = Guid.Parse(HiddenAppIdSelect.Value.ToString());

            if (Search_masterData.Text != "")
            {
                getDataPageSearch(App_ID, Search_masterData.Text);
                HiddenFlagCari.Value = 1.ToString();
            }
            else
            {
                getDataPage(App_ID);
                HiddenFlagCari.Value = 0.ToString();
            }
        }
    }
}