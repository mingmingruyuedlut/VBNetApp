
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class MasterFileRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetMasterFiles() As List(Of MasterFile)
            Dim mfList = New List(Of MasterFile)()
            Dim query = "SELECT * FROM MasterFile"
            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(query)
                mfList = ConvertDbTableToMasterFileList(dt)
            Catch ex As Exception
                Log_Anything("GetMasterfiles - " & GetExceptionInfo(ex))
            End Try

            Return mfList
        End Function

        Public Function GetMasterFileByName(mfName As String) As MasterFile
            Dim mf = New MasterFile()
            Dim query = "SELECT * FROM MasterFile WHERE MasterFile_Name = @MasterFile_Name"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@MasterFile_Name", mfName))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                mf = ConvertDbTableToMasterFileList(dt).FirstOrDefault()
            Catch ex As Exception
                Log_Anything("GetMasterfileByName - " & GetExceptionInfo(ex))
            End Try

            Return mf
        End Function

        Private Function ConvertDbTableRowToMasterFile(row As DataRow) As MasterFile
            If row Is Nothing Then
                Return Nothing
            End If

            Dim mfName As String = row(MasterFileColumnConstant.MasterFileName).ToString()
            Dim mfRevision As String = row(MasterFileColumnConstant.MasterFileRevision).ToString()
            Dim mfApplicationNotes As String = row(MasterFileColumnConstant.MasterFileApplicationNotes).ToString()
            Dim omBase As Integer = Convert.ToInt32(row(MasterFileColumnConstant.OverheadMemoryBase).ToString())
            Dim omPlus As Integer = Convert.ToInt32(row(MasterFileColumnConstant.OverheadMemoryPlus).ToString())
            Dim mf = New MasterFile(mfName, mfRevision, mfApplicationNotes, omBase, omPlus)
            Return mf
        End Function

        Private Function ConvertDbTableToMasterFileList(dt As DataTable) As List(Of MasterFile)
            Dim mfList = New List(Of MasterFile)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim mf = ConvertDbTableRowToMasterFile(row)
                    mfList.Add(mf)
                Next
            End If
            Return mfList
        End Function
    End Class
End Namespace

