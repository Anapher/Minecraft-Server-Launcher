<Serializable> _
Public Class MinecraftItem
    Private _Name As String
    Private _Id As Integer
    Private _Image As String
    Private _meta As Integer

    Public Property Name() As String
        Get
            Return _Name
        End Get
        <Obsolete("Nicht zu benutzen", True)>
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _Id
        End Get
        <Obsolete("Nicht zu benutzen", True)>
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property

    Public Property Meta As Integer
        Get
            Return _meta
        End Get
        <Obsolete("Nicht zu benutzen", True)>
        Set(value As Integer)
            _meta = value
        End Set
    End Property

    Public ReadOnly Property Hex As String
        Get
            Return ID.ToString("X")
        End Get
    End Property

    Public ReadOnly Property Image As BitmapImage
        Get
            Dim itempath = String.Format("pack://application:,,,/resources/items/{0}-{1}.png", ID, Meta)
            Dim s = New Uri(itempath, UriKind.Absolute)
            Return New BitmapImage(s)
        End Get
    End Property

    Public ReadOnly Property IDToString As String
        Get
            If Me.Meta > 0 Then
                Return String.Format("{0}:{1}", ID, Meta)
            Else
                Return ID.ToString()
            End If
        End Get
    End Property
    Public Sub New(ItemName As String, ID As Integer, Image As String, meta As Integer)
        Me._Name = ItemName
        Me._Id = ID
        Me._Image = Image
        Me._meta = meta
    End Sub

    <Obsolete("Nicht benutzbar", True)> _
    Public Sub New()
    End Sub
End Class
