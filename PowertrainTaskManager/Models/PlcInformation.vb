
Namespace Models
    Public Class PlcInformation
        Public Property ProcessorType As String
        Public Property ApplicationNotes As String
        Public Property TotalBytesAvailable As Integer
        Public Property MaxNodes As Integer
        Public Property MaxConnections As Integer

        Public Sub New()
        End Sub

        Public Sub New(ByVal processorType As String, ByVal applicationNotes As String, ByVal totalBytesAvailable As Integer, ByVal maxNodes As Integer, ByVal maxConnections As Integer)
            Me.ProcessorType = processorType
            Me.ApplicationNotes = applicationNotes
            Me.TotalBytesAvailable = totalBytesAvailable
            Me.MaxNodes = maxNodes
            Me.MaxConnections = maxConnections
        End Sub
    End Class

  
End Namespace