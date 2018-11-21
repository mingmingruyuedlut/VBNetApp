
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class MemoryUsageController
        Property MemoryUsageRep As MemoryUsageRepository

        Public Sub New()

        End Sub

        Public Sub New(muRep As MemoryUsageRepository)
            MemoryUsageRep = muRep
        End Sub

        Public Sub New(dbSqlHelper As SqlHelper)
            MemoryUsageRep = New MemoryUsageRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            MemoryUsageRep = New MemoryUsageRepository(dbSqlHelper)
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
            MemoryUsageRep = New MemoryUsageRepository(dbSqlHelper)
        End Sub

        Public Function GetMemoryUsages() As List(Of MemoryUsage)
            Dim mfList = MemoryUsageRep.GetMemoryUsages()
            Return mfList
        End Function

        Public Function GetMemoryUsages(treeNodeHier As TreeNodeHierarchy) As List(Of MemoryUsage)
            Dim mfList = MemoryUsageRep.GetMemoryUsages(treeNodeHier)
            Return mfList
        End Function

        Public Function GetMemoryUsageDataTable() As DataTable
            Dim muDataTable = MemoryUsageRep.GetMemoryUsageDataTable()
            Return muDataTable
        End Function

        Public Sub InsertMemoryUsage(mu As MemoryUsage)
            MemoryUsageRep.InsertMemoryUsage(mu)
        End Sub

        Public Sub InsertMemoryUsages(muList As List(Of MemoryUsage))
            For Each mu In muList
                InsertMemoryUsage(mu)
            Next
        End Sub

        Public Sub DeleteMemoryUsages()
            MemoryUsageRep.DeleteMemoryUsages()
        End Sub

    End Class
End Namespace
