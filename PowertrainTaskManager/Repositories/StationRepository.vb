
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class StationRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Sub SetStationAccept(ByVal stationName As String, ByVal sectionName As String, ByVal areaName As String)
            Dim query As String = "update Stations set Accept=1 where Station_Name = @Station_Name and Section_Name = @Section_Name and Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Station_Name", stationName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Area_Name", areaName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("SetStationAccept - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Function GetStationsByMemberName(ByVal stationName As String, ByVal sectionName As String, ByVal areaName As String) As DataTable
            Dim query As String = "select * from Stations where Station_Name = @Station_Name and Section_Name = @Section_Name and Area_Name = @Area_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Station_Name", stationName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Area_Name", areaName))
            Dim dt = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("GetStationsByMemberName - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Function GetStationsLikeNameForCopy(areaName As String, sectionName As String, stationName As String) As List(Of StationModel)
            Dim stationList = New List(Of StationModel)
            Dim query = "SELECT * FROM Stations WHERE Area_Name = @Area_Name AND Section_Name = @Section_Name AND Station_Name LIKE @Station_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", areaName))
            params.Add(New OleDbParameter("@Section_Name", sectionName))
            params.Add(New OleDbParameter("@Station_Name", stationName & "%"))
            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                stationList = GetListStations(dt)
            Catch ex As Exception
                Log_Anything("GetStationsByMemberName - " & GetExceptionInfo(ex))
            End Try
            Return stationList
        End Function

        Public Function GetStations() As DataTable
            Dim query As String = "select * from Stations"
            Dim dt = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
            Catch ex As Exception
                Log_Anything("GetStationsByMemberName - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Sub InsertStation(station As StationModel)
            Dim query = "INSERT INTO Stations (Area_Name,Section_Name,Station_Name,Station_Type,PLC_Type,MasterFile_Name,MasterFile_Revision,ModelAffiliation,Accept) VALUES (@Area_Name,@Section_Name,@Station_Name,@Station_Type,@PLC_Type,@MasterFile_Name,@MasterFile_Revision,@ModelAffiliation,@Accept)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", station.AreaName))
            params.Add(New OleDbParameter("@Section_Name", station.SectionName))
            params.Add(New OleDbParameter("@Station_Name", station.StationName))
            params.Add(New OleDbParameter("@Station_Type", station.StationType))
            params.Add(New OleDbParameter("@PLC_Type", station.PlcType))
            params.Add(New OleDbParameter("@MasterFile_Name", station.MasterFileName))
            params.Add(New OleDbParameter("@MasterFile_Revision", station.MasterFileRevision))
            params.Add(New OleDbParameter("@ModelAffiliation", station.ModelAffiliation))
            params.Add(New OleDbParameter("@Accept", station.Accept))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertStation - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteAllStations()
            Dim query = "DELETE * FROM Stations"
            DbSqlHelper.ExcuteNonQuery(query)
        End Sub

        Public Function GetListStations(ByVal dt As DataTable) As List(Of StationModel)
            Dim stationList = New List(Of StationModel)
            For Each row In dt.Rows
                Dim station = ConvertDbTableRowToStationModel(row)
                stationList.Add(station)
            Next
            Return stationList
        End Function

        Private Function ConvertDbTableRowToStationModel(row As DataRow) As StationModel
            If row Is Nothing Then
                Return Nothing
            End If

            Dim station = New StationModel
            station.Id = Integer.Parse(row(StationsColumnConstant.Id).ToString)
            station.AreaName = row(StationsColumnConstant.AreaName).ToString()
            station.SectionName = row(StationsColumnConstant.SectionName).ToString()
            station.StationName = row(StationsColumnConstant.StationName).ToString()
            station.StationType = row(StationsColumnConstant.StationType).ToString()
            station.PlcType = row(StationsColumnConstant.PlcType).ToString()
            station.MasterFileName = row(StationsColumnConstant.MasterFileName).ToString()
            station.MasterFileRevision = row(StationsColumnConstant.MasterFileRevision).ToString()
            station.ModelAffiliation = row(StationsColumnConstant.ModelAffiliation).ToString()
            station.Accept = IIf(String.IsNullOrWhiteSpace(row(StationsColumnConstant.Accept).ToString()), 0, Integer.Parse(row(StationsColumnConstant.Accept).ToString()))

            Return station
        End Function
    End Class
End Namespace
