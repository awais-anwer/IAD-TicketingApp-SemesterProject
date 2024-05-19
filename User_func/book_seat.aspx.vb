Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls

Public Class Bus
    Public Property BusNumber As String
    Public Property busDate As String
    Public Property Time As String
    Public Property DepartureLocation As String
    Public Property ArrivalLocation As String
    Public Property Price As Int64
End Class


Partial Class book_seat
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString
            Try
                Using connection As New SqlConnection(ConnectionString)
                    connection.Open()
                    Dim departureQuery As String = "SELECT DISTINCT Departure_location FROM Bus"
                    Using departureCommand As New SqlCommand(departureQuery, connection)
                        Using reader As SqlDataReader = departureCommand.ExecuteReader()
                            While reader.Read()
                                departureLocation.Items.Add(New ListItem(reader("Departure_location").ToString(), reader("Departure_location").ToString()))
                            End While
                        End Using
                    End Using

                    Dim arrivalQuery As String = "SELECT DISTINCT Arrival_location FROM Bus"
                    Using arrivalCommand As New SqlCommand(arrivalQuery, connection)
                        Using reader As SqlDataReader = arrivalCommand.ExecuteReader()
                            While reader.Read()
                                arrivalLocation.Items.Add(New ListItem(reader("Arrival_location").ToString(), reader("Arrival_location").ToString()))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                lblErrorMessage.Text = "An error occurred while populating dropdown lists: "
                lblErrorMessage.Visible = True
            End Try

            For i As Integer = 0 To 2
                Dim nextDate As DateTime = DateTime.Today.AddDays(i)
                dateInput.Items.Add(New ListItem(nextDate.ToString("dd MMMM yyyy"), nextDate.ToString("yyyy-MM-dd")))
            Next
        End If

    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString
        Dim busList As New List(Of Bus)()
        Dim query As String = "SELECT Bus_number, CONVERT(varchar, Date_time, 106) AS Date, CONVERT(varchar, Date_time, 108) AS Time, Departure_location, Arrival_location, Seat_price FROM Bus WHERE Departure_location = @DepartureLocation AND Arrival_location = @ArrivalLocation AND CONVERT(date, Date_time) = @Date"
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

            ' Bind the list of buses to the HTML table
            BindBusList(busList)
        Catch ex As Exception
            lblErrorMessage.Text = "An error occurred while processing your request. Please try again later."
            lblErrorMessage.Visible = True
        End Try
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

                ' Create the query string with multiple parameters

                ' Create a new anchor tag
                Dim updateLink As New HtmlAnchor()
                updateLink.InnerText = "Choose Seats"
                updateLink.HRef = "choose_seats.aspx?BusNumber=" & bus.BusNumber
                updateLink.Attributes("class") = "btn btn-primary"

                ' Create a new cell to contain the anchor tag
                Dim cellUpdateLink As New HtmlTableCell()
                cellUpdateLink.Controls.Add(updateLink)

                ' Add the cell to the row
                row.Cells.Add(cellUpdateLink)

                busTable.Rows.Add(row)
            Next
        Else
            lblErrorMessage.Text = "No buses found for the given criteria."
            lblErrorMessage.Visible = True
        End If
    End Sub


End Class
