Public Class InstalledStateToIcon
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim bool = DirectCast(value, Nullable(Of Boolean))
        If bool Then
            Return New BitmapImage(New Uri("../resources/state/check.png", UriKind.Relative))
        ElseIf Not bool Then
            Return New BitmapImage(New Uri("../resources/state/uncheck.png", UriKind.Relative))
        Else
            Return New BitmapImage(New Uri("../resources/state/none.png", UriKind.Relative))
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function
End Class
