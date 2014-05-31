Public NotInheritable Class TextBlockBehaviour
    Private Sub New()
    End Sub
    Public Shared Function GetFormattedText(obj As DependencyObject) As String
        Return DirectCast(obj.GetValue(FormattedTextProperty), String)
    End Function

    Public Shared Sub SetFormattedText(obj As DependencyObject, value As String)
        obj.SetValue(FormattedTextProperty, value)
    End Sub

    Public Shared ReadOnly FormattedTextProperty As DependencyProperty = DependencyProperty.RegisterAttached("FormattedText", GetType(String), GetType(TextBlockBehaviour), New UIPropertyMetadata("", AddressOf FormattedTextChanged))

    Private Shared Function Traverse(value As String) As Inline
        ' Get the sections/inlines
        Dim sections As String() = SplitIntoSections(value)

        ' Check for grouping
        If sections.Length.Equals(1) Then
            Dim section As String = sections(0)
            Dim token As String = ""
            ' E.g <Bold>
            Dim tokenStart As Integer, tokenEnd As Integer
            ' Where the token/section starts and ends.
            ' Check for token
            If GetTokenInfo(section, token, tokenStart, tokenEnd) Then
                ' Get the content to further examination
                Dim content As String = If(token.Length.Equals(tokenEnd - tokenStart), Nothing, section.Substring(token.Length, section.Length - 1 - token.Length * 2))

                Select Case token
                    Case "<Bold>"
                        Return New Bold(Traverse(content))
                    Case "<Italic>"
                        Return New Italic(Traverse(content))
                    Case "<Underline>"
                        Return New Underline(Traverse(content))
                    Case "<LineBreak/>"
                        Return New LineBreak()
                    Case Else
                        Return New Run(section)
                End Select
            Else
                Return New Run(section)
            End If
        Else
            ' Group together
            Dim span As New Span()

            For Each section As String In sections
                span.Inlines.Add(Traverse(section))
            Next

            Return span
        End If
    End Function

    ''' <summary>
    ''' Examines the passed string and find the first token, where it begins and where it ends.
    ''' </summary>
    ''' <param name="value">The string to examine.</param>
    ''' <param name="token">The found token.</param>
    ''' <param name="startIndex">Where the token begins.</param>
    ''' <param name="endIndex">Where the end-token ends.</param>
    ''' <returns>True if a token was found.</returns>
    Private Shared Function GetTokenInfo(value As String, ByRef token As String, ByRef startIndex As Integer, ByRef endIndex As Integer) As Boolean
        token = Nothing
        endIndex = -1

        startIndex = value.IndexOf("<")
        Dim startTokenEndIndex As Integer = value.IndexOf(">")

        If startIndex < 0 Then
            Return False
        End If

        If startTokenEndIndex < 0 Then
            Return False
        End If

        token = value.Substring(startIndex, startTokenEndIndex - startIndex + 1)

        If token.EndsWith("/>") Then
            endIndex = startIndex + token.Length
            Return True
        End If

        Dim endToken As String = token.Insert(1, "/")

        Dim nesting As Integer = 0
        Dim temp_startTokenIndex As Integer = -1
        Dim temp_endTokenIndex As Integer = -1
        Dim pos As Integer = 0
        Do
            temp_startTokenIndex = value.IndexOf(token, pos)
            temp_endTokenIndex = value.IndexOf(endToken, pos)

            If temp_startTokenIndex >= 0 AndAlso temp_startTokenIndex < temp_endTokenIndex Then
                nesting += 1
                pos = temp_startTokenIndex + token.Length
            ElseIf temp_endTokenIndex >= 0 AndAlso nesting > 0 Then
                nesting -= 1
                pos = temp_endTokenIndex + endToken.Length
            Else
                ' Invalid tokenized string
                Return False

            End If
        Loop While nesting > 0
        endIndex = pos
        Return True
    End Function

    ''' <summary>
    ''' Splits the string into sections of tokens and regular text.
    ''' </summary>
    ''' <param name="value">The string to split.</param>
    ''' <returns>An array with the sections.</returns>
    Private Shared Function SplitIntoSections(value As String) As String()
        Dim sections As New List(Of String)()

        While Not String.IsNullOrEmpty(value)
            Dim token As String = ""
            Dim tokenStartIndex As Integer, tokenEndIndex As Integer

            ' Check if this is a token section
            If GetTokenInfo(value, token, tokenStartIndex, tokenEndIndex) Then
                ' Add pretext if the token isn't from the start
                If tokenStartIndex > 0 Then
                    sections.Add(value.Substring(0, tokenStartIndex))
                End If

                sections.Add(value.Substring(tokenStartIndex, tokenEndIndex - tokenStartIndex))
                ' Trim away
                value = value.Substring(tokenEndIndex)
            Else
                ' No tokens, just add the text
                sections.Add(value)
                value = Nothing
            End If
        End While

        Return sections.ToArray()
    End Function

    Private Shared Sub FormattedTextChanged(sender As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim value As String = TryCast(e.NewValue, String)

        Dim textBlock As TextBlock = TryCast(sender, TextBlock)
        textBlock.Inlines.Clear()
        If textBlock IsNot Nothing Then
            textBlock.Inlines.Add(Traverse(value))
        End If
    End Sub
End Class
