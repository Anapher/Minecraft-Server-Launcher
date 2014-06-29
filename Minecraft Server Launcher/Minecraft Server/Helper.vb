Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports System.Collections.ObjectModel
Imports System.Runtime.InteropServices

Public Class Helper
    Public Shared Function GetIP(ServerPropertiesPath As String) As String
        If Not File.Exists(ServerPropertiesPath) Then Return "localhost"
        Dim regex As New Regex("server-ip=(?<ip>(.*?))" & vbLf)
        Dim matches = regex.Matches(File.ReadAllText(ServerPropertiesPath))
        If matches.Count = 0 Then Return "localhost"
        Dim ip = matches(0).Groups("ip").Value
        If String.IsNullOrWhiteSpace(ip) Then Return "localhost" Else Return ip
    End Function

    Public Shared Function GetjavaPath() As String
        '64 Bit

        Using hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            Using key = hklm.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment", False)
                If Not key Is Nothing Then
                    Dim subkeys = key.GetSubKeyNames
                    If subkeys IsNot Nothing AndAlso subkeys.Count > 0 Then
                        Dim newkey = hklm.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment\" & subkeys(0), False)
                        Dim fi As New FileInfo(Path.Combine(newkey.GetValue("JavaHome").ToString(), "bin", "java.exe"))
                        If fi.Exists Then Return fi.FullName
                    End If
                End If
            End Using
        End Using


        '32 Bit
        Using regkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment", False)
            If Not regkey Is Nothing Then
                Dim subkeys = regkey.GetSubKeyNames
                If Not subkeys.Count = 0 Then
                    Dim newkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment\" & subkeys(0), False)
                    Dim fi As New FileInfo(Path.Combine(newkey.GetValue("JavaHome").ToString(), "bin", "java.exe"))
                    If fi.Exists Then Return fi.FullName
                End If
            End If
        End Using
        Return Nothing
    End Function

    Public Shared Function RoundSize(Bytes As Long) As String
        Select Case Bytes
            Case Is < 1024
                Return Bytes.ToString() & " B"
            Case Is < 1048576
                Return Math.Round(Bytes / 1024, 1).ToString() & " KB"
            Case Else
                Return Math.Round(Bytes / 1024 / 1024, 1).ToString() & " MB"
        End Select
    End Function

    <DllImport("wininet.dll")> _
    Private Shared Function InternetGetConnectedState(ByRef connDescription As Integer, ReservedValue As Integer) As Boolean
    End Function
    ''' <summary>
    ''' Checks if an internet connection is available.
    ''' </summary>
    ''' <returns>This function returns a boolean. true = is available</returns>
    Public Shared Function ConnectionAvailable() As Boolean
        Dim Desc As Integer
        Return InternetGetConnectedState(Desc, 0)
    End Function
End Class
