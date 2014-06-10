Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports System.Collections.ObjectModel

Public Class Helper
    Public Shared Sub GetUsernamePasswordSalt(path As FileInfo, ByRef username As String, ByRef password As String, ByRef salt As String, ByRef port As Integer)
        If path.Exists Then
            Dim regex As New Regex("username: (?<user>(.*?))" & vbLf & "password: (?<passw>(.*?))" & vbLf & "salt: (?<salt>(.*?))" & vbLf & vbLf & "# The port to listen on \(default 21111\)" & vbLf & "port: (?<port>([0-9]{1,5}))" & vbLf)
            Dim matches = regex.Matches(File.ReadAllText(path.FullName))
            If matches.Count = 0 Then Return
            username = matches(0).Groups("user").Value
            password = matches(0).Groups("passw").Value
            salt = matches(0).Groups("salt").Value
            port = Integer.Parse(matches(0).Groups("port").Value)
        Else
            username = "admin"
            password = "password"
            salt = "saltines"
            port = 21111
        End If
    End Sub

    Public Shared Function GetIP(ServerPropertiesPath As String) As String
        If Not File.Exists(ServerPropertiesPath) Then Return "localhost"
        Dim regex As New Regex("server-ip=(?<ip>(.*?))" & vbLf)
        Dim matches = regex.Matches(File.ReadAllText(ServerPropertiesPath))
        If matches.Count = 0 Then Return "localhost"
        Dim ip = matches(0).Groups("ip").Value
        If String.IsNullOrWhiteSpace(ip) Then Return "localhost" Else Return ip
    End Function

    Public Shared Function GetjavaPath() As String
        '32 Bit
        Using regkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment", False)
            If Not regkey Is Nothing Then
                Dim subkeys = regkey.GetSubKeyNames
                If Not subkeys.Count = 0 Then
                    Dim newkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment\" & subkeys(0), False)
                    Return newkey.GetValue("JavaHome").ToString()
                End If
            End If
        End Using

        '64 Bit

        Using hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            Using key = hklm.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment", False)
                If Not key Is Nothing Then
                    Dim subkeys = key.GetSubKeyNames
                    If subkeys IsNot Nothing AndAlso subkeys.Count > 0 Then
                        Dim newkey = hklm.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment\" & subkeys(0), False)
                        Return newkey.GetValue("JavaHome").ToString()
                    End If
                End If
            End Using
        End Using
        Return Nothing
    End Function

    Public Shared Function SwiftAPIExists(FolderPath As DirectoryInfo) As Boolean
        If Not FolderPath.Exists Then Return False
        Using mymd5 = System.Security.Cryptography.MD5.Create
            For Each fi In FolderPath.GetFiles("*.jar", SearchOption.TopDirectoryOnly)
                Using Stream = fi.OpenRead()
                    Dim hash = BitConverter.ToString(mymd5.ComputeHash(Stream)).Replace("-", "").ToUpper()
                    If hash = "26652A12A89346573974CFFE08881EAF" Then
                        Return True
                    End If
                End Using
            Next
        End Using
        Return False
    End Function
End Class
