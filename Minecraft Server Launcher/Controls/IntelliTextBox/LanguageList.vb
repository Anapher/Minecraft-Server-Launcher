<Serializable> _
Public Class LanguageList
    Inherits PropertyChangedBase

    Private _Deutsch As String
    Public Property Deutsch() As String
        Get
            Return _Deutsch
        End Get
        Set(ByVal value As String)
            SetProperty(value, _Deutsch)
        End Set
    End Property

    Private _English As String
    Public Property English() As String
        Get
            Return _English
        End Get
        Set(ByVal value As String)
            SetProperty(value, _English)
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Overrides Function ToString() As String
        If String.IsNullOrWhiteSpace(English) Then
            Return Deutsch
        Else
            Return English
        End If
    End Function

    Public Sub New(Deutsch As String, English As String)
        Me.Deutsch = Deutsch
        Me.English = English
    End Sub

    Public Function GetText(region As String) As String
        Dim result = String.Empty
        Select Case region
            Case "de-de"
                If Not String.IsNullOrEmpty(Deutsch) Then
                    result = Deutsch
                Else
                    result = English
                End If
            Case Else
                If Not String.IsNullOrEmpty(English) Then
                    result = English
                Else
                    result = Deutsch
                End If
        End Select
        Return result
    End Function
End Class