Imports System.Data.SqlClient

Partial Class update_singleBus
    Inherits System.Web.UI.Page
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

    Private Sub update_singleBus_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oldBusNumber As String = Request.QueryString("BusNumber")
            Dim old_bus_date As String = Request.QueryString("Date")
            Dim oldTime As String = Request.QueryString("Time")
            Dim oldDepartureLocation As String = Request.QueryString("DepartureLocation")
            Dim oldArrivalLocation As String = Request.QueryString("ArrivalLocation")
            Dim busPrice As Int32 = Request.QueryString("SeatPrice")

            'Dim parsedDate As DateTime
            ' Populate fields with retrieved values
            busNumber.Text = oldBusNumber
            'DateTime.TryParse(old_bus_date, parsedDate)
            'dateInput.Text = parsedDate.ToString("yyyy-MM-dd")
            PopulateDateDropdown(old_bus_date)
            time.Text = oldTime
            departureLocation.Text = oldDepartureLocation
            arrivalLocation.Text = oldArrivalLocation
            seatPrice.Text = busPrice

        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        ' Retrieve the original values from the query string
        Dim originalDate As String = Request.QueryString("Date")
        Dim originalTime As String = Request.QueryString("Time")
        Dim originalDepartureLocation As String = Request.QueryString("DepartureLocation")
        Dim originalArrivalLocation As String = Request.QueryString("ArrivalLocation")
        Dim originalPrice As Int32 = Convert.ToInt32(Request.QueryString("SeatPrice"))

        ' Retrieve the new values from the form
        If validateInputs() Then
            Dim newDate As String = dateInput.SelectedValue
            Dim newTime As String = time.Text
            Dim newDepartureLocation As String = departureLocation.Text
            Dim newArrivalLocation As String = arrivalLocation.Text
            Dim newPrice As Int32 = Convert.ToInt32(seatPrice.Text)

            ' Check if any values have changed
            If newDate <> originalDate OrElse newTime <> originalTime OrElse newDepartureLocation <> originalDepartureLocation OrElse newArrivalLocation <> originalArrivalLocation OrElse newPrice <> originalPrice Then

                ' Update the data in the database
                Dim isDone = UpdateBusData(busNumber.Text, newDate, newTime, newDepartureLocation, newArrivalLocation, newPrice)

                ' Redirect
                If isDone Then
                    Response.Redirect("update_bus.aspx")
                Else
                    lblErrorMessage.Text = "Something went wrong. Try again"
                    lblErrorMessage.Visible = True
                End If

            Else
                ' No changes were made
                Response.Redirect("update_bus.aspx")
            End If
        End If

    End Sub

    Private Function UpdateBusData(busNumber As String, new_date As String, new_time As String, departureLocation As String, arrivalLocation As String, price As Int32) As Boolean
        Dim isDone As Boolean
        Dim query As String = "UPDATE Bus SET Date_time = @Date_time, Departure_location = @DepartureLocation, Arrival_location = @ArrivalLocation, Seat_price = @Price WHERE Bus_number = @BusNumber"
        Try
            Using connection As New SqlConnection(ConnectionString)
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Date_time", DateTime.Parse(new_date & " " & new_time))
                    command.Parameters.AddWithValue("@DepartureLocation", departureLocation)
                    command.Parameters.AddWithValue("@ArrivalLocation", arrivalLocation)
                    command.Parameters.AddWithValue("@Price", price)
                    command.Parameters.AddWithValue("@BusNumber", busNumber)

                    connection.Open()
                    command.ExecuteNonQuery()

                End Using
            End Using
            isDone = True
        Catch ex As Exception
            isDone = False
        End Try

        Return isDone
    End Function

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

        Return True
    End Function

    Private Sub PopulateDateDropdown(old_date As String)
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

        ' Find and select the item that matches old_date
        Dim selectedItem As ListItem = dateInput.Items.FindByValue(old_date)
        If selectedItem IsNot Nothing Then
            dateInput.ClearSelection()
            selectedItem.Selected = True
        End If
    End Sub
End Class
