<Serializable> _
Public Class Ram
    Private _ram As Integer
    Public Property Ram As Integer
        Get
            Return _ram
        End Get
        Set(value As Integer)
            _ram = value
        End Set
    End Property

    Private _Text As String
    <Xml.Serialization.XmlIgnore> _
    Public Property Text() As String
        Get
            Return _Text
        End Get
        Set(ByVal value As String)
            _Text = value
        End Set
    End Property

    Public Sub New(ram As Integer, text As String)
        Me.Ram = ram
        Me.Text = text
    End Sub

    Public Sub New()
    End Sub
End Class