
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories
Namespace Controllers
    Public Class AreasController
        Property AreasControllerRep As AreasRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal areaStuctureRep As AreasRepository)
            AreasControllerRep = areaStuctureRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            AreasControllerRep = New AreasRepository(dbSqlHelper)
        End Sub
	
	Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            AreasControllerRep = New AreasRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            AreasControllerRep = New AreasRepository(dbSqlHelper)
        End Sub

        Public Function GetListAreas() As List(Of Areas)
            Dim areaLists
            areaLists = AreasControllerRep.GetListAreas(AreasControllerRep.GetAreasDataTable())
            Return areaLists
        End Function

        Public Sub InsertAreas(ByVal areaList As List(Of Areas))
            For Each areaObj In areaList
                InsertAreas(areaObj)
            Next
        End Sub

        Public Sub InsertAreas(areaObj As Areas)
            AreasControllerRep.InsertArea(areaObj)
        End Sub

        Public Sub DeleteAllAreas()
            AreasControllerRep.DeleteAllAreas()
        End Sub

    End Class
End Namespace

