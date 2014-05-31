Imports System.Timers

<Serializable> _
<Xml.Serialization.XmlInclude(GetType(TimerExecuteCommand))> _
<Xml.Serialization.XmlInclude(GetType(TimerCreateBackup))> _
Public MustInherit Class TimerBase
    Inherits PropertyChangedBase

    Private _Interval As Integer
    Public Property Interval() As Integer
        Get
            Return _Interval
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _Interval)
            If _timer IsNot Nothing Then RefreshInterval()
        End Set
    End Property

    Protected MustOverride Sub RefreshInterval()

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            SetProperty(value, _name)
        End Set
    End Property

    Public ReadOnly Property TaskDescription As String
        Get
            Select Case Me.TaskMode
                Case Minecraft_Server_Launcher.TaskMode.CreateBackup
                    Return Application.Current.FindResource("CreateBackup").ToString()
                Case Minecraft_Server_Launcher.TaskMode.ExecuteCommand
                    Return Application.Current.FindResource("ExecuteCommand").ToString()
                Case Else
                    Return String.Empty
            End Select
        End Get
    End Property

    Private _TaskMode As TaskMode
    Public Property TaskMode() As TaskMode
        Get
            Return _TaskMode
        End Get
        Set(ByVal value As TaskMode)
            _TaskMode = value
        End Set
    End Property

    Public MustOverride Sub Load(parameter As Object)

    Protected _timer As Timer
    Private _IsEnabled As Boolean
    Public Property IsEnabled As Boolean
        Get
            Return _IsEnabled
        End Get
        Set(value As Boolean)
            SetProperty(value, _IsEnabled)
            If _timer IsNot Nothing Then _timer.Enabled = value
        End Set
    End Property

    Private _editCommand As RelayCommand
    Public ReadOnly Property EditCommand As RelayCommand
        Get
            If _editCommand Is Nothing Then _editCommand = New RelayCommand(AddressOf EditTimer)
            Return _editCommand
        End Get
    End Property

    Public Event Edit(sender As Object, e As EventArgs)

    Private Sub EditTimer(parameter As Object)
        RaiseEvent Edit(Me, EventArgs.Empty)
    End Sub
End Class

Public Enum TaskMode
    ExecuteCommand
    CreateBackup
End Enum