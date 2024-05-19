Imports System.Data.SqlClient
Partial Class User_func_ShowTickets
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load


        Dim passenger_email As String = "gmustafa@gmail.com"

        ' Assuming you have a function to retrieve the seats based on the passenger's email
        Dim seats As List(Of Seat) = GetSeatsForPassenger(passenger_email)

        If seats.Count > 0 Then
            For Each seat As Seat In seats
                FormatTicket(seat)
            Next
        Else
            lblErrorMessage.Text = "No booked seats."
            lblErrorMessage.Visible = True
        End If

    End Sub

    ' Sample class to represent a seat
    Public Class Seat
        Public Property SeatId As Integer
        Public Property PassengerEmail As String
        Public Property BookingTime As DateTime
        Public Property BusNumber As String
    End Class

    ' Sample function to retrieve seats for a passenger (replace with your actual logic)

    Private Function GetSeatsForPassenger(ByVal passenger_email As String) As List(Of Seat)
        Dim seats As New List(Of Seat)()


        ' Replace the connection string with your actual database connection string
        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

        ' Query to retrieve seat information for the given passenger email
        Dim query As String = "SELECT Seat_no,passenger_email , Booking_time, bus_number FROM seat_t WHERE passenger_email = @PassengerEmail"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@PassengerEmail", passenger_email)
                connection.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()
                While reader.Read()
                    Dim seat As New Seat()
                    seat.SeatId = reader.GetInt32(0)
                    seat.PassengerEmail = reader.GetString(1)
                    seat.BookingTime = reader.GetDateTime(2)
                    seat.BusNumber = reader.GetString(3)
                    seats.Add(seat)
                End While
            End Using
        End Using

        Return seats
    End Function


    ' Sample function to format the ticket display (replace with your actual logic)
    ' Sample function to format the ticket display (replace with your actual logic)
    Private Sub FormatTicket(ByVal seat As Seat)
        Dim seatTicket As New Panel()
        seatTicket.CssClass = "ticket"

        Dim seatNumber As New Label()
        seatNumber.Text = "<span>Seat Number: " & seat.SeatId.ToString() & "</span>"
        seatTicket.Controls.Add(seatNumber)

        Dim passengerEmail As New Label()
        passengerEmail.Text = "<span>Passenger Email: " & seat.PassengerEmail & "</span>"
        seatTicket.Controls.Add(passengerEmail)

        Dim bookingTime As New Label()
        bookingTime.Text = "<span>Booking Time: " & seat.BookingTime.ToString() & "</span>"
        seatTicket.Controls.Add(bookingTime)

        Dim busNumber As New Label()
        busNumber.Text = "<span>Bus Number: " & seat.BusNumber & "</span>"
        seatTicket.Controls.Add(busNumber)

        ' Retrieve additional information based on busNumber
        Dim additionalInfo As String = GetAdditionalInfo(seat.BusNumber)
        seatTicket.Controls.Add(New LiteralControl(additionalInfo))

        Dim ticketsContainer As Control = form1.FindControl("ticketsContainer")
        ticketsContainer.Controls.Add(seatTicket)

        ' Add a horizontal rule after each ticket
        ticketsContainer.Controls.Add(New LiteralControl("<hr />"))
    End Sub



    Private Function GetAdditionalInfo(ByVal busNumber As String) As String
        Dim additionalInfo As New StringBuilder()

        ' Replace the connection string with your actual database connection string
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

        ' Query to retrieve seat price, departure location, and arrival location based on busNumber
        Dim query As String = "SELECT seat_price, departure_location, arrival_location FROM bus WHERE bus_number = @BusNumber"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@BusNumber", busNumber)
                connection.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    Dim seatPrice As Int32 = reader("seat_price")

                    Dim departureLocation As String = reader.GetString(1)
                    Dim arrivalLocation As String = reader.GetString(2)

                    additionalInfo.Append("<br />Seat Price: " & seatPrice.ToString())
                    additionalInfo.Append("<br />Departure Location: " & departureLocation)
                    additionalInfo.Append("<br />Arrival Location: " & arrivalLocation)
                End If
            End Using
        End Using

        Return additionalInfo.ToString()
    End Function


End Class
