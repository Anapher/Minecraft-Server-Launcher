Imports System.IO

Public Class Paths
    Private Shared _Instance As Paths
    Public Shared ReadOnly Property GetPaths() As Paths
        Get
            If _Instance Is Nothing Then _Instance = New Paths
            Return _Instance
        End Get
    End Property

    Public ReadOnly Property MinecraftServerLauncherPath() As DirectoryInfo
        Get
            Return New DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MSL"))
        End Get
    End Property

    Public ReadOnly Property MinecraftServerFolder() As DirectoryInfo
        Get
            Return New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
        End Get
    End Property

    Public ReadOnly Property MSLBackupsFolder() As DirectoryInfo
        Get
            Return New DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MSL", "Backups"))
        End Get
    End Property

    Public ReadOnly Property MSLCommandsFolder() As DirectoryInfo
        Get
            Return New DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MSL", "Commands"))
        End Get
    End Property

    Public ReadOnly Property MSLSettings() As FileInfo
        Get
            Return New FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MSL", "Settings.xml"))
        End Get
    End Property

    Public ReadOnly Property MSLLibraries() As DirectoryInfo
        Get
            Return New DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MSL", "libraries"))
        End Get
    End Property

    Public ReadOnly Property MSLItemDatabase() As FileInfo
        Get
            Return New FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MSL", "items", "ItemDB.xml"))
        End Get
    End Property
End Class
