
Namespace Models
    Public Class MasterFile
        Public Property Id As Integer
        Public Property MasterFileName As String
        Public Property MasterFileRevision As String
        Public Property MasterFileApplicationNotes As String
        Public Property OverheadMemoryBase As Integer
        Public Property OverheadMemoryPlus As Integer

        Public Sub New()

        End Sub

        Public Sub New(masterFileName As String, masterFileRevision As String, masterFileApplicationNotes As String, overheadMemoryBase As Integer, overheadMemoryPlus As Integer)
            Me.MasterFileName = masterFileName
            Me.MasterFileRevision = masterFileRevision
            Me.MasterFileApplicationNotes = masterFileApplicationNotes
            Me.OverheadMemoryBase = overheadMemoryBase
            Me.OverheadMemoryPlus = overheadMemoryPlus
        End Sub
    End Class
End Namespace
