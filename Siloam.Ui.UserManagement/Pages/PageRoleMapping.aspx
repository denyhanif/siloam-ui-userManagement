<%@ Page Title="Page-Role" Language="C#" AutoEventWireup="true" CodeBehind="PageRoleMapping.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Siloam.Ui.UserManagement.Pages.PageRoleMapping" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Script ----------------------------------------------------------------->

    <script type="text/javascript">

        //fungsi engine pencarian
        $.expr[":"].containsNoCase = function (el, i, m) {  
        var search = m[3];  
        if (!search) return false;  
        return eval("/" + search + "/i").test($(el).text());  
        }; 

        //fungsi pencarian data di gridview client side - app search
        $(document).ready(function () {  
        $("[id$='Search_TextApp']").keyup(function () {  
            if ($("[id$='Search_TextApp']").val().length > 1) {  
                $("[id$='GridViewApplication'] tr").hide();  
                $("[id$='GridViewApplication'] tr:first").show();  
                $("[id$='GridViewApplication'] tr td:containsNoCase(\'" + $("[id$='Search_TextApp']").val() + '\')').parent().show();  
            }  
            else if ($("[id$='Search_TextApp']").val().length == 0) {  
                resetSearchValue();  
            }  
  
            if ($("[id$='GridViewApplication'] tr:visible").length == 1) {  
                $('.norecords').remove();  
                $("[id$='GridViewApplication']").append('<tr class="norecords"><td colspan="6" class="Normal" style="text-align: center">No records were found</td></tr>');  
            }  
        });  

            $("[id$='Search_TextApp']").keyup(function (event) {  
            if (event.keyCode == 27) {  
                resetSearchValue();  
            }  
            });  
        }); 

        function resetSearchValue() {  
        $("[id$='Search_TextApp']").val('');  
        $("[id$='GridViewApplication'] tr").show();  
        $('.norecords').remove();  
        $("[id$='Search_TextApp']").focus();  
        } 

        //fungsi pencarian data di gridview client side - page search
        $(document).ready(function () {  
        $("[id$='Search_Page']").keyup(function () {  
            if ($("[id$='Search_Page']").val().length > 1) { 
                $('.header').show(); 
                $('td.headerlabel').parent('tr.headerRole').find("i.iconRole").removeClass("fa-plus-square");
                $('td.headerlabel').parent('tr.headerRole').find("i.iconRole").addClass("fa-minus-square");

                $("[id$='tbl_pagedetail'] tr").hide();  
                //$("[id$='tbl_pagedetail'] tr:first").show();  
                $("[id$='tbl_pagedetail'] tr td:containsNoCase(\'" + $("[id$='Search_Page']").val() + '\')').parent().show();  
            }  
            else if ($("[id$='Search_Page']").val().length == 0) { 
                $('.header').hide();
                $('td.headerlabel').parent('tr.headerRole').find("i.iconRole").removeClass("fa-minus-square");
                $('td.headerlabel').parent('tr.headerRole').find("i.iconRole").addClass("fa-plus-square");

                resetSearchValue();  
            }  
  
            if ($("[id$='tbl_pagedetail'] tr:visible").length == 1) {  
                $('.norecords').remove();  
                //$("[id$='tbl_pagedetail']").append('<tr class="norecords"><td colspan="6" class="Normal" style="text-align: center">No records were found</td></tr>');  
            }  
        });  

            $("[id$='Search_Page']").keyup(function (event) {  
            if (event.keyCode == 27) {  
                resetSearchValue();  
            }  
            });  
        }); 

        function resetSearchValue() {  
        $("[id$='Search_Page']").val('');  
        $("[id$='tbl_pagedetail'] tr").show();  
        $('.norecords').remove();  
        $("[id$='Search_Page']").focus();  
        } 

        //fungsi untuk hidden row table
        $(document).ready(function () {
            $('.header').hide();
        });

        //fungsi untuk toggle row table
        $(document).ready(function () {
            $('td.headerlabel').click(function () {
                $(this).closest('tr.headerRole').nextUntil('tr.headerRole').slideToggle(100);
                $(this).parent('tr.headerRole').find("i.iconRole").toggleClass("fa-plus-square fa-minus-square");
            });
        });

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal add data
        function AddMappingPage(appid, appname, roleid, rolename) {
            $('#modalAddMapPage').modal('show');

            var hidden_appid_add = document.getElementById('<%= HiddenAppID.ClientID %>');
            var hidden_roleid_add = document.getElementById('<%= HiddenRoleID.ClientID %>');

            var label_appname_add = document.getElementById('<%= Add_LabelAppName.ClientID %>');
            var label_rolename_add = document.getElementById('<%= Add_LabelRoleName.ClientID %>');

            hidden_appid_add.value = appid;
            hidden_roleid_add.value = roleid;

            label_appname_add.innerHTML = appname;
            label_rolename_add.innerHTML = rolename;

            label_appname_add.innerHTML = label_appname_add.innerHTML.toString().replace(/_/g, " ");
            label_rolename_add.innerHTML = label_rolename_add.innerHTML.toString().replace(/_/g, " ");

            //fungsi untuk menandai data yang sudah termapping
            document.getElementById('<%= ButtonDisableDDL.ClientID %>').click();

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal update status tanpa menampilkan modal
        function UpdateStatus(roleaccessid, roleid, pageid, createby, createdate, isactive) {

            var hidden_RA_id_s = document.getElementById('<%= HideRA_ID.ClientID %>');
            var hidden_RA_roleid_s = document.getElementById('<%= HideRA_roleID.ClientID %>');
            var hidden_RA_pageid_s = document.getElementById('<%= HideRA_pageID.ClientID %>');

            var label_active_s = document.getElementById('<%= LabelStatusActive.ClientID %>');

            var hidden_active_s = document.getElementById('<%= HideRA_Active.ClientID %>');
            var hidden_createby_s = document.getElementById('<%= HideCreatby.ClientID %>');
            var hidden_createdate_s = document.getElementById('<%= HideCreatdate.ClientID %>');

            hidden_RA_id_s.value = roleaccessid;
            hidden_RA_roleid_s.value = roleid;
            hidden_RA_pageid_s.value = pageid;

            if (isactive == "True") {
                label_active_s.innerHTML = "Inactive";
            }
            else {
                label_active_s.innerHTML = "Active";
            }

            hidden_active_s.value = isactive;
            hidden_createby_s.value = createby;
            hidden_createdate_s.value = createdate;

            hidden_createdate_s.value = hidden_createdate_s.value.toString().replace(/_/g, " ");
            hidden_createby_s.value = hidden_createby_s.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            //fungsi klik otomatis button update pada server side
            document.getElementById('<%= ButtonChangeStatus.ClientID %>').click();

            return false;
        }

        //fungsi  untuk validasi gridview kosong, lalu menampilkan notifikasi text berwarna merah
        function AddFormCheck()
        {
            //use preTag --MainContent-- if using --#-- on the element ID
            if ($('#MainContent_Add_GridViewPageMapList tr td').first()[0].innerHTML == "no data") {
                $("[id$='p_Add']").removeAttr("style");
                $("[id$='p_Add']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Please Add Some Data Before Save!";

                return false;
            }
            else {
                document.getElementById('<%= Add_ButtonSaveMapPage.ClientID %>').click();
            }
        }

        //fungsi untuk me reset form input dan notifikasi menjadi kosong kembali
        function resetModalForm()
        {
            var DDL_pageName = document.getElementById('<%= Add_DDLPagename.ClientID %>');
            DDL_pageName.value = "00000000-0000-0000-0000-000000000000";

            $("[id$='p_Add']").removeAttr("style");
            document.getElementById('<%= p_Add.ClientID %>').innerHTML = "";

            //fungsi untuk menjaga style dropdown search
            $('.selectpicker').selectpicker('refresh');
            document.getElementById('<%= ButtonClearList.ClientID %>').click();
        }

        //inisialisasi bootstrap switch dan //fungsi untuk menjaga dropdown search
        function PageLoad() {
            $('.CheckBoxSwitch').bootstrapToggle();
            $('.selectpicker').selectpicker(); 
        }

        //fungsi event klik pada area diluar modal
        $(document).ready(function () { 
            $('#modalAddMapPage').on('hidden.bs.modal', function (e) {
                resetModalForm();
            });

            setAppScroll();

            //fungsi untuk menjaga style pada saat postback dalam updatepanel
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        $('.CheckBoxSwitch').bootstrapToggle();
                        $('.selectpicker').selectpicker(); 
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

        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }

        function getAppScroll(pos) {
            var position = pos.scrollTop;
            $("[id$='HiddenAppScroll']").val(position);
            //console.log("get :" + position);
        }

        function setAppScroll() {
            var position = $("[id$='HiddenAppScroll']").val();
            document.getElementById("appdiv").scrollTop = position;
            //console.log("set :" + position);
        }

    </script>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Content ---------------------------------------------------------------->

    <div class="row">
        <div class="col-sm-3">

            <b>Application List</b>
            <br />
            <div class="has-feedback">
            <asp:TextBox ID="Search_TextApp" name="Search_TextApp" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Search..." Style="margin-top:10px;" AutoCompleteType="Disabled"></asp:TextBox>
            <span class="fa fa-search form-control-feedback" style="padding-right:5px; color:darkgrey;"></span>
            </div>

            <div id="appdiv" style="height: 420px; overflow: scroll;" onscroll="getAppScroll(this)">
                <asp:UpdatePanel ID="SideApp" runat="server"> <ContentTemplate>

                    <!-- elemen untuk menyimpan sementara value row grid application yg diklik -->
                    <asp:HiddenField ID="HiddenRowSelect" runat="server"/>
                    <asp:HiddenField ID="HiddenAppIdSelect" runat="server"/>
                    <asp:HiddenField ID="HiddenAppNameSelect" runat="server"/>
                    <asp:HiddenField ID="HiddenAppScroll" runat="server"/>

                    <asp:GridView ID="GridViewApplication" runat="server" AutoGenerateColumns="False" CssClass="table small"
                        HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center" 
                        ShowHeaderWhenEmpty="True" DataKeyNames="application_id" EmptyDataText="No Data" BorderWidth="0">
                        <PagerStyle CssClass="pagination-ys" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="hidden" ItemStyle-Width="100%" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="boxShadow" HeaderStyle-BorderWidth="0">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkListApps" runat="server" Style="text-decoration: none; color: #3d3d3d" OnClick="LinkButtonApps_Click">
                                        <div style="width:100%; text-align:left;"> <asp:Label ID="LabelListApps" runat="server" Text='<%# Eval("application_name")%>'></asp:Label> </div>
                                    </asp:LinkButton>
                                    <asp:HiddenField ID="HiddenFieldAppID" Value='<%# Eval("application_id")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </ContentTemplate> </asp:UpdatePanel>
            </div>
            <br />
        </div>

        <div class="col-sm-9 Contentutama" runat="server" id="divkonten" visible="false">
            
            <div class="row borderTitle" style="padding-top: 10px; padding-bottom: 10px">
                <div class="col-lg-6 TeksHeader" style="padding-top: 5px;"><b>Role Mapping - <asp:Label ID="LabelAppTitle" runat="server" Text="Application"></asp:Label></b></div>
                <div class="col-lg-6" style="text-align:right">
                    <div class="form-inline" style="display:inline-flex;">
                        <div class="has-feedback" style="text-align:right;">
                            <asp:TextBox ID="Search_Page" name="Search_Page" runat="server" CssClass="searchBoxAnimation form-control" placeholder="Search..."  AutoCompleteType="Disabled"></asp:TextBox>
                            <span class="fa fa-search form-control-feedback searchIconAnimation" ></span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- welcome page -->
            <div class="text-center" runat="server" id="divWaiting" Style="color:lightgray; font-size:20px;">
                <br />
                -- <i class="icon icon-ic_RoleMapping" style="font-size:30px;"></i> --
                <br />
                <asp:Label ID="LabelWaiting" runat="server" Text="Please Select Application First..."></asp:Label>
                <br /><br />
            </div>

            <!-- elemen div yang isinya di build di server side -->
            <div id="divContentMapping" runat="server"></div> 

        </div>
    </div>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

    <!-- ##### Modal Mapping Page Role ##### -->
    <div class="modal fade" id="modalAddMapPage" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Map Page Role</label>
                    </h4>
                </div>

                <div class="titleHeaderBlock">
                    <table border="0" style="width:100%;">
                        <tr>
                            <td style="width:20%;"> <b> Application </b> </td>
                            <td style="width:5%;"> : </td>
                            <td> <asp:Label ID="Add_LabelAppName" runat="server"></asp:Label> </td>
                        </tr>
                        <tr>
                            <td style="width:20%;"> <b> Role </b> </td>
                            <td style="width:5%;"> : </td>
                            <td> <asp:Label ID="Add_LabelRoleName" runat="server"></asp:Label> </td>
                        </tr>
                    </table>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel ini berfungsi untuk menjaga item didalamnya tidak terrefresh oleh postback dari luar update panel ini --> 
                    <asp:UpdatePanel ID="UpdatePanelADDmapRA" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div hidden> <asp:Button ID="ButtonClearList" runat="server" Text="Clear" OnClick="ButtonClearList_Click"/> </div>

                    <asp:HiddenField ID="HiddenAppID" runat="server" />
                    <asp:HiddenField ID="HiddenRoleID" runat="server" />

                    <div class="form-group">
                        Select Page
                            <!-- set autopostback=true agar event di server side bisa diexecute dan hanya akan berlaku didalam update panel ini -->
                            <asp:DropDownList ID="Add_DDLPagename" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false" runat="server" OnSelectedIndexChanged="Add_DDLPagename_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <div hidden> <asp:Button ID="ButtonDisableDDL" runat="server" Text="Button" OnClick="ButtonDisableDDL_Click" /> </div>

                            <!-- Gridview untuk menampung data sementara sebelum di save -->
                            <div style="background-color:white; border:1px solid #ccc; border-radius:5px 5px; margin-top:5px;">
                            <asp:GridView ID="Add_GridViewPageMapList" runat="server"  CssClass="table table-hover TeksNormal small" BorderWidth="0" AutoGenerateColumns="False" EmptyDataText="no data" ShowHeader="false">
                                <RowStyle Height="15px" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Page Name" ItemStyle-Width="90%" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" DataField="page_name" SortExpression="page_name" ItemStyle-Height="15px"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" ItemStyle-Height="15px">
                                             <ItemTemplate>
                                                 <asp:ImageButton ID="ImgBtn_DeleteRow" ImageUrl="~/Assets/Icons/ic_hapus.svg" CssClass="ic_Hapus" runat="server" OnClick="ImgBtn_DeleteRow_Click"/>
                                             </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
                            </div> 
                    </div>

                    </ContentTemplate> </asp:UpdatePanel>

                    <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <!-- loading gif untuk menunggu respon proses pada akses ke server side pada update panel tertunjuk -->
                                <asp:UpdateProgress ID="uProgPageRolenMAP" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelADDmapRA">
                                    <ProgressTemplate>
                                        <img alt="" height="25px" width="25px" style="background-color:transparent; vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                                <!-- dipasangi update panel agar element didalamnya dapat diupdate dari server side dan tidak terrefresh oleh postback -->
                                <asp:UpdatePanel ID="UpdatePanelExistAdd" runat="server"> <ContentTemplate>
                                <b> <p style="color: red; display: none" id="p_Add" runat="server"> </p> </b> 
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                            <td style="width:30%; text-align:right;"> 
                                <button type="button" class="btn btn-success" onclick="return AddFormCheck()">Save & Close</button>
                                <div hidden> <asp:Button ID="Add_ButtonSaveMapPage" runat="server" Text="Save & Close" class="btn btn-success" OnClick="Add_ButtonSaveMapPage_Click"></asp:Button> </div>
                            </td>
                        </tr>
                    </table>
                    
                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Add Page Role -->

    <!-- ##### Modal Update Status Page Role ##### -->
    <div class="modal fade" id="modalUpdateStatus" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;">
       
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="javascript:window.location.reload()">×</button> 
                    <h4 class="modal-title">
                        <label>Update Status </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel dipasang agar saat button update status diklik via javascript, page tidak terefresh -->
                    <asp:UpdatePanel ID="UpdatePanelSTATUSmapRA" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="text-center">
                        <asp:HiddenField ID="HideRA_ID" runat="server" />
                        <asp:HiddenField ID="HideRA_Active" runat="server" />
 
                        Are you sure to update the status to <b> <asp:Label ID="LabelStatusActive" runat="server" Text="Label"></asp:Label> </b> ? <br /><br />

                        <asp:HiddenField ID="HideRA_roleID" runat="server" />
                        <asp:HiddenField ID="HideRA_pageID" runat="server" />
                        <asp:HiddenField ID="HideCreatby" runat="server" />
                        <asp:HiddenField ID="HideCreatdate" runat="server" />

                        <asp:Button ID="ButtonChangeStatus" runat="server" Text="Save & Close" class="btn btn-success" OnClick="ButtonChangeStatus_Click"></asp:Button>
                    </div>

                    </ContentTemplate> </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Update Status Page Role -->

</asp:Content>