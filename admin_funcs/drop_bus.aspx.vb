Imports System.Data.SqlClient
Public Class bus_to_drop
    Public Property BusNumber As String
    Public Property BusId As String
    Public Property busDate As String
    Public Property Time As String
    Public Property DepartureLocation As String
    Public Property ArrivalLocation As String
    Public Property Price As Int64
End Class

Partial Class drop_bus
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("adminLoggedIn") Is Nothing OrElse CBool(Session("adminLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If

        If Not IsPostBack Then
            PopulateDropdownLists()
            DeleteBus()
        End If

    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString
        If validateInputs() Then
            Dim busList As New List(Of bus_to_drop)()

            Dim query As String = "SELECT Bus_number, Bus_id, CONVERT(varchar, Date_time, 106) AS Date, CONVERT(varchar, Date_time, 108) AS Time, Departure_location, Arrival_location, Seat_price FROM Bus WHERE Departure_location = @DepartureLocation AND Arrival_location = @ArrivalLocation"

            Try
                Using connection As New SqlConnection(connectionString)
                    connection.Open()

                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@DepartureLocation", departureLocation.Text)
                        command.Parameters.AddWithValue("@ArrivalLocation", arrivalLocation.Text)

                        Using reader As SqlDataReader = command.ExecuteReader()
                            While reader.Read()
                                Dim bus As New bus_to_drop()
                                bus.BusNumber = reader("Bus_number").ToString()
                                bus.BusId = reader("Bus_id").ToString()
                                bus.busDate = reader("Date").ToString()
                                bus.Time = reader("Time").ToString()
                                bus.DepartureLocation = reader("Departure_location").ToString()
                                bus.ArrivalLocation = reader("Arrival_location").ToString()
                                bus.Price = Convert.ToInt32(reader("Seat_price"))
                                busList.Add(bus)
                            End While
                        End Using
                    End Using
                End Using

                Session("BusList") = busList
                BindBusList(busList)
            Catch ex As Exception
                lblErrorMessage.Text = "An error occurred while processing your request. Please try again later." &
                lblErrorMessage.Visible = True
            End Try
        End If

    End Sub

    Private Sub BindBusList(busList As List(Of bus_to_drop))
        If busList.Count > 0 Then
            busTable.Visible = True

            For Each bus As bus_to_drop In busList
                Dim row As New HtmlTableRow()

                Dim cellBusNumber As New HtmlTableCell()
                cellBusNumber.InnerText = bus.BusNumber
                row.Cells.Add(cellBusNumber)

                Dim cellDate As New HtmlTableCell()
                cellDate.InnerText = bus.busDate
                row.Cells.Add(cellDate)

                Dim cellTime As New HtmlTableCell()
                cellTime.InnerText = bus.Time
                row.Cells.Add(cellTime)

                Dim cellDepartureLocation As New HtmlTableCell()
                cellDepartureLocation.InnerText = bus.DepartureLocation
                row.Cells.Add(cellDepartureLocation)

                Dim cellArrivalLocation As New HtmlTableCell()
                cellArrivalLocation.InnerText = bus.ArrivalLocation
                row.Cells.Add(cellArrivalLocation)

                Dim queryString As String = "BusId=" & bus.BusId

                Dim DropLink As New HtmlAnchor()
                DropLink.InnerText = "Drop Bus"
                DropLink.HRef = "drop_bus.aspx?" & queryString
                DropLink.Attributes("class") = "btn btn-primary"

                Dim cellDropLink As New HtmlTableCell()
                cellDropLink.Controls.Add(DropLink)

                row.Cells.Add(cellDropLink)

                busTable.Rows.Add(row)
            Next
            lblErrorMessage.Visible = False
        Else
            lblErrorMessage.Text = "No buses found for the given criteria."
            lblErrorMessage.Visible = True
        End If
    End Sub
    Private Sub PopulateDropdownLists()
        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

        Try
            Using connection As New SqlConnection(ConnectionString)
                connection.Open()
                departureLocation.Items.Add(New ListItem("Departure Location", ""))
                Dim departureQuery As String = "SELECT DISTINCT Departure_location FROM Bus"
                Using departureCommand As New SqlCommand(departureQuery, connection)
                    Using reader As SqlDataReader = departureCommand.ExecuteReader()
                        While reader.Read()
                            departureLocation.Items.Add(New ListItem(reader("Departure_location").ToString(), reader("Departure_location").ToString()))
                        End While
                    End Using
                End Using

                arrivalLocation.Items.Add(New ListItem("Arrival Location", ""))

                Dim arrivalQuery As String = "SELECT DISTINCT Arrival_location FROM Bus"
                Using arrivalCommand As New SqlCommand(arrivalQuery, connection)
                    Using reader As SqlDataReader = arrivalCommand.ExecuteReader()
                        While reader.Read()
                            arrivalLocation.Items.Add(New ListItem(reader("Arrival_location").ToString(), reader("Arrival_location").ToString()))
                        End While
                    End Using
                End Using
            End Using

            lblErrorMessage.Visible = False
        Catch ex As Exception
            lblErrorMessage.Text = "An error occurred while connecting to the database. Please try again."
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub DeleteBus()
        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

        Dim queryString As String = Request.QueryString("BusId")
        If Not String.IsNullOrEmpty(queryString) Then
            Dim busId As String = queryString

            Dim query As String = "DELETE FROM Bus WHERE Bus_Id = @BusId"

            Try
                Using connection As New SqlConnection(ConnectionString)
                    connection.Open()
                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@BusId", busId)
                        command.ExecuteNonQuery()
                    End Using
                End Using

                Dim busList As List(Of bus_to_drop) = CType(Session("BusList"), List(Of bus_to_drop))
                If busList IsNot Nothing Then
                    Dim busToRemove As bus_to_drop = busList.FirstOrDefault(Function(bus) bus.BusId = busId)
                    If busToRemove IsNot Nothing Then
                        busList.Remove(busToRemove)
                    End If
                End If

                lblErrorMessage.Visible = False
                ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Bus deleted successfully');", True)
                If busList.Count > 0 Then
                    BindBusList(busList)
                End If
            Catch ex As Exception
                lblErrorMessage.Text = "An error occurred while deleting the bus."
                lblErrorMessage.Visible = True
            End Try
        End If
    End Sub

    Private Function validateInputs() As Boolean
        If departureLocation.SelectedValue = "" Then
            lblErrorMessage.Text = "Please select a departure location."
            lblErrorMessage.Visible = True
            Return False
        End If
        If arrivalLocation.SelectedValue = "" Then
            lblErrorMessage.Text = "Please select an arrival location."
            lblErrorMessage.Visible = True
            Return False
        End If
        lblErrorMessage.Visible = False
        Return True
    End Function

End Class
