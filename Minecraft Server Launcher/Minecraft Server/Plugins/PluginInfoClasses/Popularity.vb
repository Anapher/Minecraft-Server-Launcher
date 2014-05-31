Public Class Popularity
    Private _monthly As Integer
    Public Property monthly() As Integer
        Get
            Return _monthly
        End Get
        Set(ByVal value As Integer)
            _monthly = value
        End Set
    End Property

    Private _total As Integer
    Public Property total() As Integer
        Get
            Return _total
        End Get
        Set(ByVal value As Integer)
            _total = value
        End Set
    End Property

    Private _daily As Integer
    Public Property daily() As Integer
        Get
            Return _daily
        End Get
        Set(ByVal value As Integer)
            _daily = value
        End Set
    End Property

    Private _weekly As Integer
    Public Property weekly() As Integer
        Get
            Return _weekly
        End Get
        Set(ByVal value As Integer)
            _weekly = value
        End Set
    End Property

End Class
