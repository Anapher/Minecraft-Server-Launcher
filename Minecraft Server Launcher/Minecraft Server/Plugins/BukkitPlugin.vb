Imports Newtonsoft.Json
Imports System.Net
Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip

Public Class BukkitPlugin
    Inherits PropertyChangedBase

    Public Event LoadingFinished(sender As Object, e As EventArgs)
    Public Event DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
    Public Event DownloadFinished(sender As Object, e As EventArgs)
    Public Event RequestOpen(sender As Object, e As EventArgs)

    Private wc As System.Net.WebClient

    Private _description As String
    Public Property description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Private _plugin_name As String
    Public Property plugin_name() As String
        Get
            Return _plugin_name
        End Get
        Set(ByVal value As String)
            _plugin_name = value
        End Set
    End Property

    Private _slug As String
    Public Property slug() As String
        Get
            Return _slug
        End Get
        Set(ByVal value As String)
            _slug = value
        End Set
    End Property

    Private _IsInstalled As Nullable(Of Boolean)
    Public Property IsInstalled() As Nullable(Of Boolean)
        Get
            Return _IsInstalled
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _IsInstalled = value
        End Set
    End Property


    Private _categories As List(Of String)
    Public Property categories() As List(Of String)
        Get
            Return _categories
        End Get
        Set(ByVal value As List(Of String))
            _categories = value
        End Set
    End Property

    Public ReadOnly Property categoriesImages As List(Of categorie)
        Get
            Dim lst As New List(Of categorie)
            Dim categories As New List(Of categorie)
            With categories
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/AdminTools.png", UriKind.Absolute)), .Tooltip = "Admin Tools"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/AntiGrief.png", UriKind.Absolute)), .Tooltip = "Anti-Griefing Tools"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Chat.png", UriKind.Absolute)), .Tooltip = "Chat Related"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Fun.png", UriKind.Absolute)), .Tooltip = "Client Fun"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Teleportation.png", UriKind.Absolute)), .Tooltip = "Client Teleportation"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/DevTools.png", UriKind.Absolute)), .Tooltip = "Developer Tools"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Economy.png", UriKind.Absolute)), .Tooltip = "Economy"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Fixes.png", UriKind.Absolute)), .Tooltip = "Fixes"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Fun.png", UriKind.Absolute)), .Tooltip = "Fun"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/General.png", UriKind.Absolute)), .Tooltip = "General"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Informational.png", UriKind.Absolute)), .Tooltip = "Informational"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Mechanics.png", UriKind.Absolute)), .Tooltip = "Mechanics"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Miscellaneous.png", UriKind.Absolute)), .Tooltip = "Miscellaneous"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/RolePlaying.png", UriKind.Absolute)), .Tooltip = "Role Playing"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/Teleportation.png", UriKind.Absolute)), .Tooltip = "Teleportation"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/WebsiteAdministration.png", UriKind.Absolute)), .Tooltip = "Website Administration"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/worldmanagement.png", UriKind.Absolute)), .Tooltip = "World Editing and Management"})
                .Add(New categorie With {.Image = New BitmapImage(New Uri("pack://application:,,,/resources/CategorieIcons/worldgenerators.png", UriKind.Absolute)), .Tooltip = "World Generators"})
            End With
            For Each i In Me.categories
                For Each c In categories
                    If i.ToLower() = c.Tooltip.ToLower() Then lst.Add(c) : Exit For
                Next
            Next
            Return lst
        End Get
    End Property

    Private _logo As String
    Public Property logo() As String
        Get
            Return _logo
        End Get
        Set(ByVal value As String)
            _logo = value
        End Set
    End Property

    Private _Info As PluginInfo
    Public Property Info() As PluginInfo
        Get
            Return _Info
        End Get
        Set(ByVal value As PluginInfo)
            SetProperty(value, _Info)
        End Set
    End Property

    Public ReadOnly Property GotInfo As Boolean
        Get
            Return Info IsNot Nothing
        End Get
    End Property

    Private _OpenCommand As RelayCommand
    Public ReadOnly Property OpenCommand As RelayCommand
        Get
            If _OpenCommand Is Nothing Then _OpenCommand = New RelayCommand(Sub()
                                                                                RaiseEvent RequestOpen(Me, EventArgs.Empty)
                                                                            End Sub)
            Return _OpenCommand
        End Get
    End Property

    Private _Thumbnail As BitmapImage
    Public Property Thumbnail() As BitmapImage
        Get
            Return _Thumbnail
        End Get
        Protected Set(ByVal value As BitmapImage)
            SetProperty(value, _Thumbnail)
        End Set
    End Property

#Region "Download"
    Public Sub DownloadPlugin(Path As String, version As Version)
        Me.DownloadPlugin(New FileInfo(Path), version)
    End Sub

    Public Sub DownloadPlugin(Path As FileInfo, version As Version)
        If Path.Exists Then
            If Not MessageBox.Show(String.Format("Die Datei {0} ist bereits vorhanden. Wollen sie diese ersetzten?", version.filename), "", MessageBoxButton.OKCancel, MessageBoxImage.Stop) = MessageBoxResult.OK Then
                RaiseEvent DownloadFinished(Me, EventArgs.Empty)
                Return
            End If
        End If
        wc.DownloadFileAsync(New Uri(version.download), Path.FullName, New DeliveryInfo With {.Path = Path, .Version = version})
    End Sub

    Private Class DeliveryInfo
        Public Property Version As Version
        Public Property Path As FileInfo
    End Class

    Private Sub ProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        RaiseEvent DownloadProgressChanged(Me, e)
    End Sub

    Private Sub DownloadFileCompleted(sender As Object, e As ComponentModel.AsyncCompletedEventArgs)
        RaiseEvent DownloadFinished(Me, EventArgs.Empty)
        Dim info = DirectCast(e.UserState, DeliveryInfo)

        Dim path = New FileInfo(info.Path.FullName)
        'Using mymd5 = System.Security.Cryptography.MD5.Create
        'Using Stream = path.OpenRead()
        'If Not Convert.ToBase64String(mymd5.ComputeHash(Stream)) = info.Version.md5 Then
        'If MessageBox.Show(Application.Current.FindResource("msgHash").ToString(), Application.Current.FindResource("warning").ToString(), MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) = MessageBoxResult.Yes Then
        'path.Delete()
        'Return
        'End If
        'End If
        'End Using
        'End Using

        If path.Exists AndAlso path.Extension = ".zip" Then
            Dim fz As New FastZip
            fz.ExtractZip(path.FullName, path.Directory.FullName, Nothing)
            path.Delete()
        End If
    End Sub
#End Region

#Region "New"
    Public Sub New()
        wc = New WebClient() With {.Proxy = Nothing}
        AddHandler wc.DownloadProgressChanged, AddressOf ProgressChanged
        AddHandler wc.DownloadFileCompleted, AddressOf DownloadFileCompleted
    End Sub

    Public Sub Load()
        If Not Me.GotInfo Then
            Dim wc As New Net.WebClient() With {.Proxy = Nothing}
            AddHandler wc.DownloadStringCompleted, AddressOf LoadedStringCompleted
            AddHandler wc.DownloadProgressChanged, Sub(s, e)
                                                       RaiseEvent DownloadProgressChanged(Me, e)
                                                   End Sub
            wc.DownloadStringAsync(New Uri("http://api.bukget.org/3/plugins/bukkit/" & slug))
        Else
            RaiseEvent LoadingFinished(Me, EventArgs.Empty)
        End If
    End Sub
#End Region


    Private Sub LoadedStringCompleted(sender As Object, e As Net.DownloadStringCompletedEventArgs)
        If e.Error Is Nothing AndAlso Not e.Cancelled Then
            Me.Info = JsonConvert.DeserializeObject(Of PluginInfo)(e.Result)
        End If
        RaiseEvent LoadingFinished(Me, EventArgs.Empty)
    End Sub

    Public Sub DownloadImage()
        If Thumbnail Is Nothing Then
            If String.IsNullOrWhiteSpace(Me.logo) Then
                Me.Thumbnail = New BitmapImage(New Uri("pack://application:,,,/resources/bukkitlogo.png", UriKind.Absolute))
            Else
                Dim worker As New ComponentModel.BackgroundWorker()
                AddHandler worker.DoWork, Sub(s, e)
                                              Dim uri As Uri = TryCast(e.Argument, Uri)

                                              Using webClient As New WebClient() With {.Proxy = Nothing, .CachePolicy = New Cache.RequestCachePolicy(Cache.RequestCacheLevel.[Default])}
                                                  Try
                                                      Dim imageBytes As Byte() = Nothing

                                                      imageBytes = webClient.DownloadData(uri)

                                                      If imageBytes Is Nothing Then
                                                          e.Result = Nothing
                                                          Return
                                                      End If
                                                      Dim imageStream As New IO.MemoryStream(imageBytes)
                                                      Dim image As New BitmapImage()

                                                      image.BeginInit()
                                                      image.StreamSource = imageStream
                                                      image.CacheOption = BitmapCacheOption.OnLoad
                                                      image.EndInit()

                                                      image.Freeze()
                                                      imageStream.Close()

                                                      e.Result = image
                                                  Catch ex As WebException
                                                      e.Result = ex
                                                  End Try
                                              End Using
                                          End Sub
                AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                          Dim bitmapImage As BitmapImage = TryCast(e.Result, BitmapImage)
                                                          If bitmapImage IsNot Nothing Then
                                                              Thumbnail = bitmapImage
                                                          End If
                                                          worker.Dispose()
                                                      End Sub
                worker.RunWorkerAsync(New Uri(logo))
            End If
        End If
    End Sub
End Class
