
Public NotInheritable Class TextBoxBehaviour
    Private Sub New()
    End Sub
    Public Shared ReadOnly AlwaysScrollToEndProperty As DependencyProperty = DependencyProperty.RegisterAttached("AlwaysScrollToEnd", GetType(Boolean), GetType(TextBoxBehaviour), New PropertyMetadata(False, AddressOf AlwaysScrollToEndChanged))

    Private Shared Sub AlwaysScrollToEndChanged(sender As Object, e As DependencyPropertyChangedEventArgs)
        Dim tb As TextBox = TryCast(sender, TextBox)
        If tb IsNot Nothing Then
            Dim alwaysScrollToEnd As Boolean = (e.NewValue IsNot Nothing) AndAlso CBool(e.NewValue)
            If alwaysScrollToEnd Then
                tb.ScrollToEnd()
                AddHandler tb.TextChanged, AddressOf TextChanged
            Else
                RemoveHandler tb.TextChanged, AddressOf TextChanged
            End If
        Else
            Throw New InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to TextBox instances.")
        End If
    End Sub

    Public Shared Function GetAlwaysScrollToEnd(textBox As TextBox) As Boolean
        If textBox Is Nothing Then
            Throw New ArgumentNullException("textBox")
        End If

        Return CBool(textBox.GetValue(AlwaysScrollToEndProperty))
    End Function

    Public Shared Sub SetAlwaysScrollToEnd(textBox As TextBox, alwaysScrollToEnd As Boolean)
        If textBox Is Nothing Then
            Throw New ArgumentNullException("textBox")
        End If

        textBox.SetValue(AlwaysScrollToEndProperty, alwaysScrollToEnd)
    End Sub

    Private Shared Sub TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim txt = TryCast(sender, TextBox)
        If txt IsNot Nothing Then
            Try
                txt.Focus()
                txt.CaretIndex = txt.Text.Length
                txt.ScrollToEnd()
            Catch
            End Try
        End If
    End Sub
End Class