Public Class frmDownloadPlugins

    Private Sub SelectionChangedHandled(sender As Object, e As SelectionChangedEventArgs)
        e.Handled = True
    End Sub

    Public Sub New(InstalledPlugins As IEnumerable(Of org.phybros.thrift.Plugin))

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        With DownloadPluginsViewModel.Instance
            .Window = Me
            .Load(InstalledPlugins)
        End With
    End Sub
End Class
