
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class ModelConfigurationController
        Property ModelConfigurationRep As ModelConfigurationRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal modelConfigRep As ModelConfigurationRepository)
            ModelConfigurationRep = ModelConfigRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            ModelConfigurationRep = New ModelConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            ModelConfigurationRep = New ModelConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            ModelConfigurationRep = New ModelConfigurationRepository(dbSqlHelper)
        End Sub


        Public Function GetModelConfigurationByStation(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String) As List(Of ModelConfiguration)
            Dim modelConfigurationList = ModelConfigurationRep.GetModelConfigurationByStation(strAreaName, strSectionName, strStationName)
            Return ModelConfigurationList
        End Function

        Public Function GetModelConfiguration() As List(Of ModelConfiguration)
            Dim modelConfigurationList = ModelConfigurationRep.GetModelConfiguration()
            Return modelConfigurationList
        End Function

        Public Sub InsertModelConfiguration(ByVal listModelConfiguration As List(Of ModelConfiguration))
            ModelConfigurationRep.InsertModelConfiguration(listModelConfiguration)
        End Sub

        Public Function GetDownloadModelConfigurationCount(ByVal treeNodeHier As TreeNodeHierarchy) As Integer
            Try
                Dim listModelConfiguration = GetModelConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)
                Return listModelConfiguration.Count
            Catch ex As Exception
                Log_Anything("GetDownloadModelConfigurationCount - " & GetExceptionInfo(ex))
                Return 0
            End Try
        End Function

        Public Sub DownloadModelConfiguration(ByRef fileObj As Object, ByVal treeNodeHier As TreeNodeHierarchy)
            Try
                Dim listModelConfiguration = GetModelConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)

                For Each modelConfig As ModelConfiguration In listModelConfiguration
                    Dim memberNameArray = Split(modelConfig.MemberName, ".")
                    If (UBound(memberNameArray) <= 0) Then
                        Continue For
                    End If
                    If (modelConfig.MemberType.Contains(DbMemberTypeConstant.DbMemberTypeString)) Then
                        DownloadStringTypeMember(fileObj, modelConfig)
                    Else
                        DownloadNotStringTypeMember(fileObj, modelConfig)
                    End If

                    Application.DoEvents()
                    Form7.ProgressBar1.Value += 1
                Next
            Catch ex As Exception
                Log_Anything("DownloadModelConfiguration - " & GetExceptionInfo(ex))
                Throw ex
            End Try

        End Sub

        Public Sub UpdateMemberValue(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMemberName As String, ByVal strMemberValue As String)
            ModelConfigurationRep.UpdateMemberValue(strAreaName, strSectionName, strStationName, strMemberName, strMemberValue)
        End Sub

        Public Sub UploadModelConfiguration(ByRef fileObj As Object, ByVal treeNodeHier As TreeNodeHierarchy)
            Try
                Dim listModelConfiguration = GetModelConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)

                For Each modelConfig As ModelConfiguration In listModelConfiguration
                    Dim memberNameArray = Split(modelConfig.MemberName, ".")
                    If (UBound(memberNameArray) <= 0) Then
                        Continue For
                    End If

                    UploadTypeMember(fileObj, modelConfig)


                    Application.DoEvents()
                    Form7.ProgressBar1.Value += 1
                Next
            Catch ex As Exception
                Log_Anything("UploadModelConfiguration - " & GetExceptionInfo(ex))
                Throw ex
            End Try

        End Sub

        Private Sub DownloadNotStringTypeMember(ByRef fileObj As Object, ByVal modelConfig As ModelConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(modelConfig.MemberName)
            Dim memberValue = CommonController.GetDownloadMemberValue(modelConfig.MemberValue)
            PythonController.DownloadTag(fileObj, memberName, memberValue)
        End Sub

        Private Sub DownloadStringTypeMember(ByRef fileObj As Object, ByVal modelConfig As ModelConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(modelConfig.MemberName)
            Dim memberValue = CommonController.GetDownloadMemberValue(modelConfig.MemberValue)
            Dim actualMemberValueLength = memberValue.Length
            Dim memberLength = CommonController.GetStringTypeLength(modelConfig.MemberType)
            memberValue = CommonController.SupplementMemberValue(memberValue, memberLength)
            CommonController.DownloadStringTypeMember(fileObj, memberName, memberValue, memberLength)
            CommonController.DownloadStringTypeLength(fileObj, memberName, actualMemberValueLength)
        End Sub

        Private Sub UploadTypeMember(ByRef fileObj As Object, ByVal modelConfig As ModelConfiguration)
            Dim memberName = CommonController.GetDownloadMemberName(modelConfig.MemberName)
            Dim memberValue = PythonController.UploadTag(fileObj, memberName)
            If memberValue <> modelConfig.MemberValue Then
                UpdateMemberValue(modelConfig.AreaName, modelConfig.SectionName, modelConfig.StationName, modelConfig.MemberName, memberValue)
            End If
        End Sub
    End Class
End Namespace
