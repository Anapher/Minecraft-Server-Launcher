Public Class frmInfoBox
    Implements ComponentModel.INotifyPropertyChanged

    Public Sub New(Text As String, Title As String, OK As String)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.Text = Text
        Me.Title = Title
        Me.OKText = OK
    End Sub

    Private _text As String
    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            If _text <> value Then
                _text = value
                myPropertyChanged("Text")
            End If
        End Set
    End Property

    Private _OKText As String
    Public Property OKText() As String
        Get
            Return _OKText
        End Get
        Set(ByVal value As String)
            If value <> _OKText Then
                _OKText = value
                myPropertyChanged("OKText")
            End If

        End Set
    End Property

    Private _OKCommand As RelayCommand
    Public ReadOnly Property OKCommand() As RelayCommand
        Get
            If _OKCommand Is Nothing Then _OKCommand = New RelayCommand(Sub(parameter As Object)
                                                                            Me.Close()
                                                                        End Sub)
            Return _OKCommand
        End Get
    End Property
#Region "INotifyPropertyChanged"
    Private Sub myPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(PropertyName))
    End Sub
    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region
End Class
