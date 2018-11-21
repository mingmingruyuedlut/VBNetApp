
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class MemoryUsageRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetMemoryUsages() As List(Of MemoryUsage)
            Dim muList = New List(Of MemoryUsage)()
            Dim query = "SELECT * FROM Memory_Usage"
            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(query)
                muList = ConvertDbTableToMemoryUsageList(dt)
            Catch ex As Exception
                Log_Anything("GetMemoryUsages - " & GetExceptionInfo(ex))
            End Try

            Return muList
        End Function

        Public Function GetMemoryUsages(treeNodeHier As TreeNodeHierarchy) As List(Of MemoryUsage)
            Dim muList = New List(Of MemoryUsage)()
            Dim query = "SELECT * FROM Memory_Usage WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", treeNodeHier.AreaName))
            params.Add(New OleDbParameter("@Section_Name", treeNodeHier.SectionName))
            params.Add(New OleDbParameter("@Station_Name", treeNodeHier.StationName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                muList = ConvertDbTableToMemoryUsageList(dt)
            Catch ex As Exception
                Log_Anything("GetMemoryUsages - " & GetExceptionInfo(ex))
            End Try

            Return muList
        End Function

        Public Function GetMemoryUsageDataTable() As DataTable
            Dim query = "SELECT * FROM Memory_Usage"
            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(query)
                Return dt
            Catch ex As Exception
                Log_Anything("GetMemoryUsages - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        Public Sub InsertMemoryUsage(mu As MemoryUsage)
            Dim query = "INSERT INTO Memory_Usage (Area_Name, Section_Name, Station_Name, PLCType, Total_Mem, Total_Mem_Rsvd, Mem_Available, Mem_Used, Percent_Available, Percent_Used) Values (@Area_Name, @Section_Name, @Station_Name, @PLCType, @Total_Mem, @Total_Mem_Rsvd, @Mem_Available, @Mem_Used, @Percent_Available, @Percent_Used)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", mu.AreaName))
            params.Add(New OleDbParameter("@Section_Name", mu.SectionName))
            params.Add(New OleDbParameter("@Station_Name", mu.StationName))
            params.Add(New OleDbParameter("@PLCType", mu.PlcType))
            params.Add(New OleDbParameter("@Total_Mem", mu.TotalMem))
            params.Add(New OleDbParameter("@Total_Mem_Rsvd", mu.TotalMemRsvd))
            params.Add(New OleDbParameter("@Mem_Available", mu.MemAvailable))
            params.Add(New OleDbParameter("@Mem_Used", mu.MemUsed))
            params.Add(New OleDbParameter("@Percent_Available", mu.PercentAvailable))
            params.Add(New OleDbParameter("@Percent_Used", mu.PercentUsed))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertMemoryUsage - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteMemoryUsages()
            Dim query = "DELETE * FROM Memory_Usage"
            Try
                DbSqlHelper.ExcuteNonQuery(query)
            Catch ex As Exception
                Log_Anything("DeleteMemoryUsages - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Private Function ConvertDbTableRowToMemoryUsage(row As DataRow) As MemoryUsage
            If row Is Nothing Then
                Return Nothing
            End If
            Dim areaName As String = row(MemoryUsageColumnConstant.AreaName).ToString()
            Dim sectionName As String = row(MemoryUsageColumnConstant.SectionName).ToString()
            Dim stationName As String = row(MemoryUsageColumnConstant.StationName).ToString()
            Dim plcType As String = row(MemoryUsageColumnConstant.PlcType).ToString()
            Dim totalMem As Integer = Convert.ToInt32(row(MemoryUsageColumnConstant.TotalMem).ToString())
            Dim totalMemRsvd As Integer = Convert.ToInt32(row(MemoryUsageColumnConstant.TotalMemRsvd).ToString())
            Dim memAvailable As Integer = Convert.ToInt32(row(MemoryUsageColumnConstant.MemAvailable).ToString())
            Dim memUsed As Integer = Convert.ToInt32(row(MemoryUsageColumnConstant.MemUsed).ToString())
            Dim percentAvailable As Integer = Convert.ToInt32(row(MemoryUsageColumnConstant.PercentAvailable).ToString())
            Dim percentUsed As Integer = Convert.ToInt32(row(MemoryUsageColumnConstant.PercentUsed).ToString())
            Dim mu = New MemoryUsage(areaName, sectionName, stationName, plcType, totalMem, totalMemRsvd, memAvailable, memUsed, percentAvailable, percentUsed)
            Return mu
        End Function

        Private Function ConvertDbTableToMemoryUsageList(dt As DataTable) As List(Of MemoryUsage)
            Dim muList = New List(Of MemoryUsage)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim mu = ConvertDbTableRowToMemoryUsage(row)
                    muList.Add(mu)
                Next
            End If
            Return muList
        End Function
    End Class
End Namespace

