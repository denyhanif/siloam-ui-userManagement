<%@ Page Title="Page" Language="C#" AutoEventWireup="true" CodeBehind="PageMaster.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Siloam.Ui.UserManagement.Pages.PageMaster" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Script ----------------------------------------------------------------->

     <script type="text/javascript"> 

       //fungsi pencarian data di gridview client side
       $.expr[":"].containsNoCase = function (el, i, m) {  
        var search = m[3];  
        if (!search) return false;  
        return eval("/" + search + "/i").test($(el).text());  
        }; 

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

        //fungsi mamanggil modal add data
        function addPageData()
        {
            var appid = document.getElementById('<%= HiddenAppIdSelect.ClientID %>');
            var appname = document.getElementById('<%= LabelAppTitle.ClientID %>');

            var add_appid = document.getElementById('<%= Add_HiddenDDLAppId.ClientID %>');
            var add_appname = document.getElementById('<%= Add_LabelDDLAppname.ClientID %>');

            if (appname.innerHTML != "Application") {
                $('#modalAddPage').modal('show');
                add_appname.innerHTML = appname.innerHTML;
                add_appid.value = appid.value;
            }
            else
            {
                $('#modalUnselect').modal('show');
            }
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal edit
        function EditDetails(pageid, appid, pagename, createby, createdate, active) {
            $('#modalEditPage').modal('show');

            var Edit_hidden_pageid = document.getElementById('<%= Edit_HiddenPageID.ClientID %>');
            var Edit_hidden_appid = document.getElementById('<%= Edit_HiddenAppID.ClientID %>');
            var Edit_hidden_Text_pagename = document.getElementById('<%= Edit_HiddenTextPagename.ClientID %>');

            var Edit_Text_pagename = document.getElementById('<%= Edit_TextPagename.ClientID %>');

            var Edit_hidden_active = document.getElementById('<%= Edit_HiddenIsActive.ClientID %>');
            var Edit_hidden_createby = document.getElementById('<%= Edit_HiddenCreateby.ClientID %>');
            var Edit_hidden_createdate = document.getElementById('<%= Edit_HiddenCreatedate.ClientID %>');

            Edit_hidden_pageid.value = pageid;
            Edit_hidden_appid.value = appid;
            Edit_hidden_Text_pagename.value = pagename;

            Edit_Text_pagename.value = pagename;

            Edit_hidden_active.value = active;
            Edit_hidden_createby.value = createby;
            Edit_hidden_createdate.value = createdate;

            //fungsi untuk mereplace karakter underscore(/_) menjadi spasi secara global(/g)
            Edit_hidden_Text_pagename.value = Edit_hidden_Text_pagename.value.toString().replace(/_/g, " ");
            Edit_Text_pagename.value = Edit_Text_pagename.value.toString().replace(/_/g, " ");
            Edit_hidden_createdate.value = Edit_hidden_createdate.value.toString().replace(/_/g, " ");
            Edit_hidden_createby.value = Edit_hidden_createby.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal update status tanpa menampilkan modal
        function UpdateStatus(pageid, appid, pagename, createby, createdate, active) {     

            var hidden_pageid_s = document.getElementById('<%= HidePageID.ClientID %>');
            var hidden_appid_s = document.getElementById('<%= HideAppID.ClientID %>');
            var hidden_pagename_s = document.getElementById('<%= HidePageName.ClientID %>');

            var label_active_s = document.getElementById('<%= LabelStatusActive.ClientID %>');
            var label_pagename_s = document.getElementById('<%= LblPageName.ClientID %>');

            var hidden_active_s = document.getElementById('<%= HidePageActive.ClientID %>');
            var hidden_createby_s = document.getElementById('<%= HideCreatby.ClientID %>');
            var hidden_createdate_s = document.getElementById('<%= HideCreatdate.ClientID %>');

            hidden_pageid_s.value = pageid;
            hidden_appid_s.value = appid;
            hidden_pagename_s.value = pagename;

            label_pagename_s.innerHTML = pagename;

            if (active == "True") {
                label_active_s.innerHTML = "Inactive";
            }
            else {
                label_active_s.innerHTML = "Active";
            }

            hidden_active_s.value = active;
            hidden_createby_s.value = createby;
            hidden_createdate_s.value = createdate;

            hidden_pagename_s.value = hidden_pagename_s.value.toString().replace(/_/g, " ");
            hidden_createdate_s.value = hidden_createdate_s.value.toString().replace(/_/g, " ");
            label_pagename_s.innerHTML = label_pagename_s.innerHTML.toString().replace(/_/g, " ");
            hidden_createby_s.value = hidden_createby_s.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            //fungsi klik otomatis button update pada server side
            document.getElementById('<%= ButtonChangeStatus.ClientID %>').click();

            return false;
        } 

        //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
        function AddListPageCheck()
        {
            if ($("[id$='Add_TextPagename']").val().length == 0) {
                $("[id$='Add_TextPagename']").focus();
                $("[id$='p_Add']").removeAttr("style");
                $("[id$='p_Add']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Page Name cannot be empty!";

                return false;
            }
            else
            {
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "";
            }
        }

        //fungsi  untuk validasi gridview kosong, lalu menampilkan notifikasi text berwarna merah
        function AddFormCheck()
        {
            //use preTag --MainContent-- if using --#-- on the element ID
            if($('#MainContent_Add_GridViewPagenameList tr td').first()[0].innerHTML == "no data")
            {
                $("[id$='Add_TextPagename']").focus();
                $("[id$='p_Add']").removeAttr("style");
                $("[id$='p_Add']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Please Add Some Data Before Save!";

                return false;
            }
        }

        //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
        function EditFormCheck()
        {
            if ($("[id$='Edit_TextPagename']").val().length == 0)
            {
                $("[id$='Edit_TextPagename']").focus();
                $("[id$='p_Edit']").removeAttr("style");
                $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Page Name cannot be empty!";

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

            document.getElementById('<%= Add_TextPagename.ClientID %>').value = "";

            document.getElementById('<%= ButtonClearList.ClientID %>').click();
        }

        //fungsi event klik pada area diluar modal
        $(document).ready(function () { 
            $('#modalEditPage').on('hidden.bs.modal', function (e) {
                resetModalForm();
            });

            $('#modalAddPage').on('hidden.bs.modal', function (e) {
                resetModalForm();
            });

            setAppScroll();

            //fungsi mempertahankan style bootstrap switch saat postback dalam update panel
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

        //fungsi untuk menambahkan event pada tombol enter saat add list page 
        $(document).keypress(function(event){
            document.getElementById('<%= Add_TextPagename.ClientID %>').addEventListener("keyup", function(event) {
                event.preventDefault();
                if (event.keyCode === 13) {
                    document.getElementById('<%= Add_ImgBtnAddPageList.ClientID %>').click();
                }
            });  
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

        //fungsi untuk menonaktifkan tombol Enter
        //$(document).keypress(
        //function(event){
        //    if (event.which == '13') {
        //      event.preventDefault();
        //    }
        //});

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
                <div class="col-lg-6 TeksHeader" style="padding-top: 5px;"><b style="float:left">Page List - <asp:Label ID="LabelAppTitle" runat="server" Text="Application"></asp:Label></b>

                    <%--update progress untuk menunggu loading pindah page--%>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePaneDataPagee">
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
                <div class="col-lg-6" style="text-align:right;">

                    <div class="form-inline" style="display:inline-flex;">
                        <div class="has-feedback" style="text-align:right; margin-right:10px;">
                            <asp:TextBox ID="Search_masterData" name="Search_masterData" runat="server" CssClass="searchBoxAnimation form-control" onkeyup="cariData(event)" placeholder="Search..."  AutoCompleteType="Disabled"></asp:TextBox>
                            <span class="fa fa-search form-control-feedback searchIconAnimation"></span>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanelCari" runat="server"> <ContentTemplate>
                            <div hidden> <asp:Button ID="ButtonCari" runat="server" Text="Button" OnClick="ButtonCari_Click" /> </div>
                            <asp:HiddenField ID="HiddenFlagCari" runat="server" />
                        </ContentTemplate> </asp:UpdatePanel>
                        <button type="button" class="TombolAddData btn btn-primary TeksNormal" onclick="addPageData()"><i class="fa fa-plus"></i>&nbsp; Add Page</button>
                    </div>

                </div>
            </div>

            <!-- welcome page -->
            <div class="text-center" runat="server" id="divWaiting" Style="color:lightgray; font-size:20px;">
                <br />
                -- <i class="icon icon-ic_Page" style="font-size:30px;"></i> --
                <br />
                <asp:Label ID="LabelWaiting" runat="server" Text="Please Select Application First..."></asp:Label>
                <br /><br />
            </div>

            <asp:UpdatePanel ID="UpdatePaneDataPagee" runat="server" UpdateMode="Always"> <ContentTemplate>

            <asp:GridView ID="GridViewPage" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover TeksNormal"
                AllowPaging="True" PageSize="9" OnPageIndexChanging="GridViewPage_PageIndexChanging" ShowHeaderWhenEmpty="True" 
                DataKeyNames="page_id" EmptyDataText="No Data" BorderWidth="0" ShowHeader="false">
                <PagerStyle CssClass="pagination-ys" />
                <Columns>      
                    <asp:BoundField HeaderText="Page Name" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" DataField="page_name" SortExpression="page_name" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>    
                    <asp:BoundField HeaderText="Page ID" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="50%" ItemStyle-HorizontalAlign="Left" DataField="page_id" SortExpression="page_id" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>  
                    <asp:TemplateField HeaderText="Is Active" ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <input ID="CheckBoxStatus" class="CheckBoxSwitch" type="checkbox" <%# Eval("is_active").ToString() == "True" ? "checked" : "unchecked"%> data-toggle="toggle" data-on="Active" data-off="Inactive" data-onstyle="success" data-offstyle="default" data-size="mini" onchange=<%# "UpdateStatus('" + Eval("page_id") + "','" + Eval("application_id") + "','" + Eval("page_name").ToString().Replace(" ","_")  + "','" + Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <a href="#" style="text-decoration: none;" onclick=<%# "EditDetails('" + Eval("page_id") + "','" + Eval("application_id") + "','" + Eval("page_name").ToString().Replace(" ","_")  + "','" + Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>> <asp:Image ID="ImageEdit" ImageUrl="~/Assets/Icons/ic_Edit.svg" CssClass="ic_Edit" runat="server" /></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            </ContentTemplate> </asp:UpdatePanel>

        </div>
    </div>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

    <!-- ##### Modal Edit Page ##### -->
    <div class="modal fade" id="modalEditPage" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Edit Page</label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel ini berfungsi untuk menjaga item didalamnya tidak terrefresh oleh postback dari luar update panel ini --> 
                    <asp:UpdatePanel ID="UpdatePanelEDITpage" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <asp:HiddenField ID="Edit_HiddenPageID" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenAppID" runat="server" />

                    <div class="form-group">
                        Page Name
                            <asp:TextBox ID="Edit_TextPagename" runat="server" CssClass="MaxWidthTextbox form-control" placeholder="Type here..."></asp:TextBox>
                            <asp:HiddenField ID="Edit_HiddenTextPagename" runat="server" />
                    </div>

                    <asp:HiddenField ID="Edit_HiddenCreateby" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenCreatedate" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenIsActive" runat="server" />
                    <br />

                    <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <!-- loading gif untuk menunggu respon proses pada akses ke server side pada update panel tertunjuk -->
                                <asp:UpdateProgress ID="uProgPagenameEdit" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelEDITpage">
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
                                    <asp:Button ID="Edit_ButtonSavePage" runat="server" Text="Save & Close" class="btn btn-success"  OnClientClick="return EditFormCheck()" OnClick="Edit_ButtonSavePage_Click"></asp:Button>
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
    <!-- End of Modal Edit Page -->

    <!-- ##### Modal Add Page ##### -->
    <div class="modal fade" id="modalAddPage" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;"  data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>Add Page</label>
                    </h4>
                </div>

                <div class="titleHeaderBlock">
                    <table border="0" style="width:100%;">
                        <tr>
                            <td style="width:20%;"> <b> Application </b> </td>
                            <td style="width:5%"> : </td>
                            <td> <asp:Label ID="Add_LabelDDLAppname" runat="server" Text="Label"></asp:Label> </td>
                        </tr>                      
                    </table>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanelADDpage" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div hidden> <asp:Button ID="ButtonClearList" runat="server" Text="Clear" OnClick="ButtonClearList_Click"/> </div>

                    <asp:HiddenField ID="Add_HiddenDDLAppId" runat="server" />
                    <div class="form-group">
                        Add Page Name 
                            <div class="form-inline">
                                <asp:TextBox ID="Add_TextPagename" runat="server" CssClass="MaxWidthTextbox form-control" Style="width:400px;" placeholder="Type here..."></asp:TextBox>
                                <asp:ImageButton ID="Add_ImgBtnAddPageList" ImageUrl="~/Assets/Icons/ic_add.svg" runat="server" class="ic_Add" OnClientClick="return AddListPageCheck()" OnClick="Add_ImgBtnAddPageList_Click" Style="margin-bottom:-8px"/> <b> Add </b>
                            </div>

                        <!-- Gridview untuk menampung data sementara sebelum di save -->
                        <div style="background-color:white; border:1px solid #ccc; border-radius:5px 5px; margin-top:5px;">
                        <asp:GridView ID="Add_GridViewPagenameList" runat="server"  CssClass="table table-hover TeksNormal small" BorderWidth="0" AutoGenerateColumns="False" EmptyDataText="no data" ShowHeader="false">
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
                    <br />

                    </ContentTemplate> </asp:UpdatePanel>

                    <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <asp:UpdateProgress ID="uProgPagenameAdd" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelADDpage">
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
                                    <asp:Button ID="Add_ButtonSavePage" runat="server" Text="Save & Close" class="btn btn-success" OnClientClick="return AddFormCheck()" OnClick="Add_ButtonSavePage_Click"></asp:Button>
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
                    
                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Add Page -->

    <!-- ##### Modal Update Status Page ##### -->
    <div class="modal fade" id="modalUpdateStatus" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;">
       
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="javascript:window.location.reload()">×</button> 
                    <h4 class="modal-title">
                        <label>Update Status <asp:Label ID="LblPageName" runat="server" Text="Label"></asp:Label> </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel dipasang agar saat button update status diklik via javascript, page tidak terefresh -->
                    <asp:UpdatePanel ID="UpdatePanelSTATUSpage" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="text-center">
                        <asp:HiddenField ID="HidePageID" runat="server" />
                        <asp:HiddenField ID="HideAppID" runat="server" />
                        <asp:HiddenField ID="HidePageActive" runat="server" />
 
                        Are you sure to update the status to <b> <asp:Label ID="LabelStatusActive" runat="server" Text="Label"></asp:Label> </b> ? <br /><br />

                        <asp:HiddenField ID="HidePageName" runat="server" />
                        <asp:HiddenField ID="HideCreatby" runat="server" />
                        <asp:HiddenField ID="HideCreatdate" runat="server" />

                        <asp:Button ID="ButtonChangeStatus" runat="server" Text="Save & Close" class="btn btn-success" OnClick="ButtonChangeStatus_Click"></asp:Button>
                    </div>

                    </ContentTemplate> </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Update Status Page -->

    <!-- Modal Unselect -->
    <div class="modal fade" id="modalUnselect" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
      
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">×</button> 
                    <h4 class="modal-title">
                        <label> Add Page  </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <div class="text-center" Style="color:lightgray; font-size:20px;">
                        -- <i class="icon icon-ic_Page" style="font-size:30px;"></i> --
                        <br />
                        Please Select Application First!
                        <br /><br />
                        <button type="button" class="btn btn-success" data-dismiss="modal"> &nbsp; &nbsp; &nbsp; OK &nbsp; &nbsp; &nbsp; </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Unselect -->

</asp:Content>