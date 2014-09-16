Imports System.Collections.ObjectModel

Public Class ItemSelectionViewModel
    Inherits PropertyChangedBase

#Region "Singleton"
    Private Shared _Instance As ItemSelectionViewModel
    Public Shared ReadOnly Property Instance As ItemSelectionViewModel
        Get
            If _Instance Is Nothing Then _Instance = New ItemSelectionViewModel()
            Return _Instance
        End Get
    End Property

    Public Sub New()
        Me.Items = ItemData.Instance.lstMinecraftItem
    End Sub
#End Region

#Region "Properties"
    Private _items As List(Of MinecraftItem)
    Public Property Items() As List(Of MinecraftItem)
        Get
            Return _items
        End Get
        Set(ByVal value As List(Of MinecraftItem))
            SetProperty(value, _items)
            RefreshList(value)
        End Set
    End Property

    Private _ViewSource As CollectionView
    Public Property ViewSource() As CollectionView
        Get
            Return _ViewSource
        End Get
        Set(ByVal value As CollectionView)
            SetProperty(value, _ViewSource)
        End Set
    End Property

    Private Sub RefreshList(defaultlist As List(Of MinecraftItem))
        If defaultlist IsNot Nothing Then
            ViewSource = DirectCast(CollectionViewSource.GetDefaultView(defaultlist), CollectionView)
            ViewSource.Filter = Function(item)
                                    If String.IsNullOrWhiteSpace(txt) Then
                                        Return True
                                    Else
                                        Select Case True
                                            Case radID
                                                Return DirectCast(item, MinecraftItem).IDToString.StartsWith(txt)
                                            Case radName
                                                Return DirectCast(item, MinecraftItem).Name.ToUpper().Contains(txt.ToUpper())
                                            Case Else : Return False
                                        End Select
                                    End If
                                End Function
        End If
    End Sub

    Private _txt As String
    Public Property txt As String
        Get
            Return _txt
        End Get
        Set(ByVal value As String)
            SetProperty(value, _txt)
            If ViewSource IsNot Nothing Then ViewSource.Refresh()
        End Set
    End Property

    Private _radName As Boolean = True
    Public Property radName() As Boolean
        Get
            Return _radName
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _radName)
            If ViewSource IsNot Nothing Then ViewSource.Refresh()
        End Set
    End Property

    Private _radID As Boolean
    Public Property radID() As Boolean
        Get
            Return _radID
        End Get
        Set(ByVal value As Boolean)
            SetProperty(value, _radID)
            If ViewSource IsNot Nothing Then ViewSource.Refresh()
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
