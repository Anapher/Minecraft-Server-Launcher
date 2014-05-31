Imports System.Globalization
Imports System.Windows.Markup
Imports System.ComponentModel

<ContentProperty("Text")> _
Public Class OutlinedTextBlock
    Inherits FrameworkElement
    Public Shared ReadOnly FillProperty As DependencyProperty = DependencyProperty.Register("Fill", GetType(Brush), GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender))

    Public Shared ReadOnly StrokeProperty As DependencyProperty = DependencyProperty.Register("Stroke", GetType(Brush), GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender))

    Public Shared ReadOnly StrokeThicknessProperty As DependencyProperty = DependencyProperty.Register("StrokeThickness", GetType(Double), GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender))

    Public Shared ReadOnly FontFamilyProperty As DependencyProperty = TextElement.FontFamilyProperty.AddOwner(GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextUpdated))

    Public Shared ReadOnly FontSizeProperty As DependencyProperty = TextElement.FontSizeProperty.AddOwner(GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextUpdated))

    Public Shared ReadOnly FontStretchProperty As DependencyProperty = TextElement.FontStretchProperty.AddOwner(GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextUpdated))

    Public Shared ReadOnly FontStyleProperty As DependencyProperty = TextElement.FontStyleProperty.AddOwner(GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextUpdated))

    Public Shared ReadOnly FontWeightProperty As DependencyProperty = TextElement.FontWeightProperty.AddOwner(GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextUpdated))

    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextInvalidated))

    Public Shared ReadOnly TextAlignmentProperty As DependencyProperty = DependencyProperty.Register("TextAlignment", GetType(TextAlignment), GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextUpdated))

    Public Shared ReadOnly TextDecorationsProperty As DependencyProperty = DependencyProperty.Register("TextDecorations", GetType(TextDecorationCollection), GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextUpdated))

    Public Shared ReadOnly TextTrimmingProperty As DependencyProperty = DependencyProperty.Register("TextTrimming", GetType(TextTrimming), GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(AddressOf OnFormattedTextUpdated))

    Public Shared ReadOnly TextWrappingProperty As DependencyProperty = DependencyProperty.Register("TextWrapping", GetType(TextWrapping), GetType(OutlinedTextBlock), New FrameworkPropertyMetadata(TextWrapping.NoWrap, AddressOf OnFormattedTextUpdated))

    Private formattedText As FormattedText
    Private textGeometry As Geometry

    Public Sub New()
        Me.TextDecorations = New TextDecorationCollection()
    End Sub

    Public Property Fill() As Brush
        Get
            Return DirectCast(GetValue(FillProperty), Brush)
        End Get
        Set(value As Brush)
            SetValue(FillProperty, value)
        End Set
    End Property

    Public Property FontFamily() As FontFamily
        Get
            Return DirectCast(GetValue(FontFamilyProperty), FontFamily)
        End Get
        Set(value As FontFamily)
            SetValue(FontFamilyProperty, value)
        End Set
    End Property

    <TypeConverter(GetType(FontSizeConverter))> _
    Public Property FontSize() As Double
        Get
            Return CDbl(GetValue(FontSizeProperty))
        End Get
        Set(value As Double)
            SetValue(FontSizeProperty, value)
        End Set
    End Property

    Public Property FontStretch() As FontStretch
        Get
            Return DirectCast(GetValue(FontStretchProperty), FontStretch)
        End Get
        Set(value As FontStretch)
            SetValue(FontStretchProperty, value)
        End Set
    End Property

    Public Property FontStyle() As FontStyle
        Get
            Return DirectCast(GetValue(FontStyleProperty), FontStyle)
        End Get
        Set(value As FontStyle)
            SetValue(FontStyleProperty, value)
        End Set
    End Property

    Public Property FontWeight() As FontWeight
        Get
            Return DirectCast(GetValue(FontWeightProperty), FontWeight)
        End Get
        Set(value As FontWeight)
            SetValue(FontWeightProperty, value)
        End Set
    End Property

    Public Property Stroke() As Brush
        Get
            Return DirectCast(GetValue(StrokeProperty), Brush)
        End Get
        Set(value As Brush)
            SetValue(StrokeProperty, value)
        End Set
    End Property

    Public Property StrokeThickness() As Double
        Get
            Return CDbl(GetValue(StrokeThicknessProperty))
        End Get
        Set(value As Double)
            SetValue(StrokeThicknessProperty, value)
        End Set
    End Property

    Public Property Text() As String
        Get
            Return DirectCast(GetValue(TextProperty), String)
        End Get
        Set(value As String)
            SetValue(TextProperty, value)
        End Set
    End Property

    Public Property TextAlignment() As TextAlignment
        Get
            Return DirectCast(GetValue(TextAlignmentProperty), TextAlignment)
        End Get
        Set(value As TextAlignment)
            SetValue(TextAlignmentProperty, value)
        End Set
    End Property

    Public Property TextDecorations() As TextDecorationCollection
        Get
            Return DirectCast(Me.GetValue(TextDecorationsProperty), TextDecorationCollection)
        End Get
        Set(value As TextDecorationCollection)
            Me.SetValue(TextDecorationsProperty, value)
        End Set
    End Property

    Public Property TextTrimming() As TextTrimming
        Get
            Return DirectCast(GetValue(TextTrimmingProperty), TextTrimming)
        End Get
        Set(value As TextTrimming)
            SetValue(TextTrimmingProperty, value)
        End Set
    End Property

    Public Property TextWrapping() As TextWrapping
        Get
            Return DirectCast(GetValue(TextWrappingProperty), TextWrapping)
        End Get
        Set(value As TextWrapping)
            SetValue(TextWrappingProperty, value)
        End Set
    End Property

    Protected Overrides Sub OnRender(drawingContext As DrawingContext)
        Me.EnsureGeometry()

        drawingContext.DrawGeometry(Me.Fill, New Pen(Me.Stroke, Me.StrokeThickness), Me.textGeometry)
    End Sub

    Protected Overrides Function MeasureOverride(availableSize As Size) As Size
        Me.EnsureFormattedText()

        ' constrain the formatted text according to the available size
        ' the Math.Min call is important - without this constraint (which seems arbitrary, but is the maximum allowable text width), things blow up when availableSize is infinite in both directions
        ' the Math.Max call is to ensure we don't hit zero, which will cause MaxTextHeight to throw
        Me.formattedText.MaxTextWidth = Math.Min(3579139, availableSize.Width)
        Me.formattedText.MaxTextHeight = Math.Max(0.0001, availableSize.Height)

        ' return the desired size
        Return New Size(Me.formattedText.Width, Me.formattedText.Height)
    End Function

    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        Me.EnsureFormattedText()

        ' update the formatted text with the final size
        Me.formattedText.MaxTextWidth = finalSize.Width
        Me.formattedText.MaxTextHeight = finalSize.Height

        ' need to re-generate the geometry now that the dimensions have changed
        Me.textGeometry = Nothing

        Return finalSize
    End Function

    Private Shared Sub OnFormattedTextInvalidated(dependencyObject As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim outlinedTextBlock = DirectCast(dependencyObject, OutlinedTextBlock)
        outlinedTextBlock.formattedText = Nothing
        outlinedTextBlock.textGeometry = Nothing

        outlinedTextBlock.InvalidateMeasure()
        outlinedTextBlock.InvalidateVisual()
    End Sub

    Private Shared Sub OnFormattedTextUpdated(dependencyObject As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim outlinedTextBlock = DirectCast(dependencyObject, OutlinedTextBlock)
        outlinedTextBlock.UpdateFormattedText()
        outlinedTextBlock.textGeometry = Nothing

        outlinedTextBlock.InvalidateMeasure()
        outlinedTextBlock.InvalidateVisual()
    End Sub

    Private Sub EnsureFormattedText()
        If Me.formattedText IsNot Nothing OrElse Me.Text Is Nothing Then
            Return
        End If

        Me.formattedText = New FormattedText(Me.Text, CultureInfo.CurrentUICulture, Me.FlowDirection, New Typeface(Me.FontFamily, Me.FontStyle, Me.FontWeight, FontStretches.Normal), Me.FontSize, Brushes.Black)

        Me.UpdateFormattedText()
    End Sub

    Private Sub UpdateFormattedText()
        If Me.formattedText Is Nothing Then
            Return
        End If

        Me.formattedText.MaxLineCount = If(Me.TextWrapping = TextWrapping.NoWrap, 1, Integer.MaxValue)
        Me.formattedText.TextAlignment = Me.TextAlignment
        Me.formattedText.Trimming = Me.TextTrimming

        Me.formattedText.SetFontSize(Me.FontSize)
        Me.formattedText.SetFontStyle(Me.FontStyle)
        Me.formattedText.SetFontWeight(Me.FontWeight)
        Me.formattedText.SetFontFamily(Me.FontFamily)
        Me.formattedText.SetFontStretch(Me.FontStretch)
        Me.formattedText.SetTextDecorations(Me.TextDecorations)
    End Sub

    Private Sub EnsureGeometry()
        If Me.textGeometry IsNot Nothing Then
            Return
        End If

        Me.EnsureFormattedText()
        Me.textGeometry = Me.formattedText.BuildGeometry(New Point(0, 0))
    End Sub
End Class
