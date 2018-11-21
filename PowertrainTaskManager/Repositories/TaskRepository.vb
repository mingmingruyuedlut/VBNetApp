
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class TaskRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetTasks() As DataTable
            Dim query As String = "select * from Tasks"
            Dim dt = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
            Catch ex As Exception
                Log_Anything("GetTasksByMemberName - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Function GetTaskList(areaName As String, sectionName As String, stationName As String) As List(Of TaskModel)
            Dim taskList = New List(Of TaskModel)
            Dim query = "SELECT * FROM Tasks WHERE  Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))
            Try
                Dim dt = DbSqlHelper.ExcuteDataSet(CommandType.Text, query, params).Tables(0)
                taskList = GetListTasks(dt)
            Catch ex As Exception
                Log_Anything("GetTasksByMemberName - " & GetExceptionInfo(ex))
            End Try
            Return taskList
        End Function

        Public Sub InsertTask(taskObj As TaskModel)
            Dim query = "INSERT INTO Tasks(Area_Name,Section_Name,Station_Name,Task_Name,MasterFile_Name,ModelAffiliation,Task_Memory,Task_MemoryPLUS,Task_Nodes,Task_Connection,MaxNoOfInstances) VALUES (@Area_Name,@Section_Name,@Station_Name,@Task_Name,@MasterFile_Name,@ModelAffiliation,@Task_Memory,@Task_MemoryPLUS,@Task_Nodes,@Task_Connection,@MaxNoOfInstances)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", taskObj.AreaName))
            params.Add(New OleDbParameter("@Section_Name", taskObj.SectionName))
            params.Add(New OleDbParameter("@Station_Name", taskObj.StationName))
            params.Add(New OleDbParameter("@Task_Name", taskObj.TaskName))
            params.Add(New OleDbParameter("@MasterFile_Name", taskObj.MasterFileName))
            params.Add(New OleDbParameter("@ModelAffiliation", taskObj.ModelAffiliation))
            params.Add(New OleDbParameter("@Task_Memory", taskObj.TaskMemory))
            params.Add(New OleDbParameter("@Task_MemoryPLUS", taskObj.TaskMemoryPlus))
            params.Add(New OleDbParameter("@Task_Nodes", taskObj.TaskNodes))
            params.Add(New OleDbParameter("@Task_Connection", taskObj.TaskConnection))
            params.Add(New OleDbParameter("@MaxNoOfInstances", taskObj.MaxNoOfInstances))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertArea - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteAllTasks()
            Dim query = "DELETE * FROM Tasks"
            DbSqlHelper.ExcuteNonQuery(query)
        End Sub

        Public Function GetListTasks(ByVal dt As DataTable) As List(Of TaskModel)
            Dim taskList = New List(Of TaskModel)
            For Each row In dt.Rows
                Dim taskModel = ConvertDbTableRowToTaskModel(row)
                taskList.Add(taskModel)
            Next
            Return taskList
        End Function

        Private Function ConvertDbTableRowToTaskModel(row As DataRow) As TaskModel
            If row Is Nothing Then
                Return Nothing
            End If

            Dim taskModel = New TaskModel
            taskModel.Id = Integer.Parse(row(TasksColumnConstant.Id).ToString())
            taskModel.AreaName = row(TasksColumnConstant.AreaName).ToString()
            taskModel.SectionName = row(TasksColumnConstant.SectionName).ToString()
            taskModel.StationName = row(TasksColumnConstant.StationName).ToString()
            taskModel.TaskName = row(TasksColumnConstant.TaskName).ToString()
            taskModel.MasterFileName = row(TasksColumnConstant.MasterFileName).ToString()
            taskModel.ModelAffiliation = row(TasksColumnConstant.ModelAffiliation).ToString()
            taskModel.TaskMemory = IIf(String.IsNullOrWhiteSpace(row(TasksColumnConstant.TaskMemory).ToString()), 0, Integer.Parse(row(TasksColumnConstant.TaskMemory).ToString))
            taskModel.TaskMemoryPlus = IIf(String.IsNullOrWhiteSpace(row(TasksColumnConstant.TaskMemoryPlus).ToString()), 0, Integer.Parse(row(TasksColumnConstant.TaskMemoryPlus).ToString))
            taskModel.TaskNodes = IIf(String.IsNullOrWhiteSpace(row(TasksColumnConstant.TaskNodes).ToString()), 0, Integer.Parse(row(TasksColumnConstant.TaskNodes).ToString))
            taskModel.TaskConnection = IIf(String.IsNullOrWhiteSpace(row(TasksColumnConstant.TaskConnection).ToString()), 0, Integer.Parse(row(TasksColumnConstant.TaskConnection).ToString))
            taskModel.MaxNoOfInstances = IIf(String.IsNullOrWhiteSpace(row(TasksColumnConstant.MaxNoOfInstances).ToString()), 0, Integer.Parse(row(TasksColumnConstant.MaxNoOfInstances).ToString))

            Return taskModel
        End Function
    End Class
End Namespace
