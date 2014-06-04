Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Controls.Primitives
Imports org.phybros.thrift
Imports System.Collections.ObjectModel
Imports System.Globalization

Public Class IntelliTextBox
    Inherits TextBox

    Public Shared ReadOnly lstCommandsProperty As DependencyProperty = DependencyProperty.Register("lstCommands", GetType(ObservableCollection(Of IntelliTextBoxCommand)), GetType(IntelliTextBox), New PropertyMetadata(New ObservableCollection(Of IntelliTextBoxCommand)))

    Public Property lstCommands As ObservableCollection(Of IntelliTextBoxCommand)
        Get
            Return DirectCast(Me.GetValue(lstCommandsProperty), ObservableCollection(Of IntelliTextBoxCommand))
        End Get
        Set(value As ObservableCollection(Of IntelliTextBoxCommand))
            Me.SetValue(lstCommandsProperty, value)
        End Set
    End Property

    Public Shared ReadOnly IntelliSenseIsEnabledProperty As DependencyProperty = DependencyProperty.Register("IntelliSenseIsEnabled", GetType(Boolean), GetType(IntelliTextBox), New PropertyMetadata(True))

    Public Property IntelliSenseIsEnabled() As Boolean
        Get
            Return DirectCast(Me.GetValue(IntelliSenseIsEnabledProperty), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Me.SetValue(IntelliSenseIsEnabledProperty, value)
        End Set
    End Property

    Public Shared ReadOnly lstPlayersProperty As DependencyProperty = DependencyProperty.Register("Players", GetType(List(Of Player)), GetType(IntelliTextBox), New PropertyMetadata(New List(Of Player)))

    Public Property Players() As List(Of Player)
        Get
            Return DirectCast(Me.GetValue(lstPlayersProperty), List(Of Player))
        End Get
        Set(ByVal value As List(Of Player))
            Me.SetValue(lstPlayersProperty, value)
        End Set
    End Property

    Public Shared ReadOnly lstItemProperty As DependencyProperty = DependencyProperty.Register("Items", GetType(List(Of MinecraftItem)), GetType(IntelliTextBox), New PropertyMetadata(New List(Of MinecraftItem)))

    Public Property Items() As List(Of MinecraftItem)
        Get
            Return DirectCast(Me.GetValue(lstItemProperty), List(Of MinecraftItem))
        End Get
        Set(ByVal value As List(Of MinecraftItem))
            Me.SetValue(lstItemProperty, value)
        End Set
    End Property

    Public Shared ReadOnly lstPluginsProperty As DependencyProperty = DependencyProperty.Register("Plugins", GetType(ObservableCollection(Of Plugin)), GetType(IntelliTextBox), New PropertyMetadata(New ObservableCollection(Of Plugin)))

    Public Property Plugins() As ObservableCollection(Of Plugin)
        Get
            Return DirectCast(Me.GetValue(lstPluginsProperty), ObservableCollection(Of Plugin))
        End Get
        Set(ByVal value As ObservableCollection(Of Plugin))
            Me.SetValue(lstPluginsProperty, value)
        End Set
    End Property

    Public Shared ReadOnly lstBansProperty As DependencyProperty = DependencyProperty.Register("Bans", GetType(ObservableCollection(Of BannedPlayer)), GetType(IntelliTextBox), New PropertyMetadata(New ObservableCollection(Of BannedPlayer)))

    Public Property Bans() As ObservableCollection(Of BannedPlayer)
        Get
            Return DirectCast(Me.GetValue(lstBansProperty), ObservableCollection(Of BannedPlayer))
        End Get
        Set(ByVal value As ObservableCollection(Of BannedPlayer))
            Me.SetValue(lstBansProperty, value)
        End Set
    End Property

    Public Shared ReadOnly pShowInfoBox As DependencyProperty = DependencyProperty.Register("ShowInfoBox", GetType(Boolean), GetType(IntelliTextBox), New PropertyMetadata(True))

    Public Property ShowInfoBox As Boolean
        Get
            Return Boolean.Parse(Me.GetValue(pShowInfoBox).ToString())
        End Get
        Set(value As Boolean)
            Me.SetValue(pShowInfoBox, value)
        End Set
    End Property

    Public Shared ReadOnly pLanguage As DependencyProperty = DependencyProperty.Register("InfoLanguage", GetType(Language), GetType(IntelliTextBox), New PropertyMetadata(New Language With {.Code = "en"}))

    Public Property InfoLanguage As Language
        Get
            Return DirectCast(Me.GetValue(pLanguage), Language)
        End Get
        Set(value As Language)
            Me.SetValue(pLanguage, value)
        End Set
    End Property
#Region "constructure"
    Public Sub New()
        AddHandler Me.Loaded, AddressOf TextBox_Loaded
        AddHandler Application.Current.Deactivated, Sub()
                                                        Me.AssistPopup.IsOpen = False
                                                        Me.InfoPopup.IsOpen = False
                                                    End Sub
        AddHandler Application.Current.Activated, Sub()
                                                      FilterAssistBoxItemsSource()
                                                  End Sub
        AddHandler AssistListBox.SelectionChanged, Sub()
                                                       ResetAssistListBoxLocation()
                                                   End Sub
    End Sub

    Private IsAdded As Boolean = False

    Private Sub TextBox_Loaded(sender As Object, e As RoutedEventArgs)
        If Me.Parent.GetType <> GetType(Grid) Then
            Throw New Exception("Dieses Steuerelement muss in einem Grid sein")
        End If
        If Not IsAdded Then
            TryCast(Me.Parent, Grid).Children.Add(AssistPopup)
            TryCast(Me.Parent, Grid).Children.Add(InfoPopup)
            IsAdded = True

            With AssistListBox
                .MaxHeight = 80
                .MinWidth = 100
                .HorizontalAlignment = Windows.HorizontalAlignment.Left
                .VerticalAlignment = Windows.VerticalAlignment.Top
                AddHandler .MouseDoubleClick, AddressOf AssistListBox_MouseDoubleClick
                AddHandler .PreviewKeyDown, AddressOf AssistListBox_PreviewKeyDown
            End With
            With AssistPopup
                .IsOpen = False
                .AllowsTransparency = True
            End With
            With CommandTextBlock
                .Margin = New Thickness(5, 5, 5, 10)
                .FontSize = 15
                .Text = "op <Player name>"
            End With
            With InfoTextBlock
                .Margin = New Thickness(5)
                .FontSize = 12
                .Text = "Ernnent den angegebenen Spieler zum Operator"
            End With
            With InfoPopup
                .IsOpen = False
                .AllowsTransparency = True
                Dim content = New StackPanel()
                content.Orientation = Orientation.Vertical
                content.Children.Add(CommandTextBlock)
                content.Children.Add(InfoTextBlock)
                Dim grid = New Grid()
                grid.Background = Brushes.WhiteSmoke
                grid.Children.Add(content)
                .Child = grid
                .MinWidth = 200
                .MinHeight = 50
            End With
            With AssistPopup
                .Child = AssistListBox
            End With
            Dim w As Window = Window.GetWindow(Me)
            If w IsNot Nothing Then
                AddHandler w.LocationChanged, Sub(s As Object, args As EventArgs)
                                                  Dim offset = AssistPopup.HorizontalOffset
                                                  AssistPopup.HorizontalOffset = offset + 1
                                                  AssistPopup.HorizontalOffset = offset
                                                  InfoPopup.HorizontalOffset = offset + 1
                                                  InfoPopup.HorizontalOffset = offset
                                                  ResetAssistListBoxLocation()
                                              End Sub
            End If
        End If
    End Sub

    Private Sub AssistListBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.Key
            Case Key.Enter, Key.Tab, Key.Space
                InsertAssistWord()
                e.Handled = True
            Case Key.Back
                Me.Text.Remove(Me.Text.Length - 1, 1)
                Me.Focus()
        End Select
    End Sub

    Private Sub AssistListBox_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        InsertAssistWord()
    End Sub

    Private Function InsertAssistWord() As Boolean
        If Not Me.AssistPopup.IsOpen Then Return False
        Dim isInserted As Boolean = False
        If AssistListBox.SelectedIndex <> -1 Then
            Dim selectedString As String = AssistListBox.SelectedItem.ToString()
            If AddWhiteSpace Then selectedString &= " "
            AddWhiteSpace = False
            Me.InsertText(selectedString)
            isInserted = True
        End If

        AssistPopup.IsOpen = False
        IsAssistKeyPressed = False
        Return isInserted
    End Function
#End Region

#Region "Insert Text"
    Public Sub InsertText(text As String)
        Focus()
        If String.IsNullOrWhiteSpace(text) Then Return
        Dim s = Me.Text.Split({" "}, StringSplitOptions.None)
        If s.Length > 1 Then
            Dim current = s(s.Length - 1)
            Dim oldtextstartindex = Me.Text.Length - current.Length
            Dim oldtextlength = current.Length
            Me.Text = Me.Text.Remove(oldtextstartindex, oldtextlength)
            Me.Text &= text
        Else
            Me.Text = text
        End If
        Me.CaretIndex = Me.Text.Length
    End Sub
#End Region

#Region "Content Assist"
    Private IsAssistKeyPressed As Boolean = False
    Private AssistListBox As New ListBox()
    Private AddWhiteSpace As Boolean = False
    Private AssistPopup As New Popup()
    Private InfoPopup As New Popup()
    Private InfoTextBlock As New TextBlock()
    Private CommandTextBlock As New TextBlock()
    Private _lstLastCommands As New List(Of String)
    Private _currentCommandIndex As Integer = -1

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        ResetAssistListBoxLocation()
    End Sub

    Private _didUseTab As Boolean

    Protected Overrides Sub OnPreviewKeyDown(e As System.Windows.Input.KeyEventArgs)
        If IntelliSenseIsEnabled Then
            If e.Key = Key.Tab Then _didUseTab = True
            Select Case e.Key
                Case Key.Return
                    _lstLastCommands.Add(Me.Text)
                Case Key.Back
                    If Me.Text.Length > 0 Then
                        Dim newText As String
                        newText = Me.Text.Remove(Me.Text.Length - 1, 1)
                        FilterAssistBoxItemsSource(newText)
                        ResetAssistListBoxLocation(newText)
                    Else
                        FilterAssistBoxItemsSource()
                        ResetAssistListBoxLocation()
                    End If
                Case Key.Tab, Key.Space
                    If e.Key = Key.Space Then 'Damit der Befehl nicht zwei mal eingefügt wird
                        For Each c In lstCommands
                            If c.Command.ToLower() = Me.Text.ToLower() Then
                                FilterAssistBoxItemsSource(Me.Text & " ")
                                ResetAssistListBoxLocation(Me.Text & " ")
                                Exit Select
                            End If
                        Next
                        AddWhiteSpace = True
                    End If
                    If InsertAssistWord() Then
                        AssistListBox.SelectedIndex = 0
                        e.Handled = True
                    End If
                    If e.Key = Key.Space Then
                        Dim newText As String
                        newText = Me.Text & " "
                        FilterAssistBoxItemsSource(newText)
                        ResetAssistListBoxLocation(newText)
                    Else
                        FilterAssistBoxItemsSource()
                        ResetAssistListBoxLocation()
                    End If
                Case Key.Down
                    If AssistPopup.IsOpen Then
                        If AssistListBox.Items.Count - 1 > AssistListBox.SelectedIndex Then
                            AssistListBox.SelectedIndex += 1
                            BringSelectionIntoView()
                            RefreshCommandText()
                        End If
                    Else
                        If _lstLastCommands.Count > 0 Then
                            If Not _currentCommandIndex = -1 Then
                                Dim newIndex = _currentCommandIndex + 1
                                If newIndex <= _lstLastCommands.Count - 1 Then
                                    Me.Text = _lstLastCommands(newIndex)
                                    _currentCommandIndex = newIndex
                                Else
                                    Me.Text = String.Empty
                                End If
                            End If
                            Me.CaretIndex = Me.Text.Length
                        End If
                    End If
                Case Key.Up
                    If AssistPopup.IsOpen Then
                        If AssistListBox.SelectedIndex > 0 Then
                            AssistListBox.SelectedIndex -= 1
                            BringSelectionIntoView()
                            RefreshCommandText()
                        End If
                    Else
                        If _lstLastCommands.Count > 0 Then
                            If _currentCommandIndex = -1 Then
                                Dim newIndex = _lstLastCommands.Count - 1
                                _currentCommandIndex = newIndex
                                Me.Text = _lstLastCommands(newIndex)
                            Else
                                Dim newIndex = _currentCommandIndex - 1
                                If newIndex > -1 Then
                                    Me.Text = _lstLastCommands(newIndex)
                                    _currentCommandIndex = newIndex
                                End If
                            End If
                            Me.CaretIndex = Me.Text.Length
                        End If
                    End If
            End Select
        End If
        MyBase.OnPreviewKeyDown(e)
    End Sub

    Private Sub RefreshCommandText()
        If AssistListBox.SelectedItem IsNot Nothing Then
            Dim list = lstCommands.Where(Function(x) x.Command = AssistListBox.SelectedItem.ToString())
            If list.Count = 0 Then
                Dim rootfounded As Boolean = False
                Dim commands = 0
                Dim pluginname As String = String.Empty
                For Each c In lstCommands
                    If c.RootCommand = AssistListBox.SelectedItem.ToString() Then
                        rootfounded = True
                        commands += 1
                        pluginname = c.RootCommand
                    End If
                Next
                If rootfounded Then
                    SetPopupTextToPlugin(pluginname, commands)
                End If
            Else
                RefreshInfoBox(list(0), -1)
            End If
        End If
    End Sub

    Protected Overrides Sub OnTextChanged(e As TextChangedEventArgs)
        MyBase.OnTextChanged(e)
        If String.IsNullOrWhiteSpace(Me.Text) Then
            FilterAssistBoxItemsSource()
        End If
    End Sub

    Private HaveFocus As Boolean = False

    Protected Overrides Sub OnLostFocus(e As RoutedEventArgs)
        MyBase.OnLostFocus(e)
        HaveFocus = False
        If IntelliSenseIsEnabled Then
            FilterAssistBoxItemsSource()
        End If
    End Sub

    Protected Overrides Sub OnGotFocus(e As RoutedEventArgs)
        MyBase.OnGotFocus(e)
        HaveFocus = True
        If IntelliSenseIsEnabled Then
            FilterAssistBoxItemsSource()
        End If
    End Sub

    Private Sub SetPopupTextToPlugin(PluginName As String, FoundedCommands As Integer)
        Dim inlines = CommandTextBlock.Inlines
        inlines.Clear()
        inlines.Add(PluginName)
        InfoTextBlock.Text = String.Format(Application.Current.FindResource("CommandsFound").ToString(), FoundedCommands.ToString())
    End Sub

    Private Sub RefreshInfoBox(command As IntelliTextBoxCommand, index As Integer)
        InfoTextBlock.Text = command.Description.GetText(InfoLanguage.Code)
        Dim inlines = CommandTextBlock.Inlines
        inlines.Clear()

        If index = 0 Then
            inlines.Add(GetInline(command.Command, True))
            For Each t In command.Token
                If t.CommandText IsNot Nothing Then
                    inlines.Add(" " & t.CommandText.GetText(InfoLanguage.Code))
                End If
            Next
        ElseIf index = -1 Then
            Dim s = command.Command
            For Each t In command.Token
                If t.CommandText IsNot Nothing Then
                    s &= " " & t.CommandText.GetText(InfoLanguage.Code)
                End If
            Next
            inlines.Add(s)
        Else
            inlines.Add(command.Command)
            For Each t In command.Token
                Dim txt = " " & t.CommandText.GetText(InfoLanguage.Code)
                If command.Token.IndexOf(t) = index - 1 Then
                    inlines.Add(GetInline(txt, True))
                Else
                    inlines.Add(txt)
                End If
            Next
        End If
    End Sub

    Private Function GetInline(text As String, bold As Boolean) As Inline
        If bold Then
            Dim b = New Bold
            b.Inlines.Add(text)
            Return b
        Else
            Dim s = New Span
            s.Inlines.Add(text)
            Return s
        End If
    End Function

    Private Sub FilterAssistBoxItemsSource(Optional s As String = Nothing)
        If s Is Nothing Then s = Me.Text
        If _didUseTab Then _didUseTab = False : Return
        If lstCommands Is Nothing Then Return
        If Not HaveFocus Then
            AssistPopup.IsOpen = False
            InfoPopup.IsOpen = False
            Return
        End If
        Dim tokens As String() = s.Split({" "}, StringSplitOptions.RemoveEmptyEntries)
        Dim temp As New List(Of String)
        Dim GotCommand As Boolean = False

        If tokens.Length > 1 OrElse (s.EndsWith(" ") AndAlso tokens.Length > 0) Then
            Dim IsAPlugin As Boolean = False
            For Each i In Me.lstCommands
                If i.IsPluginCommandList AndAlso tokens(0).ToLower() = i.RootCommand.ToLower() Then
                    IsAPlugin = True : Exit For
                End If
            Next
            If IsAPlugin AndAlso ((tokens.Length = 2 AndAlso Not s.EndsWith(" ")) OrElse (tokens.Length = 1 AndAlso s.EndsWith(" "))) Then
                For Each i In Me.lstCommands
                    If (i.IsPluginCommandList AndAlso tokens(0).ToLower() = i.RootCommand.ToLower()) Then
                        If tokens.Length = 1 Then
                            temp.Add(i.Command)
                        ElseIf i.Command.ToLower().StartsWith(tokens(1).ToLower()) Then
                            temp.Add(i.Command)
                        End If
                    End If
                Next
                AssistListBox.ItemsSource = temp
                If temp.Count > 0 Then
                    AssistListBox.SelectedIndex = 0
                End If
                RefreshCommandText()
            Else
                Dim c As IntelliTextBoxCommand = Nothing
                For Each ct In lstCommands
                    If ct.IsPluginCommandList Then
                        If ct.RootCommand.ToLower() = tokens(0).ToLower() AndAlso ct.Command.ToLower() = tokens(1).ToLower() Then
                            c = ct
                            Exit For
                        End If
                    Else
                        If ct.Command.ToLower() = tokens(0).ToLower() Then c = ct : Exit For
                    End If
                Next
                If c IsNot Nothing Then
                    If c.IsPluginCommandList Then
                        If (tokens.Length = 2 AndAlso Not s.EndsWith(" ")) OrElse (tokens.Length = 1 AndAlso s.EndsWith(" ")) Then
                            For Each i In Me.lstCommands
                                If i.RootCommand = c.RootCommand Then
                                    If s.EndsWith(" ") Then
                                        temp.Add(i.Command)
                                    Else
                                        If i.Command.ToLower().StartsWith(tokens(1).ToLower()) Then temp.Add(i.Command)
                                    End If
                                End If
                            Next
                            GotCommand = True
                        Else
                            Dim index = tokens.Length - 2 'Minus den 0-Basierten ausgleichswert und Minus den das Wort "Dynmap"
                            If s.EndsWith(" ") Then index += 1 'Wenn der Text mit einem Leerzeichen endet, sollen die verfügbaren Commands angezeigt werden
                            RefreshInfoBox(c, index) 'Erstmal die InfoBox aktualisieren
                            Dim commandtokens = c.Token 'Erstmal die Tokens in ne neue Variable hauen
                            Dim bool = False
                            If IsAPlugin Then
                                bool = commandtokens.Count > tokens.Length - 2
                            Else
                                bool = commandtokens.Count >= tokens.Length - 2
                            End If
                            If bool Then 'Wenn der Command genug Tokens hat...
                                Dim currentToken As Token 'deklarieren wir erstmal eine Variable, die diesen Token dann beinhalten wird
                                If commandtokens.Count = 1 Then
                                    currentToken = commandtokens(0)
                                Else
                                    If s.EndsWith(" ") Then
                                        currentToken = commandtokens(tokens.Length - 1)
                                    Else
                                        currentToken = commandtokens(tokens.Length - 2)
                                    End If
                                End If

                                If Not currentToken.IsNothing Then
                                    Dim currenttokenText = tokens(tokens.Length - 1).ToLower()
                                    temp = GetParameters(s, currentToken, currenttokenText)
                                Else
                                End If
                            End If
                            GotCommand = True
                        End If
                    Else
                        Dim ccount = c.Token.Count + 1
                        If s.EndsWith(" ") Then ccount -= 1
                        If ccount > 0 Then
                            If ccount < tokens.Length Then
                                Return
                            End If
                        Else
                            Return
                        End If

                        If c IsNot Nothing OrElse (c IsNot Nothing AndAlso Not c.Token.Count < tokens.Length - 1 AndAlso Not s.EndsWith(" ")) Then
                            Dim index = tokens.Length - 1
                            If s.EndsWith(" ") Then index += 1
                            GotCommand = True
                            RefreshInfoBox(c, index)
                            Dim commandtokens = c.Token
                            If commandtokens.Count >= tokens.Length - 1 Then
                                Dim currentToken As Token
                                If commandtokens.Count = 1 Then
                                    currentToken = commandtokens(0)
                                Else
                                    If s.EndsWith(" ") Then
                                        currentToken = commandtokens(tokens.Length - 1)
                                    Else
                                        currentToken = commandtokens(tokens.Length - 2)
                                    End If
                                End If
                                If Not currentToken.IsNothing Then
                                    Dim currenttokenText = tokens(tokens.Length - 1).ToLower()
                                    temp = GetParameters(s, currentToken, currenttokenText)
                                Else
                                End If
                            End If
                        End If
                    End If

                End If

                AssistListBox.ItemsSource = temp
            End If

        Else
            If tokens.Length > 0 Then
                For Each c In lstCommands
                    Dim stringtotest = ""
                    If c.IsPluginCommandList Then
                        stringtotest = c.RootCommand
                    Else
                        stringtotest = c.Command
                    End If
                    If stringtotest.ToLower().StartsWith(tokens(0).ToLower()) AndAlso Not temp.Contains(stringtotest) Then
                        temp.Add(stringtotest)
                        GotCommand = True
                    End If
                Next
                AssistListBox.ItemsSource = temp
                If temp.Count > 0 Then
                    AssistListBox.SelectedIndex = 0
                End If
                RefreshCommandText()
            End If
        End If

        AssistListBox.SelectedIndex = 0
        If Me.ShowInfoBox Then
            InfoPopup.IsOpen = Not String.IsNullOrWhiteSpace(Me.Text) AndAlso GotCommand
        End If

        If temp.Count() = 0 Then
            AssistPopup.IsOpen = False
        Else
            AssistPopup.IsOpen = True
            InfoPopup.IsOpen = Me.ShowInfoBox
            Dim txt = (s.Split(" "c)(s.Split(" "c).Length - 1)).ToLower()
            For i = 0 To AssistListBox.Items.Count - 1
                If txt = AssistListBox.Items(i).ToString().ToLower() Then
                    AssistListBox.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Function GetParameters(s As String, currenttoken As Token, currenttokenText As String) As List(Of String)
        Dim temp = New List(Of String)
        Select Case currenttoken.WhatToUse
            Case UseThis.List
                For Each t In currenttoken.LST
                    If t.ToLower.StartsWith(currenttokenText) AndAlso t.ToLower() <> currenttokenText OrElse s.EndsWith(" ") Then
                        temp.Add(t)
                    End If
                Next
            Case UseThis.Playerlist
                For Each p In Me.Players
                    temp.Add(p.Name)
                Next
            Case UseThis.ItemList
                For Each i In Me.Items
                    If i.Name.ToLower().StartsWith(currenttokenText) OrElse s.EndsWith(" ") Then
                        temp.Add(i.Name.Replace(" "c, "_"c))
                    End If
                Next
            Case UseThis.PluginList
                If Me.Plugins IsNot Nothing Then
                    For Each p In Me.Plugins
                        If p.Name.ToLower().StartsWith(currenttokenText) OrElse s.EndsWith(" ") Then
                            temp.Add(p.Name)
                        End If
                    Next
                End If
            Case UseThis.BanList
                For Each bp In Me.Bans
                    If Not bp.IPIsBanned AndAlso bp.name.ToLower().StartsWith(currenttokenText) OrElse s.EndsWith(" ") Then
                        temp.Add(bp.name)
                    End If
                Next
            Case UseThis.BanIPList
                For Each bp In Me.Bans
                    If bp.IPIsBanned AndAlso bp.name.ToLower().StartsWith(currenttokenText) OrElse s.EndsWith(" ") Then
                        temp.Add(bp.name)
                    End If
                Next
            Case UseThis.CommandList
                For Each c In Me.lstCommands
                    temp.Add(c.Command)
                Next
        End Select
        Return temp
    End Function

    Protected Overrides Sub OnTextInput(e As System.Windows.Input.TextCompositionEventArgs)
        MyBase.OnTextInput(e)
        If Not IntelliSenseIsEnabled Then Return
        If IsAssistKeyPressed = False AndAlso e.Text.Length = 1 Then
            FilterAssistBoxItemsSource()
            ResetAssistListBoxLocation()
            IsAssistKeyPressed = True
            Return
        End If

        If IsAssistKeyPressed Then
            FilterAssistBoxItemsSource()
        End If
    End Sub

    Private Sub BringSelectionIntoView()
        Dim selector As Selector = TryCast(AssistListBox, Selector)
        If TypeOf selector Is ListBox Then
            TryCast(selector, ListBox).ScrollIntoView(selector.SelectedItem)
        End If
    End Sub

    Private Sub ResetAssistListBoxLocation(Optional s As String = Nothing)
        If String.IsNullOrEmpty(s) Then s = Me.Text
        Dim rect As Rect = Me.GetRectFromCharacterIndex(Me.Text.Length)
        Dim left As Double = If(rect.X >= 20, rect.X, 20)
        Dim top As Double = If(rect.Y >= 20, rect.Y + 20, 10)
        left += Me.Padding.Left
        top += Me.Padding.Top
        AssistPopup.HorizontalOffset = left
        AssistPopup.VerticalOffset = -10

        InfoPopup.HorizontalOffset = left + AssistListBox.ActualWidth + 20
        InfoPopup.VerticalOffset = -5
    End Sub
#End Region
End Class