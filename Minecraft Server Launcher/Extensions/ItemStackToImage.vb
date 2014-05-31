Imports org.phybros.thrift
Imports System.IO
Imports System.Drawing

Public Class ItemStackToImage
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim itmstack = DirectCast(value, ItemStack)
        If itmstack Is Nothing Then Return Nothing
        Dim itempath = String.Format("pack://application:,,,/resources/items/{0}-{1}.png", itmstack.TypeId, itmstack.Data)
        Dim s = New Uri(itempath, UriKind.Absolute)
        If ItemData.Instance.lstMinecraftItem IsNot Nothing AndAlso Not ItemData.Instance.lstMinecraftItem.ContainItem(itmstack.TypeId, itmstack.Data) Then
            If ItemData.Instance.lstMinecraftItem.ContainItem(itmstack.TypeId, 0) Then
                s = New Uri(String.Format("pack://application:,,,/resources/items/{0}-0.png", itmstack.TypeId))
            Else
                Return New BitmapImage(New Uri("pack://application:,,,/resources/items/placeholder.png", UriKind.Absolute))
            End If
        End If
        If itmstack.TypeId = 0 AndAlso parameter IsNot Nothing AndAlso parameter.ToString() = "ReturnNothing" Then
            Return Nothing
        End If
        Return New BitmapImage(s)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function
End Class
