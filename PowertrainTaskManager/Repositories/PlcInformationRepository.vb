
Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Repositories
    Public Class PlcInformationRepository
        Property DbSqlHelper As SqlHelper

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            Me.DbSqlHelper = dbSqlHelper
        End Sub

        Public Function GetPlcInformationByProcessorType(ByVal processorType As String) As List(Of PlcInformation)
            Dim plcInfoList = New List(Of PlcInformation)()
            Dim query = "SELECT * FROM PLC_Information WHERE Processor_Type = @Processor_Type"
            Dim params = New List(Of OleDbParameter)()
            params.Add(New OleDbParameter("@Processor_Type", processorType))

            Try
                Dim dt = DbSqlHelper.ExcuteDataTable(CommandType.Text, query, params)
                plcInfoList = ConvertDbTableToPlcInformationList(dt)
            Catch ex As Exception
                Log_Anything("GetPlcInformationByProcessorType - " & GetExceptionInfo(ex))
            End Try

            Return plcInfoList
        End Function

        Public Function GetPlcInformation() As List(Of PlcInformation)
            Dim plcInfoList = New List(Of PlcInformation)()
            Dim query = "SELECT * FROM PLC_Information"
            Try
                Dim dt = DbSqlHelper.ExcuteDataSet(query).Tables(0)
                plcInfoList = ConvertDbTableToPlcInformationList(dt)
            Catch ex As Exception
                Log_Anything("GetPlcInformation - " & GetExceptionInfo(ex))
            End Try

            Return plcInfoList
        End Function

        Private Function ConvertDbTableRowToPlcInformation(ByVal row As DataRow) As PlcInformation
            If row Is Nothing Then
                Return Nothing
            End If

            Dim processorType As String = row(PlcInformationColumnConstant.ProcessorType).ToString()
            Dim applicationNotes As String = row(PlcInformationColumnConstant.ApplicationNotes).ToString()
            Dim totalBytesAvailable As Integer = Convert.ToInt32(row(PlcInformationColumnConstant.TotalBytesAvailable).ToString())
            Dim maxNodes As Integer = Convert.ToInt32(row(PlcInformationColumnConstant.MaxNodes).ToString())
            Dim maxConnections As Integer = Convert.ToInt32(row(PlcInformationColumnConstant.MaxConnections).ToString())
            Dim plcInfo = New PlcInformation(processorType, applicationNotes, totalBytesAvailable, maxNodes, maxConnections)
            Return plcInfo
        End Function

        Private Function ConvertDbTableToPlcInformationList(ByVal dt As DataTable) As List(Of PlcInformation)
            Dim plcInfoList = New List(Of PlcInformation)()
            Dim row As DataRow
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    Dim plcInfo = ConvertDbTableRowToPlcInformation(row)
                    plcInfoList.Add(plcInfo)
                Next
            End If
            Return plcInfoList
        End Function
    End Class
End Namespace

