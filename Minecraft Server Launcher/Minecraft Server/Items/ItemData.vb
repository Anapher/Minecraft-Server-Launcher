﻿Public Class ItemData
    Private Shared _instance As ItemData
    Public Shared ReadOnly Property Instance As ItemData
        Get
            If _instance Is Nothing Then _instance = New ItemData
            Return _instance
        End Get
    End Property

    Private _MinecraftItems As MinecraftItems
    Public Property lstMinecraftItem As MinecraftItems
        Get
            Return _MinecraftItems
        End Get
        Set(value As MinecraftItems)
            _MinecraftItems = value
        End Set
    End Property

    Public Sub New()
        lstMinecraftItem = MinecraftItems.Load(IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ItemDB.xml"))
    End Sub
End Class