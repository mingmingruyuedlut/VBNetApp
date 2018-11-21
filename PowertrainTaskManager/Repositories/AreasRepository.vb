
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Namespace Repositories
    Public Class AreasRepository
        Dim DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetAreasDataTable() As DataTable
            Dim query = "SELECT * FROM Areas"
            Dim dt = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
            Catch ex As Exception
                Log_Anything("GetAreasDataTable - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Sub InsertArea(areaObj As Areas)
            Dim query = "INSERT INTO Areas (Area_Name) VALUES (@Area_Name)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaObj.AreaName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertArea - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteAllAreas()
            Dim query = "DELETE * FROM Areas"
            DbSqlHelper.ExcuteNonQuery(query)
        End Sub

        Public Function GetListAreas(ByVal dt As DataTable) As List(Of Areas)
            Dim listAreas = New List(Of Areas)
            For i = 0 To dt.Rows.Count - 1
                Dim areas = New Areas
                areas.Id = Integer.Parse(dt.Rows(i)("ID").ToString)
                areas.AreaName = dt.Rows(i)("Area_Name").ToString
                listAreas.Add(areas)
            Next
            Return listAreas
        End Function
    End Class
End Namespace
