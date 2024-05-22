
Partial Class _Default
    Inherits System.Web.UI.Page
    Private Sub _Default_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Clear()
        Response.Redirect("login_page.aspx")
    End Sub
End Class
