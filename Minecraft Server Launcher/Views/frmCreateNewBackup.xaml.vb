Imports System.IO

Public Class frmCreateNewBackup
    Implements ComponentModel.INotifyPropertyChanged

    Private _Backup As Backup
    Public Property Backup() As Backup
        Get
            Return _Backup
        End Get
        Set(ByVal value As Backup)
            If value IsNot _Backup Then
                _Backup = value
                myPropertyChanged("Backup")
            End If
        End Set
    End Property

    Private _RootPath As DirectoryInfo
    Private _BackupManager As BackupManager
    Private _eventhandler As EventHandler
    Public Sub New(RootPath As DirectoryInfo, BackupManager As BackupManager, BackupFinishedEvent As EventHandler)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Dim lst As New List(Of BackupBase)
        For Each di In RootPath.GetDirectories("*", SearchOption.TopDirectoryOnly)
            If Not di.Name = "MSL" Then
                Dim b = New BackupFolder()
                b.IsChecked = False
                b.Name = di.Name
                b.Path = di.Name
                b.BackupFiles = BackupManager.GetFilesFromFolder(di, di.Name)
                lst.Add(b)
            End If
        Next
        For Each fi In RootPath.GetFiles("*.*", SearchOption.TopDirectoryOnly)
            If Not fi.Extension = ".exe" Then
                Dim b = New BackupFile()
                b.IsChecked = False
                b.Name = fi.Name
                b.Path = fi.Name
                lst.Add(b)
            End If
        Next
        Dim newbackup As New Backup
        With newbackup
            .BackupFiles = lst
            .UpdateEventHandler()
        End With
        Me.Backup = newbackup
        _RootPath = RootPath
        _BackupManager = BackupManager
        _eventhandler = BackupFinishedEvent
    End Sub

    Private _createbackupcommand As RelayCommand
    Public ReadOnly Property CreateBackupCommand() As RelayCommand
        Get
            If _createbackupcommand Is Nothing Then _createbackupcommand = New RelayCommand(AddressOf CreateBackup)
            Return _createbackupcommand
        End Get
    End Property

    Private Sub CreateBackup(parameter As Object)
        Backup.CreateType = CreateType.manually
        For i = Backup.BackupFiles.Count - 1 To 0 Step -1
            Dim b = Backup.BackupFiles(i)
            If TypeOf b Is BackupFile Then
                If Not b.IsChecked Then Backup.BackupFiles.RemoveAt(i)
            ElseIf TypeOf b Is BackupFolder Then
                For a = b.BackupFiles.Count - 1 To 0 Step -1
                    Dim backupfile = b.BackupFiles(a)
                    If TypeOf backupfile Is BackupFile Then
                        If Not backupfile.IsChecked Then b.BackupFiles.RemoveAt(a)
                    ElseIf TypeOf backupfile Is BackupFolder Then
                        BackupManager.RemoveAllUnCheckedFiles(DirectCast(backupfile, BackupFolder), DirectCast(b, BackupFolder))
                    End If
                Next
                If b.BackupFiles.Count = 0 Then Backup.BackupFiles.Remove(b)
            End If
        Next
        _BackupManager.CreateBackup(Backup, _RootPath, False, _eventhandler)
        DialogResult = True
        Me.Close()
    End Sub

    Private Sub myPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(PropertyName))
    End Sub

    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
