Public Class DownloadCraftbukkitViewModel
    Inherits PropertyChangedBase

#Region "Singleton & Constructor"
    Private Shared _Instance As DownloadCraftbukkitViewModel
    Public Shared ReadOnly Property Instance As DownloadCraftbukkitViewModel
        Get
            If _Instance Is Nothing Then _Instance = New DownloadCraftbukkitViewModel
            Return _Instance
        End Get
    End Property

    Public Sub New()
        GUIIsEnabled = False
        Downloader = New CraftbukkitDownloader
        Downloader.LoadVersions()
        AddHandler Downloader.GotVersions, Sub(sender, e)
                                               GUIIsEnabled = True
                                               OnPropertyChanged("ButtonDownloadIsEnabled")
                                           End Sub
        AddHandler Downloader.DownloadCraftbukkitCompleted, Sub(sender, e)
                                                                Dim frm = New frmMessageBox(Application.Current.FindResource("CBDFText").ToString(), Application.Current.FindResource("CBDFTitle").ToString(), Application.Current.FindResource("SCBDFOK").ToString(), Application.Current.FindResource("CBDFCancel").ToString()) With {.Owner = myWindow}
                                                                If frm.ShowDialog() Then
                                                                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location)
                                                                    Application.Current.Shutdown()
                                                                Else
                                                                    Application.Current.Shutdown()
                                                                End If
                                                            End Sub
    End Sub
#End Region

#Region "Properties"
    Property myWindow As Window

    Private _ChooseSelectedVersion As Boolean
    Public Property ChooseSelectedVersion() As Boolean
        Get
            Return _ChooseSelectedVersion
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ChooseSelectedVersion)
            OnPropertyChanged("ButtonDownloadIsEnabled")
        End Set
    End Property

    Private _ChooseDevelopmentVersion As Boolean
    Public Property ChooseDevelopmentVersion() As Boolean
        Get
            Return _ChooseDevelopmentVersion
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ChooseDevelopmentVersion)
        End Set
    End Property

    Private _ChooseBetaVersion As Boolean
    Public Property ChooseBetaVersion() As Boolean
        Get
            Return _ChooseBetaVersion
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ChooseBetaVersion)
        End Set
    End Property

    Private _ChooseRecommendedVersion As Boolean = True
    Public Property ChooseRecommendedVersion() As Boolean
        Get
            Return _ChooseRecommendedVersion
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _ChooseRecommendedVersion)
        End Set
    End Property

    Private _GUIIsEnabled As Boolean = True
    Public Property GUIIsEnabled() As Boolean
        Get
            Return _GUIIsEnabled
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _GUIIsEnabled)
        End Set
    End Property

    Private _Downloader As CraftbukkitDownloader
    Public Property Downloader() As CraftbukkitDownloader
        Get
            Return _Downloader
        End Get
        Set(ByVal value As CraftbukkitDownloader)
            SetProperty(value, _Downloader)
        End Set
    End Property

    Private _lstIndex As Integer = -1
    Public Property lstIndex() As Integer
        Get
            Return _lstIndex
        End Get
        Set(ByVal value As Integer)
            If value <> _lstIndex Then
                ChooseSelectedVersion = True
                ChooseDevelopmentVersion = False
                ChooseRecommendedVersion = False
                ChooseBetaVersion = False
            End If
            SetProperty(value, _lstIndex)
            OnPropertyChanged("ButtonDownloadIsEnabled")
        End Set
    End Property

    Public ReadOnly Property ButtonDownloadIsEnabled As Boolean
        Get
            Return (Not ChooseSelectedVersion AndAlso GUIIsEnabled) OrElse (GUIIsEnabled AndAlso lstIndex > -1)
        End Get
    End Property


#End Region

#Region "Methods"

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
            Case "Download"
                GUIIsEnabled = False
                Dim currentversion As BukkitVersion = Nothing
                Select Case True
                    Case ChooseBetaVersion
                        currentversion = Downloader.BetaBuild
                    Case ChooseDevelopmentVersion
                        currentversion = Downloader.DevelopmentBuild
                    Case ChooseRecommendedVersion
                        currentversion = Downloader.RecommendedBuild
                    Case ChooseSelectedVersion
                        currentversion = Downloader.lstBukkitVersions(lstIndex)
                End Select
                Downloader.Download(currentversion)
                OnPropertyChanged("ButtonDownloadIsEnabled")
            Case "OpenInfos"
                If lstIndex > -1 Then
                    Process.Start("http://dl.bukkit.org/" & Downloader.lstBukkitVersions(lstIndex).InfoURL)
                End If
        End Select
    End Sub
#End Region


End Class
