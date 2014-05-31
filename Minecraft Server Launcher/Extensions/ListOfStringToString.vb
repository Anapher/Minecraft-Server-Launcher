Public Class ListOfStringToString
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim lst = DirectCast(value, List(Of String))
        Return String.Join(", ", lst)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function
End Class
