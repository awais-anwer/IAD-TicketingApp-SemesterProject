<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login_page.aspx.vb" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="Styles/login.css" rel="stylesheet" />
</head>
<body>
    <h2>Bus Ticket Booking App</h2>   
    <div class="login-container">
    <h2>Login</h2>
    <form id="form" runat="server"> 
        <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter Your email address" Required="true" />
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter your password" Required="true" />
        <asp:Button ID="cmdSubmit" runat="server" Text="Login" CssClass="submit-button" />
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="False"></asp:Label>
    </form>
        <div class="register-link">
            <p>New here? <a href="registration_page.aspx">Create Account</a></p>
        </div>
    </div>
</body>
</html>

