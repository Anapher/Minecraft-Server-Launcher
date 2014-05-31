<Serializable> _
Public Class BackupFile
    Inherits BackupBase

    Public Overrides Property BackupFiles() As List(Of BackupBase)
        Get
            Return New List(Of BackupBase)
        End Get
        Set(value As List(Of BackupBase))
            Throw New ArgumentException()
        End Set
    End Property
End Class