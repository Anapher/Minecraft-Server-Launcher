Public Class Permission
    Private _default As Object
    Public Property [default]() As Object
        Get
            Return _default
        End Get
        Set(value As Object)
            _default = value
        End Set
    End Property

    Private _role As String
    Public Property role() As String
        Get
            Return _role
        End Get
        Set(value As String)
            _role = value
        End Set
    End Property
End Class
