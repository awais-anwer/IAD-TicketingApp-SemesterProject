<%@ Page Language="VB" AutoEventWireup="false" CodeFile="cancel_booking.aspx.vb" Inherits="cancel_booking" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Booking Cancel</title>
    <link href="../Styles/navbar.css" rel="stylesheet" />
    <link href="../Styles/cancel_booking.css" rel="stylesheet" />
</head>
<body>
     <div class="navbar">
        <a href="book_seat.aspx">Book Seats</a>
        <a href="ShowTickets.aspx">See Tickets</a>
        <a href="../Default.aspx" class="right">Logout</a>
    </div>
    <form id="form1" runat="server">
        <div class="card">
            <h2 id="message" runat="server"></h2>
        </div>
    </form>
</body>
</html>
