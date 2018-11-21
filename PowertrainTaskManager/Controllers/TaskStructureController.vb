
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories
Namespace Controllers
    Public Class TaskStructureController
        Property TaskStructureControllerRep As TaskStructureRepository

        Public Sub New(ByVal taskStuctureRep As TaskStructureRepository)
            TaskStructureControllerRep = taskStuctureRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            TaskStructureControllerRep = New TaskStructureRepository(dbSqlHelper)
        End Sub

        Public Function GetListTaskStructure(ByVal strTaskConfigName As String) As List(Of TaskStructure)
            Dim taskListStructure
            TaskListStructure = TaskStructureControllerRep.GetListTaskStructure(TaskStructureControllerRep.GetTaskStructureDataTable(strTaskConfigName))
            Return TaskListStructure
        End Function

        Public Function GetListTaskStructure() As List(Of TaskStructure)
            Dim taskListStructure
            taskListStructure = TaskStructureControllerRep.GetListTaskStructure(TaskStructureControllerRep.GetTaskStructureDataTable())
            Return taskListStructure
        End Function

        Public Function GetListString(ByVal listTaskStructure As List(Of TaskStructure)) As List(Of String)
            Dim listString As List(Of String) = New List(Of String)
            For i = 0 To listTaskStructure.Count - 1
                Dim description As String
                If listTaskStructure(i).MemberDescription1 = "" Then
                    description = listTaskStructure(i).MemberName
                Else
                    description = listTaskStructure(i).MemberDescription1
                End If
                If listTaskStructure(i).MemberName = StructureConstant.Space Then
                    listString.Add(OrderChangeConstant.Space)
                Else
                    listString.Add(description)
                End If
            Next
            Return listString
        End Function

        Public Sub DeleteTaskStructureSpace(ByVal strTaskConfigName As String)
            TaskStructureControllerRep.DeleteTaskStructureSpace(strTaskConfigName)
        End Sub

        Public Sub InsertTaskStructureSpace(ByVal strTaskConfigName As String, ByVal parent As String, ByVal order As Integer)
            TaskStructureControllerRep.InsertTaskStructureSpace(strTaskConfigName, parent, order)
        End Sub

        Public Sub UpdateTaskStructureMemberOrderbyMemberName(ByVal memberName As String, ByVal order As Integer, ByVal strTaskConfigName As String)
            TaskStructureControllerRep.UpdateTaskStructureMemberOrderbyMemberName(memberName, order, strTaskConfigName)
        End Sub

        Public Function GetTaskNameByMemberName(ByVal memberName As String, ByVal strTaskConfigName As String) As String
            Dim taskName
            taskName = TaskStructureControllerRep.GetTaskNameByMemberName(memberName, strTaskConfigName)
            Return taskName
        End Function
    End Class
End Namespace

