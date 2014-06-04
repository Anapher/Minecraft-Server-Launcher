Imports System.IO
Imports System.Collections.ObjectModel

<Serializable> _
Public Class CommandList
    Private _PluginName As String
    Public Property PluginName() As String
        Get
            Return _PluginName
        End Get
        Set(ByVal value As String)
            _PluginName = value
        End Set
    End Property

    Private _Commands As ObservableCollection(Of IntelliTextBoxCommand)
    Public Property Commands() As ObservableCollection(Of IntelliTextBoxCommand)
        Get
            Return _Commands
        End Get
        Set(ByVal value As ObservableCollection(Of IntelliTextBoxCommand))
            _Commands = value
        End Set
    End Property

    Public Sub New()
        Commands = New ObservableCollection(Of IntelliTextBoxCommand)
    End Sub

    Public Shared Function Load(Path As String) As CommandList
        Using sr As New IO.StreamReader(Path)
            Dim xmls As New Xml.Serialization.XmlSerializer(GetType(CommandList))
            Dim obj = DirectCast(xmls.Deserialize(sr), CommandList)
            Return obj
        End Using
    End Function

    Public Sub Save(Path As String)
        Using sw As New IO.StreamWriter(Path, False)
            Dim xmls As New Xml.Serialization.XmlSerializer(GetType(CommandList))
            xmls.Serialize(sw, Me)
        End Using
    End Sub

    Private _Filename As FileInfo
    <Xml.Serialization.XmlIgnore> _
    Public Property Filename() As FileInfo
        Get
            Return _Filename
        End Get
        Set(ByVal value As FileInfo)
            _Filename = value
        End Set
    End Property

End Class
