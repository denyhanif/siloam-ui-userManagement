<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.ChangePassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Script ----------------------------------------------------------------->

    <script type="text/javascript">
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

        //fungsi event klik pada area diluar modal
        $(document).ready(function () { 
            $('#modalAfterSave').on('hidden.bs.modal', function (e) {
                 document.getElementById('<%= ButtonRelogin.ClientID %>').click();
             });
        });

        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }
    </script>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Content ---------------------------------------------------------------->

    <div class="row">
        <div class="col-sm-4">
        </div>

        <div class="col-sm-4 Contentutama">
            <div class="row borderTitle" style="padding-top: 10px; padding-bottom: 10px">
                <div class="col-lg-12 TeksHeader" style="padding-top: 5px;"><b> Change Password </b> </div>
            </div>

            <br />

            <div class="form-group">
                Old password
                    <asp:TextBox ID="Pass_TextOldPass" runat="server" CssClass="MaxWidthTextbox form-control" TextMode="Password" placeholder="Old password..."></asp:TextBox>
            </div>
            <div class="form-group">
                New password
                    <asp:TextBox ID="Pass_TextNewPass" runat="server" CssClass="MaxWidthTextbox form-control" TextMode="Password" placeholder="New password..."></asp:TextBox>
            </div>
            <div class="form-group">
                Confirm new password
                    <asp:TextBox ID="Pass_TextNewPass_confirm" runat="server" CssClass="MaxWidthTextbox form-control" TextMode="Password" placeholder="Confirm new password..."></asp:TextBox>
            </div>

            <table border="0" style="width:100%">
                <tr>
                    <td> 
                        <asp:UpdatePanel ID="UpdatePanelCekPass" runat="server"> <ContentTemplate>
                        <b> <p style="color: red; display: none" id="p_Add" runat="server"> </p> </b> 
                        </ContentTemplate> </asp:UpdatePanel>
                    </td>
                    <td style="width:30%; text-align:right;">             
                        <asp:UpdatePanel ID="UpdatePanelSAVE" runat="server"> <ContentTemplate>
                        <asp:Button ID="Pass_ButtonSavePass" runat="server" Text="Change Password" CssClass="btn btn-success" OnClientClick="return FormCheck()" OnClick="Pass_ButtonSavePass_Click"></asp:Button>
                        </ContentTemplate> </asp:UpdatePanel>

                        <asp:UpdateProgress ID="PassuProgSAVE" runat="server" AssociatedUpdatePanelID="UpdatePanelSAVE">
                            <ProgressTemplate>
                                <div class="modal-backdrop" style="background-color:black; opacity:0.4; text-align:center">
                                    <img alt="" height="200px" width="200px" style="background-color:transparent; vertical-align:middle; margin-top:120px;" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                     </td>
                </tr>
            </table>

            <br />
        </div>

        <div class="col-sm-4">
        </div>
    </div>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

    <!-- ##### Modal Change Password ##### -->
    <div class="modal fade" id="modalAfterSave" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
       
        <asp:UpdatePanel ID="UpdatePanelAfterChangepass" runat="server" UpdateMode="Conditional"> <ContentTemplate>

        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">×</button> 
                    <h4 class="modal-title">
                        <label style="color:#4e9c36;"> <i class="fa fa-check"></i> Change Password Success  </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <div class="text-center">
                        Your Password Has Been Changed <br /> <b> Klik OK to Relogin. </b>
                        <br /><br />
                        <asp:Button ID="ButtonRelogin" runat="server" Text="OK" class="btn btn-success" Style="width:90px;" OnClick="ButtonRelogin_Click"/> 
                    </div>
                </div>
            </div>
        </div>
        </ContentTemplate> </asp:UpdatePanel>
    </div>
    <!-- End of Modal Change Password -->

</asp:Content>
