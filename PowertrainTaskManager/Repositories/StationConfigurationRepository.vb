
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class StationConfigurationRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetStationConfigurationBySection(ByVal strAreaName As String, ByVal strSectionName As String) As List(Of StationConfiguration)
            Dim stationConfigurationList = New List(Of StationConfiguration)()
            Dim query As String = "SELECT * FROM Stations_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", strAreaName))
            params.Add(New OleDbParameter("@Section_Name", strSectionName))
            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                stationConfigurationList = ConvertDbTableToStationConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetStationConfigurationBySection - " & GetExceptionInfo(ex))
            End Try
            Return stationConfigurationList
        End Function

        Public Function GetStationConfiguration() As List(Of StationConfiguration)
            Dim stationConfigurationList = New List(Of StationConfiguration)()
            Dim query As String = "SELECT * FROM Stations_Configuration "
            Try
                Dim dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
                stationConfigurationList = ConvertDbTableToStationConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetStationConfigurationBySection - " & GetExceptionInfo(ex))
            End Try
            Return stationConfigurationList
        End Function

        Public Function GetStationConfiguration(areaName As String, sectionName As String, stationName As String) As List(Of StationConfiguration)
            Dim stationConfigurationList = New List(Of StationConfiguration)()
            Dim query = "SELECT * FROM Stations_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                stationConfigurationList = ConvertDbTableToStationConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetStationConfiguration - " & GetExceptionInfo(ex))
            End Try

            Return stationConfigurationList
        End Function

        Public Function GetStationConfiguration(areaName As String) As List(Of StationConfiguration)
            Dim stationConfigurationList = New List(Of StationConfiguration)()
            Dim query = "SELECT * FROM Stations_Configuration WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                stationConfigurationList = ConvertDbTableToStationConfigurationList(dt)
            Catch ex As Exception
                Log_Anything("GetStationConfiguration - " & GetExceptionInfo(ex))
            End Try

            Return stationConfigurationList
        End Function

        Public Sub DeleteStationConfiguration(areaName As String, sectionName As String, stationName As String)
            Dim query = "DELETE * FROM Stations_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteStationConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteStationConfiguration(areaName As String, sectionName As String)
            Dim query = "DELETE * FROM Stations_Configuration WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteStationConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteStationConfiguration(areaName As String)
            Dim query = "DELETE * FROM Stations_Configuration WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteStationConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateStationConfiguration(areaName As String, newAreaName As String)
            Dim query = "UPDATE Stations_Configuration SET Area_Name = @New_Area_Name WHERE Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Area_Name", newAreaName))
            params.Add(New OleDbParameter("@Area_Name", areaName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateStationConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateStationConfiguration(areaName As String, sectionName As String, newSectionName As String)
            Dim query = "UPDATE Stations_Configuration SET Section_Name = @New_Section_Name WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Section_Name", newSectionName))
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateStationConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateStationConfiguration(areaName As String, sectionName As String, stationName As String, newStationName As String)
            Dim query = "UPDATE Stations_Configuration SET Station_Name = @New_Station_Name WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@New_Station_Name", newStationName))
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateStationConfiguration - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateMemberValue(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMemberName As String, ByVal strMemberValue As String)
            Dim query As String = "update Stations_Configuration set Member_Value = @Member_Value WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name = @Station_Name and Member_Name = @Member_Name"
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

        Public Sub InsertStationConfiguration(ByVal listStationConfiguration As List(Of StationConfiguration))
            For i = 0 To listStationConfiguration.Count - 1
                Dim query = "insert into Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Member_Type,Base_Tag) values (@Area_Name,@Section_Name,@Station_Name,@Member_Name,@Member_Value,@Member_Type,@Base_Tag)"
                Dim params = New List(Of OleDbParameter)()
                params.Add(New OleDbParameter("@Area_Name", listStationConfiguration(i).AreaName))
                params.Add(New OleDbParameter("@Section_Name", listStationConfiguration(i).SectionName))
                params.Add(New OleDbParameter("@Station_Name", listStationConfiguration(i).StationName))
                params.Add(New OleDbParameter("@Member_Name", listStationConfiguration(i).MemberName))
                params.Add(New OleDbParameter("@Member_Value", listStationConfiguration(i).MemberValue))
                params.Add(New OleDbParameter("@Member_Type", listStationConfiguration(i).MemberType))
                params.Add(New OleDbParameter("@Base_Tag", listStationConfiguration(i).BaseTag))
                Try
                    DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
                Catch ex As Exception
                    Log_Anything("InsertStationConfiguration - " & GetExceptionInfo(ex))
                End Try
            Next
        End Sub

        Private Function ConvertDbTableRowToStationConfiguration(ByVal row As DataRow) As StationConfiguration
            If row Is Nothing Then
                Return Nothing
            End If

            Dim areaName As String = row(StationsConfigurationColumnConstant.AreaName).ToString()
            Dim sectionName As String = row(StationsConfigurationColumnConstant.SectionName).ToString()
            Dim stationName As String = row(StationsConfigurationColumnConstant.StationName).ToString()
            Dim memberName As String = row(StationsConfigurationColumnConstant.MemberName).ToString()
            Dim memberValue As String = row(StationsConfigurationColumnConstant.MemberValue).ToString()
            Dim memberType As String = row(StationsConfigurationColumnConstant.MemberType).ToString()
            Dim baseTag As String = row(StationsConfigurationColumnConstant.BaseTag).ToString()
            Dim stationConfiguration = New StationConfiguration(areaName, sectionName, stationName, memberName, memberValue, memberType, baseTag)
            Return stationConfiguration
        End Function

        Private Function ConvertDbTableToStationConfigurationList(ByVal dt As DataTable) As List(Of StationConfiguration)
            Dim stationConfigurationList = New List(Of StationConfiguration)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim stationConfiguration = ConvertDbTableRowToStationConfiguration(row)
                    stationConfigurationList.Add(stationConfiguration)
                Next
            End If
            Return stationConfigurationList
        End Function
    End Class
End Namespace

