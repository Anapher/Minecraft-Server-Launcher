Imports org.phybros.thrift

Public Class ItemToTextConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim i = DirectCast(value, ItemStack)
        If i Is Nothing Then Return Nothing
        If i.TypeId = 0 Then Return Nothing
        Dim txt As String = String.Empty
        For Each e In ItemData.Instance.lstMinecraftItem
            If e.ID = i.TypeId AndAlso e.Meta = i.Data Then
                If e.Meta > 0 Then
                    txt = String.Format("{0} ({1}:{2})", e.Name, e.ID, e.Meta)
                Else
                    txt = String.Format("{0} ({1})", e.Name, e.ID)
                End If
                Exit For
            End If
        Next
        If String.IsNullOrEmpty(txt) Then
            For Each e In ItemData.Instance.lstMinecraftItem
                If e.ID = i.TypeId Then
                    If e.Meta > 0 Then
                        txt = String.Format("{0} ({1}:{2})", e.Name, e.ID, e.Meta)
                    Else
                        txt = String.Format("{0} ({1})", e.Name, e.ID)
                    End If
                    Exit For
                End If
            Next
            If String.IsNullOrEmpty(txt) Then
                txt = String.Format("{0} ({1})", Application.Current.FindResource("UnknownItem").ToString(), i.TypeId)
            End If
        End If
        'If i.Durability > 0 Then
        'txt &= System.Environment.NewLine & String.Format("{0}: {1}", Application.Current.FindResource("Durability"), i.Durability.ToString())
        'End If
        'If i.Enchantments IsNot Nothing AndAlso i.Enchantments.Count > 0 Then
        'Dim txtenchantments As String = Application.Current.FindResource("enchantments").ToString() & ": "
        'For Each e In i.Enchantments
        'txtenchantments &= System.Environment.NewLine & EnchantmentToString(e.Key) & " " & IntegerToRomanNumber(e.Value)
        'Next
        'txt &= System.Environment.NewLine & txtenchantments
        'End If
        Return txt
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Binding.DoNothing
    End Function

    Private Function IntegerToRomanNumber(i As Integer) As String
        Select Case i
            Case 1
                Return "I"
            Case 2
                Return "II"
            Case 3
                Return "III"
            Case 4
                Return "IV"
            Case 5
                Return "V"
            Case Else
                Return String.Empty
        End Select
    End Function

    Private Function EnchantmentToString(enchantment As Enchantment) As String
        Select Case enchantment
            Case org.phybros.thrift.Enchantment.PROTECTION_FIRE
                Return "Fire Protection"
            Case org.phybros.thrift.Enchantment.PROTECTION_FALL
                Return "Feather Falling"
            Case org.phybros.thrift.Enchantment.PROTECTION_EXPLOSIONS
                Return "Blast Protection"
            Case org.phybros.thrift.Enchantment.PROTECTION_PROJECTILE
                Return "Projectile Protection"
            Case org.phybros.thrift.Enchantment.PROTECTION_ENVIRONMENTAL
                Return "Protection"
            Case org.phybros.thrift.Enchantment.OXYGEN
                Return "Respiration"
            Case org.phybros.thrift.Enchantment.WATER_WORKER
                Return "Aqua Affinity"
            Case org.phybros.thrift.Enchantment.DAMAGE_ALL
                Return "Sharpness"
            Case org.phybros.thrift.Enchantment.DAMAGE_UNDEAD
                Return "Smite"
            Case org.phybros.thrift.Enchantment.DAMAGE_ARTHROPODS
                Return "Bane of Arthropods"
            Case org.phybros.thrift.Enchantment.KNOCKBACK
                Return "Knockback"
            Case org.phybros.thrift.Enchantment.FIRE_ASPECT
                Return "Fire Aspect"
            Case org.phybros.thrift.Enchantment.LOOT_BONUS_MOBS
                Return "Looting"
            Case org.phybros.thrift.Enchantment.DIG_SPEED
                Return "Efficiency"
            Case org.phybros.thrift.Enchantment.SILK_TOUCH
                Return "Silk Touch"
            Case org.phybros.thrift.Enchantment.DURABILITY
                Return "Unbreaking"
            Case org.phybros.thrift.Enchantment.LOOT_BONUS_BLOCKS
                Return "Fortune"
            Case org.phybros.thrift.Enchantment.ARROW_DAMAGE
                Return "Power"
            Case org.phybros.thrift.Enchantment.ARROW_KNOCKBACK
                Return "Punch"
            Case org.phybros.thrift.Enchantment.ARROW_FIRE
                Return "Flame"
            Case org.phybros.thrift.Enchantment.ARROW_INFINITE
                Return "Infinity"
            Case Else
                Return String.Empty
        End Select
    End Function
End Class
