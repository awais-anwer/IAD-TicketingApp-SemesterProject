<%@ Page Language="VB" AutoEventWireup="false" CodeFile="update_singleBus.aspx.vb" Inherits="update_singleBus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Bus</title>
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
        .form-group {
            margin-bottom: 20px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }
        .form-group input[type="text"], 
        .form-group input[type="date"], 
        .form-group input[type="time"],
        .form-group input[type="number"] {
            width: calc(100% - 10px);
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }
        .form-group input[type="submit"] {
            width: 100%;
            padding: 10px;
            background-color: #007bff;
            color: #fff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }
        .form-group input[type="submit"]:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="updateForm" runat="server">
        <div class="container">
            <div class="form-group">
                <label for="txtBusNumber">Bus Number:</label>
                <asp:TextBox ID="txtBusNumber" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtDate">Date:</label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" type="date"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtTime">Time:</label>
                <asp:TextBox ID="txtTime" runat="server" CssClass="form-control" type="time"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtDepartureLocation">Departure Location:</label>
                <asp:TextBox ID="txtDepartureLocation" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtArrivalLocation">Arrival Location:</label>
                <asp:TextBox ID="txtArrivalLocation" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtPrice">Price:</label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" type="number"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" />
            </div>
        </div>
    </form>
</body>
</html>
