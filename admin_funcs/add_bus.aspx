<%@ Page Language="VB" AutoEventWireup="false" CodeFile="add_bus.aspx.vb" Inherits="add_bus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Bus</title>
    <link href="../Styles/add_bus.css" rel="stylesheet" />
</head>
<body>
    <form id="ticketForm" runat="server">
         <h2>Add Buss</h2>
        <div>
            <label for="busNumber">Bus Number:</label>
            <asp:TextBox ID="busNumber" runat="server"></asp:TextBox>
        </div>
        <div>
            <label for="departureLocation">Departure Location:</label>
            <asp:TextBox ID="departureLocation" runat="server"></asp:TextBox>
        </div>
        <div>
            <label for="arrivalLocation">Arrival Location:</label>
            <asp:TextBox ID="arrivalLocation" runat="server"></asp:TextBox>
        </div>
        <div>
            <label for="dateInput">Date:</label>
            <asp:DropDownList ID="dateInput" runat="server" CssClass="drop-down-control"></asp:DropDownList>
        </div>
        <div>
            <label for="time">Time:</label>
            <asp:TextBox ID="time" runat="server" type="time"></asp:TextBox>
        </div>
        <div>
            <label for="seatPrice">Seat Price:</label>
            <asp:TextBox ID="seatPrice" runat="server" type="number"></asp:TextBox>
        </div>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn" />
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="False"></asp:Label>
        <div class="admin-portal-link">
            <p>Go back to <a href="admin_page.aspx">Admin Portal</a></p>
        </div>
    </form>
</body>
</html>
