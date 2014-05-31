Public Class BannedPlayer
    Private _name As String
    Public Property name() As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Private _created As DateTime
    Public Property created() As DateTime
        Get
            Return _created
        End Get
        Set(ByVal value As DateTime)
            _created = value
        End Set
    End Property

    Private _source As String
    Public Property source() As String
        Get
            Return _source
        End Get
        Set(ByVal value As String)
            _source = value
        End Set
    End Property

    Private _expires As String
    Public Property expires() As String
        Get
            Return _expires
        End Get
        Set(ByVal value As String)
            _expires = value
        End Set
    End Property

    Private _reason As String
    Public Property reason() As String
        Get
            Return _reason
        End Get
        Set(ByVal value As String)
            _reason = value
        End Set
    End Property

    Private _uuid As String
    Public Property uuid() As String
        Get
            Return _uuid
        End Get
        Set(ByVal value As String)
            _uuid = value
        End Set
    End Property

    Private _ip As String
    Public Property ip() As String
        Get
            Return _ip
        End Get
        Set(ByVal value As String)
            _ip = value
        End Set
    End Property

    Public ReadOnly Property IPIsBanned As Boolean
        Get
            Return Not String.IsNullOrEmpty(_ip)
        End Get
    End Property

    Public ReadOnly Property Text As String
        Get
            If String.IsNullOrEmpty(name) Then Return ip Else Return name
        End Get
    End Property

    Public ReadOnly Property CreatedFormatted As String
        Get
            Dim cultureinfo = System.Threading.Thread.CurrentThread.CurrentCulture
            Return created.ToString(cultureinfo.DateTimeFormat)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return name
    End Function
End Class