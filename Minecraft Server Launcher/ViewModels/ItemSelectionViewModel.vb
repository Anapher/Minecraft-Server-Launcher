Imports System.Collections.ObjectModel

Public Class ItemSelectionViewModel
    Inherits PropertyChangedBase

#Region "Singleton"
    Private Shared _Instance As ItemSelectionViewModel
    Public Shared ReadOnly Property Instance As ItemSelectionViewModel
        Get
            If _Instance Is Nothing Then _Instance = New ItemSelectionViewModel
            Return _Instance
        End Get
    End Property

    Public Sub New()
        RefreshItems()
    End Sub

    Private _allitems As ObservableCollection(Of MinecraftItem)
    Private Sub RefreshItems()
        If String.IsNullOrWhiteSpace(txt) Then
            If _allitems Is Nothing Then
                _allitems = New ObservableCollection(Of MinecraftItem)
                For Each i In ItemData.Instance.lstMinecraftItem
                    _allitems.Add(i)
                Next
            End If
            Items = _allitems
        Else
            Items = New ObservableCollection(Of MinecraftItem)
            For Each i In ItemData.Instance.lstMinecraftItem
                If radName Then
                    If i.Name.ToLower().Contains(txt.ToLower()) Then
                        Items.Add(i)
                    End If
                ElseIf radID Then
                    If i.ID.ToString().StartsWith(txt) Then
                        Items.Add(i)
                    End If
                End If
            Next
        End If
    End Sub
#End Region

#Region "Properties"
    Private _items As ObservableCollection(Of MinecraftItem) = New ObservableCollection(Of MinecraftItem)
    Public Property Items() As ObservableCollection(Of MinecraftItem)
        Get
            Return _items
        End Get
        Set(ByVal value As ObservableCollection(Of MinecraftItem))
            SetProperty(value, _items)
        End Set
    End Property

    Private _txt As String
    Public Property txt As String
        Get
            Return _txt
        End Get
        Set(ByVal value As String)
            SetProperty(value, _txt)
            RefreshItems()
        End Set
    End Property

    Private _radName As Boolean = True
    Public Property radName() As Boolean
        Get
            Return _radName
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _radName)
            RefreshItems()
        End Set
    End Property

    Private _radID As Boolean
    Public Property radID() As Boolean
        Get
            Return _radID
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _radID)
            RefreshItems()
        End Set
    End Property

    Private _lstIndex As Integer = -1
    Public Property lstIndex() As Integer
        Get
            Return _lstIndex
        End Get
        Set(ByVal value As Integer)
            SetProperty(value, _lstIndex)
            If lstIndex > -1 Then OkIsEnabled = True Else OkIsEnabled = False
        End Set
    End Property

    Private _OkIsEnabled As Boolean = False
    Public Property OkIsEnabled() As Boolean
        Get
            Return _OkIsEnabled
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _OkIsEnabled)
        End Set
    End Property

    Public ReadOnly Property SelectedItem As MinecraftItem
        Get
            Return Items(lstIndex)
        End Get
    End Property
#End Region
End Class
