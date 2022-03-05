<%@ Page Title="Organization" Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="OrganizationMaster.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.OrganizationMaster" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Script ----------------------------------------------------------------->

    <script type="text/javascript">

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal edit
        function EditDetails(orgid, orgname, orghopeid, orgmobileid, orgaxid, createby, createdate, active) {
            $('#modalEditOrg').modal('show');

            var Edit_hidden_orgid = document.getElementById('<%= Edit_HiddenOrgID.ClientID %>');
            var Edit_hidden_Text_orgname = document.getElementById('<%= Edit_hiddenTextOrgname.ClientID %>');

            var Edit_Text_orgname = document.getElementById('<%= Edit_TextOrgname.ClientID %>');
            var Edit_ddl_hopeid = document.getElementById('<%= Edit_DDLHopeID.ClientID %>');
            var Edit_ddl_mobileid = document.getElementById('<%= Edit_DDLMobileID.ClientID %>');
            var Edit_ddl_axid = document.getElementById('<%= Edit_DDLAxID.ClientID %>');

            var Edit_hidden_hopeid = document.getElementById('<%= Edit_HiddenOrgHopeID.ClientID %>');
            var Edit_hidden_mobileid = document.getElementById('<%= Edit_HiddenOrgMobileID.ClientID %>');
            var Edit_hidden_axid = document.getElementById('<%= Edit_HiddenOrgAxID.ClientID %>');
            var Edit_hidden_active = document.getElementById('<%= Edit_HiddenIsActive.ClientID %>');
            var Edit_hidden_createby = document.getElementById('<%= Edit_HiddenCreateby.ClientID %>');
            var Edit_hidden_createdate = document.getElementById('<%= Edit_HiddenCreatedate.ClientID %>');

            Edit_hidden_orgid.value = orgid;
            Edit_hidden_Text_orgname.value = orgname;

            Edit_Text_orgname.value = orgname;
            Edit_ddl_hopeid.value = orghopeid; 
            Edit_ddl_mobileid.value = orgmobileid;
            Edit_ddl_axid.value = orgaxid;

            Edit_hidden_hopeid.value = orghopeid; 
            Edit_hidden_mobileid.value = orgmobileid;
            Edit_hidden_axid.value = orgaxid;
            Edit_hidden_active.value = active;
            Edit_hidden_createby.value = createby;
            Edit_hidden_createdate.value = createdate;

            //fungsi untuk mereplace karakter underscore(/_) menjadi spasi secara global(/g)
            Edit_hidden_Text_orgname.value = Edit_hidden_Text_orgname.value.toString().replace(/_/g, " ");
            Edit_Text_orgname.value = Edit_Text_orgname.value.toString().replace(/_/g, " ");
            Edit_hidden_createdate.value = Edit_hidden_createdate.value.toString().replace(/_/g, " ");
            Edit_hidden_createby.value = Edit_hidden_createby.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            //fungsi mempertahankan style dropdown search
            $('.selectpicker').selectpicker('refresh');

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal update status tanpa menampilkan modal
        function UpdateStatus(orgid, orgname, orghopeid, orgmobileid, orgaxid, createby, createdate, active) {     

            var hidden_orgid_s = document.getElementById('<%= HideOrgID.ClientID %>');
            var hidden_orgname_s = document.getElementById('<%= HideOrgName.ClientID %>');
            var hidden_hopeid_s = document.getElementById('<%= HideOrgHopeID.ClientID %>');
            var hidden_mobileid_s = document.getElementById('<%= HideOrgMobileID.ClientID %>');
            var hidden_axid_s = document.getElementById('<%= HideOrgAxID.ClientID %>');

            var label_active_s = document.getElementById('<%= LabelStatusActive.ClientID %>');
            var label_orgname_s = document.getElementById('<%= LblOrgName.ClientID %>');

            var hidden_active_s = document.getElementById('<%= HideOrgActive.ClientID %>');
            var hidden_createby_s = document.getElementById('<%= HideCreatby.ClientID %>');
            var hidden_createdate_s = document.getElementById('<%= HideCreatdate.ClientID %>');

            hidden_orgid_s.value = orgid;
            hidden_orgname_s.value = orgname;
            hidden_hopeid_s.value = orghopeid;
            hidden_mobileid_s.value = orgmobileid;
            hidden_axid_s.value = orgaxid;

            label_orgname_s.innerHTML = orgname;
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
            hidden_orgname_s.value = hidden_orgname_s.value.toString().replace(/_/g, " ");
            hidden_createdate_s.value = hidden_createdate_s.value.toString().replace(/_/g, " ");
            label_orgname_s.innerHTML = label_orgname_s.innerHTML.toString().replace(/_/g, " ");
            hidden_createby_s.value = hidden_createby_s.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            //fungsi klik otomatis button update pada server side
            document.getElementById('<%= ButtonChangeStatus.ClientID %>').click();

            return false;
        }

        //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
        function AddFormCheck()
        {
            if ($("[id$='Add_TextOrgname']").val().length == 0)
            {
                $("[id$='Add_TextOrgname']").focus();
                $("[id$='p_Add']").removeAttr("style");
                $("[id$='p_Add']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Organization Name cannot be empty!";

                return false;
            }
        }

        //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
        function EditFormCheck()
        {
            if ($("[id$='Edit_TextOrgname']").val().length == 0)
            {
                $("[id$='Edit_TextOrgname']").focus();
                $("[id$='p_Edit']").removeAttr("style");
                $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Organization Name cannot be empty!";

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

            document.getElementById('<%= Add_TextOrgname.ClientID %>').value = "";

            document.getElementById('<%= Add_DDLHopeID.ClientID %>').value = 0;
            document.getElementById('<%= Add_DDLMobileID.ClientID %>').value = "";
            document.getElementById('<%= Add_DDLAxID.ClientID %>').value = "";

            //fungsi mempertahankan style dropdown search
            $('.selectpicker').selectpicker('refresh');
        }

        //inisialisasi bootstrap switch dan //fungsi untuk menjaga dropdown search
        function PageLoad() {
            $('.CheckBoxSwitch').bootstrapToggle();
            $('.selectpicker').selectpicker(); 
        }

        //fungsi event klik pada area diluar modal
        $(document).ready(function () { 
            $('#modalEditOrg').on('hidden.bs.modal', function (e) {
                 resetModalForm();
             });

            $('#modalAddOrg').on('hidden.bs.modal', function (e) {
                 resetModalForm();
            });

            //fungsi memepertahankan style bootstrap switch saat postback dalam update panel
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        $('.CheckBoxSwitch').bootstrapToggle();
                    }
                });
            };
        });    

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
                <div class="col-sm-6 TeksHeader" style="padding-top: 5px;"><b style="float:left">Manage Organization </b>

                    <%--update progress untuk menunggu loading pindah page--%>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePaneDataOrg">
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
                        <button type="button" class="TombolAddData btn btn-primary TeksNormal" data-toggle="modal" data-target="#modalAddOrg"><i class="fa fa-plus"></i>&nbsp; Add Organization</button>
                    </div>

                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePaneDataOrg" runat="server" UpdateMode="Always"> <ContentTemplate>

            <asp:GridView ID="GridViewOrganization" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover TeksNormal"
                AllowPaging="True" PageSize="9" OnPageIndexChanging="GridViewOrganization_PageIndexChanging" ShowHeaderWhenEmpty="True" 
                DataKeyNames="organization_id" EmptyDataText="No Data" BorderWidth="0">
                <PagerStyle CssClass="pagination-ys" />

                <Columns>      
                    <asp:BoundField HeaderText="Organization Name" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" DataField="organization_name" SortExpression="organization_name" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="HOPE ID" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left" DataField="hope_organization_id" SortExpression="hope_organization_id" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="MOBILE ID" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" DataField="mobile_organization_id" SortExpression="mobile_organization_id" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="AX ID" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left" DataField="ax_organization_id" SortExpression="ax_organization_id" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>

                    <asp:TemplateField HeaderText="Is Active" ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <input ID="CheckBoxStatus" class="CheckBoxSwitch" type="checkbox" <%# Eval("is_active").ToString() == "True" ? "checked" : "unchecked"%> data-toggle="toggle" data-on="Active" data-off="Inactive" data-onstyle="success" data-offstyle="default" data-size="mini" onchange=<%# "UpdateStatus('" + Eval("organization_id") + "','" + Eval("organization_name").ToString().Replace(" ","_")  + "','" + Eval("hope_organization_id") + "','" + Eval("mobile_organization_id") + "','" + Eval("ax_organization_id")  + "','" + Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <a href="#" style="text-decoration: none;" onclick=<%# "EditDetails('" + Eval("organization_id") + "','" + Eval("organization_name").ToString().Replace(" ","_")  + "','" + Eval("hope_organization_id") + "','" + Eval("mobile_organization_id") + "','" + Eval("ax_organization_id")  + "','" + Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>> <asp:Image ID="ImageEdit" ImageUrl="~/Assets/Icons/ic_Edit.svg" CssClass="ic_Edit" runat="server" /></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

            </ContentTemplate> </asp:UpdatePanel>

        </div>
    </div>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

    <!-- ##### Modal Edit Org ##### -->
    <div class="modal fade" id="modalEditOrg" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Edit Organization</label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel ini berfungsi untuk menjaga item didalamnya tidak terrefresh oleh postback dari luar update panel ini --> 
                    <asp:UpdatePanel ID="UpdatePanelEDITorg" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <asp:HiddenField ID="Edit_HiddenOrgID" runat="server" />
                    <div class="form-group">
                        Organization Name
                            <!-- set autopostback=true agar event di server side bisa diexecute dan hanya akan berlaku didalam update panel ini -->
                            <asp:TextBox ID="Edit_TextOrgname" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..."></asp:TextBox>
                            <asp:HiddenField ID="Edit_hiddenTextOrgname" runat="server" />
                    </div>
                    <div class="form-group">
                        HOPE ID 
                            <asp:HiddenField ID="Edit_HiddenOrgHopeID" runat="server" />
                            <asp:DropDownList ID="Edit_DDLHopeID" runat="server" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        MOBILE ID 
                            <asp:HiddenField ID="Edit_HiddenOrgMobileID" runat="server" />
                            <asp:DropDownList ID="Edit_DDLMobileID" runat="server" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        AX ID 
                            <asp:HiddenField ID="Edit_HiddenOrgAxID" runat="server" />
                            <asp:DropDownList ID="Edit_DDLAxID" runat="server" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false"></asp:DropDownList>
                    </div>
                    <asp:HiddenField ID="Edit_HiddenCreateby" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenCreatedate" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenIsActive" runat="server" />
                    <br />

                     <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <!-- loading gif untuk menunggu respon proses pada akses ke server side pada update panel tertunjuk -->
                                <asp:UpdateProgress ID="uProgOrgnameEdit" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelEDITorg">
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
                                    <asp:Button ID="Edit_ButtonSaveOrg" runat="server" Text="Save & Close" class="btn btn-success" OnClientClick="return EditFormCheck()" OnClick="Edit_ButtonSaveOrg_Click"></asp:Button>
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
    <!-- End of Modal Edit Org -->

    <!-- ##### Modal Add Org ##### -->
    <div class="modal fade" id="modalAddOrg" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Add Organization</label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanelADDorg" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="form-group">
                        Organization Name
                            <asp:TextBox ID="Add_TextOrgname" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..." ></asp:TextBox>
                    </div>
                   <div class="form-group">
                        HOPE ID 
                            <asp:HiddenField ID="Add_HiddenOrgHopeID" runat="server" />
                            <asp:DropDownList ID="Add_DDLHopeID" runat="server" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        MOBILE ID 
                            <asp:HiddenField ID="Add_HiddenOrgMobileID" runat="server" />
                            <asp:DropDownList ID="Add_DDLMobileID" runat="server" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false" ></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        AX ID 
                            <asp:HiddenField ID="Add_HiddenOrgAxID" runat="server" />
                            <asp:DropDownList ID="Add_DDLAxID" runat="server" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false" ></asp:DropDownList>
                    </div>
                    <br />

                     <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <asp:UpdateProgress ID="uProgOrgnameAdd" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelADDorg">
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
                                    <asp:Button ID="Add_ButtonSaveOrg" runat="server" Text="Save & Close" class="btn btn-success" OnClientClick="return AddFormCheck()" OnClick="Add_ButtonSaveOrg_Click"></asp:Button>
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
                    </table>
                    
                    </ContentTemplate> </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Add Org -->

    <!-- ##### Modal Update Status Org ##### -->
    <div class="modal fade" id="modalUpdateStatus" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;">

        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="javascript:window.location.reload()">×</button> 
                    <h4 class="modal-title">
                        <label>Update Status <asp:Label ID="LblOrgName" runat="server" Text="Label"></asp:Label> </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel dipasang agar saat button update status diklik via javascript, page tidak terefresh -->
                    <asp:UpdatePanel ID="UpdatePanelSTATUSorg" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="text-center">
                        <asp:HiddenField ID="HideOrgID" runat="server" />
                        <asp:HiddenField ID="HideOrgActive" runat="server" />
 
                        Are you sure to update the status to <b> <asp:Label ID="LabelStatusActive" runat="server" Text="Label"></asp:Label> </b> ? <br /><br />

                        <asp:HiddenField ID="HideOrgName" runat="server" />
                        <asp:HiddenField ID="HideOrgHopeID" runat="server" />
                        <asp:HiddenField ID="HideOrgMobileID" runat="server" />
                        <asp:HiddenField ID="HideOrgAxID" runat="server" />
                        <asp:HiddenField ID="HideCreatby" runat="server" />
                        <asp:HiddenField ID="HideCreatdate" runat="server" />

                        <asp:Button ID="ButtonChangeStatus" runat="server" Text="Save & Close" class="btn btn-success" OnClick="ButtonChangeStatus_Click"></asp:Button>
                    </div>

                    </ContentTemplate> </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Update Status Org -->

</asp:Content>