<%@ Page Language="VB" AutoEventWireup="false" CodeFile="update_bus.aspx.vb" Inherits="update_bus" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Search Bus</title>
<style>
        /* CSS styles */
        body {
            font-family: Arial, sans-serif;
            background-color: #f2f2f2;
            margin: 0;
            padding: 0;
        }
        .container {
            width: 80%;
            margin: 50px auto;
            background-color: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        .bus-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        .bus-table th, .bus-table td {
            border: 1px solid #ccc;
            padding: 8px;
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="ticketForm" runat="server">
        <div class="container">
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
            <asp:Button ID="btnSubmit" runat="server" Text="Search" CssClass="btn" />
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="False"></asp:Label>
            <div class="admin-portal-link">
                <p>Go back to <a href="admin_page.aspx">Admin Portal</a></p>
            </div>
        </div>
        
        <table id="busTable" runat="server" class="bus-table" visible="false">
            <thead>
                <tr>
                    <th>Bus Number</th>
                    <th>Date</th>
                    <th>Time</th>
                    <th>Departure Location</th>
                    <th>Arrival Location</th>
                </tr>
            </thead>
            <tbody>
                <%-- Bus rows will be dynamically added here --%>
            </tbody>
        </table>
    </form>
</body>
</html>
