Imports org.phybros.thrift
Imports Exceptionless

Public Class frmTest
    Implements ComponentModel.INotifyPropertyChanged

    Private _lst As PlayerInventory
    Public Property List As PlayerInventory
        Get
            Return _lst
        End Get
        Set(value As PlayerInventory)
            _lst = value
            RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs("List"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim newInventory = New PlayerInventory
        newInventory.Inventory = New List(Of ItemStack) From {New ItemStack With {.Amount = 64, .TypeId = 5}, New ItemStack With {.Amount = 32, .TypeId = 12}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}}
        newInventory.Armor = New PlayerArmor With {.Boots = New ItemStack With {.Amount = 0, .TypeId = 0}, .Chestplate = New ItemStack With {.Amount = 0, .TypeId = 311}, .Helmet = New ItemStack With {.Amount = 64, .TypeId = 314}, .Leggings = New ItemStack With {.Amount = 64, .TypeId = 308}}
        Me.List = newInventory
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim backup As New BackupManager("D:\Dokumente\Visual Studio 2013\Projects\Minecraft Server Launcher\Source\MahApps.Metro\Minecraft Server Launcher\bin\Debug\MSL\Backups")
        AddHandler backup.ProgressChanged, Sub()
                                               Me.test.Value = backup.Progress
                                           End Sub
        Dim files As New List(Of IO.FileInfo)
        Dim dirs As New List(Of IO.DirectoryInfo)
        files.Add(New IO.FileInfo("D:\Dokumente\Visual Studio 2013\Projects\Minecraft Server Launcher\Source\MahApps.Metro\Minecraft Server Launcher\bin\Debug\server.properties"))
        files.Add(New IO.FileInfo("D:\Dokumente\Visual Studio 2013\Projects\Minecraft Server Launcher\Source\MahApps.Metro\Minecraft Server Launcher\bin\Debug\help.yml"))
        dirs.Add(New IO.DirectoryInfo("D:\Dokumente\Visual Studio 2013\Projects\Minecraft Server Launcher\Source\MahApps.Metro\Minecraft Server Launcher\bin\Debug\world"))
        dirs.Add(New IO.DirectoryInfo("D:\Dokumente\Visual Studio 2013\Projects\Minecraft Server Launcher\Source\MahApps.Metro\Minecraft Server Launcher\bin\Debug\world_nether"))
        dirs.Add(New IO.DirectoryInfo("D:\Dokumente\Visual Studio 2013\Projects\Minecraft Server Launcher\Source\MahApps.Metro\Minecraft Server Launcher\bin\Debug\world_the_end"))
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Dim backup As New BackupManager("D:\Dokumente\Visual Studio 2013\Projects\Minecraft Server Launcher\Source\MahApps.Metro\Minecraft Server Launcher\bin\Debug\MSL\Backups")
        AddHandler backup.ProgressChanged, Sub()
                                               Me.test.Value = backup.Progress
                                           End Sub
        backup.LoadBackups()
        If backup.BackupList.Count > 0 Then
            Dim b = backup.BackupList(0)
            backup.RestoreBackup(b, b.BackupFiles, "D:\Dokumente\Visual Studio 2013\Projects\Minecraft Server Launcher\Source\MahApps.Metro\Minecraft Server Launcher\bin\Debug\MSL\Backups\Test")
        End If
    End Sub
End Class
