<%@ Page Language="VB" AutoEventWireup="false" CodeFile="add_bus.aspx.vb" Inherits="add_bus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Bus</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            margin: 0;
            padding: 0;
        }

        form {
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            width: 300px;
            margin: 50px auto;
            padding: 20px;
        }

        label {
            display: block;
            margin-bottom: 5px;
        }

        input[type="text"], input[type="date"], input[type="time"] {
            width: 100%;
            padding: 8px;
            margin-bottom: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }

        input[type="date"]::-webkit-inner-spin-button,
        input[type="date"]::-webkit-calendar-picker-indicator {
            display: none;
            -webkit-appearance: none;
        }

        input[type="time"]::-webkit-calendar-picker-indicator {
            display: none;
        }

        input[type="time"] {
            -moz-appearance:textfield;
        }

        input[type="date"] {
            -webkit-appearance: none;
            -moz-appearance: none;
        }

        input[type="submit"] {
            background-color: #007bff;
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }

        input[type="submit"]:hover {
            background-color: #0056b3;
        }
        /* Your CSS styles here */
        .error-message {
            color: red;
            font-size: 14px;
            margin-top: 5px;
        }
    </style>
</head>
<body>
    <form id="ticketForm" runat="server">
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
            <asp:TextBox ID="dateInput" runat="server" type="date"></asp:TextBox>
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
    </form></body>
</html>
