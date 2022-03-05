<%@ Page Title="Application" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ApplicationMaster.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.ApplicationMaster" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Script ----------------------------------------------------------------->

    <script type="text/javascript">

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal edit
        function EditDetails(appid, appname, url, createby, createdate, active) {
            $('#modalEditApp').modal('show');

            var Edit_hidden_appid = document.getElementById('<%= Edit_HiddenAppID.ClientID %>');
            var Edit_hidden_Text_appname = document.getElementById('<%= Edit_HiddenTextAppname.ClientID %>');

            var Edit_Text_appname = document.getElementById('<%= Edit_TextAppname.ClientID %>');
            var Edit_Text_Url = document.getElementById('<%= Edit_TextUrl.ClientID %>');

            var Edit_hidden_active = document.getElementById('<%= Edit_HiddenIsActive.ClientID %>');
            var Edit_hidden_createby = document.getElementById('<%= Edit_HiddenCreateby.ClientID %>');
            var Edit_hidden_createdate = document.getElementById('<%= Edit_HiddenCreatedate.ClientID %>');

            Edit_hidden_appid.value = appid;
            Edit_hidden_Text_appname.value = appname;

            Edit_Text_appname.value = appname;
            Edit_Text_Url.value = url;

            Edit_hidden_active.value = active;
            Edit_hidden_createby.value = createby;
            Edit_hidden_createdate.value = createdate;

            //fungsi untuk mereplace karakter underscore(/_) menjadi spasi secara global(/g)
            Edit_hidden_Text_appname.value = Edit_hidden_Text_appname.value.toString().replace(/_/g, " ");
            Edit_Text_appname.value = Edit_Text_appname.value.toString().replace(/_/g, " ");
            Edit_hidden_createdate.value = Edit_hidden_createdate.value.toString().replace(/_/g, " ");
            Edit_hidden_createby.value = Edit_hidden_createby.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal update status tanpa menampilkan modal
        function UpdateStatus(appid, appname, url, createby, createdate, active) {     

            var hidden_appid_s = document.getElementById('<%= HideAppID.ClientID %>');
            var hidden_appname_s = document.getElementById('<%= HideAppName.ClientID %>');
            var hidden_Url_s = document.getElementById('<%= HideUrl.ClientID %>');

            var label_active_s = document.getElementById('<%= LabelStatusActive.ClientID %>');
            var label_appname_s = document.getElementById('<%= LblAppName.ClientID %>');

            var hidden_active_s = document.getElementById('<%= HideAppActive.ClientID %>');
            var hidden_createby_s = document.getElementById('<%= HideCreatby.ClientID %>');
            var hidden_createdate_s = document.getElementById('<%= HideCreatdate.ClientID %>');

            hidden_appid_s.value = appid;
            hidden_appname_s.value = appname;
            hidden_Url_s.value = url;

            label_appname_s.innerHTML = appname;
            if (active == "True") {
                label_active_s.innerHTML = "Inactive";
            }
            else {
                label_active_s.innerHTML = "Active";
            }

            hidden_active_s.value = active;
            hidden_createby_s.value = createby;
            hidden_createdate_s.value = createdate;

            //fungsi untuk mereplace karakter underscore(/_) menjadi spasi secara global(/g)
            hidden_appname_s.value = hidden_appname_s.value.toString().replace(/_/g, " ");
            hidden_createdate_s.value = hidden_createdate_s.value.toString().replace(/_/g, " ");
            label_appname_s.innerHTML = label_appname_s.innerHTML.toString().replace(/_/g, " ");
            hidden_createby_s.value = hidden_createby_s.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            //fungsi klik otomatis button update pada server side
            document.getElementById('<%= ButtonChangeStatus.ClientID %>').click();

            return false;
        }

        //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
        function AddFormCheck()
        {
            if ($("[id$='Add_TextAppname']").val().length == 0) {
                $("[id$='Add_TextAppname']").focus();
                $("[id$='p_Add']").removeAttr("style");
                $("[id$='p_Add']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Application Name cannot be empty!";

                return false;
            }
        }

        //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
        function EditFormCheck()
        {
            if ($("[id$='Edit_TextAppname']").val().length == 0) {
                $("[id$='Edit_TextAppname']").focus();
                $("[id$='p_Edit']").removeAttr("style");
                $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Application Name cannot be empty!";

                return false;
            }
        }

        //fungsi untuk me reset form input dan notifikasi menjadi kosong kembali
        function resetModalForm()
        {
            $("[id$='p_Add']").removeAttr("style");
            document.getElementById('<%= p_Add.ClientID %>').innerHTML = "";
            $("[id$='p_Edit']").removeAttr("style");
            document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "";

            document.getElementById('<%= Add_TextAppname.ClientID %>').value = "";
            document.getElementById('<%= Add_TextUrl.ClientID %>').value = "";
        }

        //fungsi event klik pada area diluar modal
        $(document).ready(function () { 
            $('#modalEditApp').on('hidden.bs.modal', function (e) {
                 resetModalForm();
             });

            $('#modalAddApp').on('hidden.bs.modal', function (e) {
                 resetModalForm();
            });

            //fungsi untuk mempertahankan style saat postback di updatepanel
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        $('.CheckBoxSwitch').bootstrapToggle();
                    }
                });
            };
        });

        //inisialisasi bootstrap switch
        function PageLoad() {
            $('.CheckBoxSwitch').bootstrapToggle();
        }

        //fungsi untuk menonaktifkan tombol Enter
        $(document).keypress(
        function(event){
            if (event.which == '13') {
              event.preventDefault();
            }
        });

        //fungsi untuk klik button search by enter button
        function cariData(evt)
        {
            evt = (evt) ? evt : window.event;
            var key = evt.keyCode || evt.charCode;
            if (key == 13)
            {
                document.getElementById('<%= ButtonCari.ClientID %>').click();
            }

            if (document.getElementById('<%= Search_masterData.ClientID %>').value == "")
            {
                document.getElementById('<%= ButtonCari.ClientID %>').click();
            }
        }

        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }

    </script>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Content ---------------------------------------------------------------->

    <div class="row">      
        <div class="col-sm-12 Contentutama">

            <div class="row borderTitle" style="padding-top: 10px; padding-bottom: 10px">
                <div class="col-sm-6 TeksHeader" style="padding-top: 5px;"><b style="float:left">Manage Application </b>

                    <%--update progress untuk menunggu loading pindah page--%>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePaneDataApp">
                        <ProgressTemplate>
                                <img alt="" height="25px" width="25px" style="background-color:transparent; vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanelCari">
                        <ProgressTemplate>
                                <img alt="" height="25px" width="25px" style="background-color:transparent; vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                </div>
                <div class="col-sm-6" style="text-align:right;">

                    <div class="form-inline" style="display:inline-flex;">
                        <div class="has-feedback" style="text-align:right; margin-right:10px;">
                            <asp:TextBox ID="Search_masterData" name="Search_masterData" runat="server" CssClass="searchBoxAnimation form-control" onkeyup="cariData(event)" placeholder="Search..."  AutoCompleteType="Disabled"></asp:TextBox>
                            <span class="fa fa-search form-control-feedback searchIconAnimation"></span>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanelCari" runat="server"> <ContentTemplate>
                            <div hidden> <asp:Button ID="ButtonCari" runat="server" Text="Button" OnClick="ButtonCari_Click" /> </div>
                            <asp:HiddenField ID="HiddenFlagCari" runat="server" />
                        </ContentTemplate> </asp:UpdatePanel>
                        <button type="button" class="TombolAddData btn btn-primary TeksNormal" data-toggle="modal" data-target="#modalAddApp"><i class="fa fa-plus"></i>&nbsp; Add Application</button>
                    </div>

                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePaneDataApp" runat="server" UpdateMode="Always"> 
            <ContentTemplate>

            <asp:GridView ID="GridViewApplication" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover TeksNormal"
                AllowPaging="True" PageSize="9" OnPageIndexChanging="GridViewApplication_PageIndexChanging" ShowHeaderWhenEmpty="True"
                DataKeyNames="application_id" EmptyDataText="No Data" BorderWidth="0">
                <PagerStyle CssClass="pagination-ys" />

                <Columns>      
                    <asp:BoundField HeaderText="Application Name" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" DataField="application_name" SortExpression="application_name" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="Application ID" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" DataField="application_id" SortExpression="application_id" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="Application URL" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" DataField="url" SortExpression="url" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>

                    <asp:TemplateField HeaderText="Is Active" ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <input ID="CheckBoxStatus" class="CheckBoxSwitch" type="checkbox" <%# Eval("is_active").ToString() == "True" ? "checked" : "unchecked"%> data-toggle="toggle" data-on="Active" data-off="Inactive" data-onstyle="success" data-offstyle="default" data-size="mini" onchange=<%# "UpdateStatus('" + Eval("application_id") + "','" + Eval("application_name").ToString().Replace(" ","_")  + "','" + Eval("url")  + "','" + Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <a href="#" style="text-decoration: none;" onclick=<%# "EditDetails('" + Eval("application_id") + "','" + Eval("application_name").ToString().Replace(" ","_")  + "','" + Eval("url")  + "','" + Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>> <asp:Image ID="ImageEdit" ImageUrl="~/Assets/Icons/ic_Edit.svg" CssClass="ic_Edit" runat="server" /></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

            </ContentTemplate> </asp:UpdatePanel>

        </div>
    </div>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

    <!-- ##### Modal Edit App ##### -->
    <div class="modal fade" id="modalEditApp" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Edit Application</label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel ini berfungsi untuk menjaga item didalamnya tidak terrefresh oleh postback dari luar update panel ini --> 
                    <asp:UpdatePanel ID="UpdatePanelEDITapp" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <asp:HiddenField ID="Edit_HiddenAppID" runat="server" />
                    <div class="form-group">
                        Application Name
                            <asp:TextBox ID="Edit_TextAppname" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..."></asp:TextBox>
                            <%--untuk menampung data awal appname yg sebelum di edit--%>
                            <asp:HiddenField ID="Edit_HiddenTextAppname" runat="server" />
                    </div>
                    <div class="form-group">
                        Url 
                            <asp:TextBox ID="Edit_TextUrl" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..."></asp:TextBox>
                    </div>
                    <asp:HiddenField ID="Edit_HiddenCreateby" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenCreatedate" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenIsActive" runat="server" />
                    <br />

                    <table border="0" style="width:100%">
                        <tr>
                            <td>
                                <!-- loading gif untuk menunggu respon proses pada akses ke server side pada update panel tertunjuk -->
                                <asp:UpdateProgress ID="uProgAppnameEdit" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelEDITapp">
                                    <ProgressTemplate>
                                        <img alt="" height="25px" width="25px" style="background-color:transparent; vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                                <!-- dipasangi update panel agar element didalamnya dapat diupdate dari server side dan tidak terrefresh oleh postback -->
                                <asp:UpdatePanel ID="UpdatePanelExistEdit" runat="server"> <ContentTemplate>
                                <b> <p style="color: red; display: none" id="p_Edit" runat="server"> </p> </b> 
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                            <td style="width:30%; text-align:right;"> 
                                <asp:UpdatePanel ID="UpdatePanelSAVEedit" runat="server"> <ContentTemplate>
                                    <asp:Button ID="Edit_ButtonSaveApp" runat="server" Text="Save & Close" CssClass="btn btn-success" OnClientClick="return EditFormCheck()" OnClick="Edit_ButtonSaveApp_Click"></asp:Button>                  
                                </ContentTemplate> </asp:UpdatePanel>

                                <%--update progress yang berbentuk modal hanya untuk button save--%>
                                <asp:UpdateProgress ID="EdituProgSAVE" runat="server" AssociatedUpdatePanelID="UpdatePanelSAVEedit">
                                    <ProgressTemplate>
                                        <div class="modal-backdrop" style="background-color:black; opacity:0.4; text-align:center">
                                            <img alt="" height="200px" width="200px" style="background-color:transparent; vertical-align:middle; margin-top:120px;" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>

                    </ContentTemplate> </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Edit App -->

    <!-- ##### Modal Add App ##### -->
    <div class="modal fade" id="modalAddApp" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Add Application</label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanelADDapp" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="form-group">
                        Application Name
                            <asp:TextBox ID="Add_TextAppname" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..."></asp:TextBox>
                    </div>
                    <div class="form-group">
                        Url 
                            <asp:TextBox ID="Add_TextUrl" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..."></asp:TextBox>
                    </div>
                    <br />
                    
                     <t able border="0" style="width:100%">
                        <tr>
                            <td> 
                                <asp:UpdateProgress ID="uProgAppnameAdd" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelADDapp">
                                    <ProgressTemplate>
                                        <img alt="" height="25px" width="25px" style="background-color:transparent; vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                                <asp:UpdatePanel ID="UpdatePanelExistAdd" runat="server"> <ContentTemplate>
                                <b> <p style="color: red; display: none" id="p_Add" runat="server"> </p> </b> 
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                            <td style="width:30%; text-align:right;"> 
                                <asp:UpdatePanel ID="UpdatePanelSAVEadd" runat="server"> <ContentTemplate>
                                    <asp:Button ID="Add_ButtonSaveApp" runat="server" Text="Save & Close" CssClass="btn btn-success" OnClientClick="return AddFormCheck()" OnClick="Add_ButtonSaveApp_Click"></asp:Button> 
                                </ContentTemplate> </asp:UpdatePanel>

                                <asp:UpdateProgress ID="AdduProgSAVE" runat="server" AssociatedUpdatePanelID="UpdatePanelSAVEadd">
                                    <ProgressTemplate>
                                        <div class="modal-backdrop" style="background-color:black; opacity:0.4; text-align:center">
                                            <img alt="" height="200px" width="200px" style="background-color:transparent; vertical-align:middle; margin-top:120px;" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                       </tr>
                    </t>

                    </ContentTemplate> </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Add App -->

    <!-- ##### Modal Update Status App ##### -->
    <div class="modal fade" id="modalUpdateStatus" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;">
       
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="javascript:window.location.reload()">×</button> 
                    <h4 class="modal-title">
                        <label>Update Status <asp:Label ID="LblAppName" runat="server" Text="Label"></asp:Label> </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel dipasang agar saat button update status diklik via javascript, page tidak terefresh -->
                    <asp:UpdatePanel ID="UpdatePanelSTATUSapp" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="text-center">
                        <asp:HiddenField ID="HideAppID" runat="server" />
                        <asp:HiddenField ID="HideAppActive" runat="server" />
 
                        Are you sure to update the status to <b> <asp:Label ID="LabelStatusActive" runat="server" Text="Label"></asp:Label> </b> ? <br /><br />

                        <asp:HiddenField ID="HideAppName" runat="server" />
                        <asp:HiddenField ID="HideUrl" runat="server" />
                        <asp:HiddenField ID="HideCreatby" runat="server" />
                        <asp:HiddenField ID="HideCreatdate" runat="server" />

                        <asp:Button ID="ButtonChangeStatus" runat="server" Text="Save & Close" class="btn btn-success" OnClick="ButtonChangeStatus_Click"></asp:Button>
                    </div>

                    </ContentTemplate> </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Update Status App --> 

</asp:Content>
