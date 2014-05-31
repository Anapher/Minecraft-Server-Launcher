Imports System.Text
Imports org.phybros.thrift
Imports Thrift.Protocol
Imports Thrift.Transport

Public Class ThriftAPI
    Implements IDisposable
    Private socket As TSocket
    Private protocol As TBinaryProtocol
    Private client As SwiftApi.Client
    Private _username, _password, _salt, _ip As String
    Private _port As Integer
#Region "Properties"
    Private _Functions As ThriftAPIFunctions
    Public ReadOnly Property Functions As ThriftAPIFunctions
        Get
            Return _Functions
        End Get
    End Property
#End Region

    Public Sub New(username As String, password As String, salt As String, IP As String, port As Integer)
        _username = username
        _password = password
        _salt = salt
        _ip = IP
        _port = port
    End Sub

    Public Sub Start()
        socket = New TSocket(_ip, _port)
        socket.Open()
        protocol = New TBinaryProtocol(New TFramedTransport(socket))
        client = New SwiftApi.Client(protocol)
        _Functions = New ThriftAPIFunctions(client, AddressOf getAuthString)
    End Sub

    Public Sub [Stop]()
        If socket IsNot Nothing Then
            socket.Close()
        End If
    End Sub

    Public Function getAuthString(ByVal MethodName As String) As String
        Dim PreHash As String = _username & MethodName & _password & _salt
        Return GetSHA256String(PreHash)
    End Function

    Public Function GetSHA256String(ByVal data As String) As String
        Dim sh As New System.Security.Cryptography.SHA256Managed()
        Dim result() As Byte = sh.ComputeHash(Encoding.UTF8.GetBytes(data))
        Dim sb As New StringBuilder()

        For i As Integer = 0 To result.Length - 1
            sb.Append(result(i).ToString("x2"))
        Next

        Return sb.ToString().ToLower()
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' So ermitteln Sie überflüssige Aufrufe

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: Verwalteten Zustand löschen (verwaltete Objekte).
                If socket IsNot Nothing Then socket.Dispose()
                If Me.client IsNot Nothing Then Me.client.Dispose()
                If Me.protocol IsNot Nothing Then Me.protocol.Dispose()
            End If

            ' TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalize() unten überschreiben.
            ' TODO: Große Felder auf NULL festlegen.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: Finalize() nur überschreiben, wenn Dispose(ByVal disposing As Boolean) oben über Code zum Freigeben von nicht verwalteten Ressourcen verfügt.
    'Protected Overrides Sub Finalize()
    '    ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(disposing As Boolean) Bereinigungscode ein.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
