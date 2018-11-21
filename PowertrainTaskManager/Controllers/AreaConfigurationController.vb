Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class AreaConfigurationController
        Property AreaConfigurationRep As AreaConfigurationRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal areaConfigRep As AreaConfigurationRepository)
            AreaConfigurationRep = AreaConfigRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            AreaConfigurationRep = New AreaConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            AreaConfigurationRep = New AreaConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            AreaConfigurationRep = New AreaConfigurationRepository(dbSqlHelper)
        End Sub

        Public Function GetAreaConfigurationByStation(ByVal strAreaName As String) As List(Of AreaConfiguration)
            Dim areaConfigurationList = AreaConfigurationRep.GetAreaConfigurationByStation(strAreaName)
            Return AreaConfigurationList
        End Function

        Public Function GetAreaConfiguration() As List(Of AreaConfiguration)
            Dim areaConfigurationList = AreaConfigurationRep.GetAreaConfiguration()
            Return areaConfigurationList
        End Function

        Public Function CheckAreaConfigurationExisted(areaName As String) As Boolean
            Dim configList = AreaConfigurationRep.GetAreaConfiguration(areaName)
            If configList.Count > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Sub InsertAreaConfiguration(ByVal areaConfig As AreaConfiguration)
            AreaConfigurationRep.InsertAreaConfiguration(areaConfig)
        End Sub

        Public Sub InsertAreaConfiguration(ByVal areaConfigList As List(Of AreaConfiguration))
            For Each areaConfig In areaConfigList
                AreaConfigurationRep.InsertAreaConfiguration(areaConfig)
            Next
        End Sub

        Public Sub DeleteAreaConfiguration(ByVal areaName As String)
            AreaConfigurationRep.DeleteAreaConfiguration(areaName)
        End Sub

        Public Sub UpdateAreaConfiguration(ByVal areaConfig As AreaConfiguration)
            AreaConfigurationRep.UpdateAreaConfiguration(areaConfig)
        End Sub

        Public Sub UpdateAreaConfiguration(ByVal areaConfigList As List(Of AreaConfiguration))
            For Each areaConfig In areaConfigList
                AreaConfigurationRep.UpdateAreaConfiguration(areaConfig)
            Next
        End Sub

        Public Function ValidateAreaConfiguration(areaName As String) As Boolean
            Dim areaConfigurationList = AreaConfigurationRep.GetAreaConfigurationByStation(areaName)
            If areaConfigurationList IsNot Nothing OrElse areaConfigurationList.Count > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Function GetDownloadAreaConfigurationCount(ByVal treeNodeHier As TreeNodeHierarchy) As Integer
            Try
                Dim areaConfigurationList = GetAreaConfigurationByStation(treeNodeHier.AreaName)
                Return areaConfigurationList.Count
            Catch ex As Exception
                Log_Anything("GetDownloadAreaConfigurationCount - " & GetExceptionInfo(ex))
                Return 0
            End Try
        End Function

        Public Sub DownloadAreaConfiguration(ByRef fileObj As Object, ByVal treeNodeHier As TreeNodeHierarchy)
            Try
                Dim listAreaConfiguration = GetAreaConfigurationByStation(treeNodeHier.AreaName)
                Dim areaStruController = New AreaStructureController()
                areaStruController.Init()
                Dim modelIdInfo = areaStruController.GetListAreaStructure(AreaStructureConstant.ModelID).FirstOrDefault()

                For Each areaConfig As AreaConfiguration In listAreaConfiguration
                    Dim modelNumber = areaConfig.ModelNumber
                    Dim modelNumberInPlc = modelNumber - 1
                    Dim modelName = areaConfig.ModelName
                    If Not (IsNumeric(modelNumber)) Then
                        Continue For
                    End If
                    If (String.IsNullOrWhiteSpace(modelName)) Then
                        Continue For
                    End If

                    Dim modelNameLength = GetModelNameLength(modelName, modelIdInfo)
                    DownloadModelId(fileObj, modelName, modelNumberInPlc, modelNameLength)
                    DownloadModelIdLength(fileObj, modelNumberInPlc, modelNameLength)

                    Application.DoEvents()
                    Form7.ProgressBar1.Value += 1
                Next
            Catch ex As Exception
                Log_Anything("DownloadModelConfiguration - " & GetExceptionInfo(ex))
                Throw ex
            End Try

        End Sub

        Public Sub UploadAreaConfiguration(ByRef fileObj As Object, ByVal treeNodeHier As TreeNodeHierarchy)
            Try
                Dim listAreaConfiguration = GetAreaConfigurationByStation(treeNodeHier.AreaName)

                For Each areaConfig As AreaConfiguration In listAreaConfiguration
                    Dim modelNumber = areaConfig.ModelNumber
                    Dim modelName = areaConfig.ModelName
                    If Not (IsNumeric(modelNumber)) Then
                        Continue For
                    End If
                    If (String.IsNullOrWhiteSpace(modelName)) Then
                        Continue For
                    End If

                    UploadModelName(fileObj, areaConfig)

                    Application.DoEvents()
                    Form7.ProgressBar1.Value += 1
                Next
            Catch ex As Exception
                Log_Anything("UploadModelConfiguration - " & GetExceptionInfo(ex))
                Throw ex
            End Try

        End Sub
        Private Sub UploadModelName(ByRef fileObj As Object, ByVal areaConfig As AreaConfiguration)
            Dim modelNumber = areaConfig.ModelNumber
            Dim modelNumberInPlc = modelNumber - 1
            Dim memberName = "Model[" & modelNumberInPlc & "].ModelID"
            Dim memberValue = PythonController.UploadTag(fileObj, memberName)
            If memberValue <> areaConfig.ModelName Then
                UpdateAreaConfiguration(areaConfig.AreaName, areaConfig.ModelNumber, memberValue)
            End If
        End Sub

        Public Sub UpdateAreaConfiguration(ByVal strAreaName As String, ByVal strModelNumber As String, ByVal strModelName As String)
            AreaConfigurationRep.UpdateAreaConfiguration(strAreaName, strModelNumber, strModelName)
        End Sub

        Public Sub UpdateAreaConfiguration(areaName As String, newAreaName As String)
            AreaConfigurationRep.UpdateAreaConfiguration(areaName, newAreaName)
        End Sub

        Private Function GetModelNameLength(ByVal modelName As String, ByVal modelIdInfo As AreaStructure) As Integer
            Dim modelIdTypeLength = 16
            If (modelIdInfo.MemberType.Contains(DbMemberTypeConstant.DbMemberTypeString)) Then
                modelIdTypeLength = CommonController.GetStringTypeLength(modelIdInfo.MemberType)
            End If

            If (modelName.Length < modelIdTypeLength) Then
                Return modelName.Length
            Else
                Return modelIdTypeLength
            End If
        End Function

        Private Sub DownloadModelId(ByRef fileObj As Object, ByVal modelName As String, ByVal modelNumberInPlc As Integer, ByVal modelNameLength As Integer)
            For k = 0 To modelNameLength - 1
                Dim tagValue = Asc(Mid(modelName, k + 1, 1))
                Dim tagName = "Model[" & modelNumberInPlc & "].ModelID.Data[" & k & "]"
                PythonController.DownloadTag(fileObj, tagName, tagValue)
            Next
        End Sub

        Private Sub DownloadModelIdLength(ByRef fileObj As Object, ByVal modelNumberInPlc As Integer, ByVal modelNameLength As Integer)
            Dim tagName = "Model[" & modelNumberInPlc & "].ModelID.Len"
            Dim tagValue = modelNameLength
            PythonController.DownloadTag(fileObj, tagName, tagValue)
        End Sub

        Public Function CheckAreaConfigurationEmpty(ByVal modelConfigList As List(Of AreaConfiguration), ByRef msg As String) As Boolean
            Dim emptyModelNameCount = modelConfigList.Where(Function(x) String.IsNullOrWhiteSpace(x.ModelName)).Count
            If emptyModelNameCount > 0 Then
                msg = "Some Model ID is empty, please input the target value."
                Return False
            End If
            Return True
        End Function

        Public Function CheckAreaConfigurationLength(ByVal modelConfigList As List(Of AreaConfiguration), ByVal modelIdLength As Integer, ByRef msg As String) As Boolean
            Dim outOfLengthModelConfigList = modelConfigList.Where(Function(x) x.ModelName.Length > modelIdLength).ToList()
            For Each modelConfig In outOfLengthModelConfigList
                If String.IsNullOrWhiteSpace(msg) Then
                    msg = "Model ID: " & modelConfig.ModelNumber & " - Name must be " & modelIdLength & " characters or less."
                Else
                    msg = msg & vbCrLf & "Model ID: " & modelConfig.ModelNumber & " - Name must be " & modelIdLength & " characters or less."
                End If
            Next
            If outOfLengthModelConfigList.Count > 0 Then
                Return False
            End If
            Return True
        End Function

        Public Function CheckDupliatedAreaConfiguration(ByVal modelConfigList As List(Of AreaConfiguration), ByRef msg As String) As Boolean
            Dim duplicatedModelConfigList = modelConfigList.
                    GroupBy(Function(x) x.ModelName).
                    Select(Function(g) New AreaConfigurationGroup(g.Key, g.Select(Function(x) x).ToList())).
                    ToList()
            For Each modelConfig In duplicatedModelConfigList
                If IsNothing(modelConfig.AreaConfigurationList) Then
                    Continue For
                End If
                If modelConfig.AreaConfigurationList.Count > 1 Then
                    If String.IsNullOrWhiteSpace(msg) Then
                        msg = "Model ID: "
                    Else
                        msg = msg & vbCrLf & "Model ID: "
                    End If

                    For Each mConfig In modelConfig.AreaConfigurationList
                        msg = msg & mConfig.ModelNumber & ","
                    Next
                    msg = msg.Substring(0, msg.Length - 1) & " Duplicates not allowed"
                End If
            Next

            If String.IsNullOrWhiteSpace(msg) Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
