
Namespace Models
    Public Class MasterFilesTask
        Public Property Id As Integer
        Public Property MasterFileName As String
        Public Property TaskName As String
        Public Property MemoryUsed As Integer
        Public Property Version As Integer
        Public Property MultiStation As Integer
        Public Property MaxNoOfInstances As Integer
        Public Property ModelAffiliation As String
        Public Property L5XFileName As String

        Public Sub New(ByVal masterFileName As String, ByVal taskName As String, ByVal memoryUsed As Integer, ByVal version As Integer, ByVal multiStation As Integer, ByVal maxNoOfInstances As Integer, ByVal modelAffiliation As String, ByVal l5XFileName As String)
            Me.MasterFileName = masterFileName
            Me.TaskName = taskName
            Me.MemoryUsed = memoryUsed
            Me.Version = version
            Me.MultiStation = multiStation
            Me.MaxNoOfInstances = maxNoOfInstances
            Me.ModelAffiliation = modelAffiliation
            Me.L5XFileName = l5XFileName
        End Sub
        Public Sub New()

        End Sub
    End Class
End Namespace
