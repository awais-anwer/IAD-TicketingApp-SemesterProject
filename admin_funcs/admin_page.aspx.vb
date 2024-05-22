
Partial Class admin_page
    Inherits System.Web.UI.Page

    Private Sub admin_page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("adminLoggedIn") Is Nothing OrElse CBool(Session("adminLoggedIn")) = False Then
            Response.Redirect("../login_page.aspx")
        End If
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Response.Redirect("../Default.aspx")
    End Sub

    Private Sub btnAddBus_Click(sender As Object, e As EventArgs) Handles btnAddBus.Click
        Response.Redirect("add_bus.aspx")
    End Sub

    Private Sub btnUpdateBus_Click(sender As Object, e As EventArgs) Handles btnUpdateBus.Click
        Response.Redirect("update_bus.aspx")
    End Sub

    Private Sub btnViewBuses_Click(sender As Object, e As EventArgs) Handles btnViewBuses.Click
        Response.Redirect("update_bus.aspx")
    End Sub

    Private Sub btnDropBus_Click(sender As Object, e As EventArgs) Handles btnDropBus.Click
        Response.Redirect("drop_bus.aspx")
    End Sub
End Class
