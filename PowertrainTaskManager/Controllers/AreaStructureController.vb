
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories
Namespace Controllers
    Public Class AreaStructureController
        Property AreaStructureControllerRep As AreaStructureRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal areaStuctureRep As AreaStructureRepository)
            AreaStructureControllerRep = areaStuctureRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            AreaStructureControllerRep = New AreaStructureRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            AreaStructureControllerRep = New AreaStructureRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            AreaStructureControllerRep = New AreaStructureRepository(dbSqlHelper)
        End Sub

        Public Function GetListAreaStructure(ByVal strAreaMemberName As String) As List(Of AreaStructure)
            Dim areaListStructure
            areaListStructure = AreaStructureControllerRep.GetListAreaStructure(AreaStructureControllerRep.GetAreaStructureDataTable(strAreaMemberName))
            Return areaListStructure
        End Function

        Public Function GetListAreaStructure() As List(Of AreaStructure)
            Dim areaListStructure
            areaListStructure = AreaStructureControllerRep.GetListAreaStructure(AreaStructureControllerRep.GetAreaStructureDataTable())
            Return areaListStructure
        End Function

    End Class
End Namespace

