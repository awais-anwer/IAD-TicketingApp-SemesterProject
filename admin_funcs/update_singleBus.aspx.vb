Imports System.Data.SqlClient

Partial Class update_singleBus
    Inherits System.Web.UI.Page
    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

    Private Sub update_singleBus_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("adminLoggedIn") Is Nothing OrElse CBool(Session("adminLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If

        If Not IsPostBack Then
            Dim oldBusNumber As String = Request.QueryString("BusNumber")
            Dim BusId As String = Request.QueryString("BusId")

            Dim old_bus_date As String = Request.QueryString("Date")
            Dim oldTime As String = Request.QueryString("Time")
            Dim oldDepartureLocation As String = Request.QueryString("DepartureLocation")
            Dim oldArrivalLocation As String = Request.QueryString("ArrivalLocation")
            Dim busPrice As Int32 = Request.QueryString("SeatPrice")

            busNumber.Text = oldBusNumber

            PopulateDateDropdown(old_bus_date)
            time.Text = oldTime
            departureLocation.Text = oldDepartureLocation
            arrivalLocation.Text = oldArrivalLocation
            seatPrice.Text = busPrice

        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim BusId As String = Request.QueryString("BusId")
        Dim originalDate As String = Request.QueryString("Date")
        Dim originalTime As String = Request.QueryString("Time")
        Dim originalDepartureLocation As String = Request.QueryString("DepartureLocation")
        Dim originalArrivalLocation As String = Request.QueryString("ArrivalLocation")
        Dim originalPrice As Int32 = Convert.ToInt32(Request.QueryString("SeatPrice"))

        If validateInputs() Then
            Dim newDate As String = dateInput.SelectedValue
            Dim newTime As String = time.Text
            Dim newDepartureLocation As String = departureLocation.Text
            Dim newArrivalLocation As String = arrivalLocation.Text
            Dim newPrice As Int32 = Convert.ToInt32(seatPrice.Text)

            If newDate <> originalDate OrElse newTime <> originalTime OrElse newDepartureLocation <> originalDepartureLocation OrElse newArrivalLocation <> originalArrivalLocation OrElse newPrice <> originalPrice Then

                Dim isDone = UpdateBusData(BusId, busNumber.Text, newDate, newTime, newDepartureLocation, newArrivalLocation, newPrice)

                If isDone Then
                    Response.Redirect("update_bus.aspx")
                Else
                    lblErrorMessage.Text = "Something went wrong. Try again"
                    lblErrorMessage.Visible = True
                End If

            Else
                Response.Redirect("update_bus.aspx")
            End If
        End If

    End Sub

    Private Function UpdateBusData(busId As String, busNumber As String, new_date As String, new_time As String, departureLocation As String, arrivalLocation As String, price As Int32) As Boolean
        Dim isDone As Boolean
        Dim query As String = "UPDATE Bus SET Date_time = @Date_time, Departure_location = @DepartureLocation, Arrival_location = @ArrivalLocation, Seat_price = @Price WHERE Bus_id = @BusId"
        Try
            Using connection As New SqlConnection(ConnectionString)
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Date_time", DateTime.Parse(new_date & " " & new_time))
                    command.Parameters.AddWithValue("@DepartureLocation", departureLocation)
                    command.Parameters.AddWithValue("@ArrivalLocation", arrivalLocation)
                    command.Parameters.AddWithValue("@Price", price)
                    command.Parameters.AddWithValue("@BusId", busId)

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
        Dim datesList As New List(Of ListItem)()

        For i As Integer = 0 To 4
            Dim nextDate As DateTime = DateTime.Today.AddDays(i)
            datesList.Add(New ListItem(nextDate.ToString("dd MMMM yyyy"), nextDate.ToString("MM-dd-yyyy")))
        Next

        dateInput.DataSource = datesList
        dateInput.DataBind()

        Dim selectedItem As ListItem = dateInput.Items.FindByValue(old_date)
        If selectedItem IsNot Nothing Then
            dateInput.ClearSelection()
            selectedItem.Selected = True
        End If
    End Sub
End Class