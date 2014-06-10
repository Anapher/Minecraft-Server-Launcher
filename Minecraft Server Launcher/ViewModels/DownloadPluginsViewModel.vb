Public Class DownloadPluginsViewModel
    Inherits PropertyChangedBase

#Region "Singleton & Constructor"
    Private Shared _Instance As DownloadPluginsViewModel
    Public Shared ReadOnly Property Instance As DownloadPluginsViewModel
        Get
            If _Instance Is Nothing Then _Instance = New DownloadPluginsViewModel
            Return _Instance
        End Get
    End Property

    Public Sub Load(InstalledPlugins As IEnumerable(Of org.phybros.thrift.Plugin))
        _Plugins = New PluginManager(InstalledPlugins)
        AddHandler _Plugins.ProgressChanged, AddressOf ProgressChange
        AddHandler _Plugins.DownloadFinished, Sub(s, e)
                                                  Me.ProgressValue = 0
                                              End Sub
        AddHandler _Plugins.PluginRequestOpen, Sub(s, e)
                                                   Dim plg = DirectCast(s, BukkitPlugin)
                                                   Me.PluginsSelectedIndex = CurrentPluginList.IndexOf(plg)
                                                   ExecuteCommand("OpenPlugin")
                                               End Sub
        ProgressIsIndeterminate = False
        _Plugins.DownloadPluginList()
    End Sub

    Private Sub ProgressChange(sender As Object, e As Net.DownloadProgressChangedEventArgs)
        Me.ProgressValue = e.ProgressPercentage
    End Sub
#End Region

#Region "Properties"
    Private _Plugins As PluginManager
    Public Property Plugins() As PluginManager
        Get
            Return _Plugins
        End Get
        Set(ByVal value As PluginManager)
            SetProperty(value, _Plugins)
        End Set
    End Property

    Private _CurrentPluginList As List(Of BukkitPlugin)
    Public Property CurrentPluginList() As List(Of BukkitPlugin)
        Get
            Return _CurrentPluginList
        End Get
        Set(ByVal value As List(Of BukkitPlugin))
            SetProperty(value, _CurrentPluginList)
        End Set
    End Property

    Private _SearchText As String
    Public Property SearchText() As String
        Get
            Return _SearchText
        End Get
        Set(ByVal value As String)
            SetProperty(value, _SearchText)
        End Set
    End Property

    Private _PluginsSelectedIndex As Integer
    Public Property PluginsSelectedIndex() As Integer
        Get
            Return _PluginsSelectedIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _PluginsSelectedIndex)
        End Set
    End Property

    Private _currentPlugin As BukkitPlugin
    Public Property CurrentPlugin() As BukkitPlugin
        Get
            Return _currentPlugin
        End Get
        Set(ByVal value As BukkitPlugin)
            SetProperty(value, _currentPlugin)
        End Set
    End Property

    Private _TabControlSelectedIndex As Integer = 0
    Public Property TabControlSelectedIndex() As Integer
        Get
            Return _TabControlSelectedIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _TabControlSelectedIndex)
            OnPropertyChanged("CanShowCommands")
        End Set
    End Property

    Private _PluginVersionsIndex As Integer = 0
    Public Property PluginVersionsIndex() As Integer
        Get
            Return _PluginVersionsIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _PluginVersionsIndex)
            OnPropertyChanged("CurrentPluginVersion")
            OnPropertyChanged("PluginVersionVisible")
            OnPropertyChanged("CanShowCommands")
            OnPropertyChanged("ButtonDownloadEnabled")
        End Set
    End Property

    Public ReadOnly Property CurrentPluginVersion As Version
        Get
            If PluginVersionsIndex > -1 AndAlso CurrentPlugin IsNot Nothing AndAlso CurrentPlugin.Info IsNot Nothing Then
                Return CurrentPlugin.Info.versions(PluginVersionsIndex)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property PluginVersionVisible As Visibility
        Get
            If PluginsSelectedIndex > -1 Then Return Visibility.Visible Else Return Visibility.Hidden
        End Get
    End Property

    Private _ProgressValue As Double
    Public Property ProgressValue() As Double
        Get
            Return _ProgressValue
        End Get
        Set(ByVal value As Double)
            SetProperty(value, _ProgressValue)
        End Set
    End Property

    Private _ProgressIsIndeterminate As Boolean
    Public Property ProgressIsIndeterminate() As Boolean
        Get
            Return _ProgressIsIndeterminate
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ProgressIsIndeterminate)
        End Set
    End Property

    Private _Window As Window
    Public Property Window() As Window
        Get
            Return _Window
        End Get
        Set(ByVal value As Window)
            _Window = value
        End Set
    End Property

    Public ReadOnly Property ButtonDownloadEnabled() As Boolean
        Get
            Return IsNotDownloading AndAlso CurrentPluginVersion IsNot Nothing
        End Get
    End Property

    Private _IsNotDownloading As Boolean = True
    Public Property IsNotDownloading() As Boolean
        Get
            Return _IsNotDownloading
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _IsNotDownloading)
            OnPropertyChanged("ButtonDownloadEnabled")
        End Set
    End Property

    Private _IsDownloading As Boolean = False
    Public Property IsDownloading() As Boolean
        Get
            Return _IsDownloading
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _IsDownloading)
            IsNotDownloading = Not value
        End Set
    End Property

    Public ReadOnly Property CanShowCommands As Boolean
        Get
            If Me.CurrentPluginVersion IsNot Nothing AndAlso Me.CurrentPluginVersion.commands IsNot Nothing AndAlso Me.CurrentPluginVersion.commands.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
#End Region

#Region "Command"
    Private _command As RelayCommand

    Public ReadOnly Property Command As RelayCommand
        Get
            If _command Is Nothing Then _command = New RelayCommand(AddressOf ExecuteCommand)
            Return _command
        End Get
    End Property

    Public Sub ExecuteCommand(parameter As Object)
        Select Case parameter.ToString()
            Case "search"
                If Not String.IsNullOrWhiteSpace(SearchText) AndAlso Plugins.AllPlugins IsNot Nothing Then
                    Dim newlst As New List(Of BukkitPlugin)
                    For Each i In Plugins.AllPlugins
                        If i.plugin_name.ToLower().StartsWith(SearchText.ToLower()) Then
                            newlst.Add(i)
                            i.DownloadImage()
                        End If
                    Next
                    Me.CurrentPluginList = newlst
                End If
            Case "OpenPlugin"
                CurrentPlugin = CurrentPluginList(PluginsSelectedIndex)
                Me.ProgressIsIndeterminate = True
                AddHandler CurrentPlugin.LoadingFinished, Sub(s, e)
                                                              Me.ProgressIsIndeterminate = False
                                                              OnPropertyChanged("CurrentPluginVersion")
                                                              OnPropertyChanged("PluginVersionVisible")
                                                              ProgressValue = 0
                                                              OnPropertyChanged("ButtonDownloadEnabled")
                                                          End Sub
                AddHandler CurrentPlugin.DownloadProgressChanged, Sub(s, e)
                                                                      Me.ProgressValue = e.ProgressPercentage
                                                                  End Sub
                AddHandler CurrentPlugin.DownloadFinished, Sub(s, e)
                                                               Me.ProgressValue = 0
                                                               Me.IsDownloading = False
                                                           End Sub
                CurrentPlugin.Load()
                TabControlSelectedIndex = 1
                PluginVersionsIndex = 0
            Case "ShowChangelog"
                Dim frm = New frmChangelog() With {.Owner = Window}
                frm.ShowDialog()
            Case "ShowCommands"
                Dim frm = New frmCommands() With {.Owner = Window}
                frm.DataContext = CurrentPluginVersion
                frm.ShowDialog()
            Case "GoBack"
                TabControlSelectedIndex = 0
            Case "DownloadPlugin"
                IsDownloading = True
                CurrentPlugin.DownloadPlugin(IO.Path.Combine(Paths.GetPaths.MinecraftServerFolder.FullName, "plugins", CurrentPluginVersion.filename), CurrentPluginVersion)
        End Select
    End Sub
#End Region

End Class
