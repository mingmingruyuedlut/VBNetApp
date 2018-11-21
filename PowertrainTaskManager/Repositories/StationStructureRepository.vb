
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Namespace Repositories
    Public Class StationStructureRepository
        Dim DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetStationStructureDatable() As DataTable
            Dim query As String = "SELECT * FROM Station_Structures WHERE Visible = 1 ORDER BY MEMBERORDER"
            Dim dt As DataTable = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
            Catch ex As Exception
                Log_Anything("GetStationStructureDatable - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function
        Public Function GetStationStructureDatableByMemberGroup(ByVal strMemberGroup) As DataTable
            Dim query As String = "SELECT * FROM Station_Structures WHERE Visible = 1 and MemberDescription_3 = @MemberGroup"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@MemberGroup", strMemberGroup))
            Dim dt As DataTable = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("GetStationStructureDatable - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Function GetStationStructureDatableByMemberName(ByVal memberName As String) As DataTable
            Dim query As String = "select Parent from Station_Structures where MemberDescription_1 = @MemberName or MemberName = @MemberName"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@MemberName", memberName))
            Dim dt = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("GetParentByMemberName - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Sub DeleteStationStructureByBase()
            Dim query As String = "Delete from Station_Structures where MemberName='SPACE'"
            Try
                DbSqlHelper.ExcuteNonQuery(query)
            Catch ex As Exception
                Log_Anything("DeleteStationStructureByBase - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub InsertStationStructure(ByVal parent As String, ByVal order As Integer)
            Dim query As String = "insert into Station_Structures ([Parent],[MemberName],[MemberOrder],[Visible],[Global]) values(@Parent,'SPACE',@MemberOrder ,1,1)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Parent", parent))
            params.Add(New OleDbParameter("@MemberOrder", order))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertStationStructure - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateStationStructureMemberOrderbyMemberName(ByVal memberName As String, ByVal order As Integer)
            Dim query As String = "update Station_Structures set MemberOrder=@MemberOrder where MemberDescription_1 = @MemberName or MemberName = @MemberName"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@MemberOrder", order))
            params.Add(New OleDbParameter("@MemberName", memberName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateStationStructureMemberOrderbyMemberName - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Function GetParentByMemberName(ByVal memberName As String) As String
            Dim dt = GetStationStructureDatableByMemberName(memberName)
            Dim parent = dt(0)("Parent").ToString
            Return parent
        End Function

        Public Function GetListStationStructure(ByVal dt As DataTable) As List(Of StationStructure)
            Dim listStationStructure As List(Of StationStructure) = New List(Of StationStructure)
            For i = 0 To dt.Rows.Count - 1
                Dim stationstructure As StationStructure = New StationStructure
                stationstructure.ID = Integer.Parse(dt.Rows(i)("ID").ToString)
                stationstructure.Parent = dt.Rows(i)("Parent").ToString
                stationstructure.MemberName = dt.Rows(i)("MemberName").ToString
                stationstructure.MemberType = dt.Rows(i)("MemberType").ToString
                stationstructure.MemberOrder = dt.Rows(i)("MemberOrder").ToString
                stationstructure.MemberDescription1 = dt.Rows(i)("MemberDescription_1").ToString
                stationstructure.MemberDescription2 = dt.Rows(i)("MemberDescription_2").ToString
                stationstructure.MemberDescription3 = dt.Rows(i)("MemberDescription_3").ToString
                stationstructure.MemberValues = dt.Rows(i)("MemberValues").ToString
                stationstructure.TaskXrefName = dt.Rows(i)("TaskXrefName").ToString
                stationstructure.Visible = Integer.Parse(IIf(dt.Rows(i)("Visible").ToString = "", 0, dt.Rows(i)("Visible").ToString))
                stationstructure.Global1 = Integer.Parse(IIf(dt.Rows(i)("Global").ToString = "", 0, dt.Rows(i)("Global").ToString))
                stationstructure.BASE = Integer.Parse(IIf(dt.Rows(i)("BASE").ToString = "", 0, dt.Rows(i)("BASE").ToString))
                stationstructure.MaxLength = Integer.Parse(IIf(dt.Rows(i)("MaxLength").ToString = "", 0, dt.Rows(i)("MaxLength").ToString))
                stationstructure.MinValue = Integer.Parse(IIf(dt.Rows(i)("MinValue").ToString = "", 0, dt.Rows(i)("MinValue").ToString))
                stationstructure.MaxValue = Integer.Parse(IIf(dt.Rows(i)("MaxValue").ToString = "", 0, dt.Rows(i)("MaxValue").ToString))
                stationstructure.ExclusionString = dt.Rows(i)("ExclusionString").ToString
                stationstructure.Version = dt.Rows(i)("Version").ToString
                stationstructure.MemberGroup = dt.Rows(i)("MemberGroup").ToString
                listStationStructure.Add(stationstructure)
            Next
            Return listStationStructure
        End Function
    End Class
End Namespace
