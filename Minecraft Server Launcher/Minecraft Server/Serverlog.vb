Public Class Serverlog
    Private _Datum As Date
    Public Property Datum() As Date
        Get
            Return _Datum
        End Get
        Set(ByVal value As Date)
            _Datum = value
        End Set
    End Property

    Private _Log As String
    Public Property Log() As String
        Get
            Return _Log
        End Get
        Set(ByVal value As String)
            _Log = value
        End Set
    End Property

    Public ReadOnly Property DateToString As String
        Get
            Return Datum.ToString("dd.MM.yyyy")
        End Get
    End Property

    Private _Number As String
    Public Property Number() As String
        Get
            Return _Number
        End Get
        Set(ByVal value As String)
            _Number = value
        End Set
    End Property


    Public Sub New(Datum As Date, log As String, Number As String)
        Me.Datum = Datum
        Me.Log = log
        Me.Number = Number
    End Sub
End Class

Public Class ServerlogNode
    Private _Datum As Date
    Public Property Datum() As Date
        Get
            Return _Datum
        End Get
        Set(ByVal value As Date)
            _Datum = value
        End Set
    End Property

    Public ReadOnly Property DateToString As String
        Get
            Return Datum.ToString("dd.MM.yyyy")
        End Get
    End Property

    Private _Serverlogs As List(Of Serverlog)
    Public Property Serverlogs() As List(Of Serverlog)
        Get
            Return _Serverlogs
        End Get
        Set(ByVal value As List(Of Serverlog))
            _Serverlogs = value
        End Set
    End Property

    Public Property Log() As String
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)
        End Set
    End Property


    Public Sub New(Datum As Date)
        Me.Datum = Datum
        Me._Serverlogs = New List(Of Serverlog)
    End Sub
End Class