Imports System.IO

<Serializable> _
Public Class Backup
    Implements ComponentModel.INotifyPropertyChanged

    Private _CreateDate As DateTime
    Public Property CreateDate() As DateTime
        Get
            Return _CreateDate
        End Get
        Set(ByVal value As DateTime)
            _CreateDate = value
        End Set
    End Property

    Private _CreateType As CreateType
    Public Property CreateType() As CreateType
        Get
            Return _CreateType
        End Get
        Set(ByVal value As CreateType)
            _CreateType = value
        End Set
    End Property

    Private _BackupSize As Long
    Public Property BackupSize() As Long
        Get
            Return _BackupSize
        End Get
        Set(ByVal value As Long)
            _BackupSize = value
        End Set
    End Property

    Private _BackupFiles As List(Of BackupBase)
    Public Property BackupFiles() As List(Of BackupBase)
        Get
            Return _BackupFiles
        End Get
        Set(ByVal value As List(Of BackupBase))
            If BackupFiles IsNot value Then
                _BackupFiles = value
                OnPropertyChanged("BackupFiles")
                OnPropertyChanged("BackupFilesCount")
            End If
        End Set
    End Property

    Public ReadOnly Property BackupFilesCount As Integer
        Get
            Dim result = 0
            For Each f In Me.BackupFiles
                If TypeOf f Is BackupFile Then
                    result += 1
                ElseIf TypeOf f Is BackupFolder Then
                    result += GetFileCount(DirectCast(f, BackupFolder))
                End If
            Next
            Return result
        End Get
    End Property

    Private Function GetFileCount(folder As BackupFolder) As Integer
        Dim result = 0
        For Each f In folder.BackupFiles
            If TypeOf f Is BackupFile Then
                result += 1
            ElseIf TypeOf f Is BackupFolder Then
                result += GetFileCount(DirectCast(f, BackupFolder))
            End If
        Next
        Return result
    End Function

    Public Sub UpdateEventHandler()
        For Each f In Me.BackupFiles
            If TypeOf f Is BackupFile Then
                AddHandler f.CheckedChanged, AddressOf CheckedChanged
            ElseIf TypeOf f Is BackupFolder Then
                AddCheckedEventHandler(DirectCast(f, BackupFolder))
            End If
        Next
    End Sub

    Private Sub AddCheckedEventHandler(di As BackupFolder)
        For Each f In di.BackupFiles
            If TypeOf f Is BackupFile Then
                AddHandler f.CheckedChanged, AddressOf CheckedChanged
            ElseIf TypeOf f Is BackupFolder Then
                AddCheckedEventHandler(DirectCast(f, BackupFolder))
            End If
        Next
    End Sub

    Private Sub CheckedChanged(sender As Object, e As EventArgs)
        OnPropertyChanged("SomethingIsChecked")
    End Sub

    Private _BackupPath As FileInfo
    <Xml.Serialization.XmlIgnore> _
    Public Property BackupPath() As FileInfo
        Get
            Return _BackupPath
        End Get
        Set(ByVal value As FileInfo)
            _BackupPath = value
        End Set
    End Property

    Public ReadOnly Property BackupSizeToString As String
        Get
            Select Case BackupSize
                Case Is < 1024
                    Return BackupSize.ToString() & " B"
                Case Is < 1048576
                    Return Math.Round(BackupSize / 1024, 1).ToString() & " KB"
                Case Else
                    Return Math.Round(BackupSize / 1024 / 1024, 1).ToString() & " MB"
            End Select
        End Get
    End Property

    Private _SomethingIsChecked As Boolean
    Public ReadOnly Property SomethingIsChecked As Boolean
        Get
            _SomethingIsChecked = False
            For Each f In Me.BackupFiles
                If TypeOf f Is BackupFile Then
                    If f.IsChecked Then _SomethingIsChecked = True
                ElseIf TypeOf f Is BackupFolder Then
                    CheckIfChecked(DirectCast(f, BackupFolder))
                End If
            Next
            Return _SomethingIsChecked
        End Get
    End Property

    Private Sub CheckIfChecked(di As BackupFolder)
        For Each f In di.BackupFiles
            If TypeOf f Is BackupFile Then
                If f.IsChecked Then _SomethingIsChecked = True
            ElseIf TypeOf f Is BackupFolder Then
                CheckIfChecked(DirectCast(f, BackupFolder))
            End If
        Next
    End Sub

    Protected Sub OnPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(PropertyName))
    End Sub

    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Enum CreateType
    Automatically
    manually
End Enum