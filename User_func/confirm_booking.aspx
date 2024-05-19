<%@ Page Language="VB" AutoEventWireup="false" CodeFile="confirm_booking.aspx.vb" Inherits="User_func_confirm_booking" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Confirm Booking</title>
    <link href="../Styles/confirm_booking.css" rel="stylesheet" />
    <link href="../Styles/navbar.css" rel="stylesheet" />
</head>
<body>
     <div class="navbar">
        <a href="book_seat.aspx">Book Seats</a>
        <a href="ShowTickets.aspx">See Tickets</a>
        <a href="javascript:void(0);" class="right" onclick="logout()">Logout</a>
    </div>
    <form id="form1" runat="server">
        <div class="container">
            <div id="bookingSummary">
                <asp:Label ID="lblTotalSeats" runat="server" CssClass="label"></asp:Label><br />
                <asp:Label ID="lblTotalPrice" runat="server" CssClass="label"></asp:Label><br />
            </div>
            <div id="seatTickets" runat="server">
                <!-- Seat tickets will be dynamically added here -->
            </div>
            <asp:Label ID="lblerrormessage" runat="server"></asp:Label>
            <asp:Button ID="cmdConfirm" runat="server" Text="Confirm" CssClass="btn-confirm" />
        </div>
    </form>
</body>
</html>
