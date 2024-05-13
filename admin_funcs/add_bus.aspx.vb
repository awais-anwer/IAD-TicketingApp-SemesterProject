Imports System.Data.SqlClient
Partial Class add_bus
    Inherits System.Web.UI.Page
    Private Sub add_bus_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            PopulateDateDropdown()
        End If
    End Sub

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
                        command.Parameters.AddWithValue("@DateTime", DateTime.Parse(dateInput.SelectedValue & " " & time.Text))
                        command.Parameters.AddWithValue("@SeatPrice", Integer.Parse(seatPrice.Text))
                        command.Parameters.AddWithValue("@TotalSeats", 45)

                        command.ExecuteNonQuery()
                    End Using
                End Using

                ' Insertion successful, 
                ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Bus added successfully.');", True)
            Catch ex As Exception
                ' Error occurred while inserting data
                lblErrorMessage.Text = "An error occurred while processing your request. Please try again later."
            End Try

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
            lblErrorMessage.Text = "Please fill in all the required fields."
            lblErrorMessage.Visible = True
            Return False
        End If

        Dim numberRegex As String = "^[A-Z]+-\d+$"
        If Not System.Text.RegularExpressions.Regex.IsMatch(busNumber.Text.Trim(), numberRegex) Then
            ' Display error message for invalid email format
            lblErrorMessage.Text = "Bus number is not valid. Please enter a valid number"
            lblErrorMessage.Visible = True
            Return False
        End If

        Return True
    End Function

    Private Sub PopulateDateDropdown()
        ' a list to store dates for the next five days
        Dim datesList As New List(Of ListItem)()

        ' Loop to add dates for the next five days to the list
        For i As Integer = 0 To 4
            Dim nextDate As DateTime = DateTime.Today.AddDays(i)
            datesList.Add(New ListItem(nextDate.ToString("dd MMMM yyyy"), nextDate.ToString("MM-dd-yyyy")))
        Next

        ' Bind the list of dates to the dropdown list
        dateInput.DataSource = datesList
        dateInput.DataBind()
    End Sub

End Class
