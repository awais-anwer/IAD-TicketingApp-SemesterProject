Imports System.Data.SqlClient
Imports System.Security.Cryptography
Partial Class registration_page
    Inherits System.Web.UI.Page

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("conn_string").ConnectionString

        If ValidateInputs() Then
            Try
                ' Check if the email already exists in the database
                Dim emailExists As Boolean = False
                Using connection As New SqlConnection(connectionString)
                    connection.Open()
                    Dim query As String = "SELECT COUNT(*) FROM App_User WHERE Email = @Email"
                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@Email", txtEmail.Text)
                        Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                        emailExists = (count > 0)
                    End Using
                End Using

                If emailExists Then
                    ' Show error message if email already present in db
                    lblErrorMessage.Text = "The email you entered is already present. Try another"
                    lblErrorMessage.Visible = True
                Else

                    Using Connection As New SqlConnection(connectionString)
                        Connection.Open()

                        Dim query As String = "INSERT INTO App_User (Name, Email, Password, User_type)" &
                        "VALUES (@Name, @Email, @Password, @User_type)"

                        Dim hashedPassword As String = HashPassword(txtPassword.Text)

                        Using command As New SqlCommand(query, Connection)
                            ' Add parameters to the command
                            command.Parameters.AddWithValue("@Name", txtFullName.Text)
                            command.Parameters.AddWithValue("@Email", txtEmail.Text)
                            command.Parameters.AddWithValue("@Password", hashedPassword)
                            command.Parameters.AddWithValue("@User_type", "passenger")

                            command.ExecuteNonQuery()


                            lblErrorMessage.Visible = False ' Hide any previous error message
                            Session("passengerLoggedIn") = True
                            Session("passengerEmail") = txtEmail.Text
                            Session("passengerName") = txtFullName.Text
                            Response.Redirect("User_func/book_seat.aspx")
                        End Using
                    End Using
                End If

            Catch ex As Exception
                ' Show error message if connection fails or any other exception occurs
                lblErrorMessage.Text = "An error occurred while connecting to the database."
                lblErrorMessage.Visible = True
            End Try
        End If

    End Sub

    Protected Function ValidateInputs() As Boolean
        ' Check if full name is empty
        If String.IsNullOrEmpty(txtFullName.Text.Trim()) Then
            ' Display error message for empty full name
            lblErrorMessage.Text = "Please Enter Full Name"
            lblErrorMessage.Visible = True
            Return False
        End If

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

        ' Check if confirm password is empty
        If String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()) Then
            ' Display error message for empty confirm password
            lblErrorMessage.Text = "Please confirm your password."
            lblErrorMessage.Visible = True
            Return False
        End If

        ' Check if password and confirm password match
        If txtPassword.Text.Trim() <> txtConfirmPassword.Text.Trim() Then
            ' Display error message for password mismatch
            lblErrorMessage.Text = "Passwords do not match."
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
