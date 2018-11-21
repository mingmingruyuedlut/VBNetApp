
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class TaskConfigurationController
        Property TaskConfigurationRep As TaskConfigurationRepository

        Public Sub New()
        End Sub

        Public Sub New(ByVal taskConfigRep As TaskConfigurationRepository)
            TaskConfigurationRep = taskConfigRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            TaskConfigurationRep = New TaskConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            TaskConfigurationRep = New TaskConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            TaskConfigurationRep = New TaskConfigurationRepository(dbSqlHelper)
        End Sub

        Public Function GetTaskConfigurationByStation(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String) As List(Of TaskConfiguration)
            Dim taskConfigurationList = TaskConfigurationRep.GetTaskConfigurationByStation(strAreaName, strSectionName, strStationName)
            Return taskConfigurationList
        End Function

        Public Function GetTaskConfiguration() As List(Of TaskConfiguration)
            Dim taskConfigurationList = TaskConfigurationRep.GetTaskConfiguration()
            Return taskConfigurationList
        End Function

        Public Function GetTaskConfiguration(stationTreeNodeHier As TreeNodeHierarchy) As List(Of TaskConfiguration)
            Dim taskConfigurationList = TaskConfigurationRep.GetTaskConfiguration(stationTreeNodeHier.AreaName, stationTreeNodeHier.SectionName, stationTreeNodeHier.StationName)
            Return taskConfigurationList
        End Function

        Public Function CheckTaskConfigurationExisted(taskConfig As TaskConfiguration) As Boolean
            Dim taskConfigurationList = TaskConfigurationRep.GetTaskConfiguration(taskConfig.AreaName, taskConfig.SectionName, taskConfig.StationName, taskConfig.TaskName)
            If taskConfigurationList.Count > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Sub InsertTaskConfiguration(ByVal listTaskConfiguration As List(Of TaskConfiguration))
            TaskConfigurationRep.InsertTaskConfiguration(listTaskConfiguration)
        End Sub

        Public Sub InsertTaskConfiguration(ByVal listTaskConfiguration As List(Of TaskConfiguration), ByVal listTaskModel As List(Of TaskModel), ByVal listTaskStructure As List(Of TaskStructure))
            Dim listTaskConfigurationNew = New List(Of TaskConfiguration)
            For i = 0 To listTaskModel.Count - 1
                For j = 0 To listTaskStructure.Count - 1
                    If listTaskStructure(j).TaskName = listTaskModel(i).ModelAffiliation Then
                        For k = 0 To listTaskConfiguration.Count - 1
                            If listTaskConfiguration(k).AreaName = listTaskModel(i).AreaName And listTaskConfiguration(k).SectionName = listTaskModel(i).SectionName And listTaskConfiguration(k).StationName = listTaskModel(i).StationName And listTaskConfiguration(k).TaskName = listTaskModel(i).TaskName And listTaskConfiguration(k).TaskInstance < getTaskCount(listTaskModel(i).TaskName) And listTaskConfiguration(k).MemberName.IndexOf(listTaskStructure(j).MemberName) > -1 Then
                                listTaskConfigurationNew.Add(listTaskConfiguration(k))
                            End If
                        Next
                    End If
                Next
            Next
            TaskConfigurationRep.InsertTaskConfiguration(listTaskConfigurationNew)
        End Sub

        Public Sub DeleteTaskConfiguration(areaName As String, sectionName As String, stationName As String, taskName As String)
            TaskConfigurationRep.DeleteTaskConfiguration(areaName, sectionName, stationName, taskName)
        End Sub

        Public Sub DeleteTaskConfiguration(areaName As String, sectionName As String, stationName As String)
            TaskConfigurationRep.DeleteTaskConfiguration(areaName, sectionName, stationName)
        End Sub

        Public Sub DeleteTaskConfiguration(areaName As String, sectionName As String)
            TaskConfigurationRep.DeleteTaskConfiguration(areaName, sectionName)
        End Sub

        Public Sub DeleteTaskConfiguration(areaName As String)
            TaskConfigurationRep.DeleteTaskConfiguration(areaName)
        End Sub

        Public Sub UpdateTaskConfiguration(areaName As String, newAreaName As String)
            TaskConfigurationRep.UpdateTaskConfiguration(areaName, newAreaName)
        End Sub

        Public Sub UpdateTaskConfiguration(areaName As String, sectionName As String, newSectionName As String)
            TaskConfigurationRep.UpdateTaskConfiguration(areaName, sectionName, newSectionName)
        End Sub

        Public Sub UpdateTaskConfiguration(areaName As String, sectionName As String, stationName As String, newStationName As String)
            TaskConfigurationRep.UpdateTaskConfiguration(areaName, sectionName, stationName, newStationName)
        End Sub

        Private Function getTaskCount(ByVal taskName As String) As Integer
            If taskName.IndexOf("(", StringComparison.OrdinalIgnoreCase) > -1 Then
                Dim startPosition = taskName.IndexOf("(", StringComparison.OrdinalIgnoreCase)
                Dim endPosition = taskName.IndexOf(")", StringComparison.OrdinalIgnoreCase)
                Dim typeLength = taskName.Substring(startPosition + 1, endPosition - startPosition - 1)
                Return typeLength
            Else
                Return 0
            End If
        End Function

        Public Function VerifyTaskConfiguration(ByVal taskConfigList As List(Of TaskConfiguration), ByVal strTaskName As String) As TaskConfigValidateResult
            Dim taskConfigValList = taskConfigList.Where(Function(x) (x.TaskName.Equals(strTaskName))).ToList()
            If (taskConfigValList.Count = 0) Then
                Return TaskConfigValidateResult.NotFound
            End If

            Dim taskConfigVal = taskConfigValList.FirstOrDefault(Function(x) (x.MemberName.EndsWith(ConfigurationConstant.DotNumber) OrElse x.MemberName.EndsWith(ConfigurationConstant.DotTask_Number) OrElse x.MemberName.EndsWith(ConfigurationConstant.DotTaskNumber)))
            If (taskConfigVal Is Nothing) Then
                Return TaskConfigValidateResult.NotFoundValidationField
            End If

            If (taskConfigVal.MemberValue > 0) Then
                Return TaskConfigValidateResult.Valid
            End If

            Return TaskConfigValidateResult.Invalid
        End Function

        Public Function VerifyTaskConfiguration(ByVal taskConfigList As List(Of TaskConfiguration), ByVal strTaskName As String, ByVal intTaskInstance As Integer) As TaskConfigValidateResult
            Dim taskConfigValList = taskConfigList.Where(Function(x) (x.TaskName.Equals(strTaskName))).ToList()
            If (taskConfigValList.Count = 0) Then
                Return TaskConfigValidateResult.NotFound
            End If

            taskConfigValList = taskConfigValList.Where(Function(x) (x.TaskInstance.Equals(intTaskInstance))).ToList()
            If (taskConfigValList.Count = 0) Then
                Return TaskConfigValidateResult.NotFoundValidationField
            End If

            Dim taskConfigVal = taskConfigValList.FirstOrDefault(Function(x) (x.MemberName.EndsWith(ConfigurationConstant.DotNumber) OrElse x.MemberName.EndsWith(ConfigurationConstant.DotTask_Number) OrElse x.MemberName.EndsWith(ConfigurationConstant.DotTaskNumber)))
            If (taskConfigVal Is Nothing) Then
                Return TaskConfigValidateResult.NotFoundValidationField
            End If

            If (taskConfigVal.MemberValue > 0) Then
                Return TaskConfigValidateResult.Valid
            End If

            Return TaskConfigValidateResult.Invalid
        End Function


        Public Function GetDownloadTaskConfigurationCount(ByVal treeNodeHier As TreeNodeHierarchy) As Integer
            Try
                Dim listTaskConfiguration = GetTaskConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)
                Return listTaskConfiguration.Count
            Catch ex As Exception
                Log_Anything("Get_Download_TaskConfiguration - " & GetExceptionInfo(ex))
                Return 0
            End Try
        End Function

        Public Sub DownloadTaskConfiguration(ByRef fileObj As Object, ByVal treeNodeHier As TreeNodeHierarchy)
            Try
                Dim listTaskConfiguration = GetTaskConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)

                For Each taskConfig As TaskConfiguration In listTaskConfiguration
                    Dim memberNameArray = Split(taskConfig.MemberName, ".")
                    If UBound(memberNameArray) <= 0 Then
                        Continue For
                    End If
                    If (taskConfig.MemberType.Contains(DbMemberTypeConstant.DbMemberTypeString)) Then
                        DownloadStringTypeMember(fileObj, taskConfig)
                    Else
                        DownloadNotStringTypeMember(fileObj, taskConfig)
                    End If

                    Application.DoEvents()
                    Form7.ProgressBar1.Value += 1
                Next
            Catch ex As Exception
                Log_Anything("Download_TaskConfiguration - " & GetExceptionInfo(ex))
                Throw ex
            End Try

        End Sub

        Public Sub UploadTaskConfiguration(ByRef fileObj As Object, ByVal treeNodeHier As TreeNodeHierarchy)
            Try
                Dim listTaskConfiguration = GetTaskConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)

                For Each taskConfig As TaskConfiguration In listTaskConfiguration
                    Dim memberNameArray = Split(taskConfig.MemberName, ".")
                    If UBound(memberNameArray) <= 0 Then
                        Continue For
                    End If
                    UploadMemberValue(fileObj, taskConfig)
                    Application.DoEvents()
                    Form7.ProgressBar1.Value += 1
                Next
            Catch ex As Exception
                Log_Anything("Upload_TaskConfiguration - " & GetExceptionInfo(ex))
                Throw ex
            End Try

        End Sub

        Private Sub UploadMemberValue(ByRef fileObj As Object, ByVal taskConfig As TaskConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(taskConfig.MemberName, taskConfig.BaseTag)
            Dim memberValue = PythonController.UploadTag(fileObj, memberName)
            If memberValue <> taskConfig.MemberValue Then
                UpdateMemberValue(taskConfig.AreaName, taskConfig.SectionName, taskConfig.StationName, taskConfig.MemberName, taskConfig.TaskName, memberValue)
            End If
        End Sub

        Public Sub UpdateMemberValue(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMemberName As String, ByVal strTaskName As String, ByVal strMemberValue As String)
            TaskConfigurationRep.UpdateMemberValue(strAreaName, strSectionName, strStationName, strMemberName, strTaskName, strMemberValue)
        End Sub

        Private Sub DownloadNotStringTypeMember(ByRef fileObj As Object, ByVal taskConfig As TaskConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(taskConfig.MemberName, taskConfig.BaseTag)
            Dim memberValue = CommonController.GetDownloadMemberValue(taskConfig.MemberValue)
            PythonController.DownloadTag(fileObj, memberName, memberValue)
        End Sub

        Private Sub DownloadStringTypeMember(ByRef fileObj As Object, ByVal taskConfig As TaskConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(taskConfig.MemberName, taskConfig.BaseTag)
            Dim memberValue = CommonController.GetDownloadMemberValue(taskConfig.MemberValue)
            Dim actualMemberValueLength = memberValue.Length
            Dim memberLength = CommonController.GetStringTypeLength(taskConfig.MemberType)
            memberValue = CommonController.SupplementMemberValue(memberValue, memberLength)
            CommonController.DownloadStringTypeMember(fileObj, memberName, memberValue, memberLength)
            CommonController.DownloadStringTypeLength(fileObj, memberName, actualMemberValueLength)
        End Sub
    End Class
End Namespace
