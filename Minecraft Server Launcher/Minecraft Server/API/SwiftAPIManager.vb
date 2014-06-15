Imports System.IO
Imports System.Text.RegularExpressions

Public Class SwiftAPIManager
    Inherits PropertyChangedBase

    Private _MinecraftFolder As DirectoryInfo
    Private Const SwiftAPIHash = "26652A12A89346573974CFFE08881EAF"

    Public Event GeneratedInformationsComplete(sender As Object, e As EventArgs)

    Public ReadOnly Property ConfigPath As FileInfo
        Get
            Return New FileInfo(Path.Combine(_MinecraftFolder.FullName, "plugins", "SwiftApi", "config.yml"))
        End Get
    End Property

    Private _Username As String
    Public Property Username As String
        Get
            Return _Username
        End Get
        Protected Set(value As String)
            SetProperty(value, _Username)
        End Set
    End Property

    Private _password As String
    Public Property Password() As String
        Get
            Return _password
        End Get
        Protected Set(ByVal value As String)
            SetProperty(value, _password)
        End Set
    End Property

    Private _Salt As String
    Public Property Salt() As String
        Get
            Return _Salt
        End Get
        Protected Set(ByVal value As String)
            SetProperty(value, _Salt)
        End Set
    End Property

    Private _port As Integer
    Public Property Port() As Integer
        Get
            Return _port
        End Get
        Protected Set(ByVal value As Integer)
            SetProperty(value, _port)
        End Set
    End Property

    Public Sub Load()
        Dim config = ConfigPath
        If config.Exists Then
            Dim regex As New Regex("username: (?<user>(.*?))" & vbLf & "password: (?<passw>(.*?))" & vbLf & "salt: (?<salt>(.*?))" & vbLf & vbLf & "# The port to listen on \(default 21111\)" & vbLf & "port: (?<port>([0-9]{1,5}))" & vbLf)
            Dim matches = regex.Matches(File.ReadAllText(config.FullName))
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

    Public Sub New(MinecraftFolder As DirectoryInfo)
        Me._MinecraftFolder = MinecraftFolder
        rnd = New Random()
    End Sub

    Public Shared Function Exists(PluginsFolderPath As DirectoryInfo) As Boolean
        If Not PluginsFolderPath.Exists Then Return False
        Using mymd5 = System.Security.Cryptography.MD5.Create
            For Each fi In PluginsFolderPath.GetFiles("*.jar", SearchOption.TopDirectoryOnly)
                Using Stream = fi.OpenRead()
                    Dim hash = BitConverter.ToString(mymd5.ComputeHash(Stream)).Replace("-", "").ToUpper()
                    If hash = SwiftAPIHash Then
                        Return True
                    End If
                End Using
            Next
        End Using
        Return False
    End Function

    Private _GenerateInformationsCommand As RelayCommand
    Public ReadOnly Property GenerateInformationsCommand As RelayCommand
        Get
            If _GenerateInformationsCommand Is Nothing Then _GenerateInformationsCommand = New RelayCommand(Sub(parameter As Object)
                                                                                                                GenerateRandomInformations()
                                                                                                            End Sub)
            Return _GenerateInformationsCommand
        End Get
    End Property

    Public Sub GenerateRandomInformations()
        If Not ConfigPath.Exists Then Return
        Dim newusername = GenerateString(10)
        Dim newpassword = GenerateString(16)
        Dim newsalt = GenerateString(20)

        Dim txt = File.ReadAllText(ConfigPath.FullName)
        txt = Regex.Replace(txt, "^username: .*$", String.Format("username: {0}", newusername), RegexOptions.Multiline)
        txt = Regex.Replace(txt, "^password: .*$", String.Format("password: {0}", newpassword), RegexOptions.Multiline)
        txt = Regex.Replace(txt, "^salt: .*$", String.Format("salt: {0}", newsalt), RegexOptions.Multiline)
        File.WriteAllText(ConfigPath.FullName, txt)
        RaiseEvent GeneratedInformationsComplete(Me, EventArgs.Empty)
    End Sub

    Private rnd As Random
    Private Function GenerateString(length As Integer) As String
        Dim valid As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
        Dim result As New Text.StringBuilder()
        While 0 < length
            result.Append(valid.ToCharArray()(rnd.Next(valid.Length)))
            length -= 1
        End While
        Return result.ToString()
    End Function
End Class
