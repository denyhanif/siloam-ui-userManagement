<%@ Page Title="User" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="UserMaster.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.UserMaster" enableEventValidation="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Script ----------------------------------------------------------------->

    <script type="text/javascript">

        //fungsi calendar
        function dateStart() {
            var dp = $('#<%=Add_TextBday.ClientID%>');
            dp.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd M yyyy",
                language: "tr"
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        }

        //fungsi calendar
        function dateStartEdit() {
            var dp = $('#<%=Edit_Textbday.ClientID%>');
            dp.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd M yyyy",
                language: "tr"
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        }

        //fungsi clear textbox bday dalam sekali keypress backspace
        function deleteText(evt, input)
        {
            evt = (evt) ? evt : window.event;
            var key = evt.keyCode || evt.charCode;
            if (key == 8 || key == 46)
            {
                if (input == "MainContent_Add_TextBday")
                {
                    document.getElementById('<%= Add_TextBday.ClientID %>').value = "";
                }
                else if (input == "MainContent_Edit_TextBday")
                {
                    document.getElementById('<%= Edit_Textbday.ClientID %>').value = "";
                }
            }
        }

        //fungsi untuk membatasi inputan hanya berupa number
        function isNumber(evt)
        {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keycode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
            {
                return false;
            }
            return true;
        }

        //fungsi untuk membatasi inputan hanya berupa regex
        function isUserName(evt)
        {
            evt = (evt) ? evt : window.event;
            var regex = new RegExp("^[a-zA-Z0-9_\.\\\\-]+$");
            var key = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
            if (!regex.test(key)) {
               evt.preventDefault();
               return false;
            }
            return true;
        }

        //function load data hope user id saat add data
        function LoadHOPE()
        {
            document.getElementById('<%= TextBoxDDLHope.ClientID %>').value = "unnamed";
            document.getElementById('<%= HiddenUserIdSelect.ClientID %>').value = "0";
            $('.CheckBoxSwitch').bootstrapToggle();
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal display
        function ViewDetails(username, fullname, email, bday, phone, hopeid, internal, ad, proint, lastlogin, exppassdate, lastmodifby, lockcounter, active) {
            $('#modalDisplayUser').modal('show');

            var Label_username = document.getElementById('<%= Label_username.ClientID %>');
            var Label_fullname = document.getElementById('<%= Label_fullname.ClientID %>');
            var Label_email = document.getElementById('<%= Label_email.ClientID %>');
            var Label_bday = document.getElementById('<%= Label_bday.ClientID %>');
            var Label_phone = document.getElementById('<%= Label_phone.ClientID %>');
            var Label_hopeid = document.getElementById('<%= Label_hopeid.ClientID %>');
            var Label_lastlogin = document.getElementById('<%= Label_lastlogin.ClientID %>');
            var Label_exppassdate = document.getElementById('<%= Label_exppassdate.ClientID %>');
            var Label_lastmodif = document.getElementById('<%= Label_lastmodified.ClientID %>');
            var Label_lock = document.getElementById('<%= Label_Locked.ClientID %>');
            var Label_active = document.getElementById('<%= Label_active.ClientID %>');
            var Label_usertype = document.getElementById('<%= Label_usertype.ClientID %>');
            var Hidden_userDetail = document.getElementById('<%= HiddenUsernameDetail.ClientID %>');

            Label_username.innerHTML = username;
            Label_fullname.innerHTML = fullname;
            Label_email.innerHTML = email;
            Label_bday.innerHTML = bday;
            Label_phone.innerHTML = phone;
            Label_hopeid.innerHTML = hopeid;
            Label_lastlogin.innerHTML = lastlogin;
            Label_exppassdate.innerHTML = exppassdate;
            Label_lastmodif.innerHTML = lastmodifby;
            Label_lock.innerHTML = lockcounter;
            Label_active.innerHTML = active;
            Hidden_userDetail.value = username;

            if (proint == "True")
            {
                Label_usertype.innerHTML = "ProInt";
            }
            if (ad == "True")
            {
                Label_usertype.innerHTML = "AD";
            }
            if (internal == "True" && proint == "False" && ad == "False")
            {
                Label_usertype.innerHTML = "Internal";
            }
            if (internal == "False")
            {
                Label_usertype.innerHTML = "External";
            }

            if (lockcounter >= 10) {
                Label_lock.innerHTML = "Locked";
                document.getElementById("spanLocked").style.backgroundColor = "#b85c5c";
                document.getElementById("tombolRelease").style.display = "block";
             }
            else {
                Label_lock.innerHTML = "Unlocked";
                document.getElementById("spanLocked").style.backgroundColor = "#5cb85c";
                document.getElementById("tombolRelease").style.display = "none";
             }

            if (active == "True") {
                Label_active.innerHTML = "Active";
                document.getElementById("spanActive").style.backgroundColor = "#5cb85c";
             }
            else {
                Label_active.innerHTML = "Inactive";
                document.getElementById("spanActive").style.backgroundColor = "#b85c5c";
             }

            //fungsi untuk mereplace karakter underscore(/_) menjadi spasi secara global(/g)
            Label_username.innerHTML = Label_username.innerHTML.toString().replace(/\+/g, "\\");
            Label_fullname.innerHTML = Label_fullname.innerHTML.toString().replace(/_/g, " ");
            Label_bday.innerHTML = Label_bday.innerHTML.toString().replace(/_/g, " ");
            Label_lastlogin.innerHTML = Label_lastlogin.innerHTML.toString().replace(/_/g, " ");
            Label_exppassdate.innerHTML = Label_exppassdate.innerHTML.toString().replace(/_/g, " ");
            Label_lastmodif.innerHTML = Label_lastmodif.innerHTML.toString().replace(/\+/g, "\\");
            Hidden_userDetail.value = Hidden_userDetail.value.toString().replace(/\+/g, "\\");

            //using moment.js untuk ubah format tanggal
            //Label_bday.innerHTML = moment(Label_bday.innerHTML, 'DD/MM/YYYY HH:mm:ss').format('DD MMM YYYY');

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal edit
        function EditDetails(userid, username, password, fullname, email, bday, phone, hopeid, internal, ad, proint, lastlogin, exppassdate, lcounter, createby, createdate, active) {           
            $('#modalEditUser').modal('show');

            var Edit_hidden_userid = document.getElementById('<%= Edit_HiddenUserID.ClientID %>');
            var Edit_hidden_username = document.getElementById('<%= Edit_HiddenUsername.ClientID %>');
            var Edit_hidden_hopeID = document.getElementById('<%= Edit_HiddenHopeUserID.ClientID %>');

            var Edit_usertype = document.getElementById('<%= Edit_Labelusertype.ClientID %>');
            var Edit_username = document.getElementById('<%= Edit_Labelusername.ClientID %>');
            var Edit_hopeid = document.getElementById('<%= Edit_HiddenUserIdSelect.ClientID %>');
            var Edit_fullname = document.getElementById('<%= Edit_Textfullname.ClientID %>');
            var Edit_email = document.getElementById('<%= Edit_Textemail.ClientID %>');
            var Edit_bday = document.getElementById('<%= Edit_Textbday.ClientID %>');
            var Edit_phone = document.getElementById('<%= Edit_Textphone.ClientID %>');

            var Edit_hidden_password = document.getElementById('<%= Edit_HiddenPassword.ClientID %>');
            var Edit_hidden_internal = document.getElementById('<%= Edit_HiddenInternal.ClientID %>');
            var Edit_hidden_ad = document.getElementById('<%= Edit_HiddenAD.ClientID %>');
            var Edit_hidden_proint = document.getElementById('<%= Edit_HiddenProint.ClientID %>');
            var Edit_hidden_lastlogin = document.getElementById('<%= Edit_HiddenLastLogin.ClientID %>');
            var Edit_hidden_exppassdate = document.getElementById('<%= Edit_HiddenExpPassDate.ClientID %>');
            var Edit_hidden_active = document.getElementById('<%= Edit_HiddenIsActive.ClientID %>');
            var Edit_hidden_lcounter = document.getElementById('<%= Edit_HiddenLcounter.ClientID %>');
            var Edit_hidden_createby = document.getElementById('<%= Edit_HiddenCreateby.ClientID %>');
            var Edit_hidden_createdate = document.getElementById('<%= Edit_HiddenCreatedate.ClientID %>');
            
            Edit_hidden_userid.value = userid;
            Edit_hidden_hopeID.value = hopeid;

            if (proint == "True")
            {
                Edit_usertype.innerHTML = "ProInt";
                Edit_fullname.disabled = true;
            }
            if (ad == "True")
            {
                Edit_usertype.innerHTML = "AD";
                Edit_fullname.disabled = true;
                Edit_email.disabled = true;
            }
            if (internal == "True" && proint == "False" && ad == "False")
            {
                Edit_usertype.innerHTML = "Internal";
            }
            if (internal == "False")
            {
                Edit_usertype.innerHTML = "External";
            }

            Edit_username.innerHTML = username;
            Edit_hidden_username.value = username;
            Edit_hopeid.value = hopeid;
            Edit_fullname.value = fullname; 
            Edit_email.value = email;
            Edit_bday.value = bday;
            Edit_phone.value = phone;

            Edit_hidden_password.value = password;
            Edit_hidden_internal.value = internal;
            Edit_hidden_ad.value = ad;
            Edit_hidden_proint.value = proint;
            Edit_hidden_lastlogin.value = lastlogin;
            Edit_hidden_exppassdate.value = exppassdate;
            Edit_hidden_active.value = active;
            Edit_hidden_lcounter.value = lcounter;
            Edit_hidden_createby.value = createby;
            Edit_hidden_createdate.value = createdate;

            //fungsi untuk mereplace karakter underscore(/_) menjadi spasi secara global(/g)
            Edit_username.innerHTML = Edit_username.innerHTML.toString().replace(/\+/g, "\\");
            Edit_hidden_username.value = Edit_hidden_username.value.toString().replace(/\+/g, "\\");
            Edit_fullname.value = Edit_fullname.value.toString().replace(/_/g, " ");
            Edit_bday.value = Edit_bday.value.toString().replace(/_/g, " ");
            Edit_hidden_createby.value = Edit_hidden_createby.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");
            Edit_hidden_createdate.value = Edit_hidden_createdate.value.toString().replace(/_/g, " ");
            Edit_hidden_lastlogin.value = Edit_hidden_lastlogin.value.toString().replace(/_/g, " ");
            Edit_hidden_exppassdate.value = Edit_hidden_exppassdate.value.toString().replace(/_/g, " ");

            //using moment.js untuk ubah format tanggal
            //Edit_bday.value = moment(Edit_bday.value, 'DD/MM/YYYY HH:mm:ss').format('DD MMM YYYY');

            document.getElementById('<%= Edit_ButtonLoadHopeID.ClientID %>').click();

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal update status tanpa menampilkan modal
        function UpdateStatus(userid, username, password, fullname, email, bday, phone, hopeid, internal, ad, proint, lastlogin, exppassdate, lcounter, createby, createdate, active) {     

            var hidden_userid_s = document.getElementById('<%= HideUserID.ClientID %>');
            var hidden_username_s = document.getElementById('<%= HideUserName.ClientID %>');

            var label_active_s = document.getElementById('<%= LabelStatusActive.ClientID %>');
            var label_username_s = document.getElementById('<%= LblUserName.ClientID %>');

            var hidden_password_s = document.getElementById('<%= HidePassword.ClientID %>');
            var hidden_fullname_s = document.getElementById('<%= HideFUllName.ClientID %>');
            var hidden_hopeid_s = document.getElementById('<%= HideHopeID.ClientID %>');
            var hidden_email_s = document.getElementById('<%= HideEmail.ClientID %>');
            var hidden_bday_s = document.getElementById('<%= HideBday.ClientID %>');
            var hidden_phone_s = document.getElementById('<%= HidePhone.ClientID %>');
            var hidden_lockcounter_s = document.getElementById('<%= HideLCounter.ClientID %>');
            var hidden_lastlogin_s = document.getElementById('<%= HideLastLogin.ClientID %>');
            var hidden_exppassdate_s = document.getElementById('<%= HideExpPassDate.ClientID %>');
            var hidden_internal_s = document.getElementById('<%= HideInternal.ClientID %>');
            var hidden_ad_s = document.getElementById('<%= HideAD.ClientID %>');
            var hidden_proint_s = document.getElementById('<%= HideProint.ClientID %>');
            var hidden_active_s = document.getElementById('<%= HideUserActive.ClientID %>');
            var hidden_createby_s = document.getElementById('<%= HideCreatby.ClientID %>');
            var hidden_createdate_s = document.getElementById('<%= HideCreatdate.ClientID %>');

             hidden_userid_s.value = userid;
             hidden_username_s.value = username;
             hidden_password_s.value = password;
             hidden_fullname_s.value = fullname;
             hidden_hopeid_s.value = hopeid;
             hidden_email_s.value = email;
             hidden_bday_s.value = bday;
             hidden_phone_s.value = phone;
             hidden_lockcounter_s.value = lcounter;
             hidden_lastlogin_s.value = lastlogin;
             hidden_exppassdate_s.value = exppassdate;
             hidden_internal_s.value = internal;
             hidden_ad_s.value = ad;
             hidden_proint_s.value = proint;

             label_username_s.innerHTML = username;

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
             label_username_s.innerHTML = label_username_s.innerHTML.toString().replace(/\+/g, "\\");
             hidden_username_s.value = hidden_username_s.value.toString().replace(/\+/g, "\\");
             hidden_fullname_s.value = hidden_fullname_s.value.toString().replace(/_/g, " ");
             hidden_createby_s.value = hidden_createby_s.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");
             hidden_createdate_s.value = hidden_createdate_s.value.toString().replace(/_/g, " ");
             hidden_bday_s.value = hidden_bday_s.value.toString().replace(/_/g, " ");
             hidden_lastlogin_s.value = hidden_lastlogin_s.value.toString().replace(/_/g, " ");
             hidden_exppassdate_s.value = hidden_exppassdate_s.value.toString().replace(/_/g, " ");

            //fungsi klik otomatis button update pada server side
            document.getElementById('<%= ButtonChangeStatus.ClientID %>').click();

            return false;
        }

        //fungsi untuk mendapatkan nilai dari halaman, lalu mengirimkannya ke modal reset password
        function ResetPassword(userid, username, password, fullname, email, bday, phone, hopeid, internal, ad, proint, lastlogin, exppassdate, lcounter, createby, createdate, active) {     
            $('#modalResetPass').modal('show');

            var hidden_userid_r = document.getElementById('<%= reset_userid.ClientID %>');
            var hidden_username_r = document.getElementById('<%= reset_username.ClientID %>');
            
            var label_username_r = document.getElementById('<%= reset_labelusername.ClientID %>');

            var hidden_password_r = document.getElementById('<%= reset_password.ClientID %>');
            var hidden_fullname_r = document.getElementById('<%= reset_fullname.ClientID %>');
            var hidden_hopeid_r = document.getElementById('<%= reset_hopeid.ClientID %>');
            var hidden_email_r = document.getElementById('<%= reset_email.ClientID %>');
            var hidden_bday_r = document.getElementById('<%= reset_bday.ClientID %>');
            var hidden_phone_r = document.getElementById('<%= reset_phone.ClientID %>');
            var hidden_lockcounter_r = document.getElementById('<%= reset_lcounter.ClientID %>');
            var hidden_lastlogin_r = document.getElementById('<%= reset_lastlogin.ClientID %>');
            var hidden_exppassdate_r = document.getElementById('<%= reset_exppassdate.ClientID %>');
            var hidden_internal_r = document.getElementById('<%= reset_internal.ClientID %>');
            var hidden_ad_r = document.getElementById('<%= reset_ad.ClientID %>');
            var hidden_proint_r = document.getElementById('<%= reset_proint.ClientID %>');
            var hidden_active_r = document.getElementById('<%= reset_isactive.ClientID %>');
            var hidden_createby_r = document.getElementById('<%= reset_createdby.ClientID %>');
            var hidden_createdate_r = document.getElementById('<%= reset_createddate.ClientID %>');

            hidden_userid_r.value = userid;
            hidden_username_r.value = username;

            label_username_r.innerHTML = username;

            hidden_password_r.value = password;
            hidden_fullname_r.value = fullname;
            hidden_hopeid_r.value = hopeid;
            hidden_email_r.value = email;
            hidden_bday_r.value = bday;
            hidden_phone_r.value = phone;
            hidden_lockcounter_r.value = lcounter;
            hidden_lastlogin_r.value = lastlogin;
            hidden_exppassdate_r.value = exppassdate;
            hidden_internal_r.value = internal;
            hidden_ad_r.value = ad;
            hidden_proint_r.value = proint;
            hidden_active_r.value = active;
            hidden_createby_r.value = createby;
            hidden_createdate_r.value = createdate;

            //fungsi untuk mereplace karakter underscore(/_) menjadi spasi secara global(/g)
            label_username_r.innerHTML = label_username_r.innerHTML.toString().replace(/\+/g, "\\");
            hidden_username_r.value = hidden_username_r.value.toString().replace(/\+/g, "\\");
            hidden_fullname_r.value = hidden_fullname_r.value.toString().replace(/_/g, " ");
            hidden_createby_r.value = hidden_createby_r.value.toString().replace(/_/g, " ").replace(/\+/g, "\\");
            hidden_createdate_r.value = hidden_createdate_r.value.toString().replace(/_/g, " ");
            hidden_bday_r.value = hidden_bday_r.value.toString().replace(/_/g, " ");
            hidden_lastlogin_r.value = hidden_lastlogin_r.value.toString().replace(/_/g, " ");
            hidden_exppassdate_r.value = hidden_expassdate_r.value.toString().replace(/_/g, " ");

            return false;
        }

        //fungsi untuk menampilkan data mapping user
        function ViewMapping(userid, username, fullname) {
            $('#modalDisplayMapping').modal('show');

            var Label_username = document.getElementById('<%= LabelUname.ClientID %>');
            var Label_fullname = document.getElementById('<%= LabelFname.ClientID %>');

            Label_username.innerHTML = username;
            Label_fullname.innerHTML = fullname;

            Label_username.innerHTML = Label_username.innerHTML.toString().replace(/\+/g, "\\");
            Label_fullname.innerHTML = Label_fullname.innerHTML.toString().replace(/_/g, " ");

            var hidden_userid_vm = document.getElementById('<%= HF_userid_viewmapping.ClientID %>');
            hidden_userid_vm.value = userid;

            document.getElementById('<%= ButtonViewMapping.ClientID %>').click();
            return false;
        }

        //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
        function AddFormCheck()
        {
            var DDL_userType = document.getElementById('<%= Add_DDLUserType.ClientID %>');

            if (DDL_userType.value == "1")  //ProInt
            {
                if ($("[id$='Add_TextUsername']").val().length == 0) {
                    $("[id$='Add_TextUsername']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Username cannot be empty!";

                    return false;
                }
                else if ($("[id$='Add_TextBday']").val().length == 0) {
                    $("[id$='Add_TextBday']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Birthday cannot be empty!";

                    return false;
                }
            }
            else if (DDL_userType.value == "2")  //AD
            {
                 if ($("[id$='Add_TextUsername']").val().length == 0) {
                    $("[id$='Add_TextUsername']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Username cannot be empty!";

                    return false;
                }
                else if ($("[id$='Add_TextFullname']").val().length == 0) {
                    $("[id$='Add_TextFullname']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Fullname cannot be empty!";

                    return false;
                }
                else if ($("[id$='Add_TextEmail']").val().length == 0) {
                    $("[id$='Add_TextEmail']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Email cannot be empty!";

                    return false;
                }
            }
            else if (DDL_userType.value == "3")  //Internal
            {
                if ($("[id$='Add_TextUsername']").val().length == 0) {
                    $("[id$='Add_TextUsername']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Username cannot be empty!";

                    return false;
                }
                else if ($("[id$='Add_TextFullname']").val().length == 0) {
                    $("[id$='Add_TextFullname']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Fullname cannot be empty!";

                    return false;
                }
            }
            else if (DDL_userType.value == "4")  //Eksternal
            {
                if ($("[id$='Add_TextUsername']").val().length == 0) {
                    $("[id$='Add_TextUsername']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Username cannot be empty!";

                    return false;
                }
                else if ($("[id$='Add_TextFullname']").val().length == 0) {
                    $("[id$='Add_TextFullname']").focus();
                    $("[id$='p_Add']").removeAttr("style");
                    $("[id$='p_Add']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Fullname cannot be empty!";

                    return false;
                }
            }
            else if (DDL_userType.value == "-1")  //no select
            {
                DDL_userType.focus();
                $("[id$='p_Add']").removeAttr("style");
                $("[id$='p_Add']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Please Select User Type First!";

                return false;
            }

            var uname = document.getElementById("<%= Add_TextUsername.ClientID %>").value;
            var fname = document.getElementById("<%= Add_TextFullname.ClientID %>").value;
            var ename = document.getElementById("<%= Add_TextEmail.ClientID %>").value;

            if (uname.indexOf('\'') > -1 || uname.indexOf('"') > -1 || fname.indexOf('\'') > -1 || fname.indexOf('"') > -1 || ename.indexOf('\'') > -1 || ename.indexOf('"') > -1) {
                $("[id$='p_Add']").removeAttr("style");
                $("[id$='p_Add']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Add.ClientID %>').innerHTML = "Single or Double Quotes Found, Please Remove!";

                return false;
            }
        }

        //fungsi  untuk validasi input text kosong, lalu menampilkan notifikasi text berwarna merah
        function EditFormCheck()
        {
            var Label_userType = document.getElementById('<%= Edit_Labelusertype.ClientID %>');

            if (Label_userType.innerHTML == "ProInt")
            {
                if ($("[id$='Edit_Textfullname']").val().length == 0) {
                    $("[id$='Edit_Textfullname']").focus();
                    $("[id$='p_Edit']").removeAttr("style");
                    $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Fullname cannot be empty!";

                    return false;
                }
                else if ($("[id$='Edit_Textbday']").val().length == 0)
                {
                    $("[id$='Edit_Textbday']").focus();
                    $("[id$='p_Edit']").removeAttr("style");
                    $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Birthday cannot be empty!";

                    return false;
                }
            }
            else if (Label_userType.innerHTML == "AD")
            {
                if ($("[id$='Edit_Textfullname']").val().length == 0)
                {
                    $("[id$='Edit_Textfullname']").focus();
                    $("[id$='p_Edit']").removeAttr("style");
                    $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Fullname cannot be empty!";

                    return false;
                }
                else if ($("[id$='Edit_Textemail']").val().length == 0)
                {
                    $("[id$='Edit_Textemail']").focus();
                    $("[id$='p_Edit']").removeAttr("style");
                    $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Email cannot be empty!";

                    return false;
                }
            }
            else if (Label_userType.innerHTML == "Internal")
            {
                if ($("[id$='Edit_Textfullname']").val().length == 0) {
                    $("[id$='Edit_Textfullname']").focus();
                    $("[id$='p_Edit']").removeAttr("style");
                    $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Fullname cannot be empty!";

                    return false;
                }
            }
            else if (Label_userType.innerHTML == "Eksternal")
            {
                if ($("[id$='Edit_Textfullname']").val().length == 0) {
                    $("[id$='Edit_Textfullname']").focus();
                    $("[id$='p_Edit']").removeAttr("style");
                    $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                    document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Fullname cannot be empty!";

                    return false;
                }
            }

            var fname = document.getElementById("<%= Edit_Textfullname.ClientID %>").value;
            var ename = document.getElementById("<%= Edit_Textemail.ClientID %>").value;

            if (fname.indexOf('\'') > -1 || fname.indexOf('"') > -1 || ename.indexOf('\'') > -1 || ename.indexOf('"') > -1) {
                $("[id$='p_Edit']").removeAttr("style");
                $("[id$='p_Edit']").attr("style", "display:block; color:red;");
                document.getElementById('<%= p_Edit.ClientID %>').innerHTML = "Single or Double Quotes Found, Please Remove!";

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

            document.getElementById('<%= Add_TextUsername.ClientID %>').value = "";
            document.getElementById('<%= Add_TextFullname.ClientID %>').value = "";
            document.getElementById('<%= Add_TextEmail.ClientID %>').value = "";
            document.getElementById('<%= Add_TextPhone.ClientID %>').value = "";
            document.getElementById('<%= Add_TextBday.ClientID %>').value = "";

            document.getElementById('<%= Add_TextFullname.ClientID %>').disabled = false;
            document.getElementById('<%= Add_TextEmail.ClientID %>').disabled = false;

            document.getElementById('<%= Add_DDLUserType.ClientID %>').value = "-1";
        }

        //fungsi event klik pada area diluar modal
        $(document).ready(function () { 
            $('#modalEditUser').on('hidden.bs.modal', function (e) {
                 resetModalForm();
             });

            $('#modalAddUser').on('hidden.bs.modal', function (e) {
                 resetModalForm();
            });

            //fungsi untuk menjaga style saat postback dalam updatepanel
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

        //fungsi toggle DDL hope user - ADD
        function showGridUserHope()
        {
            var grid = document.getElementById("GridHopeUser");
            if (grid.style.display === "none")
            {
                grid.style.display = "block";
                document.getElementById('<%= Add_TextDDLHopeID.ClientID %>').focus();
            }
            else
            {
                grid.style.display = "none";
            }
        }

        //fungsi toggle DDL hope user - EDIT
        function showGridUserHope_Edit()
        {
            var grid = document.getElementById("Edit_GridHopeUser");
            if (grid.style.display === "none")
            {
                grid.style.display = "block";
                document.getElementById('<%= Edit_TextDDLHopeID.ClientID %>').focus();
            }
            else
            {
                grid.style.display = "none";
            }
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

        function hideAddUser()
        {
            $("[id$='add_tomboladduser']").attr("style", "display:none;");
        }

        function hideEditUser()
        {
            var x = document.getElementsByClassName('flagedit');
            if (x.length > 0)
            {
                var i;
                for (i = 0; i < x.length; i++)
                {
                    x[i].style.display = "none"
                }
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
                <div class="col-sm-6 TeksHeader" style="padding-top: 5px;"><b style="float:left">Manage User &nbsp; </b>
                    
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePaneDataUser">
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
                            <span class="fa fa-search form-control-feedback searchIconAnimation" ></span>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanelCari" runat="server"> <ContentTemplate>
                            <div hidden> <asp:Button ID="ButtonCari" runat="server" Text="Button" OnClick="ButtonCari_Click" /> </div>
                            <asp:HiddenField ID="HiddenFlagCari" runat="server" />
                        </ContentTemplate> </asp:UpdatePanel>
                        <button id="add_tomboladduser" type="button" class="TombolAddData btn btn-primary TeksNormal" onclick="return LoadHOPE()" data-toggle="modal" data-target="#modalAddUser"><i class="fa fa-plus"></i>&nbsp; Add User</button>
                    </div>

                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePaneDataUser" runat="server" UpdateMode="Always"> <ContentTemplate>

            <asp:GridView ID="GridViewUserSimple" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover TeksNormal"
                AllowPaging="True" PageSize="9" OnPageIndexChanging="GridViewUserSimple_PageIndexChanging" ShowHeaderWhenEmpty="True"
                DataKeyNames="user_id" EmptyDataText="No Data" BorderWidth="0">
                <PagerStyle CssClass="pagination-ys" />
                <Columns>
                    <asp:BoundField HeaderText="Username" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" DataField="user_name" SortExpression="user_name" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="Full Name" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left" DataField="full_name" SortExpression="full_name" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>
                    <asp:BoundField HeaderText="Email" HeaderStyle-CssClass="TeksHeaderTable" ItemStyle-Width="22%" ItemStyle-HorizontalAlign="Left" DataField="email" SortExpression="email" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0"></asp:BoundField>

                    <asp:TemplateField HeaderText="Password" ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <a href="#" style="text-decoration: none;" title="Reset" onclick=<%# "ResetPassword('" + Eval("user_id") + "','" + Eval("user_name").ToString().Replace("\\","+")  + "','" + Eval("password")  + "','" + Eval("full_name").ToString().Replace(" ","_") +  "','"  + Eval("email")  + "','" + Eval("birthday").ToString().Replace(" ","_")  + "','" + Eval("handphone")  + "','" + Eval("hope_user_id")  + "','" + Eval("is_internal")  + "','" + Eval("is_ad")  + "','" + Eval("is_proint")  + "','" + Eval("last_login_date").ToString().Replace(" ","_") + "','" + Eval("exp_pass_date").ToString().Replace(" ","_")  + "','" + Eval("lock_counter")  + "','"+ Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>> Reset </a>                         
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Is Active" ItemStyle-Width="10%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <input id="CheckBoxStatus" class="CheckBoxSwitch" type="checkbox" <%# Eval("is_active").ToString() == "True" ? "checked" : "unchecked"%> data-toggle="toggle" data-on="Active" data-off="Inactive" data-onstyle="success" data-offstyle="default" data-size="mini" onchange=<%# "UpdateStatus('" + Eval("user_id") + "','" + Eval("user_name").ToString().Replace("\\","+")  + "','" + Eval("password")  + "','" + Eval("full_name").ToString().Replace(" ","_") +  "','"  + Eval("email")  + "','" + Eval("birthday").ToString().Replace(" ","_")  + "','" + Eval("handphone")  + "','" + Eval("hope_user_id")  + "','" + Eval("is_internal")  + "','" + Eval("is_ad")  + "','" + Eval("is_proint")  + "','" + Eval("last_login_date").ToString().Replace(" ","_") + "','" + Eval("exp_pass_date").ToString().Replace(" ","_")  + "','" + Eval("lock_counter")  + "','"+ Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="13%" HeaderStyle-ForeColor="#1a2269" ItemStyle-HorizontalAlign="center" ItemStyle-BorderWidth="0" HeaderStyle-BorderWidth="0" HeaderStyle-CssClass="text-center TeksHeaderTable">
                        <ItemTemplate>
                            <a href="#" style="text-decoration: none;" title="Edit" class="flagedit" onclick=<%# "EditDetails('" + Eval("user_id") + "','" + Eval("user_name").ToString().Replace("\\","+")  + "','" + Eval("password")  + "','" + Eval("full_name").ToString().Replace(" ","_") +  "','"  + Eval("email")  + "','" + Eval("birthday", "{0:dd MMM yyyy}").ToString().Replace(" ","_")  + "','" + Eval("handphone")  + "','" + Eval("hope_user_id")  + "','" + Eval("is_internal")  + "','" + Eval("is_ad")  + "','" + Eval("is_proint")  + "','" + Eval("last_login_date").ToString().Replace(" ","_") + "','" + Eval("exp_pass_date").ToString().Replace(" ","_")  + "','" + Eval("lock_counter")  + "','"+ Eval("created_by").ToString().Replace(" ","_").Replace("\\","+")  + "','" + Eval("created_date").ToString().Replace(" ","_")  + "','" + Eval("is_active") + "')" %>><asp:Image ID="ImageEdit" ImageUrl="~/Assets/Icons/ic_Edit.svg" CssClass="ic_Edit" runat="server" /></a>&nbsp;&nbsp;
                            <a href="#" style="text-decoration: none;" title="Authorization" onclick=<%# "ViewMapping('" + Eval("user_id") + "','" + Eval("user_name").ToString().Replace("\\","+") + "','" + Eval("full_name").ToString().Replace(" ","_") +  "')" %>><asp:Image ID="ImageMapping" ImageUrl="~/Assets/Icons/ic_mapping.svg" CssClass="ic_Mapping" runat="server" /></i></a>&nbsp;&nbsp;
                            <a href="#" style="text-decoration: none;" title="Detail" onclick=<%# "ViewDetails('" + Eval("user_name").ToString().Replace("\\","+") + "','" + Eval("full_name").ToString().Replace(" ","_") +  "','" + Eval("email")  + "','" + Eval("birthday", "{0:dd MMM yyyy}").ToString().Replace(" ","_")  + "','" + Eval("handphone")  + "','" + Eval("hope_user_id")  + "','" + Eval("is_internal")  + "','" + Eval("is_ad")  + "','" + Eval("is_proint")  + "','" + Eval("last_login_date").ToString().Replace(" ","_") + "','" + Eval("exp_pass_date").ToString().Replace(" ","_")  + "','" + Eval("modified_by").ToString().Replace("\\","+").Replace(" ","_") + "','" + Eval("lock_counter")  + "','" +  Eval("is_active") + "')" %>><asp:Image ID="ImageDetail" ImageUrl='<%# int.Parse(Eval("lock_counter").ToString()) >= 10 ? "~/Assets/Icons/ic_Locked.svg" : "~/Assets/Icons/ic_Detail.svg" %>' CssClass="ic_Detail" runat="server" /></i></a>&nbsp;
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                <asp:HiddenField ID="HFFlagEdit" runat="server" />

            </ContentTemplate> </asp:UpdatePanel>

        </div>
    </div>

    <!--###########################################################################################################################################-->
    <!------------------------------------------------------------------ The Modal ------------------------------------------------------------------>

    <!-- ##### Modal Edit User ##### -->
    <div class="modal fade" id="modalEditUser" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 25px;" data-keyboard="false">
       
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label> Edit User </label>
                    </h4>
                </div>

                <div class="titleHeaderBlock">
                    <table border="0" style="width:100%;">
                        <tr>
                            <td style="width:30%"> <b> User Type :  </b> <asp:Label ID="Edit_Labelusertype" runat="server"></asp:Label> </td>
                            <td> &nbsp; </td>
                            <td> <b> Username : </b>  <asp:Label ID="Edit_Labelusername" runat="server"></asp:Label> <asp:Button ID="ButtonPass" runat="server" style="display:none;" Text="Password" OnClick="ButtonPass_Click" /> </td>
                        </tr>
                    </table>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel ini berfungsi untuk menjaga item didalamnya tidak terrefresh oleh postback dari luar update panel ini --> 
                    <asp:UpdatePanel ID="UpdatePanelEDITuser" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <asp:HiddenField ID="Edit_HiddenUserID" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenUsername" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenHopeUserID" runat="server" />

                    <%--untuk load data hope saat proses tambah data--%>
                    <div hidden><asp:Button ID="Edit_ButtonLoadHopeID" runat="server" Text="Button" OnClick="Edit_ButtonLoadHopeID_Click" /></div>
                    
                    <div class="form-group">
                    Hope User ID                
                        <asp:UpdatePanel ID="Edit_UpdatePanelTexBoxDDL" runat="server"> <ContentTemplate>

                        <div class="has-feedback">
                        <asp:TextBox ID="Edit_TextBoxDDLHope" runat="server" CssClass="MaxWidthTextbox form-control texttoDark" Enabled="true" onclick="return showGridUserHope_Edit()" placeholder="Nothing selected"></asp:TextBox>
                        <span class="fa fa-sort-down form-control-feedback" style="padding-right:0px; color:darkgrey;"></span>
                        </div>

                        </ContentTemplate> </asp:UpdatePanel>

                        <div id="Edit_GridHopeUser" style="display:none; z-index:100; position:absolute; width:94%;" >
                        <div class="BoxOfCustomDropdown">

                            <asp:UpdatePanel ID="Edit_SideGrid" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                            <div class="form-inline">
                            <asp:TextBox ID="Edit_TextDDLHopeID" runat="server" CssClass="MaxWidthTextbox form-control" style="width:400px" AutoCompleteType="Disabled" OnTextChanged="Edit_SearchHopeUser_Click" AutoPostBack="true"></asp:TextBox>
                            <asp:Button ID="Edit_SearchHopeUser" Style="float:right;" runat="server" CssClass="btn btn-default" Text="Find" OnClick="Edit_SearchHopeUser_Click"/>
                            </div>

                            <div style="height: 200px; overflow: scroll;">
                                <!-- elemen untuk menyimpan sementara value row grid yg diklik -->
                                <asp:HiddenField ID="Edit_HiddenRowSelect" runat="server"/>
                                <asp:HiddenField ID="Edit_HiddenUserIdSelect" runat="server"/>

                                <asp:HiddenField ID="Edit_TempUserHopeID" runat="server"/>
                                <asp:HiddenField ID="Edit_TempUserHopeUsername" runat="server"/>

                                <asp:GridView ID="Edit_GridViewUserHope" runat="server" AutoGenerateColumns="False" CssClass="table small"
                                    HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center"
                                    ShowHeaderWhenEmpty="True" DataKeyNames="userId" EmptyDataText="No Data" BorderWidth="0">
                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="hidden" ItemStyle-Width="100%" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="boxShadow" HeaderStyle-BorderWidth="0">
                                            <ItemTemplate>
                                                <asp:Button ID="Edit_LinkListUserHope" CssClass="ExLinkButton" runat="server" OnClientClick="return showGridUserHope_Edit()" OnClick="Edit_LinkListUserHope_Click" Text='<%# Eval("userName")%>'></asp:Button>
                                                <asp:HiddenField ID="Edit_HiddenFieldUserHopeID" Value='<%# Eval("userId")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            </ContentTemplate> </asp:UpdatePanel>

                        </div>
                        </div>

                    </div>

                    <div class="form-group">
                    Full Name 
                    <asp:TextBox ID="Edit_Textfullname" runat="server" Enabled="true" CssClass="MaxWidthTextbox form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                    Email 
                    <asp:TextBox ID="Edit_Textemail" runat="server" CssClass="MaxWidthTextbox form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                    Phone
                    <asp:TextBox ID="Edit_Textphone" runat="server" CssClass="form-control" Style="width:40%;"></asp:TextBox>
                    </div>

                    <div class="form-group">
                    Birthday 
                    <asp:TextBox ID="Edit_Textbday" runat="server" CssClass="form-control" placeholder="dd MMM yyyy" onmousedown="dateStartEdit();" onkeyup="deleteText(event, this.id)" Style="width:40%;" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>

                    <div class="form-group">
                    <div style="font-weight:bold; padding-bottom:5px;">Nomor Surat Izin Praktek Dokter</div>
                    <table border="0" style="width:100%">
                        <tr>
                            <td>Hospital Unit</td>
                            <td>&nbsp;</td>
                            <td>No SIP</td>
                            <td>&nbsp;</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width:40%;">
                                <asp:DropDownList ID="ddlEditUnitSIP" name="Edit_DDLUnitSIP" runat="server" CssClass="form-control" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td>        
                                <asp:TextBox ID="txtEditUnitSIP" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btnAddEditUnitSIP" runat="server" Text="Add" class="btn btn-primary" OnClick="btnAddEditUnitSIP_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    </div> 
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvw_EditUnitSIP" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-condensed"
                                    DataKeyNames="doctor_sip_id" EmptyDataText="Tidak ada data SIP" BorderWidth="1" HeaderStyle-BorderWidth="1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Hospital Unit" HeaderStyle-ForeColor="#2a3593" ItemStyle-Width="50%" HeaderStyle-Font-Size="11px" ItemStyle-BorderWidth="1" HeaderStyle-BorderWidth="1">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="doctor_sip_id" runat="server" Value='<%# Bind("doctor_sip_id") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="user_id" runat="server" Value='<%# Bind("user_id") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="organization_id" runat="server" Value='<%# Bind("organization_id") %>'></asp:HiddenField>
                                                <asp:Label Font-Size="11px" Font-Names="Helvetica, Arial, sans-serif" ID="organization_name" runat="server" Text='<%# Bind("organization_name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No SIP" HeaderStyle-ForeColor="#2a3593" ItemStyle-Width="40%" HeaderStyle-Font-Size="11px" ItemStyle-BorderWidth="1" HeaderStyle-BorderWidth="1">
                                            <ItemTemplate>
                                                <asp:Label Font-Size="11px" Font-Names="Helvetica, Arial, sans-serif" ID="sip_no" runat="server" Text='<%# Bind("sip_no") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="10%" HeaderStyle-Font-Size="11px" ItemStyle-BorderWidth="1" HeaderStyle-BorderWidth="1" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/Assets/Icons/ic_hapus.svg" OnClick ="btndeleteedit_Click" Style="width: 12px; height: 12px; margin-top: 3px;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
                         </div>
                    </div>
                    <br />

                    <asp:HiddenField ID="Edit_HiddenPassword" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenInternal" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenAD" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenProint" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenLastLogin" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenExpPassDate" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenLcounter" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenCreateby" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenCreatedate" runat="server" />
                    <asp:HiddenField ID="Edit_HiddenIsActive" runat="server" />

                    <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <!-- loading gif untuk menunggu respon proses pada akses ke server side pada update panel tertunjuk -->
                                <asp:UpdateProgress ID="uProgUsernameEdit" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelEDITuser">
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
                                    <asp:Button ID="Edit_ButtonSaveUser" runat="server" Text="Save & Close" class="btn btn-success" OnClientClick="return EditFormCheck()" OnClick="Edit_ButtonSaveUser_Click"></asp:Button>
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
    <!-- End Of Modal Edit User -->
    
    <!-- ##### Modal Add User ##### -->
    <div class="modal fade" id="modalAddUser" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 25px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="return resetModalForm()">×</button>
                    <h4 class="modal-title">
                        <label> Add User </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    
                    <asp:UpdatePanel ID="UpdatePanelADDuser" runat="server" UpdateMode="Conditional"> <ContentTemplate>
    
                    <div class="form-group">
                    <table border="0" style="width:100%">
                        <tr>
                            <td>User Type</td>
                            <td>&nbsp;</td>
                            <td>Username <asp:Label ID="Add_LabelDomainUsername" Style="margin-left:77px; color:#7b88ff;" runat="server" Text="Label" Visible="false"> SILOAMHOSPITALS\ </asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width:40%;">
                                <asp:DropDownList ID="Add_DDLUserType" name="Add_DDLUserType" runat="server" CssClass="form-control" OnSelectedIndexChanged="Add_DDLUserType_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Enabled="true" Text="--Select Type--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="ProInt" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="AD" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Internal" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Eksternal" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td>        
                                <asp:TextBox ID="Add_TextUsername" runat="server" CssClass="form-control" onkeypress="return isUserName(event)" OnTextChanged="Add_TextUsername_TextChanged" AutoPostBack="true"></asp:TextBox>                                
                            </td>
                        </tr>
                    </table>
                    </div> 
                    
                    <div class="form-group">
                    Hope User ID
                        <asp:UpdatePanel ID="UpdatePanelTexBoxDDL" runat="server"> <ContentTemplate>

                        <div class="has-feedback">
                        <asp:TextBox ID="TextBoxDDLHope" runat="server" CssClass="MaxWidthTextbox form-control texttoDark" ReadOnly="true" Style="background-color:white !important;" onclick="return showGridUserHope()" placeholder="Nothing selected"></asp:TextBox>
                        <span class="fa fa-sort-down form-control-feedback" style="padding-right:0px; color:darkgrey;"></span>
                        </div>

                        </ContentTemplate> </asp:UpdatePanel>

                        <div id="GridHopeUser" style="display:none; z-index:100; position:absolute; width:94%;" >
                        <div style="border:1px; border-radius: 5px 5px; padding:5px; background-color:#f5f5f5; box-shadow: 0px 1px 8px darkgrey;">

                            <asp:UpdatePanel ID="SideGrid" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                            <div class="form-inline">
                            <asp:TextBox ID="Add_TextDDLHopeID" runat="server" CssClass="MaxWidthTextbox form-control" style="width:400px" AutoCompleteType="Disabled" OnTextChanged="SearchHopeUser_Click" AutoPostBack="true"></asp:TextBox>
                            <asp:Button ID="SearchHopeUser" Style="float:right;" runat="server" CssClass="btn btn-default" Text="Find" OnClick="SearchHopeUser_Click"/>
                            </div>

                            <div style="height: 200px; overflow: scroll;">
                                <!-- elemen untuk menyimpan sementara value row grid application yg diklik -->
                                <asp:HiddenField ID="HiddenRowSelect" runat="server"/>
                                <asp:HiddenField ID="HiddenUserIdSelect" runat="server"/>
                                <asp:HiddenField ID="HiddenUsernameSelect" runat="server"/>

                                <asp:GridView ID="GridViewUserHope" runat="server" AutoGenerateColumns="False" CssClass="table small"
                                    HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center"
                                    ShowHeaderWhenEmpty="True" DataKeyNames="userId" EmptyDataText="No Data" BorderWidth="0">
                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="hidden" ItemStyle-Width="100%" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="boxShadow" HeaderStyle-BorderWidth="0">
                                            <ItemTemplate>
                                                <asp:Button ID="LinkListUserHope" CssClass="ExLinkButton" runat="server" OnClientClick="return showGridUserHope()" OnClick="LinkListUserHope_Click" Text='<%# Eval("userName")%>'></asp:Button>
                                                <asp:HiddenField ID="HiddenFieldUserHopeID" Value='<%# Eval("userId")%>' runat="server" />
                                                <asp:HiddenField ID="HiddenFieldUserHopename" Value='<%# Eval("name")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            </ContentTemplate> </asp:UpdatePanel>

                        </div>
                        </div>
                    </div>

                    <asp:UpdatePanel ID="UpdatePanelFormAdd" runat="server"> <ContentTemplate>
                    <div class="form-group">
                    Full Name 
                    <asp:TextBox ID="Add_TextFullname" runat="server" CssClass="MaxWidthTextbox form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                    Email 
                    <asp:TextBox ID="Add_TextEmail" runat="server" CssClass="MaxWidthTextbox form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                    Phone 
                    <asp:TextBox ID="Add_TextPhone" runat="server" CssClass="form-control"  Style="width:40%;" onkeypress="return isNumber(event)"></asp:TextBox>
                    </div>

                    <div class="form-group">
                    Birthday <asp:Label ID="Add_LabelBintangBday" runat="server" Text="Label" Visible="false" Style="color:red;"> * </asp:Label>
                    <asp:TextBox ID="Add_TextBday" runat="server" CssClass="form-control" placeholder="dd MMM yyyy" onmousedown="dateStart();" onkeyup="deleteText(event, this.id)" Style="width:40%;" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>

                    <div class="form-group">
                    <div style="font-weight:bold; padding-bottom:5px;">Nomor Surat Izin Praktek Dokter</div>
                    <table border="0" style="width:100%">
                        <tr>
                            <td>Hospital Unit</td>
                            <td>&nbsp;</td>
                            <td>No SIP</td>
                            <td>&nbsp;</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width:40%;">
                                <asp:DropDownList ID="ddlAddUnitSIP" name="Add_DDLUnitSIP" runat="server" CssClass="form-control" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td>        
                                <asp:TextBox ID="txtAddUnitSIP" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>                                
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btnAddUnitSIP" runat="server" Text="Add" class="btn btn-primary" OnClick="btnAddUnitSIP_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    </div> 
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvw_AddUnitSIP" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-condensed"
                                    DataKeyNames="doctor_sip_id" EmptyDataText="Tidak ada data SIP" BorderWidth="1" HeaderStyle-BorderWidth="1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Hospital Unit" HeaderStyle-ForeColor="#2a3593" ItemStyle-Width="50%" HeaderStyle-Font-Size="11px" ItemStyle-BorderWidth="1" HeaderStyle-BorderWidth="1">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="doctor_sip_id" runat="server" Value='<%# Bind("doctor_sip_id") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="user_id" runat="server" Value='<%# Bind("user_id") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="organization_id" runat="server" Value='<%# Bind("organization_id") %>'></asp:HiddenField>
                                                <asp:Label Font-Size="11px" Font-Names="Helvetica, Arial, sans-serif" ID="organization_name" runat="server" Text='<%# Bind("organization_name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No SIP" HeaderStyle-ForeColor="#2a3593" ItemStyle-Width="40%" HeaderStyle-Font-Size="11px" ItemStyle-BorderWidth="1" HeaderStyle-BorderWidth="1">
                                            <ItemTemplate>
                                                <asp:Label Font-Size="11px" Font-Names="Helvetica, Arial, sans-serif" ID="sip_no" runat="server" Text='<%# Bind("sip_no") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="10%" HeaderStyle-Font-Size="11px" ItemStyle-BorderWidth="1" HeaderStyle-BorderWidth="1"  ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/Assets/Icons/ic_hapus.svg" OnClick ="btndelete_Click" Style="width: 12px; height: 12px; margin-top: 3px;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
                         </div>
                    </div>
                    <br />
                    </ContentTemplate></asp:UpdatePanel>

                    <table border="0" style="width:100%">
                        <tr>
                            <td> 
                                <asp:UpdateProgress ID="uProgUsernameADD" style="float:left;" runat="server" AssociatedUpdatePanelID="UpdatePanelADDuser">
                                    <ProgressTemplate>
                                        <img alt="" height="25px" width="25px" style="background-color:transparent; vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress> 

                                <asp:UpdatePanel ID="UpdatePanelExist" runat="server"> <ContentTemplate>
                                <b> <p style="color: red; display: none" id="p_Add" runat="server"> </p> </b> 
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                            <td style="width:30%; text-align:right;"> 
                                <asp:UpdatePanel ID="UpdatePanelSAVEadd" runat="server"> <ContentTemplate>
                                    <asp:Button ID="Add_ButtonSaveUser" runat="server" Text="Save & Close" class="btn btn-success" OnClientClick="return AddFormCheck()" OnClick="Add_ButtonSaveUser_Click"></asp:Button>
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
                    
                    </ContentTemplate>  </asp:UpdatePanel>
                </div>
            </div>
        </div>    
    </div>
     <!-- End of Modal Add User -->

    <!-- ##### Modal Display ##### User -->
    <div class="modal fade" id="modalDisplayUser" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">×</button>
                    <h4 class="modal-title">
                        <label> Detail User </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body text-center">

                    <table class="table table-striped text-left" style="margin-top:-15px;">
                        <tr>
                            <td style="border-width:0;"><b>Fullname </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_fullname" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Username </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_username" runat="server" Text="Label"></asp:Label>
                                <asp:HiddenField ID="HiddenUsernameDetail" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Email </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_email" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Hope User ID </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_hopeid" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Phone </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_phone" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Date of Birth </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_bday" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>User Type </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_usertype" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Last Login Date </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_lastlogin" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Expired Password Date </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_exppassdate" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Last Modified Data By </b></td>
                            <td style="border-width:0;">
                                <asp:Label ID="Label_lastmodified" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>Is Active </b></td>
                            <td style="border-width:0;">
                                <span id="spanActive" class="badge" style="background-color:white; width: 65px">
                                <asp:Label ID="Label_active" runat="server" Text="Label"></asp:Label> </span></td>  
                        </tr>
                        <tr>
                            <td style="border-width:0;"><b>User Locked </b></td>
                            <td style="border-width:0;">
                                <asp:UpdatePanel ID="UpdatePanelDISPLAYuser" runat="server"> <ContentTemplate>
                                <span id="spanLocked" class="badge" style="background-color:#5cb85c; width: 65px">
                                <asp:Label ID="Label_Locked" runat="server" Text="Label"></asp:Label> </span>
                                <div id="tombolRelease" style="float:right;">
                                    <asp:Button ID="Button_Release" runat="server" CssClass="btn btn-primary" style="height:20px; padding-top: 0px;" Text="Release" OnClick="Button_Release_Click" />
                                </div>
                                </ContentTemplate> </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
        
    </div>
    <!-- End Of Modal Display User -->

    <!-- ##### Modal Update Status User ##### -->
    <div class="modal fade" id="modalUpdateStatus" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;">

        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="javascript:window.location.reload()">×</button> 
                    <h4 class="modal-title">
                        <label>Update Status <asp:Label ID="LblUserName" runat="server" Text="Label"></asp:Label> </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <!-- update panel dipasang agar saat button update status diklik via javascript, page tidak terefresh -->
                    <asp:UpdatePanel ID="UpdatePanelSTATUSuser" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="text-center">
                        <asp:HiddenField ID="HideUserID" runat="server" />
                        <asp:HiddenField ID="HideUserActive" runat="server" />
 
                        Are you sure to update the status to <b> <asp:Label ID="LabelStatusActive" runat="server" Text="Label"></asp:Label> </b> ? <br /><br />

                        <asp:HiddenField ID="HideUserName" runat="server" />
                        <asp:HiddenField ID="HidePassword" runat="server" />
                        <asp:HiddenField ID="HideFUllName" runat="server" />
                        <asp:HiddenField ID="HideHopeID" runat="server" />
                        <asp:HiddenField ID="HideEmail" runat="server" />
                        <asp:HiddenField ID="HideBday" runat="server" />
                        <asp:HiddenField ID="HidePhone" runat="server" />
                        <asp:HiddenField ID="HideLCounter" runat="server" />
                        <asp:HiddenField ID="HideLastLogin" runat="server" />
                        <asp:HiddenField ID="HideExpPassDate" runat="server" />
                        <asp:HiddenField ID="HideInternal" runat="server" />
                        <asp:HiddenField ID="HideAD" runat="server" />
                        <asp:HiddenField ID="HideProint" runat="server" />
                        <asp:HiddenField ID="HideCreatby" runat="server" />
                        <asp:HiddenField ID="HideCreatdate" runat="server" />

                        <asp:Button ID="ButtonChangeStatus" runat="server" Text="Save & Close" class="btn btn-success" OnClick="ButtonChangeStatus_Click"></asp:Button>
                    </div>

                    </ContentTemplate> </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Update Status User -->

    <!-- ##### Modal Reset Password User ##### -->
    <div class="modal fade" id="modalResetPass" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
      
        <!-- loading progress modal -->
        <asp:UpdateProgress ID="uProgReset" runat="server" AssociatedUpdatePanelID="UpdatePanelResetpass">
            <ProgressTemplate>
                <div class="modal-backdrop" style="background-color:transparent; margin-top:15%;text-align:center">
                    <img alt="" height="200px" width="200px" style="background-color:transparent;vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">×</button> 
                    <h4 class="modal-title">
                        <label> Reset Password </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanelResetpass" runat="server" UpdateMode="Conditional"> <ContentTemplate>

                    <div class="text-center">
                        <asp:HiddenField ID="reset_userid" runat="server" />
                        <asp:HiddenField ID="reset_isactive" runat="server" />
 
                        Are you sure to reset the password of <b> <asp:Label ID="reset_labelusername" runat="server" Text="Label"></asp:Label> </b> ? <br /><br />

                        <asp:HiddenField ID="reset_username" runat="server" />
                        <asp:HiddenField ID="reset_password" runat="server" />
                        <asp:HiddenField ID="reset_fullname" runat="server" />
                        <asp:HiddenField ID="reset_hopeid" runat="server" />
                        <asp:HiddenField ID="reset_email" runat="server" />
                        <asp:HiddenField ID="reset_bday" runat="server" />
                        <asp:HiddenField ID="reset_phone" runat="server" />
                        <asp:HiddenField ID="reset_lcounter" runat="server" />
                        <asp:HiddenField ID="reset_lastlogin" runat="server" />
                        <asp:HiddenField ID="reset_exppassdate" runat="server" />
                        <asp:HiddenField ID="reset_internal" runat="server" />
                        <asp:HiddenField ID="reset_ad" runat="server" />
                        <asp:HiddenField ID="reset_proint" runat="server" />
                        <asp:HiddenField ID="reset_createdby" runat="server" />
                        <asp:HiddenField ID="reset_createddate" runat="server" />

                        <asp:Button ID="ButtonResetPass" runat="server" Text="RESET" class="btn btn-danger" OnClick="ButtonResetPass_Click" Visible="false"></asp:Button> 
                        <asp:Button ID="ButtonResetPassGlobal" runat="server" Text="RESET" class="btn btn-danger" OnClick="ButtonResetPassGlobal_Click"></asp:Button> 
                    </div>

                    </ContentTemplate> </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- End of Modal Reset Password User -->

    <!-- ##### Modal After Reset Password User ##### -->
    <div class="modal fade" id="modalAfterReset" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
       
        <asp:UpdatePanel ID="UpdatePanelAfterResetpass" runat="server" UpdateMode="Conditional"> <ContentTemplate>

        <div class="modal-dialog" style="width: 500px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">×</button> 
                    <h4 class="modal-title">
                        <label>Reset Password Succeed </label>
                    </h4>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <div class="text-center">
                        The Password has been reseted <br /> <b>New Password : <asp:Label ID="LabelNewPass" runat="server" Text="-"></asp:Label> </b>
                        <br /><br />
                        <button type="button" class="btn btn-success" data-dismiss="modal"> &nbsp; &nbsp; &nbsp; OK &nbsp; &nbsp; &nbsp; </button> 
                    </div>
                </div>
            </div>
        </div>
        </ContentTemplate> </asp:UpdatePanel>
    </div>
    <!-- End of Modal After Reset Password User -->

    <!-- ##### Modal Display Mapping ##### -->
    <div class="modal fade" id="modalDisplayMapping" role="dialog" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 50px;" data-keyboard="false">
        
        <div class="modal-dialog" style="width: 600px">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">×</button>
                    <h4 class="modal-title">
                        <label style="float:left"> Mapping User </label>
                        <asp:UpdateProgress ID="UpdateProgressVM" runat="server" AssociatedUpdatePanelID="UpdatePanelViewMapp">
                        <ProgressTemplate>
                                &nbsp; <img alt="" height="25px" width="25px" style="background-color:transparent; vertical-align:middle" src="<%= Page.ResolveClientUrl("~/Assets/loading.gif") %>" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    </h4>
                </div>

                <div class="titleHeaderBlock">
                    <table border="0" style="width:100%;">
                        <tr>
                            <td> <b> Username :  </b> <asp:Label ID="LabelUname" runat="server"></asp:Label> </td>
                            <td> <b> Fullname : </b>  <asp:Label ID="LabelFname" runat="server"></asp:Label> </td>
                        </tr>
                    </table>
                </div>

                <!-- Modal body -->
                <div class="modal-body text-center">
                    <asp:UpdatePanel ID="UpdatePanelViewMapp" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="HF_userid_viewmapping" runat="server" />
                            <div id="divSimpleMapping" runat="server"></div>
                            <asp:Button ID="ButtonViewMapping" runat="server" Text="Button" CssClass="hidden" OnClick="ButtonViewMapping_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
            </div>
        </div>
        
    </div>
    <!-- End Of Modal Display User -->

</asp:Content>

