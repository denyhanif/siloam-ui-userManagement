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
    public partial class OrganizationMaster : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (!IsPostBack)
            {
                loadLinkActive();
                getDataOrganization();
                getDDLdata_OrgHope();
                getDDLdata_OrgMobile();
                getDDLdata_OrgAX();
                HiddenFlagCari.Value = 0.ToString();
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
            myObject = (HtmlContainerControl)Master.FindControl("divBoxOrg");

            myObject.Style.Add("Background-color", "#1a2269");
            myObject.Style.Add("border-right", "5px solid #ffd800");
            myObject.Style.Add("color", "#f2c22c");
        }

        //get list data organization
        void getDataOrganization()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Organization> ListOrgData = new List<Organization>();
                var Get_Org = clsOrganization.GetDataOrganization();
                var Get_DataOrg = JsonConvert.DeserializeObject<Result_Data_Organization>(Get_Org.Result.ToString());

                ListOrgData = Get_DataOrg.list;

                if (ListOrgData.Count() > 0)
                {
                    DataTable dt_org = Helper.ToDataTable(ListOrgData);

                    //fungsi sorting datatable
                    dt_org.DefaultView.Sort = "organization_name ASC";
                    dt_org = dt_org.DefaultView.ToTable();

                    GridViewOrganization.DataSource = dt_org;
                    GridViewOrganization.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Org.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrganization", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrganization", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
            }
        }

        //fungsi link pagination pada gridview
        protected void GridViewOrganization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewOrganization.PageIndex = e.NewPageIndex;
            if (HiddenFlagCari.Value == "0")
            {
                getDataOrganization();
            }
            else if (HiddenFlagCari.Value == "1")
            {
                getDataOrganizationSearch(Search_masterData.Text);
            }        
        }

        //fungsi untuk mengecek data yang diiputkan apakah sudah ada di DB atau belum
        public bool getDataOrgExist(string orgname)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool eksis = false;
            try
            {
                if (orgname == "")
                {
                    eksis = true;
                }
                else
                {
                    List<Organization> ListOrgData = new List<Organization>();
                    var Get_Orgs = clsOrganization.GetDataOrganizationbyOrgName(orgname);
                    var Get_DataOrg = JsonConvert.DeserializeObject<Result_Data_Organization>(Get_Orgs.Result.ToString());

                    ListOrgData = Get_DataOrg.list;

                    if (ListOrgData.Count() > 0)
                    {
                        eksis = true;
                    }
                    else
                    {
                        eksis = false;
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrgExist", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrgExist", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
            return eksis;
        }

        //fungsi get data untuk bind ke DropDownList
        void getDDLdata_OrgHope()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<XOrgHOPE> ListOrgHOPE = new List<XOrgHOPE>();
                var Get_OrgHOPE = clsXOrgHOPE.GetDataOrgHopeID();
                var Get_DataOrgHOPE = JsonConvert.DeserializeObject<Result_Data_XorgHope>(Get_OrgHOPE.Result.ToString());

                //ListOrgHOPE = Get_DataOrgHOPE.list;
                ListOrgHOPE.Add(new XOrgHOPE { organizationId = 0, name = "--select organization--" });
                ListOrgHOPE.Add(new XOrgHOPE { organizationId = -1, name = "Default Organization" });
                ListOrgHOPE.Add(new XOrgHOPE { organizationId = -2, name = "Alternate Organization" });
                ListOrgHOPE.AddRange(Get_DataOrgHOPE.list);

                if (ListOrgHOPE.Count() > 0)
                {
                    DataTable dt_org_hope = Helper.ToDataTable(ListOrgHOPE);

                    Edit_DDLHopeID.DataTextField = "name";
                    Edit_DDLHopeID.DataValueField = "organizationId";

                    Edit_DDLHopeID.DataSource = dt_org_hope;
                    Edit_DDLHopeID.DataBind();

                    Add_DDLHopeID.DataTextField = "name";
                    Add_DDLHopeID.DataValueField = "organizationId";

                    Add_DDLHopeID.DataSource = dt_org_hope;
                    Add_DDLHopeID.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_OrgHOPE.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDDLdata_OrgHope", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDDLdata_OrgHope", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi get data untuk bind ke DropDownList
        void getDDLdata_OrgMobile()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<XOrgMOBILE> ListOrgMOBILE = new List<XOrgMOBILE>();
                var Get_OrgMOBILE = clsXOrgMOBILE.GetDataOrgMobileID();
                var Get_DataOrgMOBILE = JsonConvert.DeserializeObject<Result_Data_XorgMobile>(Get_OrgMOBILE.Result.ToString());

                //ListOrgMOBILE = Get_DataOrgMOBILE.list;
                ListOrgMOBILE.Add(new XOrgMOBILE { hospital_id = null, name = "--select organization--" });
                ListOrgMOBILE.AddRange(Get_DataOrgMOBILE.list);

                if (ListOrgMOBILE.Count() > 0)
                {
                    DataTable dt_org_mobile = Helper.ToDataTable(ListOrgMOBILE);

                    Edit_DDLMobileID.DataTextField = "name";
                    Edit_DDLMobileID.DataValueField = "hospital_id";

                    Edit_DDLMobileID.DataSource = dt_org_mobile;
                    Edit_DDLMobileID.DataBind();

                    Add_DDLMobileID.DataTextField = "name";
                    Add_DDLMobileID.DataValueField = "hospital_id";

                    Add_DDLMobileID.DataSource = dt_org_mobile;
                    Add_DDLMobileID.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_OrgMOBILE.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDDLdata_OrgMobile", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDDLdata_OrgMobile", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi get data untuk bind ke DropDownList
        void getDDLdata_OrgAX()
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<XOrgAX> ListOrgAX = new List<XOrgAX>();
                var Get_OrgAX = clsXOrgAX.GetDataOrgAxID();
                var Get_DataOrgAX = JsonConvert.DeserializeObject<Result_Data_XorgAX>(Get_OrgAX.Result.ToString());

                //ListOrgAX = Get_DataOrgAX.list;
                ListOrgAX.Add(new XOrgAX { id = "", name = "--select organization--" });
                ListOrgAX.AddRange(Get_DataOrgAX.list);

                if (ListOrgAX.Count() > 0)
                {
                    DataTable dt_org_ax = Helper.ToDataTable(ListOrgAX);

                    Edit_DDLAxID.DataTextField = "name";
                    Edit_DDLAxID.DataValueField = "id";

                    Edit_DDLAxID.DataSource = dt_org_ax;
                    Edit_DDLAxID.DataBind();

                    Add_DDLAxID.DataTextField = "name";
                    Add_DDLAxID.DataValueField = "id";

                    Add_DDLAxID.DataSource = dt_org_ax;
                    Add_DDLAxID.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_OrgAX.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, Status, "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDDLdata_OrgAX", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDDLdata_OrgAX", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
        }

        //fungsi untuk mengecek data sudah ada di DB atau belum
        public bool getDataOrgHOPEExist(Int64? orgHopeID)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool eksis = false;
            try
            {
                if (orgHopeID == 0)
                {
                    eksis = false;
                }
                else
                {
                    List<Organization> ListOrgData = new List<Organization>();
                    var Get_Orgs = clsOrganization.GetDataOrganizationbyHopeID(orgHopeID);
                    var Get_DataOrg = JsonConvert.DeserializeObject<Result_Data_Organization>(Get_Orgs.Result.ToString());

                    ListOrgData = Get_DataOrg.list;

                    if (ListOrgData.Count() > 0)
                    {
                        eksis = true;
                    }
                    else
                    {
                        eksis = false;
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrgHOPEExist", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrgHOPEExist", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
            return eksis;
        }

        //fungsi untuk mengecek data sudah ada di DB atau belum
        public bool getDataOrgMOBILEExist(Guid? orgMobileID)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool eksis = false;
            try
            {
                if (orgMobileID == null)
                {
                    eksis = false;
                }
                else
                {
                    List<Organization> ListOrgData = new List<Organization>();
                    var Get_Orgs = clsOrganization.GetDataOrganizationbyMobileID(orgMobileID);
                    var Get_DataOrg = JsonConvert.DeserializeObject<Result_Data_Organization>(Get_Orgs.Result.ToString());

                    ListOrgData = Get_DataOrg.list;

                    if (ListOrgData.Count() > 0)
                    {
                        eksis = true;
                    }
                    else
                    {
                        eksis = false;
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrgMOBILEExist", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrgMOBILEExist", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
            return eksis;
        }

        //fungsi untuk mengecek data sudah ada di DB atau belum
        public bool getDataOrgAXExist(string orgAxID)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bool eksis = false;
            try
            {
                if (orgAxID == "")
                {
                    eksis = false;
                }
                else
                {
                    List<Organization> ListOrgData = new List<Organization>();
                    var Get_Orgs = clsOrganization.GetDataOrganizationbyAxID(orgAxID);
                    var Get_DataOrg = JsonConvert.DeserializeObject<Result_Data_Organization>(Get_Orgs.Result.ToString());

                    ListOrgData = Get_DataOrg.list;

                    if (ListOrgData.Count() > 0)
                    {
                        eksis = true;
                    }
                    else
                    {
                        eksis = false;
                    }
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrgAXExist", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                p_Add.InnerText = "";
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrgAXExist", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
            }
            return eksis;
        }

        //fungsi untuk me reset form input dan notifikasi menjadi kosong kembali
        void clearFormAdd()
        {
            Add_TextOrgname.Text = "";
            Add_DDLHopeID.SelectedValue = 0.ToString();
            Add_DDLMobileID.SelectedValue = "";
            Add_DDLAxID.SelectedValue = "";

            p_Add.InnerText = "";
            p_Edit.InnerText = "";
        }

        //fungsi button save edit data
        protected void Edit_ButtonSaveOrg_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            int tandaiSave = 0;

            Organization model_Org = new Organization();

            model_Org.organization_id = Int64.Parse(Edit_HiddenOrgID.Value.ToString());
            model_Org.organization_name = Edit_TextOrgname.Text.ToString();
            model_Org.hope_organization_id = Int64.Parse(Edit_DDLHopeID.SelectedValue.ToString());
            if (Edit_DDLMobileID.SelectedValue != "")
            {
                model_Org.mobile_organization_id = Guid.Parse(Edit_DDLMobileID.SelectedValue.ToString());
            }
            else
            {
                model_Org.mobile_organization_id = null;
            }
            model_Org.ax_organization_id = Edit_DDLAxID.SelectedValue.ToString();
            model_Org.is_active = true;
            model_Org.created_by = Edit_HiddenCreateby.Value.ToString();
            model_Org.created_date = DateTime.Parse(Edit_HiddenCreatedate.Value.ToString());
            model_Org.modified_by = Helper.UserLogin(this.Page);
            model_Org.modified_date = DateTime.Now;

            if (getDataOrgExist(Edit_TextOrgname.Text.ToString()) == false)
            {
                tandaiSave = 1;
            }
            else
            {
                if (Edit_TextOrgname.Text != Edit_hiddenTextOrgname.Value.ToString())
                {
                    p_Edit.Attributes.Remove("style");
                    p_Edit.Attributes.Add("style", "display:block; color:red;");
                    p_Edit.InnerText = "Organization Name Already Exist!";

                    Edit_TextOrgname.Text = Edit_hiddenTextOrgname.Value.ToString();
                    goto SKIP;
                }
                else
                {
                    tandaiSave = 1;
                }
            }

            if (getDataOrgHOPEExist(model_Org.hope_organization_id) == false)
            {
                tandaiSave = 1;
            }
            else
            {
                if (Edit_DDLHopeID.SelectedValue.ToString() != Edit_HiddenOrgHopeID.Value.ToString())
                {
                    p_Edit.Attributes.Remove("style");
                    p_Edit.Attributes.Add("style", "display:block; color:red;");
                    p_Edit.InnerText = "Hope Organization already used in another Organization!";

                    goto SKIP;
                }
                else
                {
                    tandaiSave = 1;
                }
            }

            if (getDataOrgMOBILEExist(model_Org.mobile_organization_id) == false)
            {
                tandaiSave = 1;
            }
            else
            {
                if (Edit_DDLMobileID.SelectedValue.ToString() != Edit_HiddenOrgMobileID.Value.ToString())
                {
                    p_Edit.Attributes.Remove("style");
                    p_Edit.Attributes.Add("style", "display:block; color:red;");
                    p_Edit.InnerText = "Mobile Organization already used in another Organization!";

                    goto SKIP;
                }
                else
                {
                    tandaiSave = 1;
                }
            }

            if (getDataOrgAXExist(model_Org.ax_organization_id) == false)
            {
                tandaiSave = 1;
            }
            else
            {
                if (Edit_DDLAxID.SelectedValue.ToString() != Edit_HiddenOrgAxID.Value.ToString())
                {
                    p_Edit.Attributes.Remove("style");
                    p_Edit.Attributes.Add("style", "display:block; color:red;");
                    p_Edit.InnerText = "Ax Organization already used in another Organization!";

                    goto SKIP;
                }
                else
                {
                    tandaiSave = 1;
                }
            }

            if (tandaiSave == 1)
            {
                var hasil = clsOrganization.PutDataOrganization(model_Org.organization_id, model_Org);

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
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalEditOrg').modal('hide');", addScriptTags: true);
                    ShowToastr("Organization : data successfully changed", "Save Success", "success");
                    getDataOrganization();

                    clearFormAdd();
                }
            }

        SKIP:
            tandaiSave = 0;


            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Edit_ButtonSaveOrg_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button save add data
        protected void Add_ButtonSaveOrg_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Organization model_Org = new Organization();

            model_Org.organization_name = Add_TextOrgname.Text.ToString();
            model_Org.hope_organization_id = Int64.Parse(Add_DDLHopeID.SelectedValue.ToString());
            if (Add_DDLMobileID.SelectedValue != "")
            {
                model_Org.mobile_organization_id = Guid.Parse(Add_DDLMobileID.SelectedValue.ToString());
            }
            else
            {
                model_Org.mobile_organization_id = null;
            }
            model_Org.ax_organization_id = Add_DDLAxID.SelectedValue.ToString();
            model_Org.is_active = true;
            model_Org.created_by = Helper.UserLogin(this.Page);
            model_Org.created_date = DateTime.Now;
            model_Org.modified_by = null;
            model_Org.modified_date = DateTime.Now;

            if (getDataOrgExist(Add_TextOrgname.Text.ToString()) == false)
            {
                if (getDataOrgHOPEExist(model_Org.hope_organization_id) == true)
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Hope Organization already used in another Organization!";
                }
                else if (getDataOrgMOBILEExist(model_Org.mobile_organization_id) == true)
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Mobile Organization already used in another Organization!";
                }
                else if (getDataOrgAXExist(model_Org.ax_organization_id) == true)
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Ax Organization already used in another Organization!";
                }
                else
                {
                    var hasil = clsOrganization.PostDataOrganization(model_Org);

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
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalAddOrg').modal('hide');", addScriptTags: true);
                        ShowToastr("Organization : data successfully added", "Save Success", "success");
                        getDataOrganization();

                        clearFormAdd();
                    }
                }
            }
            else
            {
                if (getDataOrgExist(Add_TextOrgname.Text.ToString()) == true)
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerText = "Organization Name Already Exist!";

                    Add_TextOrgname.Text = "";
                }
            }


            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Add_ButtonSaveOrg_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi button update status active
        protected void ButtonChangeStatus_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Organization model_Org = new Organization();

            model_Org.organization_id = Int64.Parse(HideOrgID.Value.ToString());
            model_Org.organization_name = HideOrgName.Value.ToString();
            model_Org.hope_organization_id = Int64.Parse(HideOrgHopeID.Value.ToString());
            if (HideOrgMobileID.Value.ToString() != "")
            {
                model_Org.mobile_organization_id = Guid.Parse(HideOrgMobileID.Value.ToString());
            }
            else
            {
                model_Org.mobile_organization_id = null;
            }
            model_Org.ax_organization_id = HideOrgAxID.Value.ToString();

            if (bool.Parse(HideOrgActive.Value.ToString()) == true)
            {
                model_Org.is_active = false;
            }
            else if (bool.Parse(HideOrgActive.Value.ToString()) == false)
            {
                model_Org.is_active = true;
            }
            model_Org.created_by = HideCreatby.Value.ToString();
            model_Org.created_date = DateTime.Parse(HideCreatdate.Value.ToString());
            model_Org.modified_by = Helper.UserLogin(this.Page);
            model_Org.modified_date = DateTime.Now;

            var hasil = clsOrganization.PutDataOrganization(model_Org.organization_id, model_Org);

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                ShowToastr("Organization : status successfully updated", "Save Success", "success");
                getDataOrganization();
            }


            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "ButtonChangeStatus_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //get list data organization pencarian
        void getDataOrganizationSearch(string keyword)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<Organization> ListOrgData = new List<Organization>();
                var Get_Org = clsOrganization.GetDataOrganizationbySearch(keyword);
                var Get_DataOrg = JsonConvert.DeserializeObject<Result_Data_Organization>(Get_Org.Result.ToString());

                ListOrgData = Get_DataOrg.list;

                if (ListOrgData.Count() > 0)
                {
                    DataTable dt_org = Helper.ToDataTable(ListOrgData);

                    //fungsi sorting datatable
                    dt_org.DefaultView.Sort = "organization_name ASC";
                    dt_org = dt_org.DefaultView.ToTable();

                    GridViewOrganization.DataSource = dt_org;
                    GridViewOrganization.DataBind();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_Org.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    ShowToastr(Message, "Data not found", "warning");
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrganizationSearch", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception exx)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "getDataOrganizationSearch", StartTime, "ERROR", MyUser.GetUsername(), "", "", exx.Message));
                ShowToastr("Please Check Your Connection!", "Error Load Data", "Error");
            }
        }

        //fungsi klik button pencarian
        protected void ButtonCari_Click(object sender, EventArgs e)
        {
            if (Search_masterData.Text != "")
            {
                getDataOrganizationSearch(Search_masterData.Text);
                HiddenFlagCari.Value = 1.ToString();
            }
            else
            {
                getDataOrganization();
                HiddenFlagCari.Value = 0.ToString();
            }
        }
    }
}