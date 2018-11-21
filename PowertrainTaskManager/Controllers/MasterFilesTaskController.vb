
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class MasterFilesTaskController
        Property MasterFileTasksRep As MasterFilesTaskRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal areaConfigRep As MasterFilesTaskRepository)
            MasterFileTasksRep = AreaConfigRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            MasterFileTasksRep = New MasterFilesTaskRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            MasterFileTasksRep = New MasterFilesTaskRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        'It's for master file db
        Public Sub InitMaster()
            'DBConnectionString = MasterConnectionString: This is the prefix part of the whole db connection string
            Dim dbConnStr As String = My.Settings.DBConnectionString & My.Settings.DatabasePath & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            MasterFileTasksRep = New MasterFilesTaskRepository(dbSqlHelper)
        End Sub

        Public Function GetMasterFilesTaskByStation(ByVal masterFilesTask As String) As List(Of MasterFilesTask)
            Dim masterFilesTaskList = MasterFileTasksRep.GetMasterfilesTaskByMasterFileName(masterFilesTask)
            Return MasterFilesTaskList
        End Function

        Public Function GetMasterFileTasksByMfNameAndMultiStation(mfName As String, multiStation As Integer) As List(Of MasterFilesTask)
            Dim mfTaskList = MasterFileTasksRep.GetMasterFileTasksByMfNameAndMultiStation(mfName, multiStation)
            Return mfTaskList
        End Function

        Public Function GetMasterFileTasks() As List(Of MasterFilesTask)
            Dim masterFileTaskList = MasterFileTasksRep.GetMasterfileTasks()
            Return masterFileTaskList
        End Function

    End Class
End Namespace
