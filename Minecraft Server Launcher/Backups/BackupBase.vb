<Serializable> _
<Xml.Serialization.XmlInclude(GetType(BackupFile))> _
<Xml.Serialization.XmlInclude(GetType(BackupFolder))> _
Public MustInherit Class BackupBase
    Implements ComponentModel.INotifyPropertyChanged

    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _Path As String
    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(ByVal value As String)
            _Path = value
        End Set
    End Property

    Public MustOverride Property BackupFiles As List(Of BackupBase)

    Private _IsChecked As Boolean
    <Xml.Serialization.XmlIgnore> _
    Public Property IsChecked() As Boolean
        Get
            Return _IsChecked
        End Get
        Set(ByVal value As Boolean)
            If Not value = _IsChecked Then
                _IsChecked = value
                RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs("IsChecked"))
                If Me.BackupFiles IsNot Nothing AndAlso Me.BackupFiles.Count > 0 Then
                    For Each b In Me.BackupFiles
                        b.IsChecked = value
                    Next
                End If
                RaiseEvent CheckedChanged(Me, EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event CheckedChanged(sender As Object, e As EventArgs)
    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class