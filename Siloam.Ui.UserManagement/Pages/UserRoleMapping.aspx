<%@ Page Title="UserRole Mapping" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="UserRoleMapping.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.UserRoleMapping" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Script ----------------------------------------------------------------->

    <script type="text/javascript">

        function UsernameSuggestionUMS() {

            $("#MainContent_txtItemUsername_AC").autocomplete({
                source: "Common/AutoCompleteUsername.aspx",
                minLength: 0,
                open: function () {
                    $('ul.ui-autocomplete').prepend('<li>'
                        + '<table style="width:100%; border-bottom:1px solid lightgrey; background-color:#f4f4f4;">'
                        + '<tr>'
                        + '<td style="width:100%; padding:5px; vertical-align:top; font-weight:bold;"> User Name </td>'
                        + '</tr>'
                        + '</table>'
                        + '</li>');
                },
                position: { my: "left top", at: "left bottom", collision: "flip" },
                select: function (event, ui) {
                    //assign value back to the form element
                    if (ui.item) {
                        $(event.target).val(ui.item.user_name);

                        document.getElementById('<%= HiddenUsrID.ClientID %>').value = ui.item.user_id;
                        document.getElementById('<%= HiddenUsrName.ClientID %>').value = ui.item.user_name;
                        document.getElementById('<%= ButtonAjaxSearchUsername.ClientID %>').click();  
                    }
                }
            })
                .focus(function () {
                    $(this).autocomplete("search");
                })
                .autocomplete("instance")._renderItem = function (ul, item) {

                    return $("<li>")
                        .append('<table style="width:100%; border-bottom:1px solid lightgrey;">'
                            + '<tr>'
                            + '<td style="width:100%; padding:5px; vertical-align:top;">' + item.user_name + '</td>'
                            + '</tr>'
                            + '</table>')
                        .appendTo(ul);

                };
        }

        //jquery search 
         $.expr[":"].containsNoCase = function (el, i, m) {  
        var search = m[3];  
        if (!search) return false;  
        return eval("/" + search + "/i").test($(el).text());  
        }; 

        //jquery search data application //fungsi pencarian data di gridview client side
        $(document).ready(function () {  
        $("[id$='Search_TextApp']").keyup(function () {  
            if ($("[id$='Search_TextApp']").val().length > 1) {  
                $("[id$='GridViewApplication'] tr").hide();  
                $("[id$='GridViewApplication'] tr:first").show();  
                $("[id$='GridViewApplication'] tr td:containsNoCase(\'" + $("[id$='Search_TextApp']").val() + '\')').parent().show();  
            }  
            else if ($("[id$='Search_TextApp']").val().length == 0) {  
                resetSearchValueApp();  
            }  
  
            if ($("[id$='GridViewApplication'] tr:visible").length == 1) {  
                $('.norecords').remove();  
                $("[id$='GridViewApplication']").append('<tr class="norecords"><td colspan="6" class="Normal" style="text-align: center">No records were found</td></tr>');  
            }  
        });  

            $("[id$='Search_TextApp']").keyup(function (event) {  
            if (event.keyCode == 27) {  
                resetSearchValueApp();  
            }  
            });  
        }); 

        function resetSearchValueApp() {  
        $("[id$='Search_TextApp']").val('');  
        $("[id$='GridViewApplication'] tr").show();  
        $('.norecords').remove();  
        $("[id$='Search_TextApp']").focus();  
        } 

        //jquery search data organization //fungsi pencarian data di gridview client side
        $(document).ready(function () {  
        $("[id$='Search_TextOrg']").keyup(function () {  
            if ($("[id$='Search_TextOrg']").val().length > 1) {  
                $("[id$='GridViewOrganization'] tr").hide();  
                $("[id$='GridViewOrganization'] tr:first").show();  
                $("[id$='GridViewOrganization'] tr td:containsNoCase(\'" + $("[id$='Search_TextOrg']").val() + '\')').parent().show();  
            }  
            else if ($("[id$='Search_TextOrg']").val().length == 0) {  
                resetSearchValueOrg();  
            }  
  
            if ($("[id$='GridViewOrganization'] tr:visible").length == 1) {  
                $('.norecords').remove();  
                $("[id$='GridViewOrganization']").append('<tr class="norecords"><td colspan="6" class="Normal" style="text-align: center">No records were found</td></tr>');  
            }  
        });  

            $("[id$='Search_TextOrg']").keyup(function (event) {  
            if (event.keyCode == 27) {  
                resetSearchValueOrg();  
            }  
            });  
        }); 

        function resetSearchValueOrg() {  
        $("[id$='Search_TextOrg']").val('');  
        $("[id$='GridViewOrganization'] tr").show();  
        $('.norecords').remove();  
        $("[id$='Search_TextOrg']").focus();  
        } 

        //fungsi pencarian data di gridview client side - username search
        $(document).ready(function () {  
        $("[id$='Search_Username']").keyup(function () {  
            if ($("[id$='Search_Username']").val().length > 1) { 
                $('.header').show();
                $('td.headerlabel').parent('tr.headerRole').find("i.iconRole").removeClass("fa-plus-square");
                $('td.headerlabel').parent('tr.headerRole').find("i.iconRole").addClass("fa-minus-square");

                $("[id$='tbl_userdetail'] tr").hide();  
                //$("[id$='tbl_userdetail'] tr:first").show();  
                $("[id$='tbl_userdetail'] tr td:containsNoCase(\'" + $("[id$='Search_Username']").val() + '\')').parent().show();  
            }  
            else if ($("[id$='Search_Username']").val().length == 0) { 
                $('.header').hide();
                $('td.headerlabel').parent('tr.headerRole').find("i.iconRole").removeClass("fa-minus-square");
                $('td.headerlabel').parent('tr.headerRole').find("i.iconRole").addClass("fa-plus-square");

                resetSearchValue();  
            }  
  
            if ($("[id$='tbl_userdetail'] tr:visible").length == 1) {  
                $('.norecords').remove();  
                //$("[id$='tbl_userdetail']").append('<tr class="norecords"><td colspan="6" class="Normal" style="text-align: center">No records were found</td></tr>');  
            }  
        });  

            $("[id$='Search_Username']").keyup(function (event) {  
            if (event.keyCode == 27) {  
                resetSearchValue();  
            }  
            });  
        }); 

        function resetSearchValue() {  
        $("[id$='Search_Username']").val('');  
        $("[id$='tbl_userdetail'] tr").show();  
        $('.norecords').remove();  
        $("[id$='Search_Username']").focus();  
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
        function AddMappingUser(orgid, orgname, appid, appname, roleid, rolename) {
            $('#modalAddMapUser').modal('show');

            var hidden_orgid_add = document.getElementById('<%= HiddenOrgID.ClientID %>');
            var hidden_appid_add = document.getElementById('<%= HiddenAppID.ClientID %>');
            var hidden_roleid_add = document.getElementById('<%= HiddenRoleID.ClientID %>');

            var label_orgname_add = document.getElementById('<%= Add_LabelOrgName.ClientID %>');
            var label_appname_add = document.getElementById('<%= Add_LabelAppName.ClientID %>');
            var label_rolename_add = document.getElementById('<%= Add_LabelRoleName.ClientID %>');

            hidden_orgid_add.value = orgid;
            hidden_appid_add.value = appid;
            hidden_roleid_add.value = roleid;

            label_orgname_add.innerHTML = orgname;
            label_appname_add.innerHTML = appname;
            label_rolename_add.innerHTML = rolename;

            label_orgname_add.innerHTML = label_orgname_add.innerHTML.toString().replace(/_/g, " ");
            label_appname_add.innerHTML = label_appname_add.innerHTML.toString().replace(/_/g, " ");
            label_rolename_add.innerHTML = label_rolename_add.innerHTML.toString().replace(/_/g, " ");

            //fungsi klik untuk disable item yang sudah termapping
            <%--document.getElementById('<%= ButtonDisableDDL.ClientID %>').click();--%>

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal add data
        function SwitchTheRole(orgid, orgname, appid, appname, userroleid, userid, username, createdby, createddate, isactive) {
            $('#modalSwitchTheRole').modal('show');

            var hidden_UR_sw = document.getElementById('<%= SW_HiddenURID.ClientID %>');
            var hidden_orgid_add = document.getElementById('<%= SW_HiddenOrgID.ClientID %>');
            var hidden_appid_add = document.getElementById('<%= SW_HiddenAppID.ClientID %>');
            var hidden_userid_add = document.getElementById('<%= SW_HiddenUserID.ClientID %>');

            var label_orgname_add = document.getElementById('<%= SW_LabelOrgName.ClientID %>');
            var label_appname_add = document.getElementById('<%= SW_LabelAppName.ClientID %>');
            var label_username_add = document.getElementById('<%= SW_LabelUserName.ClientID %>');

            var hidden_creatby_sw = document.getElementById('<%= SW_HiddenCreateBy.ClientID %>');
            var hidden_createdate_sw = document.getElementById('<%= SW_HiddenCreateDate.ClientID %>');
            var hidden_isactive_sw = document.getElementById('<%= SW_HiddenIsActive.ClientID %>');

            hidden_UR_sw.value = userroleid;
            hidden_orgid_add.value = orgid;
            hidden_appid_add.value = appid;
            hidden_userid_add.value = userid;

            label_orgname_add.innerHTML = orgname;
            label_appname_add.innerHTML = appname;
            label_username_add.innerHTML = username;

            hidden_creatby_sw.value = createdby;
            hidden_createdate_sw.value = createddate;
            hidden_isactive_sw.value = isactive;

            label_orgname_add.innerHTML = label_orgname_add.innerHTML.toString().replace(/_/g, " ");
            label_appname_add.innerHTML = label_appname_add.innerHTML.toString().replace(/_/g, " ");
            hidden_createdate_sw.value = hidden_createdate_sw.value.toString().replace(/_/g, " ");
            hidden_creatby_sw.value = hidden_creatby_sw.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal update status tanpa menampilkan modal
        function UpdateStatus(userroleid, userid, orgid, appid, roleid, createdby, createddate, isactive) {

            var hidden_UR_id_s = document.getElementById('<%= HideUR_ID.ClientID %>');
            var hidden_UR_userid_s = document.getElementById('<%= HideUR_userID.ClientID %>');
            var hidden_UR_orgid_s = document.getElementById('<%= HideUR_orgID.ClientID %>');
            var hidden_UR_appid_s = document.getElementById('<%= HideUR_appID.ClientID %>');
            var hidden_UR_roleid_s = document.getElementById('<%= HideUR_roleID.ClientID %>');

            var label_active_s = document.getElementById('<%= LabelStatusActive.ClientID %>');

            var hidden_active_s = document.getElementById('<%= HideUR_Active.ClientID %>');
            var hidden_createby_s = document.getElementById('<%= HideUR_Creatby.ClientID %>');
            var hidden_createdate_s = document.getElementById('<%= HideUR_Creatdate.ClientID %>');

            hidden_UR_id_s.value = userroleid;
            hidden_UR_userid_s.value = userid;
            hidden_UR_orgid_s.value = orgid;
            hidden_UR_appid_s.value = appid;
            hidden_UR_roleid_s.value = roleid;

            if (isactive == "True") {
                label_active_s.innerHTML = "Inactive";
            }
            else {
                label_active_s.innerHTML = "Active";
            }

            hidden_active_s.value = isactive;
            hidden_createby_s.value = createdby;
            hidden_createdate_s.value = createddate;

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
            if ($('#MainContent_Add_GridViewUserMapList tr td').first()[0].innerHTML == "no data") {
                $("[id$='p_Add']").removeAttr("style");
                $("[id$='p_Add']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Please Add Some Data Before Save!";
                return false;
            }
            else {
                document.getElementById('<%= Add_ButtonSaveMapUser.ClientID %>').click();
            }
        }

         //fungsi  untuk validasi DDL kosong atau sama, lalu menampilkan notifikasi text berwarna merah
        function SWFormCheck()
        {
            var DDL_roleName = document.getElementById('<%= SW_DDLRolename.ClientID %>');

            if (DDL_roleName.value != "00000000-0000-0000-0000-000000000000") {
                document.getElementById('<%= SW_ButtonSwitchRole.ClientID %>').click();      
            }
            else {
                $("[id$='p_sw']").removeAttr("style");
                $("[id$='p_sw']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_sw.ClientID %>').innerHTML = "Please Select Data First!";
                return false;
            }
        }

        //fungsi untuk me reset form input dan notifikasi menjadi kosong kembali
        function resetModalForm()
        {
            <%--var DDL_userName = document.getElementById('<%= Add_DDLUsername.ClientID %>');
            DDL_userName.value = "00000000-0000-0000-0000-000000000000";--%>
            var DDL_roleName = document.getElementById('<%= SW_DDLRolename.ClientID %>');
            DDL_roleName.value = "00000000-0000-0000-0000-000000000000";

            $("[id$='p_Add']").removeAttr("style");
            document.getElementById('<%= p_Add.ClientID %>').innerHTML = "";
            $("[id$='p_sw']").removeAttr("style");
            document.getElementById('<%= p_sw.ClientID %>').innerHTML = "";

            //fungsi untuk menjaga style dropdown search
            $('.selectpicker').selectpicker('refresh');

            document.getElementById('<%= ButtonClearList.ClientID %>').click();
        }

        //inisialisasi bootstrap switch dan //fungsi untuk menjaga bootstrap select
        function PageLoad() {
            $('.CheckBoxSwitch').bootstrapToggle();
            $('.selectpicker').selectpicker(); 
        }

        //fungsi event klik pada area diluar modal
        $(document).ready(function () { 
            $('#modalAddMapUser').on('hidden.bs.modal', function (e) {
                resetModalForm();
            });

            $('#modalSwitchTheRole').on('hidden.bs.modal', function (e) {
                resetModalForm();
            });

            setAppScroll();
            setOrgScroll();
            UsernameSuggestionUMS();

            //fungsi untuk menjaga style saat postback dalam updatepanel
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        $('.CheckBoxSwitch').bootstrapToggle();
                        $('.selectpicker').selectpicker(); 

                        UsernameSuggestionUMS();
                    }
                });
            };
        });
       
        //fungsi untuk menonaktifkan tombol Enter
        $(document).keypress(
            function (event) {
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

        function getOrgScroll(pos) {
            var position = pos.scrollTop;
            $("[id$='HiddenOrgScroll']").val(position);
            //console.log("get :" + position);
        }

        function setOrgScroll() {
            if (document.getElementById("orgdiv") != null) {
                var position = $("[id$='HiddenOrgScroll']").val();
                document.getElementById("orgdiv").scrollTop = position;
                //console.log("set :" + position);
            }
        }

    </script>

    <style>
        .ui-autocomplete {
            max-height: 200px;
            overflow-y: auto;
            overflow-x: hidden;
            z-index: 2147483647;
        }
    </style>

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
                        <asp:HiddenField ID="HiddenAppRowSelect" runat="server"/>
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

        <div class="col-sm-9 Contentutama" style="margin-top:-26.5px; min-height:650px;" runat="server" id="divkonten" visible="false">

            <!-- menampilkan status aplikasi dan organisasi yg sedang aktif -->
            <div class="row borderTitle" style="padding-top: 20px; padding-bottom: 0px;">
                <div class="col-lg-12 TeksNormal text-right" style="padding-top: 10px; font-size:11px;">
                    <b Style="color:#171717"> <%--<i class="icon-ic_App" style="vertical-align:middle; color:#1a2268;"></i>--%> Application : </b> <asp:Label ID="LabelAppTitle" Style="color:#171717" runat="server" Text="Application"></asp:Label>  &nbsp; &nbsp; 
                    <b Style="color:#171717"> <%--<i class="icon-ic_Organization" style="vertical-align:middle; color:#4d9b35;"></i>--%> Organization : </b> <asp:Label ID="LabelOrgTitle" Style="color:#171717" runat="server" Text="Organization"></asp:Label>
                </div> 
            </div>

            <div class="row">
                <div class="col-sm-4 borderSideList">

                    <!-- textbox pencarian dg style -->
                    <div class="groupOrg">
                        <asp:TextBox ID="Search_TextOrg" name="Search_TextOrg" runat="server" CssClass="MaxWidthTextbox inputOrg form-control" Style="margin-top:10px; background-color:transparent;" AutoCompleteType="Disabled" placeholder=" "></asp:TextBox>
                        <span class="fa fa-search form-control-feedback" style="padding-right:5px; color:darkgrey;"></span>
                        <asp:Label ID="LabelHospital" runat="server" CssClass="labelOrg" > Search Hospital </asp:Label>
                    </div>
                    
                    <div id="orgdiv" style="height: 420px; overflow: scroll;" onscroll="getOrgScroll(this)">
                        <asp:UpdatePanel ID="SideOrg" runat="server"> <ContentTemplate>

                            <!-- elemen untuk menyimpan sementara value row grid application yg diklik -->
                            <asp:HiddenField ID="HiddenOrgRowSelect" runat="server"/>
                            <asp:HiddenField ID="HiddenOrgIdSelect" runat="server"/>
                            <asp:HiddenField ID="HiddenOrgNameSelect" runat="server"/>
                            <asp:HiddenField ID="HiddenOrgScroll" runat="server"/>

                            <asp:GridView ID="GridViewOrganization" runat="server" AutoGenerateColumns="False" CssClass="table small"
                                HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center"
                                ShowHeaderWhenEmpty="True" DataKeyNames="organization_id" EmptyDataText="No Data" BorderWidth="0">
                                <PagerStyle CssClass="pagination-ys" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="hidden" ItemStyle-Width="100%" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="boxShadowDark" HeaderStyle-BorderWidth="0">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkListOrg" runat="server" Style="text-decoration: none; color: #3d3d3d" OnClick="LinkListOrg_Click">
                                                <div style="width:100%; text-align:left;"> <asp:Label ID="LabelListOrg" runat="server" Text='<%# Eval("organization_name")%>'></asp:Label> </div>
                                            </asp:LinkButton>
                                            <asp:HiddenField ID="HiddenFieldOrgID" Value='<%# Eval("organization_id")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </ContentTemplate> </asp:UpdatePanel>
                    </div>
                    <br />
                </div>

                 <div class="col-sm-8">
                     <div class="TeksNormal borderTitle" style="margin-left:-15px; margin-right:-15px; padding-left:15px; padding-top:16px; padding-bottom:8px;">
                         <div class="row">
                             <div class="col-lg-6">
                                 <div style="margin-top: 2px;">
                                    <b>User Role Mapping</b>
                                 </div>
                             </div>
                             <div class="col-lg-6" style="text-align:right">
                                <div class="form-inline" style="display:inline-flex;">
                                <div class="has-feedback" style="text-align:right; margin-right:15px; margin-top:-5px; margin-bottom:-5px;">
                                    <asp:TextBox ID="Search_Username" name="Search_Username" runat="server" CssClass="searchBoxAnimation form-control" placeholder="Search..."  AutoCompleteType="Disabled"></asp:TextBox>
                                    <span class="fa fa-search form-control-feedback searchIconAnimation"></span>
                                </div>
                                </div>
                             </div>
                         </div>
                     </div>

                     <!-- welcome page -->
                      <div class="text-center" runat="server" id="divWaiting" style="color:lightgray; font-size:20px;">
                        <br />
                        -- <i class="icon icon-ic_UserMapping" style="font-size:30px;"></i> --
                        <br />
                        <asp:Label ID="LabelWaiting" runat="server" Text="Please Select Application & Organization First..."></asp:Label>
                        <br /><br />
                     </div>
                     
                     <!-- elemen div yang isinya di build di server side -->
                     <div id="divContentMapping" runat="server"></div>
                 </div>
            </div>
        </div>
    </div>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

    <!-- ##### Modal Mapping User Role ##### -->
    <div class="modal fade" id="modalAddMapUser" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">

        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Map User Role</label>
                    </h4>
                </div>

                <div class="titleHeaderBlock">
                    <table border="0" style="width:100%;">
                        <tr>
                            <td style="width:20%;"> <b> Organization </b> </td>
                            <td> : </td>
                            <td> <asp:Label ID="Add_LabelOrgName" runat="server"></asp:Label> </td>
                        </tr>
                        <tr>
                            <td style="width:20%;"> <b> Application </b> </td>
                            <td> : </td>
                            <td> <asp:Label ID="Add_LabelAppName" runat="server"></asp:Label> </td>
                        </tr>
                        <tr>
                            <td style="width:20%;"> <b> Role </b> </td>
                            <td> : </td>
                            <td> <b> <asp:Label ID="Add_LabelRoleName" runat="server"></asp:Label> </b> </td>
                        </tr>
                    </table>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel ini berfungsi untuk menjaga item didalamnya tidak terrefresh oleh postback dari luar update panel ini --> 
                    <asp:UpdatePanel ID="UpdatePanelADDmapUR" runat="server"> <ContentTemplate>

                    <!-- hidden button yang akan diklik dari javascript -->
                    <div class="hidden"> <asp:Button ID="ButtonClearList" runat="server" Text="Clear" OnClick="ButtonClearList_Click"/> </div>

                    <asp:HiddenField ID="HiddenOrgID" runat="server" />
                    <asp:HiddenField ID="HiddenAppID" runat="server" />
                    <asp:HiddenField ID="HiddenRoleID" runat="server" />

                    <div class="form-group">
                        Select User

                            <!-- kotak pencarian Autocomplete -->
                            <div style="margin-top: 10px;">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-3" style="width: 100%;">
                                                <div class="has-feedback" style="width: 100%;">                                       
                                                    <asp:TextBox ID="txtItemUsername_AC" runat="server" Placeholder="~select user~" CssClass="btn btn-default" Style="width: 100%; max-width:100%; text-align:left; background-color:white;" class="autosuggest"></asp:TextBox>
                                                    <span class="fa fa-caret-down form-control-feedback" style="z-index: 0;"></span>
                                                </div>
                                                <%--<asp:HiddenField ID="HF_flagfocusdrugsearch" runat="server" />--%>
                                                <asp:HiddenField ID="HiddenUsrID" runat="server" />
                                                <asp:HiddenField ID="HiddenUsrName" runat="server" />
                                                <asp:Button ID="ButtonAjaxSearchUsername" runat="server" Text="Button" CssClass="hidden" OnClick="ButtonAjaxSearchUsername_Click" />
                                            </div>
                                            <div class="col-sm-3">
                                                <%--<div class="loadingdrug" style="display: none;">
                                                    <div class="modal-backdrop" style="background-color: white; opacity: 0; text-align: center">
                                                    </div>
                                                    &nbsp;
                                                    <img alt="" style="background-color: transparent; height: 20px;" src="<%= Page.ResolveClientUrl("~/Images/Background/small-loader.gif") %>" />
                                                    <asp:HiddenField ID="HFloadingdrug" runat="server" />
                                                </div>--%>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <!-- end kotak pencarian -->

                            <!-- set autopostback=true agar event di server side bisa diexecute dan hanya akan berlaku didalam update panel ini --> 
                            <%--<asp:DropDownList ID="Add_DDLUsername" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false" runat="server" OnSelectedIndexChanged="Add_DDLUsername_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <div class="hidden"> <asp:Button ID="ButtonDisableDDL" runat="server" Text="Button" OnClick="ButtonDisableDDL_Click" /> </div>--%>

                            <!-- Gridview untuk menampung data sementara sebelum di save -->
                            <div style="background-color:white; border:1px solid #ccc; border-radius:5px 5px; margin-top:5px;">
                            <asp:GridView ID="Add_GridViewUserMapList" runat="server"  CssClass="table table-hover TeksNormal small" BorderWidth="0" AutoGenerateColumns="False" EmptyDataText="no data" ShowHeader="false">
                                <RowStyle Height="15px" />
                                    <Columns>
                                        <asp:BoundField HeaderText="User Name" ItemStyle-Width="90%" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" DataField="user_name" SortExpression="user_name" ItemStyle-Height="15px"></asp:BoundField>
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
                                <asp:UpdateProgress ID="uProgUserRolenMAP" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelADDmapUR">
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
                                <div class="hidden"> <asp:Button ID="Add_ButtonSaveMapUser" runat="server" Text="Save & Close" class="btn btn-success" OnClick="Add_ButtonSaveMapUser_Click"></asp:Button> </div>
                            </td>
                        </tr>
                    </table>
                    
                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Add User Role -->

    <!-- ##### Modal Switch User Role ##### -->
    <div class="modal fade" id="modalSwitchTheRole" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">

        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Switch Role User</label>
                    </h4>
                </div>

                <div class="titleHeaderBlock">
                    <table border="0" style="width:100%;">
                        <tr>
                            <td style="width:20%;"> <b> Organization </b> </td>
                            <td> : </td>
                            <td> <asp:Label ID="SW_LabelOrgName" runat="server"></asp:Label> </td>
                        </tr>
                        <tr>
                            <td style="width:20%;"> <b> Application </b> </td>
                            <td> : </td>
                            <td> <asp:Label ID="SW_LabelAppName" runat="server"></asp:Label> </td>
                        </tr>
                        <tr>
                            <td style="width:20%;"> <b> Username </b> </td>
                            <td> : </td>
                            <td> <b> <asp:Label ID="SW_LabelUserName" runat="server"></asp:Label> </b> </td>
                        </tr>
                    </table>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel ini berfungsi untuk menjaga item didalamnya tidak terrefresh oleh postback dari luar update panel ini --> 
                    <asp:UpdatePanel ID="UpdatePanelSWmapUR" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <asp:HiddenField ID="SW_HiddenURID" runat="server" />
                    <asp:HiddenField ID="SW_HiddenOrgID" runat="server" />
                    <asp:HiddenField ID="SW_HiddenAppID" runat="server" />
                    <asp:HiddenField ID="SW_HiddenUserID" runat="server" />

                    <div class="form-group">
                        Select Role
                            <!-- set autopostback=true agar event di server side bisa diexecute dan hanya akan berlaku didalam update panel ini -->
                            <asp:DropDownList ID="SW_DDLRolename" data-live-search="true" CssClass="selectpicker" data-size="9" data-width="100%" data-dropup-auto="false" runat="server"></asp:DropDownList>
                    </div>

                    <asp:HiddenField ID="SW_HiddenCreateBy" runat="server" />
                    <asp:HiddenField ID="SW_HiddenCreateDate" runat="server" />
                    <asp:HiddenField ID="SW_HiddenIsActive" runat="server" />

                    </ContentTemplate> </asp:UpdatePanel>

                    <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <!-- loading gif untuk menunggu respon proses pada akses ke server side pada update panel tertunjuk -->
                                <asp:UpdateProgress ID="uProgSwitchRole" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelSWmapUR">
                                    <ProgressTemplate>
                                        <img alt="" height="25px" width="25px" style="background-color:transparent; vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                                <!-- dipasangi update panel agar element didalamnya dapat diupdate dari server side dan tidak terrefresh oleh postback -->
                                <asp:UpdatePanel ID="UpdatePanelExistSW" runat="server"> <ContentTemplate>
                                <b> <p style="color: red; display: none" id="p_sw" runat="server"> </p> </b> 
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                            <td style="width:30%; text-align:right;"> 
                                <button type="button" class="btn btn-success" onclick="return SWFormCheck()">Save & Close</button>
                                <div class="hidden"><asp:Button ID="SW_ButtonSwitchRole" runat="server" Text="Save & Close" class="btn btn-success" OnClick="SW_ButtonSwitchRole_Click"></asp:Button></div>
                            </td>
                        </tr>
                    </table>                    
                    
                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Add User Role -->

    <!-- ##### Modal Update Status User Role ##### -->
    <div class="modal fade" id="modalUpdateStatus" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" >
       
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
                    <asp:UpdatePanel ID="UpdatePanelSTATUSmapUR" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="text-center">
                        <asp:HiddenField ID="HideUR_ID" runat="server" />
                        <asp:HiddenField ID="HideUR_Active" runat="server" />
 
                        Are you sure to update the status to <b> <asp:Label ID="LabelStatusActive" runat="server" Text="Label"></asp:Label> </b> ? <br /><br />

                        <asp:HiddenField ID="HideUR_userID" runat="server" />
                        <asp:HiddenField ID="HideUR_orgID" runat="server" />
                        <asp:HiddenField ID="HideUR_appID" runat="server" />
                        <asp:HiddenField ID="HideUR_roleID" runat="server" />
                        <asp:HiddenField ID="HideUR_Creatby" runat="server" />
                        <asp:HiddenField ID="HideUR_Creatdate" runat="server" />

                        <asp:Button ID="ButtonChangeStatus" runat="server" Text="Save & Close" class="btn btn-success" OnClick="ButtonChangeStatus_Click"></asp:Button>
                    </div>

                    </ContentTemplate> </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Update Status User Role -->

</asp:Content>
