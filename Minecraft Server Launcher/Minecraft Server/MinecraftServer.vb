Imports System.IO
Imports org.phybros.thrift
Imports System.Threading
Imports System.Collections.ObjectModel
Imports System.IO.Compression
Imports System.Text.RegularExpressions

Public Class MinecraftServer
    Inherits PropertyChangedBase
    Public WithEvents p As Process
    Private CommandWriter As System.IO.StreamWriter
    Private tServerUpdate As Thread
#Region "Events"
    Public Event StateChanged(sender As Object, e As StateChangedEventArgs)
    Public Event ServerChanged(sender As Object, e As EventArgs)
    Public Event StartFileNotFound(sender As Object, e As EventArgs)
    Public Event BannedListChanged(sender As Object, e As EventArgs)
    Public Event DynmapEnabled(sender As Object, e As EventArgs)
    Public Event PlayerChanged(sender As Object, e As EventArgs)
    Public Event LauncherUpdatesFound(sender As Object, e As EventArgs)
#End Region

#Region "Delegats"
    Public Delegate Function JavaPath() As String
#End Region

#Region "Properties"
    Private _Dynmap As Dynmap
    Public Property Dynmap() As Dynmap
        Get
            Return _Dynmap
        End Get
        Set(ByVal value As Dynmap)
            SetProperty(value, _Dynmap)
        End Set
    End Property

    Public ReadOnly Property FullIP As String
        Get
            If ServerSettings IsNot Nothing Then
                Return IP & ":" & ServerSettings.ServerPort.ToString()
            Else
                Return IP
            End If
        End Get
    End Property

    Public ReadOnly Property IP As String
        Get
            Dim result = String.Empty
            If Not String.IsNullOrWhiteSpace(ServerSettings.ServerIP) Then
                result = ServerSettings.ServerIP
            Else
                Dim strHostName As String = ""
                strHostName = System.Net.Dns.GetHostName()
                Dim addr = System.Net.Dns.GetHostEntry(strHostName).AddressList
                result = addr(addr.Length - 3).ToString()
            End If
            Return result
        End Get
    End Property

    Private _ThriftAPI As ThriftAPI
    Public Property ThriftAPI As ThriftAPI
        Get
            Return _ThriftAPI
        End Get
        Set(value As ThriftAPI)
            SetProperty(value, _ThriftAPI)
        End Set
    End Property

    Private _ThriftAPIIsAvailable As Boolean
    Public Property ThriftAPIIsAvailable() As Boolean
        Get
            Return _ThriftAPIIsAvailable
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ThriftAPIIsAvailable)
        End Set
    End Property

    Private _IsRunning As Boolean
    Public Property IsRunning() As Boolean
        Get
            Return _IsRunning
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _IsRunning)
        End Set
    End Property

    Private _server As Server
    Public Property Server As Server
        Get
            Return _server
        End Get
        Set(ByVal value As Server)
            SetProperty(value, _server)
            OnPropertyChanged("PlayersFound")
            'If (_server Is Nothing AndAlso value IsNot Nothing) OrElse _server.Port <> value.Port Then OnPropertyChanged("FullIP")
        End Set
    End Property

    Private _lstPlayers As ObservableCollection(Of Player)
    Public Property lstPlayers() As ObservableCollection(Of Player)
        Get
            Return _lstPlayers
        End Get
        Set(ByVal value As ObservableCollection(Of Player))
            SetProperty(value, _lstPlayers)
        End Set
    End Property

    Private _ServerSettings As ServerSettings
    Public Property ServerSettings() As ServerSettings
        Get
            Return _ServerSettings
        End Get
        Set(ByVal value As ServerSettings)
            SetProperty(value, _ServerSettings)
        End Set
    End Property

    Private _Serverlogs As List(Of ServerlogNode)
    Public Property Serverlogs() As List(Of ServerlogNode)
        Get
            Return _Serverlogs
        End Get
        Set(ByVal value As List(Of ServerlogNode))
            SetProperty(value, _Serverlogs)
        End Set
    End Property

    Private _LauncherSettings As Settings
    Public Property LauncherSettings() As Settings
        Get
            Return _LauncherSettings
        End Get
        Set(ByVal value As Settings)
            SetProperty(value, _LauncherSettings)
        End Set
    End Property

    Private _lstPlugins As ObservableCollection(Of Plugin)
    Public Property lstPlugins() As ObservableCollection(Of Plugin)
        Get
            Return _lstPlugins
        End Get
        Set(ByVal value As ObservableCollection(Of Plugin))
            SetProperty(value, _lstPlugins)
        End Set
    End Property

    Private _Commands As IntelliSenseManager
    Public Property Commands() As IntelliSenseManager
        Get
            Return _Commands
        End Get
        Set(ByVal value As IntelliSenseManager)
            SetProperty(value, _Commands)
        End Set
    End Property

    Private _Banlist As BanlistInfo
    Public Property Banlist() As BanlistInfo
        Get
            Return _Banlist
        End Get
        Set(ByVal value As BanlistInfo)
            SetProperty(value, _Banlist)
        End Set
    End Property

    Private _Whitelist As WhitelistInfo
    Public Property Whitelist() As WhitelistInfo
        Get
            Return _Whitelist
        End Get
        Set(ByVal value As WhitelistInfo)
            SetProperty(value, _Whitelist)
        End Set
    End Property

    Private _BackupManager As BackupManager
    Public Property BackupManager() As BackupManager
        Get
            Return _BackupManager
        End Get
        Set(ByVal value As BackupManager)
            SetProperty(value, _BackupManager)
        End Set
    End Property

    Private _SwiftAPI As SwiftAPIManager
    Public Property SwiftAPI() As SwiftAPIManager
        Get
            Return _SwiftAPI
        End Get
        Set(ByVal value As SwiftAPIManager)
            SetProperty(value, _SwiftAPI)
        End Set
    End Property

    Private _Updater As Updater
    Public Property Updater() As Updater
        Get
            Return _Updater
        End Get
        Set(ByVal value As Updater)
            SetProperty(value, _Updater)
        End Set
    End Property

#End Region

    Public Sub RefreshServerlogs()
        Dim rootPath As New DirectoryInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "logs"))
        If Not rootPath.Exists Then Return
        Me.Serverlogs = MinecraftHelper.GetServerlogs(rootPath)
    End Sub

    Private _GetJavaPath As JavaPath
    Public Sub New(GetJavaPathFunction As JavaPath)
        lstPlayers = New ObservableCollection(Of Player)
        ServerSettings = New ServerSettings(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "server.properties"))
        Dynmap = New Dynmap(AddressOf Me.ExecuteCommand)
        Commands = New IntelliSenseManager(Paths.GetPaths.MSLCommandsFolder)
        Banlist = New BanlistInfo
        Whitelist = New WhitelistInfo
        Dim dir = Paths.GetPaths.MSLBackupsFolder
        If Not dir.Exists Then dir.Create()
        BackupManager = New BackupManager(dir)
        SwiftAPI = New SwiftAPIManager(Paths.GetPaths.MinecraftServerFolder)
        _GetJavaPath = GetJavaPathFunction
    End Sub

    Public Function StartServer() As Boolean
        LoadSettings()
#If Not Debug Then
                Updater = New Updater(LauncherSettings.ActivLanguage.Code)
                If LauncherSettings.CheckForUpdatesOnStart Then CheckForUpdates()
#End If
        BackupManager.LoadBackups()
        Dim fiCraftBukkit As New FileInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "craftbukkit.jar"))
        Dim fiSwiftAPIConfig As New FileInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "plugins", "SwiftApi", "config.yml"))
        If Not fiCraftBukkit.Exists Then RaiseEvent StartFileNotFound(Me, EventArgs.Empty) : Return False
        If Not SwiftAPIManager.Exists(New DirectoryInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "plugins"))) Then
            Dim result As Boolean?
            Application.Current.Dispatcher.Invoke(Sub()
                                                      Application.Current.MainWindow.Visibility = Visibility.Hidden
                                                      Dim frm As New frmMessageBox(Application.Current.FindResource("SAPINFText").ToString(), Application.Current.FindResource("SAPINFTitle").ToString(), Application.Current.FindResource("SAPINFOK").ToString(), Application.Current.FindResource("Close").ToString()) With {.ShowInTaskbar = True, .WindowStartupLocation = WindowStartupLocation.CenterScreen}
                                                      result = frm.ShowDialog()
                                                      Application.Current.MainWindow.Visibility = Visibility.Visible
                                                  End Sub)
            If result Then
                Using client As New Net.WebClient() With {.Proxy = Nothing}
                    Dim pluginFolder As New DirectoryInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "plugins"))
                    If Not pluginFolder.Exists Then pluginFolder.Create()
                    client.DownloadFile("http://dev.bukkit.org/media/files/736/928/SwiftApi.jar", Path.Combine(pluginFolder.FullName, "SwiftApi.jar"))
                End Using
            Else
                Application.Current.Dispatcher.Invoke(Sub() Application.Current.Shutdown())
                Return False
            End If
        End If
        SwiftAPI.Load()
        ThriftAPI = New ThriftAPI(SwiftAPI.Username, SwiftAPI.Password, SwiftAPI.Salt, "localhost", SwiftAPI.Port)
        p = New Process()
        With p.StartInfo
            Dim javapath As String = Helper.GetjavaPath
            If String.IsNullOrEmpty(javapath) Then
                If Not String.IsNullOrEmpty(LauncherSettings.JavaPath) AndAlso File.Exists(LauncherSettings.JavaPath) Then
                    javapath = LauncherSettings.JavaPath
                Else
                    javapath = _GetJavaPath.Invoke()
                    If String.IsNullOrEmpty(javapath) Then Return False
                    LauncherSettings.JavaPath = javapath
                End If
            End If
            .FileName = javapath
            .Arguments = String.Format("-Xmx{0}M -Xms{0}M -jar craftbukkit.jar -nojline", LauncherSettings.Ram.Ram)
            .RedirectStandardError = True
            .RedirectStandardInput = True
            .RedirectStandardOutput = True
            .CreateNoWindow = True
            .UseShellExecute = False
        End With
        p.EnableRaisingEvents = True
        p.Start()
        p.BeginErrorReadLine()
        p.BeginOutputReadLine()
        CommandWriter = p.StandardInput
        IsRunning = True
        Banlist.Load()
        Whitelist.Load()
        tServerUpdate = New Thread(AddressOf UpdateThread)
        tServerUpdate.IsBackground = True
        tServerUpdate.Start()
        ServerSettings.Load()
        Dynmap.Load(ServerSettings.LevelName, IP)
        RefreshServerlogs()
        Return True
    End Function

    Private Sub LoadSettings()
        Dim fiSettings = Paths.GetPaths.MSLSettings
        If fiSettings.Exists Then
            LauncherSettings = Settings.Load(fiSettings.FullName, AddressOf ExecuteCommand, BackupManager)
        Else
            LauncherSettings = New Settings(fiSettings.FullName, AddressOf ExecuteCommand, BackupManager)
        End If
        OnPropertyChanged("FullIP")
    End Sub

    Private Sub RefreshPlugins()
        If ThriftAPI.Functions Is Nothing Then Return
        Dim newlist = New ObservableCollection(Of Plugin)(ThriftAPI.Functions.GetPlugins())
        If lstPlugins Is Nothing OrElse lstPlugins.Count <> newlist.Count Then
            lstPlugins = newlist
            Commands.Load(newlist)
        End If
    End Sub

    Private Sub UpdateThread()
        While IsRunning
            Thread.Sleep(LauncherSettings.RefreshInterval)
            If ThriftAPI.Functions IsNot Nothing Then
                Try
                    Dim oldServer = Server
                    Server = ThriftAPI.Functions.GetServer()
                    If Server Is Nothing Then Continue While
                    RaiseEvent ServerChanged(Me, EventArgs.Empty)
                    Dim compare1to2 = From a In Server.OnlinePlayers From b In lstPlayers.Where(Function(b) b.Name = a.Name).DefaultIfEmpty() Select {a, b}
                    Dim compare2to1 = From a In lstPlayers From b In Server.OnlinePlayers.Where(Function(b) b.Name = a.Name).DefaultIfEmpty() Select {a, b}

                    For Each s In compare1to2
                        If s(1) Is Nothing Then
                            Application.Current.Dispatcher.BeginInvoke(Sub()
                                                                           lstPlayers.Add(s(0))
                                                                           RaiseEvent PlayerChanged(Me, EventArgs.Empty)
                                                                       End Sub)
                        Else
                            Dim a = s(0)
                            With s(1)
                                .Exhaustion = a.Exhaustion
                                .Health = a.Health
                                .FirstPlayed = a.FirstPlayed
                                .FoodLevel = a.FoodLevel
                                .Gamemode = a.Gamemode
                                .HealthDouble = a.HealthDouble
                                .Inventory = a.Inventory
                                .Ip = a.Ip
                                .IsBanned = a.IsBanned
                                .IsInVehicle = a.IsInVehicle
                                .IsOp = a.IsOp
                                .IsSleeping = a.IsSleeping
                                .IsSneaking = a.IsSneaking
                                .IsSprinting = a.IsSprinting
                                .IsWhitelisted = a.IsWhitelisted
                                .LastPlayed = a.LastPlayed
                                .Level = a.Level
                                .LevelProgress = a.LevelProgress
                                .Location = a.Location
                                .Name = a.Name
                                .Port = a.Port
                                .XpToNextLevel = a.XpToNextLevel
                            End With
                        End If
                    Next

                    For i = 0 To compare2to1.Count - 1
                        Dim s = compare2to1(i)
                        If s IsNot Nothing AndAlso s(1) Is Nothing Then
                            Application.Current.Dispatcher.BeginInvoke(Sub()
                                                                           lstPlayers.Remove(s(0))
                                                                           RaiseEvent PlayerChanged(Me, EventArgs.Empty)
                                                                       End Sub)
                        End If
                    Next
                    OnPropertyChanged("lstPlayers")
                Catch ex As Exception
                End Try
            End If
            Banlist.Load()
            Whitelist.Load()
            RefreshPlugins()
        End While
    End Sub

    Public Sub StopServer()
        If ThriftAPI IsNot Nothing Then ThriftAPI.Stop() : ThriftAPIIsAvailable = False
        If p IsNot Nothing AndAlso IsRunning AndAlso Not p.HasExited Then p.Kill()
        IsRunning = False
        LauncherSettings.Save()
    End Sub

    Public Sub ExecuteCommand(command As String)
        CommandWriter.WriteLine(command)
    End Sub
    Private Sub p_ErrorDataReceived(sender As Object, e As DataReceivedEventArgs) Handles p.ErrorDataReceived
        Write(e.Data)
    End Sub

    Private Sub p_Exited(sender As Object, e As EventArgs) Handles p.Exited
        IsRunning = False
        Using ThriftAPI
            ThriftAPI.Stop()
            ThriftAPIIsAvailable = False
        End Using
        p.Dispose()
        p = Nothing
    End Sub

    Private Sub CheckForUpdates()
        _updater.CheckForUpdates()
        AddHandler _Updater.UpdatesFound, Sub(sender, e)
                                              RaiseEvent LauncherUpdatesFound(Me, EventArgs.Empty)
                                          End Sub
    End Sub

    Private Sub p_OutputDataReceived(sender As Object, e As DataReceivedEventArgs) Handles p.OutputDataReceived
        Write(e.Data)
    End Sub

    Private Sub Write(Line As String)
        If Line Is Nothing Then Return
        Select Case True
            Case Regex.IsMatch(Line, "^\[[0-9]{2}:[0-9]{2}:[0-9]{2} WARN\]: \*\*\*\* FAILED TO BIND TO PORT!$")
                Me.IsRunning = False
            Case Regex.IsMatch(Line, "^\[[0-9]{2}:[0-9]{2}:[0-9]{2} INFO\]: \[SwiftApi\] Started up and listening on port [1-9]{1,5}$")
                ThriftAPI.Start()
                ThriftAPIIsAvailable = True
            Case Regex.IsMatch(Line, "^\[[0-9]{2}:[0-9]{2}:[0-9]{2} INFO\]: \[dynmap\] Enabled$")
                RaiseEvent DynmapEnabled(Me, EventArgs.Empty)
            Case Regex.IsMatch(Line, "^\[[0-9]{2}:[0-9]{2}:[0-9]{2} INFO\]: Done \(.*\)! For help, type ""help"" or ""\?""$")
                If Not ServerSettings.HasChanged Then
                    ServerSettings.Load()
                End If
            Case Regex.IsMatch(Line, "^\[[0-9]{2}:[0-9]{2}:[0-9]{2} INFO\]: CONSOLE: Reload complete\.$")
                SwiftAPI.Load()
                ThriftAPI.UpdateAuthStrings(SwiftAPI.Username, SwiftAPI.Password, SwiftAPI.Salt)
                ThriftAPI.Stop()
        End Select
        RaiseEvent StateChanged(Me, New StateChangedEventArgs(Line))
    End Sub
End Class