Imports System.Data.SqlClient
Partial Class User_func_choose_seats
    Inherits System.Web.UI.Page
    Dim connString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("passengerLoggedIn") Is Nothing OrElse CBool(Session("passengerLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If

        Dim BusId As String

        If Not IsPostBack Then
            BusId = Request.QueryString("BusId")
            Session("BusId") = BusId
        Else
            BusId = Session("BusId").ToString()
        End If

        PopulateSeats(BusId)

    End Sub


    Protected Sub Seat_Click(sender As Object, e As EventArgs)
        Dim seatButton As Button = DirectCast(sender, Button)
        Dim seatId As Integer = Convert.ToInt32(seatButton.ID)
        Dim toBook As Dictionary(Of Integer, String) = If(Session("to_be_book") IsNot Nothing, TryCast(Session("to_be_book"), Dictionary(Of Integer, String)), New Dictionary(Of Integer, String)())
        Dim BusId As String = Session("BusId").ToString()
        Dim bookingTime As String = DateTime.Now.ToString("dd MMMM yyyy hh:mm:ss")
        Dim userEmail As String = Session("passengerEmail").ToString()


        If CanBookSeat(userEmail, BusId, toBook, connString) Then
            BookSeat(seatId, bookingTime, toBook)
            seatButton.BackColor = System.Drawing.Color.Red
        Else
            DisplayMaxSeatsAlert(BusId)
        End If
    End Sub
    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNext.Click

        Response.Redirect("confirm_booking.aspx")

    End Sub
    Private Sub PopulateSeats(ByVal BusId As String)
        If Not String.IsNullOrEmpty(BusId) Then
            Dim seatData As New Dictionary(Of Integer, Boolean)()
            Using conn As New SqlConnection(connString)
                conn.Open()
                Dim querySeatData As String = "SELECT Seat_no, isBooked FROM Seat_t WHERE Bus_id = @BusId"
                Using cmdSeatData As New SqlCommand(querySeatData, conn)
                    cmdSeatData.Parameters.AddWithValue("@BusId", BusId)
                    Dim reader As SqlDataReader = cmdSeatData.ExecuteReader()
                    While reader.Read()
                        Dim seatNumber As Integer = Convert.ToInt32(reader("Seat_no"))
                        Dim isBooked As Boolean = Convert.ToBoolean(reader("isBooked"))
                        seatData(seatNumber) = isBooked
                    End While
                End Using
            End Using

            Dim totalSeats As Integer = 45
            Dim seatsPerRow As Integer = 4
            Dim rows As Integer = 10

            For i As Integer = 1 To rows
                For j As Integer = 1 To seatsPerRow
                    Dim seatNumber As Integer = ((i - 1) * seatsPerRow) + j
                    If j = 3 Then
                        Dim divElement As New HtmlGenericControl("div")
                        divElement.Attributes("class") = "space"
                        phSeats.Controls.Add(divElement)
                    End If

                    Dim seatButton As New Button()
                    seatButton.ID = seatNumber.ToString()
                    seatButton.Text = seatNumber.ToString()
                    seatButton.CssClass = "seat"
                    If seatData.ContainsKey(seatNumber) AndAlso seatData(seatNumber) Then
                        seatButton.Enabled = False
                        seatButton.BackColor = Drawing.Color.Red
                    Else
                        AddHandler seatButton.Click, AddressOf Seat_Click
                    End If

                    phSeats.Controls.Add(seatButton)
                Next

                phSeats.Controls.Add(New LiteralControl("<br />"))
            Next

            Dim lastRowSeats As Integer = 5
            For j As Integer = 1 To lastRowSeats
                Dim seatNumber As Integer = (rows * seatsPerRow) + j
                Dim seatButton As New Button()
                seatButton.ID = seatNumber.ToString()
                seatButton.Text = seatNumber.ToString()
                seatButton.CssClass = "seat"
                If seatData.ContainsKey(seatNumber) AndAlso seatData(seatNumber) Then
                    seatButton.CssClass &= " booked"
                    seatButton.Enabled = False
                    seatButton.BackColor = Drawing.Color.Red
                Else
                    AddHandler seatButton.Click, AddressOf Seat_Click
                End If
                phSeats.Controls.Add(seatButton)
            Next
        End If
    End Sub

    Private Function CanBookSeat(userEmail As String, BusId As String, toBook As Dictionary(Of Integer, String), connString As String) As Boolean
        Dim seatsBooked As Integer = GetSeatsBookedCount(userEmail, BusId, connString)
        Return seatsBooked < 3 AndAlso seatsBooked + toBook.Count < 3
    End Function

    Private Function GetSeatsBookedCount(userEmail As String, BusId As String, connString As String) As Integer
        Dim seatsBooked As Integer = 0
        Using conn As New SqlConnection(connString)
            conn.Open()
            Dim query As String = "SELECT COUNT(*) FROM seat_t WHERE passenger_email = @UserEmail AND bus_id = @BusId"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserEmail", userEmail)
                cmd.Parameters.AddWithValue("@BusId", BusId)
                seatsBooked = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
        Return seatsBooked
    End Function

    Private Sub BookSeat(seatId As Integer, bookingTime As String, ByRef toBook As Dictionary(Of Integer, String))
        If Not toBook.ContainsKey(seatId) Then
            toBook.Add(seatId, bookingTime)
            Session("to_be_book") = toBook
            cmdNext.Enabled = True
        End If
    End Sub

    Private Sub DisplayMaxSeatsAlert(BusId As String)
        Session.Remove("to_be_book")

        Dim script As String = "alert('You can book a maximum of 3 seats.');" &
                               "window.location.href='choose_seats.aspx?BusId=" & BusId & "';"

        ClientScript.RegisterStartupScript(Me.GetType(), "alertRedirect", script, True)
    End Sub


End Class