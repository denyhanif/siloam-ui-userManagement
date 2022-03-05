<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_page.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.Login_page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <link rel="stylesheet" href="~/Content/Custom.css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <!-- Bootstrap 3.3.6 -->
    <link rel="stylesheet" href="~/Content/bootstrap.css" />
    <link rel="stylesheet" href="~/Content/Site.css" />
    <link rel="stylesheet" href="~/Content/dist/css/AdminLTE.min.css" />

    <!-- Font Awesome -->
    <link rel="stylesheet" href="~/Content/font-awesome/css/font-awesome.css" />
    <link rel="stylesheet" href="~/Content/font-awesome/css/style.css" />

    <!-- Toast -->
    <link rel="stylesheet" href="~/Content/plugins/toast/toastr.css" />

    <!-- beackground pada body -->
    <style>
        body {
            background-image: url("<%= Page.ResolveClientUrl("~/Assets/Backgrounds/BG_Log_in.png") %>") !important;
            background-repeat: no-repeat;
            background-position: center;
            -moz-background-size: cover;
            -webkit-background-size: cover;
            -o-background-size: cover;
            background-size: cover;
        }

        .btn {
            transition: all 0.5s;
            color: #1a2268;
            outline: 1px solid #1a2268; /*#4d9b35*/
            width: 300px;
            max-width: 300px;
            margin-left: 2px;
        }

            .btn:hover {
                color: white;
                background-color: #1a2268;
                outline: 1px solid #1a2268;
            }

        .login-area {
            margin-top: 3%;
            margin-left: auto;
            margin-right: auto;
            width: 450px;
        }
    </style>

    <script type="text/javascript" src="../Scripts/jquery-3.3.1.js"></script>
    <script type="text/javascript">

        //fungsi agar tidak bisa back saat sudah logout
        history.pushState(null, null, document.title);
        window.addEventListener('popstate', function () {
            history.pushState(null, null, document.title);
        });

        //fungsi validasi form login kosong
        function CheckField() {
            var UserName = $("[id$='txtUsername']").val();
            var PassWord = $("[id$='txtPassword']").val();

            $("[id$='txtUsername']").removeAttr("style");
            $("[id$='txtPassword']").removeAttr("style");

            if (UserName.length <= 0 && PassWord.length <= 0) {
                //$("[id$='txtUsername']").attr("style", "display:block; border-color:red;");
                //$("[id$='txtPassword']").attr("style", "display:block; border-color:red;");
                $("[id$='pError']").removeAttr("style");
                $("[id$='txtUsername']").focus();
                $("[id$='pError']").attr("style", "display:block; color:red;");
                document.getElementById("pError").innerHTML = "Masukkan Username dan Password !";

                return false;
            }
            else if (UserName.length <= 0 && PassWord.length > 0) {
                // $("[id$='txtUsername']").attr("style", "display:block; border-color:red;");
                $("[id$='txtUsername']").focus();
                $("[id$='pError']").removeAttr("style");
                $("[id$='pError']").attr("style", "display:block; color:red;");
                document.getElementById("pError").innerHTML = "Masukkan Username !";

                return false;
            }
            else if (UserName.length > 0 && PassWord.length <= 0) {

                if (document.getElementById('<%= CheckBoxLoginAD.ClientID %>').checked == true) {
                    return true;
                }
                else {

                    //$("[id$='txtPassword']").attr("style", "display:block; border-color:red;");
                    $("[id$='txtPassword']").focus();
                    $("[id$='pError']").removeAttr("style");
                    $("[id$='pError']").attr("style", "display:block; color:red;");
                    document.getElementById("pError").innerHTML = "Masukkan Password !";

                    return false;
                }
            }
            else {
                return true;
            }
        }

        //fungsi untuk memberikan style warna random pada button login
        $(document).ready(function () {
            var colors = ['#c43d32', '#f2c22c', '#4d9b35', '#7b88ff', '#1a2268'];

            $('.MenuBox').mouseenter(function () {
                var rand = colors[Math.floor(Math.random() * colors.length)];
                $(this).css('background-color', rand);
            });

            $('.MenuBox').mouseleave(function () {
                $(this).css('background-color', '');
            });

            toastr.options.positionClass = "toast-top-center";
        });

        //fungsi toogle icon eye pada form password
        function Toggle() {
            var temp = document.getElementById('<%= txtPassword.ClientID %>');
            if (temp.type === "password") {
                temp.type = "text";
                $('.mata').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
            }
            else {
                temp.type = "password";
                $('.mata').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
            }
        }

        //fungsi untuk membatasi inputan hanya berupa regex
        function isUserName(evt) {
            evt = (evt) ? evt : window.event;
            var regex = new RegExp("^[a-zA-Z0-9_\.\\\\-]+$");
            var key = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
            if (!regex.test(key)) {
                evt.preventDefault();
                return false;
            }
            return true;
        }

        function FormCheck() {
            var Usernamee = $("[id$='ForgotUsername']").val();
            var Emaill = $("[id$='ForgotEmail']").val();

            if (Usernamee.length <= 0) {
                $("[id$='p_Forgot']").removeAttr("style");
                $("[id$='ForgotUsername']").focus();
                $("[id$='p_Forgot']").attr("style", "display:block; color:red;");
                document.getElementById("p_Forgot").innerHTML = "Username tidak boleh kosong !";

                return false;
            }
            else if (Emaill.length <= 0) {
                $("[id$='p_Forgot']").removeAttr("style");
                $("[id$='ForgotEmail']").focus();
                $("[id$='p_Forgot']").attr("style", "display:block; color:red;");
                document.getElementById("p_Forgot").innerHTML = "Email tidak boleh kosong !";

                return false;
            }
        }

        function redirectlogin() {
            document.getElementById("<%= ButtonRedirect.ClientID %>").click();
        }

        function hideChangePass() {
            $('#modalChangePass').modal('hide');
            changePassSuccess();
        }

        function changePassSuccess() {
            toastr.success('Change Password Success.', 'Success');
            toastr.options.positionClass = "toast-top-center";
        }

    </script>

</head>

<body class="">

    <form id="form1" runat="server">

        <!-- untuk melengkapi update panel ID upError -->
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Content/plugins/jQuery/jQuery-2.2.0.min.js" />
                <asp:ScriptReference Path="~/Scripts/bootstrap.min.js" />
                <asp:ScriptReference Path="~/Content/plugins/toast/toastr.min.js" />
            </Scripts>
        </asp:ScriptManager>

        <!-- loading progress modal -->
        <asp:UpdateProgress ID="uProgLogin" runat="server" AssociatedUpdatePanelID="upError">
            <ProgressTemplate>
                <div class="modal-backdrop" style="background-color: white; opacity: 0.4; vertical-align: middle; text-align: center; padding-top: 200px;">
                    <img alt="" height="200px" width="200px" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <!-- login form -->
        <div class="login-area" style="width: 450px;">

            <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:Image ID="ImageLogin" runat="server" ImageUrl="~/Assets/Backgrounds/shieldsso.png" Style="width: 450px; height: 500px; position: absolute; z-index: 5;" />
                    <%--<div class="login-box-body" style="background-color: white; border-radius:5px; box-shadow: 0px 0px 4px #ededed;">--%>

                    <div class="login-area" style="background-color: transparent; border-radius: 5px; box-shadow: 0px 0px 4px transparent; position: absolute; z-index: 10;">

                        <div class="" style="color: Blue; font-weight: bold; text-align: center; margin-top: 15px;">
                            <img src="<%= Page.ResolveClientUrl("~/Assets/Icons/ic_LogoWeb.png") %>" width="50px" />
                            <br />
                            <img src="<%= Page.ResolveClientUrl("~/Assets/Icons/ic_LogoText.png") %>" width="40%" />
                            <br />
                        </div>

                        <div style="text-align: left; color: black; font-size: 11px; padding-left: 75px; margin-top: 10px;">Username</div>
                        <div class="FormTextLogin form-inline" style="margin-left: auto; margin-right: auto;">
                            <i class="fa fa-user" style="margin-left: 10px; margin-top: 5px; font-size: 14px; width: 12px; color: #171717;"></i>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="FormTextDelBox" onkeypress="return isUserName(event)" placeholder="Username"></asp:TextBox>
                        </div>

                        <div style="text-align: left; color: black; font-size: 11px; padding-left: 84px; margin-top: 10px;">Password</div>
                        <div class="FormTextLogin form-inline" style="margin-left: auto; margin-right: auto; width: 285px;">
                            <i class="fa fa-lock" style="margin-left: 10px; margin-top: 5px; font-size: 17px; width: 12px; color: #171717;"></i>
                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="FormTextDelBox" placeholder="Password"></asp:TextBox>
                            <a href="#" style="text-decoration: none; color: #171717;" onclick="Toggle()"><i class="fa fa-eye-slash mata"></i></a>
                        </div>

                        <div style="text-align: center; margin-top: 5px; margin-right: auto; margin-left: auto; font-size: 12px;">
                            <p style="text-align: right; color: red; display: none;" id="pError" runat="server">
                            </p>
                        </div>

                        <table border="0" style="width: 300px; margin-left: 69px;">
                            <tr>
                                <td style="text-align: left;">
                                    <small style="color: #171717; display: none;">
                                        <asp:CheckBox ID="CheckBoxLoginAD" runat="server" OnCheckedChanged="CheckBoxLoginAD_CheckedChanged" AutoPostBack="true" Style="vertical-align: middle;" />
                                        Login by Active Directory </small>
                                </td>
                                <td style="text-align: right;">
                                    <small><a href="#modalForgotPass" data-toggle="modal" style="text-align: right; color: green;">Forgot Password?</a></small>
                                </td>
                            </tr>
                        </table>

                        <br />

                        <div style="text-align: center;">
                            <asp:Button ID="btnSignIn" OnClientClick="return CheckField();" runat="server" CssClass="MenuBox btn btn-success btn-flat btn-outline hidden" Style="width:265px;" Text="Sign In" Font-Bold="true" OnClick="btnSignIn_Click" />
                            <asp:ImageButton ID="ImageButtonSignIn" ImageUrl="~/Assets/Backgrounds/tombolA.png" onmouseover="this.src='../Assets/Backgrounds/tombolB.png'" onmouseout="this.src='../Assets/Backgrounds/tombolA.png'" style="width:200px; outline-width:0px !important;" runat="server" onclientclick="return CheckField();" OnClick="btnSignIn_Click" />

                            <asp:Button ID="ButtonRedirect" runat="server" Text="Redirect" OnClick="ButtonRedirect_Click" style="display:none;"/>
                        </div>
                        <br />

                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSignIn" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

        </div>

        <!--###########################################################################################################################################-->
        <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

        <!-- ##### Modal Edit App ##### -->
        <div class="modal fade" id="modalForgotPass" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 100px;" data-keyboard="false">

            <div class="modal-dialog" style="width: 500px">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">×</button>
                        <h4 class="modal-title">
                            <label>Forgot Password</label>
                        </h4>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">

                        <!-- update panel ini berfungsi untuk menjaga item didalamnya tidak terrefresh oleh postback dari luar update panel ini -->
                        <asp:UpdatePanel ID="UpdatePanelForgot" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="text-align: justify">
                                    Silakan masukkan username dan email lalu klik submit, setelah itu password baru akan dikirim melalui email yang terdaftar atas username.<br />
                                    <br />
                                </div>
                                <div class="form-group">
                                    Username
                                <asp:TextBox ID="ForgotUsername" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..."></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    Email
                                <asp:TextBox ID="ForgotEmail" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..."></asp:TextBox>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <br />

                        <table border="0" style="width: 100%">
                            <tr>
                                <td>
                                    <!-- loading gif untuk menunggu respon proses pada akses ke server side pada update panel tertunjuk -->
                                    <asp:UpdateProgress ID="uProgForgot" style="float: left;" runat="server" AssociatedUpdatePanelID="UpdatePanelForgot">
                                        <ProgressTemplate>
                                            <img alt="" height="25px" width="25px" style="background-color: transparent; vertical-align: middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                    <!-- dipasangi update panel agar element didalamnya dapat diupdate dari server side dan tidak terrefresh oleh postback -->
                                    <asp:UpdatePanel ID="UpdatePanelNotif" runat="server">
                                        <ContentTemplate>
                                            <b>
                                                <p style="color: red; display: none" id="p_Forgot" runat="server"></p>
                                            </b>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 30%; text-align: right;">
                                    <%--<asp:UpdatePanel ID="UpdatePanelSubmit" runat="server"> <ContentTemplate>--%>
                                    <asp:Button ID="Forgot_ButtonSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" Style="outline: 0; color: white; width: 150px;" OnClientClick="return FormCheck()" OnClick="Forgot_ButtonSubmit_Click"></asp:Button>
                                    <%--</ContentTemplate> </asp:UpdatePanel>--%>

                                    <%--update progress yang berbentuk modal hanya untuk button save--%>
                                    <%-- <asp:UpdateProgress ID="ForgotuProgSAVE" runat="server" AssociatedUpdatePanelID="UpdatePanelSubmit">
                                        <ProgressTemplate>
                                            <div class="modal-backdrop" style="background-color:black; opacity:0.4; text-align:center">
                                                <img alt="" height="200px" width="200px" style="background-color:transparent; vertical-align:middle; margin-top:120px;" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>--%>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
            </div>
        </div>
        <!-- End of Modal Edit App -->

        <!-- ##### Modal Change Pass ##### -->
        <div class="modal fade" id="modalChangePass" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 100px;" data-keyboard="false">

            <div class="modal-dialog" style="width: 550px">
                <div class="modal-content" style="border-radius: 6px;">

                    <!-- Modal Header -->
                    <div class="modal-header" style="padding: 8px;">
                        <button type="button" class="close" data-dismiss="modal">×</button>
                        <div>
                            <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                            <b>
                            <asp:Label ID="LabelChangePassTitle" runat="server" Text="-"></asp:Label>
                            </b>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                
                                <div style="text-align:center;">
                                    <iframe name="iframechangepass" id="iframechangepass" runat="server" style="width: 500px; height: 300px; border: none; overflow-y: auto;"></iframe>
                                </div>
                                <div style="text-align: right; display:none;">
                                    <button class="btn btn-success btn-sm" style="width: 100px;" data-dismiss="modal">Ok </button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <!-- End of Modal Forgot Pass -->

    </form>
</body>
</html>
