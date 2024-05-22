Imports System.Data.SqlClient
Imports System.Security.Cryptography
Partial Class login
    Inherits System.Web.UI.Page

    Private Sub cmdSubmit_Click(sender As Object, e As EventArgs) Handles cmdSubmit.Click

        Dim connectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

        If ValidateInputs() Then

            Dim email As String = txtEmail.Text.Trim()
            Dim password As String = txtPassword.Text.Trim()

            ' SQL query to fetch user details based on email
            Dim query As String = "SELECT Name, Email, Password, User_Type FROM App_User WHERE Email = @Email"
            Try

                Using connection As New SqlConnection(connectionString)
                    connection.Open()
                    Using command As New SqlCommand(query, connection)
                        ' Add parameters to the command
                        command.Parameters.AddWithValue("@Email", email)

                        Dim reader As SqlDataReader = command.ExecuteReader()

                        ' Check if any records were returned
                        If reader.HasRows Then
                            ' Fetch the user details
                            reader.Read()
                            Dim name As String = reader("Name").ToString()
                            Dim storedEmail As String = reader("Email").ToString()
                            Dim storedPassword As String = reader("Password").ToString()
                            Dim userType As String = reader("User_Type").ToString()

                            ' Check if the entered password matches the stored password
                            Dim hashedPassword As String = HashPassword(password)
                            If storedPassword = hashedPassword Then
                                ' Check user type and redirect accordingly
                                If userType = "admin" Then
                                    ' Redirect to admin page
                                    Session("adminLoggedIn") = True
                                    Response.Redirect("admin_funcs/admin_page.aspx")
                                ElseIf userType = "passenger" Then
                                    ' Redirect to user page
                                    Session("passengerLoggedIn") = True
                                    Session("passengerEmail") = storedEmail
                                    Session("passengerName") = name
                                    Response.Redirect("User_func/book_seat.aspx")
                                End If
                            Else
                                ' Invalid password
                                lblErrorMessage.Text = "Invalid username or password"
                                lblErrorMessage.Visible = True
                            End If
                        Else
                            ' No records found for the entered email
                            lblErrorMessage.Text = "Invalid username or password"
                            lblErrorMessage.Visible = True
                        End If
                        reader.Close()
                    End Using
                End Using

            Catch ex As Exception
                ' Show error message if connection fails or any other exception occurs
                lblErrorMessage.Text = "Connection error please try again."
                lblErrorMessage.Visible = True
            End Try
        End If

    End Sub


    Protected Function ValidateInputs() As Boolean

        ' Check if email is valid
        Dim emailRegex As String = "\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b"
        If Not System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), emailRegex) Then
            ' Display error message for invalid email format
            lblErrorMessage.Text = "Please enter a valid email address."
            lblErrorMessage.Visible = True
            Return False
        End If

        ' Check if password is empty
        If String.IsNullOrEmpty(txtPassword.Text.Trim()) Then
            ' Display error message for empty password
            lblErrorMessage.Text = "Please enter your password."
            lblErrorMessage.Visible = True
            Return False
        End If

        ' All inputs are valid
        Return True
    End Function

    Public Function HashPassword(password As String) As String
        Using sha256 As SHA256 = sha256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Dim builder As New StringBuilder()
            For Each b As Byte In hash
                builder.Append(b.ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function

End Class
