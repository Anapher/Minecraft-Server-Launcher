Public Class Command
    Private _usage As String
    Public Property usage() As String
        Get
            Return _usage
        End Get
        Set(ByVal value As String)
            _usage = value
        End Set
    End Property

    Private _aliases As List(Of String)
    Public Property aliases() As List(Of String)
        Get
            Return _aliases
        End Get
        Set(ByVal value As List(Of String))
            _aliases = value
        End Set
    End Property

    Private _command As String
    Public Property command() As String
        Get
            Return _command
        End Get
        Set(ByVal value As String)
            _command = value
        End Set
    End Property

    Private _permissions As String
    Public Property permission() As String
        Get
            Return _permissions
        End Get
        Set(ByVal value As String)
            _permissions = value
        End Set
    End Property
End Class
