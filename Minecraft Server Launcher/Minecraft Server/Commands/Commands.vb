Imports System.IO
Imports org.phybros.thrift
Imports System.Collections.ObjectModel

Public Class Commands
    Inherits PropertyChangedBase

    Private di As DirectoryInfo
    Private _lstCommands As ObservableCollection(Of IntelliTextBoxCommand)
    Public Property lstCommands() As ObservableCollection(Of IntelliTextBoxCommand)
        Get
            Return _lstCommands
        End Get
        Set(ByVal value As ObservableCollection(Of IntelliTextBoxCommand))
            SetProperty(value, _lstCommands)
        End Set
    End Property

    Private _AllCommands As ObservableCollection(Of CommandList)
    Public Property AllCommands() As ObservableCollection(Of CommandList)
        Get
            Return _AllCommands
        End Get
        Set(ByVal value As ObservableCollection(Of CommandList))
            SetProperty(value, _AllCommands)
        End Set
    End Property

    Public Sub New()
        _lstCommands = New ObservableCollection(Of IntelliTextBoxCommand)
        _AllCommands = New ObservableCollection(Of CommandList)
        di = New DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MSL", "Commands"))
        If Not di.Exists Then Return
        For Each fi In di.GetFiles("*.mcc")
            Dim cmd As CommandList
            Try
                cmd = CommandList.Load(fi.FullName)
            Catch ex As Exception
                Continue For
            End Try
            If cmd.PluginName = "Server" Then
                AddlistToCommands(cmd.Commands)
                AllCommands.Add(cmd)
                Exit For
            End If
        Next
    End Sub

    Private Sub AddlistToCommands(lst As List(Of IntelliTextBoxCommand))
        For Each i In lst
            Me.lstCommands.Add(i)
        Next
    End Sub

    Public Sub Load(lstPlugins As ObservableCollection(Of Plugin))
        If Not di.Exists Then Return
        If _lstCommands IsNot Nothing Then _lstCommands.Clear()
        If _AllCommands IsNot Nothing Then _AllCommands.Clear()

        For Each fi In di.GetFiles("*.mcc")
            Dim cmd As CommandList
            Try
                cmd = CommandList.Load(fi.FullName)
            Catch ex As Exception
                Continue For
            End Try
            If cmd.PluginName = "Server" Then
                AddlistToCommands(cmd.Commands)
            Else
                For Each p In lstPlugins
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
            _AllCommands.Add(cmd)
        Next
    End Sub
End Class
