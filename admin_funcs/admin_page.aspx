<%@ Page Language="VB" AutoEventWireup="false" CodeFile="admin_page.aspx.vb" Inherits="admin_page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Portal</title>
    <link href="../Styles/admin_page.css" rel="stylesheet" />
</head>
<body>
    <h1>Welcome to Admin Portal</h1>
    <form id="form1" runat="server">
         <div class="options-container">
            <asp:Button ID="btnViewBuses" runat="server" Text="View Available Buses" CssClass="admin-button" />    
            <asp:Button ID="btnUpdateBus" runat="server" Text="Update Bus" CssClass="admin-button" />
            <asp:Button ID="btnAddBus" runat="server" Text="Add Bus" CssClass="admin-button" />
            <asp:Button ID="btnDropBus" runat="server" Text="Drop Bus" CssClass="admin-button" />
            <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="admin-button" />
        </div>
    </form>
</body>
</html>
