<%@ Page  Title="User Ticketing" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="UserTicketing.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.UserTicketing" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Script ----------------------------------------------------------------->

    <script type="text/javascript">

        function HopeIDSuggestionSOAP() {

            $("#MainContent_txtItemHopeID_AC").autocomplete({
                source: "Common/AutoCompleteHopeID.aspx",
                minLength: 0,
                open: function () {
                    $('ul.ui-autocomplete').prepend('<li>'
                        + '<table style="width:385px; border-bottom:1px solid lightgrey; background-color:#f4f4f4;">'
                        + '<tr>'
                        + '<td style="width:40%; padding:5px; vertical-align:top; font-weight:bold;"> Hope User ID </td>'
                        + '<td style="width:60%; padding:5px; vertical-align:top; font-weight:bold;"> Hope User Name </td>'
                        + '</tr>'
                        + '</table>'
                        + '</li>');
                },
                position: { my: "left top", at: "left bottom", collision: "flip" },
                select: function (event, ui) {
                    //assign value back to the form element
                    if (ui.item) {
                        $(event.target).val(ui.item.userId);
                        document.getElementById('<%= p_Add.ClientID %>').innerText = "";

                        document.getElementById('<%= HF_ItemSelectedHopeId.ClientID %>').value = ui.item.userId;
                        document.getElementById('<%= TK_LabelHopeID.ClientID %>').innerHTML = ui.item.userId;
                        document.getElementById('<%= TK_LabelHopeName.ClientID %>').innerHTML = ui.item.userName;
                        <%--document.getElementById('<%= ButtonAjaxSearchDiagProc.ClientID %>').click();--%>
                    }
                }
            })
                .focus(function () {
                    $(this).autocomplete("search");
                })
                .autocomplete("instance")._renderItem = function (ul, item) {

                    return $("<li>")
                        .append('<table style="width:385px; border-bottom:1px solid lightgrey;">'
                            + '<tr>'
                            + '<td style="width:40%; padding:5px; vertical-align:top;">' + item.userId + '</td>'
                            + '<td style="width:60%; padding:5px; vertical-align:top;">' + item.userName + '</td>'
                            + '</tr>'
                            + '</table>')
                        .appendTo(ul);

                };
        }

        //function get list data ticket by statusnya
        function viewTicket(status)
        {
            var reject = document.getElementById('<%= HiddenTicketIsreject.ClientID %>');
            var valid = document.getElementById('<%= HiddenTicketIsvalid.ClientID %>');
            var thestatus = document.getElementById('<%= HiddenTicketStatusName.ClientID %>');

            if (status == 'new')
            {
                //alert('New Ticket');   
                reject.value = "false";
                valid.value = "false";
                thestatus.value = "New";
            }
            else if (status == 'approved')
            {
                //alert('Approved Ticket');
                reject.value = "false";
                valid.value = "true";
                thestatus.value = "Approved";
            }
            else if (status == 'rejected')
            {
                //alert('Rejected Ticket');
                reject.value = "true";
                valid.value = "false";
                thestatus.value = "Rejected";
            }
            else if (status == 'all')
            {
                //alert('All Ticket');
                thestatus.value = "All";
            }            
            document.getElementById('<%= TK_ButtonFilterStatus.ClientID %>').click();
        }

        //get data detail satu ticket dan passing data ke modal
        function DetailsTicket(tiketID, isInternal, userName, passWord, fullName, email, phone, appId, hopeOrgId, mobileOrgId, axOrgId, roleId, isReject, isValid, createDate, remark, rejectDate, rejectBy, validDate, validBy, orgId, orgName, appName, roleName, status)
        {
            $('#modalDetailsTicket').modal('show');

            var label_ticketID = document.getElementById('<%= TK_LabelticketID.ClientID %>');
            var label_createDate = document.getElementById('<%= TK_LabelCreateDate.ClientID %>');
            var label_statusTicket = document.getElementById('<%= TK_LabelStatus.ClientID %>');
            var label_usertype = document.getElementById('<%= TK_LabelUserType.ClientID %>');
            var label_userName = document.getElementById('<%= TK_LabelUsername.ClientID %>');
            var label_password = document.getElementById('<%= TK_LabelPassword.ClientID %>');
            var label_fullname = document.getElementById('<%= TK_LabelFullname.ClientID %>');
            var label_email = document.getElementById('<%= TK_LabelEmail.ClientID %>');
            var label_phone = document.getElementById('<%= TK_LabelPhone.ClientID %>');
            var label_appName = document.getElementById('<%= TK_LabelAppName.ClientID %>');
            var label_orgName = document.getElementById('<%= TK_LabelOrgName.ClientID %>');
            var label_roleName = document.getElementById('<%= TK_LabelRoleName.ClientID %>');
            var text_remarks = document.getElementById('<%= TK_TextboxRemarks.ClientID %>');           

            //penambahan label UMS pada ticket ID
            label_ticketID.innerHTML = "UMS" + tiketID;
            label_createDate.innerHTML = createDate;
            label_statusTicket.innerHTML = status;

            //pengaturan akses button sesuai dg status ticket
            if (status == "new")
            {
                $("[class$='warnaStatus']").attr("style", "color:#5e6acf;");
                document.getElementById('<%= TK_ButtonApprove.ClientID %>').disabled = false;
                document.getElementById('<%= TK_ButtonReject.ClientID %>').disabled = false;
                $("[id$='divRemarks']").attr("style", "display:none;");
                $("[id$='hopesection']").attr("style", "display:block;");
                text_remarks.disabled = false;

                if (userName.split("+").length > 1)
                {
                    alert("\nWARNING!\nUsername tidak boleh mengandung SPASI.\nSilakan Reject Tiket Ini.");
                    document.getElementById('<%= TK_ButtonApprove.ClientID %>').disabled = true;
                }
            }
            else if (status == "approved")
            {
                $("[class$='warnaStatus']").attr("style", "color:#4d9b35;");
                document.getElementById('<%= TK_ButtonApprove.ClientID %>').disabled = true;
                document.getElementById('<%= TK_ButtonReject.ClientID %>').disabled = true;
                $("[id$='divRemarks']").attr("style", "display:none;");
                $("[id$='hopesection']").attr("style", "display:none;");
                text_remarks.disabled = false;
            }
            else if (status == "rejected")
            {
                $("[class$='warnaStatus']").attr("style", "color:#c43d32;");
                document.getElementById('<%= TK_ButtonApprove.ClientID %>').disabled = true;
                document.getElementById('<%= TK_ButtonReject.ClientID %>').disabled = true;
                $("[id$='divRemarks']").attr("style", "display:block;");
                $("[id$='hopesection']").attr("style", "display:none;");
                text_remarks.disabled = true;
            }

            if (isInternal.toLowerCase() == "true")
            {
                label_usertype.innerHTML = "INTERNAL";
            }
            else
            {
                label_usertype.innerHTML = "EKSTERNAL";
            }

            label_userName.innerHTML = userName;
            label_password.innerHTML = passWord;
            label_fullname.innerHTML = fullName;
            label_email.innerHTML = email;
            label_phone.innerHTML = phone;
            label_appName.innerHTML = appName;
            label_orgName.innerHTML = orgName;
            label_roleName.innerHTML = roleName;
            text_remarks.value = remark;

            label_createDate.innerHTML = label_createDate.innerHTML.toString().replace(/_/g, " ");
            label_fullname.innerHTML = label_fullname.innerHTML.toString().replace(/_/g, " ");
            label_appName.innerHTML = label_appName.innerHTML.toString().replace(/_/g, " ");
            label_orgName.innerHTML = label_orgName.innerHTML.toString().replace(/_/g, " ");
            label_roleName.innerHTML = label_roleName.innerHTML.toString().replace(/_/g, " ");
            text_remarks.value = text_remarks.value.toString().replace(/_/g, " ");
            label_userName.innerHTML = label_userName.innerHTML.toString().replace(/\+/g, " ");

            //hidden value ------------------------------------------------------------------------

            var hidden_ticketID = document.getElementById('<%= HF_ticketID.ClientID %>');
            var hidden_isInternal = document.getElementById('<%= HF_isInternal.ClientID %>');
            var hidden_username = document.getElementById('<%= HF_userName.ClientID %>');
            var hidden_password = document.getElementById('<%= HF_password.ClientID %>');
            var hidden_fullname = document.getElementById('<%= HF_fullname.ClientID %>');
            var hidden_email = document.getElementById('<%= HF_email.ClientID %>');
            var hidden_phone = document.getElementById('<%= HF_phone.ClientID %>');
            var hidden_appID = document.getElementById('<%= HF_appID.ClientID %>');
            var hidden_hopeOrgID = document.getElementById('<%= HF_hopeOrgId.ClientID %>');
            var hidden_mobileOrgID = document.getElementById('<%= HF_mobileOrgId.ClientID %>');
            var hidden_axOrgID = document.getElementById('<%= HF_axOrgId.ClientID %>');
            var hidden_roleID = document.getElementById('<%= HF_roleId.ClientID %>');
            var hidden_isReject = document.getElementById('<%= HF_isReject.ClientID %>');
            var hidden_isValid = document.getElementById('<%= HF_isValid.ClientID %>');
            var hidden_createDate = document.getElementById('<%= HF_createDate.ClientID %>');
            var hidden_remarks = document.getElementById('<%= HF_remark.ClientID %>');
            var hidden_rejectDate = document.getElementById('<%= HF_rejectDate.ClientID %>');
            var hidden_rejectBy = document.getElementById('<%= HF_rejectBy.ClientID %>');
            var hidden_validDate = document.getElementById('<%= HF_validDate.ClientID %>');
            var hidden_validBy = document.getElementById('<%= HF_validBy.ClientID %>');
            var hidden_orgID = document.getElementById('<%= HF_orgId.ClientID %>');
            var hidden_orgName = document.getElementById('<%= HF_orgName.ClientID %>');
            var hidden_appName = document.getElementById('<%= HF_appName.ClientID %>');
            var hidden_roleName = document.getElementById('<%= HF_roleName.ClientID %>');
            var hidden_ticketStatus = document.getElementById('<%= HF_ticketStatus.ClientID %>');

            hidden_ticketID.value = tiketID;
            hidden_isInternal.value = isInternal;
            hidden_username.value = userName;
            hidden_password.value = passWord;
            hidden_fullname.value = fullName;
            hidden_email.value = email;
            hidden_phone.value = phone;
            hidden_appID.value = appId;
            hidden_hopeOrgID.value = hopeOrgId;
            hidden_mobileOrgID.value = mobileOrgId;
            hidden_axOrgID.value = axOrgId;
            hidden_roleID.value = roleId;
            hidden_isReject.value = isReject;
            hidden_isValid.value = isValid;
            hidden_createDate.value = createDate;
            hidden_remarks.value = remark;
            hidden_rejectDate.value = rejectDate;
            hidden_rejectBy.value = rejectBy;
            hidden_validDate.value = validDate;
            hidden_validBy.value = validBy;
            hidden_orgID.value = orgId;
            hidden_orgName.value = orgName;
            hidden_appName.value = appName;
            hidden_roleName.value = roleName;
            hidden_ticketStatus.value = status;

            hidden_fullname.value = hidden_fullname.value.toString().replace(/_/g, " ");
            hidden_createDate.value = hidden_createDate.value.toString().replace(/_/g, " ");
            hidden_remarks.value = hidden_remarks.value.toString().replace(/_/g, " ");
            hidden_rejectDate.value = hidden_rejectDate.value.toString().replace(/_/g, " ");
            hidden_validDate.value = hidden_validDate.value.toString().replace(/_/g, " ");
            hidden_appName.value = hidden_appName.value.toString().replace(/_/g, " ");
            hidden_orgName.value = hidden_orgName.value.toString().replace(/_/g, " ");
            hidden_roleName.value = hidden_roleName.value.toString().replace(/_/g, " ");
            hidden_rejectBy.value = hidden_rejectBy.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");
            hidden_validBy.value = hidden_validBy.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");
            hidden_username.value = hidden_username.value.toString().replace(/\+/g, " ");
        }

        //function cek text remark, apakah empty atau tidak untuk proses reject
        function RemarkCheck(action)
        {
            document.getElementById('<%= p_Add.ClientID %>').innerText = "";

            if (action == "MainContent_TK_ButtonReject")
            {
                $("[id$='divRemarks']").attr("style", "display:block;");

                $("[id$='TK_ButtonApprove']").attr("style", "display:none;");
                $("[id$='TK_ButtonCancel']").attr("style", "display:inline-block; width:100px;");

                var textbox_remarks = document.getElementById('<%= TK_TextboxRemarks.ClientID %>');
                if (textbox_remarks.value == "")
                {
                    document.getElementById('<%= TK_TextboxRemarks.ClientID %>').focus();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            if (action == "TK_ButtonCancel")
            {
                resetModalForm();
            }
        }

        //fucntion reset form pada modal
        function resetModalForm()
        {
            document.getElementById('<%= TK_TextboxRemarks.ClientID %>').value = "";
            $("[id$='divRemarks']").attr("style", "display:none;");

            $("[id$='TK_ButtonApprove']").attr("style", "display:inline-block; width:100px;");
            $("[id$='TK_ButtonCancel']").attr("style", "display:none;");

            document.getElementById('<%= p_Add.ClientID %>').innerText = "";

            document.getElementById('<%= HF_ItemSelectedHopeId.ClientID %>').value = "0";
            document.getElementById('<%= TK_LabelHopeID.ClientID %>').innerHTML = "0";
            document.getElementById('<%= TK_LabelHopeName.ClientID %>').innerHTML = "unnamed";
        }

        function resetHopeData() {
            document.getElementById('<%= HF_ItemSelectedHopeId.ClientID %>').value = "0";
            document.getElementById('<%= TK_LabelHopeID.ClientID %>').innerHTML = "0";
            document.getElementById('<%= TK_LabelHopeName.ClientID %>').innerHTML = "unnamed";
        }

        //fungsi event klik pada area diluar modal
        $(document).ready(function () { 
            $('#modalDetailsTicket').on('hidden.bs.modal', function (e) {
                 resetModalForm();
            });

            HopeIDSuggestionSOAP();
            setOrgScroll();

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_beginRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {

                    }
                });
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {

                        HopeIDSuggestionSOAP();
                    }
                });
            };
        });

        //modal konfirmasi untuk change role
        function POPmodalChangeRole(ticketID, newRole, oldRole, URID, UserID, createBy, createDate, Uname)
        {
            var U_name = document.getElementById('<%= CT_LabelUname.ClientID %>');
            var ticket_ID = document.getElementById('<%= CT_LabelticketID.ClientID %>');
            var old_Role = document.getElementById('<%= CT_oldrole.ClientID %>');
            var new_Role = document.getElementById('<%= CT_newrole.ClientID %>');
            var UR_ID = document.getElementById('<%= Hidden_CT_URID.ClientID %>');
            var User_ID = document.getElementById('<%= Hidden_CT_userID.ClientID %>');
            var create_By = document.getElementById('<%= Hidden_CT_createBy.ClientID %>');
            var create_Date = document.getElementById('<%= Hidden_CT_createDate.ClientID %>');

            U_name.innerHTML = Uname;
            ticket_ID.innerHTML = ticketID;
            old_Role.innerHTML = oldRole;
            new_Role.innerHTML = newRole;
            UR_ID.value = URID;
            User_ID.value = UserID;
            create_By.value = createBy;
            create_Date.value = createDate;

            create_Date.value = create_Date.value.toString().replace(/_/g, " ");
            create_By.value = create_By.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            $('#modalChangeRole').modal('show');
            $('#modalDetailsTicket').modal('hide'); 
        }

        //hide change role
        function cancelChangeRole()
        {
            $('#modalChangeRole').modal('hide');
            $('#modalDetailsTicket').modal('show'); 
        }

        //modal konfirmasi data user role already exist
        function POPmodalAlreadyExist(ticketID, Uname)
        {
            var U_name = document.getElementById('<%= AE_LabelUname.ClientID %>');
            var ticket_ID = document.getElementById('<%= AE_LabelticketID.ClientID %>');

            U_name.innerHTML = Uname;
            ticket_ID.innerHTML = ticketID;

            $('#modalAlreadyExist').modal('show');
            $('#modalDetailsTicket').modal('hide');
        }

        //hide already exist 
        function cancelAlreadyExist()
        {
            $('#modalAlreadyExist').modal('hide');
            $('#modalDetailsTicket').modal('show');
        }

        //modal info username is not active
        function POPmodalInfoActiveUSer(ticketID, Uname)
        {
            var U_name = document.getElementById('<%= AU_LabelUname.ClientID %>');
            var ticket_ID = document.getElementById('<%= AU_LabelticketID.ClientID %>');

            U_name.innerHTML = Uname;
            ticket_ID.innerHTML = ticketID;

            $('#modalInfoActiveUSer').modal('show');
            $('#modalDetailsTicket').modal('hide');
        }

        //hide already exist 
        function cancelActiveUSer()
        {
            $('#modalInfoActiveUSer').modal('hide');
            $('#modalDetailsTicket').modal('show');
        }

        //modal konfirmasi untuk aktivasi user role kembali
        function POPmodalActivationRole(ticketID, URID, UserID, createBy, createDate, RoleID, Uname)
        {
            var U_name = document.getElementById('<%= AR_LabelUname.ClientID %>');
            var ticket_ID = document.getElementById('<%= AR_LabelticketID.ClientID %>');
            var UR_ID = document.getElementById('<%= Hidden_AR_URID.ClientID %>');
            var User_ID = document.getElementById('<%= Hidden_AR_userID.ClientID %>');
            var create_By = document.getElementById('<%= Hidden_AR_createBy.ClientID %>');
            var create_Date = document.getElementById('<%= Hidden_AR_createDate.ClientID %>');
            var Role_ID = document.getElementById('<%= Hidden_AR_roleID.ClientID %>');

            U_name.innerHTML = Uname;
            ticket_ID.innerHTML = ticketID;
            UR_ID.value = URID;
            User_ID.value = UserID;
            create_By.value = createBy;
            create_Date.value = createDate;
            Role_ID.value = RoleID;

            create_Date.value = create_Date.value.toString().replace(/_/g, " ");
            create_By.value = create_By.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");

            $('#modalActivationRole').modal('show');
            $('#modalDetailsTicket').modal('hide'); 
        }

        //hide activation user role
        function cancelStatus()
        {
            $('#modalActivationRole').modal('hide');
            $('#modalDetailsTicket').modal('show');
        }

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

        function txtPreventEnter() {
            var c = event.keyCode;
            if (c == 13) {
                return false;
            }
        }

        function getOrgScroll(pos) {
            var position = pos.scrollTop;
            $("[id$='HiddenOrgScroll']").val(position);
            //console.log("get :" + position);
        }

        function setOrgScroll() {
            var position = $("[id$='HiddenOrgScroll']").val();
            document.getElementById("orgdiv").scrollTop = position;
            //console.log("set :" + position);
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

            <%--panel filter status ticket--%>
            <div class="Contentutama" style="margin-bottom:15px;">
                <div class="borderTitle" style="padding-left:15px; padding-top:10px; padding-bottom:5px;"> 
                <b> Filter Status </b> 
                </div>

                <asp:UpdatePanel ID="UpdatePanelFilterStatus" runat="server"> <ContentTemplate>
                <div style="padding-top:5px; padding-bottom:5px;">
                    <div id="divstatusNew" runat="server" class="boxShadowTicket" onclick="viewTicket('new')">
                        <i class="fa fa-circle" style="color:#5e6acf; margin-right:5px;"></i> New
                    </div>
                    <div id="divstatusApproved" runat="server" class="boxShadowTicket" onclick="viewTicket('approved')">
                        <i class="fa fa-circle" style="color:#4d9b35; margin-right:5px;"></i> Approved
                    </div>
                    <div id="divstatusRejected" runat="server" class="boxShadowTicket" onclick="viewTicket('rejected')">
                        <i class="fa fa-circle" style="color:#c43d32; margin-right:5px;"></i> Rejected
                    </div>
                    <div id="divstatusAll" runat="server" class="boxShadowTicket" onclick="viewTicket('all')">
                        <i class="fa fa-circle" style="color:#f2c32e; margin-right:5px;"></i> All Ticket
                    </div>
                    
                    <div hidden> 
                        <asp:Button ID="TK_ButtonFilterStatus" runat="server" Text="Button" OnClick="TK_ButtonFilterStatus_Click" />
                        <asp:HiddenField ID="HiddenTicketIsreject" runat="server" />
                        <asp:HiddenField ID="HiddenTicketIsvalid" runat="server" />
                        <asp:HiddenField ID="HiddenTicketStatusName" runat="server" />
                    </div> 
                </div>
                </ContentTemplate> </asp:UpdatePanel>

            </div>

            <%--panel filter hospitals dan info outstanding ticket--%>
            <div class="Contentutama">
                <div class="borderTitle" style="padding-left:15px; padding-top:10px; padding-bottom:5px;"> 
                <b> Filter Hospital </b> 
                </div>

                <div id="orgdiv" style="max-height: 280px; overflow: scroll;" onscroll="getOrgScroll(this)">                      
                <asp:UpdatePanel ID="UpdatePanelOrgTicketCount" UpdateMode="Always" runat="server"> <ContentTemplate>

                <!-- elemen untuk menyimpan sementara value row grid application yg diklik -->
                <asp:HiddenField ID="HiddenOrgRowSelect" runat="server"/>
                <asp:HiddenField ID="HiddenOrgIdSelect" runat="server"/>
                <asp:HiddenField ID="HiddenOrgNameSelect" runat="server"/>
                <asp:HiddenField ID="HiddenOrgScroll" runat="server"/>

                <asp:GridView ID="GridViewOrganization" runat="server" AutoGenerateColumns="False" CssClass="table table-hover small"
                    HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center"
                    ShowHeaderWhenEmpty="True" DataKeyNames="organization_id" EmptyDataText="No Data" BorderWidth="0">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="hidden" ItemStyle-Width="90%" ItemStyle-HorizontalAlign="Left"  ItemStyle-BorderWidth="0">
                            <ItemTemplate>
                                <div style="margin-left:8px;">
                                <asp:LinkButton ID="LinkListOrg" runat="server" Style="text-decoration: none; color: #3d3d3d;" OnClick="LinkListOrg_Click">
                                    <div style="width:100%; text-align:left;"> <asp:Label ID="LabelListOrg" runat="server" Text='<%# Eval("organization_name")%>'></asp:Label> </div>
                                </asp:LinkButton>
                                <asp:HiddenField ID="HiddenFieldOrgID" Value='<%# Eval("organization_id")%>' runat="server" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="hidden" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0">
                            <ItemTemplate> 
                                <asp:LinkButton ID="LinkListOrg2" runat="server" Style="text-decoration: none;" OnClick="LinkListOrg_Click">
                                    <asp:label ID="LabelTicketNew" runat="server" CssClass="BadgeConter" Text='<%# Eval("counter")%>' Visible='<%# Eval("counter").ToString() == "0" ? false : true %>' ToolTip="New Ticket"></asp:label> 
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                </ContentTemplate> </asp:UpdatePanel>
                </div>
            </div>

        </div>

        <div class="col-sm-9 Contentutama">

            <%--header ticket list--%>
            <div class="row borderTitle" style="padding-top: 10px; padding-bottom: 10px">
                <div class="col-sm-8 TeksHeader" style="padding-top: 5px;"><b style="float:left">Ticket List on :  <asp:Label ID="LabelOrgTitle" runat="server" Text="Organization" style="font-weight:normal; font-size:14px;"></asp:Label> </b>

                    <%--update progress untuk menunggu loading pindah page--%>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelDataTicket">
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
                <div class="col-sm-4" style="text-align:right">

                    <div class="form-inline" style="display:inline-flex;">
                    <div class="has-feedback" style="text-align:right; margin-right:0px;">
                        <asp:TextBox ID="Search_masterData" name="Search_masterData" runat="server" CssClass="searchBoxAnimation form-control" onkeyup="cariData(event)" placeholder="Search..."  AutoCompleteType="Disabled"></asp:TextBox>
                        <span class="fa fa-search form-control-feedback searchIconAnimation"></span>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelCari" runat="server"> <ContentTemplate>
                        <div hidden> <asp:Button ID="ButtonCari" runat="server" Text="Button" OnClick="ButtonCari_Click" /> </div>
                        <asp:HiddenField ID="HiddenFlagCari" runat="server" />
                    </ContentTemplate> </asp:UpdatePanel>
                    </div>

                </div>
            </div>

            <%--ticket list--%>
            <asp:UpdatePanel ID="UpdatePanelDataTicket" UpdateMode="Always" runat="server"> <ContentTemplate>

                <asp:GridView ID="GridViewOutstandingTicket" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover TeksNormal"
                AllowPaging="True" PageSize="9" OnPageIndexChanging="GridViewOutstandingTicket_PageIndexChanging" ShowHeaderWhenEmpty="True" 
                DataKeyNames="user_ticketing_id" EmptyDataText="No Data" BorderWidth="0">
                <PagerStyle CssClass="pagination-ys" /> <rowstyle height="30" />
                <Columns>           
                    <asp:TemplateField HeaderText="Ticket No." ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-left TeksHeaderTable">
                        <ItemTemplate>
                            <a href="#" style="text-decoration: none;" onclick=<%# "DetailsTicket('" + Eval("user_ticketing_id") + "','" + Eval("is_internal")  + "','" + Eval("user_name").ToString().Replace(" ","+")  + "','" + Eval("password")  + "','" + Eval("full_name").ToString().Replace(" ","_")  + "','" + Eval("email")  + "','" + Eval("handphone") + "','" + Eval("application_id") + "','" + Eval("hope_organization_id") + "','" + Eval("mobile_organization_id") + "','" + Eval("ax_organization_id") + "','" + Eval("role_id") + "','" + Eval("is_rejected") + "','" + Eval("is_validated") + "','" + Eval("created_date").ToString().Replace(" ","_") + "','" + Eval("remark").ToString().Replace(" ","_") + "','" + Eval("rejected_date").ToString().Replace(" ","_") + "','" + Eval("rejected_by").ToString().Replace(" ","_").Replace("\\","+") + "','" + Eval("validated_date").ToString().Replace(" ","_") + "','" + Eval("validated_by").ToString().Replace(" ","_").Replace("\\","+") + "','" + Eval("organization_id") + "','" + Eval("organization_name").ToString().Replace(" ","_") + "','" + Eval("application_name").ToString().Replace(" ","_") + "','" + Eval("role_name").ToString().Replace(" ","_") + "','" + Eval("ticket_status") + "')" %>>
                            <asp:Label ID="LabelTiketNo" runat="server" Text='<%# "UMS" + Eval("user_ticketing_id") %>'></asp:Label>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="Username" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" DataField="user_name" SortExpression="user_name" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="Fullname" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" DataField="full_name" SortExpression="full_name" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="Created Date Time" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" DataField="created_date" SortExpression="created_date" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>

                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="12%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="left" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-left TeksHeaderTable">
                        <ItemTemplate>
                            <i class="fa fa-circle" style="color:<%# Eval("ticket_status").ToString() == "new" ? "#5e6acf" : Eval("ticket_status").ToString() == "approved" ? "#4d9b35" : Eval("ticket_status").ToString() == "rejected" ? "#c43d32" : "white"%>; margin-right:5px;"></i>
                            <asp:Label ID="LabelStatusTicket" Style="text-transform:capitalize;" runat="server" Text='<%# Eval("ticket_status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="View" ItemStyle-Width="8%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <a href="#" style="text-decoration: none;" title="View Ticket" onclick=<%# "DetailsTicket('" + Eval("user_ticketing_id") + "','" + Eval("is_internal")  + "','" + Eval("user_name").ToString().Replace(" ","+")  + "','" + Eval("password")  + "','" + Eval("full_name").ToString().Replace(" ","_")  + "','" + Eval("email")  + "','" + Eval("handphone") + "','" + Eval("application_id") + "','" + Eval("hope_organization_id") + "','" + Eval("mobile_organization_id") + "','" + Eval("ax_organization_id") + "','" + Eval("role_id") + "','" + Eval("is_rejected") + "','" + Eval("is_validated") + "','" + Eval("created_date").ToString().Replace(" ","_") + "','" + Eval("remark").ToString().Replace(" ","_") + "','" + Eval("rejected_date").ToString().Replace(" ","_") + "','" + Eval("rejected_by").ToString().Replace(" ","_").Replace("\\","+") + "','" + Eval("validated_date").ToString().Replace(" ","_") + "','" + Eval("validated_by").ToString().Replace(" ","_").Replace("\\","+") + "','" + Eval("organization_id") + "','" + Eval("organization_name").ToString().Replace(" ","_") + "','" + Eval("application_name").ToString().Replace(" ","_") + "','" + Eval("role_name").ToString().Replace(" ","_") + "','" + Eval("ticket_status") + "')" %>> <asp:Image ID="ImageEdit" ImageUrl="~/Assets/Icons/ic_Eye.svg" CssClass="ic_Eye" runat="server" /></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>

            </ContentTemplate> </asp:UpdatePanel>

        </div>
    </div>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

    <!-- ##### Modal Ticketing ##### -->
    <div class="modal fade" id="modalDetailsTicket" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 25px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 600px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>ID <asp:Label ID="TK_LabelticketID" runat="server" Text="Label"></asp:Label> </label>                       
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    
                    <div style="border-bottom: 1px solid #efefef; margin-left: -15px; padding-left: 15px; margin-right: -15px; padding-right: 15px; height: 35px;">
                    <table border="0" class="table" style="margin-top:-15px;">
                        <tr>
                            <td style="width:60%; text-align:left; border-width:0;">
                                <b>Incoming Date :</b> <asp:Label ID="TK_LabelCreateDate" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="width:40%; text-align:right; border-width:0;">
                                <b>Status :</b> <i class="fa fa-circle warnaStatus" style="margin-right:3px; margin-left:3px;"></i>
                                           <asp:Label ID="TK_LabelStatus" Style="text-transform:capitalize;" runat="server" Text="Status"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>

                     <table class="table table-striped text-left small" style="margin-top:0px;">
                        <tr>
                            <td style="border-width:0; width:30%;"><b>User Type </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelUserType" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0; width:30%;"><b>Username </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelUsername" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0; width:30%;"><b>Password </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelPassword" runat="server" Text="Label" style="display:none;"></asp:Label> ******** </td>
                        </tr>
                        <tr>
                            <td style="border-width:0; width:30%;"><b>Full Name </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelFullname" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0; width:30%;"><b>Email </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelEmail" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0; width:30%;"><b>Mobile No. </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelPhone" runat="server" Text="Label"></asp:Label></td>
                        </tr>                       
                    </table>
                       
                    <table style="background-color:#e5e5e5; margin-top:-15px; border-bottom:2px solid #adadad; border-top:2px solid #adadad;" class="table small" >
                         <tr>
                            <td style="border-width:0; width:30%;"><b>APPLICATION </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelAppName" runat="server" Text="Label"></asp:Label></td>
                        </tr> 
                        <tr>
                            <td style="border-width:0; width:30%;"><b>ORGANIZATION </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelOrgName" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0; width:30%;"><b>ROLE </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelRoleName" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                    </table>

                    <div id="hopesection">
                    <!-- kotak pencarian autocomplete -->

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-3" style="width: 30%; font-weight:bold;">
                                    Select Hope Data <i class="fa fa-question-circle" style="font-size:12px; color: darkgrey;" title="Silakan lengkapi Data Hope User sebelum proses Approve (jika data ditemukan).&#010;Note : Jika Sebelumnya Data User sudah pernah terdaftar, maka Hope User ID tidak akan terupdate."></i>
                                </div>

                                <div class="col-sm-7" style="padding-right:0px;">
                                    <div class="has-feedback">
                                        <asp:TextBox ID="txtItemHopeID_AC" runat="server" Placeholder="Select..." Style="width: 100%; max-width:100%;" class="autosuggest" onkeydown="return txtPreventEnter();"></asp:TextBox>
                                        <span class="fa fa-caret-down form-control-feedback" style="margin-top: -6px; z-index: 0;"></span>
                                    </div>
                                </div>

                                <div class="col-sm-1" style="padding-right:0px; padding-left:5px;">
                                    <asp:Button ID="ButtonResetHope" runat="server" CssClass="btn btn-default" style="height: 26px; font-size: 12px; width: 100%; padding: 2px;" Text="Reset" OnClientClick="resetHopeData();" />
                                </div>

                            </div>
                            <asp:HiddenField ID="HF_ItemSelectedHopeId" runat="server" Value="0" />
                            <%--<asp:Button ID="ButtonAjaxSearchDiagProc" runat="server" Text="Button" CssClass="hidden" OnClick="ButtonAjaxSearchDiagProc_Click" />--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <!-- end kotak pencarian autocomplete -->

                    <table style="background-color:#f9f9f9; margin-top:10px; border-bottom:2px solid #e5e5e5; border-top:2px solid #e5e5e5;" class="table small" >
                         <tr>
                            <td style="border-width:0; width:30%;"><b>HOPE USER ID </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelHopeID" runat="server" Text="0"></asp:Label></td>
                        </tr> 
                        <tr>
                            <td style="border-width:0; width:30%;"><b>HOPE USERNAME </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="TK_LabelHopeName" runat="server" Text="unnamed"></asp:Label></td>
                        </tr>
                    </table>

                    </div>

                    <div id="divRemarks" style="display:none;">
                        Reason of Rejection <b style="color:red;">*</b> <br />
                        <asp:TextBox ID="TK_TextboxRemarks" TextMode="MultiLine" CssClass="form-control" Style="width:100%; max-width:100%;" runat="server" placeholder="Please write the reason..."></asp:TextBox>
                    </div>

                    <asp:HiddenField ID="HF_ticketID" runat="server" />
                    <asp:HiddenField ID="HF_isInternal" runat="server" />
                    <asp:HiddenField ID="HF_userName" runat="server" />
                    <asp:HiddenField ID="HF_password" runat="server" />
                    <asp:HiddenField ID="HF_fullname" runat="server" />
                    <asp:HiddenField ID="HF_email" runat="server" />
                    <asp:HiddenField ID="HF_phone" runat="server" />
                    <asp:HiddenField ID="HF_appID" runat="server" />
                    <asp:HiddenField ID="HF_hopeOrgId" runat="server" />
                    <asp:HiddenField ID="HF_mobileOrgId" runat="server" />
                    <asp:HiddenField ID="HF_axOrgId" runat="server" />
                    <asp:HiddenField ID="HF_roleId" runat="server" />
                    <asp:HiddenField ID="HF_isReject" runat="server" />
                    <asp:HiddenField ID="HF_isValid" runat="server" />
                    <asp:HiddenField ID="HF_createDate" runat="server" />
                    <asp:HiddenField ID="HF_remark" runat="server" />
                    <asp:HiddenField ID="HF_rejectDate" runat="server" />
                    <asp:HiddenField ID="HF_rejectBy" runat="server" />
                    <asp:HiddenField ID="HF_validDate" runat="server" />
                    <asp:HiddenField ID="HF_validBy" runat="server" />
                    <asp:HiddenField ID="HF_orgId" runat="server" />
                    <asp:HiddenField ID="HF_orgName" runat="server" />
                    <asp:HiddenField ID="HF_appName" runat="server" />
                    <asp:HiddenField ID="HF_roleName" runat="server" />
                    <asp:HiddenField ID="HF_ticketStatus" runat="server" />

                    <br />
                    
                     <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <asp:UpdatePanel ID="UpdatePanelValidasiTicket" runat="server"> <ContentTemplate>
                                <b> <p style="color: red; display: none" id="p_Add" runat="server"> </p> </b> 
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                            <td style="width:50%; text-align:right;"> 
                                <asp:UpdatePanel ID="UpdatePanelActionTicket" runat="server"> <ContentTemplate>
                                    <asp:Button ID="TK_ButtonReject" runat="server" Text="Reject" CssClass="btn btn-danger" width="100px" OnClientClick="return RemarkCheck(this.id)" OnClick="TK_ButtonReject_Click"></asp:Button> &nbsp; &nbsp;
                                    <asp:Button ID="TK_ButtonApprove" runat="server" Text="Approve" CssClass="btn btn-success" width="100px" OnClick="TK_ButtonApprove_Click"></asp:Button>   
                                    <button type="button" ID="TK_ButtonCancel" class="btn btn-default" onclick="return RemarkCheck(this.id)" width="100px" Style="display:none;">Cancel</button> 
                                </ContentTemplate> </asp:UpdatePanel>

                                <asp:UpdateProgress ID="AdduProgSAVE" runat="server" AssociatedUpdatePanelID="UpdatePanelActionTicket">
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
    <!-- End of Modal Ticketing -->

    <!-- ##### Modal Change Role ##### -->
    <div class="modal fade" id="modalChangeRole" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 25px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <%--penambahan label UMS00 pada ticket ID--%>
                        <label>ID UMS<asp:Label ID="CT_LabelticketID" runat="server" Text="Label"></asp:Label> - <asp:Label ID="CT_LabelUname" runat="server" Text="Label"></asp:Label> </label>                       
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <div class="text-left">

                    User has already mapped to this app & organization as <b><asp:Label ID="CT_oldrole" runat="server" Text="Label"></asp:Label> </b> <br />
                    Would you like to change it to <b><asp:Label ID="CT_newrole" runat="server" Text="Label"></asp:Label></b> ?<br />
    
                    <asp:HiddenField ID="Hidden_CT_URID" runat="server" />
                    <asp:HiddenField ID="Hidden_CT_userID" runat="server" />
                    <asp:HiddenField ID="Hidden_CT_createBy" runat="server" />
                    <asp:HiddenField ID="Hidden_CT_createDate" runat="server" />

                    </div>

                    <br />
                    
                     <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <asp:UpdatePanel ID="UpdatePanelValidasiCHangeRole" runat="server"> <ContentTemplate>
                                <b> <p style="color: red; display: none" id="p_Edit" runat="server"> </p> </b> 
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                            <td style="width:50%; text-align:right;"> 
                                <asp:UpdatePanel ID="UpdatePanelActionConfirm" runat="server"> <ContentTemplate>
                                    <button type="button" ID="CT_ButtonLeave" class="btn btn-default" onclick="return cancelChangeRole()" style="width:100px; background-color:#6c757d; color:white;">Back</button> &nbsp; 
                                    <asp:Button ID="CT_ButtonChangeRole" runat="server" Text="Change Role" CssClass="btn btn-success" width="100px" OnClick="CT_ButtonChangeRole_Click"></asp:Button> 
                                </ContentTemplate> </asp:UpdatePanel>

                                <asp:UpdateProgress ID="AdduProgCHANGE" runat="server" AssociatedUpdatePanelID="UpdatePanelActionConfirm">
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
    <!-- End of Modal Change Role -->

    <!-- ##### Modal Activation User Role ##### -->
    <div class="modal fade" id="modalActivationRole" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 25px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label>ID UMS<asp:Label ID="AR_LabelticketID" runat="server" Text="Label"></asp:Label> - <asp:Label ID="AR_LabelUname" runat="server" Text="Label"></asp:Label> </label>                       
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <div class="text-left">

                    User has already mapped exactly as the request but is <b>INACTIVE</b>. <br />
                    Would you like to <b>ACTIVE</b> it?<br />

                    <asp:HiddenField ID="Hidden_AR_URID" runat="server" />
                    <asp:HiddenField ID="Hidden_AR_userID" runat="server" />
                    <asp:HiddenField ID="Hidden_AR_createBy" runat="server" />
                    <asp:HiddenField ID="Hidden_AR_createDate" runat="server" />
                    <asp:HiddenField ID="Hidden_AR_roleID" runat="server" />

                    </div>

                    <br />
                    
                     <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <asp:UpdatePanel ID="UpdatePanelValidasiStatus" runat="server"> <ContentTemplate>
                                <b> <p style="color: red; display: none" id="p_active" runat="server"> </p> </b> 
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                            <td style="width:50%; text-align:right;"> 
                                <asp:UpdatePanel ID="UpdatePanelActionStatus" runat="server"> <ContentTemplate>
                                    <button type="button" ID="AR_ButtonLeave" class="btn btn-default" onclick="return cancelStatus()" style="width:100px; background-color:#6c757d; color:white;">Back</button> &nbsp; 
                                    <asp:Button ID="AR_ButtonActivationRole" runat="server" Text="Active" CssClass="btn btn-success" width="100px" OnClick="AR_ButtonActivationRole_Click"></asp:Button> 
                                </ContentTemplate> </asp:UpdatePanel>

                                <asp:UpdateProgress ID="AdduProgSTATUS" runat="server" AssociatedUpdatePanelID="UpdatePanelActionStatus">
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
    <!-- End of Modal Activation User Role -->

     <!-- ##### Modal Already Exist ##### -->
    <div class="modal fade" id="modalAlreadyExist" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 25px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <%--penambahan label UMS00 pada ticket ID--%>
                        <label>ID UMS<asp:Label ID="AE_LabelticketID" runat="server" Text="Label"></asp:Label> - <asp:Label ID="AE_LabelUname" runat="server" Text="Label"></asp:Label> </label>                       
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <div class="text-left">
                    
                    <b>No approval needed.</b> User has already mapped exactly as the request. <br />
                    You may reject the ticket on the previous window <br />

                    </div>

                    <br />
                    
                     <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                &nbsp;
                            </td>
                            <td style="width:50%; text-align:right;"> 
                                <asp:UpdatePanel ID="UpdatePanelActionExist" runat="server"> <ContentTemplate>
                                    <button type="button" ID="AE_ButtonOK" class="btn btn-default" onclick="return cancelAlreadyExist()" style="width:100px; background-color:#6c757d; color:white;">Back</button> 
                                </ContentTemplate> </asp:UpdatePanel>

                                <asp:UpdateProgress ID="AdduProgEXIST" runat="server" AssociatedUpdatePanelID="UpdatePanelActionExist">
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
    <!-- End of Modal Already Exist -->

    <!-- ##### Modal cek master user isactive ##### -->
    <div class="modal fade" id="modalInfoActiveUSer" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 25px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <%--penambahan label UMS pada ticket ID--%>
                        <label>ID UMS<asp:Label ID="AU_LabelticketID" runat="server" Text="Label"></asp:Label> - <asp:Label ID="AU_LabelUname" runat="server" Text="Label"></asp:Label> </label>                       
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <div class="text-left">
                    
                    <b>The Username is not active.</b> Please check <b>User menu</b> to activate username manually. <br />

                    </div>

                    <br />
                    
                     <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                &nbsp;
                            </td>
                            <td style="width:50%; text-align:right;"> 
                                <asp:UpdatePanel ID="UpdatePanelInfoUser" runat="server"> <ContentTemplate>
                                    <button type="button" ID="AU_ButtonOK" class="btn btn-default" onclick="return cancelActiveUSer()" style="width:100px; background-color:#6c757d; color:white;">Back</button> 
                                </ContentTemplate> </asp:UpdatePanel>

                                <asp:UpdateProgress ID="AdduProgUSERACTIVE" runat="server" AssociatedUpdatePanelID="UpdatePanelInfoUser">
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
    <!-- End of Modal cek master user isactive -->

</asp:Content>
