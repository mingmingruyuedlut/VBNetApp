
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories
Namespace Controllers
    Public Class SectionsController
        Property SectionsControllerRep As SectionsRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal sectionsRep As SectionsRepository)
            SectionsControllerRep = SectionsRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            SectionsControllerRep = New SectionsRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            SectionsControllerRep = New SectionsRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            SectionsControllerRep = New SectionsRepository(dbSqlHelper)
        End Sub

        Public Function GetListSections() As List(Of SectionModel)
            Dim areaLists
            areaLists = SectionsControllerRep.GetListSections(SectionsControllerRep.GetSections())
            Return areaLists
        End Function

        Public Sub InsertSerction(ByVal sectionList As List(Of SectionModel))
            For Each sectionObj In sectionList
                InsertSection(sectionObj)
            Next
        End Sub

        Public Sub InsertSection(sectionObj As SectionModel)
            SectionsControllerRep.InsertSection(sectionObj)
        End Sub

        Public Sub DeleteAllSections()
            SectionsControllerRep.DeleteAllSections()
        End Sub

    End Class
End Namespace

