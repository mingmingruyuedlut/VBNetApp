
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class SectionsRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub


        Public Function GetSections() As DataTable
            Dim query As String = "select * from Sections "
            Dim dt = New DataTable
            Try
                dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
            Catch ex As Exception
                Log_Anything("GetSections - " & GetExceptionInfo(ex))
            End Try
            Return dt
        End Function

        Public Sub InsertSection(sectionObj As SectionModel)
            Dim query = "INSERT INTO Sections (Area_Name,Section_Name) VALUES (@Area_Name,@Section_Name)"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Area_Name", sectionObj.AreaName))
            params.Add(New OleDbParameter("@Section_Name", sectionObj.SectionName))
            Try
                DbSqlHelper.ExcuteNonQuery(CommandType.Text, query, params)
            Catch ex As Exception
                Log_Anything("InsertSection - " & GetExceptionInfo(ex))
            End Try
        End Sub

        Public Sub DeleteAllSections()
            Dim query = "DELETE * FROM Sections"
            DbSqlHelper.ExcuteNonQuery(query)
        End Sub

        Public Function GetListSections(ByVal dt As DataTable) As List(Of SectionModel)
            Dim listSectionStructure As List(Of SectionModel) = New List(Of SectionModel)
            For i = 0 To dt.Rows.Count - 1
                Dim sectionModel As SectionModel = New SectionModel
                sectionModel.Id = Integer.Parse(dt.Rows(i)("ID").ToString)
                sectionModel.AreaName = dt.Rows(i)("Area_Name").ToString
                sectionModel.SectionName = dt.Rows(i)("Section_Name").ToString
                listSectionStructure.Add(sectionModel)
            Next
            Return listSectionStructure
        End Function
    End Class
End Namespace
