<Serializable> _
Public Class MinecraftItems
    Inherits List(Of MinecraftItem)

    Public Shared Function Load(Path As String) As MinecraftItems
        Dim result As MinecraftItems
        If Not IO.File.Exists(Path) Then Return Nothing
        Using s As New IO.StreamReader(Path)
            Dim xmls = New Xml.Serialization.XmlSerializer(GetType(MinecraftItems))
            result = DirectCast(xmls.Deserialize(s), MinecraftItems)
            Return result
        End Using
    End Function

    Public Function ContainItem(id As Integer, meta As Integer) As Boolean
        Dim result = False
        Me.ForEach(Sub(itm As MinecraftItem)
                       If itm.ID = id AndAlso itm.Meta = meta Then
                           result = True
                           Return
                       End If
                   End Sub)
        Return result
    End Function

    Public Sub New()
    End Sub
End Class
