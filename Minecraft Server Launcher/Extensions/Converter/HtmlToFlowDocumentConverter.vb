Imports System.IO
Imports System.Xml
Imports System.Diagnostics
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows
Imports System.Windows.Documents
Imports HTMLConverter

Public Class HtmlToFlowDocumentConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If value IsNot Nothing Then
            Dim flowDocument As New FlowDocument()
            Dim fixedString = System.Text.RegularExpressions.Regex.Replace(value.ToString(), "font-size:.*%", "font-size:18")
            Dim xaml As String = HtmlToXamlConverter.ConvertHtmlToXaml(fixedString, False)

            Using stream As New MemoryStream((New ASCIIEncoding()).GetBytes(xaml))
                Dim text As New TextRange(flowDocument.ContentStart, flowDocument.ContentEnd)
                Try
                    text.Load(stream, DataFormats.Xaml)
                Catch ex As Exception
                    Return Binding.DoNothing
                End Try
            End Using
            For Each b In flowDocument.Blocks
                Dim c = TryCast(b, Paragraph)
                If c IsNot Nothing Then
                    For Each i In c.Inlines
                        If i.GetType() Is GetType(Hyperlink) Then
                            Dim hyperlink = DirectCast(i, Hyperlink)
                            AddHandler hyperlink.RequestNavigate, AddressOf HyperLinkRequestNagigation
                            Dim s = hyperlink.NavigateUri.AbsoluteUri
                        End If
                    Next
                End If
            Next
            Return flowDocument
        End If
        Return value
    End Function

    Private Sub HyperLinkRequestNagigation(sender As Object, e As Navigation.RequestNavigateEventArgs)
        Process.Start(e.Uri.AbsoluteUri)
    End Sub

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function
End Class