Imports System.IO
Imports System.IO.Compression

Public Class MinecraftHelper
    Public Shared Function GetServerlogs(dir As DirectoryInfo) As List(Of ServerlogNode)
        Dim myserverlogs As New List(Of ServerlogNode)
        For Each fi As FileInfo In dir.GetFiles("*.gz")
            Dim sSplit = fi.Name.Split("-"c)
            Dim datum = New Date(Integer.Parse(sSplit(0)), Integer.Parse(sSplit(1)), Integer.Parse(sSplit(2)))
            Dim Number = sSplit(3).Split("."c)(0)
            Dim currentNode As ServerlogNode = Nothing
            For Each s In myserverlogs
                If s.Datum.Equals(datum) Then
                    currentNode = s
                    Exit For
                End If
            Next
            If currentNode Is Nothing Then currentNode = New ServerlogNode(datum) : myserverlogs.Add(currentNode)
            If fi.Length \ 1024 > 30 Then
                Dim serverlog As New Serverlog(datum, String.Format(Application.Current.FindResource("FileIsToLarge").ToString(), fi.FullName), Number)
                currentNode.Serverlogs.Add(serverlog)
                Continue For
            End If
            Using inFile As FileStream = fi.OpenRead()
                Using Decompress As GZipStream = New GZipStream(inFile, CompressionMode.Decompress)
                    Using sr As New StreamReader(Decompress)
                        Dim serverlogstring As New Text.StringBuilder()
                        While Not sr.EndOfStream
                            serverlogstring.AppendLine(sr.ReadLine())
                        End While
                        Dim serverlog As New Serverlog(datum, serverlogstring.ToString(), Number)
                        currentNode.Serverlogs.Add(serverlog)
                    End Using
                End Using
            End Using
        Next
        Return myserverlogs
    End Function
End Class
