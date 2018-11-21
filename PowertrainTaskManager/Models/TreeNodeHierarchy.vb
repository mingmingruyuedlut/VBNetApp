
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Enums

Namespace Models
    Public Class TreeNodeHierarchy
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property StationName As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal area As String)
            AreaName = area
            SectionName = String.Empty
            StationName = String.Empty
        End Sub

        Public Sub New(ByVal area As String, ByVal section As String)
            AreaName = area
            SectionName = section
            StationName = String.Empty
        End Sub

        Public Sub New(ByVal area As String, ByVal section As String, ByVal station As String)
            AreaName = area
            SectionName = section
            StationName = station
        End Sub
    End Class

    Public Class TreeNodeHierarchyValDetail
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property StationName As String
        Public Property TaskName As String
        Public Property MasterFileName As String
        Public Property MasterFileAttributeName As String
        Public Property PlcName As String
        Public Property PlcAttributeName As String
        Public Property TaskInstance As Integer

        Public Sub New()

        End Sub

        Public Sub New(needDefaultValue As Boolean)
            If needDefaultValue Then
                AreaName = DefaultValueConstant.TreeNodeHierarchy
                SectionName = DefaultValueConstant.TreeNodeHierarchy
                StationName = DefaultValueConstant.TreeNodeHierarchy
                TaskName = DefaultValueConstant.TreeNodeHierarchy
                MasterFileName = DefaultValueConstant.TreeNodeHierarchy
                PlcName = DefaultValueConstant.TreeNodeHierarchy
                PlcAttributeName = DefaultValueConstant.TreeNodeHierarchy
                TaskInstance = 0
                MasterFileAttributeName = DefaultValueConstant.TreeNodeHierarchy
            End If
            
        End Sub

        Public Sub New(Optional area As String = "Default", Optional section As String = "Default", Optional station As String = "Default", Optional task As String = "Default", Optional masterFile As String = "Default", Optional plc As String = "Default", Optional plcAttribute As String = "Default", Optional taskInstance As Integer = 0, Optional masterFileAttribute As String = "Default")
            AreaName = area
            SectionName = section
            StationName = station
            TaskName = task
            MasterFileName = masterFile
            MasterFileAttributeName = masterFileAttribute
            PlcName = plc
            PlcAttributeName = plcAttribute
            Me.TaskInstance = taskInstance
        End Sub
    End Class

    Public Class TreeNodeHierarchyValidation
        Public Property TreType As TreeType
        Public Property TreeNodeHierType As TreeNodeHierarchyType
        Public Property TreeNodeHierValDetail As TreeNodeHierarchyValDetail

        Public Sub New()

        End Sub

        Public Sub New(treType As TreeType, treeNodeHierType As TreeNodeHierarchyType, treeNodeHierValDetail As TreeNodeHierarchyValDetail)
            Me.TreType = treType
            Me.TreeNodeHierType = treeNodeHierType
            Me.TreeNodeHierValDetail = treeNodeHierValDetail
        End Sub
    End Class
End Namespace
