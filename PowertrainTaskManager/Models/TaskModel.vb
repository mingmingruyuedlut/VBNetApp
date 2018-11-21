
Namespace Models
    Public Class TaskModel
        Public Property Id As Integer
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property StationName As String
        Public Property MasterFileName As String
        Public Property TaskName As String
        Public Property TaskMemory As String
        Public Property TaskMemoryPlus As Integer
        Public Property TaskNodes As Integer
        Public Property TaskConnection As Integer
        Public Property MaxNoOfInstances As Integer
        Public Property ModelAffiliation As String

        Public Sub New()

        End Sub
    End Class
End Namespace
