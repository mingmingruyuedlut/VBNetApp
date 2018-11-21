
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories
Namespace Controllers
    Public Class PlantController
        Property PlantControllerRep As PlantRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal plantRep As PlantRepository)
            PlantControllerRep = plantRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            PlantControllerRep = New PlantRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            PlantControllerRep = New PlantRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            PlantControllerRep = New PlantRepository(dbSqlHelper)
        End Sub

        Public Function GetListPlant() As List(Of Plant)
            Dim plantList = PlantControllerRep.GetListPlant(PlantControllerRep.GetPlantDataTable())
            Return plantList
        End Function

        Public Sub InsertPlant(plantList As List(Of Plant))
            For Each plantObj In plantList
                InsertPlant(plantObj)
            Next
        End Sub

        Public Sub InsertPlant(plantObj As Plant)
            PlantControllerRep.InsertPlant(plantObj)
        End Sub

        Public Sub DeleteAllPlant()
            PlantControllerRep.DeleteAllPlant()
        End Sub

        Public Function GetPlantName() As String
            Dim name = UiDisplayConstant.PowertrainPlant
            Dim plantObj = GetListPlant().FirstOrDefault()
            If IsNothing(plantObj) Then
                Return name
            End If
            If String.IsNullOrWhiteSpace(plantObj.PlantName) Then
                Return name
            End If
            name = plantObj.PlantName
            Return name
        End Function

    End Class
End Namespace

