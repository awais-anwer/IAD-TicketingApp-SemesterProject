Imports System.Data.SqlClient
Partial Class add_bus
    Inherits System.Web.UI.Page
    Protected Sub SubmitButton_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If validateInputs() Then
            Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

            Try
                Using connection As New SqlConnection(connectionString)
                    connection.Open()

                    Dim query As String = "INSERT INTO Bus (Bus_number, Departure_location, Arrival_location, Date_time, Seat_price, Total_seats) VALUES (@BusNumber, @DepartureLocation, @ArrivalLocation, @DateTime, @SeatPrice, @TotalSeats)"

                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@BusNumber", busNumber.Text)
                        command.Parameters.AddWithValue("@DepartureLocation", departureLocation.Text)
                        command.Parameters.AddWithValue("@ArrivalLocation", arrivalLocation.Text)
                        command.Parameters.AddWithValue("@DateTime", DateTime.Parse(dateInput.Text & " " & time.Text))
                        command.Parameters.AddWithValue("@SeatPrice", Integer.Parse(seatPrice.Text))
                        command.Parameters.AddWithValue("@TotalSeats", 45)

                        command.ExecuteNonQuery()
                    End Using
                End Using

                ' Insertion successful, you can redirect the user to another page or show a success message.
                ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Bus added successfully.');", True)
            Catch ex As Exception
                ' Error occurred while inserting data, handle the exception (e.g., display an error message).
                lblErrorMessage.Text = "An error occurred while processing your request. Please try again later." & ex.Message
            End Try
        Else
            ' Inputs validation failed, display an error message.
            lblErrorMessage.Text = "Please fill in all the required fields."
            lblErrorMessage.Visible = True
        End If
    End Sub

    Private Function validateInputs() As Boolean
        ' Validate all input fields
        If String.IsNullOrEmpty(busNumber.Text) OrElse
           String.IsNullOrEmpty(departureLocation.Text) OrElse
           String.IsNullOrEmpty(arrivalLocation.Text) OrElse
           String.IsNullOrEmpty(dateInput.Text) OrElse
           String.IsNullOrEmpty(time.Text) OrElse
           String.IsNullOrEmpty(seatPrice.Text) Then
            Return False
        End If

        Return True
    End Function
End Class
