Imports org.phybros.thrift

Public Class LocationToStringConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If value Is Nothing Then Return "X: 0, Y: 0, Z: 0"
        Dim location = DirectCast(value, Location)
        Return String.Format("X: {0}, Y: {1}, Z: {2}", Math.Round(location.X, 1), Math.Round(location.Y, 1), Math.Round(location.Z, 1))
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function
End Class
