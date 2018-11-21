
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class ModelConfigurationRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetModelConfigurationByStation(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String) As List(Of ModelConfiguration)
            Dim modelConfigurationList = New List(Of ModelConfiguration)()
            Dim query As String = "SELECT * FROM Model_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", strAreaName))
            params.Add(New OleDbParameter("@Section_Name", strSectionName))
            params.Add(New OleDbParameter("@Station_Name", strStationName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                ModelConfigurationList = ConvertDbTableToModelConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetModelConfigurationByStation - " & GetExceptionInfo(ex))
            End Try

            Return ModelConfigurationList
        End Function

        Public Sub UpdateMemberValue(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMemberName As String, ByVal strMemberValue As String)
            Dim query As String = "update Model_Configuration set Member_Value = @Member_Value WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name and Member_Name = @Member_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Member_Value", strMemberValue))
            params.Add(New OleDbParameter("@Area_Name", strAreaName))
            params.Add(New OleDbParameter("@Section_Name", strSectionName))
            params.Add(New OleDbParameter("@Station_Name", strStationName))
            params.Add(New OleDbParameter("@Member_Name", strMemberName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateMemberValue - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Function GetModelConfiguration() As List(Of ModelConfiguration)
            Dim modelConfigurationList = New List(Of ModelConfiguration)()
            Dim query As String = "SELECT * FROM Model_Configuration"
            Try
                Dim dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
                modelConfigurationList = ConvertDbTableToModelConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetModelConfigurationByStation - " & GetExceptionInfo(ex))
            End Try

            Return modelConfigurationList
        End Function

        Public Sub InsertModelConfiguration(ByVal listModelConfiguration As List(Of ModelConfiguration))
            For i = 0 To listModelConfiguration.Count - 1
                Dim query = "insert into Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Model_Instance,Member_Type) values (@Area_Name,@Section_Name,@Station_Name,@Task_Name,@Task_Instance,@Member_Name,@Member_Value,@Model_Instance,@Member_Type)"
                Dim params = New List(Of OleDbParameter)()
                params.Add(New OleDbParameter("@Area_Name", listModelConfiguration(i).AreaName))
                params.Add(New OleDbParameter("@Section_Name", listModelConfiguration(i).SectionName))
                params.Add(New OleDbParameter("@Station_Name", listModelConfiguration(i).StationName))
                params.Add(New OleDbParameter("@Task_Name", listModelConfiguration(i).TaskName))
                params.Add(New OleDbParameter("@Task_Instance", listModelConfiguration(i).TaskInstance))
                params.Add(New OleDbParameter("@Member_Name", listModelConfiguration(i).MemberName))
                params.Add(New OleDbParameter("@Member_Value", listModelConfiguration(i).MemberValue))
                params.Add(New OleDbParameter("@Model_Instance", listModelConfiguration(i).ModelInstance))
                params.Add(New OleDbParameter("@Member_Type", listModelConfiguration(i).MemberType))
                Try
                    DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
                Catch ex As Exception
                    Log_Anything("InsertArea - " & GetExceptionInfo(ex))
                End Try
            Next
        End Sub

        Private Function ConvertDbTableRowToModelConfiguration(ByVal row As DataRow) As ModelConfiguration
            If row Is Nothing Then
                Return Nothing
            End If

            Dim areaName As String = row(ModelConfigurationColumnConstant.AreaName).ToString()
            Dim sectionName As String = row(ModelConfigurationColumnConstant.SectionName).ToString()
            Dim stationName As String = row(ModelConfigurationColumnConstant.StationName).ToString()
            Dim taskName As String = row(ModelConfigurationColumnConstant.TaskName).ToString()
            Dim taskInstance As String = row(ModelConfigurationColumnConstant.TaskInstance).ToString()
            Dim memberName As String = row(ModelConfigurationColumnConstant.MemberName).ToString()
            Dim memberValue As String = row(ModelConfigurationColumnConstant.MemberValue).ToString()
            Dim modelInstance As String = row(ModelConfigurationColumnConstant.ModelInstance).ToString()
            Dim memberType As String = row(ModelConfigurationColumnConstant.MemberType).ToString()
            Dim modelConfiguration = New ModelConfiguration(areaName, sectionName, stationName, taskName, taskInstance, memberName, memberValue, modelInstance, memberType)
            Return ModelConfiguration
        End Function

        Private Function ConvertDbTableToModelConfigurationList(ByVal dt As DataTable) As List(Of ModelConfiguration)
            Dim modelConfigurationList = New List(Of ModelConfiguration)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim modelConfiguration = ConvertDbTableRowToModelConfiguration(row)
                    ModelConfigurationList.Add(ModelConfiguration)
                Next
            End If
            Return ModelConfigurationList
        End Function
    End Class
End Namespace
