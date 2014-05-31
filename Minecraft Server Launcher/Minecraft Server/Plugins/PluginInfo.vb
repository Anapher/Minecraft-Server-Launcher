Public Class PluginInfo
    Private _website As String
    Public Property website() As String
        Get
            Return _website
        End Get
        Set(ByVal value As String)
            _website = value
        End Set
    End Property

    Private _dbo_page As String
    Public Property dbo_page() As String
        Get
            Return _dbo_page
        End Get
        Set(ByVal value As String)
            _dbo_page = value
        End Set
    End Property

    Private _main As String
    Public Property main() As String
        Get
            Return _main
        End Get
        Set(ByVal value As String)
            _main = value
        End Set
    End Property

    Private _description As String
    Public Property description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Private _curse_id As String
    Public Property curse_id() As String
        Get
            Return _curse_id
        End Get
        Set(ByVal value As String)
            _curse_id = value
        End Set
    End Property

    Private _versions As List(Of Version)
    Public Property versions() As List(Of Version)
        Get
            Return _versions
        End Get
        Set(ByVal value As List(Of Version))
            _versions = value
        End Set
    End Property

    Private _popularity As Popularity
    Public Property popularity() As Popularity
        Get
            Return _popularity
        End Get
        Set(ByVal value As Popularity)
            _popularity = value
        End Set
    End Property

    Private _plugin_name As String
    Public Property plugin_name() As String
        Get
            Return _plugin_name
        End Get
        Set(ByVal value As String)
            _plugin_name = value
        End Set
    End Property

    Private _server As String
    Public Property server() As String
        Get
            Return _server
        End Get
        Set(ByVal value As String)
            _server = value
        End Set
    End Property

    Private _curse_link As String
    Public Property curse_link() As String
        Get
            Return _curse_link
        End Get
        Set(ByVal value As String)
            _curse_link = value
        End Set
    End Property

    Private _OpenURLCommand As RelayCommand
    Public ReadOnly Property OpenURLCommand As RelayCommand
        Get
            If _OpenURLCommand Is Nothing Then
                _OpenURLCommand = New RelayCommand(Sub(e)
                                                       Select Case e.ToString()
                                                           Case "Curse"
                                                               Process.Start(Me.curse_link)
                                                           Case "Website"
                                                               Process.Start(Me.website)
                                                           Case "BukkitSite"
                                                               Process.Start(Me.dbo_page)
                                                       End Select
                                                   End Sub)
            End If
            Return _OpenURLCommand
        End Get
    End Property

    Private _logo_full As String
    Public Property logo_full() As String
        Get
            Return _logo_full
        End Get
        Set(ByVal value As String)
            _logo_full = value
        End Set
    End Property

    Private _authors As List(Of String)
    Public Property authors() As List(Of String)
        Get
            Return _authors
        End Get
        Set(ByVal value As List(Of String))
            _authors = value
        End Set
    End Property

    Private _logo As String
    Public Property logo() As String
        Get
            Return _logo
        End Get
        Set(ByVal value As String)
            _logo = value
        End Set
    End Property

    Private _slug As String
    Public Property slug() As String
        Get
            Return _slug
        End Get
        Set(ByVal value As String)
            _slug = value
        End Set
    End Property

    Private _categories As List(Of String)
    Public Property categories() As List(Of String)
        Get
            Return _categories
        End Get
        Set(ByVal value As List(Of String))
            _categories = value
        End Set
    End Property

    Private _stage As String
    Public Property stage() As String
        Get
            Return _stage
        End Get
        Set(ByVal value As String)
            _stage = value
        End Set
    End Property

End Class
