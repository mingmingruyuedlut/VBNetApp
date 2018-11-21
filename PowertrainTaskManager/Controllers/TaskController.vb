
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class TaskController
        Property TasksControllerRep As TaskRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal tasksRep As TaskRepository)
            TasksControllerRep = TasksRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            TasksControllerRep = New TaskRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            TasksControllerRep = New TaskRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            TasksControllerRep = New TaskRepository(dbSqlHelper)
        End Sub

        Public Function GetListTasks() As List(Of TaskModel)
            Dim tasksList
            TasksList = TasksControllerRep.GetListTasks(TasksControllerRep.GetTasks())
            Return TasksList
        End Function

        Public Function GetTaskList(stationTreeNodeHier As TreeNodeHierarchy) As List(Of TaskModel)
            Dim tasksList = TasksControllerRep.GetTaskList(stationTreeNodeHier.AreaName, stationTreeNodeHier.SectionName, stationTreeNodeHier.StationName)
            Return tasksList
        End Function

        Public Sub InsertTask(taskList As List(Of TaskModel))
            For Each taskObj In taskList
                InsertTask(taskObj)
            Next
        End Sub

        Public Sub InsertTask(taskObj As TaskModel)
            TasksControllerRep.InsertTask(taskObj)
        End Sub

        Public Sub DeleteAllTasks()
            TasksControllerRep.DeleteAllTasks()
        End Sub
    End Class
End Namespace

