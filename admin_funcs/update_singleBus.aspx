<%@ Page Language="VB" AutoEventWireup="false" CodeFile="update_singleBus.aspx.vb" Inherits="update_singleBus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Bus</title>
    <link href="../Styles/update_single_bus.css" rel="stylesheet" />
</head>
<body>
    <form id="updateForm" runat="server">
        <div class="container">
            <h2>UPDATE BUS</h2>
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
