
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class SectionConfigurationController
        Property SectionConfigurationRep As SectionConfigurationRepository

        Public Sub New()
        End Sub

        Public Sub New(ByVal taskConfigRep As SectionConfigurationRepository)
            SectionConfigurationRep = taskConfigRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            SectionConfigurationRep = New SectionConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            SectionConfigurationRep = New SectionConfigurationRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            SectionConfigurationRep = New SectionConfigurationRepository(dbSqlHelper)
        End Sub

        Public Function CheckSectionConfigurationExisted(areaName As String, sectionName As String) As Boolean
            Dim configList = SectionConfigurationRep.GetSectionConfiguration(areaName, sectionName)
            If configList.Count > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Sub DeleteSectionConfiguration(areaName As String, sectionName As String)
            SectionConfigurationRep.DeleteSectionConfiguration(areaName, sectionName)
        End Sub

        Public Sub DeleteSectionConfiguration(areaName As String)
            SectionConfigurationRep.DeleteSectionConfiguration(areaName)
        End Sub

        Public Sub UpdateSectionConfiguration(areaName As String, newAreaName As String)
            SectionConfigurationRep.UpdateSectionConfiguration(areaName, newAreaName)
        End Sub

        Public Sub UpdateSectionConfiguration(areaName As String, section As String, newSectionName As String)
            SectionConfigurationRep.UpdateSectionConfiguration(areaName, section, newSectionName)
        End Sub
    End Class
End Namespace
