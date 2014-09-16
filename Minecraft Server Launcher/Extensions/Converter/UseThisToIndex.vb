Public Class UseThisToIndex
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Select Case DirectCast([Enum].Parse(GetType(UseThis), value.ToString()), UseThis)
            Case UseThis.List
                Return 0
            Case UseThis.Playerlist
                Return 1
            Case UseThis.ItemList
                Return 2
            Case UseThis.PluginList
                Return 3
            Case UseThis.BanList
                Return 4
            Case UseThis.BanIPList
                Return 5
            Case UseThis.CommandList
                Return 6
            Case Else
                Return -1
        End Select
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Select Case Integer.Parse(value.ToString())
            Case 0
                Return UseThis.List
            Case 1
                Return UseThis.Playerlist
            Case 2
                Return UseThis.ItemList
            Case 3
                Return UseThis.PluginList
            Case 4
                Return UseThis.BanList
            Case 5
                Return UseThis.BanIPList
            Case 6
                Return UseThis.CommandList
            Case Else
                Return UseThis.List
        End Select
    End Function
End Class
