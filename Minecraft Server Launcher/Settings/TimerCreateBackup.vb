Imports System.Windows.Threading
Imports System.Timers

Public Class TimerCreateBackup
    Inherits TimerBase

    Private _BackupManager As BackupManager
    Public Overrides Sub Load(BackupManager As Object)
        _timer = New Timer
        With _timer
            RefreshInterval()
            .Enabled = IsEnabled
            AddHandler .Elapsed, AddressOf DoBackup
        End With
        Me._BackupManager = DirectCast(BackupManager, BackupManager)
    End Sub

    Private Sub DoBackup(sender As Object, e As EventArgs)
        Dim backup As New Backup
        With backup
            .BackupFiles = BackupFiles
            .CreateType = CreateType.Automatically
        End With
        _BackupManager.CreateBackup(backup, New IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory), True, Sub()
                                                                                                                   _BackupManager.LoadBackups()
                                                                                                               End Sub, False)
    End Sub

    Private _BackupFiles As List(Of BackupBase)
    Public Property BackupFiles() As List(Of BackupBase)
        Get
            Return _BackupFiles
        End Get
        Set(ByVal value As List(Of BackupBase))
            SetProperty(value, _BackupFiles)
        End Set
    End Property

    Public Sub New()
        Me.TaskMode = Minecraft_Server_Launcher.TaskMode.CreateBackup
        Me.BackupFiles = New List(Of BackupBase)
    End Sub

    Protected Overrides Sub RefreshInterval()
        _timer.Interval = Me.Interval * 60000
    End Sub
End Class
