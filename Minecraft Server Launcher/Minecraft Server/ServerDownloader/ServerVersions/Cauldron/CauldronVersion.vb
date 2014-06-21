Public Class CauldronVersion
    Inherits ServerVersion

    Private _Downloadlink As String
    Public Overrides ReadOnly Property DownloadLink As String
        Get
            Return _Downloadlink
        End Get
    End Property

    Private _Version As String
    Public ReadOnly Property Version() As String
        Get
            Return _Version
        End Get
    End Property

    Private _MinecraftVersion As String
    Public ReadOnly Property MinecraftVersion() As String
        Get
            Return _MinecraftVersion
        End Get
    End Property

    Private _Time As DateTime
    Public ReadOnly Property Time() As DateTime
        Get
            Return _Time
        End Get
    End Property

    Public Sub New(Version As String, MinecraftVersion As String, Time As DateTime, Downloadlink As String)
        Me._Version = Version
        Me._MinecraftVersion = MinecraftVersion
        Me._Time = Time
        Me._Downloadlink = Downloadlink
    End Sub
End Class
