<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShowTickets.aspx.vb" Inherits="User_func_ShowTickets" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Booked Tickets</title>
    <link href="../Styles/show_tickets.css" rel="stylesheet" />
    <link href="../Styles/navbar.css" rel="stylesheet" />
</head>
<body>
    <div class="navbar">
        <a href="book_seat.aspx">Book Seats</a>
        <a href="ShowTickets.aspx">See Tickets</a>
        <a href="javascript:void(0);" class="right" onclick="logout()">Logout</a>
    </div>
    <form id="form1" runat="server">
        <div class="container" id="ticketsContainer" runat="server">
            <h1>Following are your booked tickets</h1>
            <!-- Tickets will be dynamically added here -->
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="False"></asp:Label>
        </div>
    </form>
</body>
</html>
