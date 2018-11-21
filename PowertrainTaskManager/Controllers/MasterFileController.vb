
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class MasterFileController
        Property MasterFileRep As MasterFileRepository

        Public Sub New()

        End Sub

        Public Sub New(mfRep As MasterFileRepository)
            MasterFileRep = mfRep
        End Sub

        Public Sub New(dbSqlHelper As SqlHelper)
            MasterFileRep = New MasterFileRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            MasterFileRep = New MasterFileRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        'It's for master file db
        Public Sub InitMaster()
            Dim dbConnStr As String = My.Settings.DBConnectionString & My.Settings.DatabasePath & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            MasterFileRep = New MasterFileRepository(dbSqlHelper)
        End Sub

        Public Function GetMasterFiles() As List(Of MasterFile)
            Dim mfList = MasterFileRep.GetMasterFiles()
            Return mfList
        End Function

        Public Function GetMasterFileByName(mfName As String) As MasterFile
            Dim mf = MasterFileRep.GetMasterFileByName(mfName)
            Return mf
        End Function

    End Class
End Namespace

