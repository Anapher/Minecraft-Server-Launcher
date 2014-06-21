Imports Exceptionless
Class MainWindow

    Private Sub MainWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        e.Cancel = MainViewModel.Instance.OnClosing()
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Exceptionless.ExceptionlessClient.Current.Register(False)
        MainViewModel.Instance.DynmapRefresh = Sub()
                                                   Me.Dispatcher.Invoke(Sub()
                                                                            Dim ip = webDynmap.Tag.ToString()
                                                                            If Not String.IsNullOrWhiteSpace(ip) Then
                                                                                webDynmap.Navigate(ip)
                                                                            End If
                                                                        End Sub)
                                               End Sub
    End Sub

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        If Not String.IsNullOrWhiteSpace(webDynmap.Tag.ToString()) Then
            webDynmap.Navigate(webDynmap.Tag.ToString())
        End If
    End Sub

    Private Sub Hyperlink_RequestNavigate(sender As Object, e As RequestNavigateEventArgs)
        Process.Start(e.Uri.OriginalString)
    End Sub
End Class