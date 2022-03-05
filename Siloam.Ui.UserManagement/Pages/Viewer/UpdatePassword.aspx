<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdatePassword.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.Viewer.UpdatePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" style="height: 100%; overflow-y: hidden;">
<head id="HeadUMS" runat="server">
    <meta charset="utf-8" />

    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <title>User Management System</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <!-- Font Awesome -->
    <link rel="stylesheet" href="~/Content/font-awesome/css/font-awesome.css" />
    <link rel="stylesheet" href="~/Content/font-awesome/css/style.css" />

    <!-- Bootstrap -->
    <link rel="stylesheet" href="~/Content/bootstrap.css" />

    <!-- Toast -->
    <link rel="stylesheet" href="~/Content/plugins/toast/toastr.css" />

</head>

<body style="padding-top: 0px; padding-bottom: 0px;">
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />

                <asp:ScriptReference Path="~/Content/plugins/toast/toastr.min.js" />

                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>

        <div class="container-fluid">
            <asp:UpdatePanel ID="UpdatePanelChangePass" runat="server">
                <ContentTemplate>

                    <asp:HiddenField ID="HF_Username" runat="server" />

                    <div class="row">

                        <div class="col-sm-4 col-sm-offset-4 Contentutama">
                            <div class="row borderTitle" style="padding-top: 10px; padding-bottom: 10px; display: none;">
                                <div class="col-sm-8 TeksHeader" style="padding-top: 5px;"><b>
                                    <label id="lblbhs_changepass" style="font-size: 16px;">Change Password</label>
                                </b></div>
                                <div class="col-sm-4 text-right">
                                    <asp:UpdateProgress ID="PassuProgSAVE" runat="server" AssociatedUpdatePanelID="UpdatePanelSAVE">
                                        <ProgressTemplate>
                                            <img alt="" style="background-color: transparent; height: 20px;" src="<%= Page.ResolveClientUrl("~/Images/Background/small-loader.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                </div>
                            </div>

                            <br />

                            <div class="form-group">
                                <label id="lblbhs_recentpass">Recent password</label>
                                <asp:TextBox ID="Pass_TextOldPass" runat="server" CssClass="MaxWidthTextbox form-control" Style="height: 34px;" TextMode="Password" placeholder="Type here..." AutoCompleteType="Disabled"></asp:TextBox>
                                <a href="#" style="text-decoration: none; color: #171717; right: 25px; margin-top: -27px; position: absolute;" onclick="Toggle('Pass_TextOldPass','.mata1')"><i class="fa fa-eye-slash mata1"></i></a>
                            </div>
                            <div class="form-group">
                                <label id="lblbhs_newpass">New password</label>
                                <asp:TextBox ID="Pass_TextNewPass" runat="server" CssClass="MaxWidthTextbox form-control" Style="height: 34px;" TextMode="Password" placeholder="Type here..." AutoCompleteType="Disabled"></asp:TextBox>
                                <a href="#" style="text-decoration: none; color: #171717; right: 25px; margin-top: -27px; position: absolute;" onclick="Toggle('Pass_TextNewPass','.mata2')"><i class="fa fa-eye-slash mata2"></i></a>
                            </div>
                            <div class="form-group">
                                <label id="lblbhs_confirmnewpass">Confirm new password</label>
                                <asp:TextBox ID="Pass_TextNewPass_confirm" runat="server" CssClass="MaxWidthTextbox form-control" Style="height: 34px;" TextMode="Password" placeholder="Type here..." AutoCompleteType="Disabled"></asp:TextBox>
                                <a href="#" style="text-decoration: none; color: #171717; right: 25px; margin-top: -27px; position: absolute;" onclick="Toggle('Pass_TextNewPass_confirm','.mata3')"><i class="fa fa-eye-slash mata3"></i></a>
                            </div>

                            <table border="0" style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanelCekPass" runat="server">
                                            <ContentTemplate>
                                                <b>
                                                    <p style="color: red; display: none" id="p_Add" runat="server"></p>
                                                </b>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td style="width: 30%; text-align: right;">
                                        <asp:UpdatePanel ID="UpdatePanelSAVE" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="Pass_ButtonSavePass" runat="server" Text="Change Password" CssClass="btn btn-success" OnClientClick="return FormCheck()" OnClick="Pass_ButtonSavePass_Click"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>

                            <br />
                        </div>

                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <script type="text/javascript">

            //fungsi toogle icon eye pada form password
            function Toggle(obj, cls) {
                var temp = document.getElementById(obj);
                if (temp.type === "password") {
                    temp.type = "text";
                    $(cls).removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                }
                else {
                    temp.type = "password";
                    $(cls).removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
            }

            //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
            function FormCheck()
            {
                if ($("[id$='Pass_TextOldPass']").val().length == 0) {
                    $("[id$='Pass_TextOldPass']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Old Password cannot be empty!";

                    return false;
                }
                else if ($("[id$='Pass_TextNewPass']").val().length == 0) {
                    $("[id$='Pass_TextNewPass']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "New Password cannot be empty!";

                    return false;
                }
                else if ($("[id$='Pass_TextNewPass_confirm']").val().length == 0) {
                    $("[id$='Pass_TextNewPass_confirm']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Confirm New Password cannot be empty!";

                    return false;
                }
                else if ($("[id$='Pass_TextNewPass']").val() != $("[id$='Pass_TextNewPass_confirm']").val()) {
                    $("[id$='Pass_TextNewPass_confirm']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Confirm New Password must be same with New Password!";

                    return false;
                }

                return validatePass($("[id$='Pass_TextNewPass']").val());
               
            }

            function validatePass(objval) {
                var value = objval;
                var regex = /^(?=.{8,})(?=.*[a-zA-Z])(?=.*[0-9]).*$/; //(?=.*[@#$%^&+=])
                var bolvalue = regex.test(value);
                if (bolvalue == true) {
                    $("[id$='p_Add']").removeAttr("style");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "";
                    return true;
                }
                else {
                    $("[id$='Pass_TextNewPass']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "The password must has minimum 8 characters at least 1 Alphabet and 1 Number!"; //and 1 Special Character
                    return false;
                }
            }


            $(document).ready(function () {

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                if (prm != null) {
                    prm.add_endRequest(function (sender, e) {
                        if (sender._postBackSettings.panelsToUpdate != null) {

                        }
                    });
                };
            });

        </script>

    </form>
</body>
</html>

