Public Class frmIntelliCommand
    Implements ComponentModel.INotifyPropertyChanged

#Region "Constructors"
    Public Sub New(IntelliSenseManager As IntelliSenseManager)
        Me.New(IntelliSenseManager, New CommandList())
        Me.IsEdit = False
    End Sub

    Private _IntelliSenseManager As IntelliSenseManager
    Public Sub New(IntelliSenseManager As IntelliSenseManager, IntelliCommand As CommandList)
        InitializeComponent()
        Me._IntelliSenseManager = IntelliSenseManager
        Me.CurrentCommandList = IntelliCommand
        AddNewCommand.Execute(Nothing)
        AddNewToken.Execute(Nothing)
    End Sub
#End Region

    Private _CurrentCommandList As CommandList
    Public Property CurrentCommandList() As CommandList
        Get
            Return _CurrentCommandList
        End Get
        Set(ByVal value As CommandList)
            If value IsNot _CurrentCommandList Then
                _CurrentCommandList = value
                myPropertyChanged("CurrentCommandList")
            End If
        End Set
    End Property

    Private _IsEdit As Boolean = True
    Public Property IsEdit() As Boolean
        Get
            Return _IsEdit
        End Get
        Set(ByVal value As Boolean)
            _IsEdit = value
        End Set
    End Property

    Private _CurrentCommand As IntelliTextBoxCommand
    Public Property CurrentCommand() As IntelliTextBoxCommand
        Get
            Return _CurrentCommand
        End Get
        Set(ByVal value As IntelliTextBoxCommand)
            If value IsNot _CurrentCommand Then
                _CurrentCommand = value
                myPropertyChanged("CurrentCommand")
                If CommandIsNew Then CommandIsNew = False
                AddNewToken.Execute(Nothing)
            End If
        End Set
    End Property

    Private _CurrentToken As Token
    Public Property CurrentToken() As Token
        Get
            Return _CurrentToken
        End Get
        Set(ByVal value As Token)
            If value IsNot _CurrentToken Then
                _CurrentToken = value
                myPropertyChanged("CurrentToken")
                If TokenIsNew Then TokenIsNew = False
            End If
        End Set
    End Property

    Private _CommandIsNew As Boolean
    Public Property CommandIsNew() As Boolean
        Get
            Return _CommandIsNew
        End Get
        Set(ByVal value As Boolean)
            If Not value = _CommandIsNew Then
                _CommandIsNew = value
                myPropertyChanged("CommandIsNew")
            End If
        End Set
    End Property

    Private _TokenIsNew As Boolean
    Public Property TokenIsNew() As Boolean
        Get
            Return _TokenIsNew
        End Get
        Set(ByVal value As Boolean)
            If Not value = _TokenIsNew Then
                _TokenIsNew = value
                myPropertyChanged("TokenIsNew")
            End If
        End Set
    End Property

#Region "Commands"
    Private _AddNewCommand As RelayCommand
    Public ReadOnly Property AddNewCommand As RelayCommand
        Get
            If _AddNewCommand Is Nothing Then _AddNewCommand = New RelayCommand(Sub(parameter As Object)
                                                                                    CurrentCommand = Nothing
                                                                                    Dim cmd = New IntelliTextBoxCommand()
                                                                                    cmd.Description = New LanguageList()
                                                                                    CurrentCommand = cmd
                                                                                    CommandIsNew = True
                                                                                End Sub)
            Return _AddNewCommand
        End Get
    End Property

    Private _AddNewToken As RelayCommand
    Public ReadOnly Property AddNewToken As RelayCommand
        Get
            If _AddNewToken Is Nothing Then _AddNewToken = New RelayCommand(Sub(parameter As Object)
                                                                                Dim t = New Token()
                                                                                t.CommandText = New LanguageList()
                                                                                CurrentToken = t
                                                                                TokenIsNew = True
                                                                            End Sub)
            Return _AddNewToken
        End Get
    End Property

    Private _AddCommand As RelayCommand
    Public ReadOnly Property AddCommand As RelayCommand
        Get
            If _AddCommand Is Nothing Then _AddCommand = New RelayCommand(Sub()
                                                                              If String.IsNullOrWhiteSpace(CurrentCommand.Command) Then
                                                                                  Dim frm As New frmInfoBox(Application.Current.FindResource("ErrorNoCommand").ToString(), Application.Current.FindResource("Exception").ToString(), Application.Current.FindResource("OK").ToString()) With {.Owner = Me}
                                                                                  frm.ShowDialog()
                                                                                  Return
                                                                              End If
                                                                              If String.IsNullOrWhiteSpace(CurrentCommand.Description.ToString()) Then
                                                                                  Dim frm As New frmInfoBox(Application.Current.FindResource("ErrorNoDescription").ToString(), Application.Current.FindResource("Exception").ToString(), Application.Current.FindResource("OK").ToString()) With {.Owner = Me}
                                                                                  frm.ShowDialog()
                                                                                  Return
                                                                              End If
                                                                              Me.CurrentCommandList.Commands.Add(CurrentCommand)
                                                                              myPropertyChanged("CurrentCommandList")
                                                                              CommandIsNew = False
                                                                          End Sub)
            Return _AddCommand
        End Get
    End Property

    Private _AddToken As RelayCommand
    Public ReadOnly Property AddToken As RelayCommand
        Get
            If _AddToken Is Nothing Then _AddToken = New RelayCommand(Sub()
                                                                          If String.IsNullOrWhiteSpace(CurrentToken.CommandText.ToString()) Then
                                                                              Dim frm As New frmInfoBox(Application.Current.FindResource("ErrorNoDescription").ToString(), Application.Current.FindResource("Exception").ToString(), Application.Current.FindResource("OK").ToString()) With {.Owner = Me}
                                                                              frm.ShowDialog()
                                                                              Return
                                                                          End If
                                                                          If CurrentToken.WhatToUse = UseThis.List AndAlso CurrentToken.LST.Count = 0 AndAlso Not CurrentToken.IsNothing Then
                                                                              Dim frm As New frmInfoBox(Application.Current.FindResource("ErrorListEmpty").ToString(), Application.Current.FindResource("Exception").ToString(), Application.Current.FindResource("OK").ToString()) With {.Owner = Me}
                                                                              frm.ShowDialog()
                                                                              Return
                                                                          End If
                                                                          Me.CurrentCommand.Token.Add(CurrentToken)
                                                                          myPropertyChanged("CurrentCommand")
                                                                          TokenIsNew = False
                                                                      End Sub)
            Return _AddToken
        End Get
    End Property

    Private _WantSave As Boolean = False
    Private _SaveAndCloseCommand As RelayCommand
    Public ReadOnly Property SaveAndClose As RelayCommand
        Get
            If _SaveAndCloseCommand Is Nothing Then _SaveAndCloseCommand = New RelayCommand(Sub()
                                                                                                _WantSave = True
                                                                                                Me.Close()
                                                                                            End Sub)
            Return _SaveAndCloseCommand
        End Get
    End Property

    Private _RemoveCommand As RelayCommand
    Public ReadOnly Property RemoveCommand As RelayCommand
        Get
            If _RemoveCommand Is Nothing Then _RemoveCommand = New RelayCommand(Sub(parameter As Object)
                                                                                    CurrentCommandList.Commands.Remove(CurrentCommand)
                                                                                    AddNewCommand.Execute(Nothing)
                                                                                End Sub)
            Return _RemoveCommand
        End Get
    End Property

    Private _RemoveToken As RelayCommand
    Public ReadOnly Property RemoveToken As RelayCommand
        Get
            If _RemoveToken Is Nothing Then _RemoveToken = New RelayCommand(Sub(parameter As Object)
                                                                                CurrentCommand.Token.Remove(CurrentToken)
                                                                                AddNewToken.Execute(Nothing)
                                                                            End Sub)
            Return _RemoveToken
        End Get
    End Property

    Private _AddItemToCustomList As RelayCommand
    Public ReadOnly Property AddItemToCustomList As RelayCommand
        Get
            If _AddItemToCustomList Is Nothing Then _AddItemToCustomList = New RelayCommand(Sub(parameter As Object)
                                                                                                Dim txt = DirectCast(parameter, TextBox)
                                                                                                If String.IsNullOrWhiteSpace(txt.Text) Then Return
                                                                                                Me.CurrentToken.LST.Add(txt.Text)
                                                                                                txt.Clear()
                                                                                            End Sub)
            Return _AddItemToCustomList
        End Get
    End Property

    Private _RemoveListEntry As RelayCommand
    Public ReadOnly Property RemoveListEntry As RelayCommand
        Get
            _RemoveListEntry = New RelayCommand(Sub(parameter As Object)
                                                    Dim item = parameter.ToString()
                                                    If String.IsNullOrEmpty(item) Then Return
                                                    CurrentToken.LST.Remove(item)
                                                End Sub)
            Return _RemoveListEntry
        End Get
    End Property
#End Region


#Region "INotifyPropertyChanged"
    Private Sub myPropertyChanged(PropertyName As String)
        RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(PropertyName))
    End Sub

    Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region

    Private Sub frmIntelliCommand_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        Dim value = InformationsCheck()
        If value Then
            Dim frm As New frmMessageBox(Application.Current.FindResource("ErrorEnterPluginname").ToString(), Application.Current.FindResource("Exception").ToString(), Application.Current.FindResource("Close").ToString(), Application.Current.FindResource("Cancel").ToString()) With {.Owner = Me}
            If Not frm.ShowDialog() Then
                e.Cancel = True
            End If
        ElseIf Not value Then
            Dim frm As New frmMessageBox(Application.Current.FindResource("ErrorNoCommands").ToString(), Application.Current.FindResource("Exception").ToString(), Application.Current.FindResource("Close").ToString(), Application.Current.FindResource("Cancel").ToString()) With {.Owner = Me}
            If Not frm.ShowDialog() Then
                e.Cancel = True
            End If
        ElseIf Not value.HasValue AndAlso _WantSave Then
            Save()
        ElseIf Not value.HasValue AndAlso Not _WantSave Then
            Dim frm As New frmMessageBox(Application.Current.FindResource("QuestionSaveChanged").ToString(), Application.Current.FindResource("save").ToString())
            If frm.ShowDialog() Then
                Save()
            End If
        End If
        _IntelliSenseManager.Reload()
    End Sub

    Private Sub Save()
        If Not IsEdit Then
            Dim sfg As New Microsoft.Win32.SaveFileDialog()
            With sfg
                .AddExtension = True
                .Filter = String.Format("{0}|*.mcc", Application.Current.FindResource("CommandFile").ToString())
                .OverwritePrompt = True
                .FileName = String.Empty
                .Title = Application.Current.FindResource("SaveCommand").ToString()
                If .ShowDialog() Then
                    CurrentCommandList.Filename = New IO.FileInfo(.FileName)
                Else
                    Return
                End If
            End With
        End If
        CurrentCommandList.Save(CurrentCommandList.Filename.FullName)
    End Sub

    Private Function AllIsOK() As Boolean
        Return Not String.IsNullOrWhiteSpace(CurrentCommandList.PluginName) AndAlso CurrentCommandList.Commands.Count > 0
    End Function

    ''' <returns>True = PluginNameError, False = Cancel</returns>
    Private Function InformationsCheck() As Nullable(Of Boolean)
        If String.IsNullOrWhiteSpace(CurrentCommandList.PluginName) Then
            Return True
        End If
        If CurrentCommandList.Commands IsNot Nothing AndAlso Not CurrentCommandList.Commands.Count > 0 Then
            Return False
        End If
        Return Nothing
    End Function
End Class
