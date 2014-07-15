Public Class frmIntelliSenseManager
    Private _IntelliSenseManager As IntelliSenseManager
    Public ReadOnly Property IntelliSenseManager As IntelliSenseManager
        Get
            Return _IntelliSenseManager
        End Get
    End Property

    Public Sub New(IntelliSenseManager As IntelliSenseManager)
        _IntelliSenseManager = IntelliSenseManager
        InitializeComponent()
    End Sub

    Private _EditCommand As RelayCommand
    Public ReadOnly Property EditCommand As RelayCommand
        Get
            If _EditCommand Is Nothing Then _EditCommand = New RelayCommand(Sub(parameter As Object)
                                                                                Dim intelliCommand = DirectCast(parameter, CommandList)
                                                                                If intelliCommand Is Nothing Then Return
                                                                                Dim frm As New frmIntelliCommand(IntelliSenseManager, intelliCommand) With {.Owner = Me}
                                                                                frm.ShowDialog()
                                                                            End Sub)
            Return _EditCommand
        End Get
    End Property

    Private _CreateNewCommand As RelayCommand
    Public ReadOnly Property CreateNewCommand As RelayCommand
        Get
            If _CreateNewCommand Is Nothing Then _CreateNewCommand = New RelayCommand(Sub(parameter As Object)
                                                                                          Dim frm As New frmIntelliCommand(IntelliSenseManager) With {.Owner = Me}
                                                                                          frm.ShowDialog()
                                                                                      End Sub)
            Return _CreateNewCommand
        End Get
    End Property
End Class
