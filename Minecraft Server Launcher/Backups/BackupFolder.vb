Public Class BackupFolder
    Inherits BackupBase
    Private _BackupFiles As List(Of BackupBase)

    Public Overrides Property BackupFiles() As List(Of BackupBase)
        Get
            Return _BackupFiles
        End Get
        Set(ByVal value As List(Of BackupBase))
            _BackupFiles = value
        End Set
    End Property
End Class