<%@ Page Language="VB" AutoEventWireup="false" CodeFile="choose_seats.aspx.vb" Inherits="User_func_choose_seats" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Choose Seats</title>
    <link href="../Styles/choose_seat.css" rel="stylesheet" />
    <link href="../Styles/navbar.css" rel="stylesheet" />
</head>
<body>
     <div class="navbar">
        <a href="book_seat.aspx">Book Seats</a>
        <a href="ShowTickets.aspx">See Tickets</a>
        <a href="javascript:void(0);" class="right" onclick="logout()">Logout</a>
    </div>
    <form id="form1" runat="server">
        <h1>Choose Seats</h1>
        <div>
            <asp:PlaceHolder ID="phSeats" runat="server"></asp:PlaceHolder>
        </div>
        <div>
            <asp:Button ID="cmdNext" runat ="server" text="Next" Enabled="false" />
        </div>
    </form>
</body>
</html>
