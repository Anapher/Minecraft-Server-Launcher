Public Class ExhaustionVisualizer
    Public Shared ReadOnly ExhaustionValueProperty As DependencyProperty = DependencyProperty.Register("ExhaustionValue", GetType(Double), GetType(ExhaustionVisualizer), New FrameworkPropertyMetadata(AddressOf ExhaustionValueChanged))
    Public Property ExhaustionValue As Integer
        Get
            Return DirectCast(Me.GetValue(ExhaustionValueProperty), Integer)
        End Get
        Set(value As Integer)
            Me.SetValue(ExhaustionValueProperty, value)
        End Set
    End Property

    Private Shared Sub ExhaustionValueChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim dp = TryCast(d, ExhaustionVisualizer)
        If dp IsNot Nothing Then
            Select Case DirectCast(e.NewValue, Double)
                Case Is < 2
                    dp.img.Source = New BitmapImage(New Uri("/Controls/ExhaustionVisualizer/resources/Low.png", UriKind.Relative))
                Case 2 To 3.5
                    dp.img.Source = New BitmapImage(New Uri("/Controls/ExhaustionVisualizer/resources/Normal.png", UriKind.Relative))
                Case Is > 3.5
                    dp.img.Source = New BitmapImage(New Uri("/Controls/ExhaustionVisualizer/resources/High.png", UriKind.Relative))
            End Select
        End If
    End Sub
End Class
