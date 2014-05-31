Imports System.ComponentModel
Imports Exceptionless

Public Class frmSubmitException
    Implements INotifyPropertyChanged

    Private _exception As Exception
    Public Property Exception As Exception
        Get
            Return _exception
        End Get
        Set(value As Exception)
            If value IsNot _exception Then
                _exception = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Exception"))
            End If
        End Set
    End Property

    Private _txtNote As String
    Public Property txtNote() As String
        Get
            Return _txtNote
        End Get
        Set(ByVal value As String)
            If value <> _txtNote Then
                _txtNote = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("txtNote"))
            End If
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private Sub ButtonSendReport_Click(sender As Object, e As RoutedEventArgs)
        If Not String.IsNullOrWhiteSpace(txtNote) Then Exception.ToExceptionless().SetUserDescription(txtNote).Submit() Else Exception.ToExceptionless().Submit()
        ExceptionlessClient.Current.ProcessQueue()
        Application.Current.Shutdown()
    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As RoutedEventArgs)
        Application.Current.Shutdown()
    End Sub
End Class
