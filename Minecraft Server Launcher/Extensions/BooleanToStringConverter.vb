Public Class BoolToStringConverter
    Implements IValueConverter

    Private _FalseValue As String
    Public Property FalseValue() As String
        Get
            Return _FalseValue
        End Get
        Set(value As String)
            _FalseValue = value
        End Set
    End Property

    Private _TrueValue As String
    Public Property TrueValue() As String
        Get
            Return _TrueValue
        End Get
        Set(value As String)
            _TrueValue = value
        End Set
    End Property

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If value Is Nothing Then
            Return FalseValue
        Else
            If Boolean.Parse(value.ToString()) Then Return TrueValue Else Return FalseValue
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If value IsNot Nothing Then
            Return value.Equals(TrueValue)
        Else
            Return False
        End If
    End Function
End Class
