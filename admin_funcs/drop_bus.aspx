<%@ Page Language="VB" AutoEventWireup="false" CodeFile="drop_bus.aspx.vb" Inherits="drop_bus" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Drop Bus</title>
<link href="../Styles/drop_bus.css" rel="stylesheet" />
</head>
<body>
    <form id="ticketForm" runat="server">
        <div class="container">
            <h2>Search Bus to drop</h2>
            <div class="select-container">
                <label for="departureLocation">Departure Location:</label>
                <asp:DropDownList ID="departureLocation" runat="server"></asp:DropDownList>
            </div>
            <div class="select-container">
                <label for="arrivalLocation">Arrival Location:</label>
                <asp:DropDownList ID="arrivalLocation" runat="server"></asp:DropDownList >
            </div>
            <asp:Button ID="btnSubmit" runat="server" Text="Search" CssClass="btn" />
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="False"></asp:Label>
            <div class="admin-portal-link">
                <p>Go back to <a href="admin_page.aspx">Admin Portal</a></p>
            </div>
        </div>

    <ContentTemplate>
        <table id="busTable" runat="server" class="bus-table" visible="false">
            <thead>
                <tr>
                    <th>Bus Number</th>
                    <th>Date</th>
                    <th>Time</th>
                    <th>Departure Location</th>
                    <th>Arrival Location</th>
                    <th>Select to Drop</th>
                </tr>
            </thead>
            <tbody>
                <%-- Bus rows will be dynamically added here --%>
            </tbody>
        </table>
    </ContentTemplate>

    </form>
</body>
</html>
