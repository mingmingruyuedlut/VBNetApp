
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Namespace Repositories
    Public Class AreaStructureRepository
        Dim DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetAreaStructureDataTable(ByVal strAreaMemberName As String) As DataTable
            Dim query As String = "SELECT * FROM Area_Structures WHERE MemberName = @MemberName"
            Dim dt As DataTable = New DataTable
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@AreaXrefName", strAreaMemberName))
            Try
                dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("GetAreaStructureDataTable - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Function GetAreaStructureDataTable() As DataTable
            Dim query As String = "SELECT * FROM Area_Structures"
            Dim dt As DataTable = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
            Catch ex As Exception
                Log_Anything("GetAreaStructureDataTable - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Function GetListAreaStructure(ByVal dt As DataTable) As List(Of AreaStructure)
            Dim listAreaStructure As List(Of AreaStructure) = New List(Of AreaStructure)
            For i = 0 To dt.Rows.Count - 1
                Dim areastructure As AreaStructure = New AreaStructure
                Areastructure.Id = Integer.Parse(dt.Rows(i)("ID").ToString)
                areastructure.Parent = dt.Rows(i)("Parent").ToString
                Areastructure.MemberName = dt.Rows(i)("MemberName").ToString
                Areastructure.MemberType = dt.Rows(i)("MemberType").ToString
                Areastructure.MemberOrder = dt.Rows(i)("MemberOrder").ToString
                Areastructure.MemberDescription1 = dt.Rows(i)("MemberDescription_1").ToString
                Areastructure.MemberDescription2 = dt.Rows(i)("MemberDescription_2").ToString
                Areastructure.MemberDescription3 = dt.Rows(i)("MemberDescription_3").ToString
                Areastructure.MemberValues = dt.Rows(i)("MemberValues").ToString
                Areastructure.TaskXrefName = dt.Rows(i)("TaskXrefName").ToString
                Areastructure.Visible = Integer.Parse(IIf(dt.Rows(i)("Visible").ToString = "", 0, dt.Rows(i)("Visible").ToString))
                Areastructure.Global1 = Integer.Parse(IIf(dt.Rows(i)("Global").ToString = "", 0, dt.Rows(i)("Global").ToString))
                Areastructure.Base = Integer.Parse(IIf(dt.Rows(i)("BASE").ToString = "", 0, dt.Rows(i)("BASE").ToString))
                Areastructure.MaxLength = Integer.Parse(IIf(dt.Rows(i)("MaxLength").ToString = "", 0, dt.Rows(i)("MaxLength").ToString))
                Areastructure.MinValue = Integer.Parse(IIf(dt.Rows(i)("MinValue").ToString = "", 0, dt.Rows(i)("MinValue").ToString))
                Areastructure.MaxValue = Integer.Parse(IIf(dt.Rows(i)("MaxValue").ToString = "", 0, dt.Rows(i)("MaxValue").ToString))
                areastructure.ExclusionString = dt.Rows(i)("ExclusionString").ToString
                areastructure.MemberAffiliation = dt.Rows(i)("MemberAffiliation").ToString
                Areastructure.Version = dt.Rows(i)("Version").ToString
                listAreaStructure.Add(Areastructure)
            Next
            Return listAreaStructure
        End Function
    End Class
End Namespace
