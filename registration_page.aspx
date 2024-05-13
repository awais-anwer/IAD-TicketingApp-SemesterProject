<%@ Page Language="VB" AutoEventWireup="false" CodeFile="registration_page.aspx.vb" Inherits="registration_page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Student Registration</title>
    <link href="Styles/registration_page.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="registration-container">
            <h2>User Registration</h2>
            <div class="form-group">
                <label for="txtFullName">Full Name:</label>
                <asp:TextBox ID="txtFullName" runat="server" placeholder="FirstName LastName" />
            </div>
            <div class="form-group">
                <label for="txtEmail">Email Address:</label>
                <asp:TextBox ID="txtEmail" runat="server" type="email" placeholder="someone@something.com" />
            </div>
            <div class="form-group">
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" type="password" placeholder="Enter your password" />
            </div>
            <div class="form-group">
                <label for="txtConfirmPassword">Confirm Password:</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" type="password" placeholder="Confirm your password" />
            </div>
            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="submit-button" />
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="False"></asp:Label>
        </div>
    </form>
</body>
</html>
