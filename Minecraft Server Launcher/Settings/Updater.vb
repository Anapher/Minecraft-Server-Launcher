Imports updateSystemDotNet
Imports updateSystemDotNet.Core.Types

Public Class Updater
    Inherits PropertyChangedBase
    Public Event UpdatesFound(sender As Object, e As appEventArgs.updateFoundEventArgs)
    Private CurrentLanguage As Languages
    Private WithEvents _updController As updateController

    Public ReadOnly Property UpdateController As updateController
        Get
            Return _updController
        End Get
    End Property

    Public Sub New(Culture As String)
        _updController = New updateController

        UpdateController.updateUrl = "http://minecraftserverlauncher.besaba.com/Update"
        UpdateController.projectId = "d19afbe8-4b90-44d4-9d87-fc3206a854a4"
        UpdateController.publicKey = "<RSAKeyValue><Modulus>rspk8Abt6yFPOPQ70+bsta4sHaCzG0JxIZgIAb0DIVnJwuNm64MDwFN2dXMGWZYm7ILoQ38eUk8RKQt7Z/vQ7mZQkQh29A05eTz9Yg4RTf9naJCxD9lvIKHLpKGjDvOQ75pMfEIs7o5rz6lB8ICHLE28oERKBNpscobKHWnPsODR2TNP6V+h2s34w5SVxfA2plL/ogx54AWDbtwr5swl5hd4ktZF0Ga5nacuB4mKnc5h1v5UWnXeCOUX9E4xyE3KYKGYG9hK3eXkrssT0q9O+KyH9c7j4WEdf79CF6TLh3oDpzV0AlLSEweofCzkYocAFPdzyPvBv+2c/hSRrfUmOOM875+jBAk8K6R/wTE7QPPSNSaII03q0AavRoi6QUn+KV3il9y0SvgJoeZULoeghQKROv7mJ5szQz6gWS6oLzLztYBCyOT81EG7bNwVjWKwKI2kD4qRU2XVY+OCyEyKt8mmXp21PhKWBEZxf6sZWQAUBrV83n3fK4k06jlKOpSvSTCgka6fxENcrmXRKZ2SdfWOZQuIeY8kctaT/9pFqBMCKgAp4zKkj65ey3rdR5U6O3thvQf2M6Z60QTNOS7xUb9YEUZ4WIqr4Jnb88fLudGhhhgGPgzmq/yXdlYrzsZc0zB8gybIGwPTyOcXZV01GbLxUTnIWKSJv6pgDwBzFTc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"

        UpdateController.releaseFilter.checkForFinal = True
        UpdateController.releaseFilter.checkForBeta = False
        UpdateController.releaseFilter.checkForAlpha = False

        UpdateController.restartApplication = True
        UpdateController.retrieveHostVersion = True
        If Culture.StartsWith("de") Then CurrentLanguage = Languages.Deutsch Else CurrentLanguage = Languages.English
        UpdateController.Language = CurrentLanguage
    End Sub

    Private Sub _updController_downloadUpdatesCompleted(sender As Object, e As ComponentModel.AsyncCompletedEventArgs) Handles _updController.downloadUpdatesCompleted
        UpdateController.applyUpdate()
        Application.Current.Dispatcher.Invoke(Sub() Application.Current.Shutdown())
    End Sub

    Private Sub _updController_downloadUpdatesProgressChanged(sender As Object, e As appEventArgs.downloadUpdatesProgressChangedEventArgs) Handles _updController.downloadUpdatesProgressChanged
        Me.CurrentProgress = e.ProgressPercentage
    End Sub

    Private Sub updController_updateFound(sender As Object, e As appEventArgs.updateFoundEventArgs) Handles _updController.updateFound
        Me.NewVersion = e.Result.newUpdatePackages(e.Result.newUpdatePackages.Count - 1).releaseInfo.Version

        Dim size As Long = 0
        Dim sb As New Text.StringBuilder

        For Each package As updatePackage In e.Result.newUpdatePackages
            sb.Append(String.Format("{1}{0}------------------------------{0}{2}{0}", Environment.NewLine, String.Format(Application.Current.FindResource("UpdatePackageTitle").ToString(), package.releaseInfo.Version, package.ReleaseDate), If(CurrentLanguage = Languages.Deutsch, e.Result.Changelogs(package).germanChanges, e.Result.Changelogs(package).englishChanges)))
            size += package.packageSize
        Next
        Me.Changelog = sb.ToString()
        Me.UpdateSize = Helper.RoundSize(size)
        RaiseEvent UpdatesFound(Me, e)
    End Sub

#Region "Methods"
    Public Sub CheckForUpdates()
        UpdateController.checkForUpdatesAsync()
    End Sub

    Public Sub Update()
        UpdateController.downloadUpdates()
        IsBusy = True
    End Sub
#End Region

#Region "Properties"
    Public ReadOnly Property CurrentVersion As String
        Get
            Return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        End Get
    End Property

    Private _NewVersion As String
    Public Property NewVersion() As String
        Get
            Return _NewVersion
        End Get
        Protected Set(ByVal value As String)
            SetProperty(value, _NewVersion)
        End Set
    End Property

    Private _Changelog As String
    Public Property Changelog() As String
        Get
            Return _Changelog
        End Get
        Protected Set(ByVal value As String)
            SetProperty(value, _Changelog)
        End Set
    End Property

    Private _UpdateSize As String
    Public Property UpdateSize() As String
        Get
            Return _UpdateSize
        End Get
        Protected Set(ByVal value As String)
            SetProperty(value, _UpdateSize)
        End Set
    End Property

    Private _CurrentProgress As Integer
    Public Property CurrentProgress() As Integer
        Get
            Return _CurrentProgress
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _CurrentProgress)
        End Set
    End Property

    Private _IsBusy As Boolean
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _IsBusy)
        End Set
    End Property
#End Region
End Class
