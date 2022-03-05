using System;
using System.Web.UI;

using Siloam.Ui.UserManagement.Pages.Common;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Siloam.Ui.UserManagement.Pages
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();

        }

        //fungsi untuk menampilkan toast via akses javascript
        void ShowToastr(string message, string title, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "toastr_message",
                String.Format("toastr.{0}('{1}', '{2}');", type.ToLower(), message, title), addScriptTags: true);
        }

        //fungsi save change password
        protected void Pass_ButtonSavePass_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var hasil = clsUser.ChangePasswordUser(Helper.UserLogin(this.Page), Pass_TextOldPass.Text, Pass_TextNewPass.Text, Helper.UserLogin(this.Page));

            var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(hasil.Result);
            var Status = Response.Property("status").Value.ToString();
            var Message = Response.Property("message").Value.ToString();

            if (Status == "Fail")
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:red;");
                p_Add.InnerText = Message;
                ShowToastr(Status + "! " + Message, "Save Failed", "error");
            }
            else
            {
                p_Add.Attributes.Remove("style");
                p_Add.Attributes.Add("style", "display:block; color:green;");
                p_Add.InnerText = "Change Password Success!";
                
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalAfterSave').modal('show');", addScriptTags: true);
                clearFormPass();
            }
            Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", MyUser.GetUsername(), "Pass_ButtonSavePass_Click", StartTime, "OK", MyUser.GetUsername(), "", "", ""));
        }

        //fungsi untuk clear form input
        void clearFormPass()
        {
            Pass_TextOldPass.Text = "";
            Pass_TextNewPass.Text = "";
            Pass_TextNewPass_confirm.Text = "";
        }

        //fungsi klik button relogin
        protected void ButtonRelogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Login_page.aspx", false);
        }
    }
}