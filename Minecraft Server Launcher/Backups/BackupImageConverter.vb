Public Class BackupImageConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is BackupFolder Then
            Return New BitmapImage(New Uri("../resources/folder.png", UriKind.Relative))
        Else
            Return New BitmapImage(New Uri("../resources/file.png", UriKind.Relative))
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function
End Class
