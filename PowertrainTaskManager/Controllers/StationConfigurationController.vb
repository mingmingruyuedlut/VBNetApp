
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class StationConfigurationController
        Property StationConfigurationRep As StationConfigurationRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal stationConfigRep As StationConfigurationRepository)
            StationConfigurationRep = stationConfigRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            StationConfigurationRep = New StationConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            StationConfigurationRep = New StationConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            StationConfigurationRep = New StationConfigurationRepository(dbSqlHelper)
        End Sub

        Public Function GetStationConfigurationBySection(ByVal strAreaName As String, ByVal strSectionName As String) As List(Of StationConfiguration)
            Dim stationConfigurationList = StationConfigurationRep.GetStationConfigurationBySection(strAreaName, strSectionName)
            Return stationConfigurationList
        End Function

        Public Function GetStationConfiguration(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String) As List(Of StationConfiguration)
            Dim stationConfigurationList = StationConfigurationRep.GetStationConfiguration(strAreaName, strSectionName, strStationName)
            Return stationConfigurationList
        End Function

        'Public Function GetStationConfiguration(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, stationInstance As Integer) As List(Of StationConfiguration)
        '    Dim stationConfigurationList = StationConfigurationRep.GetStationConfiguration(strAreaName, strSectionName, strStationName, stationInstance)
        '    Return stationConfigurationList
        'End Function

        Public Function GetStationConfiguration(stationTreeNodeHier As TreeNodeHierarchy) As List(Of StationConfiguration)
            Dim configList = StationConfigurationRep.GetStationConfiguration(stationTreeNodeHier.AreaName, stationTreeNodeHier.SectionName, stationTreeNodeHier.StationName)
            Return configList
        End Function

        Public Function GetStationConfiguration() As List(Of StationConfiguration)
            Dim stationConfigurationList = StationConfigurationRep.GetStationConfiguration()
            Return stationConfigurationList
        End Function

        Public Sub InsertStationConfiguration(ByVal listStationConfiguration As List(Of StationConfiguration))
            StationConfigurationRep.InsertStationConfiguration(listStationConfiguration)
        End Sub

        Public Sub InsertStationConfiguration(ByVal listStationConfiguration As List(Of StationConfiguration), ByVal listStation As List(Of StationModel), ByVal listStationStructure As List(Of StationStructure))
            Dim listStationConfigurationNew = New List(Of StationConfiguration)
            For i = 0 To listStation.Count - 1
                For j = 0 To listStationStructure.Count - 1
                    For k = 0 To listStationConfiguration.Count - 1
                        If listStationConfiguration(k).AreaName = listStation(i).AreaName And listStationConfiguration(k).SectionName = listStation(i).SectionName And listStationConfiguration(k).StationName = listStation(i).StationName And listStationConfiguration(k).MemberName = listStationStructure(j).Parent & "." & listStationStructure(j).MemberName Then
                            listStationConfigurationNew.Add(listStationConfiguration(k))
                            Exit For
                        End If
                    Next
                Next
            Next
            If listStationConfigurationNew.Count > 0 Then
                StationConfigurationRep.InsertStationConfiguration(listStationConfigurationNew)
            End If
        End Sub

        Public Function CheckStationConfigurationExisted(areaName As String, sectionName As String, stationName As String) As Boolean
            Dim configList = StationConfigurationRep.GetStationConfiguration(areaName, sectionName, stationName)
            If configList.Count > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Function CheckStationConfigurationExisted(areaName As String) As Boolean
            Dim configList = StationConfigurationRep.GetStationConfiguration(areaName)
            If configList.Count > 0 Then
                Return True
            End If
            Return False
        End Function

        'Public Sub DeleteStationConfiguration(areaName As String, sectionName As String, stationName As String, stationInstance As Integer)
        '    StationConfigurationRep.DeleteStationConfiguration(areaName, sectionName, stationName, stationInstance)
        'End Sub

        Public Sub DeleteStationConfiguration(areaName As String, sectionName As String, stationName As String)
            StationConfigurationRep.DeleteStationConfiguration(areaName, sectionName, stationName)
        End Sub

        Public Sub DeleteStationConfiguration(areaName As String, sectionName As String)
            StationConfigurationRep.DeleteStationConfiguration(areaName, sectionName)
        End Sub

        Public Sub DeleteStationConfiguration(areaName As String)
            StationConfigurationRep.DeleteStationConfiguration(areaName)
        End Sub

        Public Sub UpdateStationConfiguration(areaName As String, newAreaName As String)
            StationConfigurationRep.UpdateStationConfiguration(areaName, newAreaName)
        End Sub

        Public Sub UpdateStationConfiguration(areaName As String, sectionName As String, newSectionName As String)
            StationConfigurationRep.UpdateStationConfiguration(areaName, sectionName, newSectionName)
        End Sub

        Public Sub UpdateStationConfiguration(areaName As String, sectionName As String, stationName As String, newStationName As String)
            StationConfigurationRep.UpdateStationConfiguration(areaName, sectionName, stationName, newStationName)
        End Sub

        Public Function VerifyStationConfiguration(ByVal stationConfigList As List(Of StationConfiguration), ByVal stationName As String) As StationConfigValidateResult
            Dim stationConfigValList = stationConfigList.Where(Function(x) (x.StationName.Equals(stationName))).ToList()
            If (stationConfigValList.Count = 0) Then
                Return StationConfigValidateResult.NotFound
            End If

            Dim stationConfigVal = stationConfigValList.FirstOrDefault(Function(x) (x.MemberName.Equals(ConfigurationConstant.StationTaskNumber) Or
                                                                                    x.MemberName.Equals(ConfigurationConstant.StationTaskNumberNew)))
            If (stationConfigVal Is Nothing) Then
                Return StationConfigValidateResult.NotFound
            End If

            If (stationConfigVal.MemberValue > 0) Then
                Return StationConfigValidateResult.Valid
            End If

            Return StationConfigValidateResult.Invalid
        End Function


        Public Function GetDownloadStationConfigurationCount(ByVal treeNodeHier As TreeNodeHierarchy) As Integer
            Try
                Dim listStationConfiguration = GetStationConfigurationBySection(treeNodeHier.AreaName, treeNodeHier.SectionName)
                Return listStationConfiguration.Count
            Catch ex As Exception
                Log_Anything("GetDownloadStationConfigurationCount - " & GetExceptionInfo(ex))
                Return 0
            End Try
        End Function

        Public Sub DownloadStationConfiguration(ByRef fileObj As Object, ByVal treeNodeHier As TreeNodeHierarchy)
            Try
                Dim listStationConfiguration = GetStationConfigurationBySection(treeNodeHier.AreaName, treeNodeHier.SectionName)

                For Each stationConfig As StationConfiguration In listStationConfiguration
                    Dim memberNameArray = Split(stationConfig.MemberName, ".")
                    If UBound(memberNameArray) <= 0 Then
                        Continue For
                    End If
                    If (stationConfig.MemberType.Contains(DbMemberTypeConstant.DbMemberTypeString)) Then
                        DownloadStringTypeMember(fileObj, stationConfig)
                    Else
                        DownloadNotStringTypeMember(fileObj, stationConfig)
                    End If

                    Application.DoEvents()
                    Form7.ProgressBar1.Value += 1
                Next
            Catch ex As Exception
                Log_Anything("DownloadStationConfiguration - " & GetExceptionInfo(ex))
                Throw ex
            End Try

        End Sub


        Public Sub UploadStationConfiguration(ByRef fileObj As Object, ByVal treeNodeHier As TreeNodeHierarchy)
            Try
                Dim listStationConfiguration = GetStationConfigurationBySection(treeNodeHier.AreaName, treeNodeHier.SectionName)

                For Each stationConfig As StationConfiguration In listStationConfiguration
                    Dim memberNameArray = Split(stationConfig.MemberName, ".")
                    If UBound(memberNameArray) <= 0 Then
                        Continue For
                    End If
                    UploadTypeMember(fileObj, stationConfig)
                    Application.DoEvents()
                    Form7.ProgressBar1.Value += 1
                Next
            Catch ex As Exception
                Log_Anything("UploadStationConfiguration - " & GetExceptionInfo(ex))
                Throw ex
            End Try

        End Sub

        Private Sub UploadTypeMember(ByRef fileObj As Object, ByVal stationConfig As StationConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(stationConfig.MemberName, stationConfig.BaseTag)
            Dim memberValue = PythonController.UploadTag(fileObj, memberName)
            If memberValue <> stationConfig.MemberValue Then
                UpdateMemberValue(stationConfig.AreaName, stationConfig.SectionName, stationConfig.StationName, stationConfig.MemberName, memberValue)
            End If
        End Sub

        Public Sub UpdateMemberValue(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMemberName As String, ByVal strMemberValue As String)
            StationConfigurationRep.UpdateMemberValue(strAreaName, strSectionName, strStationName, strMemberName, strMemberValue)
        End Sub

        Private Sub DownloadNotStringTypeMember(ByRef fileObj As Object, ByVal stationConfig As StationConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(stationConfig.MemberName, stationConfig.BaseTag)
            Dim memberValue = CommonController.GetDownloadMemberValue(stationConfig.MemberValue)
            PythonController.DownloadTag(fileObj, memberName, memberValue)
        End Sub

        Private Sub DownloadStringTypeMember(ByRef fileObj As Object, ByVal stationConfig As StationConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(stationConfig.MemberName, stationConfig.BaseTag)
            Dim memberValue = CommonController.GetDownloadMemberValue(stationConfig.MemberValue)
            Dim actualMemberValueLength = memberValue.Length
            Dim memberLength = CommonController.GetStringTypeLength(stationConfig.MemberType)
            memberValue = CommonController.SupplementMemberValue(memberValue, memberLength)
            CommonController.DownloadStringTypeMember(fileObj, memberName, memberValue, memberLength)
            CommonController.DownloadStringTypeLength(fileObj, memberName, actualMemberValueLength)
        End Sub


    End Class
End Namespace

