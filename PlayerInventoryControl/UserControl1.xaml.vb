Imports org.phybros.thrift
Public Class UserControl1
    Public Property Inventory As PlayerInventory

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Inventory = New PlayerInventory
        Inventory.Inventory = New List(Of ItemStack) From {New ItemStack With {.Amount = 64, .TypeId = 5}, New ItemStack With {.Amount = 32, .TypeId = 12}, New ItemStack With {.Amount = 16, .TypeId = 1}}
    End Sub
End Class
