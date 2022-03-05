using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Siloam.Ui.UserManagement.API_Code.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Siloam.Ui.UserManagement.Pages.Viewer
{
    public partial class UpdatePassword : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (!IsPostBack)
            {
                HF_Username.Value = Request.QueryString["Username"];

                var registryflag = ConfigurationManager.AppSettings["registryflag"].ToString();
                if (registryflag == "1")
                {
                    ConfigurationManager.AppSettings["urlUserManagement"] = SiloamConfig.Functions.GetValue("urlUserManagement").ToString();
                }
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Page_Load", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        protected void Pass_ButtonSavePass_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                var hasil = clsUser.ChangePasswordUser(HF_Username.Value, Pass_TextOldPass.Text, Pass_TextNewPass.Text, HF_Username.Value);
                //var hasilCentral = clsCommon.ChangePasswordUserCentral(HF_Username.Value, Pass_TextOldPass.Text, Pass_TextNewPass.Text, HF_Username.Value);

                var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
                var Status = Response.Property("status").Value.ToString();
                var Message = Response.Property("message").Value.ToString();

                if (Status == "Fail")
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:red;");
                    p_Add.InnerHtml = Message;
                    //ShowToastr(Status + "! " + Message, "Save Failed", "error");
                }
                else
                {
                    p_Add.Attributes.Remove("style");
                    p_Add.Attributes.Add("style", "display:block; color:green;");
                    p_Add.InnerHtml = "Change Password Success! <br />Silakan Login Kembali.";

                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalAfterSave').modal('show');", addScriptTags: true);
                    clearFormPass();

                    Pass_TextOldPass.Enabled = false;
                    Pass_TextNewPass.Enabled = false;
                    Pass_TextNewPass_confirm.Enabled = false;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hidemodal", "parent.hideChangePass();", true);
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Pass_ButtonSavePass_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
            }
            catch (Exception ex)
            {
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Pass_ButtonSavePass_Click", StartTime, "ERROR", MyUser.GetUsername(), "", "", ex.Message));
                throw ex;
            }
        }

        //fungsi untuk clear form input
        void clearFormPass()
        {
            Pass_TextOldPass.Text = "";
            Pass_TextNewPass.Text = "";
            Pass_TextNewPass_confirm.Text = "";
        }
    }
}