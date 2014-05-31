Public Class BukkitVersion
    Private _buildnr As String, _downloadlink As String, _type As String, _version As String, _InfoUrl As String

    Public Property InfoURL As String
        Get
            Return _InfoUrl
        End Get
        Set(value As String)
            _InfoUrl = value
        End Set
    End Property

    Public Property Buildnr As String
        Get
            Return _buildnr
        End Get
        Set(value As String)
            _buildnr = value
        End Set
    End Property

    Public Property DownloadLink As String
        Get
            Return _downloadlink
        End Get
        Set(value As String)
            _downloadlink = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
        End Set
    End Property

    Public Property version As String
        Get
            Return _version
        End Get
        Set(value As String)
            _version = value
        End Set
    End Property

    Public Sub New(buildnr As String, dl As String, type As String, version As String, InfoURL As String)
        Me.Buildnr = buildnr
        Me.DownloadLink = dl
        Me.Type = type
        Me.version = version
        Me._InfoUrl = InfoURL
    End Sub
End Class
