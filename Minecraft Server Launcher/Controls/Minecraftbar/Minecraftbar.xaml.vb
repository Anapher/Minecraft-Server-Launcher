Public Class Minecraftbar
    Private ReadOnly HeartFullUri As New Uri("/Controls/Minecraftbar/resources/MetroHeartFull.png", UriKind.Relative)
    Private ReadOnly HeartHalfUri As New Uri("/Controls/Minecraftbar/resources/MetroHeartHalf.png", UriKind.Relative)
    Private ReadOnly HeartEmptyUri As New Uri("/Controls/Minecraftbar/resources/MetroHeartEmpty.png", UriKind.Relative)

    Private ReadOnly FoodFullUri As New Uri("/Controls/Minecraftbar/resources/MetroFoodFull.png", UriKind.Relative)
    Private ReadOnly FoodHalfUri As New Uri("/Controls/Minecraftbar/resources/MetroFoodHalf.png", UriKind.Relative)
    Private ReadOnly FoodEmptyUri As New Uri("/Controls/Minecraftbar/resources/MetroFoodEmpty.png", UriKind.Relative)

    Private ImageArray As List(Of Image)
    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        ImageArray = New List(Of Image)
        ImageArray.AddRange({img0, img1, img2, img3, img4, img5, img6, img7, img8, img9})
        UpdateImages(0)
        ImageArray.ForEach(Sub(img)
                               img.Width = 16
                               img.Height = 16
                           End Sub)
    End Sub

    Public Shared ReadOnly BarTypeProperty As DependencyProperty = DependencyProperty.Register("BarType", GetType(MinecraftbarType), GetType(Minecraftbar), New FrameworkPropertyMetadata(MinecraftbarType.Health, New PropertyChangedCallback(AddressOf UpdateImages)))
    Public Property BarType As MinecraftbarType
        Get
            Return DirectCast(Me.GetValue(BarTypeProperty), MinecraftbarType)
        End Get
        Set(value As MinecraftbarType)
            Me.SetValue(BarTypeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Integer), GetType(Minecraftbar), New FrameworkPropertyMetadata(0, New PropertyChangedCallback(AddressOf Update)))

    Private _value As Integer
    Public Property Value() As Integer
        Get
            Return DirectCast(Me.GetValue(ValueProperty), Integer)
        End Get
        Set(ByVal value As Integer)
            Me.SetValue(ValueProperty, value)
        End Set
    End Property

    Private Shared Sub Update(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim i = TryCast(d, Minecraftbar)
        If i IsNot Nothing Then i.UpdateImages(DirectCast(e.NewValue, Integer))
    End Sub

    Private Shared Sub UpdateImages(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim i = TryCast(d, Minecraftbar)
        If i IsNot Nothing Then i.UpdateImages(i.Value)
    End Sub

    Private Sub UpdateImages(NewValue As Integer)
        Dim overflowvalue = NewValue
        Dim fullvalue As BitmapImage = Nothing
        Dim halfvalue As BitmapImage = Nothing
        Dim novalue As BitmapImage = Nothing
        Select Case Me.BarType
            Case MinecraftbarType.Health
                fullvalue = New BitmapImage(HeartFullUri)
                halfvalue = New BitmapImage(HeartHalfUri)
                novalue = New BitmapImage(HeartEmptyUri)
            Case MinecraftbarType.Food
                fullvalue = New BitmapImage(FoodFullUri)
                halfvalue = New BitmapImage(FoodHalfUri)
                novalue = New BitmapImage(FoodEmptyUri)
        End Select
        ImageArray.ForEach(Sub(img) img.Source = novalue)
        If overflowvalue = 0 Then
            ImageArray.ForEach(Sub(i) i.Source = novalue)
        Else
            Dim currentimg As Integer = 0
            For i = 0 To NewValue Step 2
                If overflowvalue >= 2 Then
                    ImageArray(currentimg).Source = fullvalue
                    currentimg += 1
                    overflowvalue -= 2
                ElseIf overflowvalue = 1 Then
                    ImageArray(currentimg).Source = halfvalue
                    Exit For
                End If
            Next
        End If
    End Sub
End Class

Public Enum MinecraftbarType
    Health
    Food
End Enum