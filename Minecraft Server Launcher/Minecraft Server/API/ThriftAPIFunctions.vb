Imports org.phybros.thrift

Public Class ThriftAPIFunctions
    Private client As SwiftApi.Client
    Private GetAuthenticationString As GetAuthStringDelegate
    Public Delegate Function GetAuthStringDelegate(MethodeName As String) As String
    Public Event AnErrorOccurred(sender As Object, e As Thrift.TApplicationException)

    Public Sub New(client As SwiftApi.Client, authstring As GetAuthStringDelegate)
        Me.client = client
        GetAuthenticationString = authstring
    End Sub

#Region "Functions"
    Public Function AddItemToInventory(playername As String, item As ItemStack) As Boolean
        Try
            Return client.addItemToInventory(GetAuthenticationString("addItemToInventory"), playername, item)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function AddToWhitelist(playername As String) As Boolean
        Try
            Return client.addToWhitelist(GetAuthenticationString("addToWhitelist"), playername)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function Announce(message As String) As Boolean
        Try
            Return client.announce(GetAuthenticationString("announce"), message)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function Ban(playername As String) As Boolean
        Try
            Return client.ban(GetAuthenticationString("ban"), playername)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function BanIP(ip As String) As Boolean
        Try
            Return client.banIp(GetAuthenticationString("banIp"), ip)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function DeOP(playername As String, NotifyPlayer As Boolean) As Boolean
        Try
            Return client.deOp(GetAuthenticationString("deOp"), playername, NotifyPlayer)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function GetBannedIPs() As List(Of String)
        Try
            Return client.getBannedIps(GetAuthenticationString("getBannedIps"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetBannedPlayers() As List(Of OfflinePlayer)
        Try
            Return client.getBannedPlayers(GetAuthenticationString("getBannedPlayers"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetBukkitVersion() As String
        Try
            Return client.getBukkitVersion(GetAuthenticationString("getBukkitVersion"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return String.Empty
    End Function

    Public Function GetOfflinePlayer(playername As String) As OfflinePlayer
        Try
            Return client.getOfflinePlayer(GetAuthenticationString("getOfflinePlayer"), playername)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetOfflinePlayers() As List(Of OfflinePlayer)
        Try
            Return client.getOfflinePlayers(GetAuthenticationString("getOfflinePlayers"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetOPs() As List(Of OfflinePlayer)
        Try
            Return client.getOps(GetAuthenticationString("getOps"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetPlayer(playername As String) As Player
        Try
            Return client.getPlayer(GetAuthenticationString("getPlayer"), playername)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetPlayers(playername As String) As List(Of Player)
        Try
            Return client.getPlayers(GetAuthenticationString("getPlayers"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetPlugin(name As String) As Plugin
        Try
            Return client.getPlugin(GetAuthenticationString("getPlugin"), name)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetPlugins() As List(Of Plugin)
        Try
            Return client.getPlugins(GetAuthenticationString("getPlugins"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetServer() As Server
        Try
            Return client.getServer(GetAuthenticationString("getServer"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetServerVersion() As String
        Try
            Return client.getServerVersion(GetAuthenticationString("getServerVersion"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetWhitelist() As List(Of OfflinePlayer)
        Try
            Return client.getWhitelist(GetAuthenticationString("getWhitelist"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetWorld(worldName As String) As World
        Try
            Return client.getWorld(GetAuthenticationString("getWorld"), worldName)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function GetWorlds(worldName As String) As List(Of World)
        Try
            Return client.getWorlds(GetAuthenticationString("getWorlds"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return Nothing
    End Function

    Public Function InstallPlugin(downloadurl As String, md5 As String) As Boolean
        Try
            Return client.installPlugin(GetAuthenticationString("installPlugin"), downloadurl, md5)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function Kick(playername As String, message As String) As Boolean
        Try
            Return client.kick(GetAuthenticationString("kick"), playername, message)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function OP(playername As String, notifyPlayer As Boolean) As Boolean
        Try
            Return client.op(GetAuthenticationString("op"), playername, notifyPlayer)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function Ping() As Boolean
        Try
            Return client.ping(GetAuthenticationString("ping"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Sub ReloadServer()
        Try
            client.reloadServer(GetAuthenticationString("reloadServer"))
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
    End Sub

    Public Function RemoveInventoryItem(playername As String, itemindex As Integer) As Boolean
        Try
            Return client.removeInventoryItem(GetAuthenticationString("removeInventoryItem"), playername, itemindex)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function RemoveFromWhitelist(playername As String) As Boolean
        Try
            Return client.removeFromWhitelist(GetAuthenticationString("removeFromWhitelist"), playername)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function ReplacePlugin(pluginName As String, DownloadURL As String, md5 As String) As Boolean
        Try
            Return client.replacePlugin(GetAuthenticationString("replacePlugin"), pluginName, DownloadURL, md5)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Sub runConsoleCommand(command As String)
        Try
            client.runConsoleCommand(GetAuthenticationString("runConsoleCommand"), command)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
    End Sub

    Public Function SaveWorld(worldname As String) As Boolean
        Try
            Return client.saveWorld(GetAuthenticationString("saveWorld"), worldname)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function setFileContents(filename As String, fileContents As String) As Boolean
        Try
            Return client.setFileContents(GetAuthenticationString("setFileContents"), filename, fileContents)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function SetGameMode(playername As String, mode As GameMode) As Boolean
        Try
            Return client.setGameMode(GetAuthenticationString("setGameMode"), playername, mode)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function SetPVP(worldName As String, IsPVP As Boolean) As Boolean
        Try
            Return client.setPvp(GetAuthenticationString("setPvp"), worldName, IsPVP)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function SetStorm(worldName As String, hasStorm As Boolean) As Boolean
        Try
            Return client.setStorm(GetAuthenticationString("setStorm"), worldName, hasStorm)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function SetThundering(worldName As String, isThundering As Boolean) As Boolean
        Try
            Return client.setThundering(GetAuthenticationString("setThundering"), worldName, isThundering)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function SetWorldTime(worldName As String, time As Long) As Boolean
        Try
            Return client.setWorldTime(GetAuthenticationString("setWorldTime"), worldName, time)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function UnBan(playername As String) As Boolean
        Try
            Return client.unBan(GetAuthenticationString("unBan"), playername)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function UnBanIP(IP As String) As Boolean
        Try
            Return client.unBanIp(GetAuthenticationString("unBanIp"), IP)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function

    Public Function UpdateInventoryItem(playername As String, item As ItemStack, itemIndex As Integer) As Boolean
        Try
            Return client.updateInventoryItem(GetAuthenticationString("updateInventoryItem"), playername, item, itemIndex)
        Catch ex As Thrift.TApplicationException
            RaiseEvent AnErrorOccurred(Me, ex)
        End Try
        Return False
    End Function
#End Region
End Class
