Imports System.Data.SqlClient
Partial Class User_func_ShowTickets
    Inherits System.Web.UI.Page
    Dim connectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Session("passengerLoggedIn") Is Nothing OrElse CBool(Session("passengerLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If

        Dim passenger_email As String = Session("passengerEmail").ToString()
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

    Public Class Seat
        Public Property SeatId As Integer
        Public Property PassengerEmail As String
        Public Property BookingTime As DateTime
        Public Property BusNumber As String
        Public Property BusId As String

        Public Property SeatPrice As String
        Public Property DepartureLocation As String
        Public Property ArrivalLocation As String
        Public Property DepartureTime As DateTime

    End Class

    Private Function GetSeatsForPassenger(ByVal passenger_email As String) As List(Of Seat)
        Dim seats As New List(Of Seat)()
        Dim currentDateTime As DateTime = DateTime.Now
        Dim formattedDateTime As String = currentDateTime.ToString("MM/dd/yyyy HH:mm:ss")

        Dim seatQuery As String = "SELECT s.Seat_no, s.passenger_email, s.Booking_time, s.bus_id, b.bus_number, b.seat_price, b.departure_location, b.arrival_location, b.Date_time " &
                              "FROM seat_t s " &
                              "INNER JOIN bus b ON s.bus_id = b.bus_id " &
                              "WHERE s.passenger_email = @PassengerEmail AND b.date_time > @formattedDateTime"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(seatQuery, connection)
                command.Parameters.AddWithValue("@PassengerEmail", passenger_email)
                command.Parameters.AddWithValue("@formattedDateTime", formattedDateTime)
                connection.Open()

                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim seat As New Seat()
                        seat.SeatId = reader.GetInt32(0)
                        seat.PassengerEmail = reader.GetString(1)
                        seat.BookingTime = reader.GetDateTime(2)
                        seat.BusId = reader.GetInt32(3)
                        seat.BusNumber = reader.GetString(4)
                        seat.SeatPrice = reader.GetInt32(5)
                        seat.DepartureLocation = reader.GetString(6)
                        seat.ArrivalLocation = reader.GetString(7)
                        seat.DepartureTime = reader.GetDateTime(8)
                        seats.Add(seat)
                    End While
                End Using
            End Using
        End Using

        Return seats
    End Function

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

        Dim seatPrice As New Label()
        seatPrice.Text = "<span>Seat Price: " & seat.SeatPrice.ToString() & "</span>"
        seatTicket.Controls.Add(seatPrice)

        Dim departureLocation As New Label()
        departureLocation.Text = "<span>Departure Location: " & seat.DepartureLocation & "</span>"
        seatTicket.Controls.Add(departureLocation)

        Dim arrivalLocation As New Label()
        arrivalLocation.Text = "<span>Arrival Location: " & seat.ArrivalLocation & "</span>"
        seatTicket.Controls.Add(arrivalLocation)

        Dim departureTime As New Label()
        departureTime.Text = "<span>Departure Time: " & seat.DepartureTime.ToString() & "</span>"
        seatTicket.Controls.Add(departureTime)

        Dim cancelBookingLink As New HtmlAnchor()
        cancelBookingLink.InnerText = "Cancel Booking"
        cancelBookingLink.HRef = "cancel_booking.aspx?bus_id=" & seat.BusId & "&" & "seat_no=" & seat.SeatId
        cancelBookingLink.Attributes("class") = "cancel-booking-link"
        seatTicket.Controls.Add(cancelBookingLink)

        Dim ticketsContainer As Control = form1.FindControl("ticketsContainer")
        ticketsContainer.Controls.Add(seatTicket)

        ticketsContainer.Controls.Add(New LiteralControl("<hr />"))
    End Sub

End Class