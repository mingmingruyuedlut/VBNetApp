
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Namespace Repositories
    Public Class TaskStructureRepository
        Dim DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetTaskStructureDataTable(ByVal strTaskConfigName As String) As DataTable
            Dim query As String = "SELECT * FROM Task_Structures WHERE TaskXrefName = @TaskXrefName AND Visible = 1 ORDER BY MEMBERORDER"
            Dim dt As DataTable = New DataTable
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@TaskXrefName", strTaskConfigName))
            Try
                dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("GetStationConfigurationBySection - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Function GetTaskStructureDataTable() As DataTable
            Dim query As String = "SELECT * FROM Task_Structures WHERE Visible = 1 ORDER BY MEMBERORDER"
            Dim dt As DataTable = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
            Catch ex As Exception
                Log_Anything("GetStationConfigurationBySection - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Sub DeleteTaskStructureSpace(ByVal strTaskConfigName As String)
            Dim query As String = "Delete from Task_Structures where MemberName='SPACE' and TaskXrefName = @TaskXrefName"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@TaskXrefName", strTaskConfigName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("DeleteTaskStructureSpace - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub InsertTaskStructureSpace(ByVal strTaskConfigName As String, ByVal parent As String, ByVal order As Integer)
            Dim query As String = "INSERT INTO Task_Structures([TaskName],[MemberName],[MemberOrder],[TaskXrefName],[Visible],[Global]) VALUES(@TaskName, @MemberName, @MemberOrder, @TaskXrefName, @Visible, @Global)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@TaskName", Parent))
            params.Add(New OleDbParameter("@MemberName", "SPACE"))
            params.Add(New OleDbParameter("@MemberOrder", Order))
            params.Add(New OleDbParameter("@TaskXrefName", strTaskConfigName))
            params.Add(New OleDbParameter("@Visible", 1))
            params.Add(New OleDbParameter("@Global", 1))

            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertTaskStructureSpace - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub UpdateTaskStructureMemberOrderbyMemberName(ByVal memberName As String, ByVal order As Integer, ByVal strTaskConfigName As String)
            Dim query As String = "update Task_Structures set MemberOrder=@MemberOrder where (MemberDescription_1 = @MemberName or MemberName = @MemberName) and TaskXrefName = @TaskXrefName"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@MemberOrder", Order))
            params.Add(New OleDbParameter("@MemberName", MemberName))
            params.Add(New OleDbParameter("@TaskXrefName", strTaskConfigName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("UpdateTaskStructureMemberOrderbyMemberName - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Function GetTaskNameByMemberName(ByVal memberName As String, ByVal strTaskConfigName As String) As String
            Dim parent As String = ""
            Dim query As String = "select TaskName from Task_Structures where (MemberDescription_1 = @MemberName or MemberName = @MemberName) and TaskXrefName = @TaskXrefName"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@MemberName", MemberName))
            params.Add(New OleDbParameter("@TaskXrefName", strTaskConfigName))
            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                Parent = dt(0)("TaskName").ToString
            Catch ex As Exception
                Log_Anything("GetTaskNameByMemberName - " & GetExceptionInfo(ex))
            End Try
            Return Parent
        End Function

        Public Function GetListTaskStructure(ByVal dt As DataTable) As List(Of TaskStructure)
            Dim listTaskStructure As List(Of TaskStructure) = New List(Of TaskStructure)
            For i = 0 To dt.Rows.Count - 1
                Dim taskstructure As TaskStructure = New TaskStructure
                taskstructure.ID = Integer.Parse(dt.Rows(i)("ID").ToString)
                taskstructure.TaskName = dt.Rows(i)("TaskName").ToString
                taskstructure.MemberName = dt.Rows(i)("MemberName").ToString
                taskstructure.MemberType = dt.Rows(i)("MemberType").ToString
                taskstructure.MemberOrder = dt.Rows(i)("MemberOrder").ToString
                taskstructure.MemberDescription1 = dt.Rows(i)("MemberDescription_1").ToString
                taskstructure.MemberDescription2 = dt.Rows(i)("MemberDescription_2").ToString
                taskstructure.MemberDescription3 = dt.Rows(i)("MemberDescription_3").ToString
                taskstructure.MemberValues = dt.Rows(i)("MemberValues").ToString
                taskstructure.TaskXrefName = dt.Rows(i)("TaskXrefName").ToString
                taskstructure.Visible = Integer.Parse(IIf(dt.Rows(i)("Visible").ToString = "", 0, dt.Rows(i)("Visible").ToString))
                taskstructure.Global1 = Integer.Parse(IIf(dt.Rows(i)("Global").ToString = "", 0, dt.Rows(i)("Global").ToString))
                taskstructure.BASE = Integer.Parse(IIf(dt.Rows(i)("BASE").ToString = "", 0, dt.Rows(i)("BASE").ToString))
                taskstructure.MaxLength = Integer.Parse(IIf(dt.Rows(i)("MaxLength").ToString = "", 0, dt.Rows(i)("MaxLength").ToString))
                taskstructure.MinValue = Integer.Parse(IIf(dt.Rows(i)("MinValue").ToString = "", 0, dt.Rows(i)("MinValue").ToString))
                taskstructure.MaxValue = Integer.Parse(IIf(dt.Rows(i)("MaxValue").ToString = "", 0, dt.Rows(i)("MaxValue").ToString))
                taskstructure.ExclusionString = dt.Rows(i)("ExclusionString").ToString
                taskstructure.Version = dt.Rows(i)("Version").ToString
                ListTaskStructure.Add(taskstructure)
            Next
            Return ListTaskStructure
        End Function
    End Class
End Namespace
