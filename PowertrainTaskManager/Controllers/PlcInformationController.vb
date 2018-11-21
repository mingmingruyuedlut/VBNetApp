
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class PlcInformationController
        Property PlcInformationRep As PlcInformationRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal plcInfoRep As PlcInformationRepository)
            PlcInformationRep = plcInfoRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            PlcInformationRep = New PlcInformationRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            PlcInformationRep = New PlcInformationRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        'It's for master file db
        Public Sub InitMaster()
            Dim dbConnStr As String = My.Settings.DBConnectionString & My.Settings.DatabasePath & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            PlcInformationRep = New PlcInformationRepository(dbSqlHelper)
        End Sub

        Public Function GetPlcInformationByProcessorType(processorType As String) As PlcInformation
            Dim plcInfo = PlcInformationRep.GetPlcInformationByProcessorType(processorType).FirstOrDefault()
            Return plcInfo
        End Function

        Public Function GetPlcInformation() As List(Of PlcInformation)
            Dim plcInfoList = PlcInformationRep.GetPlcInformation()
            Return plcInfoList
        End Function
    End Class
End Namespace

