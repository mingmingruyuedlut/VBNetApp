
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class PlcConfigurationController
        Property PlcConfigurationRep As PlcConfigurationRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal plcConfigRep As PlcConfigurationRepository)
            PlcConfigurationRep = plcConfigRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            PlcConfigurationRep = New PlcConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            PlcConfigurationRep = New PlcConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            PlcConfigurationRep = New PlcConfigurationRepository(dbSqlHelper)
        End Sub

        Public Function GetPlcConfigurationByStation(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String) As List(Of PlcConfiguration)
            Dim plcConfigurationList = PlcConfigurationRep.GetPlcConfigurationByStation(strAreaName, strSectionName, strStationName)
            Return plcConfigurationList
        End Function

        Public Function GetPlcConfiguration() As List(Of PlcConfiguration)
            Dim plcConfigurationList = PlcConfigurationRep.GetPlcConfiguration()
            Return plcConfigurationList
        End Function

        Public Function GetPlcConfiguration(stationTreeNodeHier As TreeNodeHierarchy) As List(Of PlcConfiguration)
            Dim plcConfigurationList = PlcConfigurationRep.GetPlcConfigurationByStation(stationTreeNodeHier.AreaName, stationTreeNodeHier.SectionName, stationTreeNodeHier.StationName)
            Return plcConfigurationList
        End Function

        Public Sub InsertPlcConfiguration(ByVal listPlcConfiguration As List(Of PlcConfiguration))
            PlcConfigurationRep.InsertPlcConfiguration(listPlcConfiguration)
        End Sub

        Public Function GetPlcAddressConfiguration(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String) As PlcAddressConfiguration
            Dim plcConfigurationList = GetPlcConfigurationByStation(strAreaName, strSectionName, strStationName)
            Dim plcAddressConfiguration = GetPlcAddressConfiguration(plcConfigurationList)
            Return plcAddressConfiguration
        End Function

        Public Function GetPlcAddressConfiguration(ByVal listPlcConfiguartion As List(Of PlcConfiguration)) As PlcAddressConfiguration
            Dim strIpAddress1 = String.Empty
            Dim strIpAddress2 = String.Empty
            Dim strIpAddress3 = String.Empty
            Dim strIpAddress4 = String.Empty
            Dim strPlcSlot = String.Empty

            For Each plc As PlcConfiguration In listPlcConfiguartion
                Select Case plc.MemberName
                    Case PlcsConfigurationColumnConstant.Address1
                        strIpAddress1 = plc.MemberValue
                    Case PlcsConfigurationColumnConstant.Address2
                        strIpAddress2 = plc.MemberValue
                    Case PlcsConfigurationColumnConstant.Address3
                        strIpAddress3 = plc.MemberValue
                    Case PlcsConfigurationColumnConstant.Address4
                        strIpAddress4 = plc.MemberValue
                    Case PlcsConfigurationColumnConstant.Slot
                        strPlcSlot = plc.MemberValue
                End Select
            Next

            Dim plcAddressConfig = New PlcAddressConfiguration(strIpAddress1, strIpAddress2, strIpAddress3, strIpAddress4, Integer.Parse(strPlcSlot))
            Return plcAddressConfig
        End Function

        Public Function GetPythonFileObj(ByVal treeNodeHier As TreeNodeHierarchy) As Object
            Try
                Dim plcAddressConfig = GetPlcAddressConfiguration(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)
                Dim fileObj = PythonController.GetPythonFileObj(plcAddressConfig)
                Return fileObj
            Catch ex As Exception
                Throw ex
            End Try
            
        End Function
    End Class
End Namespace
