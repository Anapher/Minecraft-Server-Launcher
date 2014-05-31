Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions

Public Class CraftbukkitDownloader
    Inherits PropertyChangedBase

    Private WithEvents client As WebClient

#Region "Events"
    Public Event GotVersions(sender As Object, e As EventArgs)
    Public Event DownloadCraftbukkitCompleted(sender As Object, e As EventArgs)
#End Region

#Region "Properties"
    Private _lstBukkitVersions As List(Of BukkitVersion)
    Public Property lstBukkitVersions() As List(Of BukkitVersion)
        Get
            Return _lstBukkitVersions
        End Get
        Set(ByVal value As List(Of BukkitVersion))
            SetProperty(value, _lstBukkitVersions)
        End Set
    End Property

    Private _DevelopmentBuild As BukkitVersion
    Public Property DevelopmentBuild() As BukkitVersion
        Get
            Return _DevelopmentBuild
        End Get
        Set(ByVal value As BukkitVersion)
            SetProperty(value, _DevelopmentBuild)
        End Set
    End Property

    Private _BetaBuild As BukkitVersion
    Public Property BetaBuild() As BukkitVersion
        Get
            Return _BetaBuild
        End Get
        Set(ByVal value As BukkitVersion)
            SetProperty(value, _BetaBuild)
        End Set
    End Property

    Private _RecommendedBuild As BukkitVersion
    Public Property RecommendedBuild() As BukkitVersion
        Get
            Return _RecommendedBuild
        End Get
        Set(ByVal value As BukkitVersion)
            SetProperty(value, _RecommendedBuild)
        End Set
    End Property

    Private _CurrentState As Double = 0
    Public Property CurrentState() As Double
        Get
            Return _CurrentState
        End Get
        Set(ByVal value As Double)
            SetProperty(value, _CurrentState)
        End Set
    End Property
#End Region

    Public Sub New()
        client = New Net.WebClient With {.Proxy = Nothing}
    End Sub

    Public Sub Download(Version As BukkitVersion)
        If Not client.IsBusy Then
            Dim FilePath = New FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "craftbukkit.jar"))
            client.DownloadFileAsync(New Uri("http://dl.bukkit.org/" & Version.DownloadLink), FilePath.FullName)
        End If
    End Sub

    Public Sub LoadVersions()
        client.DownloadStringAsync(New Uri("http://dl.bukkit.org/downloads/craftbukkit/"))
    End Sub

    Private Sub client_DownloadFileCompleted(sender As Object, e As ComponentModel.AsyncCompletedEventArgs) Handles client.DownloadFileCompleted
        RaiseEvent DownloadCraftbukkitCompleted(Me, EventArgs.Empty)
        CurrentState = 0
    End Sub

    Private Sub client_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) Handles client.DownloadProgressChanged
        CurrentState = e.ProgressPercentage
    End Sub

    Private Sub client_DownloadStringCompleted(sender As Object, e As DownloadStringCompletedEventArgs) Handles client.DownloadStringCompleted
        CurrentState = 0
        Dim regex As New Regex("<trclass=""chan-dev"">(?<txt>(.*?))</tr>")
        Dim text As String = e.Result.Replace(vbLf, "").Replace(vbCr, "").Replace(" ", "")
        lstBukkitVersions = New List(Of BukkitVersion)
        For Each m As Match In regex.Matches(text)
            Dim txt As String = m.Groups("txt").Value

            Dim reg As New Regex("<th><ahref=""(?<url>().*?)"">(?<buildnr>(.*?))</a></th><td>(?<version>(.*?))</td><td><ahref=""(.*?)"">(?<type>(.*?))</a></td><tdclass=""downloadLink""><aclass=""tooltipd""title=""(.*?)""href=""(?<dl>(.*?))"">")
            Dim matches As MatchCollection = reg.Matches(txt)
            Dim item As BukkitVersion = New BukkitVersion(matches(0).Groups("buildnr").Value, matches(0).Groups("dl").Value, matches(0).Groups("type").Value, matches(0).Groups("version").Value, matches(0).Groups("url").Value)
            lstBukkitVersions.Add(item)
        Next

        regex = New Regex("<divclass=""downloadButtonchan-rbmini""><ahref=""(?<dl>(.*?))""class=""tooltipd""title=(.*?)><small>(.*?)</small><span>(?<version>(.*?))</span><small>(.*?)</small></a><divclass=""downloadButtonDetailTab""><ahref=""(?<url>(.*?))""")
        Dim smatches As MatchCollection = regex.Matches(text)
        RecommendedBuild = New BukkitVersion("", smatches(0).Groups("dl").Value, "", smatches(0).Groups("version").Value, smatches(0).Groups("url").Value)

        regex = New Regex("<divclass=""downloadButtonchan-betamini""><ahref=""(?<dl>(.*?))""class=""tooltipd""title=(.*?)><small>(.*?)</small><span>(?<version>(.*?))</span><small>(.*?)</small></a><divclass=""downloadButtonDetailTab""><ahref=""(?<url>(.*?))""")
        smatches = regex.Matches(text)
        BetaBuild = New BukkitVersion("", smatches(0).Groups("dl").Value, "", smatches(0).Groups("version").Value, smatches(0).Groups("url").Value)

        regex = New Regex("<divclass=""downloadButtonchan-devmini""><ahref=""(?<dl>(.*?))""class=""tooltipd""title=(.*?)><small>(.*?)</small><span>(?<version>(.*?))</span><small>(.*?)</small></a><divclass=""downloadButtonDetailTab""><ahref=""(?<url>(.*?))""")
        smatches = regex.Matches(text)
        DevelopmentBuild = New BukkitVersion("", smatches(0).Groups("dl").Value, "", smatches(0).Groups("version").Value, smatches(0).Groups("url").Value)
        RaiseEvent GotVersions(Me, EventArgs.Empty)
        OnPropertyChanged("lstBukkitVersions")
    End Sub
End Class
