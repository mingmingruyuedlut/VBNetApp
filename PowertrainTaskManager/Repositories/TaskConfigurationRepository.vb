
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class TaskConfigurationRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetTaskConfigurationByStation(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String) As List(Of TaskConfiguration)
            Dim taskConfigurationList = New List(Of TaskConfiguration)()
            Dim query As String = "SELECT * FROM Tasks_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", strAreaName))
            params.Add(New OleDbParameter("@Section_Name", strSectionName))
            params.Add(New OleDbParameter("@Station_Name", strStationName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                taskConfigurationList = ConvertDbTableToTaskConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetTaskConfigurationByStation - " & GetExceptionInfo(ex))
            End Try

            Return taskConfigurationList
        End Function

        Public Sub UpdateMemberValue(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMemberName As String, ByVal strTaskName As String, ByVal strMemberValue As String)
            Dim query As String = "update Tasks_Configuration set Member_Value = @Member_Value WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name and Task_Name = @Task_Name and Member_Name = @Member_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Member_Value", strMemberValue))
            params.Add(New OleDbParameter("@Area_Name", strAreaName))
            params.Add(New OleDbParameter("@Section_Name", strSectionName))
            params.Add(New OleDbParameter("@Station_Name", strStationName))
            params.Add(New OleDbParameter("@Task_Name", strTaskName))
            params.Add(New OleDbParameter("@Member_Name", strMemberName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateMemberValue - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Function GetTaskConfiguration() As List(Of TaskConfiguration)
            Dim taskConfigurationList = New List(Of TaskConfiguration)()
            Dim query As String = "SELECT * FROM Tasks_Configuration "

            Try
                Dim dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
                taskConfigurationList = ConvertDbTableToTaskConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetTaskConfiguration - " & GetExceptionInfo(ex))
            End Try

            Return taskConfigurationList
        End Function

        Public Function GetTaskConfiguration(areaName As String, sectionName As String, stationName As String) As List(Of TaskConfiguration)
            Dim taskConfigurationList = New List(Of TaskConfiguration)()
            Dim query As String = "SELECT * FROM Tasks_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                taskConfigurationList = ConvertDbTableToTaskConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetTaskConfiguration - " & GetExceptionInfo(ex))
            End Try

            Return taskConfigurationList
        End Function

        Public Function GetTaskConfiguration(areaName As String, sectionName As String, stationName As String, taskName As String) As List(Of TaskConfiguration)
            Dim taskConfigurationList = New List(Of TaskConfiguration)()
            Dim query As String = "SELECT * FROM Tasks_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name AND Task_Name = @Task_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))
            params.Add(New OleDbParameter("@Task_Name", taskName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                taskConfigurationList = ConvertDbTableToTaskConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetTaskConfiguration - " & GetExceptionInfo(ex))
            End Try

            Return taskConfigurationList
        End Function

        Public Sub InsertTaskConfiguration(ByVal listTaskConfiguration As List(Of TaskConfiguration))
            For i = 0 To listTaskConfiguration.Count - 1
                Dim query = "insert into Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) values (@Area_Name,@Section_Name,@Station_Name,@Task_Name,@Task_Instance,@Member_Name,@Member_Value,@Member_Type,@Base_Tag)"
                Dim params = New List(Of OleDbParameter)()
                params.Add(New OleDbParameter("@Area_Name", listTaskConfiguration(i).AreaName))
                params.Add(New OleDbParameter("@Section_Name", listTaskConfiguration(i).SectionName))
                params.Add(New OleDbParameter("@Station_Name", listTaskConfiguration(i).StationName))
                params.Add(New OleDbParameter("@Task_Name", listTaskConfiguration(i).TaskName))
                params.Add(New OleDbParameter("@Task_Instance", listTaskConfiguration(i).TaskInstance))
                params.Add(New OleDbParameter("@Member_Name", listTaskConfiguration(i).MemberName))
                params.Add(New OleDbParameter("@Member_Value", listTaskConfiguration(i).MemberValue))
                params.Add(New OleDbParameter("@Member_Type", listTaskConfiguration(i).MemberType))
                params.Add(New OleDbParameter("@Base_Tag", listTaskConfiguration(i).BaseTag))
                Try
                    DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
                Catch ex As Exception
                    Log_Anything("InsertTaskConfiguration - " & GetExceptionInfo(ex))
                End Try
            Next
        End Sub

        Public Sub DeleteTaskConfiguration(areaName As String, sectionName As String, stationName As String, taskName As String)
            Dim query = "DELETE * FROM Tasks_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name AND Task_Name = @Task_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))
            params.Add(New OleDbParameter("@Task_Name", taskName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteTaskConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteTaskConfiguration(areaName As String, sectionName As String, stationName As String)
            Dim query = "DELETE * FROM Tasks_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteTaskConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteTaskConfiguration(areaName As String, sectionName As String)
            Dim query = "DELETE * FROM Tasks_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteTaskConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteTaskConfiguration(areaName As String)
            Dim query = "DELETE * FROM Tasks_Configuration WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteTaskConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateTaskConfiguration(areaName As String, newAreaName As String)
            Dim query = "UPDATE Tasks_Configuration SET Area_Name = @New_Area_Name WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Area_Name", newAreaName))
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateTaskConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateTaskConfiguration(areaName As String, sectionName As String, newSectionName As String)
            Dim query = "UPDATE Tasks_Configuration SET Section_Name = @New_Section_Name WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Section_Name", newSectionName))
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateTaskConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateTaskConfiguration(areaName As String, sectionName As String, stationName As String, newStationName As String)
            Dim query = "UPDATE Tasks_Configuration SET Station_Name = @New_Station_Name WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Station_Name", newStationName))
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateTaskConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Private Function ConvertDbTableRowToTaskConfiguration(ByVal row As DataRow) As TaskConfiguration
            If row Is Nothing
                Return Nothing
            End If

            Dim areaName As String = row(TasksConfigurationColumnConstant.AreaName).ToString()
            Dim sectionName As String = row(TasksConfigurationColumnConstant.SectionName).ToString()
            Dim stationName As String = row(TasksConfigurationColumnConstant.StationName).ToString()
            Dim taskName As String = row(TasksConfigurationColumnConstant.TaskName).ToString()
            Dim taskInstance As Integer = Integer.Parse(row(TasksConfigurationColumnConstant.TaskInstance).ToString())
            Dim memberName As String = row(TasksConfigurationColumnConstant.MemberName).ToString()
            Dim memberValue As String = row(TasksConfigurationColumnConstant.MemberValue).ToString()
            Dim memberType As String = row(TasksConfigurationColumnConstant.MemberType).ToString()
            Dim baseTag As String = row(TasksConfigurationColumnConstant.BaseTag).ToString()
            Dim taskConfiguration = New TaskConfiguration(areaName, sectionName, stationName, taskName, taskInstance, memberName, memberValue, memberType, baseTag)
            Return taskConfiguration
        End Function 

        Private Function ConvertDbTableToTaskConfigurationList(ByVal dt As DataTable) As List(Of TaskConfiguration)
            Dim taskConfigurationList = New List(Of TaskConfiguration)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim taskConfiguration = ConvertDbTableRowToTaskConfiguration(row)
                    taskConfigurationList.Add(taskConfiguration)
                Next
            End If
            Return taskConfigurationList
        End Function 
    End Class
End Namespace
