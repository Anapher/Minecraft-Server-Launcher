Imports org.phybros.thrift
Imports System.IO
Imports System.Drawing

Public Class ItemStackToImage
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim itmstack = DirectCast(value, ItemStack)
        If itmstack Is Nothing Then Return Binding.DoNothing
        Dim itempath = String.Format("pack://application:,,,/resources/items/{0}-{1}.png", itmstack.TypeId, 0)
        Dim s = New Uri(itempath, UriKind.Absolute)
        
        Return New BitmapImage(s)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function
End Class
