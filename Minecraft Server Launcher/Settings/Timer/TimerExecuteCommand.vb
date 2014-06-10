Imports System.Windows.Threading
Imports System.Timers

<Serializable> _
Public Class TimerExecuteCommand
    Inherits TimerBase

#Region "Function"
    Private _executecommand As SendCommand

    Public Overrides Sub Load(command As Object)
        Me._executecommand = DirectCast(command, SendCommand)
        _timer = New Timer
        With _timer
            RefreshInterval()
            .Enabled = IsEnabled
            AddHandler .Elapsed, AddressOf TimerTick
        End With
    End Sub

    Private Sub TimerTick(sender As Object, e As EventArgs)
        _executecommand.Invoke(Me.Command)
    End Sub
#End Region

    Private _command As String
    Public Property Command() As String
        Get
            Return _command
        End Get
        Set(ByVal value As String)
            SetProperty(value, _command)
        End Set
    End Property

    Public Sub New()
        Me.TaskMode = TaskMode.ExecuteCommand
    End Sub

    Protected Overrides Sub RefreshInterval()
        _timer.Interval = Me.Interval * 1000
    End Sub
End Class