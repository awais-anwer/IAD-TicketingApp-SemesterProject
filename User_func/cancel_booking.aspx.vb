Imports System
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls


Partial Class cancel_booking
    Inherits System.Web.UI.Page

    Private Sub cancel_booking_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("passengerLoggedIn") Is Nothing OrElse CBool(Session("passengerLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If

        If Not IsPostBack Then
            Dim busId As String = Request.QueryString("bus_id")
            Dim seatNo As String = Request.QueryString("seat_no")

            If Not String.IsNullOrEmpty(busId) AndAlso Not String.IsNullOrEmpty(seatNo) Then
                Dim connectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString
                Dim query As String = "UPDATE Seat_t SET isBooked = 0, Passenger_email = NULL WHERE Seat_no = @SeatNo AND Bus_id = @BusId"

                Try
                    Using connection As New SqlConnection(connectionString)
                        Using command As New SqlCommand(query, connection)
                            command.Parameters.AddWithValue("@SeatNo", Convert.ToInt32(seatNo))
                            command.Parameters.AddWithValue("@BusId", Convert.ToInt32(busId))

                            connection.Open()
                            Dim rowsAffected As Integer = command.ExecuteNonQuery()
                            connection.Close()

                            If rowsAffected > 0 Then
                                message.InnerText = "Successfully canceled the Booking"
                            Else
                                message.InnerText = "Seat not found or already updated"
                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    message.InnerText = "Something went wrong. Please try later"
                End Try
            Else
                Response.Redirect("ShowTickets.aspx")
            End If
        End If
    End Sub
End Class
