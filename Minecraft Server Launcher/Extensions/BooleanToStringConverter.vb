Public Class BoolToStringConverter
    Inherits DependencyObject
    Implements IValueConverter

    Public Shared ReadOnly TrueValueProperty As DependencyProperty = DependencyProperty.Register("TrueValue", GetType(String), GetType(BoolToStringConverter))
    Public Shared ReadOnly FalseValueProperty As DependencyProperty = DependencyProperty.Register("FalseValue", GetType(String), GetType(BoolToStringConverter))

    Public Property TrueValue() As String
        Get
            Return Me.GetValue(TrueValueProperty).ToString()
        End Get
        Set(value As String)
            Me.SetValue(TrueValueProperty, value)
        End Set
    End Property

    Public Property FalseValue() As String
        Get
            Return Me.GetValue(FalseValueProperty).ToString()
        End Get
        Set(value As String)
            Me.SetValue(FalseValueProperty, value)
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
