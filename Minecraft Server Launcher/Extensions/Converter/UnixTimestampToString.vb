Public Class UnixTimestampToString
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If value Is Nothing Then Return Binding.DoNothing
        Dim mydatetime = UnixTimeStampToDateTime(Double.Parse(value.ToString()))
        Return mydatetime.ToString("dd.MM.yyyy - HH:mm:ss")
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function
    Private ReadOnly UnixEpoch As New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
    Private ReadOnly MaxUnixSeconds As Double = (DateTime.MaxValue - UnixEpoch).TotalSeconds
    Private Function UnixTimeStampToDateTime(unixTimeStamp As Double) As DateTime
        Return If(unixTimeStamp > MaxUnixSeconds, UnixEpoch.AddMilliseconds(unixTimeStamp), UnixEpoch.AddSeconds(unixTimeStamp))
    End Function
End Class
