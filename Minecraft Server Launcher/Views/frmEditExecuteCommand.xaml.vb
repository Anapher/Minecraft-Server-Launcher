Public Class frmEditExecuteCommand
    Implements ComponentModel.INotifyPropertyChanged

    Private _ExecuteCommandTimer As TimerExecuteCommand
    Public Property ExecuteCommandTimer() As TimerExecuteCommand
        Get
            Return _ExecuteCommandTimer
        End Get
        Set(ByVal value As TimerExecuteCommand)
            If value IsNot _ExecuteCommandTimer Then
                _ExecuteCommandTimer = value
                myPropertyChanged("ExecuteCommandTimer")
            End If
        End Set
    End Property

    Public Sub New(ExecuteCommandTimer As TimerExecuteCommand)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.ExecuteCommandTimer = ExecuteCommandTimer
    End Sub

    Public ReadOnly Property OkCommand() As RelayCommand
        Get
            Return New RelayCommand(Sub()
                                        Me.Close()
                                    End Sub)
        End Get
    End Property

    Protected Sub myPropertyChanged(propertyname As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(propertyname))
    End Sub

    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
