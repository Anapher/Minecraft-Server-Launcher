Imports System.IO
Imports Newtonsoft.Json
Imports System.Collections.ObjectModel

Public Class WhitelistInfo
    Inherits PropertyChangedBase

    Private _FilePath As FileInfo
    Public ReadOnly Property FilePath As FileInfo
        Get
            Return _FilePath
        End Get
    End Property

    Private _Whitelist As ObservableCollection(Of WhitelistedPlayer)
    Public Property Whitelist() As ObservableCollection(Of WhitelistedPlayer)
        Get
            Return _Whitelist
        End Get
        Protected Set(ByVal value As ObservableCollection(Of WhitelistedPlayer))
            SetProperty(value, _Whitelist)
        End Set
    End Property

    Public Sub New()
        _FilePath = New FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "whitelist.json"))
        Whitelist = New ObservableCollection(Of WhitelistedPlayer)
    End Sub

    Public ReadOnly Property WhitelistedPlayerNotFound As Boolean
        Get
            Return Whitelist.Count = 0
        End Get
    End Property

    Public Sub Load()
        Dim result As New ObservableCollection(Of WhitelistedPlayer)
        If _FilePath.Exists Then
            result = JsonConvert.DeserializeObject(Of ObservableCollection(Of WhitelistedPlayer))(File.ReadAllText(FilePath.FullName))
            If result.Count <> Whitelist.Count Then
                Whitelist = result
            End If
        Else
            If Whitelist.Count > 0 Then Whitelist.Clear()
        End If
        OnPropertyChanged("WhitelistedPlayerNotFound")
    End Sub
End Class
