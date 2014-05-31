Public Class StateChangedEventArgs
    Inherits EventArgs
    Private _NewLine As String
    Public ReadOnly Property NewLine As String
        Get
            Return _NewLine
        End Get
    End Property

    Public Sub New(Line As String)
        _NewLine = Line
    End Sub
End Class