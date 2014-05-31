Public Class frmDownloadCraftbukkit

    Private Sub frmDownloadCraftbukkit_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DownloadCraftbukkitViewModel.Instance.myWindow = Me
    End Sub
End Class
