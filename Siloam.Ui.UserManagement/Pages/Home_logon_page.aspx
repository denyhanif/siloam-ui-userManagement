<%@ Page Title="Home" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Home_logon_page.aspx.cs" Inherits="Siloam.Ui.UserManagement.Pages.Home_logon_page" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            // Handler for .ready() called.
            window.setTimeout(function () {
                location.href = "UserMaster.aspx";
            }, 1500);
        });

    </script>

    <div style="text-align:center; margin-top:15%;">
        <asp:Image ID="ImageLoad" ImageUrl="~/Assets/loader.gif" style="width:100px; margin-bottom:-50px; margin-top:-50px;" runat="server" />
        <h1>..:: <asp:Label ID="LabelWelcome" runat="server" Text="WELCOME"></asp:Label> ::..</h1>
        <h1 style="margin-top:-10px;"> <asp:Label ID="LabelName" runat="server" Text="WELCOME"></asp:Label> </h1>       
    </div>

</asp:Content>
