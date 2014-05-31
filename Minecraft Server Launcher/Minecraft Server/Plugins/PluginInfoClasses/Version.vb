Public Class Version
    Private _status As String
    Public Property status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _commands As List(Of Command)
    Public Property commands() As List(Of Command)
        Get
            Return _commands
        End Get
        Set(value As List(Of Command))
            _commands = Value
        End Set
    End Property

    Private _changelog As String
    Public Property changelog() As String
        Get
            Return _changelog
        End Get
        Set(value As String)
            _changelog = Value
        End Set
    End Property

    Public ReadOnly Property ChangelogString() As String
        Get
            Dim data As Byte() = Convert.FromBase64String(changelog)
            Return Text.Encoding.UTF8.GetString(data)
        End Get
    End Property

    Private _game_versions As List(Of String)
    Public Property game_versions() As List(Of String)
        Get
            Return _game_versions
        End Get
        Set(value As List(Of String))
            _game_versions = Value
        End Set
    End Property

    Private _dbo_version As String
    Public Property dbo_version() As String
        Get
            Return _dbo_version
        End Get
        Set(value As String)
            _dbo_version = Value
        End Set
    End Property

    Private _hard_dependencies As List(Of Object)
    Public Property hard_dependencies() As List(Of Object)
        Get
            Return _hard_dependencies
        End Get
        Set(value As List(Of Object))
            _hard_dependencies = Value
        End Set
    End Property

    Private _date As Integer
    Public Property [date]() As Integer
        Get
            Return _date
        End Get
        Set(value As Integer)
            _date = Value
        End Set
    End Property

    Private _version As String
    Public Property version() As String
        Get
            Return _version
        End Get
        Set(value As String)
            _version = Value
        End Set
    End Property

    Private _link As String
    Public Property link() As String
        Get
            Return _link
        End Get
        Set(value As String)
            _link = Value
        End Set
    End Property

    Private _file_id As Integer
    Public Property file_id() As Integer
        Get
            Return _file_id
        End Get
        Set(value As Integer)
            _file_id = Value
        End Set
    End Property

    Private _md5 As String
    Public Property md5() As String
        Get
            Return _md5
        End Get
        Set(value As String)
            _md5 = Value
        End Set
    End Property

    Private _download As String
    Public Property download() As String
        Get
            Return _download
        End Get
        Set(value As String)
            _download = Value
        End Set
    End Property

    Private _filename As String
    Public Property filename() As String
        Get
            Return _filename
        End Get
        Set(value As String)
            _filename = Value
        End Set
    End Property

    Private _type As String
    Public Property type() As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = Value
        End Set
    End Property

    Private _slug As String
    Public Property slug() As String
        Get
            Return _slug
        End Get
        Set(value As String)
            _slug = Value
        End Set
    End Property

    Private _soft_dependencies As List(Of Object)
    Public Property soft_dependencies() As List(Of Object)
        Get
            Return _soft_dependencies
        End Get
        Set(value As List(Of Object))
            _soft_dependencies = Value
        End Set
    End Property

    Private _permissions As List(Of Permission)
    Public Property permissions() As List(Of Permission)
        Get
            Return _permissions
        End Get
        Set(value As List(Of Permission))
            _permissions = Value
        End Set
    End Property


End Class
