
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class MasterFilesTaskRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetMasterfilesTaskByMasterFileName(ByVal masterFileName As String) As List(Of MasterFilesTask)
            Dim masterfilesTaskList = New List(Of MasterFilesTask)()
            Dim query As String = "SELECT * FROM MasterFile_Tasks WHERE MasterFile_Name = @MasterFile_Name and Memory_Used = 0"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@MasterFile_Name", masterFileName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                MasterfilesTaskList = ConvertDbTableToMasterfilesTaskList(dt)
            Catch ex As Exception
                Log_Anything("GetMasterfilesTaskByStation - " & GetExceptionInfo(ex))
            End Try

            Return MasterfilesTaskList
        End Function

        Public Function GetMasterFileTasksByMfNameAndMultiStation(mfName As String, multiStation As Integer) As List(Of MasterFilesTask)
            Dim mfTaskList = New List(Of MasterFilesTask)()
            'Note: db table column name contains special characters, such as: 'space', 'punctuation', we need to use the delimiter to mark the column name!!!!!!
            Dim query = "SELECT * FROM MasterFile_Tasks WHERE MasterFile_Name = @MasterFile_Name AND [Multi-Station] = @Multi_Station"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@MasterFile_Name", mfName))
            params.Add(New OleDbParameter("@Multi_Station", multiStation))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                mfTaskList = ConvertDbTableToMasterfilesTaskList(dt)
            Catch ex As Exception
                Log_Anything("GetMasterFileTasksByMfNameAndMultiStation - " & GetExceptionInfo(ex))
            End Try

            Return mfTaskList
        End Function

        Public Function GetMasterfileTasks() As List(Of MasterFilesTask)
            Dim masterfileTaskList = New List(Of MasterFilesTask)()
            Dim query = "SELECT * FROM MasterFile_Tasks"
            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(query)
                masterfileTaskList = ConvertDbTableToMasterfilesTaskList(dt)
            Catch ex As Exception
                Log_Anything("GetMasterfileTasks - " & GetExceptionInfo(ex))
            End Try

            Return masterfileTaskList
        End Function

        Private Function ConvertDbTableRowToMasterfilesTask(ByVal row As DataRow) As MasterFilesTask
            If row Is Nothing Then
                Return Nothing
            End If

            Dim masterFileName As String = row(MasterFilesTaskColumnConstant.MasterFileName).ToString()
            Dim taskName As String = row(MasterFilesTaskColumnConstant.TaskName).ToString()
            Dim memoryUsed As String = row(MasterFilesTaskColumnConstant.MemoryUsed).ToString()
            Dim version As String = row(MasterFilesTaskColumnConstant.Version).ToString()
            Dim multiStation As String = row(MasterFilesTaskColumnConstant.MultiStation).ToString()
            Dim maxNoOfInstances As String = row(MasterFilesTaskColumnConstant.MaxNoOfInstances).ToString()
            Dim modelAffiliation As String = row(MasterFilesTaskColumnConstant.ModelAffiliation).ToString()
            Dim l5XFileName As String = row(MasterFilesTaskColumnConstant.L5XFileName).ToString()
            Dim masterfilesTask = New MasterFilesTask(masterFileName, taskName, memoryUsed, version, multiStation, maxNoOfInstances, modelAffiliation, l5XFileName)
            Return MasterfilesTask
        End Function

        Private Function ConvertDbTableToMasterfilesTaskList(ByVal dt As DataTable) As List(Of MasterFilesTask)
            Dim masterfilesTaskList = New List(Of MasterFilesTask)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim masterfilesTask = ConvertDbTableRowToMasterfilesTask(row)
                    MasterfilesTaskList.Add(MasterfilesTask)
                Next
            End If
            Return MasterfilesTaskList
        End Function
    End Class
End Namespace
