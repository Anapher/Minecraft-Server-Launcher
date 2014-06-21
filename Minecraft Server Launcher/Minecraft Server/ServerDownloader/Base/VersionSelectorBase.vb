Public MustInherit Class VersionSelector
    Inherits PropertyChangedBase

    Public MustOverride ReadOnly Property CurrentVersionDownloadLink As String
    Public MustOverride ReadOnly Property DownloadButtonIsEnabled() As Boolean

    Public MustOverride Sub LoadVersions()

    Public Event GotVersions(sender As Object, e As EventArgs)
    Public Event DownloadButtonEnabledChanged(sender As Object, e As EventArgs)

    Private _GotInformations As Boolean = False
    Public Property GotInformations() As Boolean
        Get
            Return _GotInformations
        End Get
        Protected Set(ByVal value As Boolean)
            SetProperty(value, _GotInformations)
        End Set
    End Property

    Protected Sub OnDownloadButtonEnabledChanged()
        RaiseEvent DownloadButtonEnabledChanged(Me, EventArgs.Empty)
    End Sub

    Protected Sub OnGotVersions()
        RaiseEvent GotVersions(Me, EventArgs.Empty)
        Me.GotInformations = True
    End Sub
End Class