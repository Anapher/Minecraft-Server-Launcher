Imports System.Net
Imports System.Text.RegularExpressions
Imports System.IO

Public Class CauldronVersionSelector
    Inherits VersionSelector

    Private _Versions As List(Of CauldronVersion)
    Public Property Versions() As List(Of CauldronVersion)
        Get
            Return _Versions
        End Get
        Set(ByVal value As List(Of CauldronVersion))
            SetProperty(value, _Versions)
        End Set
    End Property

    Public Overrides ReadOnly Property CurrentVersionDownloadLink As String
        Get
            Return Me.Versions(lstIndex).DownloadLink
        End Get
    End Property

    Public Overrides ReadOnly Property DownloadButtonIsEnabled As Boolean
        Get
            Return Me.GotInformations AndAlso Me.lstIndex > -1
        End Get
    End Property

    Private _lstIndex As Integer = -1
    Public Property lstIndex() As Integer
        Get
            Return _lstIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _lstIndex)
            MyBase.OnDownloadButtonEnabledChanged()
        End Set
    End Property

#Region "Load"
    Private wc As WebClient

    Public Overrides Sub LoadVersions()
        wc = New WebClient()
        wc.Proxy = Nothing
        AddHandler wc.DownloadStringCompleted, AddressOf wc_DownloadStringCompleted
        wc.DownloadStringAsync(New Uri("http://files.minecraftforge.net/Cauldron/"))
    End Sub

    Private Sub wc_DownloadStringCompleted(sender As Object, e As DownloadStringCompletedEventArgs)
        Dim regex As New Regex("<tr><td>(?<Version>(.*?))<\/td><td>(?<MinecraftVersion>(.*?))<\/td><td>(?<Time>(.*?))<\/td><td>.*?\(<ahref=""http:\/\/adf.ly/[0-9]{1,10}\/(?<Downloadlink>(.*?))"">Installer<\/a>\)")
        Dim text As String = e.Result.Replace(vbLf, "").Replace(vbCr, "").Replace(" ", "").Split({"<tableborder=""0"">"}, StringSplitOptions.None)(1)
        Versions = New List(Of CauldronVersion)
        For Each m As Match In regex.Matches(text)
            Versions.Add(New CauldronVersion(m.Groups("Version").Value, m.Groups("MinecraftVersion").Value, DateTime.ParseExact(m.Groups("Time").Value, "MM/dd/yyyyhh:mm:sstt", Globalization.CultureInfo.InvariantCulture), m.Groups("Downloadlink").Value))
        Next
        MyBase.OnGotVersions()
    End Sub

    Public Sub Install(File As FileInfo)
        If Not File.Exists Then Return
        Dim p As New Process()
        With p
            .StartInfo.FileName = File.FullName
            .StartInfo.Arguments = "--installServer"
            .Start()
            .WaitForExit()
        End With
        File.Delete()
        Dim fiCauldron As New FileInfo(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, String.Format("cauldron-{0}-{1}-server.jar", Me.Versions(lstIndex).MinecraftVersion, Me.Versions(lstIndex).Version)))
        If fiCauldron.Exists Then
            fiCauldron.CopyTo(Path.Combine(Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, Paths.GetPaths.MinecraftServerFileName)))
        End If
    End Sub
#End Region
End Class
