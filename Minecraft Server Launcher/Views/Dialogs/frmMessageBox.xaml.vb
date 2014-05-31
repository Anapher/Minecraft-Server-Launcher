Public Class frmMessageBox
    Implements ComponentModel.INotifyPropertyChanged

    Public Sub New(Text As String, Title As String)
        Me.New(Text, Title, Application.Current.FindResource("Yes").ToString(), Application.Current.FindResource("No").ToString())
    End Sub

    Public Sub New(Text As String, Title As String, YesText As String, NoText As String)
        InitializeComponent()
        Me.Titel = Title
        Me.Text = Text
        Me.ButtonOKText = YesText
        Me.ButtonCancelText = NoText
    End Sub

    Private _Titel As String
    Public Property Titel() As String
        Get
            Return _Titel
        End Get
        Set(ByVal value As String)
            If value <> _Titel Then
                _Titel = value
                MyPropertyChanged("Titel")
            End If
        End Set
    End Property

    Private _Text As String
    Public Property Text() As String
        Get
            Return _Text
        End Get
        Set(ByVal value As String)
            If _Text <> value Then
                _Text = value
                MyPropertyChanged("Text")
            End If
        End Set
    End Property

    Private _ButtonOKText As String
    Public Property ButtonOKText() As String
        Get
            Return _ButtonOKText
        End Get
        Set(ByVal value As String)
            If _ButtonOKText <> value Then
                _ButtonOKText = value
                MyPropertyChanged("ButtonOKText")
            End If
        End Set
    End Property

    Private _ButtonCancelText As String
    Public Property ButtonCancelText() As String
        Get
            Return _ButtonCancelText
        End Get
        Set(ByVal value As String)
            If value <> _ButtonCancelText Then
                _ButtonCancelText = value
                MyPropertyChanged("ButtonCancelText")
            End If
        End Set
    End Property

    Protected Sub MyPropertyChanged(propertyname As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(propertyname))
    End Sub
    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
