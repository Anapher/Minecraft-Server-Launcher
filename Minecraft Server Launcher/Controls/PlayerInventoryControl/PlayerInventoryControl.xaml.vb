Imports org.phybros.thrift
Imports System.ComponentModel
Imports System.Windows

Public Class PlayerInventoryControl
    Implements INotifyPropertyChanged

    Public Shared ReadOnly InventoryProperty As DependencyProperty = DependencyProperty.Register("Inventory", GetType(PlayerInventory), GetType(PlayerInventoryControl), New PropertyMetadata(Nothing, AddressOf OnFirstPropertyChanged))
    Public Shared ReadOnly SelectedIndexProperty As DependencyProperty = DependencyProperty.Register("SelectedIndex", GetType(Integer), GetType(PlayerInventoryControl), New FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault))
    Public Shared ReadOnly SelectedItemProperty As DependencyProperty = DependencyProperty.Register("SelectedItem", GetType(ItemStack), GetType(PlayerInventoryControl), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault))

    Private _inventory As PlayerInventory
    Public Property Inventory() As PlayerInventory
        Get
            Return DirectCast(Me.GetValue(InventoryProperty), PlayerInventory)
        End Get
        Set(ByVal value As PlayerInventory)
            Me.SetValue(InventoryProperty, value)
            RefreshProperties()
        End Set
    End Property

    Public Sub RefreshProperties()
        Dim i1 = lstBaseInventorySelectedIndex
        Dim i2 = lstInnerInventorySelectedIndex
        Dim i3 = lstArmorInventorySelectedIndex
        MyOnPropertyChanged("Inventory")
        MyOnPropertyChanged("InnerInventory")
        MyOnPropertyChanged("BaseInventory")
        MyOnPropertyChanged("ArmorInventory")
        lstBaseInventorySelectedIndex = i1
        lstInnerInventorySelectedIndex = i2
        lstArmorInventorySelectedIndex = i3
    End Sub

    Public Property SelectedIndex As Integer
        Get
            Return DirectCast(Me.GetValue(SelectedIndexProperty), Integer)
        End Get
        Set(value As Integer)
            Me.SetValue(SelectedIndexProperty, value)
        End Set
    End Property

    Public Property SelectedItem As ItemStack
        Get
            Return DirectCast(Me.GetValue(SelectedItemProperty), ItemStack)
        End Get
        Set(value As ItemStack)
            Me.SetValue(SelectedItemProperty, value)
        End Set
    End Property

    Private Sub UpdateSelection()
        If Inventory IsNot Nothing AndAlso Inventory.Armor IsNot Nothing Then
            Select Case True
                Case lstBaseInventorySelectedIndex > -1
                    Me.SelectedItem = BaseInventory(lstBaseInventorySelectedIndex)
                Case lstInnerInventorySelectedIndex > -1
                    Me.SelectedItem = InnerInventory(lstInnerInventorySelectedIndex)
                Case lstArmorInventorySelectedIndex > -1
                    Select Case lstArmorInventorySelectedIndex
                        Case 0
                            Me.SelectedItem = Inventory.Armor.Helmet
                        Case 1
                            Me.SelectedItem = Inventory.Armor.Chestplate
                        Case 2
                            Me.SelectedItem = Inventory.Armor.Leggings
                        Case 3
                            Me.SelectedItem = Inventory.Armor.Boots
                        Case Else
                            Me.SelectedItem = Nothing
                    End Select
                Case Else
                    Me.SelectedItem = Nothing
            End Select
            Select Case True
                Case lstBaseInventorySelectedIndex > -1
                    Me.SelectedIndex = lstBaseInventorySelectedIndex
                Case lstInnerInventorySelectedIndex > -1
                    Me.SelectedIndex = lstInnerInventorySelectedIndex + 9
                Case lstArmorInventorySelectedIndex > -1
                    Me.SelectedIndex = 100 + lstArmorInventorySelectedIndex
                Case Else
                    Me.SelectedIndex = -1
            End Select
        End If
    End Sub

    Public Shared Sub OnFirstPropertyChanged(sender As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim h = DirectCast(sender, PlayerInventoryControl)
        If h IsNot Nothing Then
            h.RefreshProperties()
        End If
    End Sub

    Public ReadOnly Property InnerInventory As List(Of ItemStack)
        Get
            If Inventory IsNot Nothing AndAlso Inventory.Inventory IsNot Nothing Then
                Dim lst As New List(Of ItemStack)
                For i = 9 To Inventory.Inventory.Count - 1
                    lst.Add(Inventory.Inventory(i))
                Next
                Return lst
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property BaseInventory As List(Of ItemStack)
        Get
            If Inventory IsNot Nothing AndAlso Inventory.Inventory IsNot Nothing Then
                Dim lst As New List(Of ItemStack)
                For i = 0 To 8
                    lst.Add(Inventory.Inventory(i))
                Next
                Return lst
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _lstInnerInventorySelectedIndex As Integer = -1
    Public Property lstInnerInventorySelectedIndex() As Integer
        Get
            Return _lstInnerInventorySelectedIndex
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(_lstInnerInventorySelectedIndex) Then
                _lstInnerInventorySelectedIndex = value
                _lstBaseInventorySelectedIndex = -1
                _lstArmorInventorySelectedIndex = -1
                MyOnPropertyChanged("lstInnerInventorySelectedIndex")
                MyOnPropertyChanged("lstBaseInventorySelectedIndex")
                MyOnPropertyChanged("lstArmorInventorySelectedIndex")
                UpdateSelection()
            End If
        End Set
    End Property

    Private _lstBaseInventorySelectedIndex As Integer = -1
    Public Property lstBaseInventorySelectedIndex() As Integer
        Get
            Return _lstBaseInventorySelectedIndex
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(_lstBaseInventorySelectedIndex) Then
                _lstBaseInventorySelectedIndex = value
                _lstInnerInventorySelectedIndex = -1
                _lstArmorInventorySelectedIndex = -1
                MyOnPropertyChanged("lstArmorInventorySelectedIndex")
                MyOnPropertyChanged("lstInnerInventorySelectedIndex")
                MyOnPropertyChanged("lstBaseInventorySelectedIndex")
                UpdateSelection()
            End If
        End Set
    End Property

    Private _lstArmorInventorySelectedIndex As Integer = -1
    Public Property lstArmorInventorySelectedIndex() As Integer
        Get
            Return _lstArmorInventorySelectedIndex
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(_lstArmorInventorySelectedIndex) Then
                _lstArmorInventorySelectedIndex = value
                _lstInnerInventorySelectedIndex = -1
                _lstBaseInventorySelectedIndex = -1
                MyOnPropertyChanged("Inventory")
                MyOnPropertyChanged("lstArmorInventorySelectedIndex")
                MyOnPropertyChanged("lstInnerInventorySelectedIndex")
                MyOnPropertyChanged("lstBaseInventorySelectedIndex")
                UpdateSelection()
            End If
        End Set
    End Property

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        'Dim newInventory = New PlayerInventory
        'newInventory.Inventory = New List(Of ItemStack) From {New ItemStack With {.Amount = 64, .TypeId = 5}, New ItemStack With {.Amount = 32, .TypeId = 12}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}, New ItemStack With {.Amount = 16, .TypeId = 1}}
        'newInventory.Armor = New PlayerArmor With {.Boots = New ItemStack With {.Amount = 0, .TypeId = 305}, .Chestplate = New ItemStack With {.Amount = 0, .TypeId = 311}, .Helmet = New ItemStack With {.Amount = 64, .TypeId = 314}, .Leggings = New ItemStack With {.Amount = 64, .TypeId = 308}}
        'Me.Inventory = newInventory
    End Sub

    Private Sub MyOnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
