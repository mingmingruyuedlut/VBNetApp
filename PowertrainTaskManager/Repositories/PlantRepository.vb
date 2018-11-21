
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Namespace Repositories
    Public Class PlantRepository
        Dim DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetPlantDataTable() As DataTable
            Dim query = "SELECT * FROM Plant"
            Dim dt = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
            Catch ex As Exception
                Log_Anything("GetStationConfigurationBySection - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Sub InsertPlant(plantObj As Plant)
            Dim query = "INSERT INTO Plant (Plant_Name) VALUES (@Plant_Name)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Plant_Name", plantObj.PlantName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertPlant - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteAllPlant()
            Dim query = "DELETE * FROM Plant"
            DbSqlHelper.ExcuteNonQuery(query)
        End Sub

        Public Function GetListPlant(ByVal dt As DataTable) As List(Of Plant)
            Dim listPlanttructure = New List(Of Plant)
            For i = 0 To dt.Rows.Count - 1
                Dim plant = New Plant
                plant.Id = Integer.Parse(dt.Rows(i)("ID").ToString)
                plant.PlantName = dt.Rows(i)("Plant_Name").ToString
                listPlanttructure.Add(plant)
            Next
            Return listPlanttructure
        End Function
    End Class
End Namespace
