Imports System.IO
Imports Newtonsoft.Json
Imports System.Collections.ObjectModel

Public Class BanlistInfo
    Inherits PropertyChangedBase

    Private _FilePathPlayer As FileInfo
    Public ReadOnly Property FilePathPlayer As FileInfo
        Get
            Return _FilePathPlayer
        End Get
    End Property

    Private _FilePathIPs As FileInfo
    Public ReadOnly Property FilePathIPs As FileInfo
        Get
            Return _FilePathIPs
        End Get
    End Property

    Private _BannedIPs As ObservableCollection(Of BannedPlayer)
    Public Property BannedIPs() As ObservableCollection(Of BannedPlayer)
        Get
            Return _BannedIPs
        End Get
        Protected Set(ByVal value As ObservableCollection(Of BannedPlayer))
            SetProperty(value, _BannedIPs)
        End Set
    End Property

    Private _BannedPlayers As ObservableCollection(Of BannedPlayer)
    Public Property BannedPlayers() As ObservableCollection(Of BannedPlayer)
        Get
            Return _BannedPlayers
        End Get
        Protected Set(ByVal value As ObservableCollection(Of BannedPlayer))
            SetProperty(value, _BannedPlayers)
        End Set
    End Property

    Public ReadOnly Property BannedPlayerFound As Boolean
        Get
            Return Me.AllBans.Count > 0
        End Get
    End Property

    Public ReadOnly Property BannedPlayerNotFound As Boolean
        Get
            Return Not Me.AllBans.Count > 0
        End Get
    End Property

    Public ReadOnly Property AllBans As ObservableCollection(Of BannedPlayer)
        Get
            Dim newlist As New ObservableCollection(Of BannedPlayer)
            If BannedIPs IsNot Nothing AndAlso BannedIPs.Count > 0 Then
                For Each i In BannedIPs
                    newlist.Add(i)
                Next
            End If
            If BannedPlayers IsNot Nothing AndAlso BannedPlayers.Count > 0 Then
                For Each i In BannedPlayers
                    newlist.Add(i)
                Next
            End If
            Return newlist
        End Get
    End Property

    Public Sub New()
        _FilePathIPs = New FileInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "banned-ips.json"))
        _FilePathPlayer = New FileInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "banned-players.json"))
        BannedIPs = New ObservableCollection(Of BannedPlayer)
        BannedPlayers = New ObservableCollection(Of BannedPlayer)
    End Sub

    Public Sub Load()
        Dim resultIPs As New ObservableCollection(Of BannedPlayer)
        If FilePathIPs.Exists Then
            resultIPs = JsonConvert.DeserializeObject(Of ObservableCollection(Of BannedPlayer))(File.ReadAllText(FilePathIPs.FullName))
            If resultIPs.Count <> BannedIPs.Count Then
                BannedIPs = resultIPs
            End If
        Else
            If BannedIPs.Count > 0 Then BannedIPs.Clear()
        End If
        Dim resultPlayer As New ObservableCollection(Of BannedPlayer)
        If FilePathPlayer.Exists Then
            resultPlayer = JsonConvert.DeserializeObject(Of ObservableCollection(Of BannedPlayer))(File.ReadAllText(FilePathPlayer.FullName))
            If resultPlayer.Count <> BannedPlayers.Count Then
                BannedPlayers = resultPlayer
            End If
        Else
            If BannedPlayers.Count > 0 Then BannedPlayers.Clear()
        End If
        OnPropertyChanged("AllBans")
        OnPropertyChanged("BannedPlayerFound")
        OnPropertyChanged("BannedPlayerNotFound")
    End Sub
End Class