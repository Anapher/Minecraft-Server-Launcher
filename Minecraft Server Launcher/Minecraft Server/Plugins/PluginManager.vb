Imports System.Net
Imports Newtonsoft.Json

Public Class PluginManager
    Inherits PropertyChangedBase
    Implements IDisposable

    Private wc As WebClient
    Public Event ProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
    Public Event DownloadFinished(sender As Object, e As EventArgs)
    Public Event PluginRequestOpen(sender As Object, e As EventArgs)

#Region "Properties"
    Private _currentPluginlist As List(Of BukkitPlugin)
    Public Property NewProperty() As List(Of BukkitPlugin)
        Get
            Return _currentPluginlist
        End Get
        Set(ByVal value As List(Of BukkitPlugin))
            SetProperty(value, _currentPluginlist)
        End Set
    End Property

    Private _AllPlugins As List(Of BukkitPlugin)
    Public Property AllPlugins() As List(Of BukkitPlugin)
        Get
            Return _AllPlugins
        End Get
        Set(ByVal value As List(Of BukkitPlugin))
            SetProperty(value, _AllPlugins)
        End Set
    End Property
#End Region

    Private _InstalledPlugins As IEnumerable(Of org.phybros.thrift.Plugin)
    Public Sub New(InstalledPlugins As IEnumerable(Of org.phybros.thrift.Plugin))
        wc = New WebClient() With {.Proxy = Nothing}
        AddHandler wc.DownloadProgressChanged, AddressOf DownloadProgressChange
        AddHandler wc.DownloadStringCompleted, AddressOf DownloadPluginListCompleted
        Me._InstalledPlugins = InstalledPlugins
    End Sub

    Public Sub DownloadPluginList()
        wc.DownloadStringAsync(New Uri("http://api.bukget.org/3/plugins?fields=slug,plugin_name,description,categories,logo"))
    End Sub

#Region "Event handler"
    Private Sub DownloadProgressChange(sender As Object, e As DownloadProgressChangedEventArgs)
        RaiseEvent ProgressChanged(Me, e)
    End Sub

    Private Sub DownloadPluginListCompleted(sender As Object, e As DownloadStringCompletedEventArgs)
        Dim s = JsonConvert.DeserializeObject(Of Linq.JArray)(e.Result)
        Dim newlst = New List(Of BukkitPlugin)
        For Each i In s
            Dim plg = JsonConvert.DeserializeObject(Of BukkitPlugin)(i.ToString())

            AddHandler plg.RequestOpen, AddressOf RequestOpen
            plg.IsInstalled = PluginIsInstalled(plg.plugin_name)
            newlst.Add(plg)
        Next
        Me.AllPlugins = newlst
        RaiseEvent DownloadFinished(Me, EventArgs.Empty)
    End Sub

    Private Function PluginIsInstalled(PluginName As String) As Nullable(Of Boolean)
        If _InstalledPlugins Is Nothing Then Return Nothing
        For Each i In _InstalledPlugins
            If i.Name.ToLower() = PluginName.ToLower() Then Return True
        Next
        Return False
    End Function

    Private Sub RequestOpen(sender As Object, e As EventArgs)
        RaiseEvent PluginRequestOpen(sender, e)
    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' So ermitteln Sie überflüssige Aufrufe

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: Verwalteten Zustand löschen (verwaltete Objekte).
                wc.Dispose()
            End If

            ' TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalize() unten überschreiben.
            ' TODO: Große Felder auf NULL festlegen.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: Finalize() nur überschreiben, wenn Dispose(ByVal disposing As Boolean) oben über Code zum Freigeben von nicht verwalteten Ressourcen verfügt.
    'Protected Overrides Sub Finalize()
    '    ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(disposing As Boolean) Bereinigungscode ein.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
