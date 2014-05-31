Imports org.phybros.thrift

Public Class ThriftAPIFunctions
    Private client As SwiftApi.Client
    Private GetAuthenticationString As GetAuthStringDelegate
    Public Delegate Function GetAuthStringDelegate(MethodeName As String) As String

    Public Sub New(client As SwiftApi.Client, authstring As GetAuthStringDelegate)
        Me.client = client
        GetAuthenticationString = authstring
    End Sub

#Region "Functions"
    Public Function AddItemToInventory(playername As String, item As ItemStack) As Boolean
        Return client.addItemToInventory(GetAuthenticationString("addItemToInventory"), playername, item)
    End Function

    Public Function AddToWhitelist(playername As String) As Boolean
        Return client.addToWhitelist(GetAuthenticationString("addToWhitelist"), playername)
    End Function

    Public Function Announce(message As String) As Boolean
        Return client.announce(GetAuthenticationString("announce"), message)
    End Function

    Public Function Ban(playername As String) As Boolean
        Return client.ban(GetAuthenticationString("ban"), playername)
    End Function

    Public Function BanIP(ip As String) As Boolean
        Return client.banIp(GetAuthenticationString("banIp"), ip)
    End Function

    Public Function DeOP(playername As String, NotifyPlayer As Boolean) As Boolean
        Return client.deOp(GetAuthenticationString("deOp"), playername, NotifyPlayer)
    End Function

    Public Function GetBannedIPs() As List(Of String)
        Return client.getBannedIps(GetAuthenticationString("getBannedIps"))
    End Function

    Public Function GetBannedPlayers() As List(Of OfflinePlayer)
        Return client.getBannedPlayers(GetAuthenticationString("getBannedPlayers"))
    End Function

    Public Function GetBukkitVersion() As String
        Return client.getBukkitVersion(GetAuthenticationString("getBukkitVersion"))
    End Function

    Public Function GetOfflinePlayer(playername As String) As OfflinePlayer
        Return client.getOfflinePlayer(GetAuthenticationString("getOfflinePlayer"), playername)
    End Function

    Public Function GetOfflinePlayers() As List(Of OfflinePlayer)
        Return client.getOfflinePlayers(GetAuthenticationString("getOfflinePlayers"))
    End Function

    Public Function GetOPs() As List(Of OfflinePlayer)
        Return client.getOps(GetAuthenticationString("getOps"))
    End Function

    Public Function GetPlayer(playername As String) As Player
        Return client.getPlayer(GetAuthenticationString("getPlayer"), playername)
    End Function

    Public Function GetPlayers(playername As String) As List(Of Player)
        Return client.getPlayers(GetAuthenticationString("getPlayers"))
    End Function

    Public Function GetPlugin(name As String) As Plugin
        Return client.getPlugin(GetAuthenticationString("getPlugin"), name)
    End Function

    Public Function GetPlugins() As List(Of Plugin)
        Return client.getPlugins(GetAuthenticationString("getPlugins"))
    End Function

    Public Function GetServer() As Server
        Return client.getServer(GetAuthenticationString("getServer"))
    End Function

    Public Function GetServerVersion() As String
        Return client.getServerVersion(GetAuthenticationString("getServerVersion"))
    End Function

    Public Function GetWhitelist() As List(Of OfflinePlayer)
        Return client.getWhitelist(GetAuthenticationString("getWhitelist"))
    End Function

    Public Function GetWorld(worldName As String) As World
        Return client.getWorld(GetAuthenticationString("getWorld"), worldName)
    End Function

    Public Function GetWorlds(worldName As String) As List(Of World)
        Return client.getWorlds(GetAuthenticationString("getWorlds"))
    End Function

    Public Function InstallPlugin(downloadurl As String, md5 As String) As Boolean
        Return client.installPlugin(GetAuthenticationString("installPlugin"), downloadurl, md5)
    End Function

    Public Function Kick(playername As String, message As String) As Boolean
        Return client.kick(GetAuthenticationString("kick"), playername, message)
    End Function

    Public Function OP(playername As String, notifyPlayer As Boolean) As Boolean
        Return client.op(GetAuthenticationString("op"), playername, notifyPlayer)
    End Function

    Public Function Ping() As Boolean
        Return client.ping(GetAuthenticationString("ping"))
    End Function

    Public Sub ReloadServer()
        client.reloadServer(GetAuthenticationString("reloadServer"))
    End Sub

    Public Function RemoveInventoryItem(playername As String, itemindex As Integer) As Boolean
        Return client.removeInventoryItem(GetAuthenticationString("removeInventoryItem"), playername, itemindex)
    End Function

    Public Function RemoveFromWhitelist(playername As String) As Boolean
        Return client.removeFromWhitelist(GetAuthenticationString("removeFromWhitelist"), playername)
    End Function

    Public Function ReplacePlugin(pluginName As String, DownloadURL As String, md5 As String) As Boolean
        Return client.replacePlugin(GetAuthenticationString("replacePlugin"), pluginName, DownloadURL, md5)
    End Function

    Public Sub runConsoleCommand(command As String)
        client.runConsoleCommand(GetAuthenticationString("runConsoleCommand"), command)
    End Sub

    Public Function SaveWorld(worldname As String) As Boolean
        Return client.saveWorld(GetAuthenticationString("saveWorld"), worldname)
    End Function

    Public Function setFileContents(filename As String, fileContents As String) As Boolean
        Return client.setFileContents(GetAuthenticationString("setFileContents"), filename, fileContents)
    End Function

    Public Function SetGameMode(playername As String, mode As GameMode) As Boolean
        Return client.setGameMode(GetAuthenticationString("setGameMode"), playername, mode)
    End Function

    Public Function SetPVP(worldName As String, IsPVP As Boolean) As Boolean
        Return client.setPvp(GetAuthenticationString("setPvp"), worldName, IsPVP)
    End Function

    Public Function SetStorm(worldName As String, hasStorm As Boolean) As Boolean
        Return client.setStorm(GetAuthenticationString("setStorm"), worldName, hasStorm)
    End Function

    Public Function SetThundering(worldName As String, isThundering As Boolean) As Boolean
        Return client.setThundering(GetAuthenticationString("setThundering"), worldName, isThundering)
    End Function

    Public Function SetWorldTime(worldName As String, time As Long) As Boolean
        Return client.setWorldTime(GetAuthenticationString("setWorldTime"), worldName, time)
    End Function

    Public Function UnBan(playername As String) As Boolean
        Return client.unBan(GetAuthenticationString("unBan"), playername)
    End Function

    Public Function UnBanIP(IP As String) As Boolean
        Return client.unBanIp(GetAuthenticationString("unBanIp"), IP)
    End Function

    Public Function UpdateInventoryItem(playername As String, item As ItemStack, itemIndex As Integer) As Boolean
        Return client.updateInventoryItem(GetAuthenticationString("updateInventoryItem"), playername, item, itemIndex)
    End Function
#End Region
End Class
