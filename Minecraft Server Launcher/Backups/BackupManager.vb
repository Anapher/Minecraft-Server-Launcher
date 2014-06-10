Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Core
Imports System.Runtime.InteropServices

Public Class BackupManager
    Inherits PropertyChangedBase

#Region "Constrcutor"
    Private _path As DirectoryInfo
    Public Sub New(Path As String)
        _path = New DirectoryInfo(Path)
    End Sub
    Public Sub New(Path As DirectoryInfo)
        _path = Path
    End Sub
#End Region

#Region "Properties"
    Private _BackupList As List(Of Backup)
    Public Property BackupList() As List(Of Backup)
        Get
            Return _BackupList
        End Get
        Set(ByVal value As List(Of Backup))
            SetProperty(value, _BackupList)
        End Set
    End Property

    Private _Progress As Integer
    Public Property Progress() As Integer
        Get
            Return _Progress
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _Progress)
        End Set
    End Property

#End Region

    Private _RestoreAllFiles As Integer = 0

    Private Function GetAllFilesFromFolder(folder As BackupFolder) As List(Of BackupFile)
        Dim lst As New List(Of BackupFile)
        For Each f In folder.BackupFiles
            If TypeOf f Is BackupFile Then
                lst.Add(DirectCast(f, BackupFile))
            ElseIf TypeOf f Is BackupFolder Then
                lst.AddRange(GetAllFilesFromFolder(DirectCast(f, BackupFolder)))
            End If
        Next
        Return lst
    End Function

#Region "Shared"
    Public Shared Function GetAllCheckedFiles(backup As Backup) As List(Of BackupBase)
        Dim lstresult = New List(Of BackupBase)
        For Each f In backup.BackupFiles
            If TypeOf f Is BackupFile Then
                If f.IsChecked Then lstresult.Add(DirectCast(f, BackupFile))
            ElseIf TypeOf f Is BackupFolder Then
                lstresult.AddRange(GetCheckedFilesFromFolder(DirectCast(f, BackupFolder)))
            End If
        Next
        Return lstresult
    End Function

    Private Shared Function GetCheckedFilesFromFolder(di As BackupFolder) As List(Of BackupBase)
        Dim lstresult = New List(Of BackupBase)
        Dim folderisadded As Boolean = False
        For Each f In di.BackupFiles
            If TypeOf f Is BackupFile Then
                If f.IsChecked Then
                    If Not folderisadded Then lstresult.Add(di) : folderisadded = True
                    lstresult.Add(DirectCast(f, BackupFile))
                End If
            ElseIf TypeOf f Is BackupFolder Then
                lstresult.AddRange(GetCheckedFilesFromFolder(DirectCast(f, BackupFolder)))
            End If
        Next
        Return lstresult
    End Function

    Public Shared Function SearchFiles(b As Backup, root As String) As List(Of FileInfo)
        Dim lstresult = New List(Of FileInfo)
        For Each f In b.BackupFiles
            If TypeOf f Is BackupFile Then
                Dim fi As New FileInfo(Path.Combine(root, f.Path))
                If fi.Exists Then
                    lstresult.Add(fi)
                End If
            End If
        Next
        Return lstresult
    End Function

    Public Shared Function SearchFolder(b As Backup, root As String) As List(Of DirectoryInfo)
        Dim lstresult = New List(Of DirectoryInfo)
        For Each f In b.BackupFiles
            If TypeOf f Is BackupFolder Then
                Dim di As New DirectoryInfo(Path.Combine(root, f.Path))
                If di.Exists Then
                    lstresult.Add(di)
                End If
            End If
        Next
        Return lstresult
    End Function

    Public Shared Function GetFilesFromFolder(di As DirectoryInfo, oldpath As String) As List(Of BackupBase)
        Dim lst As New List(Of BackupBase)
        For Each d In di.GetDirectories("*", SearchOption.TopDirectoryOnly)
            Dim x = New BackupFolder() With {.Name = d.Name, .Path = Path.Combine(oldpath, d.Name)}
            x.BackupFiles = GetFilesFromFolder(d, x.Path)
            lst.Add(x)
        Next
        For Each fi In di.GetFiles("*.*", SearchOption.TopDirectoryOnly)
            Dim x = New BackupFile() With {.Name = fi.Name, .Path = Path.Combine(oldpath, fi.Name)}
            lst.Add(x)
        Next
        Return lst
    End Function

    Public Shared Sub RemoveAllUnCheckedFiles(folder As BackupFolder, folderbefore As BackupFolder)
        For a = folder.BackupFiles.Count - 1 To 0 Step -1
            Dim backupfile = folder.BackupFiles(a)
            If TypeOf backupfile Is BackupFile Then
                If Not backupfile.IsChecked Then folder.BackupFiles.RemoveAt(a)
            ElseIf TypeOf backupfile Is BackupFolder Then
                RemoveAllUnCheckedFiles(DirectCast(backupfile, BackupFolder), folder)
            End If
        Next
        If folder.BackupFiles.Count = 0 Then folderbefore.BackupFiles.Remove(folder)
    End Sub
#End Region

    Public Event StatusChanged(sender As Object, e As StateChangedEventArgs)

    Public Sub RestoreBackup(BackupToRestore As Backup, Files As List(Of BackupBase), OutPath As String, Optional FinishedEvent As EventHandler = Nothing)
        _RestoreAllFiles = 0
        Dim counter = 0
        Dim t As New System.Threading.Thread(Sub()
                                                 Dim lstFiles As New List(Of BackupFile)
                                                 For Each f In Files
                                                     If TypeOf f Is BackupFile Then
                                                         Dim fi As New FileInfo(Path.Combine(OutPath, f.Name))
                                                         If fi.Exists Then fi.Delete()
                                                         lstFiles.Add(DirectCast(f, BackupFile))
                                                     ElseIf TypeOf f Is BackupFolder Then
                                                         Dim di As New DirectoryInfo(Path.Combine(OutPath, f.Name))
                                                         If di.Exists Then
                                                             Try
                                                                 di.Delete(True)
                                                             Catch ex As Exception
                                                             End Try
                                                         End If
                                                         lstFiles.AddRange(GetAllFilesFromFolder(DirectCast(f, BackupFolder)))
                                                     End If
                                                 Next
                                                 _RestoreAllFiles = BackupToRestore.BackupFilesCount
                                                 Using fs As FileStream = BackupToRestore.BackupPath.OpenRead()
                                                     Dim zf= New ZipFile(fs)
                                                         For Each ze As ZipEntry In zf
                                                             Dim entryFileName As String = ze.Name

                                                             For Each f In lstFiles
                                                                 If entryFileName = ZipEntry.CleanName(f.Path) Then
                                                                     Dim buffer As Byte() = New Byte(4095) {}
                                                                     Dim zipStream As Stream = zf.GetInputStream(ze)

                                                                     Dim fullzipToPath = New FileInfo(Path.Combine(OutPath, f.Path))
                                                                     If Not fullzipToPath.Directory.Exists Then
                                                                         fullzipToPath.Directory.Create()
                                                                     End If
                                                                     Using streamWriter As FileStream = fullzipToPath.Create()
                                                                         StreamUtils.Copy(zipStream, streamWriter, buffer)
                                                                     End Using
                                                                     counter += 1
                                                                     RefreshProgress(_RestoreAllFiles, counter, "RestoreBackupStatus")
                                                                     Exit For
                                                                 End If
                                                             Next

                                                         Next
                                                         zf.IsStreamOwner = True
                                                         zf.Close()
                                                 End Using
                                                 Application.Current.Dispatcher.Invoke(Sub()
                                                                                           If FinishedEvent IsNot Nothing Then FinishedEvent.Invoke(Me, EventArgs.Empty)
                                                                                           Progress = 0
                                                                                       End Sub)
                                             End Sub)
        t.IsBackground = True
        t.Start()
    End Sub

    Public Sub LoadBackups()
        Dim newbackuplist As New List(Of Backup)
        For Each fi In _path.GetFiles("*.MSLBackup")
            Using fs As FileStream = fi.OpenRead()
                Try
                    Using zf As New ZipFile(fs)
                        For Each ze As ZipEntry In zf
                            If ze.IsDirectory Then Continue For
                            If ze.Name = "Backup.info" Then
                                Using s As Stream = zf.GetInputStream(ze)
                                    Dim buffer As Byte() = New Byte(4096) {}

                                    Using ms As New MemoryStream
                                        StreamUtils.Copy(s, ms, buffer)
                                        ms.Position = 0
                                        Dim backup As Backup
                                        Dim xmls As New Xml.Serialization.XmlSerializer(GetType(Backup))
                                        backup = DirectCast(xmls.Deserialize(ms), Backup)
                                        backup.BackupPath = fi
                                        newbackuplist.Add(backup)
                                        For Each f In backup.BackupFiles
                                            f.IsChecked = True
                                        Next
                                        backup.UpdateEventHandler()
                                    End Using
                                End Using
                            End If
                        Next
                    End Using
                Catch ex As Exception
                End Try
            End Using
        Next
        Me.BackupList = newbackuplist
    End Sub

    Private _counter As Integer

    Private Sub GetInfosFromDirectory(f As DirectoryInfo, ByRef files As List(Of FileInfo), ByRef folder As List(Of DirectoryInfo))
        For Each fi In f.GetFiles("*.*", SearchOption.TopDirectoryOnly)
            files.Add(fi)
        Next
        For Each di In f.GetDirectories("*", SearchOption.TopDirectoryOnly)
            folder.Add(di)
            GetInfosFromDirectory(di, files, folder)
        Next
    End Sub

    Public Sub CreateBackup(Backup As Backup, root As DirectoryInfo, BackupUncheckedFiles As Boolean, Optional FinishedEvent As EventHandler = Nothing, Optional RaiseEvents As Boolean = True)
        _counter = 0
        Dim t As New System.Threading.Thread(Sub()
                                                 Dim lstFiles As New List(Of FileInfo)
                                                 Dim lstFolder As New List(Of DirectoryInfo)
                                                 For Each f In Backup.BackupFiles
                                                     If Not BackupUncheckedFiles AndAlso Not f.IsChecked Then Continue For
                                                     If TypeOf f Is BackupFile Then
                                                         lstFiles.Add(New FileInfo(Path.Combine(root.FullName, f.Path)))
                                                     ElseIf TypeOf f Is BackupFolder Then
                                                         GetInfosFromDirectory(New DirectoryInfo(Path.Combine(root.FullName, f.Path)), lstFiles, lstFolder)
                                                     End If
                                                 Next
                                                 With Backup
                                                     .CreateDate = DateTime.Now
                                                     Dim allfilesize As Long
                                                     Dim filist As New List(Of BackupBase)
                                                     For Each di In lstFolder
                                                         Dim x = New BackupFolder() With {.Name = di.Name}
                                                         x.BackupFiles = GetFilesFromFolder(di, x.Name)
                                                         x.Path = x.Name
                                                         filist.Add(x)
                                                         For Each fi In di.GetFiles("*.*", SearchOption.AllDirectories)
                                                             allfilesize += fi.Length
                                                         Next
                                                     Next
                                                     For Each fi In lstFiles
                                                         allfilesize += fi.Length
                                                         Dim x = New BackupFile() With {.Name = fi.Name, .Path = fi.Name}
                                                         filist.Add(x)
                                                     Next
                                                     .BackupSize = allfilesize
                                                 End With
                                                 Dim outPathname As New FileInfo(Path.Combine(_path.FullName, DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") & ".MSLBackup"))
                                                 Using fsOut As FileStream = File.Create(outPathname.FullName)
                                                     Dim zipStream As New ZipOutputStream(fsOut)
                                                     zipStream.SetLevel(3)
                                                     zipStream.Password = Nothing

                                                     Dim AllFiles = lstFiles.Count
                                                     Dim folderOffset As Integer = root.FullName.Length + (If(root.FullName.EndsWith("\"), 0, 1))
                                                     If lstFiles IsNot Nothing AndAlso lstFiles.Count > 0 Then
                                                         For Each fi In lstFiles
                                                             If Not CompressFile(fi, zipStream, folderOffset) Then
                                                                 zipStream.IsStreamOwner = True
                                                                 zipStream.Close()
                                                                 outPathname.Delete()
                                                                 Application.Current.Dispatcher.Invoke(Sub()
                                                                                                           If FinishedEvent IsNot Nothing Then FinishedEvent.Invoke(Me, EventArgs.Empty)
                                                                                                           Progress = 0
                                                                                                       End Sub)
                                                                 Return
                                                             End If
                                                             _counter += 1
                                                             If RaiseEvents Then RefreshProgress(AllFiles, _counter, "CreateBackupStatus")
                                                         Next
                                                     End If

                                                     AddBackupInfoFile(Backup, zipStream)
                                                     zipStream.IsStreamOwner = True
                                                 End Using
                                                 Application.Current.Dispatcher.Invoke(Sub()
                                                                                           If FinishedEvent IsNot Nothing Then FinishedEvent.Invoke(Me, EventArgs.Empty)
                                                                                           Progress = 0
                                                                                       End Sub)
                                             End Sub)
        t.IsBackground = True
        t.Start()
    End Sub

    Private Sub AddBackupInfoFile(Backup As Backup, ZipStream As ZipOutputStream)
        Dim xmls As New Xml.Serialization.XmlSerializer(GetType(Backup))
        Dim newEntry As New ZipEntry("Backup.info")


        Using ms As New MemoryStream()
            xmls.Serialize(ms, Backup)
            newEntry.DateTime = DateTime.Now
            newEntry.Size = ms.Length

            ZipStream.PutNextEntry(newEntry)
            ms.Position = 0
            StreamUtils.Copy(ms, ZipStream, New Byte(4095) {})
            ZipStream.CloseEntry()
        End Using
    End Sub

    Private Sub RefreshProgress(AllFiles As Integer, counter As Integer, StatusTextResource As String)
        Application.Current.Dispatcher.Invoke(Sub()
                                                  Me.Progress = counter * 100 \ AllFiles
                                                  RaiseEvent ProgressChanged(Me, EventArgs.Empty)
                                                  RaiseEvent StatusChanged(Me, New StateChangedEventArgs(String.Format(Application.Current.FindResource(StatusTextResource).ToString(), counter.ToString(), AllFiles.ToString())))
                                              End Sub)
    End Sub

    Public Event ProgressChanged(sender As Object, e As EventArgs)

    Private Function CompressFile(file As FileInfo, zipStream As ZipOutputStream, folderOffset As Integer) As Boolean
        ' If Not FileIsReadable(file) Then
        'Dim result As Boolean?
        'Application.Current.Dispatcher.Invoke(Sub()
        'Dim frm As New frmMessageBox(String.Format(Application.Current.FindResource("BackupException").ToString(), Environment.NewLine, file.Name), Application.Current.FindResource("Exception").ToString())
        'result = frm.ShowDialog()
        '                                     End Sub)
        'If Not result Then Return False
        'End If
        file.Refresh()
        Dim entryName As String
        If folderOffset > 0 Then
            entryName = file.FullName.Substring(folderOffset)
        Else
            entryName = file.Name
        End If
        entryName = ZipEntry.CleanName(entryName)
        Dim newEntry As New ZipEntry(entryName)
        newEntry.DateTime = file.LastWriteTime
        newEntry.Size = file.Length



        zipStream.PutNextEntry(newEntry)

        Dim buffer As Byte() = New Byte(4095) {}
        Try
            Using StreamReader As FileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                StreamUtils.Copy(StreamReader, zipStream, buffer)
            End Using
            zipStream.CloseEntry()
        Catch ex As Exception
            Dim result As Boolean?
            Application.Current.Dispatcher.Invoke(Sub()
                                                      Dim frm As New frmMessageBox(String.Format(Application.Current.FindResource("BackupException").ToString(), Environment.NewLine, file.Name), Application.Current.FindResource("Exception").ToString()) With {.Owner = Application.Current.MainWindow}
                                                      result = frm.ShowDialog()
                                                  End Sub)
            If Not result Then Return False
        End Try
        Return True
    End Function

    Private Function FileIsReadable(fi As FileInfo) As Boolean
        Try
            fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Return True
        Catch ex As IO.IOException
            Return False
        End Try
    End Function

    Public Sub RemoveBackup(backup As Backup)
        If backup.BackupPath.Exists Then
            backup.BackupPath.Delete()
        End If
        Me.BackupList.Remove(backup)
        LoadBackups()
    End Sub
End Class
