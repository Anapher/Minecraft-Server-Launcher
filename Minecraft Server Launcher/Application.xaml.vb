Imports Exceptionless

Class Application

    ' Ereignisse auf Anwendungsebene wie Startup, Exit und DispatcherUnhandledException
    ' können in dieser Datei verarbeitet werden.
    Private WithEvents MyDomain As AppDomain = AppDomain.CurrentDomain

#If Not Debug Then
    Dim IsHandled As Boolean = False
    Private Sub Dispatcher_UnhandledExceptions(sender As Object, e As Windows.Threading.DispatcherUnhandledExceptionEventArgs)
        e.Handled = True
        If Not IsHandled Then
            IsHandled = True
            Dim frm As New frmSubmitException() With {.Exception = DirectCast(e.Exception, Exception)}
            frm.ShowDialog()
        End If
    End Sub

    Private Sub AppDomain_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        If Not IsHandled Then
            IsHandled = True
            Dim frm As New frmSubmitException() With {.Exception = DirectCast(e.ExceptionObject, Exception)}
            frm.ShowDialog()
        End If
    End Sub

    Private Sub Application_DispatcherUnhandledException(sender As Object, e As Windows.Threading.DispatcherUnhandledExceptionEventArgs) Handles Me.DispatcherUnhandledException
        e.Handled = True
        If Not IsHandled Then
            IsHandled = True
            Dim frm As New frmSubmitException() With {.Exception = e.Exception}
            frm.ShowDialog()
        End If
    End Sub

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf AppDomain_UnhandledException
        AddHandler Dispatcher.UnhandledException, AddressOf Dispatcher_UnhandledExceptions
    End Sub
#End If
End Class
