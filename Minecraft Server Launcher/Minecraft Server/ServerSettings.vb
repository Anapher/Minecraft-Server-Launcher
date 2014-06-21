Imports System.IO

Public Class ServerSettings
    Inherits PropertyChangedBase

#Region "Server Properties"
    Private _AllowNether As Boolean
    Public Property AllowNether() As Boolean
        Get
            Return _AllowNether
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _AllowNether)
        End Set
    End Property

    Private _enablequery As Boolean
    Public Property EnableQuery() As Boolean
        Get
            Return _enablequery
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _enablequery)
        End Set
    End Property

    Private _generatorsettings As String
    Public Property GeneratorSettings() As String
        Get
            Return _generatorsettings
        End Get
        Set(ByVal value As String)
            SetProperty(value, _generatorsettings)
        End Set
    End Property

    Private _OpPermissions As Integer
    Public Property OpPermission() As Integer
        Get
            Return _OpPermissions
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _OpPermissions)
        End Set
    End Property

    Private _levelName As String
    Public Property LevelName() As String
        Get
            Return _levelName
        End Get
        Set(ByVal value As String)
            SetProperty(value, _levelName)
        End Set
    End Property

    Private _AllowFlight As Boolean
    Public Property AllowFlight() As Boolean
        Get
            Return _AllowFlight
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _AllowFlight)
        End Set
    End Property

    Private _AnnouncePlayerAchievements As Boolean
    Public Property AnnouncePlayerAchievements() As Boolean
        Get
            Return _AnnouncePlayerAchievements
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _AnnouncePlayerAchievements)
        End Set
    End Property

    Private _ServerPort As Integer
    Public Property ServerPort() As Integer
        Get
            Return _ServerPort
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _ServerPort)
        End Set
    End Property

    Private _levelType As Integer
    Public Property LevelType() As Integer
        Get
            Return _levelType
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _levelType)
        End Set
    End Property

    Private _EnableRcon As Boolean
    Public Property EnableRcon() As Boolean
        Get
            Return _EnableRcon
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _EnableRcon)
        End Set
    End Property

    Private _ForceGamemode As Boolean
    Public Property ForceGamemode() As Boolean
        Get
            Return _ForceGamemode
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ForceGamemode)
        End Set
    End Property

    Private _levelseed As String
    Public Property LevelSeed() As String
        Get
            Return _levelseed
        End Get
        Set(ByVal value As String)
            SetProperty(value, _levelseed)
        End Set
    End Property

    Private _serverIP As String
    Public Property ServerIP() As String
        Get
            Return _serverIP
        End Get
        Set(ByVal value As String)
            SetProperty(value, _serverIP)
        End Set
    End Property

    Private _MaxBuildHeight As Integer
    Public Property MaxBuildHeight() As Integer
        Get
            Return _MaxBuildHeight
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _MaxBuildHeight)
        End Set
    End Property

    Private _SpawnNPCs As Boolean
    Public Property SpawnNPCs() As Boolean
        Get
            Return _SpawnNPCs
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _SpawnNPCs)
        End Set
    End Property

    Private _Whitelist As Boolean
    Public Property Whitelist() As Boolean
        Get
            Return _Whitelist
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _Whitelist)
        End Set
    End Property

    Private _SpawnAnimals As Boolean
    Public Property SpawnAnimals() As Boolean
        Get
            Return _SpawnAnimals
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _SpawnAnimals)
        End Set
    End Property

    Private _SnooperEnabled As Boolean
    Public Property SnooperEnabled() As Boolean
        Get
            Return _SnooperEnabled
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _SnooperEnabled)
        End Set
    End Property

    Private _Hardcore As Boolean
    Public Property Hardcore() As Boolean
        Get
            Return _Hardcore
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _Hardcore)
        End Set
    End Property

    Private _OnlineMode As Boolean
    Public Property OnlineMode() As Boolean
        Get
            Return _OnlineMode
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _OnlineMode)
        End Set
    End Property

    Private _ResourcePack As String
    Public Property ResourcePack() As String
        Get
            Return _ResourcePack
        End Get
        Set(ByVal value As String)
            SetProperty(value, _ResourcePack)
        End Set
    End Property

    Private _pvp As Boolean
    Public Property PVP() As Boolean
        Get
            Return _pvp
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _pvp)
        End Set
    End Property

    Private _Difficulty As Integer
    Public Property Difficulty() As Integer
        Get
            Return _Difficulty
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _Difficulty)
        End Set
    End Property

    Private _EnableCommandBlock As Boolean
    Public Property EnableCommandBlock() As Boolean
        Get
            Return _EnableCommandBlock
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _EnableCommandBlock)
        End Set
    End Property

    Private _servername As String
    Public Property ServerName() As String
        Get
            Return _servername
        End Get
        Set(ByVal value As String)
            SetProperty(value, _servername)
        End Set
    End Property

    Private _PlayerIdleTimeout As Integer
    Public Property PlayerIdleTimeout() As Integer
        Get
            Return _PlayerIdleTimeout
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _PlayerIdleTimeout)
        End Set
    End Property

    Private _Gamemode As Integer
    Public Property GameMode() As Integer
        Get
            Return _Gamemode
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _Gamemode)
        End Set
    End Property

    Private _MaxPlayers As Integer
    Public Property MaxPlayers() As Integer
        Get
            Return _MaxPlayers
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _MaxPlayers)
        End Set
    End Property

    Private _SpawnMonsters As Boolean
    Public Property SpawnMonsters() As Boolean
        Get
            Return _SpawnMonsters
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _SpawnMonsters)
        End Set
    End Property

    Private _ViewDistance As Integer
    Public Property ViewDistance() As Integer
        Get
            Return _ViewDistance
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _ViewDistance)
        End Set
    End Property

    Private _GenerateStructures As Boolean
    Public Property GenerateStructures() As Boolean
        Get
            Return _GenerateStructures
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _GenerateStructures)
        End Set
    End Property

    Private _SpawnProtection As Integer
    Public Property SpawnProtection() As Integer
        Get
            Return _SpawnProtection
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _SpawnProtection)
        End Set
    End Property

    Private _MODT As String
    Public Property MODT() As String
        Get
            Return _MODT
        End Get
        Set(ByVal value As String)
            SetProperty(value, _MODT)
        End Set
    End Property
#End Region

#Region "MyProperties"
    Private _Path As String
    Public ReadOnly Property Path As String
        Get
            Return _Path
        End Get
    End Property

    Private _HasChanged As Boolean
    Public Property HasChanged() As Boolean
        Get
            Return _HasChanged
        End Get
        Set(ByVal value As Boolean)
            _HasChanged = value
        End Set
    End Property

    Private UpdateMode As Boolean = False

    Protected Overrides Function SetProperty(Of T)(value As T, ByRef field As T, <Runtime.CompilerServices.CallerMemberName> Optional propertyName As String = Nothing) As Boolean
        If (field Is Nothing OrElse Not field.Equals(value)) AndAlso Not UpdateMode Then HasChanged = True
        Return MyBase.SetProperty(value, field, propertyName)
    End Function         
#End Region

#Region "Methods"
    Public Sub Load()
        Dim fi = New FileInfo(Path)
        If Not fi.Exists Then Return
        UpdateMode = True 'When a property change it will set HasChanged to true. But when Updatemode is true it won't
        Using fs As New FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Using sr As New StreamReader(fs)
                While Not sr.EndOfStream
                    Dim sSplit() = sr.ReadLine().Split("="c)
                    If sSplit.Count <= 1 Then Continue While
                    Select Case sSplit(0)
                        Case "generator-settings"
                            Me.GeneratorSettings = sSplit(1)
                        Case "op-permission-level"
                            Me.OpPermission = Integer.Parse(sSplit(1))
                        Case "allow-nether"
                            Me.AllowNether = StringToBool(sSplit(1))
                        Case "level-name"
                            Me.LevelName = sSplit(1)
                        Case "enable-query"
                            Me.EnableQuery = StringToBool(sSplit(1))
                        Case "allow-flight"
                            Me.AllowFlight = StringToBool(sSplit(1))
                        Case "announce-player-achievements"
                            Me.AnnouncePlayerAchievements = StringToBool(sSplit(1))
                        Case "server-port"
                            Me.ServerPort = Integer.Parse(sSplit(1))
                        Case "level-type"
                            Me.LevelType = DirectCast([Enum].Parse(GetType(LevelType), sSplit(1)), LevelType)
                        Case "enable-rcon"
                            Me.EnableRcon = StringToBool(sSplit(1))
                        Case "force-gamemode"
                            Me.ForceGamemode = StringToBool(sSplit(1))
                        Case "level-seed"
                            Me.LevelSeed = sSplit(1)
                        Case "server-ip"
                            Me.ServerIP = sSplit(1)
                        Case "max-build-height"
                            Me.MaxBuildHeight = Integer.Parse(sSplit(1))
                        Case "spawn-npcs"
                            Me.SpawnNPCs = StringToBool(sSplit(1))
                        Case "white-list"
                            Me.Whitelist = StringToBool(sSplit(1))
                        Case "spawn-animals"
                            Me.SpawnAnimals = StringToBool(sSplit(1))
                        Case "snooper-enabled"
                            Me.SnooperEnabled = StringToBool(sSplit(1))
                        Case "hardcore"
                            Me.Hardcore = StringToBool(sSplit(1))
                        Case "online-mode"
                            Me.OnlineMode = StringToBool(sSplit(1))
                        Case "resource-pack"
                            Me.ResourcePack = sSplit(1)
                        Case "pvp"
                            Me.PVP = StringToBool(sSplit(1))
                        Case "difficulty"
                            Me.Difficulty = Integer.Parse(sSplit(1))
                        Case "enable-command-block"
                            Me.EnableCommandBlock = StringToBool(sSplit(1))
                        Case "server-name"
                            Me.ServerName = sSplit(1)
                        Case "player-idle-timeout"
                            Me.PlayerIdleTimeout = Integer.Parse(sSplit(1))
                        Case "gamemode"
                            Me.GameMode = Integer.Parse(sSplit(1))
                        Case "max-players"
                            Me.MaxPlayers = Integer.Parse(sSplit(1))
                        Case "spawn-monsters"
                            Me.SpawnMonsters = StringToBool(sSplit(1))
                        Case "view-distance"
                            Me.ViewDistance = Integer.Parse(sSplit(1))
                        Case "generate-structures"
                            Me.GenerateStructures = StringToBool(sSplit(1))
                        Case "spawn-protection"
                            Me.SpawnProtection = Integer.Parse(sSplit(1))
                        Case "motd"
                            Me.MODT = sSplit(1)
                    End Select
                End While
            End Using
        End Using
        UpdateMode = False
    End Sub

    Public Sub Save()
        Dim fi = New FileInfo(Path)
        If Not fi.Exists Then
            Return
        End If
        Dim txt = File.ReadAllLines(fi.FullName)
        For Each s In txt
            Dim sSplit() = s.Split("="c)
            If sSplit.Count <= 1 Then Continue For
            Select Case sSplit(0)
                Case "generator-settings"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.GeneratorSettings)
                Case "op-permission-level"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.OpPermission)
                Case "allow-nether"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.AllowNether))
                Case "level-name"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.LevelName)
                Case "enable-query"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.EnableQuery))
                Case "allow-flight"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.AllowFlight))
                Case "announce-player-achievements"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.AnnouncePlayerAchievements))
                Case "server-port"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.ServerPort)
                Case "level-type"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), [Enum].ToObject(GetType(LevelType), Me.LevelType)).ToString()
                Case "enable-rcon"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.EnableRcon))
                Case "force-gamemode"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.ForceGamemode)
                Case "level-seed"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.LevelSeed)
                Case "server-ip"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.ServerIP)
                Case "max-build-height"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.MaxBuildHeight)
                Case "spawn-npcs"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.SpawnNPCs))
                Case "white-list"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.Whitelist))
                Case "spawn-animals"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.SpawnAnimals))
                Case "snooper-enabled"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.SnooperEnabled))
                Case "online-mode"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.OnlineMode))
                Case "resource-pack"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.ResourcePack)
                Case "pvp"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.PVP))
                Case "difficulty"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.Difficulty)
                Case "enable-command-block"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.EnableCommandBlock))
                Case "server-name"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.ServerName)
                Case "player-idle-timeout"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.PlayerIdleTimeout)
                Case "gamemode"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.GameMode)
                Case "max-players"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.MaxPlayers)
                Case "spawn-monsters"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.SpawnMonsters))
                Case "view-distance"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.ViewDistance)
                Case "generate-structures"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), BoolToString(Me.GenerateStructures))
                Case "spawn-protection"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.SpawnProtection)
                Case "motd"
                    txt(Array.IndexOf(txt, s)) = String.Format("{0}={1}", sSplit(0), Me.MODT)
            End Select
        Next
        File.WriteAllLines(Me.Path, txt)
    End Sub

    Private Function BoolToString(b As Boolean) As String
        Return b.ToString().ToLower()
    End Function

    Private Function StringToBool(s As String) As Boolean
        If s.ToLower() = "true" Then Return True Else Return False
    End Function
#End Region
    Public Sub New(Path As String)
        _Path = Path
    End Sub
End Class

Public Enum LevelType
    [DEFAULT] = 0
    FLAT = 1
    LARGEBIOMES = 3
    AMPLIFIED = 4
    CUSTOMIZED = 5
End Enum