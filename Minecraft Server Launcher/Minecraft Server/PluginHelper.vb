Imports System.Text.RegularExpressions

Public Class PluginHelper
    Public Shared Function GetLink(lnk As String) As String
        Dim wc As New Net.WebClient
        wc.Proxy = Nothing
        Dim s As String = wc.DownloadString(lnk).Replace(vbLf, "").Replace(vbCr, "").Replace(" ", "")
        Dim regex As New Regex("<liclass=""user-actionuser-action-download""><ahref=""(?<lnk>(.*?))"">Download</a>")
        Dim matches As MatchCollection = regex.Matches(s)

        s = wc.DownloadString("http://dev.bukkit.org" & matches(0).Groups("lnk").Value).Replace(vbLf, "").Replace(vbCr, "").Replace(" ", "")
        regex = New Regex("<liclass=""user-actionuser-action-download""><span><ahref=""(?<lnk>(.*?))"">Download</a></span></li>")
        Return regex.Matches(s)(0).Groups("lnk").Value
    End Function
End Class
