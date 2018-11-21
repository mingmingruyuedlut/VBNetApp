
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class AreaConfigurationRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetAreaConfigurationByStation(ByVal strAreaName As String) As List(Of AreaConfiguration)
            Dim areaConfigurationList = New List(Of AreaConfiguration)()
            Dim query As String = "SELECT * FROM Areas_Configuration WHERE Area_Name = @Area_Name ORDER BY Model_Number"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", strAreaName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                areaConfigurationList = ConvertDbTableToAreaConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetAreaConfigurationByStation - " & GetExceptionInfo(ex))
            End Try

            Return areaConfigurationList
        End Function

        Public Sub UpdateAreaConfiguration(ByVal strAreaName As String, ByVal strModelNumber As String, ByVal strModelName As String)
            Dim query As String = "Update Areas_Configuration set Model_Name = @Model_Name WHERE Area_Name = @Area_Name and Model_Number = @Model_Number"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Model_Name", strModelName))
            params.Add(New OleDbParameter("@Area_Name", strAreaName))
            params.Add(New OleDbParameter("@Model_Number", strModelNumber))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateAreaConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateAreaConfiguration(areaName As String, newAreaName As String)
            Dim query = "UPDATE Areas_Configuration SET Area_Name = @New_Area_Name WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Area_Name", newAreaName))
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateAreaConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Function GetAreaConfiguration() As List(Of AreaConfiguration)
            Dim areaConfigurationList = New List(Of AreaConfiguration)()
            Dim query As String = "SELECT * FROM Areas_Configuration ORDER BY Model_Number"
            Try
                Dim dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
                areaConfigurationList = ConvertDbTableToAreaConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetAreaConfiguration - " & GetExceptionInfo(ex))
            End Try

            Return areaConfigurationList
        End Function

        Public Function GetAreaConfiguration(areaName As String) As List(Of AreaConfiguration)
            Dim areaConfigurationList = New List(Of AreaConfiguration)()
            Dim query As String = "SELECT * FROM Areas_Configuration WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataSet(CommandType.Text, query, params).Tables(0)
                areaConfigurationList = ConvertDbTableToAreaConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetAreaConfiguration - " & GetExceptionInfo(ex))
            End Try

            Return areaConfigurationList
        End Function

        Public Sub InsertAreaConfiguration(ByVal listAreaConfiguration As List(Of AreaConfiguration))
            For i = 0 To listAreaConfiguration.Count - 1
                Dim query = "insert into Areas_Configuration (Area_Name,Model_Number,Model_Name) values (@Area_Name,@Model_Number,@Model_Name)"
                Dim params = New List(Of OleDbParameter)()
                params.Add(New OleDbParameter("@Area_Name", listAreaConfiguration(i).AreaName))
                params.Add(New OleDbParameter("@Model_Number", listAreaConfiguration(i).ModelNumber))
                params.Add(New OleDbParameter("@Model_Name", listAreaConfiguration(i).ModelName))
                Try
                    DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
                Catch ex As Exception
                    Log_Anything("InsertArea - " & GetExceptionInfo(ex))
                End Try
            Next
        End Sub
	
	Public Sub InsertAreaConfiguration(ByVal areaConfig As AreaConfiguration)
            Dim query = "INSERT INTO Areas_Configuration (Area_Name,Model_Number,Model_Name) VALUES (@Area_Name,@Model_Number,@Model_Name)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaConfig.AreaName))
            params.Add(New OleDbParameter("@Model_Number", areaConfig.ModelNumber))
            params.Add(New OleDbParameter("@Model_Name", areaConfig.ModelName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertAreaConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteAreaConfiguration(ByVal areaName As String)
            Dim query = "DELETE * FROM Areas_Configuration WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteAreaConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateAreaConfiguration(ByVal areaConfig As AreaConfiguration)
            Dim query = "UPDATE Areas_Configuration WHERE SET Model_Name = @Model_Name WHERE Area_Name = @Area_Name AND Model_Number = @Model_Number"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaConfig.AreaName))
            params.Add(New OleDbParameter("@Model_Number", areaConfig.ModelNumber))
            params.Add(New OleDbParameter("@Model_Name", areaConfig.ModelName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateAreaConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Private Function ConvertDbTableRowToAreaConfiguration(ByVal row As DataRow) As AreaConfiguration
            If row Is Nothing Then
                Return Nothing
            End If

            Dim areaName As String = row(AreasConfigurationColumnConstant.AreaName).ToString()
            Dim modelNumber As String = row(AreasConfigurationColumnConstant.ModelNumber).ToString()
            Dim modelName As String = row(AreasConfigurationColumnConstant.ModelName).ToString()
            Dim areaConfiguration = New AreaConfiguration(areaName, modelNumber, modelName)
            Return areaConfiguration
        End Function

        Private Function ConvertDbTableToAreaConfigurationList(ByVal dt As DataTable) As List(Of AreaConfiguration)
            Dim areaConfigurationList = New List(Of AreaConfiguration)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim areaConfiguration = ConvertDbTableRowToAreaConfiguration(row)
                    areaConfigurationList.Add(areaConfiguration)
                Next
            End If
            Return areaConfigurationList
        End Function
    End Class
End Namespace
