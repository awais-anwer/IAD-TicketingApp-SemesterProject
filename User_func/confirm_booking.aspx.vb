Imports System.Data.SqlClient

Partial Class User_func_confirm_booking
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Dim BusNumber As String = Session("BusNumber").ToString()

        If Not String.IsNullOrEmpty(BusNumber) Then

            Dim seatPrice As Integer = 0
            Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString
            Dim query As String = "SELECT seat_price from bus where bus_number=@BusNumber"
            Try
                Using connection As New SqlConnection(ConnectionString)
                    connection.Open()

                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@BusNumber", BusNumber)


                        Using reader As SqlDataReader = command.ExecuteReader()
                            If reader.Read() Then
                                seatPrice = reader("seat_price")
                            End If
                        End Using
                    End Using
                End Using

            Catch ex As Exception
                lblerrormessage.Text = "An error occurred while processing your request. Please try again later."
                lblerrormessage.Visible = True
            End Try



            'in the top show something as : total seats , total price calculated as (seatData.count*seat_price)
            Dim seatData As Dictionary(Of Integer, String) = TryCast(Session("to_be_book"), Dictionary(Of Integer, String))

            If seatData IsNot Nothing Then
                lblTotalSeats.Text = "Total Seats: " & seatData.Count.ToString()
                lblTotalPrice.Text = "Total Price: " & (seatData.Count * seatPrice).ToString()
            End If




            For Each seatId As Integer In seatData.Keys
                Dim passengerUsername As String = "Ukasha"

                Dim bookingTime As String = If(seatData IsNot Nothing AndAlso seatData.ContainsKey(seatId), seatData(seatId), "Not booked yet")


                Dim seatTicket As New Literal()
                seatTicket.Text = "<div class='seat-ticket'>" &
                            "   <span>Seat Number: " & seatId.ToString() & "</span><br />" &
                            "   <span>Passenger Name: " & passengerUsername & "</span><br />" &
                            "   <span>Booking Time: " & bookingTime & "</span><br />" &
                            "   <span>Seat Price: " & seatPrice & "</span><br />" &
                            "</div>"
                form1.Controls.Add(seatTicket)
            Next

        Else
            Response.Write("Invalid BusNumber")
        End If
    End Sub

    Protected Sub cmdConfirm_Click(sender As Object, e As EventArgs) Handles cmdConfirm.Click
        Dim BusNumber As String = Session("BusNumber").ToString()

        Dim seatData As Dictionary(Of Integer, String) = TryCast(Session("to_be_book"), Dictionary(Of Integer, String))
        'to be extracted from the session
        Dim passenger_email As String = "gmustafa@gmail.com"

        If BusNumber IsNot Nothing AndAlso seatData IsNot Nothing Then

            Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

            Using conn As New SqlConnection(ConnectionString)
                conn.Open()

                For Each seatId As Integer In seatData.Keys
                    Dim bookingDate As String = seatData(seatId)

                    Dim queryUpdateSeat As String = "UPDATE Seat_t SET isBooked = 1,  booking_time = @Booking_time, passenger_email = @passenger_email WHERE Bus_number = @BusNumber AND Seat_no = @SeatId"
                    Using cmdUpdateSeat As New SqlCommand(queryUpdateSeat, conn)

                        cmdUpdateSeat.Parameters.AddWithValue("@Booking_time", bookingDate)
                        cmdUpdateSeat.Parameters.AddWithValue("@passenger_email", passenger_email)
                        cmdUpdateSeat.Parameters.AddWithValue("@BusNumber", BusNumber)
                        cmdUpdateSeat.Parameters.AddWithValue("@Seatid", seatId)

                        cmdUpdateSeat.ExecuteNonQuery()
                    End Using
                Next

                Session.Remove("to_be_book")
                cmdConfirm.Enabled = False

                Response.Write("Booking confirmed!")

            End Using
        Else
            Response.Write("Invalid booking data. Please try again.")
        End If
    End Sub



End Class

