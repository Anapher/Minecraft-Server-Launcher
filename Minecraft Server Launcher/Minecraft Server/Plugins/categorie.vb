Imports System.Globalization

Public Class categorie
    Private _Image As BitmapImage
    Public Property Image() As BitmapImage
        Get
            Return _Image
        End Get
        Set(ByVal value As BitmapImage)
            _Image = value
        End Set
    End Property

    Private _ToolTip As String
    Public Property Tooltip() As String
        Get
            Return _ToolTip
        End Get
        Set(ByVal value As String)
            _ToolTip = value
        End Set
    End Property

End Class
