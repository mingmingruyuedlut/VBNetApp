
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class PlcConfigurationRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetPlcConfigurationByStation(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String) As List(Of PlcConfiguration)
            Dim plcConfigurationList = New List(Of PlcConfiguration)()
            Dim query As String = "SELECT * FROM PLCs_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", strAreaName))
            params.Add(New OleDbParameter("@Section_Name", strSectionName))
            params.Add(New OleDbParameter("@Station_Name", strStationName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                plcConfigurationList = ConvertDbTableToPlcConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetPlcConfigurationByStation - " & GetExceptionInfo(ex))
            End Try

            Return plcConfigurationList
        End Function

        Public Function GetPlcConfiguration() As List(Of PlcConfiguration)
            Dim plcConfigurationList = New List(Of PlcConfiguration)()
            Dim query As String = "SELECT * FROM PLCs_Configuration "
            Try
                Dim dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
                plcConfigurationList = ConvertDbTableToPlcConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetPlcConfigurationByStation - " & GetExceptionInfo(ex))
            End Try

            Return plcConfigurationList
        End Function

        Public Sub InsertPlcConfiguration(ByVal listPlcConfiguration As List(Of PlcConfiguration))
            For i = 0 To listPlcConfiguration.Count - 1
                Dim query = "insert into PLCs_Configuration (Area_Name,Section_Name,Station_Name,PLC_Name,Member_Name,Member_Value) values (@Area_Name,@Section_Name,@Station_Name,@PLC_Name,@Member_Name,@Member_Value)"
                Dim params = New List(Of OleDbParameter)()
                params.Add(New OleDbParameter("@Area_Name", listPlcConfiguration(i).AreaName))
                params.Add(New OleDbParameter("@Section_Name", listPlcConfiguration(i).SectionName))
                params.Add(New OleDbParameter("@Station_Name", listPlcConfiguration(i).StationName))
                params.Add(New OleDbParameter("@PLC_Name", listPlcConfiguration(i).PlcName))
                params.Add(New OleDbParameter("@Member_Name", listPlcConfiguration(i).MemberName))
                params.Add(New OleDbParameter("@Member_Value", listPlcConfiguration(i).MemberValue))
                Try
                    DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
                Catch ex As Exception
                    Log_Anything("InsertPlc - " & GetExceptionInfo(ex))
                End Try
            Next
        End Sub

        Private Function ConvertDbTableRowToPlcConfiguration(ByVal row As DataRow) As PlcConfiguration
            If row Is Nothing Then
                Return Nothing
            End If

            Dim areaName As String = row(PlcsConfigurationColumnConstant.AreaName).ToString()
            Dim sectionName As String = row(PlcsConfigurationColumnConstant.SectionName).ToString()
            Dim stationName As String = row(PlcsConfigurationColumnConstant.StationName).ToString()
            Dim plcName As String = row(PlcsConfigurationColumnConstant.PlcName).ToString()
            Dim memberName As String = row(PlcsConfigurationColumnConstant.MemberName).ToString()
            Dim memberValue As String = row(PlcsConfigurationColumnConstant.MemberValue).ToString()
            Dim plcConfiguration = New PlcConfiguration(areaName, sectionName, stationName, plcName, memberName, memberValue)
            Return PlcConfiguration
        End Function

        Private Function ConvertDbTableToPlcConfigurationList(ByVal dt As DataTable) As List(Of PlcConfiguration)
            Dim plcConfigurationList = New List(Of PlcConfiguration)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim plcConfiguration = ConvertDbTableRowToPlcConfiguration(row)
                    plcConfigurationList.Add(plcConfiguration)
                Next
            End If
            Return plcConfigurationList
        End Function
    End Class
End Namespace
