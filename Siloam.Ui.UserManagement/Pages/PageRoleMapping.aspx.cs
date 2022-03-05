using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;

using Siloam.Ui.UserManagement.API_Code.Models;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Siloam.Ui.UserManagement.Pages.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Siloam.Ui.UserManagement.Pages
{
    public partial class PageRoleMapping : System.Web.UI.Page
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
                HiddenAppNameSelect.Value = ""; 

                //inisialisasi temp table
                dt = new DataTable();
                MakeDataTable_PageTemp();
                BindGrid_PageTemp();
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
            myObject = (HtmlContainerControl)Master.FindControl("divBoxRoleMap");

            myObject.Style.Add("Background-color", "#1a2269");
            myObject.Style.Add("border-right", "5px solid #ffd800");
            myObject.Style.Add("color", "#f2c22c");
        }

        //inisialisasi datatable
        private void MakeDataTable_PageTemp()
        {
            dt.Columns.Add("page_id");
            dt.Columns.Add("page_name");
        }

        //adding data to datatable from input user
        private void AddToDataTable_PageTemp(string id, string data)
        {
            DataRow dr = dt.NewRow();
            dr["page_id"] = id;
            dr["page_name"] = data;
            dt.Rows.Add(dr);
        }

        //bind data to gridview
        private void BindGrid_PageTemp()
        {
            Add_GridViewPageMapList.DataSource = dt;
            Add_GridViewPageMapList.DataBind();
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
        void getDataPageDDL(Guid app_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Pagee> ListPageDDL = new List<Pagee>();
                var Get_PageDDL = clsPage.GetDataPage_byAppID(app_id);
                var Get_DataPageDDL = JsonConvert.DeserializeObject<Result_Data_Page>(Get_PageDDL.Result.ToString());

                //ListPageDDL = Get_DataPageDDL.list;
                //untuk inisialisasi data pertama dari DDL lalu append data range
                ListPageDDL.Add(new Pagee { page_id = Guid.Empty, page_name = "~select page~", is_active = true });
                ListPageDDL.AddRange(Get_DataPageDDL.list);

                if (ListPageDDL.Count() > 0)
                {
                    DataTable dt_result = new DataTable();
                    DataTable dt_pageDDL = Helper.ToDataTable(ListPageDDL);

                    DataRow[] foundRows = dt_pageDDL.Select("is_active =" + true);
                    if (foundRows.Length > 0)
                    {
                        dt_result = foundRows.CopyToDataTable();
                    }

                    //fungsi sorting datatable
                    dt_result.DefaultView.Sort = "page_name ASC";
                    dt_result = dt_result.DefaultView.ToTable();

                    Add_DDLPagename.DataTextField = "page_name";
                    Add_DDLPagename.DataValueField = "page_id";

                    Add_DDLPagename.DataSource = dt_result;
                    Add_DDLPagename.DataBind();

                    Add_DDLPagename.SelectedValue = Guid.Empty.ToString();
                }
                else
                {
                    Add_DDLPagename.Items.Clear();

                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_PageDDL.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataPageDDL", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataPageDDL", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi untuk mengecek data yang diiputkan apakah sudah ada di DB atau belum
        bool getDataRoleAccessExist(Guid role_id, Guid page_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool existt = false;
            try
            {
                List<RoleAccess> ListRAExist = new List<RoleAccess>();
                var Get_RAExist = clsRoleAccess.GetDataRoleAcces_Exist(role_id, page_id);
                var Get_DataRAExist = JsonConvert.DeserializeObject<Result_Data_RoleAccess>(Get_RAExist.Result.ToString());

                ListRAExist = Get_DataRAExist.list;

                if (ListRAExist.Count() > 0)
                {
                    existt = true;
                }
                else
                {
                    existt = false;
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonChangeStatus_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonChangeStatus_Click", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
            return existt;
        }

        //fungsi untuk menandai validasi, bahwa data yg akan diinput sudah ada di DB atau belum
        protected void Add_DDLPagename_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid roleID = Guid.Parse(HiddenRoleID.Value.ToString());
            Guid pageID = Guid.Parse(Add_DDLPagename.SelectedValue.ToString());

            int cekdatasama = 0;
            p_Add.InnerText = "";

            foreach (DataRow row in dt.Rows)
            {
                if (row["page_name"].ToString() == Add_DDLPagename.SelectedItem.ToString())
                {
                    cekdatasama = 1;
                }
            }

            if (cekdatasama == 0)
            {
                if (getDataRoleAccessExist(roleID, pageID) == true)
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Page Name already Mapping in this role!";
                }
                else
                {
                    AddToDataTable_PageTemp(Add_DDLPagename.SelectedValue.ToString(),Add_DDLPagename.SelectedItem.ToString());
                    BindGrid_PageTemp();
                }
            }
            else
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:red;");
                p_Add.InnerText = "Input Page Name cannot be the same!";
            }
            Add_DDLPagename.SelectedValue = Guid.Empty.ToString();
            this.Page.SetFocus(Add_DDLPagename.ClientID);
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
            ViewState["DataTable"] = dt;
            BindGrid_PageTemp();

            p_Add.InnerText = "";
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
            HiddenAppNameSelect.Value = LblappName.Text.ToString();

            getMappingPageRoleExpand(App_ID);
            getDataPageDDL(App_ID);
            divWaiting.Visible = false;
            divkonten.Visible = true;
            Search_TextApp.Text = "";
        }

        //fungsi untuk build table dengan data mapping yang sudah terrancang
        public void getMappingPageRoleExpand(Guid application_id)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Guid appid_send = application_id;
            string appname_send = HiddenAppNameSelect.Value.ToString();

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
                                            "<td class=\"headerlabel\" style=\"height:40px; width:85%; cursor:pointer;\"> <b> <i class=\"fa fa-plus-square iconRole\"></i> &nbsp;" + dt_role.Rows[i]["role_name"].ToString() + " </b> </td>" +
                                            "<td> <button type=\"button\" class=\"TombolAddData btn btn-primary TeksNormal text-left\" style=\"color:white;\" onclick=\"AddMappingPage('" + appid_send.ToString() + "','" + appname_send.ToString().Replace(" ", "_") + "','" + dt_role.Rows[i]["role_id"].ToString() + "','" + dt_role.Rows[i]["role_name"].ToString().Replace(" ", "_") + "')\" ><i class=\"fa fa-plus\"></i>&nbsp; Map Role</button> </td>" +
                                            "</tr>");

                            List<ViewListPage> ListPageData = new List<ViewListPage>();
                            var Get_Page = clsViewListPage.GetDataPage(Guid.Parse(dt_role.Rows[i]["role_id"].ToString()), application_id);
                            var Get_DataPage = JsonConvert.DeserializeObject<Result_Data_Page_List>(Get_Page.Result.ToString());

                            ListPageData = Get_DataPage.list;
                            string iscek = "";

                            if (ListPageData.Count() > 0)
                            {
                                DataTable dt_page = Helper.ToDataTable(ListPageData);
                                //fungsi sorting datatable
                                dt_page.DefaultView.Sort = "page_name ASC";
                                dt_page = dt_page.DefaultView.ToTable();

                                if (dt_page.Rows.Count > 0)
                                {
                                    resultHTML.Append("<tr class=\"header\"> <td colspan=\"2\">");
                                    resultHTML.Append("<table id=\"tbl_pagedetail\" name=\"tbl_pagedetail\" border=\"0\" width=\"100%\" class=\"table table-striped\">");  //second table - data page

                                    for (int j = 0; j < dt_page.Rows.Count; j++)
                                    {
                                        if (dt_page.Rows[j]["is_active"].ToString() == "True")
                                        {
                                            iscek = "checked";
                                        }
                                        else
                                        {
                                            iscek = "unchecked";
                                        }

                                        resultHTML.Append(
                                               "<tr>" +
                                               "<td style=\"width:90%; border-width:0\"> &nbsp; &nbsp;" + dt_page.Rows[j]["page_name"].ToString() + " </td>" +
                                               "<td style=\"border-width:0\"> <input ID=\"CheckBoxStatus\" class=\"CheckBoxSwitch\" type=\"checkbox\" " + iscek + " data-toggle=\"toggle\" data-on=\"Active\" data-off=\"Inactive\" data-onstyle=\"success\" data-offstyle=\"default\" data-size=\"mini\"  onchange=\"UpdateStatus('" + dt_page.Rows[j]["role_access_id"].ToString() + "','" + dt_role.Rows[i]["role_id"].ToString() + "','" + dt_page.Rows[j]["page_id"].ToString() + "','" + dt_page.Rows[j]["created_by"].ToString().Replace(" ", "_").Replace("\\", "+") + "','" + dt_page.Rows[j]["created_date"].ToString().Replace(" ", "_") + "','" + dt_page.Rows[j]["is_active"].ToString() + "')\"> </td>" +
                                               "</tr>");
                                    }

                                    resultHTML.Append(" </table> ");  //end of second table - data page
                                    resultHTML.Append(" </td> </tr>  ");
                                }
                            }
                            resultHTML.Append("<tr class=\"header\"> <td colspan=\"2\"> <br /> </td> </tr>");
                        }
                        resultHTML.Append("<tr class=\"headerRole\"> <td colspan=\"2\"> <br /> </td> </tr>");
                        resultHTML.Append("</table>");
                        //end of first table - data role

                        divContentMapping.InnerHtml = resultHTML.ToString();
                    }
                    else
                    {
                        resultHTML.Append("<label class=\"boxShadow\" style=\"margin-top:7px; height:26px; padding-left:0px;\">No Data</label><br /><br />");
                        divContentMapping.InnerHtml = resultHTML.ToString();
                    }
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Role.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    resultHTML.Append("<label class=\"boxShadow\" style=\"margin-top:7px; height:26px; padding-left:0px;\">No Data</label><br /><br />");
                    divContentMapping.InnerHtml = resultHTML.ToString();
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getMappingPageRoleExpand", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getMappingPageRoleExpand", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi button save add data
        protected void Add_ButtonSaveMapPage_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Guid App_ID;
            RoleAccess model_RA = new RoleAccess();
            int isFail = 0;
            int isSuccess = 0;

            model_RA.role_id = Guid.Parse(HiddenRoleID.Value.ToString());
            model_RA.is_active = true;
            model_RA.created_by = Helper.UserLogin(this.Page);
            model_RA.created_date = DateTime.Now;
            model_RA.modified_by = null;
            model_RA.modified_date = DateTime.Now;

            var Status = "";
            var Message = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                model_RA.page_id = Guid.Parse(dt.Rows[i]["page_id"].ToString());
                
                var hasil = clsRoleAccess.PostDataRoleAccess(model_RA);

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
                    ShowToastr("Mapping Role Access : data successfully mapped, but some data may be not mapped. Please check again", "Save Success", "success");
                }
                else
                {
                    ShowToastr("Mapping Role Access : data successfully mapped", "Save Success", "success");
                }
                
                clearFormAdd();

                App_ID = Guid.Parse(HiddenAppIdSelect.Value.ToString());
                getMappingPageRoleExpand(App_ID);

                Add_DDLPagename.SelectedValue = Guid.Empty.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refresh", "$('.selectpicker').selectpicker('refresh');", addScriptTags: true);
            }
            else if (isFail == 1)
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Add_ButtonSaveMapPage_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button update status active
        protected void ButtonChangeStatus_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Guid App_ID;
            RoleAccess model_RA = new RoleAccess();

            model_RA.role_access_id = Int64.Parse(HideRA_ID.Value.ToString());
            model_RA.role_id = Guid.Parse(HideRA_roleID.Value.ToString());
            model_RA.page_id = Guid.Parse(HideRA_pageID.Value.ToString());
            if (bool.Parse(HideRA_Active.Value.ToString()) == true)
            {
                model_RA.is_active = false;
            }
            else if (bool.Parse(HideRA_Active.Value.ToString()) == false)
            {
                model_RA.is_active = true;
            }
            model_RA.created_by = HideCreatby.Value.ToString();
            model_RA.created_date = DateTime.Parse(HideCreatdate.Value.ToString());
            model_RA.modified_by = Helper.UserLogin(this.Page);
            model_RA.modified_date = DateTime.Now;

            var hasil = clsRoleAccess.PutDataRoleAccess(model_RA.role_access_id, model_RA);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                ShowToastr("Mapping Role Access : status successfully updated", "Save Success", "success");
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonChangeStatus_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk mendisable data yang sudah termapping
        protected void ButtonDisableDDL_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            getDataPageDDL(Guid.Parse(HiddenAppID.Value.ToString()));

            List<ViewListPage> ListPageDataCek = new List<ViewListPage>();
            var Get_PageCek = clsViewListPage.GetDataPage(Guid.Parse(HiddenRoleID.Value.ToString()), Guid.Parse(HiddenAppID.Value.ToString()));
            var Get_DataPageCek = JsonConvert.DeserializeObject<Result_Data_Page_List>(Get_PageCek.Result.ToString());

            ListPageDataCek = Get_DataPageCek.list;

            if (ListPageDataCek.Count() > 0)
            {
                foreach (ListItem item in Add_DDLPagename.Items)
                {
                    for (int i = 0; i < ListPageDataCek.Count; i++)
                    {
                        if (item.Value.ToString() == ListPageDataCek[i].page_id.ToString())
                        {
                            //item.Attributes.Add("disabled", "disabled");
                            //item.Attributes.Add("style","color:lightgrey;");
                            item.Text = item.Text + " - (already mapping)";
                        }
                    }
                }
            }

            //fungsi untuk menjaga style pada dropdown search
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refresh", "$('.selectpicker').selectpicker('refresh');", addScriptTags: true);

            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonDisableDDL_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }
    }
}