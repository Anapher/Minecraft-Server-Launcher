Public Class MainViewModel
    Inherits PropertyChangedBase

#Region "Singleton & Constructor"
    Private Shared _Instance As MainViewModel
    Public Shared ReadOnly Property Instance As MainViewModel
        Get
            If _Instance Is Nothing Then _Instance = New MainViewModel
            Return _Instance
        End Get
    End Property

    Public Sub New()
        MinecraftServer = New MinecraftServer(AddressOf GetJavaPath)
        Dim t As New System.Threading.Thread(Sub()
                                                 MinecraftServer.StartServer()
                                             End Sub)
        t.IsBackground = True
        t.Start()
        AddHandler MinecraftServer.BackupManager.StatusChanged, Sub(sender As Object, e As StateChangedEventArgs)
                                                                    Me.BackupStatus = e.NewLine
                                                                End Sub
        AddHandler MinecraftServer.DynmapEnabled, Sub(sender As Object, e As EventArgs)
                                                      If DynmapRefresh IsNot Nothing Then DynmapRefresh.Invoke(Me, EventArgs.Empty)
                                                  End Sub
        AddHandler MinecraftServer.SwiftAPI.GeneratedInformationsComplete, Sub(sender As Object, e As EventArgs)
                                                                               Dim frm As New frmInfoBox(String.Format(Application.Current.FindResource("infoSwiftInformationsGenerted").ToString(), Environment.NewLine, MinecraftServer.SwiftAPI.Username, MinecraftServer.SwiftAPI.Password, MinecraftServer.SwiftAPI.Salt), Application.Current.FindResource("successful").ToString(), Application.Current.FindResource("OK").ToString()) With {.Owner = myWindow}
                                                                               frm.ShowDialog()
                                                                           End Sub
        AddHandler MinecraftServer.LauncherUpdatesFound, Sub(sender As Object, e As EventArgs)
                                                             Application.Current.Dispatcher.Invoke(Sub()
                                                                                                       Dim frm = New frmUpdate(MinecraftServer.Updater) With {.Owner = Application.Current.MainWindow}
                                                                                                       frm.ShowDialog()
                                                                                                   End Sub)
                                                         End Sub
    End Sub

    Private Function GetJavaPath() As String
        Dim frm As frmGetJavaPath = Nothing
        Dim dialogresult As Boolean = False
        Application.Current.Dispatcher.Invoke(Sub()
                                                  frm = New frmGetJavaPath() With {.Owner = myWindow}
                                                  If frm.ShowDialog() Then dialogresult = True
                                              End Sub)
        If dialogresult Then
            Return frm.JavaPath
        Else
            Application.Current.Dispatcher.Invoke(Sub() Application.Current.Shutdown())
            Return Nothing
        End If
    End Function

    Private _DynmapRefresh As EventHandler
    Public Property DynmapRefresh() As EventHandler
        Get
            Return _DynmapRefresh
        End Get
        Set(ByVal value As EventHandler)
            _DynmapRefresh = value
        End Set
    End Property
#End Region

#Region "Events"

    Private _IsClosing As Boolean = False
    Public Property IsClosing() As Boolean
        Get
            Return _IsClosing
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _IsClosing)
        End Set
    End Property

    Public Function OnClosing() As Boolean
        If MinecraftServer IsNot Nothing Then
            If MinecraftServer.LauncherSettings.StopServerOnClose AndAlso MinecraftServer.IsRunning Then
                If IsClosing Then Return True
                IsClosing = True
                MinecraftServer.ExecuteCommand("stop")
                Dim t As New System.Threading.Thread(Sub()
                                                         Dim counter As Integer = 0
                                                         Dim maxvalue As Integer = 2000
                                                         While MinecraftServer.IsRunning AndAlso counter < maxvalue
                                                             System.Threading.Thread.Sleep(50)
                                                             counter += 1
                                                         End While
                                                         If MinecraftServer.IsRunning Then MinecraftServer.StopServer()
                                                         Application.Current.Dispatcher.Invoke(Sub() Application.Current.Shutdown())
                                                     End Sub)
                t.IsBackground = True
                t.Start()
                Return True
            Else
                MinecraftServer.StopServer()
            End If
        End If
        Return False
    End Function

    Private Sub _MinecraftServer_BannedListChanged(sender As Object, e As EventArgs) Handles _MinecraftServer.BannedListChanged
        OnPropertyChanged("NoBansTextVisibility")
    End Sub

    Private Sub _MinecraftServer_PlayerChanged(sender As Object, e As EventArgs) Handles _MinecraftServer.PlayerChanged
        OnPropertyChanged("NoPlayersFound")
    End Sub

    Private Sub _MinecraftServer_ServerChanged(sender As Object, e As EventArgs) Handles _MinecraftServer.ServerChanged
        OnPropertyChanged("IsOP")
        OnPropertyChanged("CBBGamemodeIndex")
        OnPropertyChanged("lstPlayersWithoutSelected")
        OnPropertyChanged("TpIsAvailable")
    End Sub

#If Not Debug Then
    Private Sub _MinecraftServer_StartFileNotFound(sender As Object, e As EventArgs) Handles _MinecraftServer.StartFileNotFound
        Application.Current.Dispatcher.BeginInvoke(Sub()
                                                       Dim frmMSG As New frmMessageBox(Application.Current.FindResource("MinecraftServerFileIsMissing").ToString(), Application.Current.FindResource("Exception").ToString(), Application.Current.FindResource("DownloadMinecraftServer").ToString(), Application.Current.FindResource("CloseProgram").ToString()) With {.Owner = myWindow, .Width = 450}
                                                       If frmMSG.ShowDialog() Then
                                                           Dim frm As New frmDownloadDownloadMinecraftServer
                                                           frm.Show()
                                                       End If
                                                       Application.Current.MainWindow.Close()
                                                   End Sub)
    End Sub
#End If

    Private Sub _MinecraftServer_StateChanged(sender As Object, e As StateChangedEventArgs) Handles _MinecraftServer.StateChanged
        ConsoleText &= e.NewLine & Environment.NewLine
    End Sub
#End Region

#Region "Properties"
#Region "General"
    Private _MainTabControlIndex As Integer
    Public Property MainTabControlIndex() As Integer
        Get
            Return _MainTabControlIndex
        End Get
        Set(ByVal value As Integer)
            If _TabControlSettingsIndex = 5 AndAlso MinecraftServer.ServerSettings.HasChanged Then
                CheckIfSettingsSave()
            End If
            SetProperty(value, _MainTabControlIndex)
        End Set
    End Property

    Private WithEvents _MinecraftServer As MinecraftServer
    Public Property MinecraftServer As MinecraftServer
        Get
            Return _MinecraftServer
        End Get
        Set(value As MinecraftServer)
            SetProperty(value, _MinecraftServer)
        End Set
    End Property

    Public ReadOnly Property myWindow As Window
        Get
            Return Application.Current.MainWindow
        End Get
    End Property
#End Region

#Region "Console"
    Private _ConsoleText As String
    Public Property ConsoleText As String
        Get
            Return _ConsoleText
        End Get
        Set(value As String)
            SetProperty(value, _ConsoleText)
        End Set
    End Property

    Private _txtCommand As String
    Public Property txtCommand() As String
        Get
            Return _txtCommand
        End Get
        Set(ByVal value As String)
            SetProperty(value, _txtCommand)
        End Set
    End Property
#End Region

#Region "Player"
    Public ReadOnly Property GridPlayerIsVisible As Visibility
        Get
            If lstPlayerIndex > -1 Then Return Visibility.Visible Else Return Visibility.Hidden
        End Get
    End Property

    Public ReadOnly Property NoPlayersFound As Boolean
        Get
            Return MinecraftServer.lstPlayers.Count = 0
        End Get
    End Property

    Private _lstPlayerIndex As Integer = -1
    Public Property lstPlayerIndex() As Integer
        Get
            Return _lstPlayerIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _lstPlayerIndex)
            OnPropertyChanged("lstPlayersWithoutSelected")
            OnPropertyChanged("IsOP")
            OnPropertyChanged("CBBGamemodeIndex")
            OnPropertyChanged("GridPlayerIsVisible")
            OnPropertyChanged("CurrentInventory")
        End Set
    End Property


    Public ReadOnly Property CurrentInventory As org.phybros.thrift.PlayerInventory
        Get
            If lstPlayerIndex > -1 AndAlso Not MinecraftServer.lstPlayers.Count < lstPlayerIndex + 1 Then
                Return MinecraftServer.Server.OnlinePlayers(lstPlayerIndex).Inventory
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property lstPlayersWithoutSelected() As List(Of org.phybros.thrift.Player)
        Get
            Dim lst As New List(Of org.phybros.thrift.Player)
            For i = 0 To MinecraftServer.lstPlayers.Count - 1
                If Not i = lstPlayerIndex Then
                    lst.Add(MinecraftServer.lstPlayers(i))
                End If
            Next
            Return lst
        End Get
    End Property

    Private _SelectedItem As MinecraftItem
    Public Property SelectedItem() As MinecraftItem
        Get
            Return _SelectedItem
        End Get
        Set(ByVal value As MinecraftItem)
            SetProperty(value, _SelectedItem)
        End Set
    End Property

    Private _ItemAmount As Double = 1
    Public Property ItemAmount() As Double
        Get
            Return _ItemAmount
        End Get
        Set(ByVal value As Double)
            SetProperty(value, _ItemAmount)
        End Set
    End Property

    Public Property IsOP As Boolean
        Get
            If lstPlayerIndex = -1 Then Return False
            If MinecraftServer.lstPlayers.Count < lstPlayerIndex + 1 Then
                Return False
            End If
            Return MinecraftServer.Server.OnlinePlayers(lstPlayerIndex).IsOp
        End Get
        Set(value As Boolean)
            If Not value = MinecraftServer.Server.OnlinePlayers(lstPlayerIndex).IsOp Then
                If value Then
                    MinecraftServer.ThriftAPI.Functions.OP(MinecraftServer.Server.OnlinePlayers(lstPlayerIndex).Name, True)
                Else
                    MinecraftServer.ThriftAPI.Functions.DeOP(MinecraftServer.Server.OnlinePlayers(lstPlayerIndex).Name, True)
                End If
            End If
        End Set
    End Property

    Public Property CBBGamemodeIndex() As Integer
        Get
            If lstPlayerIndex = -1 Then Return 0
            If MinecraftServer.lstPlayers.Count < lstPlayerIndex + 1 Then
                Return 0
            End If
            Return MinecraftServer.Server.OnlinePlayers(lstPlayerIndex).Gamemode
        End Get
        Set(ByVal value As Integer)
            Dim player = MinecraftServer.Server.OnlinePlayers(lstPlayerIndex)
            If Not value = player.Gamemode Then
                Select Case value
                    Case 0
                        MinecraftServer.ThriftAPI.Functions.SetGameMode(player.Name, org.phybros.thrift.GameMode.SURVIVAL)
                    Case 1
                        MinecraftServer.ThriftAPI.Functions.SetGameMode(player.Name, org.phybros.thrift.GameMode.CREATIVE)
                    Case 2
                        MinecraftServer.ThriftAPI.Functions.SetGameMode(player.Name, org.phybros.thrift.GameMode.ADVENTURE)
                End Select
            End If
        End Set
    End Property

    Private _txtSendMessage As String
    Public Property txtSendMessage() As String
        Get
            Return _txtSendMessage
        End Get
        Set(ByVal value As String)
            SetProperty(value, _txtSendMessage)
        End Set
    End Property

    Private _CbbTpPlayer As Integer
    Public Property CBBTpPlayer() As Integer
        Get
            Return _CbbTpPlayer
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _CbbTpPlayer)
        End Set
    End Property

    Public ReadOnly Property TpIsAvailable As Boolean
        Get
            Return lstPlayersWithoutSelected.Count > 0
        End Get
    End Property

    Private _cbbEffectIndex As Integer = 0
    Public Property CBBEffectIndex() As Integer
        Get
            Return _cbbEffectIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _cbbEffectIndex)
        End Set
    End Property

    Private _nudDuration As Integer
    Public Property NudDuration() As Integer
        Get
            Return _nudDuration
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _nudDuration)
        End Set
    End Property

    Private _cbbxptypeIndex As Integer = 0
    Public Property CBBXpTypeIndex() As Integer
        Get
            Return _cbbxptypeIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _cbbxptypeIndex)
        End Set
    End Property

    Private _cbbxpIsEnabled As Boolean
    Public Property CBBXPIsEnabled() As Boolean
        Get
            Return _cbbxpIsEnabled
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _cbbxpIsEnabled)
        End Set
    End Property

    Private _nudxpamount As Integer
    Public Property NUDXpAmount() As Integer
        Get
            Return _nudxpamount
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                CBBXpTypeIndex = 1
                CBBXPIsEnabled = False
            Else
                CBBXPIsEnabled = True
            End If
            SetProperty(value, _nudxpamount)
        End Set
    End Property

    Private _ItemToChange As org.phybros.thrift.ItemStack
    Public Property ItemToChange() As org.phybros.thrift.ItemStack
        Get
            Return _ItemToChange
        End Get
        Set(ByVal value As org.phybros.thrift.ItemStack)
            If value IsNot Nothing AndAlso _ItemToChange IsNot Nothing AndAlso value.Amount <> _ItemToChange.Amount Then
                NudItemAmount = value.Amount
            End If
            SetProperty(value, _ItemToChange)
            If NewSelectedItem IsNot Nothing Then
                Dim valueconverter = New ItemToTextConverter
                Dim newvalue = valueconverter.Convert(NewSelectedItem, Nothing, Nothing, Nothing)
                If newvalue Is Nothing Then CurrentItemText = String.Empty Else CurrentItemText = newvalue.ToString()
            End If
        End Set
    End Property

    Private _NewSelectedItem As org.phybros.thrift.ItemStack
    Public Property NewSelectedItem() As org.phybros.thrift.ItemStack
        Get
            If _NewSelectedItem Is Nothing Then Return ItemToChange
            Return _NewSelectedItem
        End Get
        Set(ByVal value As org.phybros.thrift.ItemStack)
            SetProperty(value, _NewSelectedItem)
        End Set
    End Property

    Private _ItemToChangeIndex As Integer
    Public Property ItemToChangeIndex() As Integer
        Get
            Return _ItemToChangeIndex
        End Get
        Set(ByVal value As Integer)
            If value > -1 AndAlso _ItemToChangeIndex > -1 AndAlso value <> _ItemToChangeIndex Then NewSelectedItem = Nothing
            SetProperty(value, _ItemToChangeIndex)
        End Set
    End Property

    Private _NudItemAmount As Integer
    Public Property NudItemAmount() As Integer
        Get
            Return _NudItemAmount
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _NudItemAmount)
        End Set
    End Property

    Private _CurrentItemText As String
    Public Property CurrentItemText() As String
        Get
            Return _CurrentItemText
        End Get
        Set(ByVal value As String)
            SetProperty(value, _CurrentItemText)
        End Set
    End Property
#End Region

#Region "Server"
#Region "Informations"
    Private _cbbweatherindex As Integer = 0
    Public Property CBBWeatherIndex() As Integer
        Get
            Return _cbbweatherindex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _cbbweatherindex)
        End Set
    End Property

    Private _txtAnnounce As String
    Public Property txtAnnounce() As String
        Get
            Return _txtAnnounce
        End Get
        Set(ByVal value As String)
            SetProperty(value, _txtAnnounce)
        End Set
    End Property
#End Region

#Region "Ban list"
    Private _UnBanPlayer As RelayCommand
    Public ReadOnly Property UnBanPlayer As RelayCommand
        Get
            If _UnBanPlayer Is Nothing Then _UnBanPlayer = New RelayCommand(Sub(parameter)
                                                                                If parameter IsNot Nothing Then
                                                                                    Dim lstBanItem = DirectCast(parameter, BannedPlayer)
                                                                                    If lstBanItem.IPIsBanned Then
                                                                                        MinecraftServer.ExecuteCommand(String.Format("pardon-ip {0}", lstBanItem.ip))
                                                                                    Else
                                                                                        MinecraftServer.ExecuteCommand(String.Format("pardon {0}", lstBanItem.name))
                                                                                    End If
                                                                                End If
                                                                            End Sub)
            Return _UnBanPlayer
        End Get
    End Property
#End Region

#Region "Whitelist"
    Private _AddPlayerToWhitelist As RelayCommand
    Public ReadOnly Property AddPlayerToWhitelist As RelayCommand
        Get
            If _AddPlayerToWhitelist Is Nothing Then _AddPlayerToWhitelist = New RelayCommand(Sub(parameter As Object)
                                                                                                  If parameter IsNot Nothing Then
                                                                                                      Dim txt = DirectCast(parameter, TextBox)
                                                                                                      If Not String.IsNullOrWhiteSpace(txt.Text) Then
                                                                                                          MinecraftServer.ThriftAPI.Functions.AddToWhitelist(txt.Text)
                                                                                                          txt.Text = String.Empty
                                                                                                      End If
                                                                                                  End If
                                                                                              End Sub)
            Return _AddPlayerToWhitelist
        End Get
    End Property

    Private _RemovePlayerFromWhitelist As RelayCommand
    Public ReadOnly Property RemovePlayerFromWhitelist As RelayCommand
        Get
            If _RemovePlayerFromWhitelist Is Nothing Then _RemovePlayerFromWhitelist = New RelayCommand(Sub(parameter As Object)
                                                                                                            If parameter IsNot Nothing Then
                                                                                                                Dim wplayer = DirectCast(parameter, WhitelistedPlayer)
                                                                                                                MinecraftServer.ThriftAPI.Functions.RemoveFromWhitelist(wplayer.name)
                                                                                                            End If
                                                                                                        End Sub)
            Return _RemovePlayerFromWhitelist
        End Get
    End Property
#End Region

#Region "Settings"
    Private _TabControlSettingsIndex As Integer
    Public Property TabControlSettingsIndex() As Integer
        Get
            Return _TabControlSettingsIndex
        End Get
        Set(ByVal value As Integer)
            If _TabControlSettingsIndex = 5 AndAlso value <> 5 AndAlso MinecraftServer.ServerSettings.HasChanged Then
                CheckIfSettingsSave()
            End If
            SetProperty(value, _TabControlSettingsIndex)
        End Set
    End Property

    Private _TimerName As String
    Public Property TimerName() As String
        Get
            Return _TimerName
        End Get
        Set(ByVal value As String)
            SetProperty(value, _TimerName)
            CheckCanTimerAdd()
        End Set
    End Property

    Private _TimerCommand As String
    Public Property TimerCommand() As String
        Get
            Return _TimerCommand
        End Get
        Set(ByVal value As String)
            SetProperty(value, _TimerCommand)
            CheckCanTimerAdd()
        End Set
    End Property

    Private _TimerInterval As Double = 1
    Public Property TimerInterval() As Double
        Get
            Return _TimerInterval
        End Get
        Set(ByVal value As Double)
            SetProperty(value, _TimerInterval)
            CheckCanTimerAdd()
        End Set
    End Property

    Private _CanAddTimer As Boolean = False
    Public Property CanAddTimer() As Boolean
        Get
            Return _CanAddTimer
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _CanAddTimer)
        End Set
    End Property

#End Region

#Region "Plugins"
    Private _lstPluginsIndex As Integer = -1
    Public Property lstPluginsIndex() As Integer
        Get
            Return _lstPluginsIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _lstPluginsIndex)
            OnPropertyChanged("GridPluginsIsVisibility")
        End Set
    End Property

    Public ReadOnly Property GridPluginsIsVisibility As Visibility
        Get
            If lstPluginsIndex > -1 Then Return Visibility.Visible Else Return Visibility.Hidden
        End Get
    End Property
#End Region
#End Region

#Region "LauncherSettings"
    Private _BackupSelectedIndex As Integer
    Public Property BackupSelectedIndex() As Integer
        Get
            Return _BackupSelectedIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _BackupSelectedIndex)
        End Set
    End Property

    Private _BackupIsWorking As Boolean
    Public Property BackupIsWorking() As Boolean
        Get
            Return _BackupIsWorking
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _BackupIsWorking)
        End Set
    End Property

    Private _BackupIsNotBusy As Boolean = True
    Public Property BackupIsNotBusy() As Boolean
        Get
            Return _BackupIsNotBusy
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _BackupIsNotBusy)
        End Set
    End Property

    Private _BackupStatus As String
    Public Property BackupStatus() As String
        Get
            Return _BackupStatus
        End Get
        Set(ByVal value As String)
            SetProperty(value, _BackupStatus)
        End Set
    End Property

    Private _SelectedTimer As TimerBase
    Public Property SelectedTimer() As TimerBase
        Get
            Return _SelectedTimer
        End Get
        Set(ByVal value As TimerBase)
            SetProperty(value, _SelectedTimer)
        End Set
    End Property

#End Region
#End Region

#Region "Methods"
    Private Sub CheckIfSettingsSave()
        MinecraftServer.ServerSettings.HasChanged = False
        If MinecraftServer.LauncherSettings.AskSaveSettings Then
            Dim frmMSG As New frmMessageBox(Application.Current.FindResource("SSText").ToString(), Application.Current.FindResource("SSTitle").ToString(), Application.Current.FindResource("SSOK").ToString(), Application.Current.FindResource("SSCancel").ToString()) With {.Owner = myWindow}
            If Not frmMSG.ShowDialog() Then
                MinecraftServer.ServerSettings.Load()
                Return
            End If
        End If
        MinecraftServer.ServerSettings.Save()
        MinecraftServer.ServerSettings.Load()
    End Sub

    Private Sub CheckCanTimerAdd()
        If TimerInterval > 0 AndAlso Not String.IsNullOrWhiteSpace(TimerCommand) AndAlso Not String.IsNullOrWhiteSpace(TimerName) Then
            CanAddTimer = True
        Else
            CanAddTimer = False
        End If
    End Sub

    Private Sub RestoreFinished(sender As Object, e As EventArgs)
        Me.ConsoleText &= "=============================== Restarting Server ===============================" & Environment.NewLine
        If Not MinecraftServer.IsRunning Then MinecraftServer.StartServer()
        BackupIsNotBusy = True
        BackupStatus = String.Empty
    End Sub

    Private Sub RestoreBackup()
        Dim backup = MinecraftServer.BackupManager.BackupList(BackupSelectedIndex)
        If backup.SomethingIsChecked Then
            If IO.File.Exists(backup.BackupPath.FullName) Then
                Dim frm As New frmMessageBox(Application.Current.FindResource("ServerHaveToStopText").ToString(), Application.Current.FindResource("ServerHaveToStopTitle").ToString(), Application.Current.FindResource("StopServer").ToString(), Application.Current.FindResource("Cancel").ToString()) With {.Owner = myWindow}
                If frm.ShowDialog() Then
                    BackupIsNotBusy = False
                    BackupStatus = "Server wird gestoppt..."
                    MinecraftServer.ExecuteCommand("stop")
                    BackupIsWorking = True
                    Dim t As New System.Threading.Thread(Sub()
                                                             Dim counter As Integer = 0
                                                             Dim maxvalue As Integer = 2000
                                                             While MinecraftServer.IsRunning AndAlso counter < maxvalue
                                                                 System.Threading.Thread.Sleep(50)
                                                                 counter += 1
                                                             End While
                                                             If MinecraftServer.IsRunning Then MinecraftServer.StopServer()
                                                             Dim lst = BackupManager.GetAllCheckedFiles(backup)
                                                             Application.Current.Dispatcher.BeginInvoke(Sub()
                                                                                                            BackupIsWorking = False
                                                                                                            If MinecraftServer.LauncherSettings.BackupFilesFirst Then
                                                                                                                backup.CreateType = CreateType.Automatically
                                                                                                                MinecraftServer.BackupManager.CreateBackup(backup, New IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory), False, Sub()
                                                                                                                                                                                                                                           MinecraftServer.BackupManager.RestoreBackup(backup, lst, AppDomain.CurrentDomain.BaseDirectory, AddressOf RestoreFinished)
                                                                                                                                                                                                                                       End Sub)
                                                                                                            Else
                                                                                                                MinecraftServer.BackupManager.RestoreBackup(backup, lst, AppDomain.CurrentDomain.BaseDirectory, AddressOf RestoreFinished)
                                                                                                            End If
                                                                                                        End Sub)
                                                         End Sub)
                    t.Start()
                End If
            Else
                MinecraftServer.BackupManager.LoadBackups()
            End If
        End If
    End Sub

    Private Sub CreateBackup()
        Dim frm As New frmCreateNewBackup(New IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory), MinecraftServer.BackupManager, Sub()
                                                                                                                                          BackupIsNotBusy = True
                                                                                                                                          MinecraftServer.BackupManager.LoadBackups()
                                                                                                                                          BackupStatus = String.Empty
                                                                                                                                      End Sub)
        frm.Owner = myWindow
        If frm.ShowDialog() Then
            BackupIsNotBusy = False
        End If
    End Sub
#End Region

#Region "Commands"
#Region "ServerCommand"
    Private _GeneralServerCommand As RelayCommand
    Public ReadOnly Property GeneralServerCommand() As RelayCommand
        Get
            If _GeneralServerCommand Is Nothing Then _GeneralServerCommand = New RelayCommand(AddressOf GeneralServerCommands)
            Return _GeneralServerCommand
        End Get
    End Property

    Private Sub GeneralServerCommands(parameter As Object)
        Select Case parameter.ToString()
            Case "ExecuteCommand"
                If Not String.IsNullOrWhiteSpace(txtCommand) Then
                    MinecraftServer.ExecuteCommand(txtCommand)
                    txtCommand = String.Empty
                End If
            Case "reload"
                MinecraftServer.ThriftAPI.Functions.ReloadServer()
            Case "saveall"
                MinecraftServer.ExecuteCommand("save-all")
            Case "timeday"
                MinecraftServer.ThriftAPI.Functions.SetWorldTime(MinecraftServer.ServerSettings.LevelName, 0)
            Case "timenight"
                MinecraftServer.ThriftAPI.Functions.SetWorldTime(MinecraftServer.ServerSettings.LevelName, 18000)
            Case "chngweather"
                Select Case CBBWeatherIndex
                    Case 0
                        MinecraftServer.ExecuteCommand("weather rain")
                    Case 1
                        MinecraftServer.ExecuteCommand("weather thunder")
                    Case 2
                        MinecraftServer.ExecuteCommand("weather clear")
                End Select
            Case "Announce"
                If Not String.IsNullOrWhiteSpace(txtAnnounce) Then
                    MinecraftServer.ExecuteCommand("say " & txtAnnounce)
                    txtAnnounce = String.Empty
                End If
            Case "SaveSettings"
                MinecraftServer.ServerSettings.Save()
                MinecraftServer.ServerSettings.Load()
            Case "DownloadPlugins"
                Dim frm As New frmDownloadPlugins(MinecraftServer.lstPlugins) With {.Owner = myWindow}
                frm.ShowDialog()
        End Select
    End Sub
#End Region

#Region "PlayerCommand"
    Private _GeneralPlayerCommand As RelayCommand
    Public ReadOnly Property GeneralPlayerCommand() As RelayCommand
        Get
            If _GeneralPlayerCommand Is Nothing Then _GeneralPlayerCommand = New RelayCommand(AddressOf GeneralPlayerCommands)
            Return _GeneralPlayerCommand
        End Get
    End Property

    Private Sub GeneralPlayerCommands(parameter As Object)
        Select Case parameter.ToString()
            Case "ClearInventory"
                MinecraftServer.ExecuteCommand("clear " & MinecraftServer.lstPlayers(lstPlayerIndex).Name)
            Case "SelectItem"
                Dim frm = New frmSelectItem With {.Owner = myWindow}
                If frm.ShowDialog() Then
                    Me.SelectedItem = ItemSelectionViewModel.Instance.SelectedItem
                End If
            Case "GiveItem"
                If SelectedItem IsNot Nothing Then
                    MinecraftServer.ExecuteCommand(String.Format("give {0} {1} {2}", MinecraftServer.lstPlayers(lstPlayerIndex).Name, SelectedItem.IDToString, ItemAmount))
                End If
            Case "KickPlayer"
                MinecraftServer.ExecuteCommand("kick " & MinecraftServer.lstPlayers(lstPlayerIndex).Name)
            Case "BanPlayer"
                MinecraftServer.ExecuteCommand("ban " & MinecraftServer.lstPlayers(lstPlayerIndex).Name)
            Case "BanIP"
                MinecraftServer.ExecuteCommand("ban-ip " & MinecraftServer.lstPlayers(lstPlayerIndex).Name)
            Case "SendMessage"
                If Not String.IsNullOrWhiteSpace(txtSendMessage) Then
                    MinecraftServer.ExecuteCommand(String.Format("tell {0} {1}", MinecraftServer.lstPlayers(lstPlayerIndex).Name, txtSendMessage))
                    txtSendMessage = String.Empty
                End If
            Case "Teleport"
                MinecraftServer.ExecuteCommand(String.Format("tp {0} {1}", MinecraftServer.lstPlayers(lstPlayerIndex).Name, lstPlayersWithoutSelected(CBBTpPlayer)))
                CBBTpPlayer = -1
            Case "ApplyEffect"
                MinecraftServer.ExecuteCommand(String.Format("effect {0} {1} {2}", MinecraftServer.lstPlayers(lstPlayerIndex).Name, CBBEffectIndex + 1, NudDuration))
            Case "GiveXP"
                Dim s As String
                If CBBXpTypeIndex = 1 Then s = "L" Else s = String.Empty
                MinecraftServer.ExecuteCommand(String.Format("xp {0}{1} {2}", NUDXpAmount, s, MinecraftServer.lstPlayers(lstPlayerIndex).Name))
            Case "SelectOtherItem"
                If Me.ItemToChange Is Nothing Then Return
                Dim frm = New frmSelectItem With {.Owner = myWindow}
                If frm.ShowDialog() Then
                    Me.NewSelectedItem = New org.phybros.thrift.ItemStack() With {.TypeId = ItemSelectionViewModel.Instance.SelectedItem.ID, .Data = ItemSelectionViewModel.Instance.SelectedItem.Meta}
                End If
            Case "ChangeItem"
                If ItemToChange IsNot Nothing Then
                    ItemToChange.Amount = NudItemAmount
                    ItemToChange.Data = NewSelectedItem.Data
                    ItemToChange.TypeId = NewSelectedItem.TypeId
                    MinecraftServer.ThriftAPI.Functions.UpdateInventoryItem(MinecraftServer.lstPlayers(lstPlayerIndex).Name, ItemToChange, ItemToChangeIndex)
                End If
            Case "DeleteItem"
                If ItemToChangeIndex > -1 Then
                    MinecraftServer.ThriftAPI.Functions.RemoveInventoryItem(MinecraftServer.lstPlayers(lstPlayerIndex).Name, ItemToChangeIndex)
                End If
        End Select
    End Sub
#End Region

#Region "SettingCommand"
    Private _GeneralSettingCommand As RelayCommand
    Public ReadOnly Property GeneralSettingsCommand() As RelayCommand
        Get
            If _GeneralSettingCommand Is Nothing Then _GeneralSettingCommand = New RelayCommand(AddressOf GeneralSettingCommands)
            Return _GeneralSettingCommand
        End Get
    End Property

    Private Sub GeneralSettingCommands(parameter As Object)
        Select Case parameter.ToString()
            Case "AddTimer"
                Dim tmr As New TimerExecuteCommand() With {.Command = Me.TimerCommand, .Interval = Integer.Parse(Me.TimerInterval.ToString()), .Name = Me.TimerName}
                MinecraftServer.LauncherSettings.AddTimer(tmr)
                Me.TimerName = String.Empty
                Me.TimerCommand = String.Empty
                Me.TimerInterval = 1
                tmr.Load(New SendCommand(AddressOf MinecraftServer.ExecuteCommand))
            Case "RestoreBackup"
                If BackupSelectedIndex > -1 Then
                    RestoreBackup()
                End If
            Case "CreateNewBackup"
                CreateBackup()
            Case "RemoveSelectedBackup"
                If BackupIsNotBusy AndAlso BackupSelectedIndex > -1 Then
                    MinecraftServer.BackupManager.RemoveBackup(MinecraftServer.BackupManager.BackupList(BackupSelectedIndex))
                End If
            Case "CreateBackupTimer"
                Dim frm As New frmCreateBackupTimer(MinecraftServer.LauncherSettings, New IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)) With {.Owner = myWindow}
                frm.ShowDialog()
            Case "RemoveSelectedTimer"
                MinecraftServer.LauncherSettings.RemoveTimer(SelectedTimer)
            Case "OpenIntelliSenseManager"
                Dim frm As New frmIntelliSenseManager(MinecraftServer.Commands) With {.Owner = myWindow}
                frm.ShowDialog()
        End Select
    End Sub
#End Region
#End Region
End Class
