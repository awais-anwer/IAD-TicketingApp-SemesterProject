Imports System.Data.SqlClient

Partial Class User_func_confirm_booking
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("passengerLoggedIn") Is Nothing OrElse CBool(Session("passengerLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If

        Dim BusId As String = Session("BusId").ToString()

            If Not String.IsNullOrEmpty(BusId) Then
                Try
                    Dim seatPrice As Integer = GetSeatPrice(BusId)
                    Dim seatData As Dictionary(Of Integer, String) = GetSeatDataFromSession()
                    DisplayTotalSeatsAndPrice(seatData, seatPrice)
                    DisplaySeatTickets(seatData, seatPrice)
                Catch ex As Exception

                    DisplayErrorMessage("An error occurred while processing your request. Please try again later.")
                End Try
            Else
            Response.Write("Invalid BusId")
        End If

    End Sub
    Protected Sub cmdConfirm_Click(sender As Object, e As EventArgs) Handles cmdConfirm.Click
        Dim busId As String = Session("BusId").ToString()
        Dim seatData As Dictionary(Of Integer, String) = TryCast(Session("to_be_book"), Dictionary(Of Integer, String))
        Dim passengerEmail As String = Session("passengerEmail").ToString()

        If seatData IsNot Nothing Then
            Try
                ConfirmSeatBookings(busId, seatData, passengerEmail)
                ClearSessionData()
                ConfirmBooking()
            Catch ex As Exception
                Response.Write("Invalid booking data. Please try again.")
            End Try
        Else
            Response.Write("Invalid booking data. Please try again.")
        End If
    End Sub



    Private Sub ConfirmSeatBookings(busId As String, seatData As Dictionary(Of Integer, String), passengerEmail As String)
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            For Each seatId As Integer In seatData.Keys
                Dim bookingDate As String = seatData(seatId)

                Dim queryUpdateSeat As String = "UPDATE Seat_t SET isBooked = 1, booking_time = @BookingTime, passenger_email = @PassengerEmail WHERE Bus_Id = @BusId AND Seat_no = @SeatId"
                Using cmdUpdateSeat As New SqlCommand(queryUpdateSeat, conn)
                    cmdUpdateSeat.Parameters.AddWithValue("@BookingTime", bookingDate)
                    cmdUpdateSeat.Parameters.AddWithValue("@PassengerEmail", passengerEmail)
                    cmdUpdateSeat.Parameters.AddWithValue("@BusId", busId)
                    cmdUpdateSeat.Parameters.AddWithValue("@SeatId", seatId)

                    cmdUpdateSeat.ExecuteNonQuery()
                End Using
            Next
        End Using
    End Sub

    Private Sub ClearSessionData()
        Session.Remove("to_be_book")
    End Sub

    Private Sub ConfirmBooking()
        cmdConfirm.Enabled = False
        Response.Write("<script>alert('Booking Confirmed')';</script>")

        Response.Redirect("ShowTickets.aspx")
    End Sub




    Private Function GetSeatPrice(BusId As String) As Integer
        Dim seatPrice As Integer = 0
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString
        Dim query As String = "SELECT seat_price FROM bus WHERE bus_id = @BusId"

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@BusId", BusId)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        seatPrice = Convert.ToInt32(reader("seat_price"))
                    End If
                End Using
            End Using
        End Using

        Return seatPrice
    End Function

    Private Function GetSeatDataFromSession() As Dictionary(Of Integer, String)
        Return TryCast(Session("to_be_book"), Dictionary(Of Integer, String))
    End Function

    Private Sub DisplayTotalSeatsAndPrice(seatData As Dictionary(Of Integer, String), seatPrice As Integer)
        If seatData IsNot Nothing Then
            lblTotalSeats.Text = "Total Seats: " & seatData.Count.ToString()
            lblTotalPrice.Text = "Total Price: " & (seatData.Count * seatPrice).ToString()
        End If
    End Sub

    Private Sub DisplaySeatTickets(seatData As Dictionary(Of Integer, String), seatPrice As Integer)
        For Each seatId As Integer In seatData.Keys
            Dim passengerEmail As String = Session("passengerEmail").ToString()
            Dim passengerName As String = Session("passengerName").ToString()
            Dim bookingTime As String = If(seatData.ContainsKey(seatId), seatData(seatId), "Not booked yet")

            Dim seatTicket As New Literal()
            seatTicket.Text = "<div class='seat-ticket'>" &
                          "   <span>Seat Number: " & seatId.ToString() & "</span><br />" &
                          "   <span>Passenger Email: " & passengerEmail & "</span><br />" &
                          "   <span>Passenger Name: " & passengerName & "</span><br />" &
                          "   <span>Booking Time: " & bookingTime & "</span><br />" &
                          "   <span>Seat Price: " & seatPrice & "</span><br />" &
                          "</div>"
            form1.Controls.Add(seatTicket)
        Next
    End Sub



    Private Sub DisplayErrorMessage(message As String)
        lblerrormessage.Text = message
        lblerrormessage.Visible = True
    End Sub

End Class

