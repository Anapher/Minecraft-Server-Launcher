<Serializable> _
Public Class LanguageList
    Private _Deutsch As String
    Public Property Deutsch() As String
        Get
            Return _Deutsch
        End Get
        Set(ByVal value As String)
            _Deutsch = value
        End Set
    End Property

    Private _English As String
    Public Property English() As String
        Get
            Return _English
        End Get
        Set(ByVal value As String)
            _English = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(Deutsch As String, English As String)
        Me.Deutsch = Deutsch
        Me.English = English
    End Sub
End Class