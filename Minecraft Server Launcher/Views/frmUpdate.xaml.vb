Public Class frmUpdate
    Implements ComponentModel.INotifyPropertyChanged

    Private _Updater As Updater
    Public Property Updater() As Updater
        Get
            Return _Updater
        End Get
        Set(ByVal value As Updater)
            If value IsNot _Updater Then
                _Updater = value
                myPropertyChanged("Updater")
            End If
        End Set
    End Property

    Public Sub New(Updater As Updater)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.Updater = Updater
    End Sub

    Private _Update As RelayCommand
    Public ReadOnly Property Update As RelayCommand
        Get
            If _Update Is Nothing Then _Update = New RelayCommand(Sub(parameter As Object)
                                                                      Updater.Update()
                                                                  End Sub)
            Return _Update
        End Get
    End Property

    Private Sub myPropertyChanged(propertyname As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(propertyname))
    End Sub

    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
