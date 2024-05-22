Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls

Partial Class book_seat
    Inherits System.Web.UI.Page
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("passengerLoggedIn") Is Nothing OrElse CBool(Session("passengerLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If

        If Not IsPostBack Then
            PopulateDropdownlists()
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim currentDateTime As DateTime = DateTime.Now
        Dim formattedDateTime As String = currentDateTime.ToString("MM/dd/yyyy HH:mm:ss")

        Dim busList As New List(Of Bus)()
        Dim query As String = "SELECT Bus_id, Bus_number, CONVERT(varchar, Date_time, 106) AS Date, CONVERT(varchar, Date_time, 108) AS Time, Departure_location, Arrival_location, Seat_price " &
                          "FROM Bus " &
                          "WHERE Departure_location = @DepartureLocation " &
                          "AND Arrival_location = @ArrivalLocation " &
                          "AND CAST(Date_time AS DATE) = @SelectedDate " &
                          "AND Date_time > @formattedDateTime"

        Try
            Using connection As New SqlConnection(ConnectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@DepartureLocation", departureLocation.Text)
                    command.Parameters.AddWithValue("@ArrivalLocation", arrivalLocation.Text)
                    command.Parameters.AddWithValue("@SelectedDate", Convert.ToDateTime(dateInput.Text))
                    command.Parameters.AddWithValue("@formattedDateTime", formattedDateTime)

                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim bus As New Bus()
                            bus.Bus_id = reader("Bus_id").ToString()
                            bus.BusNumber = reader("Bus_number").ToString()
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

            BindBusList(busList)

        Catch ex As Exception
            lblErrorMessage.Text = "An error occurred while processing your request. Please try again later."
            lblErrorMessage.Visible = True
        End Try
    End Sub

    Private Sub BindBusList(busList As List(Of Bus))
        If busList.Count > 0 Then
            busTable.Visible = True
            lblErrorMessage.Visible = False


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

                Dim bookSeatsLink As New HtmlAnchor()
                bookSeatsLink.InnerText = "Choose Seats"
                bookSeatsLink.HRef = "choose_seats.aspx?BusId=" & bus.Bus_id
                bookSeatsLink.Attributes("class") = "btn btn-primary"
                Dim cellUpdateLink As New HtmlTableCell()
                cellUpdateLink.Controls.Add(bookSeatsLink)
                row.Cells.Add(cellUpdateLink)

                busTable.Rows.Add(row)
            Next
        Else
            lblErrorMessage.Text = "No buses found for the given criteria."
            lblErrorMessage.Visible = True
        End If
    End Sub

    Private Function InitializeConnection() As SqlConnection
        Dim connection As New SqlConnection(ConnectionString)
        connection.Open()
        Return connection
    End Function
    Private Sub PopulateDropdownlists()
        Dim currentDateTime As DateTime = DateTime.Now
        Dim formattedDateTime As String = currentDateTime.ToString("MM/dd/yyyy HH:mm:ss")

        Try
            Using connection As SqlConnection = InitializeConnection()
                Dim departureQuery As String = "SELECT DISTINCT Departure_location FROM Bus WHERE Date_time >@formattedDateTime"
                Using departureCommand As New SqlCommand(departureQuery, connection)
                    departureCommand.Parameters.AddWithValue("@formattedDateTime", formattedDateTime)
                    Using reader As SqlDataReader = departureCommand.ExecuteReader()

                        While reader.Read()
                            departureLocation.Items.Add(New ListItem(reader("Departure_location").ToString(), reader("Departure_location").ToString()))
                        End While
                    End Using
                End Using
                Dim arrivalQuery As String = "SELECT DISTINCT Arrival_location FROM Bus WHERE Date_time > @formattedDateTime"
                Using arrivalCommand As New SqlCommand(arrivalQuery, connection)
                    arrivalCommand.Parameters.AddWithValue("@formattedDateTime", formattedDateTime)
                    Using reader As SqlDataReader = arrivalCommand.ExecuteReader()
                        While reader.Read()
                            arrivalLocation.Items.Add(New ListItem(reader("Arrival_location").ToString(), reader("Arrival_location").ToString()))
                        End While
                    End Using
                End Using
            End Using
            For i As Integer = 0 To 2
                Dim nextDate As DateTime = DateTime.Today.AddDays(i)
                dateInput.Items.Add(New ListItem(nextDate.ToString("dd MMMM yyyy"), nextDate.ToString("yyyy-MM-dd")))
            Next
        Catch ex As Exception
            lblErrorMessage.Text = "An error occurred."
            lblErrorMessage.Visible = True
        End Try
    End Sub

End Class

Public Class Bus
    Public Property Bus_id As String
    Public Property BusNumber As String
    Public Property busDate As String
    Public Property Time As String
    Public Property DepartureLocation As String
    Public Property ArrivalLocation As String
    Public Property Price As Int64
End Class

