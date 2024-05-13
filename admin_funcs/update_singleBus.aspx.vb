Imports System.Data.SqlClient

Partial Class update_singleBus
    Inherits System.Web.UI.Page
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

    Private Sub update_singleBus_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim busNumber As String = Request.QueryString("BusNumber")
            Dim bus_date As String = Request.QueryString("Date")
            Dim time As String = Request.QueryString("Time")
            Dim departureLocation As String = Request.QueryString("DepartureLocation")
            Dim arrivalLocation As String = Request.QueryString("ArrivalLocation")
            Dim busPrice As Int32 = Request.QueryString("SeatPrice")

            Dim parsedDate As DateTime
            ' Populate fields with retrieved values
            txtBusNumber.Text = busNumber
            DateTime.TryParse(bus_date, parsedDate)
            txtDate.Text = parsedDate.ToString("yyyy-MM-dd")
            txtTime.Text = time
            txtDepartureLocation.Text = departureLocation
            txtArrivalLocation.Text = arrivalLocation
            txtPrice.Text = busPrice

        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        ' Retrieve the new values from the form
        Dim newDate As String = txtDate.Text
        Dim newTime As String = txtTime.Text
        Dim newDepartureLocation As String = txtDepartureLocation.Text
        Dim newArrivalLocation As String = txtArrivalLocation.Text
        Dim newPrice As Int32 = Convert.ToInt32(txtPrice.Text)

        ' Retrieve the original values from the query string
        Dim originalDate As String = Request.QueryString("Date")
        Dim originalTime As String = Request.QueryString("Time")
        Dim originalDepartureLocation As String = Request.QueryString("DepartureLocation")
        Dim originalArrivalLocation As String = Request.QueryString("ArrivalLocation")
        Dim originalPrice As Int32 = Convert.ToInt32(Request.QueryString("SeatPrice"))

        ' Check if any values have changed
        If newDate <> originalDate OrElse newTime <> originalTime OrElse newDepartureLocation <> originalDepartureLocation OrElse newArrivalLocation <> originalArrivalLocation OrElse newPrice <> originalPrice Then
            ' Update the data in the database
            UpdateBusData(txtBusNumber.Text, newDate, newTime, newDepartureLocation, newArrivalLocation, newPrice)

            ' Insertion successful, you can redirect the user to another page or show a success message.
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Bus updated successfully.');", True)
            ' Redirect to the home page after updating
            Response.Redirect("admin_page.aspx")
        Else
            ' No changes were made, redirect to the home page without updating
            Response.Redirect("admin_page.aspx")
        End If
    End Sub

    Private Sub UpdateBusData(busNumber As String, new_date As String, new_time As String, departureLocation As String, arrivalLocation As String, price As Int32)
        Dim query As String = "UPDATE Bus SET Date_time = @Date_time, Departure_location = @DepartureLocation, Arrival_location = @ArrivalLocation, Seat_price = @Price WHERE Bus_number = @BusNumber"

        Using connection As New SqlConnection(connectionString)
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
    End Sub

End Class
