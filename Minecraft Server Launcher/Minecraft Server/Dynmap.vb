Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net

Public Class Dynmap
    Inherits PropertyChangedBase

#Region "Properties"
    Private _BrowserURL As String
    Public Property BrowserURL() As String
        Get
            Return _BrowserURL
        End Get
        Set(ByVal value As String)
            SetProperty(value, _BrowserURL)
        End Set
    End Property

    Private _DynmapExists As Boolean = True
    Public Property DynmapExists() As Boolean
        Get
            Return _DynmapExists
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _DynmapExists)
        End Set
    End Property

    Private _DownloadValue As Double
    Public Property DownloadValue() As Double
        Get
            Return _DownloadValue
        End Get
        Set(ByVal value As Double)
            SetProperty(value, _DownloadValue)
        End Set
    End Property

    Private _DownloadIsEnabled As Boolean
    Public Property DownloadIsEnabled() As Boolean
        Get
            Return _DownloadIsEnabled
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _DownloadIsEnabled)
        End Set
    End Property

    Private _ShowDownloadWindow As Boolean = False
    Public Property ShowDownloadWindow() As Boolean
        Get
            Return _ShowDownloadWindow
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ShowDownloadWindow)
        End Set
    End Property

#End Region

#Region "Methods"
    Private _ExecuteCommand As SendCommand
    Private _IP As String
    Private _WorldName As String
    Private _fiDynmap As FileInfo

    Public Sub Load(worldname As String, ip As String)
        _WorldName = worldname
        BrowserURL = String.Format("http://{0}:{1}", ip, GetPort())
        _fiDynmap = New IO.FileInfo(IO.Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "plugins", "dynmap.jar"))
        DynmapExists = _fiDynmap.Exists
        If Not DynmapExists Then DownloadIsEnabled = True : ShowDownloadWindow = True
        _IP = ip
    End Sub

    Public Sub New(ExecuteCommand As SendCommand)
        _ExecuteCommand = ExecuteCommand
    End Sub

    Private Function GetPort() As String
        Dim fi As New FileInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "Plugins", "dynmap", "configuration.txt"))
        If fi.Exists Then
            Dim txt As String = File.ReadAllText(fi.FullName).Replace(vbCr, "").Replace(vbLf, "")
            Dim reg As New Regex("webserver-port: (?<port>(.*?[0-9]{0,5}))")
            Return reg.Matches(txt)(0).Groups("port").Value
        Else
            Return "8123"
        End If
    End Function

    Private Sub DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        DownloadValue = e.ProgressPercentage
    End Sub

    Private Sub DownloadFileCompleted(sender As Object, e As ComponentModel.AsyncCompletedEventArgs)
        DynmapExists = True
        ShowDownloadWindow = False
        _ExecuteCommand("reload")
    End Sub
#End Region
#Region "Command"
    Private _command As RelayCommand
    Public ReadOnly Property Command As RelayCommand
        Get
            If _command Is Nothing Then _command = New RelayCommand(AddressOf ExecuteCommand)
            Return _command
        End Get
    End Property

    Public Sub ExecuteCommand(s As Object)
        Select Case s.ToString()
            Case "Render"
                If DynmapExists Then
                    _ExecuteCommand("dynmap fullrender " & _WorldName)
                End If
            Case "Download"
                Dim client = New WebClient() With {.Proxy = Nothing}
                AddHandler client.DownloadProgressChanged, AddressOf DownloadProgressChanged
                AddHandler client.DownloadFileCompleted, AddressOf DownloadFileCompleted
                DownloadIsEnabled = False
                Dim lnk = PluginHelper.GetLink("http://dev.bukkit.org/bukkit-mods/dynmap/")
                client.DownloadFileAsync(New Uri(lnk), _fiDynmap.FullName)
            Case "OpenInBrowser"
                Process.Start(BrowserURL)
        End Select
    End Sub
#End Region
End Class

Public Delegate Sub SendCommand(command As String)