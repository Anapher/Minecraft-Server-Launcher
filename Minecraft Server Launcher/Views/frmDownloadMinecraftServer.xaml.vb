Public Class frmDownloadDownloadMinecraftServer
    Implements ComponentModel.INotifyPropertyChanged

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        LoadVersions()
        Downloader = New MinecraftServerDownloader()
        AddHandler Downloader.DownloadMinecraftServerCompleted, Sub(s, e)
                                                                    Dim frm = New frmMessageBox(Application.Current.FindResource("CBDFText").ToString(), Application.Current.FindResource("CBDFTitle").ToString(), Application.Current.FindResource("SCBDFOK").ToString(), Application.Current.FindResource("Close").ToString()) With {.Owner = Me}
                                                                    If frm.ShowDialog() Then
                                                                        Process.Start(Application.ResourceAssembly.Location)
                                                                        Application.Current.Shutdown()
                                                                    Else
                                                                        Application.Current.Shutdown()
                                                                    End If
                                                                End Sub
    End Sub

    Private Sub LoadVersions()
        BukkitVersions = New BukkitVersionSelector()
        AddHandler BukkitVersions.GotVersions, Sub(sender, e)
                                                   MyPropertyChanged("BukkitVersions")
                                                   MyPropertyChanged("ButtonDownloadIsEnabled")
                                               End Sub
        AddHandler BukkitVersions.DownloadButtonEnabledChanged, AddressOf DownloadButtonEnabledChangedHandler
        BukkitVersions.LoadVersions()


        SpigotVersions = New SpigotVersionSelector()
        AddHandler SpigotVersions.GotVersions, Sub(s, e)
                                                   MyPropertyChanged("SpigotVersions")
                                                   MyPropertyChanged("ButtonDownloadIsEnabled")
                                               End Sub
        AddHandler SpigotVersions.DownloadButtonEnabledChanged, AddressOf DownloadButtonEnabledChangedHandler
        SpigotVersions.LoadVersions()


        CauldronVersions = New CauldronVersionSelector()
        AddHandler CauldronVersions.GotVersions, Sub(s, e)
                                                     MyPropertyChanged("CauldronVersions")
                                                     MyPropertyChanged("ButtonDownloadIsEnabled")
                                                 End Sub
        AddHandler CauldronVersions.DownloadButtonEnabledChanged, AddressOf DownloadButtonEnabledChangedHandler
        CauldronVersions.LoadVersions()
    End Sub

#Region "Download"
    Private _DownloadSelectedServerVersion As RelayCommand
    Public ReadOnly Property DownloadSelectedServerVersion As RelayCommand
        Get
            _DownloadSelectedServerVersion = New RelayCommand(Sub(parameter As Object)
                                                                  Select Case SelectedTabPage
                                                                      Case 0
                                                                          Downloader.Download(BukkitVersions.CurrentVersionDownloadLink)
                                                                      Case 1
                                                                          Downloader.Download(SpigotVersions.CurrentVersionDownloadLink)
                                                                      Case 2
                                                                          Downloader.Download(CauldronVersions.CurrentVersionDownloadLink, AddressOf CauldronVersions.Install)
                                                                  End Select
                                                              End Sub)
            Return _DownloadSelectedServerVersion
        End Get
    End Property

    Private _Downloader As MinecraftServerDownloader
    Public Property Downloader() As MinecraftServerDownloader
        Get
            Return _Downloader
        End Get
        Set(ByVal value As MinecraftServerDownloader)
            If value IsNot _Downloader Then
                _Downloader = value
                MyPropertyChanged("Downloader")
            End If
        End Set
    End Property
#End Region

#Region "GUI"
    Public ReadOnly Property ButtonDownloadIsEnabled() As Boolean
        Get
            Select Case SelectedTabPage
                Case 0
                    Return BukkitVersions IsNot Nothing AndAlso BukkitVersions.DownloadButtonIsEnabled
                Case 1
                    Return SpigotVersions IsNot Nothing AndAlso SpigotVersions.DownloadButtonIsEnabled
                Case 2
                    Return CauldronVersions IsNot Nothing AndAlso CauldronVersions.DownloadButtonIsEnabled
                Case Else
                    Return False
            End Select
        End Get
    End Property

    Private _SelectedTabPage As Integer
    Public Property SelectedTabPage() As Integer
        Get
            Return _SelectedTabPage
        End Get
        Set(ByVal value As Integer)
            If value <> _SelectedTabPage Then
                _SelectedTabPage = value
                MyPropertyChanged("ButtonDownloadIsEnabled")
            End If
        End Set
    End Property

    Private Sub DownloadButtonEnabledChangedHandler(sender As Object, e As EventArgs)
        MyPropertyChanged("ButtonDownloadIsEnabled")
    End Sub
#End Region

#Region "Server Versions"
    Private _BukkitVersions As BukkitVersionSelector
    Public Property BukkitVersions() As BukkitVersionSelector
        Get
            Return _BukkitVersions
        End Get
        Set(ByVal value As BukkitVersionSelector)
            If value IsNot _BukkitVersions Then
                _BukkitVersions = value
                MyPropertyChanged("BukkitVersions")
            End If
        End Set
    End Property

    Private _SpigotVersions As SpigotVersionSelector
    Public Property SpigotVersions() As SpigotVersionSelector
        Get
            Return _SpigotVersions
        End Get
        Set(ByVal value As SpigotVersionSelector)
            If value IsNot _SpigotVersions Then
                _SpigotVersions = value
                MyPropertyChanged("SpigotVersions")
            End If
        End Set
    End Property

    Private _CauldronVersions As CauldronVersionSelector
    Public Property CauldronVersions() As CauldronVersionSelector
        Get
            Return _CauldronVersions
        End Get
        Set(ByVal value As CauldronVersionSelector)
            If value IsNot _CauldronVersions Then
                _CauldronVersions = value
                MyPropertyChanged("CauldronVersions")
            End If
        End Set
    End Property
#End Region

#Region "INotifyPropertyChanged"
    Protected Sub MyPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(PropertyName))
    End Sub
    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region
End Class
