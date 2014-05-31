<Serializable> _
Public Class IntelliTextBoxCommand
    Private _Token As List(Of Token)
    Public Property Token() As List(Of Token)
        Get
            Return _Token
        End Get
        Set(ByVal value As List(Of Token))
            _Token = value
        End Set
    End Property

    Private _command As String
    Public Property Command() As String
        Get
            Return _command
        End Get
        Set(ByVal value As String)
            _command = value
        End Set
    End Property

    Private _Description As LanguageList
    Public Property Description() As LanguageList
        Get
            Return _Description
        End Get
        Set(ByVal value As LanguageList)
            _Description = value
        End Set
    End Property

    Public Sub New(command As String)
        Token = New List(Of Token)
        Me.Command = command
    End Sub

    Public Sub New()
        Token = New List(Of Token)
    End Sub

    Private _IsPluginCommandList As Boolean
    <Xml.Serialization.XmlIgnore> _
    Public Property IsPluginCommandList() As Boolean
        Get
            Return _IsPluginCommandList
        End Get
        Set(ByVal value As Boolean)
            _IsPluginCommandList = value
        End Set
    End Property

    Private _RootCommand As String
    <Xml.Serialization.XmlIgnore> _
    Public Property RootCommand() As String
        Get
            Return _RootCommand
        End Get
        Set(ByVal value As String)
            _RootCommand = value
        End Set
    End Property

End Class

<Serializable> _
Public Class Token
    Private _CommandText As LanguageList
    Public Property CommandText() As LanguageList
        Get
            Return _CommandText
        End Get
        Set(ByVal value As LanguageList)
            _CommandText = value
        End Set
    End Property

    Private _lst As List(Of String)
    Public Property LST() As List(Of String)
        Get
            Return _lst
        End Get
        Set(ByVal value As List(Of String))
            _lst = value
        End Set
    End Property

    Private _IsNothing As Boolean
    Public Property IsNothing() As Boolean
        Get
            Return _IsNothing
        End Get
        Set(ByVal value As Boolean)
            _IsNothing = value
        End Set
    End Property

    Private _WhatToUse As UseThis
    Public Property WhatToUse() As UseThis
        Get
            Return _WhatToUse
        End Get
        Set(ByVal value As UseThis)
            _WhatToUse = value
        End Set
    End Property

    Public Sub New(Text As List(Of String), IsNothing As Boolean)
        Me._lst = Text
        Me.IsNothing = IsNothing
    End Sub

    Public Sub New(Text As List(Of String))
        Me.New(Text, False)
    End Sub

    Private _Description As KeyValuePair(Of String, String)
    Public Property Description() As KeyValuePair(Of String, String)
        Get
            Return _Description
        End Get
        Set(ByVal value As KeyValuePair(Of String, String))
            _Description = value
        End Set
    End Property

    Public Sub New()
        Me.LST = New List(Of String)
    End Sub
End Class

<Serializable> _
Public Enum UseThis
    List
    Playerlist
    ItemList
    PluginList
    BanList
    BanIPList
    CommandList
End Enum