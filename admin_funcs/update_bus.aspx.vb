Imports System.Data.SqlClient
Public Class Bus
    Public Property BusNumber As String
    Public Property busDate As String
    Public Property Time As String
    Public Property DepartureLocation As String
    Public Property ArrivalLocation As String
    Public Property Price As Int64
    Public Property BusId As String
End Class
Partial Class update_bus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("adminLoggedIn") Is Nothing OrElse CBool(Session("adminLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If

        If Not IsPostBack Then
            PopulateDropdownLists()
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If validateInputs() Then
            Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

            Dim busList As New List(Of Bus)()

            Dim query As String = "SELECT Bus_Id,Bus_number, CONVERT(varchar, Date_time, 106) AS Date, CONVERT(varchar, Date_time, 108) AS Time, Departure_location, Arrival_location, Seat_price FROM Bus WHERE Departure_location = @DepartureLocation AND Arrival_location = @ArrivalLocation AND CONVERT(date, Date_time) = @Date"

            Try
                Using connection As New SqlConnection(ConnectionString)
                    connection.Open()

                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@DepartureLocation", departureLocation.Text)
                        command.Parameters.AddWithValue("@ArrivalLocation", arrivalLocation.Text)
                        command.Parameters.AddWithValue("@Date", Date.Parse(dateInput.Text))

                        Using reader As SqlDataReader = command.ExecuteReader()
                            While reader.Read()
                                Dim bus As New Bus()
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
                lblErrorMessage.Visible = False
                BindBusList(busList)
            Catch ex As Exception
                lblErrorMessage.Text = "Something went wrong. Please try again later."
                lblErrorMessage.Visible = True
            End Try
        End If

    End Sub

    Private Sub BindBusList(busList As List(Of Bus))
        If busList.Count > 0 Then
            busTable.Visible = True

            For Each bus As Bus In busList
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

                Dim queryString As String = "BusId=" & bus.BusId & "&BusNumber=" & bus.BusNumber & "&Date=" & bus.busDate & "&Time=" & bus.Time & "&DepartureLocation=" & bus.DepartureLocation & "&ArrivalLocation=" & bus.ArrivalLocation & "&SeatPrice=" & bus.Price

                Dim updateLink As New HtmlAnchor()
                updateLink.InnerText = "Update"
                updateLink.HRef = "update_singleBus.aspx?" & queryString
                updateLink.Attributes("class") = "btn btn-primary"

                Dim cellUpdateLink As New HtmlTableCell()
                cellUpdateLink.Controls.Add(updateLink)
                row.Cells.Add(cellUpdateLink)

                busTable.Rows.Add(row)
            Next
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
            lblErrorMessage.Text = "An error occurred while Connecting to the database."
            lblErrorMessage.Visible = True
        End Try

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

        If dateInput.Text = "" Then
            lblErrorMessage.Text = "Please select a date."
            lblErrorMessage.Visible = True
            Return False
        End If

        lblErrorMessage.Visible = False
        Return True
    End Function

End Class