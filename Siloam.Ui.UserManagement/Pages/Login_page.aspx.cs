using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Net;

using Siloam.Ui.UserManagement.API_Code.Models;
using Siloam.Ui.UserManagement.API_Code.Controller;
using Siloam.Ui.UserManagement.Pages.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.UI;
using System.Configuration;

namespace Siloam.Ui.UserManagement.Pages
{
    public partial class Login_page : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ThreadContext.Properties["Organization"] = MyUser.GetOrgId();

            if (!IsPostBack)
            {
                if (Request.QueryString["action"] != null)
                {
                    if (Request.QueryString["action"] == "clear")
                    {
                        Session.Abandon();
                    }
                }

                var registryflag = ConfigurationManager.AppSettings["registryflag"].ToString();

                if (registryflag == "1")
                {
                    ConfigurationManager.AppSettings["URLUserManagement"] = SiloamConfig.Functions.GetValue("urlUserManagement").ToString();
                }
            }
        }

        //fungsi untuk menampilkan toast via akses javascript
        void ShowToastr(string message, string title, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "toastr_message",
                String.Format("toastr.{0}('{1}', '{2}');", type.ToLower(), message, title), addScriptTags: true);
        }

        //fungsi klik button login
        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string Pesan = "";
            var finish = 0;
            try
            {
                btnSignIn.Enabled = false;
                string usernameLogin = txtUsername.Text.ToString().Replace('\\','+');
                string passwordLogin = txtPassword.Text.ToString();

                //pengecekan pada username AD
                if (usernameLogin.Contains("+") == true)
                {
                    var cekUserType = usernameLogin.Split('+');
                    if (cekUserType[0].Length >= 12)
                    {
                        passwordLogin = "12345678";
                    }

                    //login with AD manually
                    try
                    {
                        var UsernameData = usernameLogin.Split('+');
                        LdapConnection connection = new LdapConnection(UsernameData[0].ToString());
                        NetworkCredential credential = new NetworkCredential(UsernameData[1].ToString(), txtPassword.Text.ToString());
                        connection.Credential = credential;
                        connection.Bind();
                    }
                    catch (LdapException lexc)
                    {
                        string error = lexc.ServerErrorMessage;
                        pError.InnerText = "Login AD Fail! " + error;
                        pError.Attributes.Remove("style");
                        pError.Attributes.Add("style", "display:block; color:red;");
                        goto FINISHH;
                    }
                    catch (Exception exc)
                    {
                        pError.InnerText = "Login AD Fail! " + exc.ToString();
                        pError.Attributes.Remove("style");
                        pError.Attributes.Add("style", "display:block; color:red;");
                        goto FINISHH;
                    }
                }

                List<ViewLoginUser> ListLoginData = new List<ViewLoginUser>();
                var GetLogin = clsLoginUser.GetLogin(usernameLogin, passwordLogin);
                var GetDataLogin = JsonConvert.DeserializeObject<Result_login_user>(GetLogin.Result.ToString());

                ListLoginData = GetDataLogin.list;

                if (ListLoginData.Count() > 0)
                {
                    if (txtPassword.Text == "12345678" && !usernameLogin.Contains("+"))
                    {
                        if (true)
                        {
                            LabelChangePassTitle.Text = "Silakan Ganti Password Default Anda.";
                            string localIP = Helper.GetLocalIPAddress();
                            //string localIP = "10.85.138.25"; //hardcode GTN

                            var registryflag = ConfigurationManager.AppSettings["registryflag"].ToString();

                            if (registryflag == "0")
                            {
                                iframechangepass.Src = "http://" + localIP + "/UserManagement/Pages/Viewer/UpdatePassword.aspx?Username=" + txtUsername.Text;
                            }
                            else if (registryflag == "1")
                            {
                                iframechangepass.Src = "http://" + localIP + "/viewer/Form/FormViewer/FormChangePassword?Username=" + txtUsername.Text;
                            }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalChangePass", "$('#modalChangePass').modal({backdrop: 'static', keyboard: false});", true);
                        goto FINISHH;
                    }
                    }

                    int flagexp = 0;
                    var ResponseLogin = (JObject)JsonConvert.DeserializeObject<dynamic>(GetLogin.Result);
                    Pesan = ResponseLogin.Property("message").Value.ToString();
                    if (Pesan.Contains("Password expired"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertpassexpired", "alert('" + Pesan + "');", true);
                    }

                    DataTable dt_login = Helper.ToDataTable(ListLoginData);
                    Session[Helper.Session_DataLogin] = dt_login;

                    if (flagexp == 1)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect", "redirectlogin();", true);
                    }
                    else
                    {
                        Response.Redirect("~/Pages/Home_logon_page.aspx", false);
                    }
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(GetLogin.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    pError.InnerText = Status + "! " + Message;
                    pError.Attributes.Remove("style");
                    pError.Attributes.Add("style", "display:block; color:red;");
                    txtUsername.BorderColor = Color.Red;
                    txtPassword.BorderColor = Color.Red;

                    if (Message.Contains("expired"))
                    {
                        LabelChangePassTitle.Text = "Password Expired! Please update your password.";
                        string localIP = Helper.GetLocalIPAddress();
                        //string localIP = "10.85.138.25"; //hardcode GTN
                        var registryflag = ConfigurationManager.AppSettings["registryflag"].ToString();

                        if (registryflag == "0")
                        {
                            iframechangepass.Src = "http://" + localIP + "/UserManagement/Pages/Viewer/UpdatePassword.aspx?Username=" + txtUsername.Text;
                        }
                        else if (registryflag == "1")
                        {
                            iframechangepass.Src = "http://" + localIP + "/viewer/Form/FormViewer/FormChangePassword?Username=" + txtUsername.Text;
                        }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalChangePass", "$('#modalChangePass').modal({backdrop: 'static', keyboard: false}); toastr.warning('Password is Expired!', 'Warning');", true);
                    }
                }

                btnSignIn.Enabled = true;
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", txtUsername.Text.ToString().Replace('\\', '+'), "btnSignIn_Click", StartTime, "OK", txtUsername.Text.ToString().Replace('\\', '+'), "", "", ""));
            }
            catch(Exception exx)
            {
                string Status = "Fail";
                pError.InnerText = Status + "! " + exx.Message;
                pError.Attributes.Remove("style");
                pError.Attributes.Add("style", "display:block; color:red;");
                txtUsername.BorderColor = Color.Red;
                txtPassword.BorderColor = Color.Red;

                btnSignIn.Enabled = true;
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", txtUsername.Text.ToString().Replace('\\', '+'), "btnSignIn_Click", StartTime, "ERROR", txtUsername.Text.ToString().Replace('\\', '+'), "", "", exx.Message));
            }

        FINISHH:
            btnSignIn.Enabled = true;
            finish = 1;
        }

        //fungsi inisialisasi username AD secara otomatis saat checkbox dicentang
        protected void CheckBoxLoginAD_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxLoginAD.Checked)
            {
                txtUsername.Text = HttpContext.Current.User.Identity.Name.ToString();
                txtPassword.Enabled = false;
                txtUsername.Enabled = false;
            }
            else
            {
                txtUsername.Text = "";
                txtPassword.Enabled = true;
                txtUsername.Enabled = true;
            }
        }

        protected void Forgot_ButtonSubmit_Click(object sender, EventArgs e)
        {
            string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                List<User> ListUserData = new List<User>();
                var Get_User = clsUser.GetDataUserForgotPass(ForgotUsername.Text, ForgotEmail.Text);
                var Get_DataUser = JsonConvert.DeserializeObject<Result_Data_user>(Get_User.Result.ToString());

                ListUserData = Get_DataUser.list;

                if (ListUserData.Count() > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "message", "$('#modalForgotPass').modal('hide');", addScriptTags: true);
                    ShowToastr("The new password was sent to your email", "Password Successfully Reset", "success");
                    clearForm();
                }
                else
                {
                    var Response = (JObject)JsonConvert.DeserializeObject<dynamic>(Get_User.Result);
                    var Status = Response.Property("status").Value.ToString();
                    var Message = Response.Property("message").Value.ToString();

                    //ShowToastr(Message, Status, "warning");

                    p_Forgot.Attributes.Remove("style");
                    p_Forgot.Attributes.Add("style", "display:block; color:red;");
                    p_Forgot.InnerText = Status + "! " + Message;
                }
                Log.Debug(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", ForgotUsername.Text, "Forgot_ButtonSubmit_Click", StartTime, "OK", ForgotUsername.Text, "", "", ""));
            }
            catch (Exception exx)
            {
                ShowToastr("Please Check Your Connection!", "Error Load Data", "error");
                Log.Error(LogLibrary.SaveLog(MyUser.GetOrgId(), "username", ForgotUsername.Text, "Forgot_ButtonSubmit_Click", StartTime, "ERROR", ForgotUsername.Text, "", "", exx.Message));
            }
        }

        void clearForm()
        {
            ForgotUsername.Text = "";
            ForgotEmail.Text = "";

            p_Forgot.InnerText = "";
        }

        protected void ButtonRedirect_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Home_logon_page.aspx", false);
        }
    }
}