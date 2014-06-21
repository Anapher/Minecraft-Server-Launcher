Imports System.Net
Imports System.Text.RegularExpressions

Public Class BukkitVersionSelector
    Inherits VersionSelector

#Region "Versions"
    Private _Versions As List(Of BukkitVersion)
    Public Property Versions() As List(Of BukkitVersion)
        Get
            Return _Versions
        End Get
        Set(ByVal value As List(Of BukkitVersion))
            SetProperty(value, _Versions)
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
#End Region

#Region "Check"
    Private _ChooseSelectedVersion As Boolean
    Public Property ChooseSelectedVersion() As Boolean
        Get
            Return _ChooseSelectedVersion
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ChooseSelectedVersion)
            MyBase.OnDownloadButtonEnabledChanged()
        End Set
    End Property

    Private _ChooseDevelopmentVersion As Boolean
    Public Property ChooseDevelopmentVersion() As Boolean
        Get
            Return _ChooseDevelopmentVersion
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ChooseDevelopmentVersion)
        End Set
    End Property

    Private _ChooseBetaVersion As Boolean
    Public Property ChooseBetaVersion() As Boolean
        Get
            Return _ChooseBetaVersion
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ChooseBetaVersion)
        End Set
    End Property

    Private _ChooseRecommendedVersion As Boolean
    Public Property ChooseRecommendedVersion() As Boolean
        Get
            Return _ChooseRecommendedVersion
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ChooseRecommendedVersion)
        End Set
    End Property

    Private _lstIndex As Integer = -1
    Public Property lstIndex() As Integer
        Get
            Return _lstIndex
        End Get
        Set(ByVal value As Integer)
            If value <> _lstIndex Then
                ChooseSelectedVersion = True
                ChooseDevelopmentVersion = False
                ChooseRecommendedVersion = False
                ChooseBetaVersion = False
            End If
            SetProperty(value, _lstIndex)
            MyBase.OnDownloadButtonEnabledChanged()
        End Set
    End Property
#End Region

#Region "Load"
    Private wc As WebClient

    Public Overrides Sub LoadVersions()
        wc = New WebClient()
        wc.Proxy = Nothing
        AddHandler wc.DownloadStringCompleted, AddressOf wc_DownloadStringCompleted
        wc.DownloadStringAsync(New Uri("http://dl.bukkit.org/downloads/craftbukkit/"))
    End Sub

    Private Sub wc_DownloadStringCompleted(sender As Object, e As DownloadStringCompletedEventArgs)
        Dim regex As New Regex("<trclass=""chan-dev"">(?<txt>(.*?))</tr>")
        Dim text As String = e.Result.Replace(vbLf, "").Replace(vbCr, "").Replace(" ", "")
        Versions = New List(Of BukkitVersion)
        For Each m As Match In regex.Matches(text)
            Dim txt As String = m.Groups("txt").Value

            Dim reg As New Regex("<th><ahref=""(?<url>().*?)"">(?<buildnr>(.*?))</a></th><td>(?<version>(.*?))</td><td><ahref=""(.*?)"">(?<type>(.*?))</a></td><tdclass=""downloadLink""><aclass=""tooltipd""title=""(.*?)""href=""(?<dl>(.*?))"">")
            Dim matches As MatchCollection = reg.Matches(txt)
            Dim item As BukkitVersion = New BukkitVersion(matches(0).Groups("buildnr").Value, matches(0).Groups("dl").Value, matches(0).Groups("type").Value, matches(0).Groups("version").Value, matches(0).Groups("url").Value)
            Versions.Add(item)
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
        MyBase.OnGotVersions()
    End Sub
#End Region

    Public Overrides ReadOnly Property DownloadButtonIsEnabled As Boolean
        Get
            Return (Me.GotInformations AndAlso Not ChooseSelectedVersion) OrElse (Me.GotInformations AndAlso lstIndex > -1)
        End Get
    End Property

    Public Overrides ReadOnly Property CurrentVersionDownloadLink As String
        Get
            Dim lnk = String.Empty
            Select Case True
                Case ChooseBetaVersion
                    lnk = BetaBuild.DownloadLink
                Case ChooseDevelopmentVersion
                    lnk = DevelopmentBuild.DownloadLink
                Case ChooseRecommendedVersion
                    lnk = RecommendedBuild.DownloadLink
                Case Else
                    lnk = Versions(lstIndex).DownloadLink
            End Select
            Return String.Format("http://dl.bukkit.org/{0}", lnk)
        End Get
    End Property

    Public Sub New()
        Me.ChooseRecommendedVersion = True
    End Sub
End Class
