Imports System.Data.SqlClient


Partial Class User_func_choose_seats
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim BusNumber As String = Nothing
        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString
        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("BusNumber")) Then
                BusNumber = Request.QueryString("BusNumber")
                Session("BusNumber") = BusNumber
            Else
                BusNumber = Session("BusNumber").ToString
            End If

        Else
            BusNumber = Session("BusNumber").ToString()
        End If
        If Not String.IsNullOrEmpty(BusNumber) Then

            Using conn As New SqlConnection(ConnectionString)
                conn.Open()



                ' Retrieve the seat data from the Seat_t table
                Dim querySeatData As String = "SELECT Seat_no, isBooked FROM Seat_t WHERE Bus_number = @BusNumber"
                Using cmdSeatData As New SqlCommand(querySeatData, conn)
                    cmdSeatData.Parameters.AddWithValue("@BusNumber", BusNumber)
                    Dim reader As SqlDataReader = cmdSeatData.ExecuteReader()
                    Dim seatData As New Dictionary(Of Integer, Boolean)()
                    While reader.Read()
                        Dim seatNumber As Integer = Convert.ToInt32(reader("Seat_no"))
                        Dim isBooked As Boolean = Convert.ToBoolean(reader("isBooked"))
                        seatData(seatNumber) = isBooked
                    End While

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

                            ' Add a seat button
                            Dim seatButton As New Button()
                            seatButton.ID = seatNumber
                            seatButton.Text = seatNumber.ToString()
                            seatButton.CssClass = "seat"
                            If seatData.ContainsKey(seatNumber) AndAlso seatData(seatNumber) Then
                                seatButton.Enabled = False
                                seatButton.BackColor = Drawing.Color.Red
                            Else
                                AddHandler seatButton.Click, AddressOf Seat_Click
                            End If

                            ' Add the seat button to the placeholder
                            phSeats.Controls.Add(seatButton)

                            ' Add a space after every 2 seats

                        Next
                        ' Add a line break after each row
                        phSeats.Controls.Add(New LiteralControl("<br />"))
                    Next

                    ' Add the remaining seats in the last row
                    Dim lastRowSeats As Integer = 5
                    For j As Integer = 1 To lastRowSeats
                        Dim seatNumber As Integer = (rows * seatsPerRow) + j
                        Dim seatButton As New Button()
                        seatButton.ID = seatNumber
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

                End Using
            End Using
        End If
    End Sub




    Protected Sub Seat_Click(sender As Object, e As EventArgs)
        Dim seatButton As Button = DirectCast(sender, Button)
        Dim seatId As Integer = Convert.ToInt32(seatButton.ID) ' Convert seat ID to integer
        Dim toBook As Dictionary(Of Integer, String) = Nothing
        Dim BusNumber As String = Session("BusNumber")

        If Session("to_be_book") IsNot Nothing Then
            toBook = TryCast(Session("to_be_book"), Dictionary(Of Integer, String))
        Else
            toBook = New Dictionary(Of Integer, String)()
        End If

        Dim bookingTime As String = DateTime.Now.ToString("dd MMMM yyyy hh:mm:ss")

        If toBook.Count < 3 Then
            If Not toBook.ContainsKey(seatId) Then
                toBook.Add(seatId, bookingTime)
                Session("to_be_book") = toBook
                cmdNext.Enabled = True
                seatButton.BackColor = System.Drawing.Color.Red
            End If
        Else
            Session.Remove("to_be_book")

            Response.Write("<script>alert('You can book maximum 3 seats.'); window.location.href = 'choose_seats.aspx?" & BusNumber & "';</script>")
        End If
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNext.Click

        Dim BusNumber As String = Session("BusNumber").ToString()

        Dim connString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

        Dim userEmail As String = "gmustafa@gmail.com" ' will extract mail from seession data   

        Dim seatsBooked As Integer = 0

        Dim toBook As Dictionary(Of Integer, String) = TryCast(Session("to_be_book"), Dictionary(Of Integer, String))

        Using conn As New SqlConnection(connString)
            conn.Open()
            Dim query As String = "SELECT COUNT(*) FROM seat_t WHERE passenger_email = @UserEmail AND bus_number = @BusNumber"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserEmail", userEmail)
                cmd.Parameters.AddWithValue("@BusNumber", BusNumber)
                seatsBooked = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using

        If seatsBooked <= 3 And seatsBooked + toBook.Count <= 3 Then

            Response.Redirect("confirm_booking.aspx")
        Else
            Session.Remove("to_be_book")
            Response.Write("<script>alert('You can book maximum 3 seats.'); window.location.href = 'choose_seats.aspx?" & BusNumber & "';</script>")
        End If
    End Sub
End Class