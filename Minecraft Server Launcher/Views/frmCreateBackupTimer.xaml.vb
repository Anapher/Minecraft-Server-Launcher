Imports System.IO
Public Class frmCreateBackupTimer
    Implements ComponentModel.INotifyPropertyChanged

    Private _settings As New Settings
    Public Sub New(settings As Settings, RootPath As DirectoryInfo)
        Me.New(settings, New TimerCreateBackup, RootPath)
        BackupEdit = False
    End Sub

    Public Sub New(settings As Settings, BackupTimer As TimerCreateBackup, RootPath As DirectoryInfo)
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me._settings = settings
        Me.BackupTimer = BackupTimer
        Dim lst = New List(Of BackupBase)
        For Each di In RootPath.GetDirectories("*", SearchOption.TopDirectoryOnly)
            If Not di.Name = "MSL" Then
                Dim b = New BackupFolder()
                b.IsChecked = False
                Dim ffromlst As BackupFolder = Nothing
                For Each f In BackupTimer.BackupFiles
                    If f.Path = di.Name Then
                        b.IsChecked = True
                        If TypeOf f Is BackupFolder Then ffromlst = DirectCast(f, BackupFolder)
                        Exit For
                    End If
                Next

                b.Name = di.Name
                b.Path = di.Name
                b.BackupFiles = BackupManager.GetFilesFromFolder(di, di.Name)
                If ffromlst IsNot Nothing Then

                    For Each f In b.BackupFiles
                        For Each ff In ffromlst.BackupFiles
                            If f.Path = ffromlst.Path Then
                                f.IsChecked = True
                                If TypeOf f Is BackupFolder Then CheckFiles(DirectCast(f, BackupFolder), DirectCast(ff, BackupFolder))
                                Exit For
                            End If
                        Next
                    Next

                End If

                lst.Add(b)
            End If
        Next
        For Each fi In RootPath.GetFiles("*.*", SearchOption.TopDirectoryOnly)
            If Not fi.Extension = ".exe" Then
                Dim b = New BackupFile()
                b.IsChecked = False
                For Each f In BackupTimer.BackupFiles
                    If f.Path = fi.Name Then b.IsChecked = True : Exit For
                Next
                b.Name = fi.Name
                b.Path = fi.Name
                lst.Add(b)
            End If
        Next
        Me.BackupFiles = lst
        BackupEdit = True
    End Sub

    Private Sub CheckFiles(di As BackupFolder, difromlist As BackupFolder)
        For Each f In di.BackupFiles
            For Each ff In difromlist.BackupFiles
                If f.Path = ff.Path Then
                    f.IsChecked = True
                    If TypeOf f Is BackupFolder Then CheckFiles(DirectCast(f, BackupFolder), DirectCast(ff, BackupFolder))
                    Exit For
                End If
            Next
        Next
    End Sub

    Private _BackupEdit As Boolean
    Public Property BackupEdit() As Boolean
        Get
            Return _BackupEdit
        End Get
        Set(ByVal value As Boolean)
            _BackupEdit = value
        End Set
    End Property

    Private _BackupTimer As TimerCreateBackup
    Public Property BackupTimer() As TimerCreateBackup
        Get
            Return _BackupTimer
        End Get
        Set(ByVal value As TimerCreateBackup)
            If value IsNot _BackupTimer Then
                _BackupTimer = value
                myPropertyChanged("BackupTimer")
            End If
        End Set
    End Property

    Private _BackupFiles As List(Of BackupBase)
    Public Property BackupFiles() As List(Of BackupBase)
        Get
            Return _BackupFiles
        End Get
        Set(ByVal value As List(Of BackupBase))
            If value IsNot _BackupFiles Then
                _BackupFiles = value
                myPropertyChanged("BackupFiles")
            End If
        End Set
    End Property

    Private _createBackupCommand As RelayCommand
    Public ReadOnly Property CreateBackupCommand As RelayCommand
        Get
            If _createBackupCommand Is Nothing Then _createBackupCommand = New RelayCommand(AddressOf CreateBackup)
            Return _createBackupCommand
        End Get
    End Property

    Private Sub CreateBackup(parameter As Object)
        If Not Me.BackupEdit Then Me._settings.AddTimer(BackupTimer)
        BackupTimer.BackupFiles = RemoveUncheckedFiles(BackupFiles)
        Me.Close()
    End Sub

    Private Function RemoveUncheckedFiles(Backupfiles As List(Of BackupBase)) As List(Of BackupBase)
        For i = Backupfiles.Count - 1 To 0 Step -1
            Dim b = Backupfiles(i)
            If TypeOf b Is BackupFile Then
                If Not b.IsChecked Then Backupfiles.RemoveAt(i)
            ElseIf TypeOf b Is BackupFolder Then
                For a = b.BackupFiles.Count - 1 To 0 Step -1
                    Dim backupfile = b.BackupFiles(a)
                    If TypeOf backupfile Is BackupFile Then
                        If Not backupfile.IsChecked Then b.BackupFiles.RemoveAt(a)
                    ElseIf TypeOf backupfile Is BackupFolder Then
                        BackupManager.RemoveAllUnCheckedFiles(DirectCast(backupfile, BackupFolder), DirectCast(b, BackupFolder))
                    End If
                Next
                If b.BackupFiles.Count = 0 Then Backupfiles.Remove(b)
            End If
        Next
        Return Backupfiles
    End Function

    Protected Sub myPropertyChanged(propertyname As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(propertyname))
    End Sub

    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
