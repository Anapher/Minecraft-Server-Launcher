Imports System.Collections.ObjectModel

<Serializable> _
Public Class Settings
    Inherits PropertyChangedBase

#Region "Properties"
    Private _CheckForUpdatesOnStart As Boolean
    Public Property CheckForUpdatesOnStart() As Boolean
        Get
            Return _CheckForUpdatesOnStart
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _CheckForUpdatesOnStart)
        End Set
    End Property

    Private _RefreshInterval As Integer
    Public Property RefreshInterval() As Integer
        Get
            Return _RefreshInterval
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _RefreshInterval)
        End Set
    End Property

    Private _JavaPath As String
    Public Property JavaPath() As String
        Get
            Return _JavaPath
        End Get
        Set(ByVal value As String)
            SetProperty(value, _JavaPath)
        End Set
    End Property

    Private _stopServerOnClose As Boolean
    Public Property StopServerOnClose() As Boolean
        Get
            Return _stopServerOnClose
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _stopServerOnClose)
        End Set
    End Property

    Private _AskSaveSettings As Boolean
    Public Property AskSaveSettings() As Boolean
        Get
            Return _AskSaveSettings
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _AskSaveSettings)
        End Set
    End Property

    Private _BackupFilesFirst As Boolean
    Public Property BackupFilesFirst() As Boolean
        Get
            Return _BackupFilesFirst
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _BackupFilesFirst)
        End Set
    End Property

    Private _ram As Ram
    Public Property Ram() As Ram
        Get
            Return _ram
        End Get
        Set(ByVal value As Ram)
            SetProperty(value, _ram)
        End Set
    End Property

    Private _lstram As List(Of Ram)
    <Xml.Serialization.XmlIgnore> _
    Public ReadOnly Property lstRam As List(Of Ram)
        Get
            Return _lstram
        End Get
    End Property

    Private _lastLanguage As ResourceDictionary
    Private _ActivLanguage As Language
    Public Property ActivLanguage() As Language
        Get
            Return _ActivLanguage
        End Get
        Set(ByVal value As Language)
            SetProperty(value, _ActivLanguage)
            Dim dic = New ResourceDictionary() With {.Source = New Uri(value.Path, UriKind.Relative)}
            If _lastLanguage IsNot Nothing Then
                Application.Current.Resources.Remove(_lastLanguage)
            End If
            Application.Current.Resources.MergedDictionaries.Add(dic)
            _lastLanguage = dic
        End Set
    End Property

    Private _lstLanguages As List(Of Language)
    <Xml.Serialization.XmlIgnore> _
    Public ReadOnly Property lstLanguages As List(Of Language)
        Get
            Return _lstLanguages
        End Get
    End Property

    Private _Timers As ObservableCollection(Of TimerBase)
    Public Property Timers() As ObservableCollection(Of TimerBase)
        Get
            Return _Timers
        End Get
        Set(ByVal value As ObservableCollection(Of TimerBase))
            SetProperty(value, _Timers)
        End Set
    End Property

    Private _IntelliSenseIsEnabled As Boolean
    Public Property IntelliSenseIsEnabled() As Boolean
        Get
            Return _IntelliSenseIsEnabled
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _IntelliSenseIsEnabled)
        End Set
    End Property

    Private _EnableIntelliSenseInfoBox As Boolean
    Public Property EnableIntelliSenseInfoBox() As Boolean
        Get
            Return _EnableIntelliSenseInfoBox
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _EnableIntelliSenseInfoBox)
        End Set
    End Property
#End Region

#Region "Methods"
    Private _SendCommanddelegate As SendCommand
    Private _BackupManager As BackupManager
    Public Sub LoadTimer()
        For Each t In Timers
            Select Case t.TaskMode
                Case TaskMode.ExecuteCommand
                    t.Load(_SendCommanddelegate)
                Case TaskMode.CreateBackup
                    t.Load(_BackupManager)
            End Select
            AddHandler t.Edit, AddressOf EditTimer
        Next
    End Sub

    Public Sub RemoveTimer(Timer As TimerBase)
        Timer.IsEnabled = False
        RemoveHandler Timer.Edit, AddressOf EditTimer
        Me.Timers.Remove(Timer)
    End Sub

    Public Sub AddTimer(Timer As TimerBase)
        If Not Me.Timers.Contains(Timer) Then
            Me.Timers.Add(Timer)
            AddHandler Timer.Edit, AddressOf EditTimer
        End If
    End Sub

    Private Sub EditTimer(sender As Object, e As EventArgs)
        Select Case True
            Case TypeOf sender Is TimerCreateBackup
                Dim frm As New frmCreateBackupTimer(Me, DirectCast(sender, TimerCreateBackup), New IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)) With {.Owner = Application.Current.MainWindow}
                frm.ShowDialog()
            Case TypeOf sender Is TimerExecuteCommand
                Dim frm As New frmEditExecuteCommand(DirectCast(sender, TimerExecuteCommand)) With {.Owner = Application.Current.MainWindow}
                frm.ShowDialog()
        End Select
    End Sub

    Public Shared Function Load(Path As String, executecommand As SendCommand, BackupManager As BackupManager) As Settings
        Using sr As New IO.StreamReader(Path)
            Dim xmls As New Xml.Serialization.XmlSerializer(GetType(Settings))
            Dim obj As Settings
            Try
                obj = DirectCast(xmls.Deserialize(sr), Settings)
                With obj
                    .Path = Path
                    .Ram = obj.lstRam.Find(Function(x) x.Ram = obj.Ram.Ram)
                    .ActivLanguage = obj.lstLanguages.Find(Function(x) x.Code = obj.ActivLanguage.Code)
                    ._SendCommanddelegate = executecommand
                    ._BackupManager = BackupManager
                    .LoadTimer()
                    .CheckForUpdatesOnStart = True '...
                End With
            Catch ex As Exception
                Dim fi As New IO.FileInfo(Path)
                If fi.Exists Then sr.Close() : fi.Delete()
                obj = New Settings(Path, executecommand, BackupManager)
            End Try
            Return obj
        End Using
    End Function

    Public Sub Save()
        Using sw As New IO.StreamWriter(Path, False)
            Dim xmls As New Xml.Serialization.XmlSerializer(GetType(Settings))
            xmls.Serialize(sw, Me)
        End Using
    End Sub

    Private Path As String
    Public Sub New(path As String, ExecuteCommand As SendCommand, BackupManager As BackupManager)
        _lstram = New List(Of Ram) From {New Ram(256, "256 MB"), New Ram(512, "512 MB"), New Ram(1024, "1 GB"), New Ram(1536, "1.5 GB"), New Ram(2048, "2 GB"), New Ram(2560, "2.5 GB"),
                                         New Ram(3072, "3 GB"), New Ram(4096, "4 GB"), New Ram(5120, "5 GB"), New Ram(6144, "6 GB"), New Ram(8192, "8 GB"), New Ram(12288, "12 GB"), New Ram(16384, "16 GB")}
        _lstLanguages = New List(Of Language) From {New Language("Deutsch", "/resources/languages/MSL.de-de.xaml", "de-de", New Uri("/resources/languages/icons/de.png", UriKind.Relative)), New Language("English", "/resources/languages/MSL.en-us.xaml", "en-us", New Uri("/resources/languages/icons/en.png", UriKind.Relative))}
        Me.Ram = _lstram(2)
        Me.Path = path
        Me.AskSaveSettings = True
        Me.Timers = New ObservableCollection(Of TimerBase)
        Me._SendCommanddelegate = ExecuteCommand
        Me.RefreshInterval = 3000
        Me.BackupFilesFirst = True
        Me._BackupManager = BackupManager
        StopServerOnClose = False
        If Not String.IsNullOrEmpty(path) Then
#If Not Debug Then
        LoadDefaultLanguage()
#End If
        End If
        EnableIntelliSenseInfoBox = True
        IntelliSenseIsEnabled = True
        JavaPath = String.Empty
        Me.CheckForUpdatesOnStart = True
    End Sub

    Private Sub LoadDefaultLanguage()
        Dim currentCultur = System.Threading.Thread.CurrentThread.CurrentCulture
        If currentCultur.TwoLetterISOLanguageName = "de" Then
            Me.ActivLanguage = lstLanguages(0)
        Else
            Me.ActivLanguage = lstLanguages(1)
        End If
    End Sub

    Public Sub New()
        Me.New(String.Empty, Nothing, Nothing)
    End Sub
#End Region
End Class