Imports System.Collections.ObjectModel
Imports System.IO
Imports org.phybros.thrift

Public Class IntelliSenseManager
    Inherits PropertyChangedBase

    Private _lstCommands As ObservableCollection(Of IntelliTextBoxCommand)
    Public Property lstCommands() As ObservableCollection(Of IntelliTextBoxCommand)
        Get
            Return _lstCommands
        End Get
        Set(ByVal value As ObservableCollection(Of IntelliTextBoxCommand))
            SetProperty(value, _lstCommands)
        End Set
    End Property

    Private _Commands As ObservableCollection(Of CommandList)
    Public Property Commands() As ObservableCollection(Of CommandList)
        Get
            Return _Commands
        End Get
        Set(ByVal value As ObservableCollection(Of CommandList))
            SetProperty(value, _Commands)
        End Set
    End Property

    Private _CommandFolder As DirectoryInfo
    Public ReadOnly Property CommandFolder As DirectoryInfo
        Get
            Return _CommandFolder
        End Get
    End Property

    Public Sub New(CommandFolder As DirectoryInfo)
        _CommandFolder = CommandFolder
        Commands = New ObservableCollection(Of CommandList)
        _lstCommands = New ObservableCollection(Of IntelliTextBoxCommand)
        If Not CommandFolder.Exists Then Return
        For Each fi In CommandFolder.GetFiles("*.mcc")
            Dim cmd As CommandList
            Try
                cmd = CommandList.Load(fi.FullName)
            Catch ex As Exception
                Continue For
            End Try
            If cmd.PluginName = "Server" Then
                cmd.Filename = fi
                Import(cmd)
            End If
            Application.Current.Dispatcher.Invoke(Sub() Commands.Add(cmd))
        Next
    End Sub

    Private _lstPlugins As ObservableCollection(Of Plugin)

    Public Sub Load(lstPlugins As ObservableCollection(Of Plugin))
        If Not CommandFolder.Exists Then Return
        If lstCommands IsNot Nothing Then lstCommands.Clear()
        If Commands.Count > 0 Then Application.Current.Dispatcher.Invoke(Sub() Commands.Clear())
        _lstPlugins = lstPlugins
        For Each fi In CommandFolder.GetFiles("*.mcc")
            Dim cmd As CommandList
            Try
                cmd = CommandList.Load(fi.FullName)
            Catch ex As Exception
                Continue For
            End Try
            cmd.Filename = fi
            Import(cmd)
            Application.Current.Dispatcher.Invoke(Sub() Commands.Add(cmd))
        Next
    End Sub

    Public Sub Reload()
        Load(_lstPlugins)
    End Sub

    Private Sub AddlistToCommands(lst As ObservableCollection(Of IntelliTextBoxCommand))
        For Each i In lst
            Me.lstCommands.Add(i)
        Next
    End Sub

    Private _RemoveCommand As RelayCommand
    Public ReadOnly Property RemoveCommand As RelayCommand
        Get
            If _RemoveCommand Is Nothing Then _RemoveCommand = New RelayCommand(Sub(parameter As Object)
                                                                                    Remove(DirectCast(parameter, CommandList))
                                                                                End Sub)
            Return _RemoveCommand
        End Get
    End Property

    Public Property CurrentWindow As Window

    Private _ImportCommand As RelayCommand
    Public ReadOnly Property ImportCommand As RelayCommand
        Get
            If _ImportCommand Is Nothing Then _ImportCommand = New RelayCommand(Sub()
                                                                                    Dim ofd As New Microsoft.Win32.OpenFileDialog()
                                                                                    With ofd
                                                                                        .Filter = String.Format("{0}|*.mcc|{1}|*.*", Application.Current.FindResource("CommandFile").ToString(), Application.Current.FindResource("Allfiles").ToString())
                                                                                        .FileName = String.Empty
                                                                                        .CheckPathExists = True
                                                                                        .CheckFileExists = True
                                                                                        .Title = Application.Current.FindResource("SelectCommandfile").ToString()
                                                                                        If .ShowDialog(CurrentWindow) Then
                                                                                            Dim fi = New FileInfo(.FileName)
                                                                                            Dim newpath = New FileInfo(Path.Combine(CommandFolder.FullName, fi.Name))
                                                                                            If newpath.Exists Then Return
                                                                                            fi.CopyTo(newpath.FullName)
                                                                                            Dim cmd As CommandList
                                                                                            Try
                                                                                                cmd = CommandList.Load(newpath.FullName)
                                                                                            Catch ex As Exception
                                                                                                Dim infobox As New frmInfoBox(Application.Current.FindResource("ErrorOnImportCommand").ToString(), Application.Current.FindResource("Exception").ToString(), Application.Current.FindResource("OK").ToString())
                                                                                                Return
                                                                                            End Try
                                                                                            cmd.Filename = fi
                                                                                            Import(cmd)
                                                                                        End If
                                                                                    End With
                                                                                End Sub)
            Return _ImportCommand
        End Get
    End Property

    Private Sub Import(cmd As CommandList)
        If cmd.PluginName = "Server" Then
            AddlistToCommands(cmd.Commands)
        Else
            If _lstPlugins IsNot Nothing Then
                For Each p In _lstPlugins
                    If p.Name.ToLower() = cmd.PluginName.ToLower() Then
                        For Each c In cmd.Commands
                            c.IsPluginCommandList = True
                            c.RootCommand = cmd.PluginName
                        Next
                        AddlistToCommands(cmd.Commands)
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub Remove(command As CommandList)
        If Commands.Contains(command) Then Commands.Remove(command)
        For Each c In command.Commands
            If lstCommands.Contains(c) Then lstCommands.Remove(c)
        Next
        If command.Filename.Exists Then command.Filename.Delete()
    End Sub
End Class
