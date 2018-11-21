
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class SectionConfigurationRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetSectionConfiguration(areaName As String, sectionName As String) As List(Of SectionConfiguration)
            Dim configList = New List(Of SectionConfiguration)()
            Dim query = "SELECT * FROM Sections_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                configList = ConvertDbTableToSectionConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetSectionConfiguration - " & GetExceptionInfo(ex))
            End Try

            Return configList
        End Function

        Public Sub DeleteSectionConfiguration(areaName As String, sectionName As String)
            Dim query = "DELETE * FROM Sections_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteSectionConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteSectionConfiguration(areaName As String)
            Dim query = "DELETE * FROM Sections_Configuration WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteSectionConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateSectionConfiguration(areaName As String, newAreaName As String)
            Dim query = "UPDATE Sections_Configuration SET Area_Name = @New_Area_Name WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Area_Name", newAreaName))
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateSectionConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateSectionConfiguration(areaName As String, sectionName As String, newSectionName As String)
            Dim query = "UPDATE Sections_Configuration SET Section_Name = @New_Section_Name WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Section_Name", newSectionName))
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateSectionConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Private Function ConvertDbTableRowToSectionConfiguration(row As DataRow) As SectionConfiguration
            If row Is Nothing Then
                Return Nothing
            End If

            Dim areaName As String = row(SectionConfigurationColumnConstant.AreaName).ToString()
            Dim sectionName As String = row(SectionConfigurationColumnConstant.SectionName).ToString()
            Dim memberName As String = row(SectionConfigurationColumnConstant.MemberName).ToString()
            Dim memberValue As String = row(SectionConfigurationColumnConstant.MemberValue).ToString()
            Dim configObj = New SectionConfiguration(areaName, sectionName, memberName, memberValue)
            Return configObj
        End Function

        Private Function ConvertDbTableToSectionConfigurationList(dt As DataTable) As List(Of SectionConfiguration)
            Dim configList = New List(Of SectionConfiguration)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim configObj = ConvertDbTableRowToSectionConfiguration(row)
                    configList.Add(configObj)
                Next
            End If
            Return configList
        End Function
    End Class
End Namespace
