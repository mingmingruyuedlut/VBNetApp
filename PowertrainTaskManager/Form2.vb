Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data.OleDb
Imports System.IO
Imports Powertrain_Task_Manager.Controllers
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Common
Imports IronPython.Hosting
Imports Microsoft.Scripting.Hosting
Imports Microsoft.Scripting.Runtime
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models

Public Class Form2
    Public SystemColor As Color
    Public bISDirty As Boolean = False

    Dim visited As Boolean = False
    Dim bFirstTimeAreaIsLoaded As Boolean
    Dim intNumberOfModelsVisible As Integer
    Dim strModelsString As String
    Dim bShowIndexForm As Boolean

    Dim InitialValueArrayGB1() As String
    Dim ActualValueArrayGB1() As String

    Dim InitialValueArrayGB2() As String 'Used for Memorizing all control values that are in Group Box 1. This is then compared for ISDirty function (Save)
    Dim ActualValueArrayGB2() As String 'Used for Memorizing all control values that are in Group Box 2. This is then compared for ISDirty function (Save)
    Dim strLastTreePath As String 'Used for the purpose of being able to collapse a NODE in the Tree when another NODE is selected

    Dim strTaskArray() As String 'Used for creating the Sequence Graphic. This array holds the Tasks that are configured for a PLC. The order of the Tasks is based on the Task Number
    Dim intTaskStartArray() As Integer
    Dim intTaskStopArray() As Integer

    Dim intStationDataDownloaded As Integer = 0
    Dim intTaskDataDownloaded As Integer = 0
    Dim intModelIDDataDownloaded As Integer = 0
    Dim intModelDataDataDownloaded As Integer = 0
    Dim intSendData As Integer = 0
    Dim intSendMax As Integer = 3
    Dim intSendError As Integer = 0

    Private Property UpdatePending As Integer = 0
    Private Property taskItemNumber As Integer = 1
    Private Property modelItemNumber As Integer = 1
    Private Property MaxNumberOfTasks As Integer = 1
    Private Property currentTaskItemNumber As Integer = 0

    Dim taskController As TaskController
    Dim taskConfigController As TaskConfigurationController
    Dim stationConfigController As StationConfigurationController
    Dim modelConfigController As ModelConfigurationController
    Dim areaConfigController As AreaConfigurationController
    Dim areaStruController As AreaStructureController
    Dim areaController As AreasController
    Dim plcConfigController As PlcConfigurationController
    Dim plcInfoController As PlcInformationController
    Dim stationsController As StationController
    Dim plantController As PlantController
    Dim sectionController As SectionsController



    Private Sub OpenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim bFileOpen As Boolean
        OpenFileDialog1.Title = "Open Exisiting Production Project File"
        OpenFileDialog1.CheckFileExists = True
        bFileOpen = OpenProjectFile()
        If bFileOpen Then
            'ChangeProject()
            'Get the Plant Data that is saved in the Database
            GetPlantDataFromDatabase()
            bISDirty = False
            tvwProject.SelectedNode = tvwProject.Nodes(0)
            'Set the Forms Title
            SetFormTitle()

            If strUserName = "Operator" Then
                DownloadAllConfigurationToolStripMenuItem.Enabled = False
                DownloadModelInformationToolStripMenuItem.Enabled = False
                DownloadStationConfigurationToolStripMenuItem.Enabled = False
                DownloadTaskConfigurationToolStripMenuItem.Enabled = False
                UploadModelConfigurationToolStripMenuItem.Enabled = False
                UploadProjectConfigurationToolStripMenuItem.Enabled = False
                UploadAllConfigurationToolStripMenuItem.Enabled = False
            Else
                DownloadAllConfigurationToolStripMenuItem.Enabled = True
                DownloadModelInformationToolStripMenuItem.Enabled = True
                DownloadStationConfigurationToolStripMenuItem.Enabled = True
                DownloadTaskConfigurationToolStripMenuItem.Enabled = True
                UploadModelConfigurationToolStripMenuItem.Enabled = True
                UploadProjectConfigurationToolStripMenuItem.Enabled = True
                UploadAllConfigurationToolStripMenuItem.Enabled = True
            End If

        End If
    End Sub

    Private Sub ChangeProject()
        Dim dbConnStrProject As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
        Dim scdbSqlHelper = New SqlHelper(dbConnStrProject)
        ''areaStructure
        Dim areaStructureController = New AreaStructureController(scdbSqlHelper)
        Dim listAreaStructure = areaStructureController.GetListAreaStructure
        ''stationStructure
        Dim stationStructureController = New StationStructureController(scdbSqlHelper)
        Dim listStationStructure = stationStructureController.GetListStationStructure
        ''taskStructure
        Dim taskStructureController = New TaskStructureController(scdbSqlHelper)
        Dim listTaskStructure = taskStructureController.GetListTaskStructure

        Dim dbConnStrMaster As String = My.Settings.DBConnectionString & My.Settings.DatabasePath & ";"
        Dim scdbSqlHelperMaster = New SqlHelper(dbConnStrMaster)
        ''areaStructure
        Dim areaStructureControllerMaster = New AreaStructureController(scdbSqlHelperMaster)
        Dim listAreaStructureMaster = areaStructureControllerMaster.GetListAreaStructure
        ''stationStructure
        Dim stationStructureControllerMaster = New StationStructureController(scdbSqlHelperMaster)
        Dim listStationStructureMaster = stationStructureControllerMaster.GetListStationStructure
        ''taskStructure
        Dim taskStructureControllerMaster = New TaskStructureController(scdbSqlHelperMaster)
        Dim listTaskStructureMaster = taskStructureControllerMaster.GetListTaskStructure

        Dim isEqual = New IsEqual
        Dim ischangeStation = isEqual.IsEqual(listStationStructure, listStationStructureMaster)
        Dim ischangeTask = isEqual.IsEqual(listTaskStructure, listTaskStructureMaster)

        If ischangeStation = True And ischangeTask = True Then
        Else
            Dim response = MsgBox("Master DB has changed. OK to Reload?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Application.ProductName)
            If response = 6 Then
                ''Area
                Dim areasController = New AreasController(scdbSqlHelper)
                Dim listAreas = areasController.GetListAreas
                ''AreaConfiguration
                Dim areaConfigurationController = New AreaConfigurationController(scdbSqlHelper)
                Dim listAreaConfiguration = areaConfigurationController.GetAreaConfiguration
                ''ModelConfiguration
                Dim modelConfigurationController = New ModelConfigurationController(scdbSqlHelper)
                Dim listModelConfiguration = modelConfigurationController.GetModelConfiguration
                ''Plant
                Dim plantController = New PlantController(scdbSqlHelper)
                Dim listPlant = plantController.GetListPlant
                ''PLCConfiguration
                Dim plcConfigurationController = New PlcConfigurationController(scdbSqlHelper)
                Dim listPlcConfiguration = plcConfigurationController.GetPlcConfiguration
                ''sections
                Dim sectionsController = New SectionsController(scdbSqlHelper)
                Dim listsections = sectionsController.GetListSections
                ''stations
                Dim stationController = New StationController(scdbSqlHelper)
                Dim listStation = stationController.GetListStations
                ''stationconfiguration
                Dim stationConfigurationController = New StationConfigurationController(scdbSqlHelper)
                Dim listStationConfiguration = stationConfigurationController.GetStationConfiguration
                ''Task
                Dim taskController = New TaskController(scdbSqlHelper)
                Dim listTask = taskController.GetListTasks
                ''Taskconfiguration
                Dim taskConfigurationController = New TaskConfigurationController(scdbSqlHelper)
                Dim listTaskConfiguration = taskConfigurationController.GetTaskConfiguration

                File.Delete(PowertrainProjectFile)
                FileCopy(My.Settings.DatabasePath, PowertrainProjectFile)
                ''insert area
                areasController.InsertAreas(listAreas)
                ''insert areaconfiguration
                areaConfigurationController.InsertAreaConfiguration(listAreaConfiguration)
                ''insert modelconfiguration
                modelConfigurationController.InsertModelConfiguration(listModelConfiguration)
                ''insert plant
                plantController.InsertPlant(listPlant)
                ''insert plcconfiguration
                plcConfigurationController.InsertPlcConfiguration(listPlcConfiguration)
                ''insert section
                sectionsController.InsertSerction(listsections)
                ''insert station
                stationController.InsertStation(listStation)
                ''insert stationconfiguration
                stationConfigurationController.InsertStationConfiguration(listStationConfiguration, listStation, listStationStructureMaster)
                ''insert task
                taskController.InsertTask(listTask)
                ''insert taskconfiguration
                taskConfigurationController.InsertTaskConfiguration(listTaskConfiguration, listTask, listTaskStructureMaster)
            Else
                Exit Sub
            End If

        End If

    End Sub


    ''' <summary>
    ''' Init all controllers used in this form
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitAllControllers()
        'Init controllers used in this form
        taskController = New TaskController()
        taskConfigController = New TaskConfigurationController()
        stationConfigController = New StationConfigurationController()
        modelConfigController = New ModelConfigurationController()
        areaConfigController = New AreaConfigurationController()
        areaStruController = New AreaStructureController()
        areaController = New AreasController()
        plcConfigController = New PlcConfigurationController()
        plcInfoController = New PlcInformationController()
        stationsController = New StationController()
        plantController = New PlantController()
        sectionController = New SectionsController()
        taskController.Init()
        taskConfigController.Init()
        stationConfigController.Init()
        modelConfigController.Init()
        areaConfigController.Init()
        areaStruController.Init()
        areaController.Init()
        plcConfigController.Init()
        plcInfoController.Init()
        stationsController.Init()
        plantController.Init()
        sectionController.Init()
    End Sub



    Private Sub CheckUserLevel()
        If strUserName = "Operator" Or strUserName = "Maintenance" Then
            SaveAsToolStripMenuItem.Enabled = False
            SaveToolStripMenuItem.Enabled = False

            DownloadAllConfigurationToolStripMenuItem.Enabled = False
            DownloadModelInformationToolStripMenuItem.Enabled = False
            DownloadStationConfigurationToolStripMenuItem.Enabled = False
            DownloadTaskConfigurationToolStripMenuItem.Enabled = False
            UploadModelConfigurationToolStripMenuItem.Enabled = False
            UploadProjectConfigurationToolStripMenuItem.Enabled = False
            UploadAllConfigurationToolStripMenuItem.Enabled = False
        Else
            SaveAsToolStripMenuItem.Enabled = True
            SaveToolStripMenuItem.Enabled = True

            DownloadAllConfigurationToolStripMenuItem.Enabled = True
            DownloadModelInformationToolStripMenuItem.Enabled = True
            DownloadStationConfigurationToolStripMenuItem.Enabled = True
            DownloadTaskConfigurationToolStripMenuItem.Enabled = True
            UploadModelConfigurationToolStripMenuItem.Enabled = True
            UploadProjectConfigurationToolStripMenuItem.Enabled = True
            UploadAllConfigurationToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub KeepLastFileNameOpened(strLastFileName As String)
        If strLastFileName = My.Settings.LastConfigFile01 Then

        ElseIf strLastFileName = My.Settings.LastConfigFile02 Then
            My.Settings.LastConfigFile02 = My.Settings.LastConfigFile01
            My.Settings.LastConfigFile01 = strLastFileName

        ElseIf strLastFileName = My.Settings.LastConfigFile03 Then
            My.Settings.LastConfigFile03 = My.Settings.LastConfigFile02
            My.Settings.LastConfigFile02 = My.Settings.LastConfigFile01
            My.Settings.LastConfigFile01 = strLastFileName

        ElseIf strLastFileName = My.Settings.LastConfigFile04 Then
            My.Settings.LastConfigFile04 = My.Settings.LastConfigFile03
            My.Settings.LastConfigFile03 = My.Settings.LastConfigFile02
            My.Settings.LastConfigFile02 = My.Settings.LastConfigFile01
            My.Settings.LastConfigFile01 = strLastFileName

        Else
            My.Settings.LastConfigFile04 = My.Settings.LastConfigFile03
            My.Settings.LastConfigFile03 = My.Settings.LastConfigFile02
            My.Settings.LastConfigFile02 = My.Settings.LastConfigFile01
            My.Settings.LastConfigFile01 = strLastFileName
        End If

        My.Settings.Save()
        SetupPreviousFilesMenu()
    End Sub

    Private Sub SetupPreviousFilesMenu()
        Dim intPosition As Integer

        PreviousFileOpen_01.Visible = False
        PreviousFileOpen_02.Visible = False
        PreviousFileOpen_03.Visible = False
        PreviousFileOpen_04.Visible = False

        If My.Settings.LastConfigFile01 <> "" Then
            PreviousFileOpen_01.Visible = True
            intPosition = InStrRev(My.Settings.LastConfigFile01, "\")
            PreviousFileOpen_01.Text = Mid(My.Settings.LastConfigFile01, intPosition + 1, Len(My.Settings.LastConfigFile01) - intPosition)
            PreviousFileOpen_01.Tag = My.Settings.LastConfigFile01
        End If


        If My.Settings.LastConfigFile02 <> "" Then
            PreviousFileOpen_02.Visible = True
            intPosition = InStrRev(My.Settings.LastConfigFile02, "\")
            PreviousFileOpen_02.Text = Mid(My.Settings.LastConfigFile02, intPosition + 1, Len(My.Settings.LastConfigFile02) - intPosition)
            PreviousFileOpen_02.Tag = My.Settings.LastConfigFile02
        End If

        If My.Settings.LastConfigFile03 <> "" Then
            PreviousFileOpen_03.Visible = True
            intPosition = InStrRev(My.Settings.LastConfigFile03, "\")
            PreviousFileOpen_03.Text = Mid(My.Settings.LastConfigFile03, intPosition + 1, Len(My.Settings.LastConfigFile03) - intPosition)
            PreviousFileOpen_03.Tag = My.Settings.LastConfigFile03
        End If


        If My.Settings.LastConfigFile04 <> "" Then
            PreviousFileOpen_04.Visible = True
            intPosition = InStrRev(My.Settings.LastConfigFile04, "\")
            PreviousFileOpen_04.Text = Mid(My.Settings.LastConfigFile04, intPosition + 1, Len(My.Settings.LastConfigFile04) - intPosition)
            PreviousFileOpen_04.Tag = My.Settings.LastConfigFile04
        End If
    End Sub

    Private Function OpenProjectFile() As Boolean

        OpenFileDialog1.Filter = "Production Project File (*.ppf)|*.ppf"
        OpenFileDialog1.InitialDirectory = My.Settings.ProjectFolder
        OpenFileDialog1.FileName = ""

        Dim result As DialogResult = OpenFileDialog1.ShowDialog()
        Try
            If result = DialogResult.OK Then
                If PowertrainProjectFile <> "" Then
                    AskToSave()
                End If
                PowertrainProjectFile = OpenFileDialog1.FileName
                If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                    PowertrainProjectFile = PowertrainProjectFile & ".ppf"
                End If
                KeepLastFileNameOpened(PowertrainProjectFile)

                'Init all controllers after open one ppf, 
                'Must be this time to init, because we use the 'PowertrainProjectFile' to open the access db
                InitAllControllers()

                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Log_Anything("OpenProjectFile - " & GetExceptionInfo(ex))
            Return False
        End Try
    End Function

    Private Sub AskToSave()
        Dim Response As Integer
        Try
            If IsArray(InitialValueArrayGB1) Then
                If UBound(InitialValueArrayGB1) > 0 Then
                    If ISDirtyGB1() Then
                        Response = MsgBox(GroupBox1.Tag & " Values have Changed. Would you like to save your work?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                        If Response = 6 Then
                            cmdSave_Main.PerformClick()
                        End If
                    End If
                End If
            End If

            If IsArray(InitialValueArrayGB2) Then
                If UBound(InitialValueArrayGB2) > 0 Then
                    If ISDirtyGB2() Then
                        Response = MsgBox("Model Values have Changed. Would you like to save your work?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                        If Response = 6 Then
                            cmdSave_Model.PerformClick()
                        End If
                    End If
                End If
            End If

            'If bISDirty = True Then
            '    nResult = MsgBox("Changes have been made and not saved. " & vbCrLf & "Do you want to save your changes?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
            '    If nResult = 6 Then
            '        'SetPlantDataToDatabase()
            '    End If
            '    bISDirty = False
            'End If
        Catch ex As Exception
            Log_Anything("AskToSave - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Public Sub SetFormTitle()
        'Add Form Title and Version
        Me.Text = CommonController.GenerateProductionTitle(PowertrainProjectFile)
        ExpandTreeNodes()
    End Sub

    Public Sub ExpandTreeNodes()
        For Each MainNode In tvwProject.Nodes(0).Nodes
            MainNode.ExPand()
            For Each SectionNode In MainNode.Nodes
                SectionNode.Expand()
                For Each StationNode In SectionNode.Nodes
                    StationNode.Expand()
                    For Each TaskNode In StationNode.Nodes
                        TaskNode.Expand()
                    Next
                Next
            Next
        Next
    End Sub

    Private Sub GetPlantDataFromDatabase()
        Try
            tvwProject.Nodes.Clear()

            Dim mainNode As New TreeNode()
            mainNode.Name = "mainNode"
            mainNode.Text = plantController.GetPlantName()
            mainNode.Tag = TreeNodeTagConstant.PLANT
            mainNode.ImageIndex = TreeNodeImageIndexConstant.Plant
            Me.tvwProject.Nodes.Add(mainNode)
            tvwProject.Sort()

            Dim areaList = areaController.GetListAreas()
            For Each area In areaList
                AddAreaToTree(area.AreaName)
            Next

            Dim sectionList = sectionController.GetListSections()
            For Each section In sectionList
                AddSectionToTree(section.AreaName, section.SectionName)
            Next

            Dim stationList = stationsController.GetListStations()
            For Each station In stationList
                AddStationToTree(station.AreaName, station.SectionName, station.StationName)
                If Not String.IsNullOrWhiteSpace(station.PlcType) Then
                    AddPLCToProjectTree(station.AreaName, station.SectionName, station.StationName, station.PlcType)
                    If Not String.IsNullOrWhiteSpace(station.MasterFileName) Then
                        AddMasterFileToProjectTree(station.AreaName, station.SectionName, station.StationName, station.MasterFileName)
                    End If
                End If
                'If row(9).ToString > 1 Then
                '    For i = 1 To row(9).ToString
                '        AddStationInstanceToTree(row(1), row(2), row(3), i)
                '    Next
                'End If
            Next

            Dim taskList = taskController.GetListTasks()
            For Each task In taskList
                AddTaskToProjectTree(task.AreaName, task.SectionName, task.StationName, task.MasterFileName, task.TaskName)
                If task.MaxNoOfInstances > 1 Then
                    For i = 1 To task.MaxNoOfInstances
                        AddTaskInstanceToProjectTree(task.AreaName, task.SectionName, task.StationName, task.MasterFileName, task.TaskName, i)
                    Next
                End If
            Next

            tvwProject.Refresh()
            Me.Refresh()

            VerifyProjectFile()
        Catch ex As Exception
            Log_Anything("GetPlantDataFromDatabase - " & GetExceptionInfo(ex))
        End Try

    End Sub

    'Private Sub AddStationInstanceToTree(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal intInstance As Integer)
    '    Dim nod As New TreeNode()
    '    Dim strInstance As String

    '    If intInstance < 10 Then
    '        strInstance = "0" & intInstance
    '    Else
    '        strInstance = intInstance
    '    End If

    '    nod.Name = strStationName
    '    nod.Text = strInstance & "-" & strStationName
    '    nod.Tag = "STATION|" & intInstance
    '    nod.ImageIndex = 5
    '    nod.SelectedImageIndex = 5

    '    If strAreaName = "" Then
    '        MsgBox("Area Name cannot be empty", MsgBoxStyle.Critical, Application.ProductName)
    '        Exit Sub
    '    End If

    '    If strSectionName = "" Then
    '        MsgBox("Section Name cannot be empty", MsgBoxStyle.Critical, Application.ProductName)
    '        Exit Sub
    '    End If

    '    If strStationName = "" Then
    '        MsgBox("Station Name cannot be empty", MsgBoxStyle.Critical, Application.ProductName)
    '        Exit Sub
    '    End If

    '    For Each AreaNode In tvwProject.Nodes(0).Nodes
    '        If AreaNode.Text = strAreaName Then
    '            For Each sectionNode In AreaNode.Nodes
    '                If sectionNode.Text = strSectionName Then
    '                    For Each stationNode In sectionNode.Nodes
    '                        If stationNode.Text = strStationName Then

    '                            stationNode.Nodes.add(nod)
    '                            bISDirty = True

    '                        End If
    '                    Next
    '                End If
    '            Next
    '        End If
    '    Next
    'End Sub


    Private Sub AddAreaToTree(ByVal strAreaName As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.Area, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodeArea(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwProject, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
            parentNode.ExpandAll()
            tvwProject.Sort()
            bISDirty = True

        Catch ex As Exception
            Log_Anything("AddAreaToTree - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub AddSectionToTree(ByVal strAreaName As String, ByVal strSectionName As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName, strSectionName)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.Section, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodeSection(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwProject, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
            parentNode.ExpandAll()
            bISDirty = True

            'cboSections.Items.Clear()

            'For Each AreaNode In tvwProject.Nodes(0).Nodes
            '    If AreaNode.Name = strAreaName Then
            '        For Each sectionNode In AreaNode.nodes
            '            cboSections.Items.Add(sectionNode.Name)
            '        Next
            '    End If
            'Next
        Catch ex As Exception
            Log_Anything("AddSectionToTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub AddStationToTree(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName, strSectionName, strStationName)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.Station, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodeStation(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwProject, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
            parentNode.ExpandAll()
            bISDirty = True

        Catch ex As Exception
            Log_Anything("AddStationToTree - " & GetExceptionInfo(ex))
        End Try
    End Sub


    Private Sub AddTaskToProjectTree(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMasterFile As String, ByVal strTaskName As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName, strSectionName, strStationName, strTaskName, strMasterFile)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.Task, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodeTask(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwProject, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
            bISDirty = True

        Catch ex As Exception
            Log_Anything("AddTaskToProjectTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub AddTaskInstanceToProjectTree(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMasterFile As String, ByVal strTaskName As String, ByVal intInstance As Integer)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName, strSectionName, strStationName, strTaskName, strMasterFile, DefaultValueConstant.TreeNodeHierarchy, DefaultValueConstant.TreeNodeHierarchy, intInstance)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.TaskInstance, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodeTaskInstance(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwProject, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
            bISDirty = True

        Catch ex As Exception
            Log_Anything("AddInstanceTaskToProjectTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub AddPLCToProjectTree(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strPLCName As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName, strSectionName, strStationName, DefaultValueConstant.TreeNodeHierarchy, DefaultValueConstant.TreeNodeHierarchy, strPLCName)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.Plc, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodePlc(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwProject, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
            bISDirty = True

        Catch ex As Exception
            Log_Anything("AddPLCToProjectTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Function AddMasterFileToProjectTree(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strMasterFile As String) As Boolean
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName, strSectionName, strStationName, DefaultValueConstant.TreeNodeHierarchy, strMasterFile)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.MasterFile, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodeMasterFile(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwProject, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Return False
            End If

            Dim bDeleteTaskNodes As Boolean
            Dim bMasterFileFound As Boolean

            Dim masterFileNode As TreeNode = parentNode.Nodes.Cast(Of Object).FirstOrDefault(Function(x) x.Tag.Equals(TreeNodeTagConstant.MASTERFILE))
            If masterFileNode IsNot Nothing Then
                bMasterFileFound = True
                If masterFileNode.Name <> strMasterFile Then
                    Dim result = MsgBox("Different MasterFile already defined for this Area\Section\Station. If you proceed then all Tasks will be deleted from this project. Do you want to continue?", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, Application.ProductName)
                    If result = MsgBoxResult.Yes Then
                        bDeleteTaskNodes = True
                    Else
                        Return False
                    End If
                End If
            End If

            'delete the nodes related to different original master file node
            Dim removedNodeList = New List(Of String)()
            If bDeleteTaskNodes = True Then
                For Each masterNode In parentNode.Nodes
                    If masterNode.Tag <> TreeNodeTagConstant.PLC Then
                        removedNodeList.Add(masterNode.Name)
                    End If
                Next
                For Each nodeName In removedNodeList
                    parentNode.Nodes(nodeName).Remove()
                Next
            End If

            'add node
            If bMasterFileFound = False Or bDeleteTaskNodes = True Then
                parentNode.Nodes.Add(nod)
                bISDirty = True
            End If

            Return True
        Catch ex As Exception
            Log_Anything("AddMasterFileToProjectTree - " & GetExceptionInfo(ex))
            Return False
        End Try
    End Function

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        AskToSave()
        PowertrainProjectFile = ""
        tvwProject.Nodes.Clear()
        DeleteConfigurationItemsFromGroupBox()
        DeleteConfigurationItemsFromModelGroupBox()
        SplitContainer1.Visible = True
        SplitContainer2.Visible = True
        tvwProject.Nodes.Clear()
        intNumberOfModelsVisible = 0
        strAreaConfigurationName = ""
        strAreaConfigurationPosition = ""
        strAreaConfigurationType = ""
        strAreaModelDescription = ""
        cmdSave_Main.Visible = False
        cmdSave_Model.Visible = False
        ReDim InitialValueArrayGB1(0)
        ReDim InitialValueArrayGB2(0)
        Form5.Show()
        Me.Close()
    End Sub

    Private Sub CloseProjectToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CloseProjectToolStripMenuItem.Click
        If PowertrainProjectFile <> "" Then
            AskToSave()
        End If

        PowertrainProjectFile = ""
        tvwProject.Nodes.Clear()
        DeleteConfigurationItemsFromGroupBox()
        DeleteConfigurationItemsFromModelGroupBox()
        SplitContainer1.Visible = True
        SplitContainer2.Visible = True
        tvwProject.Nodes.Clear()
        intNumberOfModelsVisible = 0
        strAreaConfigurationName = ""
        strAreaConfigurationPosition = ""
        strAreaConfigurationType = ""
        strAreaModelDescription = ""
        cmdSave_Main.Visible = False
        cmdSave_Model.Visible = False
        ReDim InitialValueArrayGB1(0)
        ReDim InitialValueArrayGB2(0)
    End Sub

    Private Function CheckAreaConfiguration() As Boolean
        Try
            Dim msg = String.Empty
            Dim areaStructureModelIdObj = areaStruController.GetListAreaStructure(AreaStructureConstant.ModelID).FirstOrDefault()
            If IsNothing(areaStructureModelIdObj) Then
                Return False
            End If

            Dim tmpMemberLength = CommonController.GetStringTypeLength(areaStructureModelIdObj.MemberType)
            Dim modelConfigList = GenerateAreaConfigurationListFromUi()

            Dim valResult = areaConfigController.CheckAreaConfigurationEmpty(modelConfigList, msg)
            If valResult = False Then
                MsgBox(msg, vbCritical, Application.ProductName)
                Return False
            End If

            valResult = areaConfigController.CheckAreaConfigurationLength(modelConfigList, tmpMemberLength, msg)
            If valResult = False Then
                MsgBox(msg, vbCritical, Application.ProductName)
                Return False
            End If

            valResult = areaConfigController.CheckDupliatedAreaConfiguration(modelConfigList, msg)
            If valResult = False Then
                MsgBox(msg, vbCritical, Application.ProductName)
                Return False
            End If

            Return True
        Catch ex As Exception
            Log_Anything("CheckAreaConfiguration - " & GetExceptionInfo(ex))
            Return False
        End Try
    End Function

    Private Sub SetAreaConfigurationToDatabase()
        Try
            'to-do: Need insert the new area configuration and update the existing ones
            areaConfigController.DeleteAreaConfiguration(lbl_TaskType.Text)
            Dim areaConfigList = GenerateAreaConfigurationListFromUi()
            areaConfigController.InsertAreaConfiguration(areaConfigList)
            bISDirty = False
        Catch ex As Exception
            Log_Anything("SetAreaConfigurationToDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Function GenerateAreaConfigurationListFromUi() As List(Of AreaConfiguration)
        Dim areaConfigList = New List(Of AreaConfiguration)
        For i = 1 To intNumberOfModelsVisible
            For Each ctrl In GroupBox1.Controls
                If ctrl.name = "txt_" & strAreaConfigurationName & "_" & i Then
                    Dim areaConfig = New AreaConfiguration(lbl_TaskType.Text, i.ToString(), ctrl.text)
                    areaConfigList.Add(areaConfig)
                    Exit For
                End If
            Next
        Next
        Return areaConfigList
    End Function

    Private Sub GetAreaConfigurationFromDatabase()
        Try
            Dim areaConfigList = areaConfigController.GetAreaConfigurationByStation(lbl_TaskType.Text)
            txt_TaskNumber.Text = areaConfigList.Count
            AddEmptyModels()

            Dim i = 1
            For Each areaConfig In areaConfigList
                Dim ctrlName = "txt_" & strAreaConfigurationName & "_" & i
                Dim ctrl = CType(GroupBox1.Controls(ctrlName), TextBox)
                ctrl.Text = areaConfig.ModelName
                i += 1
            Next

            intNumberOfModelsVisible = areaConfigList.Count

            'Bind the mouse right click event
            For Each ctrl In GroupBox1.Controls
                Dim ctrlName = ctrl.Name.ToString()
                If ctrlName.Contains("txt_ModelID") Then
                    ctrl.contextmenustrip = ContextCutCopyPaste
                End If
            Next

            bISDirty = False
        Catch ex As Exception
            Log_Anything("GetAreaConfigurationFromDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub GetAreaStructureFromDatabase(ByVal strAreAName As String)
        Try
            'Remove any extra controls before adding new controls
            DeleteConfigurationItemsFromGroupBox()

            Dim areaStructureModelIdObj = areaStruController.GetListAreaStructure(AreaStructureConstant.ModelID).FirstOrDefault()
            If IsNothing(areaStructureModelIdObj) Then
                Return
            End If

            'Get Configuration Setup information
            If String.IsNullOrWhiteSpace(areaStructureModelIdObj.MemberDescription1) Then
                strAreaModelDescription = UiDisplayConstant.AreaModelDefaultDescription
            Else
                strAreaModelDescription = areaStructureModelIdObj.MemberDescription1
            End If
            strAreaConfigurationName = areaStructureModelIdObj.MemberName
            strAreaConfigurationType = areaStructureModelIdObj.MemberType
            strAreaConfigurationPosition = areaStructureModelIdObj.MemberOrder
            AddConfigurationItemToGroupBox(strAreaConfigurationType, strAreaConfigurationName, strAreaConfigurationName & "_" & 1, taskItemNumber & " - " & strAreaModelDescription, strAreaConfigurationPosition, "", "", "")

            GenerateDefaultStatusBasedOnAreaStructure(strAreAName)

        Catch ex As Exception
            Log_Anything("GetAreaStructureFromDatabase - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub GenerateDefaultStatusBasedOnAreaStructure(ByVal strAreAName As String)
        Try
            lbl_TaskType_lbl.Visible = True
            lbl_TaskType_lbl.Text = UiDisplayConstant.AreaName
            lbl_TaskType.Visible = True
            lbl_TaskType.Text = strAreAName

            txt_TaskName_lbl.Visible = False
            txt_TaskName.Visible = False

            chk_TaskBypassed.Checked = False
            chk_TaskBypassed.Visible = False
            taskItemNumber = 1

            bFirstTimeAreaIsLoaded = True
            txt_TaskNumber.Visible = True
            txt_TaskNumber.Text = 1
            txt_TaskNumber_lbl.Visible = True
            txt_TaskNumber_lbl.Text = UiDisplayConstant.NoOfModels
            intNumberOfModelsVisible = 1

        Catch ex As Exception
            Log_Anything("GenerateDefaultStatusBasedOnAreaStructure - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub SetStationConfigurationToDatabase()

        Try
            Dim Table_ As String = "Stations_Configuration"
            Dim query As String = "SELECT * FROM " & Table_
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim cmd As New OleDbCommand
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String
            Dim intTagNameHasBaseInIt As Integer
            Dim strValuesArray() As String
            Dim strSingleValueArray() As String
            Dim i As Integer
            Dim strTypeInfo() As String


            Dim strTypeAndLengthOfIndexArray() As String
            Dim strParentMemberAndDisplayText() As String
            Dim strParentAndMember() As String
            Dim strValues As String = ""
            Dim bTypeIsBool As Boolean
            Dim j As Integer

            strStationName = tvwProject.SelectedNode.Text
            strSectionName = tvwProject.SelectedNode.Parent.Text
            strAreaName = tvwProject.SelectedNode.Parent.Parent.Text

            cnn.Open()

            query = "DELETE * FROM Stations_Configuration WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "'"
            cmd = New OleDbCommand(query, cnn)
            cmd.ExecuteNonQuery()

            For Each ctrl In GroupBox1.Controls
                query = ""
                If ctrl.Visible = True And (TypeOf ctrl Is TextBox Or TypeOf ctrl Is ComboBox Or TypeOf ctrl Is CheckBox Or TypeOf ctrl Is Label Or TypeOf ctrl Is Button) Then
                    intTagNameHasBaseInIt = InStr(ctrl.tag, "BASE")
                    If intTagNameHasBaseInIt = 0 Then
                        If TypeOf ctrl Is TextBox Then
                            query = "Insert INTO Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & ctrl.Tag & "','" & ctrl.Text & "','" & ctrl.AccessibleDescription & "','Config')"
                        ElseIf TypeOf ctrl Is ComboBox Then
                            query = "Insert INTO Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & ctrl.Tag & "','" & ctrl.Text & "','" & ctrl.AccessibleDescription & "','Config')"
                        ElseIf TypeOf ctrl Is CheckBox Then
                            query = "Insert INTO Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & ctrl.Tag & "','" & ctrl.checked & "','" & ctrl.AccessibleDescription & "','Config')"
                        ElseIf TypeOf ctrl Is Button Then
                            'Button1.AccessibleDescription
                            strTypeInfo = Split(ctrl.AccessibleName, ",")

                            If ctrl.AccessibleDescription = "" Then
                                strTypeAndLengthOfIndexArray = Split(ctrl.AccessibleName, ",")
                                strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "[", "")
                                strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "]", "")

                                strParentMemberAndDisplayText = Split(ctrl.Tag, "|")
                                strParentAndMember = Split(strParentMemberAndDisplayText(0), ".")

                                'intTaskItemNumber = 1
                                bTypeIsBool = False
                                If InStr(strTypeAndLengthOfIndexArray(1), "BOOL") Then
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "BOOL", "")
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "(", "")
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), ")", "")
                                    bTypeIsBool = True
                                End If

                                For j = 1 To strTypeAndLengthOfIndexArray(1)
                                    If strValues = "" Then
                                        If bTypeIsBool = False Then
                                            strValues = strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & 0
                                        Else
                                            strValues = strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & "False"
                                        End If

                                    Else
                                        If bTypeIsBool = False Then
                                            strValues = strValues & "|" & strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & 0
                                        Else
                                            strValues = strValues & "|" & strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & "False"
                                        End If
                                    End If
                                Next
                                ctrl.AccessibleDescription = strValues
                            End If

                            strValuesArray = Split(ctrl.AccessibleDescription, "|")
                            For i = LBound(strValuesArray) To UBound(strValuesArray)
                                strSingleValueArray = Split(strValuesArray(i), ",")
                                query = "Insert INTO Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strSingleValueArray(0) & "','" & strSingleValueArray(1) & "','" & strTypeInfo(0) & "','Config')"
                                cmd = New OleDbCommand(query, cnn)
                                cmd.ExecuteNonQuery()
                            Next
                            query = ""
                        End If
                    Else
                        intTagNameHasBaseInIt = InStr(ctrl.tag, "_")
                        If intTagNameHasBaseInIt = 0 Then
                            If TypeOf ctrl Is TextBox Or TypeOf ctrl Is Label Then
                                query = "Insert INTO Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & ctrl.AccessibleName & "','" & ctrl.Text & "','" & ctrl.AccessibleDescription & "','Config')"
                            ElseIf TypeOf ctrl Is ComboBox Then
                                query = "Insert INTO Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & ctrl.AccessibleName & "','" & ctrl.Text & "','" & ctrl.AccessibleDescription & "','Config')"
                            ElseIf TypeOf ctrl Is CheckBox Then
                                query = "Insert INTO Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & ctrl.AccessibleName & "','" & ctrl.checked & "','" & ctrl.AccessibleDescription & "','Config')"
                            ElseIf TypeOf ctrl Is Button Then
                                'Button1.AccessibleDescription
                                strTypeInfo = Split(ctrl.AccessibleName, ",")

                                If ctrl.AccessibleDescription = "" Then
                                    strTypeAndLengthOfIndexArray = Split(ctrl.AccessibleName, ",")
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "[", "")
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "]", "")

                                    strParentMemberAndDisplayText = Split(ctrl.Tag, "|")
                                    strParentAndMember = Split(strParentMemberAndDisplayText(0), ".")

                                    'intTaskItemNumber = 1
                                    bTypeIsBool = False
                                    If InStr(strTypeAndLengthOfIndexArray(1), "BOOL") Then
                                        strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "BOOL", "")
                                        strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "(", "")
                                        strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), ")", "")
                                        bTypeIsBool = True
                                    End If

                                    For j = 1 To strTypeAndLengthOfIndexArray(1)
                                        If strValues = "" Then
                                            If bTypeIsBool = False Then
                                                strValues = strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & 0
                                            Else
                                                strValues = strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & "False"
                                            End If

                                        Else
                                            If bTypeIsBool = False Then
                                                strValues = strValues & "|" & strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & 0
                                            Else
                                                strValues = strValues & "|" & strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & "False"
                                            End If
                                        End If
                                    Next
                                    ctrl.AccessibleDescription = strValues
                                End If

                                strValuesArray = Split(ctrl.AccessibleDescription, "|")
                                For i = LBound(strValuesArray) To UBound(strValuesArray)
                                    strSingleValueArray = Split(strValuesArray(i), ",")
                                    query = "Insert INTO Stations_Configuration (Area_Name,Section_Name,Station_Name,Member_Name,Member_Value,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strSingleValueArray(0) & "','" & strSingleValueArray(1) & "','" & strTypeInfo(0) & "','Config')"
                                    cmd = New OleDbCommand(query, cnn)
                                    cmd.ExecuteNonQuery()
                                Next
                                query = ""
                            End If
                        End If
                    End If
                    If query <> "" Then
                        cmd = New OleDbCommand(query, cnn)
                        cmd.ExecuteNonQuery()
                    End If
                End If
            Next

            cnn.Close()
            bISDirty = False
        Catch ex As Exception
            Log_Anything("SetStationConfigurationToDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub GetStationConfigurationFromDatabase()

        Try
            Dim Table_ As String = "Stations_Configuration"
            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String
            Dim intTagNameHasBASEInIt As Integer


            strStationName = tvwProject.SelectedNode.Text
            strSectionName = tvwProject.SelectedNode.Parent.Text
            strAreaName = tvwProject.SelectedNode.Parent.Parent.Text

            Dim query As String = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "'"
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)

            cnn.Open()
            Dim cmd As New OleDbCommand(query, cnn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)

            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow

            '' ''For Each row In t1.Rows
            '' ''    For Each ctrl In GroupBox1.Controls
            '' ''        intTagNameHasBASEInIt = InStr(ctrl.tag, "BASE")
            '' ''        If intTagNameHasBASEInIt = 0 Then
            '' ''            If ctrl.Tag = row(4).ToString Then
            '' ''                If TypeOf ctrl Is TextBox Then
            '' ''                    ctrl.text = row(5).ToString
            '' ''                ElseIf TypeOf ctrl Is ComboBox Then
            '' ''                    ctrl.text = row(5).ToString
            '' ''                ElseIf TypeOf ctrl Is CheckBox Then
            '' ''                    ctrl.checked = row(5).ToString
            '' ''                End If
            '' ''            End If
            '' ''        Else
            '' ''            intTagNameHasBASEInIt = InStr(ctrl.tag, "_")
            '' ''            If intTagNameHasBASEInIt = 0 Then
            '' ''                If ctrl.AccessibleName = row(4).ToString Then
            '' ''                    If TypeOf ctrl Is TextBox Then
            '' ''                        ctrl.text = row(5).ToString
            '' ''                    ElseIf TypeOf ctrl Is ComboBox Then
            '' ''                        ctrl.text = row(5).ToString
            '' ''                    ElseIf TypeOf ctrl Is CheckBox Then
            '' ''                        ctrl.checked = row(5).ToString
            '' ''                    End If
            '' ''                End If
            '' ''            End If
            '' ''        End If
            '' ''    Next
            '' ''Next



            Dim strTempNameAndDecription() As String
            Dim strTagName As String

            If t1.Rows.Count <> 0 Then
                For Each row In t1.Rows
                    For Each ctrl In GroupBox1.Controls
                        strTagName = ctrl.Tag
                        intTagNameHasBASEInIt = InStr(ctrl.tag, "BASE")
                        If intTagNameHasBASEInIt = 0 Then
                            If ctrl.Tag = row(4).ToString And Not TypeOf ctrl Is Button Then
                                If TypeOf ctrl Is TextBox Then
                                    ctrl.text = row(5).ToString
                                ElseIf TypeOf ctrl Is ComboBox Then
                                    If IsNumeric(row(5).ToString) Then
                                        If row(5).ToString > 0 Then
                                            Dim strPosition() As String
                                            strPosition = Split(ctrl.accessibleName, ",")
                                            If UBound(strPosition) > 0 Then
                                                For j = 0 To UBound(strPosition) - 1
                                                    If strPosition(j) = row(5).ToString Then
                                                        ctrl.SelectedIndex = j
                                                        Exit For
                                                    End If
                                                Next
                                            Else
                                                ctrl.SelectedIndex = 0
                                            End If
                                            'ctrl.SelectedIndex = row(5).ToString - 1
                                        Else
                                            ctrl.SelectedIndex = 0
                                        End If
                                    Else
                                        ctrl.SelectedIndex = 0
                                    End If
                                ElseIf TypeOf ctrl Is CheckBox Then
                                    ctrl.checked = row(5).ToString
                                End If
                                Exit For
                            ElseIf TypeOf ctrl Is Button Then
                                'MsgBox(ctrl.tag)
                                strTempNameAndDecription = Split(ctrl.tag, "|")
                                If InStr(row(4).ToString, strTempNameAndDecription(0)) Then
                                    'Button1.AccessibleDescription
                                    'MsgBox(ctrl.AccessibleDescription)
                                    If ctrl.AccessibleDescription = "" Or ctrl.AccessibleDefaultActionDescription <> 1 Then
                                        ctrl.AccessibleDescription = row(4).ToString & "," & row(5).ToString
                                        ctrl.AccessibleDefaultActionDescription = 1
                                    Else
                                        ctrl.AccessibleDescription = ctrl.AccessibleDescription & "|" & row(4).ToString & "," & row(5).ToString
                                    End If
                                    Exit For
                                End If
                            End If
                        Else
                            intTagNameHasBASEInIt = InStr(ctrl.tag, "_")
                            If intTagNameHasBASEInIt = 0 Then
                                If ctrl.AccessibleName <> "" Then
                                    If ctrl.AccessibleName = row(4).ToString Then
                                        If TypeOf ctrl Is TextBox Then
                                            ctrl.text = row(5).ToString
                                        ElseIf TypeOf ctrl Is ComboBox Then
                                            If IsNumeric(row(5).ToString) Then
                                                If row(5).ToString > 0 Then
                                                    ctrl.SelectedIndex = row(5).ToString - 1
                                                Else
                                                    ctrl.SelectedIndex = 0
                                                End If
                                            Else
                                                ctrl.SelectedIndex = 0
                                            End If
                                        ElseIf TypeOf ctrl Is CheckBox Then
                                            ctrl.checked = row(5).ToString
                                        End If
                                        Exit For
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            Else
                For Each ctrl In GroupBox1.Controls
                    strTagName = ctrl.Tag
                    intTagNameHasBASEInIt = InStr(ctrl.tag, "BASE")
                    If intTagNameHasBASEInIt = 0 Then
                        If Not TypeOf ctrl Is Button Then
                            If TypeOf ctrl Is TextBox Then
                                ctrl.text = 0
                            ElseIf TypeOf ctrl Is ComboBox Then
                                ctrl.text = ctrl.items(0).ToString
                            ElseIf TypeOf ctrl Is CheckBox Then
                                ctrl.checked = False
                            End If
                            'Exit For
                        ElseIf TypeOf ctrl Is Button Then
                            'MsgBox(ctrl.tag)
                            strTempNameAndDecription = Split(ctrl.tag, "|")
                            'If InStr(row(6).ToString, strTempNameAndDecription(0)) Then
                            '    'Button1.AccessibleDescription
                            '    'MsgBox(ctrl.AccessibleDescription)
                            '    If ctrl.AccessibleDescription = "" Then
                            '        ctrl.AccessibleDescription = row(6).ToString & "," & row(7).ToString
                            '    Else
                            '        ctrl.AccessibleDescription = ctrl.AccessibleDescription & "|" & row(6).ToString & "," & row(7).ToString
                            '    End If
                            '    Exit For
                            'End If
                        End If
                    Else
                        intTagNameHasBASEInIt = InStr(ctrl.tag, "_")
                        If intTagNameHasBASEInIt = 0 Then
                            If ctrl.AccessibleName <> "" Then

                                If TypeOf ctrl Is TextBox Then
                                    ctrl.text = 0
                                ElseIf TypeOf ctrl Is ComboBox Then
                                    ctrl.text = ctrl.items(0).ToString
                                ElseIf TypeOf ctrl Is CheckBox Then
                                    ctrl.checked = False
                                End If
                                'Exit For
                            End If
                        End If
                    End If
                Next
            End If

            cnn.Close()
            bISDirty = False
        Catch ex As Exception
            Log_Anything("GetStationConfigurationFromDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub GetStationStructureFromDatabase(ByVal strStationName As String)
        Try
            Dim gTable_ As String = "Station_Structures"
            Dim Table_ As String = "Station_Structures"
            Dim bSkipThisRecord As Boolean

            Dim query As String = "SELECT * FROM " & Table_ & " WHERE Parent = 'StationConfig' AND Visible = 1 ORDER BY MEMBERORDER"
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim strTempType As String
            Dim intPositionOfIndex As Integer
            Dim strTypeAndLengthOfIndex As String = ""

            cnn.Open()


            Dim gDS As New DataSet

            Dim cmd As OleDbCommand
            Dim da As OleDbDataAdapter
            Dim t1 As DataTable
            Dim row As DataRow

            'Remove any extra controls before adding new controls
            DeleteConfigurationItemsFromGroupBox()

            lbl_TaskType.Text = strStationName
            taskItemNumber = 1


            ds.Reset()
            'query = "SELECT * FROM " & Table_ & " WHERE Parent = '" & gRow(0).ToString & "' AND Visible = 1 ORDER BY MEMBERORDER"
            query = "SELECT * FROM " & Table_ & " WHERE Visible = 1 ORDER BY MEMBERORDER"
            cmd = New OleDbCommand(query, cnn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)
            t1 = ds.Tables(Table_)

            'Get Configuration Setup information
            For Each row In t1.Rows
                bSkipThisRecord = False
                If IsNumeric(row(12).ToString) Then
                    If row(12).ToString > 0 Then
                        bSkipThisRecord = True
                        For Each ctrl In GroupBox1.Controls
                            If ctrl.Tag = "BASE" & row(12).ToString Then
                                ctrl.Visible = True
                                ctrl.AccessibleName = row(1).ToString & "." & row(2).ToString
                                ctrl.AccessibleDescription = row(3).ToString
                                If TypeOf ctrl Is CheckBox Then
                                    If row(intGlobalDescriptionToUse).ToString <> "" Then
                                        ctrl.Text = row(intGlobalDescriptionToUse).ToString
                                    Else
                                        ctrl.text = row(2).ToString
                                    End If
                                End If
                                'Exit For
                            ElseIf ctrl.tag = "BASE" & row(12).ToString & "_0" Then
                                ctrl.Visible = True
                                If row(intGlobalDescriptionToUse).ToString <> "" Then
                                    ctrl.Text = row(intGlobalDescriptionToUse).ToString
                                Else
                                    ctrl.text = row(2).ToString
                                End If
                                'Exit For
                            End If
                        Next
                    End If
                End If
                'If IsNumeric(row(12).ToString) Then
                '    If row(12).ToString <> 0 Then
                '        bSkipThisRecord = True
                '    End If
                'End If

                If row(2).ToString = "SPACE" Then
                    taskItemNumber += 1
                    bSkipThisRecord = True
                End If

                If bSkipThisRecord = False Then
                    intPositionOfIndex = InStr(row(3).ToString, "[")
                    If intPositionOfIndex <> 0 Then
                        strTempType = Mid(row(3).ToString, 1, intPositionOfIndex - 1)
                        strTypeAndLengthOfIndex = strTempType & "," & Mid(row(3).ToString, intPositionOfIndex, Len(row(3).ToString) - (intPositionOfIndex - 1))
                    Else
                        strTempType = row(3).ToString
                        strTypeAndLengthOfIndex = ""
                    End If

                    If strTempType = "BOOL" Then
                        AddConfigurationItemToGroupBox(row(3).ToString, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, row("MemberGroup").ToString)
                    ElseIf strTempType = "SINT" Then
                        AddConfigurationItemToGroupBox(row(3).ToString, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                    ElseIf strTempType = "INT" Then
                        AddConfigurationItemToGroupBox(strTempType, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                    ElseIf strTempType = "DINT" Then
                        AddConfigurationItemToGroupBox(row(3).ToString, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                    ElseIf strTempType = "REAL" Then
                        AddConfigurationItemToGroupBox(row(3).ToString, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                    ElseIf strTempType = "STRING" Then
                        AddConfigurationItemToGroupBox(row(3).ToString, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, "", "")
                    End If
                End If
            Next

            cnn.Close()
        Catch ex As Exception
            Log_Anything("GetStationStructureFromDatabase - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub SetTaskConfigurationToDatabase(ByVal intTaskInstance As Integer)

        Try
            Dim Table_ As String = "Tasks_Configuration"
            Dim query As String = "SELECT * FROM " & Table_
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim cmd As New OleDbCommand
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String
            Dim strTaskName As String
            Dim intTagNameHasBaseInIt As Integer
            Dim strValuesArray() As String
            Dim strSingleValueArray() As String
            Dim i As Integer

            Dim strTypeAndLengthOfIndexArray() As String
            Dim strParentMemberAndDisplayText() As String
            Dim strParentAndMember() As String
            Dim strValues As String = ""
            Dim bTypeIsBool As Boolean
            Dim j As Integer
            Dim strTypeInfo() As String

            If tvwProject.SelectedNode.Parent.Tag <> "TASK" Then
                strTaskName = tvwProject.SelectedNode.Text
                strStationName = tvwProject.SelectedNode.Parent.Parent.Text
                strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
            Else
                strTaskName = tvwProject.SelectedNode.Name
                strStationName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Parent.Text
            End If

            cnn.Open()

            query = "DELETE * FROM Tasks_Configuration WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "' AND TASK_Instance = " & intTaskInstance
            cmd = New OleDbCommand(query, cnn)
            cmd.ExecuteNonQuery()

            For Each ctrl In GroupBox1.Controls
                query = ""
                If ctrl.Visible = True And (TypeOf ctrl Is TextBox Or TypeOf ctrl Is ComboBox Or TypeOf ctrl Is CheckBox Or TypeOf ctrl Is Label Or TypeOf ctrl Is Button) Then
                    intTagNameHasBaseInIt = InStr(ctrl.tag, "BASE")
                    If intTagNameHasBaseInIt = 0 Then
                        If TypeOf ctrl Is TextBox Then
                            query = "Insert INTO Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intTaskInstance & "','" & ctrl.Tag & "','" & ctrl.Text & "','" & ctrl.AccessibleDescription & "','Config')"
                        ElseIf TypeOf ctrl Is ComboBox Then
                            Dim strPosition() As String
                            strPosition = Split(ctrl.AccessibleName, ",")
                            query = "Insert INTO Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intTaskInstance & "','" & ctrl.Tag & "','" & strPosition(ctrl.SelectedIndex) & "','" & ctrl.AccessibleDescription & "','Config')"
                        ElseIf TypeOf ctrl Is CheckBox Then
                            query = "Insert INTO Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intTaskInstance & "','" & ctrl.Tag & "','" & ctrl.checked & "','" & ctrl.AccessibleDescription & "','Config')"
                        ElseIf TypeOf ctrl Is Button Then
                            'Button1.AccessibleDescription
                            strTypeInfo = Split(ctrl.AccessibleName, ",")

                            If ctrl.AccessibleDescription = "" Then
                                strTypeAndLengthOfIndexArray = Split(ctrl.AccessibleName, ",")
                                strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "[", "")
                                strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "]", "")

                                strParentMemberAndDisplayText = Split(ctrl.Tag, "|")
                                strParentAndMember = Split(strParentMemberAndDisplayText(0), ".")

                                'intTaskItemNumber = 1
                                bTypeIsBool = False
                                If InStr(strTypeAndLengthOfIndexArray(1), "BOOL") Then
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "BOOL", "")
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "(", "")
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), ")", "")
                                    bTypeIsBool = True
                                End If

                                For j = 1 To strTypeAndLengthOfIndexArray(1)
                                    If strValues = "" Then
                                        If bTypeIsBool = False Then
                                            strValues = strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & 0
                                        Else
                                            strValues = strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & "False"
                                        End If

                                    Else
                                        If bTypeIsBool = False Then
                                            strValues = strValues & "|" & strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & 0
                                        Else
                                            strValues = strValues & "|" & strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & "False"
                                        End If
                                    End If
                                Next
                                ctrl.AccessibleDescription = strValues
                            End If

                            strValuesArray = Split(ctrl.AccessibleDescription, "|")

                            For i = LBound(strValuesArray) To UBound(strValuesArray)
                                strSingleValueArray = Split(strValuesArray(i), ",")
                                query = "Insert INTO Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intTaskInstance & "','" & strSingleValueArray(0) & "','" & strSingleValueArray(1) & "','" & strTypeInfo(0) & "','Config')"
                                cmd = New OleDbCommand(query, cnn)
                                cmd.ExecuteNonQuery()
                            Next
                            query = ""
                        End If
                    Else
                        intTagNameHasBaseInIt = InStr(ctrl.tag, "_")
                        If intTagNameHasBaseInIt = 0 Then
                            If TypeOf ctrl Is TextBox Or TypeOf ctrl Is Label Then
                                query = "Insert INTO Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intTaskInstance & "','" & ctrl.AccessibleName & "','" & ctrl.Text & "','" & ctrl.AccessibleDescription & "','Config')"
                            ElseIf TypeOf ctrl Is ComboBox Then
                                query = "Insert INTO Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intTaskInstance & "','" & ctrl.AccessibleName & "','" & ctrl.SelectedIndex + 1 & "','" & ctrl.AccessibleDescription & "','Config')"
                            ElseIf TypeOf ctrl Is CheckBox Then
                                query = "Insert INTO Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intTaskInstance & "','" & ctrl.AccessibleName & "','" & ctrl.checked & "','" & ctrl.AccessibleDescription & "','Config')"
                            ElseIf TypeOf ctrl Is Button Then
                                'Button1.AccessibleDescription
                                strTypeInfo = Split(ctrl.AccessibleName, ",")

                                If ctrl.AccessibleDescription = "" Then
                                    strTypeAndLengthOfIndexArray = Split(ctrl.AccessibleName, ",")
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "[", "")
                                    strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "]", "")

                                    strParentMemberAndDisplayText = Split(ctrl.Tag, "|")
                                    strParentAndMember = Split(strParentMemberAndDisplayText(0), ".")

                                    'intTaskItemNumber = 1
                                    bTypeIsBool = False
                                    If InStr(strTypeAndLengthOfIndexArray(1), "BOOL") Then
                                        strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "BOOL", "")
                                        strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), "(", "")
                                        strTypeAndLengthOfIndexArray(1) = Replace(strTypeAndLengthOfIndexArray(1), ")", "")
                                        bTypeIsBool = True
                                    End If

                                    For j = 1 To strTypeAndLengthOfIndexArray(1)
                                        If strValues = "" Then
                                            If bTypeIsBool = False Then
                                                strValues = strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & 0
                                            Else
                                                strValues = strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & "False"
                                            End If

                                        Else
                                            If bTypeIsBool = False Then
                                                strValues = strValues & "|" & strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & 0
                                            Else
                                                strValues = strValues & "|" & strParentMemberAndDisplayText(0) & "[" & j - 1 & "]" & "," & "False"
                                            End If
                                        End If
                                    Next
                                    ctrl.AccessibleDescription = strValues
                                End If

                                strValuesArray = Split(ctrl.AccessibleDescription, "|")
                                For i = LBound(strValuesArray) To UBound(strValuesArray)
                                    strSingleValueArray = Split(strValuesArray(i), ",")
                                    query = "Insert INTO Tasks_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Member_Type,Base_Tag) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intTaskInstance & "','" & strSingleValueArray(0) & "','" & strSingleValueArray(1) & "','" & strTypeInfo(0) & "','Config')"
                                    cmd = New OleDbCommand(query, cnn)
                                    cmd.ExecuteNonQuery()
                                Next
                                query = ""
                            End If
                        End If
                    End If
                    If query <> "" Then
                        cmd = New OleDbCommand(query, cnn)
                        cmd.ExecuteNonQuery()
                    End If
                End If
            Next

            cnn.Close()
            bISDirty = False
        Catch ex As Exception
            Log_Anything("SetTaskConfiguationDataToDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub GetTaskConfigurationFromDatabase(ByVal intTaskInstance As Integer, Optional ByVal TaskIsInstanceFromTree As Boolean = False)

        Try
            Dim Table_ As String = "Tasks_Configuration"
            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String
            Dim strTaskName As String
            Dim intTagNameHasBASEInIt As Integer
            Dim strTempNameAndDecription() As String

            If TaskIsInstanceFromTree = False Then
                strTaskName = tvwProject.SelectedNode.Name
                strStationName = tvwProject.SelectedNode.Parent.Parent.Name
                strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Name
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Name
            Else
                strTaskName = tvwProject.SelectedNode.Name
                strStationName = tvwProject.SelectedNode.Parent.Parent.Parent.Name
                strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Name
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Parent.Name
            End If


            If MaxNumberOfTasks = 1 Then
                intTaskInstance = -1
            End If
            Dim query As String = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "' AND Task_Instance = " & intTaskInstance & " ORDER BY MEMBER_NAME"
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)

            cnn.Open()
            Dim cmd As New OleDbCommand(query, cnn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)

            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow
            Dim strTagName As String
            Dim ctrlFirstTime As Integer = 0

            If t1.Rows.Count <> 0 Then
                For Each row In t1.Rows
                    For Each ctrl In GroupBox1.Controls
                        strTagName = ctrl.Tag
                        intTagNameHasBASEInIt = InStr(ctrl.tag, "BASE")
                        If intTagNameHasBASEInIt = 0 Then
                            If ctrl.Tag = row(6).ToString And Not TypeOf ctrl Is Button Then
                                If TypeOf ctrl Is TextBox Then
                                    ctrl.text = row(7).ToString
                                ElseIf TypeOf ctrl Is ComboBox Then
                                    If IsNumeric(row(7).ToString) Then
                                        'If row(7).ToString > 0 Then
                                        '    ctrl.SelectedIndex = row(7).ToString - 1
                                        'Else
                                        '    ctrl.SelectedIndex = 0
                                        'End If


                                        Dim strPosition() As String
                                        strPosition = Split(ctrl.accessibleName, ",")
                                        If UBound(strPosition) > 0 Then
                                            For j = 0 To UBound(strPosition) - 1
                                                If strPosition(j) = row(7).ToString Then
                                                    ctrl.SelectedIndex = j
                                                    Exit For
                                                End If
                                            Next
                                        Else
                                            ctrl.SelectedIndex = 0
                                        End If
                                    Else
                                        ctrl.SelectedIndex = 0
                                    End If
                                ElseIf TypeOf ctrl Is CheckBox Then
                                    ctrl.checked = row(7).ToString
                                End If
                                Exit For
                            ElseIf TypeOf ctrl Is Button Then
                                'MsgBox(ctrl.tag)
                                strTempNameAndDecription = Split(ctrl.tag, "|")
                                If InStr(row(6).ToString, strTempNameAndDecription(0)) Then
                                    'Button1.AccessibleDescription
                                    'MsgBox(ctrl.AccessibleDescription)
                                    If ctrl.AccessibleDescription = "" Or ctrl.AccessibleDefaultActionDescription <> 1 Then
                                        ctrl.AccessibleDescription = row(6).ToString & "," & row(7).ToString
                                        ctrl.AccessibleDefaultActionDescription = 1
                                    Else
                                        ctrl.AccessibleDescription = ctrl.AccessibleDescription & "|" & row(6).ToString & "," & row(7).ToString
                                    End If
                                    Exit For
                                End If
                            End If
                        Else
                            intTagNameHasBASEInIt = InStr(ctrl.tag, "_")
                            If intTagNameHasBASEInIt = 0 Then
                                If ctrl.AccessibleName <> "" Then
                                    If ctrl.AccessibleName = row(6).ToString Then
                                        If TypeOf ctrl Is TextBox Then
                                            ctrl.text = row(7).ToString
                                        ElseIf TypeOf ctrl Is ComboBox Then
                                            If IsNumeric(row(7).ToString) Then
                                                If row(7).ToString > 0 Then
                                                    ctrl.SelectedIndex = row(7).ToString - 1
                                                Else
                                                    ctrl.SelectedIndex = 0
                                                End If
                                            Else
                                                ctrl.SelectedIndex = 0
                                            End If
                                        ElseIf TypeOf ctrl Is CheckBox Then
                                            ctrl.checked = row(7).ToString
                                        End If
                                        Exit For
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            Else
                For Each ctrl In GroupBox1.Controls
                    strTagName = ctrl.Tag
                    intTagNameHasBASEInIt = InStr(ctrl.tag, "BASE")
                    If intTagNameHasBASEInIt = 0 Then
                        If Not TypeOf ctrl Is Button Then
                            If TypeOf ctrl Is TextBox Then
                                ctrl.text = 0
                            ElseIf TypeOf ctrl Is ComboBox Then
                                ctrl.text = ctrl.items(0).ToString
                            ElseIf TypeOf ctrl Is CheckBox Then
                                ctrl.checked = False
                            End If
                            'Exit For
                        ElseIf TypeOf ctrl Is Button Then
                            'MsgBox(ctrl.tag)
                            strTempNameAndDecription = Split(ctrl.tag, "|")
                            'If InStr(row(6).ToString, strTempNameAndDecription(0)) Then
                            '    'Button1.AccessibleDescription
                            '    'MsgBox(ctrl.AccessibleDescription)
                            '    If ctrl.AccessibleDescription = "" Then
                            '        ctrl.AccessibleDescription = row(6).ToString & "," & row(7).ToString
                            '    Else
                            '        ctrl.AccessibleDescription = ctrl.AccessibleDescription & "|" & row(6).ToString & "," & row(7).ToString
                            '    End If
                            '    Exit For
                            'End If
                        End If
                    Else
                        intTagNameHasBASEInIt = InStr(ctrl.tag, "_")
                        If intTagNameHasBASEInIt = 0 Then
                            If ctrl.AccessibleName <> "" Then

                                If TypeOf ctrl Is TextBox Then
                                    ctrl.text = 0
                                ElseIf TypeOf ctrl Is ComboBox Then
                                    ctrl.text = ctrl.items(0).ToString
                                ElseIf TypeOf ctrl Is CheckBox Then
                                    ctrl.checked = False
                                End If
                                'Exit For
                            End If
                        End If
                    End If
                Next
            End If

            Table_ = "Tasks"
            query = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "'"
            cmd = New OleDbCommand(query, cnn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)
            t1 = ds.Tables(Table_)


            NumericUpDownTaskInstance.Visible = False
            'NumericUpDownTaskInstance.Maximum = 1
            'NumericUpDownTaskInstance.Minimum = 1
            If intTaskInstance > -1 Then
                NumericUpDownTaskInstance.Value = intTaskInstance
            End If

            lbl_TaskInstance.Visible = False

            For Each row In t1.Rows
                If row(10).ToString > 1 Then
                    NumericUpDownTaskInstance.Visible = True
                    NumericUpDownTaskInstance.Minimum = 0
                    MaxNumberOfTasks = row(10).ToString
                    lbl_TaskInstance.Visible = True
                    lbl_TaskInstance.Text = ":Task Instance - Max =" & MaxNumberOfTasks
                    NumericUpDownTaskInstance.Maximum = MaxNumberOfTasks + 1
                    NumericUpDownTaskInstance.Value = intTaskInstance + 1
                    Exit For
                End If
            Next

            cnn.Close()
            bISDirty = False
        Catch ex As Exception
            Log_Anything("GetTaskConfigurationFromDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub GetTaskStructureFromDatabase(ByVal strTaskName As String, ByVal intTaskInstance As Integer, Optional ByVal TaskIsInstanceFromTree As Boolean = False)
        Dim query As String = ""
        Dim query2 As String = ""
        Try
            Dim Table_ As String = "Tasks"
            Dim strTaskConfigName As String
            Dim intTaskStringPosition
            Dim intPositionOfIndex As Integer
            Dim strTypeAndLengthOfIndex As String = ""

            Dim intPositionOfIndex2 As Integer
            Dim strTypeAndLengthOfIndex2 As String = ""

            Dim intStartPositionOfIndex As Integer
            Dim intEndPositionOfIndex As Integer
            Dim strIndexValue As String = ""
            Dim strInstance As String = ""


            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String

            Dim bSkipThisRecord As Boolean

            'Dim intTagNameHasBASEInIt As Integer
            'Dim strTempNameAndDecription() As String

            If TaskIsInstanceFromTree = False Then
                strStationName = tvwProject.SelectedNode.Parent.Parent.Name
                strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Name
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Name
            Else
                strStationName = tvwProject.SelectedNode.Parent.Parent.Parent.Name
                strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Name
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Parent.Name
            End If


            query = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "'"
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim cmd As New OleDbCommand(query, cnn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)

            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow

            For Each row In t1.Rows
                If IsNumeric(row(10).ToString) Then
                    MaxNumberOfTasks = row(10).ToString
                    If row(10).ToString > 1 Then
                        strInstance = "[" & intTaskInstance & "]"
                    Else
                        strInstance = ""
                    End If
                Else
                    strInstance = ""
                End If
                Exit For
            Next

            Dim strTagName As String
            intTaskStringPosition = InStr(strTaskName, "_Task")
            If intTaskStringPosition <> 0 Then
                strTaskConfigName = Mid(strTaskName, 1, intTaskStringPosition + 4)
            Else
                strTaskConfigName = strTaskName
            End If

            If intTaskStringPosition = 0 Then
                intTaskStringPosition = InStr(strTaskName, " ")
                If intTaskStringPosition <> 0 Then
                    strTaskConfigName = Mid(strTaskName, 1, intTaskStringPosition - 1)
                    strTaskConfigName = strTaskConfigName & "_Task"
                Else
                    strTaskConfigName = strTaskConfigName & "_Task"
                End If
            End If

            Table_ = "Task_Structures"
            query = "SELECT * FROM " & Table_ & " WHERE TaskXrefName = '" & strTaskConfigName & "' AND Visible = 1 ORDER BY MEMBERORDER"

            Dim strTempType As String
            Dim strTempType2 As String

            cnn.Open()
            cmd = New OleDbCommand(query, cnn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)
            t1 = ds.Tables(Table_)

            'Remove any extra controls before adding new controls
            DeleteConfigurationItemsFromGroupBox()

            lbl_TaskType_lbl.Visible = True
            lbl_TaskType_lbl.Text = "Task Type:"

            lbl_TaskType.AccessibleName = "TaskName_Actual"
            lbl_TaskType.Visible = True
            lbl_TaskType.Text = strTaskName

            txt_TaskName_lbl.Visible = False
            txt_TaskName.AccessibleName = "TaskName_User"
            txt_TaskName.Visible = False
            txt_TaskName.Text = strTaskName

            chk_TaskBypassed.Checked = False
            chk_TaskBypassed.Visible = False
            taskItemNumber = 1

            txt_TaskNumber.Visible = False
            txt_TaskNumber.Text = 0
            txt_TaskNumber_lbl.Visible = False
            txt_TaskNumber_lbl.Text = "Task Number:"


            For Each row In t1.Rows
                If IsNumeric(row(12).ToString) Then
                    If row(12).ToString > 0 Then
                        For Each ctrl In GroupBox1.Controls
                            strTagName = ctrl.tag
                            If ctrl.Tag = "BASE" & row(12).ToString Then
                                ctrl.Visible = True
                                ctrl.AccessibleName = row(1).ToString & strInstance & "." & row(2).ToString
                                ctrl.AccessibleDescription = row(3).ToString
                                If TypeOf ctrl Is CheckBox Then
                                    If row(intGlobalDescriptionToUse).ToString <> "" Then
                                        ctrl.Text = row(intGlobalDescriptionToUse).ToString
                                    Else
                                        ctrl.text = row(2).ToString
                                    End If
                                End If
                                'Exit For
                            ElseIf ctrl.tag = "BASE" & row(12).ToString & "_0" Then
                                ctrl.Visible = True
                                If row(intGlobalDescriptionToUse).ToString <> "" Then
                                    ctrl.Text = row(intGlobalDescriptionToUse).ToString
                                Else
                                    ctrl.text = row(2).ToString
                                End If
                                'Exit For
                            End If
                        Next
                    End If
                End If
            Next


            'Get Configuration Setup information
            For Each row In t1.Rows
                bSkipThisRecord = False
                If row(2).ToString = "SPACE" Then
                    taskItemNumber += 1
                    bSkipThisRecord = True
                End If

                If bSkipThisRecord = False Then
                    intPositionOfIndex = InStr(row(3).ToString, "[")
                    If intPositionOfIndex <> 0 Then
                        strTempType = Mid(row(3).ToString, 1, intPositionOfIndex - 1)
                        strTypeAndLengthOfIndex = strTempType & "," & Mid(row(3).ToString, intPositionOfIndex, Len(row(3).ToString) - (intPositionOfIndex - 1))
                    Else
                        strTempType = row(3).ToString
                        strTypeAndLengthOfIndex = ""
                    End If

                    intStartPositionOfIndex = InStr(row(2).ToString, "[")
                    intEndPositionOfIndex = InStr(row(2).ToString, "]")
                    If intStartPositionOfIndex <> 0 And intStartPositionOfIndex <> 0 Then
                        strIndexValue = Mid(row(2).ToString, intStartPositionOfIndex, intEndPositionOfIndex - intStartPositionOfIndex + 1)
                    Else
                        strIndexValue = ""
                    End If


                    'Check to see if the DataType is a UDT
                    If strTempType <> "BOOL" And strTempType <> "SINT" And strTempType <> "INT" And strTempType <> "DINT" And strTempType <> "STRING" And strTempType <> "REAL" Then
                        Dim Table2_ As String = "Member_Structure"
                        query2 = "SELECT * FROM " & Table2_ & " WHERE Member = '" & strTempType & "' AND Visible = 1 ORDER BY MEMBERORDER"
                        Dim cmd2 As New OleDbCommand(query2, cnn)
                        Dim ds2 As New DataSet
                        Dim da2 As New OleDbDataAdapter(cmd2)
                        da2.Fill(ds2, Table2_)
                        Dim t2 As DataTable = ds2.Tables(Table2_)
                        Dim row2 As DataRow

                        'This inside loop is to get any NON BASE Members of the TASK (Anything that isnt BOOL,SINT,INT,DINT,STRING,REAL)
                        For Each row2 In t2.Rows
                            intPositionOfIndex2 = InStr(row2(3).ToString, "[")
                            If intPositionOfIndex2 <> 0 Then
                                strTempType2 = Mid(row2(3).ToString, 1, intPositionOfIndex2 - 1)
                                strTypeAndLengthOfIndex2 = strTempType2 & "," & Mid(row2(3).ToString, intPositionOfIndex2, Len(row2(3).ToString) - (intPositionOfIndex2 - 1))
                            Else
                                strTempType2 = row2(3).ToString
                                strTypeAndLengthOfIndex2 = ""
                            End If

                            'If row2(2).ToString = "TaskNumber" Then
                            '    txt_TaskNumber_lbl.Visible = True
                            '    txt_TaskNumber.Visible = True
                            'ElseIf row2(2).ToString = "Task_Number" Then
                            '    txt_TaskNumber_lbl.Visible = True
                            '    txt_TaskNumber.Visible = True
                            'ElseIf row2(2).ToString = "Number" Then
                            '    txt_TaskNumber_lbl.Visible = True
                            '    txt_TaskNumber.Visible = True
                            'ElseIf row2(2).ToString = "Bypassed" Then
                            '    chk_TaskBypassed.Visible = True
                            'ElseIf row2(2).ToString = "Bypass" Then
                            '    chk_TaskBypassed.Visible = True
                            'Else

                            If strTempType2 = "BOOL" Then
                                AddConfigurationItemToGroupBox(strTempType2, row(1).ToString & strInstance & "." & row(2).ToString, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString & strIndexValue, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex2, row2(7).ToString)
                            ElseIf strTempType2 = "SINT" Then
                                AddConfigurationItemToGroupBox(strTempType2, row(1).ToString & strInstance & "." & row(2).ToString, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString & strIndexValue, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex2, "")
                            ElseIf strTempType2 = "INT" Then
                                AddConfigurationItemToGroupBox(strTempType2, row(1).ToString & strInstance & "." & row(2).ToString, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString & strIndexValue, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex2, "")
                            ElseIf strTempType2 = "DINT" Then
                                AddConfigurationItemToGroupBox(strTempType2, row(1).ToString & strInstance & "." & row(2).ToString, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString & strIndexValue, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex2, "")
                            ElseIf strTempType2 = "REAL" Then
                                AddConfigurationItemToGroupBox(strTempType2, row(1).ToString & strInstance & "." & row(2).ToString, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString & strIndexValue, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex2, "")
                            End If
                        Next

                    Else
                        If row(2).ToString = "TaskNumber" Then
                            txt_TaskNumber_lbl.Visible = True
                            txt_TaskNumber.Visible = True
                        ElseIf row(2).ToString = "Task_Number" Then
                            txt_TaskNumber_lbl.Visible = True
                            txt_TaskNumber.Visible = True
                        ElseIf row(2).ToString = "Number" Then
                            txt_TaskNumber_lbl.Visible = True
                            txt_TaskNumber.Visible = True
                        ElseIf row(2).ToString = "Bypassed" Then
                            chk_TaskBypassed.Visible = True
                        ElseIf row(2).ToString = "Bypass" Then
                            chk_TaskBypassed.Visible = True
                        ElseIf strTempType = "BOOL" Then
                            AddConfigurationItemToGroupBox(strTempType, row(1).ToString & strInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                        ElseIf strTempType = "SINT" Then
                            AddConfigurationItemToGroupBox(strTempType, row(1).ToString & strInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                        ElseIf strTempType = "INT" Then
                            AddConfigurationItemToGroupBox(strTempType, row(1).ToString & strInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                        ElseIf strTempType = "DINT" Then
                            AddConfigurationItemToGroupBox(strTempType, row(1).ToString & strInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                        ElseIf strTempType = "REAL" Then
                            AddConfigurationItemToGroupBox(strTempType, row(1).ToString & strInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                        End If
                    End If
                End If
            Next

            cnn.Close()

        Catch ex As Exception
            Log_Anything("GetTaskStructureFromDatabase - " & ex.Message & " Query= " & query & " Query2= " & query2)
        End Try

    End Sub

    Private Sub SetPLCConfigurationToDatabase()

        Try
            Dim Table_ As String = "PLCs_Configuration"
            Dim query As String = "SELECT * FROM " & Table_
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim cmd As New OleDbCommand
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String
            Dim strPLCName As String
            Dim intTagNameHasBaseInIt As Integer


            strPLCName = tvwProject.SelectedNode.Text
            strStationName = tvwProject.SelectedNode.Parent.Text
            strSectionName = tvwProject.SelectedNode.Parent.Parent.Text
            strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Text

            cnn.Open()

            query = "DELETE * FROM PLCs_Configuration WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND PLC_Name = '" & strPLCName & "'"
            cmd = New OleDbCommand(query, cnn)
            cmd.ExecuteNonQuery()

            For Each ctrl In GroupBox1.Controls
                query = ""
                If ctrl.Visible = True And (TypeOf ctrl Is TextBox Or TypeOf ctrl Is ComboBox Or TypeOf ctrl Is CheckBox Or TypeOf ctrl Is Label) Then
                    intTagNameHasBaseInIt = InStr(ctrl.tag, "BASE")
                    If intTagNameHasBaseInIt = 0 Then
                        If TypeOf ctrl Is TextBox Then
                            query = "Insert INTO PLCs_Configuration (Area_Name,Section_Name,Station_Name,PLC_Name,Member_Name,Member_Value) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strPLCName & "','" & ctrl.Tag & "','" & ctrl.Text & "')"
                        ElseIf TypeOf ctrl Is ComboBox Then
                            query = "Insert INTO PLCs_Configuration (Area_Name,Section_Name,Station_Name,PLC_Name,Member_Name,Member_Value) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strPLCName & "','" & ctrl.Tag & "','" & ctrl.Text & "')"
                        ElseIf TypeOf ctrl Is CheckBox Then
                            query = "Insert INTO PLCs_Configuration (Area_Name,Section_Name,Station_Name,PLC_Name,Member_Name,Member_Value) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strPLCName & "','" & ctrl.Tag & "','" & ctrl.checked & "')"
                        End If
                    Else
                        intTagNameHasBaseInIt = InStr(ctrl.tag, "_")
                        If intTagNameHasBaseInIt = 0 Then
                            If TypeOf ctrl Is TextBox Or TypeOf ctrl Is Label Then
                                query = "Insert INTO PLCs_Configuration (Area_Name,Section_Name,Station_Name,PLC_Name,Member_Name,Member_Value) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strPLCName & "','" & ctrl.AccessibleName & "','" & ctrl.Text & "')"
                            ElseIf TypeOf ctrl Is ComboBox Then
                                query = "Insert INTO PLCs_Configuration (Area_Name,Section_Name,Station_Name,PLC_Name,Member_Name,Member_Value) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strPLCName & "','" & ctrl.AccessibleName & "','" & ctrl.Text & "')"
                            ElseIf TypeOf ctrl Is CheckBox Then
                                query = "Insert INTO PLCs_Configuration (Area_Name,Section_Name,Station_Name,PLC_Name,Member_Name,Member_Value) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strPLCName & "','" & ctrl.AccessibleName & "','" & ctrl.checked & "')"
                            End If
                        End If
                    End If
                    If query <> "" Then
                        cmd = New OleDbCommand(query, cnn)
                        cmd.ExecuteNonQuery()
                    End If
                End If
            Next

            cnn.Close()
            bISDirty = False
        Catch ex As Exception
            Log_Anything("SetPLCConfigurationToDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub GetPLCStructureFromDatabase(ByVal strPLCName As String)
        Try
            Dim Table_ As String = "PLC_Structures"
            Dim intPositionOfIndex As Integer
            Dim strTypeAndLengthOfIndex As String = ""


            Dim query As String = "SELECT * FROM " & Table_ & " WHERE Visible = 1 ORDER BY MEMBERORDER"
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim strTempType As String

            cnn.Open()
            Dim cmd As New OleDbCommand(query, cnn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)


            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow
            Dim strTagName As String

            'Remove any extra controls before adding new controls
            DeleteConfigurationItemsFromGroupBox()

            lbl_TaskType_lbl.Visible = True
            lbl_TaskType_lbl.Text = "PLC Type:"
            lbl_TaskType.Visible = True
            lbl_TaskType.Text = strPLCName

            'txt_TaskName_lbl.Visible = True
            'txt_TaskName.Visible = True
            'txt_TaskName.Text = strPLCName

            chk_TaskBypassed.Checked = False
            chk_TaskBypassed.Visible = False
            taskItemNumber = 1

            txt_TaskNumber.Visible = False
            txt_TaskNumber.Text = 0
            txt_TaskNumber_lbl.Visible = False
            txt_TaskNumber_lbl.Text = "Task Number:"


            For Each row In t1.Rows
                If IsNumeric(row(12).ToString) Then
                    If row(12).ToString > 0 Then
                        For Each ctrl In GroupBox1.Controls
                            strTagName = ctrl.tag
                            If ctrl.Tag = "BASE" & row(12).ToString Then
                                ctrl.Visible = True
                                ctrl.AccessibleName = row(1).ToString & "." & row(2).ToString
                                If TypeOf ctrl Is CheckBox Then
                                    If row(intGlobalDescriptionToUse).ToString <> "" Then
                                        ctrl.Text = row(intGlobalDescriptionToUse).ToString
                                    Else
                                        ctrl.text = row(2).ToString
                                    End If
                                End If
                                'Exit For
                            ElseIf ctrl.tag = "BASE" & row(12).ToString & "_0" Then
                                ctrl.Visible = True
                                If row(intGlobalDescriptionToUse).ToString <> "" Then
                                    ctrl.Text = row(intGlobalDescriptionToUse).ToString
                                Else
                                    ctrl.text = row(2).ToString
                                End If
                                'Exit For
                            End If
                        Next
                    End If
                End If
            Next


            'Get Configuration Setup information
            For Each row In t1.Rows
                intPositionOfIndex = InStr(row(3).ToString, "[")
                If intPositionOfIndex <> 0 Then
                    strTempType = Mid(row(3).ToString, 1, intPositionOfIndex - 1)
                    strTypeAndLengthOfIndex = strTempType & "," & Mid(row(3).ToString, intPositionOfIndex, Len(row(3).ToString) - (intPositionOfIndex - 1))
                Else
                    strTempType = row(3).ToString
                    strTypeAndLengthOfIndex = ""
                End If

                If row(2).ToString = "TaskNumber" Then
                    txt_TaskNumber_lbl.Visible = True
                    txt_TaskNumber.Visible = True
                ElseIf row(2).ToString = "Task_Number" Then
                    txt_TaskNumber_lbl.Visible = True
                    txt_TaskNumber.Visible = True
                ElseIf row(2).ToString = "Number" Then
                    txt_TaskNumber_lbl.Visible = True
                    txt_TaskNumber.Visible = True
                ElseIf row(2).ToString = "Bypassed" Then
                    chk_TaskBypassed.Visible = True
                ElseIf row(2).ToString = "Bypass" Then
                    chk_TaskBypassed.Visible = True
                ElseIf strTempType = "BOOL" Then
                    AddConfigurationItemToGroupBox(strTempType, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                ElseIf strTempType = "SINT" Then
                    AddConfigurationItemToGroupBox(strTempType, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                ElseIf strTempType = "INT" Then
                    AddConfigurationItemToGroupBox(strTempType, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                ElseIf strTempType = "DINT" Then
                    AddConfigurationItemToGroupBox(strTempType, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                ElseIf strTempType = "REAL" Then
                    AddConfigurationItemToGroupBox(strTempType, row(1).ToString, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex, "")
                End If
            Next

            cnn.Close()
        Catch ex As Exception
            Log_Anything("GetPLCStructureFromDatabase - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub GetPLCConfigurationFromDatabase()

        Try
            Dim Table_ As String = "PLCs_Configuration"
            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String
            Dim strPLCName As String
            Dim intTagNameHasBASEInIt As Integer

            strPLCName = tvwProject.SelectedNode.Text
            strStationName = tvwProject.SelectedNode.Parent.Text
            strSectionName = tvwProject.SelectedNode.Parent.Parent.Text
            strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Text


            Dim query As String = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND PLC_Name = '" & strPLCName & "'"
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)

            cnn.Open()
            Dim cmd As New OleDbCommand(query, cnn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)

            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow
            Dim strTagName As String

            For Each row In t1.Rows
                For Each ctrl In GroupBox1.Controls
                    strTagName = ctrl.Tag
                    intTagNameHasBASEInIt = InStr(ctrl.tag, "BASE")
                    If intTagNameHasBASEInIt = 0 Then
                        If ctrl.Tag = row(5).ToString Then
                            If TypeOf ctrl Is TextBox Then
                                ctrl.text = row(6).ToString
                            ElseIf TypeOf ctrl Is ComboBox Then
                                ctrl.text = row(6).ToString
                            ElseIf TypeOf ctrl Is CheckBox Then
                                ctrl.checked = row(6).ToString
                                'If ctrl.Checked = True Then
                                '    tvwProject.SelectedNode.BackColor = Color.Red
                                'Else
                                '    tvwProject.SelectedNode.BackColor = Color.White
                                'End If
                            End If
                        End If
                    Else
                        intTagNameHasBASEInIt = InStr(ctrl.tag, "_")
                        If intTagNameHasBASEInIt = 0 Then
                            If ctrl.AccessibleName <> "" Then
                                If ctrl.AccessibleName = row(5).ToString Then
                                    If TypeOf ctrl Is TextBox Then
                                        ctrl.text = row(6).ToString
                                    ElseIf TypeOf ctrl Is ComboBox Then
                                        ctrl.text = row(6).ToString
                                    ElseIf TypeOf ctrl Is CheckBox Then
                                        ctrl.checked = row(6).ToString
                                        'If ctrl.Checked = True Then
                                        '    tvwProject.SelectedNode.BackColor = Color.Red
                                        'Else
                                        '    tvwProject.SelectedNode.BackColor = Color.White
                                        'End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            Next

            cnn.Close()
            bISDirty = False
        Catch ex As Exception
            Log_Anything("GetPLCConfigurationFromDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub DeleteConfigurationItemsFromGroupBox()
        Dim i As Integer
        Try
            Dim strTagName As String
            Dim intTagHasBASEinName As Integer

            lbl_TaskType.Visible = False
            lbl_TaskType_lbl.Visible = False
            txt_TaskName.Visible = False
            txt_TaskName_lbl.Visible = False
            txt_TaskNumber.Visible = False
            txt_TaskNumber_lbl.Visible = False
            chk_TaskBypassed.Visible = False

            NumericUpDownTaskInstance.Visible = False
            'NumericUpDownTaskInstance.Maximum = 1
            'NumericUpDownTaskInstance.Minimum = 1
            'NumericUpDownTaskInstance.Value = 1
            lbl_TaskInstance.Visible = False

            txt_TaskName.Text = ""
            'txt_TaskNumber.Text = ""
            chk_TaskBypassed.Checked = False


            For i = (GroupBox1.Controls.Count - 1) To 0 Step -1
                Dim ctrl As Control = GroupBox1.Controls(i)
                strTagName = ctrl.Tag
                intTagHasBASEinName = InStr(strTagName, "BASE")
                If intTagHasBASEinName = 0 Then
                    'MsgBox(ctrl.Name)
                    GroupBox1.Controls.Remove(ctrl)
                    ctrl.Dispose()
                End If
            Next i

            dgvSequenceGraphic.Visible = False

        Catch ex As Exception
            Log_Anything("DeleteConfigurationItemsFromGroupBox - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub DeleteConfigurationItemsFromModelGroupBox()
        Try
            Dim strTagName As String
            Dim intTagHasBASEinName As Integer

            lbl_Models_lbl.Visible = False
            cbo_Models.Visible = False
            chk_ModelEnabled.Visible = False

            cbo_Models.Items.Clear()
            cbo_Models.Text = ""
            chk_ModelEnabled.Checked = False

            For i As Integer = (GroupBox2.Controls.Count - 1) To 0 Step -1
                Dim ctrl As Control = GroupBox2.Controls(i)
                strTagName = ctrl.Tag
                intTagHasBASEinName = InStr(strTagName, "BASE")
                If intTagHasBASEinName = 0 Then
                    'MsgBox(ctrl.Name)
                    GroupBox2.Controls.Remove(ctrl)
                    ctrl.Dispose()
                End If
            Next i

            SplitContainer2.Panel2Collapsed = False

        Catch ex As Exception
            Log_Anything("DeleteConfigurationItemsFromModelGroupBox - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Function CopyModelConfigurationToDatabase(ByVal strExistingModelID As String, ByVal intExistingModelNumber As Integer, ByVal intNewModelNumber As Integer, ByVal strNewModelID As String, strAreaName As String) As Boolean

        Try
            Dim Table_ As String = "Model_Configuration"
            Dim query As String = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Member_Name Like 'Model[[]" & intExistingModelNumber - 1 & "%'"
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim cmd As New OleDbCommand
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)

            Dim strSectionName As String = ""
            Dim strStationName As String = ""
            Dim strTaskName As String = ""
            Dim strNewModelTag As String

            cnn.Open()

            cmd = New OleDbCommand(query, cnn)
            cmd.ExecuteNonQuery()

            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)

            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow

            If t1.Rows.Count > 0 Then
                For Each row In t1.Rows
                    strNewModelTag = Replace(row(6).ToString, "Model[" & intExistingModelNumber - 1 & "].", "Model[" & intNewModelNumber - 1 & "].")
                    query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Task_Instance,Member_Name,Member_Value,Model_Instance,Member_Type) Values ('" & row(1).ToString & "','" & row(2).ToString & "','" & row(3).ToString & "','" & row(4).ToString & "','" & row(5).ToString & "','" & strNewModelTag & "','" & row(7).ToString & "'," & intNewModelNumber - 1 & ",'" & row(9).ToString & "')"
                    cmd = New OleDbCommand(query, cnn)
                    cmd.ExecuteNonQuery()
                Next
            End If

            query = "Insert INTO Areas_Configuration (Area_Name,Model_Number,Model_Name) Values ('" & strAreaName & "'," & intNewModelNumber & ",'" & strNewModelID & "')"
            cmd = New OleDbCommand(query, cnn)
            cmd.ExecuteNonQuery()


            cnn.Close()
            bISDirty = False
            CopyModelConfigurationToDatabase = True
        Catch ex As Exception
            Log_Anything("CopyModelConfigurationToDatabase - " & GetExceptionInfo(ex))
            CopyModelConfigurationToDatabase = False
        End Try
    End Function

    Private Function DeleteModelConfigurationFromDatabase(ByVal strExistingModelID As String, ByVal intExistingModelNumber As Integer, strAreaName As String) As Boolean

        Try
            Dim Table_ As String = "Model_Configuration"
            Dim Table1_ As String = "Areas_Configuration"
            Dim query As String = "DELETE * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Member_Name Like 'Model[[]" & intExistingModelNumber - 1 & "%'"
            Dim query1 As String = ""
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim cmd As New OleDbCommand
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)

            Dim strSectionName As String = ""
            Dim strStationName As String = ""
            Dim strTaskName As String = ""
            Dim i As Integer
            Dim strNewMemberName As String = ""

            cnn.Open()

            cmd = New OleDbCommand(query, cnn)
            cmd.ExecuteNonQuery()

            query = "DELETE * FROM " & Table1_ & " WHERE Area_Name = '" & strAreaName & "' AND Model_Number = " & intExistingModelNumber
            cmd = New OleDbCommand(query, cnn)
            cmd.ExecuteNonQuery()


            For i = intExistingModelNumber To txt_TaskNumber.Text
                query1 = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Model_Instance = " & i
                cmd = New OleDbCommand(query1, cnn)
                cmd.ExecuteNonQuery()

                Dim da As New OleDbDataAdapter(cmd)
                da.Fill(ds, Table_)

                Dim t1 As DataTable = ds.Tables(Table_)
                Dim row As DataRow

                If t1.Rows.Count > 0 Then
                    For Each row In t1.Rows
                        strNewMemberName = Replace(row(6).ToString, "Model[" & i & "].", "Model[" & i - 1 & "].")
                        query = "UPDATE Model_Configuration SET Member_Name = '" & strNewMemberName & "', Model_Instance =" & i - 1 & " WHERE Area_Name = '" & strAreaName & "' AND Model_Instance = " & i & " AND Member_Name = '" & row(6).ToString & "'"
                        cmd = New OleDbCommand(query, cnn)
                        cmd.ExecuteNonQuery()
                    Next
                End If
            Next

            For i = intExistingModelNumber + 1 To txt_TaskNumber.Text
                query = "UPDATE " & Table1_ & " SET Model_Number = " & i - 1 & " WHERE Area_Name = '" & strAreaName & "' AND Model_Number = " & i
                cmd = New OleDbCommand(query, cnn)
                cmd.ExecuteNonQuery()
            Next

            cnn.Close()
            bISDirty = False
            DeleteModelConfigurationFromDatabase = True
        Catch ex As Exception
            Log_Anything("DeleteModelConfigurationFromDatabase - " & GetExceptionInfo(ex))
            DeleteModelConfigurationFromDatabase = False
        End Try
    End Function

    Private Sub SetModelConfigurationToDatabase(ByVal intModelInstance As Integer, ByVal intTaskInstance As Integer)

        Try
            Dim Table_ As String = "Model_Configuration"
            Dim query As String = "SELECT * FROM " & Table_
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim cmd As New OleDbCommand
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim strAreaName As String = ""
            Dim strSectionName As String = ""
            Dim strStationName As String = ""
            Dim strTaskName As String = ""
            Dim intTagNameHasBaseInIt As Integer
            Dim strValuesArray() As String
            Dim strSingleValueArray() As String
            Dim i As Integer
            Dim strTypeInfo() As String

            If GroupBox1.Tag = "TASK" Then
                If tvwProject.SelectedNode.Parent.Tag <> "TASK" Then
                    strTaskName = tvwProject.SelectedNode.Text
                    strStationName = tvwProject.SelectedNode.Parent.Parent.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                Else
                    strTaskName = tvwProject.SelectedNode.Name
                    strStationName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Parent.Text
                End If

            ElseIf GroupBox1.Tag = "STATION" Then
                strTaskName = tvwProject.SelectedNode.Text
                strStationName = tvwProject.SelectedNode.Text
                strSectionName = tvwProject.SelectedNode.Parent.Text
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Text
            End If

            cnn.Open()

            query = "DELETE * FROM Model_Configuration WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "' AND Model_Instance = " & intModelInstance & " AND Task_Instance = " & intTaskInstance

            cmd = New OleDbCommand(query, cnn)
            cmd.ExecuteNonQuery()

            For Each ctrl In GroupBox2.Controls

                If ctrl.Name = "cbo_Models" Then

                Else
                    query = ""
                    If ctrl.Visible = True And (TypeOf ctrl Is TextBox Or TypeOf ctrl Is ComboBox Or TypeOf ctrl Is CheckBox Or TypeOf ctrl Is Label Or TypeOf ctrl Is Button) Then
                        intTagNameHasBaseInIt = InStr(ctrl.tag, "BASE")
                        If intTagNameHasBaseInIt = 0 Then
                            If TypeOf ctrl Is TextBox Then
                                query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Model_Instance,Member_Name,Member_Value,Task_Instance,Member_Type) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intModelInstance & "','Model[" & intModelInstance & "]." & ctrl.Tag & "','" & ctrl.Text & "'," & intTaskInstance & ",'" & ctrl.AccessibleDescription & "')"
                            ElseIf TypeOf ctrl Is ComboBox Then
                                query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Model_Instance,Member_Name,Member_Value,Task_Instance,Member_Type) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intModelInstance & "','Model[" & intModelInstance & "]." & ctrl.Tag & "','" & ctrl.Text & "'," & intTaskInstance & ",'" & ctrl.AccessibleDescription & "')"
                            ElseIf TypeOf ctrl Is CheckBox Then
                                query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Model_Instance,Member_Name,Member_Value,Task_Instance,Member_Type) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intModelInstance & "','Model[" & intModelInstance & "]." & ctrl.Tag & "','" & ctrl.checked & "'," & intTaskInstance & ",'" & ctrl.AccessibleDescription & "')"
                            ElseIf TypeOf ctrl Is Button Then
                                'Button1.AccessibleDescription
                                strTypeInfo = Split(ctrl.AccessibleName, ",")

                                strValuesArray = Split(ctrl.AccessibleDescription, "|")
                                For i = LBound(strValuesArray) To UBound(strValuesArray)
                                    strSingleValueArray = Split(strValuesArray(i), ",")
                                    query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Model_Instance,Member_Name,Member_Value,Task_Instance,Member_Type) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intModelInstance & "','Model[" & intModelInstance & "]." & strSingleValueArray(0) & "','" & strSingleValueArray(1) & "'," & intTaskInstance & ",'" & strTypeInfo(0) & "')"
                                    cmd = New OleDbCommand(query, cnn)
                                    cmd.ExecuteNonQuery()
                                Next
                                query = ""
                            End If
                        Else
                            intTagNameHasBaseInIt = InStr(ctrl.tag, "_")
                            If intTagNameHasBaseInIt = 0 Then
                                If TypeOf ctrl Is TextBox Or TypeOf ctrl Is Label Then
                                    query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Model_Instance,Member_Name,Member_Value,Task_Instance,Member_Type) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intModelInstance & "','Model[" & intModelInstance & "]." & ctrl.AccessibleName & "','" & ctrl.Text & "'," & intTaskInstance & ",'" & ctrl.AccessibleDescription & "')"
                                ElseIf TypeOf ctrl Is ComboBox Then
                                    query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Model_Instance,Member_Name,Member_Value,Task_Instance,Member_Type) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intModelInstance & "','Model[" & intModelInstance & "]." & ctrl.AccessibleName & "','" & ctrl.Text & "'," & intTaskInstance & ",'" & ctrl.AccessibleDescription & "')"
                                ElseIf TypeOf ctrl Is CheckBox Then
                                    query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Model_Instance,Member_Name,Member_Value,Task_Instance,Member_Type) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intModelInstance & "','Model[" & intModelInstance & "]." & ctrl.AccessibleName & "','" & ctrl.checked & "'," & intTaskInstance & ",'" & ctrl.AccessibleDescription & "')"
                                ElseIf TypeOf ctrl Is Button Then
                                    'Button1.AccessibleDescription
                                    strTypeInfo = Split(ctrl.AccessibleName, ",")

                                    strValuesArray = Split(ctrl.AccessibleDescription, "|")
                                    For i = LBound(strValuesArray) To UBound(strValuesArray)
                                        strSingleValueArray = Split(strValuesArray(i), ",")
                                        query = "Insert INTO Model_Configuration (Area_Name,Section_Name,Station_Name,Task_Name,Model_Instance,Member_Name,Member_Value,Task_Instance,Member_Type) Values ('" & strAreaName & "','" & strSectionName & "','" & strStationName & "','" & strTaskName & "','" & intModelInstance & "','Model[" & intModelInstance & "]." & strSingleValueArray(0) & "','" & strSingleValueArray(1) & "'," & intTaskInstance & ",'" & strTypeInfo(0) & "')"
                                        cmd = New OleDbCommand(query, cnn)
                                        cmd.ExecuteNonQuery()
                                    Next
                                    query = ""
                                End If
                            End If
                        End If
                        If query <> "" Then
                            cmd = New OleDbCommand(query, cnn)
                            cmd.ExecuteNonQuery()
                        End If
                    End If
                End If
            Next

            cnn.Close()
            bISDirty = False
        Catch ex As Exception
            Log_Anything("SetModelConfigurationToDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub


    Private Sub GetModelConfigurationFromDatabase(ByVal intModelInstance As Integer)
        Dim SubPosition As Integer = 0
        Try
            Dim Table_ As String = "Model_Configuration"
            Dim strAreaName As String = ""
            Dim strSectionName As String = ""
            Dim strStationName As String = ""
            Dim strTaskName As String = ""
            Dim intTagNameHasBASEInIt As Integer
            Dim strTempNameAndDecription() As String
            Dim intTaskInstance As Integer
            Dim strTempCompareName As String = ""
            Dim strTempNameAndDescriptionSingle As String = ""


            If GroupBox1.Tag = "TASK" Then
                If tvwProject.SelectedNode.Parent.Tag <> "TASK" Then
                    strTaskName = tvwProject.SelectedNode.Text
                    strStationName = tvwProject.SelectedNode.Parent.Parent.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                Else
                    strTaskName = tvwProject.SelectedNode.Name
                    strStationName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Parent.Text
                End If
            ElseIf GroupBox1.Tag = "STATION" Then
                strTaskName = tvwProject.SelectedNode.Text
                strStationName = tvwProject.SelectedNode.Text
                strSectionName = tvwProject.SelectedNode.Parent.Text
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Text
            End If

            If NumericUpDownTaskInstance.Visible = True Then
                intTaskInstance = NumericUpDownTaskInstance.Value - 1
            Else
                intTaskInstance = -1
            End If

            Dim query As String
            If intTaskInstance = -1 Then
                query = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "' AND Model_Instance = " & intModelInstance & " ORDER BY MEMBER_NAME"
            Else
                query = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "' AND Model_Instance = " & intModelInstance & " AND Task_Instance = " & intTaskInstance & " ORDER BY MEMBER_NAME"
            End If

            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)

            cnn.Open()
            Dim cmd As New OleDbCommand(query, cnn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)

            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow
            Dim strTagName As String

            If t1.Rows.Count > 0 Then
                SubPosition = 1
                For Each row In t1.Rows
                    SubPosition = 2
                    For Each ctrl In GroupBox2.Controls
                        SubPosition = 3
                        strTagName = ctrl.Tag
                        intTagNameHasBASEInIt = InStr(ctrl.tag, "BASE")
                        If intTagNameHasBASEInIt = 0 Then
                            SubPosition = 4
                            strTempCompareName = "Model[" & intModelInstance & "]." & ctrl.Tag
                            If Not TypeOf ctrl Is Button Then
                                SubPosition = 5
                                If strTempCompareName = row(6).ToString Then
                                    SubPosition = 6
                                    strTempNameAndDescriptionSingle = ctrl.tag
                                    If TypeOf ctrl Is TextBox Then
                                        ctrl.text = row(7).ToString
                                    ElseIf TypeOf ctrl Is ComboBox Then
                                        ctrl.text = row(7).ToString
                                    ElseIf TypeOf ctrl Is CheckBox Then
                                        ctrl.checked = row(7).ToString
                                    End If
                                    Exit For
                                End If
                            ElseIf TypeOf ctrl Is Button Then
                                SubPosition = 7
                                'MsgBox(ctrl.tag)
                                strTempNameAndDecription = Split(ctrl.tag, "|")
                                If InStr(row(6).ToString, strTempNameAndDecription(0)) Then
                                    'Button1.AccessibleDescription
                                    'MsgBox(ctrl.AccessibleDescription)
                                    If ctrl.AccessibleDescription = "" Then
                                        ctrl.AccessibleDescription = Replace(row(6).ToString, "Model[" & intModelInstance & "].", "") & "," & row(7).ToString
                                    Else
                                        ctrl.AccessibleDescription = ctrl.AccessibleDescription & "|" & Replace(row(6).ToString, "Model[" & intModelInstance & "].", "") & "," & row(7).ToString
                                    End If
                                    Exit For
                                End If
                            End If
                        Else
                            SubPosition = 8
                            intTagNameHasBASEInIt = InStr(ctrl.tag, "_")
                            If intTagNameHasBASEInIt = 0 Then
                                SubPosition = 9
                                If ctrl.AccessibleName <> "" Then
                                    SubPosition = 10
                                    If "Model[" & intModelInstance & "]." & ctrl.AccessibleName = row(6).ToString Then
                                        SubPosition = 11
                                        If TypeOf ctrl Is TextBox Then
                                            ctrl.text = row(7).ToString
                                        ElseIf TypeOf ctrl Is ComboBox Then
                                            ctrl.text = row(7).ToString
                                        ElseIf TypeOf ctrl Is CheckBox Then
                                            ctrl.checked = row(7).ToString
                                        End If
                                        Exit For
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            Else
                SubPosition = 12
                Dim intPosition As Integer
                For Each ctrl In GroupBox2.Controls
                    SubPosition = 13
                    If TypeOf ctrl Is TextBox Then
                        ctrl.text = "0"
                    ElseIf TypeOf ctrl Is ComboBox Then
                        intPosition = InStr(ctrl.Tag, "BASE")
                        If intPosition = 0 Then
                            ctrl.SelectedIndex = 0
                        End If
                    ElseIf TypeOf ctrl Is CheckBox Then
                        ctrl.checked = False
                    ElseIf TypeOf ctrl Is Button Then
                        'ctrl.AccessibleName = ""
                        'ctrl.AccessibleDescription = ""
                    End If
                Next
            End If

            cnn.Close()
            bISDirty = False
        Catch ex As Exception
            SubPosition = SubPosition
            Log_Anything("GetTaskConfigurationFromDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub GetModelStructureFromDatabase(ByVal strTaskName As String, ByVal intTaskInstance As Integer)
        Try
            Dim Table_ As String = "Areas_Configuration"
            Dim strAreaName As String = ""
            Dim strSectionName As String = ""
            Dim strStationName As String = ""
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)

            Dim strModelAffiliation As String = ""
            Dim strMemberName As String = ""
            Dim strMemberType As String = ""
            Dim strMemberBase As String = ""
            Dim strGlobalDescription As String = ""
            Dim strTagName As String = ""

            Dim intIndexPosition As Integer
            Dim strTempMemberName As String = ""

            Dim strTempType As String = ""
            Dim intPositionOfIndex As Integer
            Dim int2ndPositionOfIndex As Integer

            Dim strTypeAndLengthOfIndex As String = ""
            Dim bModelsAvailable As Boolean
            Dim bMemberIsBase As Boolean
            Dim strTaskInstance As String = ""

            Dim bModelIsForTask As Boolean
            Dim bTypeAndLengthFound As Boolean

            If intTaskInstance = 99 Then
                strTaskInstance = ""
            Else
                strTaskInstance = "[" & intTaskInstance & "]"
            End If


            'Remove any extra controls before adding new controls
            DeleteConfigurationItemsFromModelGroupBox()

            If tvwProject.SelectedNode.Parent.Tag <> "TASK" Then
                If intTaskInstance > -1 Then
                    strTaskName = tvwProject.SelectedNode.Text
                    strStationName = tvwProject.SelectedNode.Parent.Parent.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                    modelItemNumber = 1
                    bModelIsForTask = True
                ElseIf intTaskInstance = -1 Then
                    'strTaskName = tvwProject.SelectedNode.Text
                    strStationName = tvwProject.SelectedNode.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Text
                    modelItemNumber = 1
                End If
            Else
                If intTaskInstance > -1 Then
                    strTaskName = tvwProject.SelectedNode.Name
                    strStationName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Parent.Text
                    modelItemNumber = 1
                    bModelIsForTask = True
                ElseIf intTaskInstance = -1 Then
                    'strTaskName = tvwProject.SelectedNode.Text
                    strStationName = tvwProject.SelectedNode.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Text
                    modelItemNumber = 1
                End If
            End If


            'Open Connection to Database
            cnn.Open()

            'Setup Model Query - Get all configured Models
            Dim query As String = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "'"
            Dim cmd As New OleDbCommand(query, cnn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)
            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow
            For Each row In t1.Rows
                cbo_Models.Items.Add(row(3).ToString)
                bModelsAvailable = True
            Next

            If bModelsAvailable = True And bModelIsForTask Then
                lbl_Models_lbl.Visible = True
                cbo_Models.Visible = True
                cbo_Models.Text = cbo_Models.Items(0)


                Table_ = "Tasks"
                query = "SELECT ModelAffiliation FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "'"

                'Get Member Affiliation from Task
                cmd = New OleDbCommand(query, cnn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds, Table_)
                t1 = ds.Tables(Table_)
                For Each row In t1.Rows
                    strModelAffiliation = row(0).ToString
                    Exit For
                Next

                'Get Model Member Type that will be used to get the structure layout of this Tasks Model information
                Table_ = "Area_Structures"
                query = "SELECT * FROM " & Table_ & " WHERE (MemberName = '" & strModelAffiliation & "' OR MemberAffiliation = '" & strModelAffiliation & "') AND Visible = 1 ORDER BY MEMBERORDER"
                cmd = New OleDbCommand(query, cnn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds, Table_)
                Dim t2 As DataTable
                t2 = ds.Tables(Table_)
                Dim row2 As DataRow
                For Each row2 In t2.Rows
                    strMemberName = row2(2).ToString
                    strMemberType = row2(3).ToString
                    strMemberBase = row2(12).ToString
                    strGlobalDescription = row2(intGlobalDescriptionToUse).ToString


                    'Check to see if the Model Member Type is a BASE Type (BOOL, SINT, INT, DINT, STRING) if not then get the UDT STructure from the MEMBERS TABLE
                    If strMemberType <> "BOOL" And strMemberType <> "SINT" And strMemberType <> "INT" And strMemberType <> "DINT" And strMemberType <> "STRING" And strMemberType <> "REAL" Then

                        intIndexPosition = InStr(strMemberType, "[")
                        If intIndexPosition <> 0 Then
                            strTempMemberName = Mid(strMemberType, 1, intIndexPosition - 1)
                        Else
                            strTempMemberName = strMemberType
                        End If

                        Table_ = "Member_Structure"
                        query = "SELECT * FROM " & Table_ & " WHERE Member = '" & strTempMemberName & "' AND Visible = 1 ORDER BY MEMBERORDER"
                        cmd = New OleDbCommand(query, cnn)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(ds, Table_)
                        t1 = ds.Tables(Table_)

                        'Get Model Structure Setup information
                        For Each row In t1.Rows
                            intPositionOfIndex = 0
                            int2ndPositionOfIndex = 0
                            bTypeAndLengthFound = False
                            intPositionOfIndex = InStr(row(3).ToString, "[")
                            If intPositionOfIndex <> 0 Then
                                int2ndPositionOfIndex = InStrRev(row(3).ToString, "[")
                                If int2ndPositionOfIndex <> intPositionOfIndex Then
                                    strTempType = Mid(row(3).ToString, 1, int2ndPositionOfIndex - 1)
                                    strTypeAndLengthOfIndex = strTempType & "," & Mid(row(3).ToString, int2ndPositionOfIndex + 1, Len(row(3).ToString) - (int2ndPositionOfIndex + 1))
                                    strTempType = Mid(strTempType, 1, intPositionOfIndex - 1)
                                    bTypeAndLengthFound = True
                                Else
                                    strTempType = Mid(row(3).ToString, 1, intPositionOfIndex - 1)
                                    strTypeAndLengthOfIndex = strTempType & "," & Mid(row(3).ToString, intPositionOfIndex + 1, Len(row(3).ToString) - (intPositionOfIndex - 1))
                                    bTypeAndLengthFound = True
                                End If
                            Else
                                strTempType = row(3).ToString
                                strTypeAndLengthOfIndex = ""
                            End If

                            strTempType = UCase(strTempType)
                            If bTypeAndLengthFound = False Then
                                intPositionOfIndex = InStr(strTypeAndLengthOfIndex, "]")
                                If intPositionOfIndex > 0 And intPositionOfIndex < Len(strTypeAndLengthOfIndex) Then
                                    strTypeAndLengthOfIndex = Mid(strTypeAndLengthOfIndex, 1, intPositionOfIndex - 1)
                                End If
                            End If

                            If row(2).ToString = "Enabled" Then
                                chk_ModelEnabled.AccessibleName = strMemberName & strTaskInstance & "." & row(2).ToString
                                If row(intGlobalDescriptionToUse).ToString <> "" Then
                                    chk_ModelEnabled.Text = row(intGlobalDescriptionToUse).ToString
                                Else
                                    chk_ModelEnabled.Text = row(2).ToString
                                End If
                                chk_ModelEnabled.Checked = False
                                chk_ModelEnabled.Visible = True
                            ElseIf strTempType = "BOOL" Then
                                AddConfigurationItemToModelGroupBox(strTempType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                            ElseIf strTempType = "SINT" Then
                                AddConfigurationItemToModelGroupBox(strTempType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                            ElseIf strTempType = "INT" Then
                                AddConfigurationItemToModelGroupBox(strTempType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                            ElseIf strTempType = "DINT" Then
                                AddConfigurationItemToModelGroupBox(strTempType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                            ElseIf strTempType = "REAL" Then
                                AddConfigurationItemToModelGroupBox(strTempType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                            ElseIf strTempType = "STRING" Then
                                AddConfigurationItemToModelGroupBox(strTempType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                            End If

                        Next
                    Else
                        bMemberIsBase = False
                        If IsNumeric(strMemberBase) Then
                            If strMemberBase > 0 Then
                                For Each ctrl In GroupBox2.Controls
                                    strTagName = ctrl.tag
                                    If ctrl.Tag = "BASE" & strMemberBase Then
                                        ctrl.Visible = True
                                        If strTaskInstance <> "" Then
                                            ctrl.AccessibleName = strMemberName & strTaskInstance & "." & strMemberName
                                            ctrl.AccessibleDescription = strMemberType
                                        Else
                                            ctrl.AccessibleName = strMemberName
                                            ctrl.AccessibleDescription = strMemberType
                                        End If

                                        If TypeOf ctrl Is CheckBox Then
                                            If strGlobalDescription <> "" Then
                                                ctrl.Text = strGlobalDescription
                                            Else
                                                ctrl.text = strMemberName
                                            End If
                                            ctrl.checked = False
                                            bMemberIsBase = True
                                        End If
                                        'Exit For
                                    ElseIf ctrl.tag = "BASE" & strMemberBase & "_0" Then
                                        ctrl.Visible = True
                                        If strGlobalDescription <> "" Then
                                            ctrl.Text = strGlobalDescription
                                        Else
                                            ctrl.text = strMemberName
                                        End If
                                        bMemberIsBase = True
                                        'Exit For
                                    End If
                                Next
                            End If
                        End If

                        If bMemberIsBase = True Then
                            'Nothing to do, just dont want to add the control again because it is a base control 
                        ElseIf strMemberType = "BOOL" Then
                            AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex)
                        ElseIf strMemberType = "SINT" Then
                            AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex)
                        ElseIf strMemberType = "INT" Then
                            AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex)
                        ElseIf strMemberType = "DINT" Then
                            AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex)
                        ElseIf strMemberType = "REAL" Then
                            AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row2(2).ToString, row2(intGlobalDescriptionToUse).ToString, row2(4).ToString, row2(8).ToString, strTypeAndLengthOfIndex)
                        End If
                    End If
                Next
            ElseIf bModelsAvailable = True And bModelIsForTask = False Then
                lbl_Models_lbl.Visible = True
                cbo_Models.Visible = True
                cbo_Models.Text = cbo_Models.Items(0)
                'Table_ = "Stations"
                'query = "SELECT ModelAffiliation FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "'"

                ''Get Member Affiliation from Task
                'cmd = New OleDbCommand(query, cnn)
                'da = New OleDbDataAdapter(cmd)
                'da.Fill(ds, Table_)
                't1 = ds.Tables(Table_)
                'For Each row In t1.Rows
                '    strModelAffiliation = row(0).ToString
                '    Exit For
                'Next

                'If strModelAffiliation <> "" Then
                'Get Model Member Type that will be used to get the structure layout of this Station Model information
                Table_ = "Area_Structures"
                query = "SELECT * FROM " & Table_ & " WHERE MemberAffiliation = 'Station' AND Visible = 1 ORDER BY MEMBERORDER"
                cmd = New OleDbCommand(query, cnn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds, Table_)
                t1 = ds.Tables(Table_)
                For Each row In t1.Rows
                    strMemberName = row(2).ToString
                    strMemberType = row(3).ToString
                Next

                bMemberIsBase = False
                If IsNumeric(row(12).ToString) Then
                    If row(12).ToString > 0 Then
                        For Each ctrl In GroupBox2.Controls
                            strTagName = ctrl.tag
                            If ctrl.Tag = "BASE" & row(12).ToString Then
                                ctrl.Visible = True
                                ctrl.AccessibleName = row(2).ToString
                                If TypeOf ctrl Is CheckBox Then
                                    If row(intGlobalDescriptionToUse).ToString <> "" Then
                                        ctrl.Text = row(intGlobalDescriptionToUse).ToString
                                        ctrl.AccessibleDescription = row(3).ToString
                                    Else
                                        ctrl.text = row(2).ToString
                                        ctrl.AccessibleDescription = row(3).ToString
                                    End If
                                    bMemberIsBase = True
                                End If
                                'Exit For
                            ElseIf ctrl.tag = "BASE" & row(12).ToString & "_0" Then
                                ctrl.Visible = True
                                If row(intGlobalDescriptionToUse).ToString <> "" Then
                                    ctrl.Text = row(intGlobalDescriptionToUse).ToString
                                Else
                                    ctrl.text = row(2).ToString
                                End If
                                bMemberIsBase = True
                                'Exit For
                            End If
                        Next
                    End If
                End If

                If bMemberIsBase = True Then
                    'Nothing to do, just dont want to add the control again because it is a base control 
                ElseIf strMemberType = "BOOL" Then
                    AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                ElseIf strMemberType = "SINT" Then
                    AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                ElseIf strMemberType = "INT" Then
                    AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                ElseIf strMemberType = "DINT" Then
                    AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                ElseIf strMemberType = "REAL" Then
                    AddConfigurationItemToModelGroupBox(strMemberType, strMemberName & strTaskInstance, row(2).ToString, row(intGlobalDescriptionToUse).ToString, row(4).ToString, row(8).ToString, strTypeAndLengthOfIndex)
                End If
            End If
            'End If
            cnn.Close()

        Catch ex As Exception
            Log_Anything("GetModelStructureFromDatabase - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub AddConfigurationItemToGroupBox(ByVal configurationType As String, ByVal strConfigurationParent As String, ByVal strConfigurationName As String, ByVal strConfigurationText As String, ByVal strConfigurationPosition As Integer, ByVal strConfigurationValues As String, ByVal strTypeAndLengthOfIndex As String, ByVal strConfigurationGroup As String)
        Dim strConfigValuesArray() As String
        Dim strSingleValueArray() As String
        Dim bAddComboBox As Boolean
        Dim strConfigurationType As String
        Dim intPosition As Integer
        Dim intLeftPosition As Integer
        Dim intTaskItemNumber As Integer
        Dim strTypeAndLength() As String
        Dim i As Integer
        Try
            Dim dpi = CType(Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Control Panel\Desktop\WindowMetrics").GetValue("AppliedDPI"), Integer)
            Dim coefficient = dpi / 96.0
            If strConfigurationText = "" Then
                strConfigurationText = strConfigurationName
            End If

            If strConfigurationValues <> "" Then
                strConfigValuesArray = Split(strConfigurationValues, ",")
                bAddComboBox = True
            End If

            'Location for Label 9,126 360,126
            'Location for Control 213,124 505,124
            'Location for Checkbox 6,126 360, 126

            intPosition = InStr(configurationType, "[")
            If intPosition > 0 Then
                strConfigurationType = Mid(configurationType, 1, intPosition - 1)
            Else
                strConfigurationType = configurationType
            End If

            intTaskItemNumber = taskItemNumber - 1

            If intTaskItemNumber > 12 Then
                intLeftPosition = intTaskItemNumber \ 13
                intTaskItemNumber = intTaskItemNumber - (intLeftPosition * 13)
            End If

            If strTypeAndLengthOfIndex <> "" Then
                strTypeAndLength = Split(strTypeAndLengthOfIndex, ",")
                Dim newLabel As New Label
                newLabel.Size = lbl_TaskType_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_TaskType_lbl.TextAlign

                newLabel.Name = "btn_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (126 + ((intTaskItemNumber) * 30)) * coefficient)
                GroupBox1.Controls.Add(newLabel)

                Dim newButton As New Button
                newButton.Size = Button1.Size
                newButton.Text = Button1.Text
                newButton.Name = "btn_" & strConfigurationName
                newButton.Tag = strConfigurationParent & "." & strConfigurationName & "|" & strConfigurationText
                newButton.AccessibleName = strTypeAndLengthOfIndex

                newButton.AccessibleDescription = ""
                newButton.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                If dpi = 120 Then
                    newButton.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                End If
                AddHandler (newButton.Click), AddressOf Button_Click
                GroupBox1.Controls.Add(newButton)
                bShowIndexForm = False
                newButton.PerformClick()

            ElseIf configurationType = "BOOL" Then
                Dim newCheckBox As New CheckBox
                newCheckBox.Size = chk_TaskBypassed.Size
                newCheckBox.AutoSize = False
                newCheckBox.TextAlign = chk_TaskBypassed.TextAlign
                newCheckBox.CheckAlign = chk_TaskBypassed.CheckAlign
                newCheckBox.AccessibleDescription = configurationType

                newCheckBox.Name = "chk_" & strConfigurationName
                newCheckBox.Text = strConfigurationText
                newCheckBox.Tag = strConfigurationParent & "." & strConfigurationName
                newCheckBox.Location = New Point(6 + ((intLeftPosition) * 354), (126 + ((intTaskItemNumber) * 30)) * coefficient)
                If strConfigurationGroup <> "" Then
                    AddHandler (newCheckBox.CheckedChanged), AddressOf chk_group
                End If
                GroupBox1.Controls.Add(newCheckBox)

            ElseIf configurationType = "SINT" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_TaskType_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_TaskType_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (126 + ((intTaskItemNumber) * 30)) * coefficient)
                GroupBox1.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    Dim strComboValues As String = ""
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                        If strComboValues = "" Then
                            strComboValues = strSingleValueArray(1)
                        Else
                            strComboValues = strComboValues & "," & strSingleValueArray(1)
                        End If
                    Next
                    newComboBox.AccessibleName = strComboValues
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If

            ElseIf configurationType = "INT" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_TaskType_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_TaskType_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (126 + ((intTaskItemNumber) * 30)) * coefficient)
                GroupBox1.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    Dim strComboValues As String = ""
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                        If strComboValues = "" Then
                            strComboValues = strSingleValueArray(1)
                        Else
                            strComboValues = strComboValues & "," & strSingleValueArray(1)
                        End If
                    Next
                    newComboBox.AccessibleName = strComboValues
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If

            ElseIf configurationType = "DINT" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_TaskType_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_TaskType_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (126 + ((intTaskItemNumber) * 30)) * coefficient)
                GroupBox1.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    Dim strComboValues As String = ""
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                        If strComboValues = "" Then
                            strComboValues = strSingleValueArray(1)
                        Else
                            strComboValues = strComboValues & "," & strSingleValueArray(1)
                        End If
                    Next
                    newComboBox.AccessibleName = strComboValues
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If

            ElseIf configurationType = "REAL" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_TaskType_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_TaskType_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (126 + ((intTaskItemNumber) * 30)) * coefficient)
                GroupBox1.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    Dim strComboValues As String = ""
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                        If strComboValues = "" Then
                            strComboValues = strSingleValueArray(1)
                        Else
                            strComboValues = strComboValues & "," & strSingleValueArray(1)
                        End If
                    Next
                    newComboBox.AccessibleName = strComboValues
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If
            ElseIf strConfigurationType = "STRING" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_TaskType_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_TaskType_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (126 + ((intTaskItemNumber) * 30)) * coefficient)
                GroupBox1.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = configurationType
                    If AreaStructureConstant.ModelID.Equals(strConfigurationParent) Then
                        newTextBox.Size = New Size(150, 26)
                    End If
                    GroupBox1.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    Dim strComboValues As String = ""
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (124 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = configurationType
                    GroupBox1.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                        If strComboValues = "" Then
                            strComboValues = strSingleValueArray(1)
                        Else
                            strComboValues = strComboValues & "," & strSingleValueArray(1)
                        End If
                    Next
                    newComboBox.AccessibleName = strComboValues
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If
            End If
            taskItemNumber += 1
        Catch ex As Exception
            Log_Anything("AddConfigurationItemToGroupBox - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub chk_group(sender As Object, e As EventArgs)
        Dim chkNow = CType(sender, CheckBox)
        Dim memberName As String = Mid(chkNow.Name, 5, chkNow.Name.Length - 4)

        Dim taskName = tvwProject.SelectedNode.Text

        Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
        Dim scdbSqlHelper = New SqlHelper(dbConnStr)
        Dim stationStructureController = New StationStructureController(scdbSqlHelper)
        Dim listStationStructure = stationStructureController.GetListStationStructure
        Dim strGroup = ""
        For i = 0 To listStationStructure.Count - 1
            If listStationStructure(i).MemberName = memberName Then
                strGroup = listStationStructure(i).MemberGroup
                Exit For
            End If
        Next
        For i = 0 To listStationStructure.Count - 1
            If listStationStructure(i).MemberName <> memberName And listStationStructure(i).MemberGroup = strGroup And strGroup <> "" Then
                Dim chkMember = CType(Controls.Find("chk_" & listStationStructure(i).MemberName, True).First, CheckBox)
                If chkNow.Checked = True Then
                    chkMember.Checked = False
                End If
            End If
        Next

    End Sub

    Private Sub AddConfigurationItemToModelGroupBox(ByVal ConfigurationType As String, ByVal strConfigurationParent As String, ByVal strConfigurationName As String, ByVal strConfigurationText As String, ByVal ConfigurationPosition As Integer, ByVal strConfigurationValues As String, ByVal strTypeAndLengthOfIndex As String)
        Dim strConfigValuesArray() As String
        Dim strSingleValueArray() As String
        Dim bAddComboBox As Boolean
        Dim strConfigurationType As String
        Dim intPosition As Integer
        Dim intLeftPosition As Integer
        Dim intModelItemNumber As Integer
        Dim strTypeAndLength() As String


        Try
            Dim dpi = CType(Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Control Panel\Desktop\WindowMetrics").GetValue("AppliedDPI"), Integer)
            Dim coefficient = dpi / 96.0
            If strConfigurationText = "" Then
                strConfigurationText = strConfigurationName
            End If

            If strConfigurationValues <> "" Then
                strConfigValuesArray = Split(strConfigurationValues, ",")
                bAddComboBox = True
            End If

            'Location for Label 9,126 360,126
            'Location for Control 213,124 505,124
            'Location for Checkbox 6,126 360, 126

            intPosition = InStr(ConfigurationType, "[")
            If intPosition > 0 Then
                strConfigurationType = Mid(ConfigurationType, 1, intPosition - 1)
            Else
                strConfigurationType = ConfigurationType
            End If

            intModelItemNumber = modelItemNumber - 1

            If intModelItemNumber > 4 Then
                intLeftPosition = intModelItemNumber \ 5
                intModelItemNumber = intModelItemNumber - (intLeftPosition * 5)
            End If

            If strTypeAndLengthOfIndex <> "" Then
                strTypeAndLength = Split(strTypeAndLengthOfIndex, ",")
                Dim newLabel As New Label
                newLabel.Size = lbl_Models_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_Models_lbl.TextAlign

                newLabel.Name = "btn_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (96 + ((intModelItemNumber) * 30)) * coefficient)
                GroupBox2.Controls.Add(newLabel)

                Dim newButton As New Button
                newButton.Size = Button1.Size
                newButton.Text = Button1.Text
                newButton.Name = "btn_" & strConfigurationName
                newButton.Tag = strConfigurationParent & "." & strConfigurationName & "|" & strConfigurationText
                newButton.AccessibleName = strTypeAndLengthOfIndex
                newButton.AccessibleDescription = ""
                newButton.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                If dpi = 120 Then
                    newButton.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                End If
                AddHandler (newButton.Click), AddressOf Button_Click
                GroupBox2.Controls.Add(newButton)
                bShowIndexForm = False
                'newButton.PerformClick()

            ElseIf ConfigurationType = "BOOL" Then
                Dim newCheckBox As New CheckBox
                newCheckBox.Size = chk_ModelEnabled.Size
                newCheckBox.AutoSize = False
                newCheckBox.TextAlign = chk_ModelEnabled.TextAlign
                newCheckBox.CheckAlign = chk_ModelEnabled.CheckAlign
                newCheckBox.AccessibleDescription = ConfigurationType

                newCheckBox.Name = "chk_" & strConfigurationName
                newCheckBox.Text = strConfigurationText
                newCheckBox.Tag = strConfigurationParent & "." & strConfigurationName
                newCheckBox.Location = New Point(6 + ((intLeftPosition) * 354), (96 + ((intModelItemNumber) * 30)) * coefficient)
                GroupBox2.Controls.Add(newCheckBox)

            ElseIf ConfigurationType = "SINT" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_Models_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_Models_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (96 + ((intModelItemNumber) * 30)) * coefficient)
                GroupBox2.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                    Next
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If

            ElseIf ConfigurationType = "INT" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_Models_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_Models_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (96 + ((intModelItemNumber) * 30)) * coefficient)
                GroupBox2.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                    Next
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If

            ElseIf ConfigurationType = "DINT" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_Models_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_Models_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (96 + ((intModelItemNumber) * 30)) * coefficient)
                GroupBox2.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                    Next
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If

            ElseIf ConfigurationType = "REAL" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_Models_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_Models_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (96 + ((intModelItemNumber) * 30)) * coefficient)
                GroupBox2.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                    Next
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If
            ElseIf strConfigurationType = "STRING" Then
                Dim newLabel As New Label
                newLabel.Size = lbl_Models_lbl.Size
                newLabel.AutoSize = False
                newLabel.TextAlign = lbl_Models_lbl.TextAlign

                newLabel.Name = "txt_" & strConfigurationName & "_lbl"
                newLabel.Text = strConfigurationText
                newLabel.Location = New Point((9 + ((intLeftPosition) * 354) * coefficient), (96 + ((intModelItemNumber) * 30)) * coefficient)
                GroupBox2.Controls.Add(newLabel)

                If bAddComboBox = False Then
                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strConfigurationName
                    newTextBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newTextBox)
                Else
                    Dim newComboBox As New ComboBox
                    newComboBox.Name = "cbo_" & strConfigurationName
                    newComboBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newComboBox.Location = New Point((227 + ((intLeftPosition) * 354)) * coefficient, (94 + ((intModelItemNumber) * 30)) * coefficient)
                    End If
                    newComboBox.AccessibleDescription = ConfigurationType
                    GroupBox2.Controls.Add(newComboBox)
                    For i = LBound(strConfigValuesArray) To UBound(strConfigValuesArray)
                        strSingleValueArray = Split(strConfigValuesArray(i), "=")
                        newComboBox.Items.Add(strSingleValueArray(0))
                    Next
                    newComboBox.Tag = strConfigurationParent & "." & strConfigurationName
                    newComboBox.Width = 150
                End If
            End If
            modelItemNumber += 1
        Catch ex As Exception
            Log_Anything("AddConfigurationItemToModelGroupBox - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub add_subject_Click(sender As Object, e As EventArgs)
        Dim tb As New TextBox
        tb.Name = "TextBox" + taskItemNumber.ToString
        tb.Location = New Point(taskItemNumber * 40, 10) ' change this if you want
        Me.Controls.Add(tb)
        Dim lb As New Label
        lb.Name = "Label" + taskItemNumber.ToString
        lb.Location = New Point(taskItemNumber * 40, 50) ' change this if you want
        Me.Controls.Add(lb)
        Dim add As New Button
        add.Name = "AddButton" + taskItemNumber.ToString
        add.Location = New Point(taskItemNumber * 40, 100) ' change this if you want
        AddHandler (add.Click), AddressOf Button_Click
        Me.Controls.Add(add)
        Dim edit As New Button
        edit.Name = "EditButton" + taskItemNumber.ToString
        edit.Location = New Point(taskItemNumber * 40, 150) ' change this if you want
        AddHandler (edit.Click), AddressOf edit_Click 'you have to make edit_Click
        Me.Controls.Add(edit)
        Dim delete As New Button
        delete.Name = "DeleteButton" + taskItemNumber.ToString
        delete.Location = New Point(taskItemNumber * 40, 200) ' change this if you want
        AddHandler (delete.Click), AddressOf delete_Click 'you have to make delete_Click
        Me.Controls.Add(delete)
        taskItemNumber += 1
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs)
        'CType(Me.Controls.Find("Label" + sender.Name.Substring(9), True).First, Label).Text = CType(Me.Controls.Find("TextBox" + sender.Name.Substring(9), True).First, TextBox).Text
        'MsgBox(sender.Tag & "," & sender.AccessibleDescription)
        Dim strTypeAndLengthOfIndex() As String
        Dim strParentMemberAndDisplayText() As String
        Dim strParentAndMember() As String
        Dim intTaskItemNumber As Integer
        Dim intLeftPosition As Integer
        Dim i As Integer
        Dim intFirstWidth As Integer
        Dim intLastWidth As Integer
        Dim intLastHeight As Integer
        Dim intCalcHeight As Integer
        Dim strValueString As String = ""
        Dim bAddCheckBox As Boolean
        Dim bAddTextBox As Boolean
        Dim bValuesPresent As Boolean

        Dim strAllValues() As String
        Dim strSingleValue() As String

        Dim intModelValuePresent As Integer
        Dim tempString As String = "00000000000000000000000000000000"
        Dim intTempValue As Integer = 0
        Dim strSingleBit As String = 0
        Dim intControlPosition As Integer = 0
        Dim intControlValue As Integer
        Dim intControlMark As Integer

        Try
            Dim dpi = CType(Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Control Panel\Desktop\WindowMetrics").GetValue("AppliedDPI"), Integer)
            Dim coefficient = dpi / 96.0
            If sender.AccessibleDescription <> "" Then
                strAllValues = Split(sender.AccessibleDescription, "|")
                bValuesPresent = True
                'MsgBox(sender.AccessibleDescription)
                If bValuesPresent = True Then
                    strSingleValue = Split(strAllValues(0), ",")
                    If UBound(strSingleValue) = 1 Then
                        If IsNumeric(strSingleValue(1)) Then
                            intTempValue = strSingleValue(1)
                            If intTempValue < 0 Then
                                intControlMark = 1
                                intTempValue = -intTempValue
                            End If
                            tempString = Convert.ToString(intTempValue, 2).PadLeft(32, "0"c)
                        End If
                    End If
                End If
            End If



            strTypeAndLengthOfIndex = Split(sender.AccessibleName, ",")
            strTypeAndLengthOfIndex(1) = Replace(strTypeAndLengthOfIndex(1), "[", "")
            strTypeAndLengthOfIndex(1) = Replace(strTypeAndLengthOfIndex(1), "]", "")

            strParentMemberAndDisplayText = Split(sender.Tag, "|")
            strParentAndMember = Split(strParentMemberAndDisplayText(0), ".")

            For i = (Form6.Controls.Count - 1) To 0 Step -1
                Dim ctrl As Control = Form6.Controls(i)
                If ctrl.Tag <> "BASE" Then
                    Form6.Controls.Remove(ctrl)
                    ctrl.Dispose()
                End If
            Next i

            intTaskItemNumber = 1

            bAddCheckBox = False
            bAddTextBox = True
            If InStr(strTypeAndLengthOfIndex(1), "BOOL") Then
                strTypeAndLengthOfIndex(1) = Replace(strTypeAndLengthOfIndex(1), "BOOL", "")
                strTypeAndLengthOfIndex(1) = Replace(strTypeAndLengthOfIndex(1), "(", "")
                strTypeAndLengthOfIndex(1) = Replace(strTypeAndLengthOfIndex(1), ")", "")
                bAddCheckBox = True
                bAddTextBox = False
            End If

            For i = 1 To strTypeAndLengthOfIndex(1)
                If intTaskItemNumber > 25 Then
                    intLeftPosition = intTaskItemNumber \ 13
                    intTaskItemNumber = intTaskItemNumber - (intLeftPosition * 26)
                ElseIf intTaskItemNumber > 12 Then
                    intLeftPosition += 1
                    intTaskItemNumber = 1
                    'intTaskItemNumber = intTaskItemNumber - (intLeftPosition * 13)
                End If

                If bAddTextBox = True Then
                    Dim newLabel As New Label
                    newLabel.Size = lbl_TaskType_lbl.Size
                    newLabel.AutoSize = False
                    newLabel.TextAlign = lbl_TaskType_lbl.TextAlign
                    newLabel.Name = "txt_" & strParentAndMember(1) & "_lbl"
                    If i < 10 Then
                        newLabel.Text = strParentMemberAndDisplayText(1) & "[0" & i & "]"
                    Else
                        newLabel.Text = strParentMemberAndDisplayText(1) & "[" & i & "]"
                    End If

                    newLabel.Location = New Point((9 + ((intLeftPosition) * 354)) * coefficient, (66 + ((intTaskItemNumber) * 30)) * coefficient)
                    Form6.Controls.Add(newLabel)


                    Dim newTextBox As New TextBox
                    newTextBox.Name = "txt_" & strParentAndMember(1)
                    newTextBox.Tag = strParentMemberAndDisplayText(0) & "[" & i - 1 & "]"
                    newTextBox.Location = New Point((213 + ((intLeftPosition) * 352)) * coefficient, (64 + ((intTaskItemNumber) * 30)) * coefficient)
                    If dpi = 120 Then
                        newTextBox.Location = New Point((227 + ((intLeftPosition) * 352)) * coefficient, (64 + ((intTaskItemNumber) * 30)) * coefficient)
                    End If
                    newTextBox.Text = 0
                    If bValuesPresent Then
                        For k = LBound(strAllValues) To UBound(strAllValues)
                            strSingleValue = Split(strAllValues(k), ",")
                            intModelValuePresent = InStr(strSingleValue(0), newTextBox.Tag)
                            If intModelValuePresent > 0 Then
                                'If strSingleValue(0) = newTextBox.Tag Then
                                newTextBox.Text = strSingleValue(1)
                                Exit For
                            End If
                        Next
                    End If

                    Form6.Controls.Add(newTextBox)
                    intTaskItemNumber += 1
                    If i = 1 Then
                        intFirstWidth = newLabel.Left
                    End If
                    intLastWidth = newTextBox.Left + newTextBox.Width
                    intCalcHeight = newTextBox.Top + newTextBox.Height + 10
                    If intCalcHeight > intLastHeight Then
                        intLastHeight = intCalcHeight
                    End If

                    If sender.AccessibleDescription = "" Then
                        If strValueString = "" Then
                            strValueString = newTextBox.Tag & "," & newTextBox.Text
                        Else
                            strValueString = strValueString & "|" & newTextBox.Tag & "," & newTextBox.Text
                        End If
                    Else
                        strValueString = sender.AccessibleDescription
                    End If

                ElseIf bAddTextBox = False And bAddCheckBox = True Then

                    Dim newCheckBox As New CheckBox

                    newCheckBox.Tag = strParentMemberAndDisplayText(0) & "[" & i - 1 & "]"
                    newCheckBox.Size = lbl_TaskType_lbl.Size
                    newCheckBox.AutoSize = False
                    newCheckBox.TextAlign = lbl_TaskType_lbl.TextAlign
                    newCheckBox.Name = "txt_" & strParentAndMember(1) & "_lbl"
                    newCheckBox.CheckAlign = newCheckBox.TextAlign
                    If i < 10 Then
                        newCheckBox.Text = strParentMemberAndDisplayText(1) & "[0" & i & "]"
                    Else
                        newCheckBox.Text = strParentMemberAndDisplayText(1) & "[" & i & "]"
                    End If

                    newCheckBox.Location = New Point((9 + ((intLeftPosition) * 354)) * coefficient, (66 + ((intTaskItemNumber) * 30)) * coefficient)

                    If bValuesPresent Then
                        'For k = LBound(strAllValues) To UBound(strAllValues)
                        '    strSingleValue = Split(strAllValues(k), ",")
                        '    If strSingleValue(0) = newCheckBox.Tag Then
                        '        newCheckBox.Checked = strSingleValue(1)
                        '        Exit For
                        '    End If
                        '    tempString = Convert.ToBoolean(strSingleValue(1))
                        'Next
                        If i = 1 Then
                            If intControlMark = 1 Then
                                newCheckBox.Checked = True
                            Else
                                newCheckBox.Checked = False
                            End If
                        Else
                            strSingleBit = Mid(tempString, 33 - i + 1, 1)
                            If strSingleBit = "1" Then
                                newCheckBox.Checked = True
                            Else
                                newCheckBox.Checked = False
                            End If
                        End If
                    End If

                    Form6.Controls.Add(newCheckBox)

                    intTaskItemNumber += 1
                    If i = 1 Then
                        intFirstWidth = newCheckBox.Left
                    End If
                    intLastWidth = newCheckBox.Left + newCheckBox.Width
                    intCalcHeight = newCheckBox.Top + newCheckBox.Height + 10
                    If intCalcHeight > intLastHeight Then
                        intLastHeight = intCalcHeight
                    End If

                    If intControlPosition = 0 Then
                        If newCheckBox.Checked = True Then
                            intControlMark = 1
                        Else
                            intControlMark = 0
                        End If
                    ElseIf intControlPosition = 1 Then
                        If newCheckBox.Checked = True Then
                            intControlValue = 1
                        Else
                            intControlValue = 0
                        End If
                    Else
                        If newCheckBox.Checked = True Then
                            intControlValue = intControlValue + (2 ^ (intControlPosition - 1))
                        End If
                    End If
                    intControlPosition += 1

                    If sender.AccessibleDescription = "" Then
                        If strValueString = "" Then
                            strValueString = newCheckBox.Tag & "," & newCheckBox.Checked
                        Else
                            strValueString = strValueString & "|" & newCheckBox.Tag & "," & newCheckBox.Checked
                        End If
                    Else
                        strValueString = sender.AccessibleDescription
                    End If
                End If
            Next
            If intControlMark = 1 Then
                intControlValue = -intControlValue
            End If

            Dim tmpTagNameAndDescription() As String
            tmpTagNameAndDescription = Split(sender.Tag, "|")

            If bAddCheckBox = True And bAddTextBox = False Then
                sender.AccessibleDescription = tmpTagNameAndDescription(0) & "," & intControlValue
            Else
                sender.AccessibleDescription = strValueString
            End If

            Form6.AccessibleDescription = sender.AccessibleDescription
            Form6.Text = strParentMemberAndDisplayText(1) & " Configuration"
            Form6.Tag = sender.Tag

            Form6.Width = intLastWidth + 50
            If Form6.Width < 250 Then Form6.Width = 250

            Form6.Height = intLastHeight + 50
            Form6.Left = 0
            Form6.Top = 0
            If bShowIndexForm = True Then
                Form6.Show()
            End If

            If strUserName = "Operator" Then
                For Each ctrl In Form6.Controls
                    ctrl.enabled = False
                Next
                Form6.btnClose.Enabled = True
            End If

        Catch ex As Exception
            Log_Anything("Form2_Button_Click - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub edit_Click(sender As Object, e As EventArgs)
        CType(Me.Controls.Find("Label" + sender.Name.Substring(9), True).First, Label).Text = CType(Me.Controls.Find("TextBox" + sender.Name.Substring(9), True).First, TextBox).Text
    End Sub

    Private Sub delete_Click(sender As Object, e As EventArgs)
        CType(Me.Controls.Find("Label" + sender.Name.Substring(9), True).First, Label).Text = CType(Me.Controls.Find("TextBox" + sender.Name.Substring(9), True).First, TextBox).Text
    End Sub

    Private Sub save(sender As Object, e As EventArgs)
        For i = 1 To taskItemNumber
            'Dim value As String
            'value = CType(Me.Controls.Find("Label" + number.ToString).First, Label).Text
            'insert into database
        Next
    End Sub

    Private Sub tvwProject_BeforeSelect(sender As Object, e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tvwProject.BeforeSelect
        Try
            Dim Response As Integer

            If IsArray(InitialValueArrayGB1) Then
                If UBound(InitialValueArrayGB1) > 0 Then
                    If ISDirtyGB1() Then
                        Response = MsgBox(GroupBox1.Tag & " Values have Changed. Would you like to save your work?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                        If Response = 6 Then
                            cmdSave_Main.PerformClick()
                        End If
                    End If
                End If
            End If

            If IsArray(InitialValueArrayGB2) Then
                If UBound(InitialValueArrayGB2) > 0 Then
                    If ISDirtyGB2() Then
                        Response = MsgBox("Model Values have Changed. Would you like to save your work?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                        If Response = 6 Then
                            cmdSave_Model.PerformClick()
                        End If
                    End If
                End If
            End If

            ReDim InitialValueArrayGB1(0)
            ReDim InitialValueArrayGB2(0)

            If strLastTreePath <> "" Then
                Dim strPathArray() As String
                strPathArray = Split(strLastTreePath, "\")

                For Each MainNode In tvwProject.Nodes
                    If MainNode.Text = strPathArray(0) Then
                        For Each AreaNode In MainNode.Nodes
                            If AreaNode.Name = strPathArray(1) Then
                                For Each SectionNode In AreaNode.Nodes
                                    If SectionNode.Name = strPathArray(2) Then
                                        For Each StationNode In SectionNode.Nodes
                                            If StationNode.Name = strPathArray(3) Then
                                                For Each MasterFileNode In StationNode.Nodes
                                                    If MasterFileNode.Text = strPathArray(4) Then
                                                        For Each TaskNode In MasterFileNode.Nodes
                                                            'If TaskNode.Name = strPathArray(5) Then
                                                            TaskNode.Collapse()
                                                            'Exit For
                                                            'End If
                                                        Next
                                                    End If
                                                Next
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next

                'tvwProject.Nodes(strPathArray(0)).Nodes(strPathArray(1)).Nodes(strPathArray(2)).Nodes(strPathArray(2)).Nodes(strPathArray(3)).Nodes(strPathArray(4)).Nodes(strPathArray(5)).Collapse()
                'MsgBox("Did it")
            End If
        Catch ex As Exception
            Log_Anything("tvwProject_BeforeSelect - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Public Sub Reload(ByVal node As System.Windows.Forms.TreeNode)
        Try
            SequenceMenuItem.Enabled = False
            DownloadTaskConfigurationToolStripMenuItem.Enabled = False
            DownloadModelInformationToolStripMenuItem.Enabled = False
            DownloadStationConfigurationToolStripMenuItem.Enabled = False
            DownloadAllConfigurationToolStripMenuItem.Enabled = False
            UploadModelConfigurationToolStripMenuItem.Enabled = False
            UploadProjectConfigurationToolStripMenuItem.Enabled = False
            UploadAllConfigurationToolStripMenuItem.Enabled = False

            If node.Tag = "PLC" Then
                SplitContainer1.Visible = True
                SplitContainer2.Visible = True
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = False
                cmdSave_Model.Enabled = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "PLC"
                GetPLCStructureFromDatabase(node.Text)
                GetPLCConfigurationFromDatabase()
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                GroupBox1.Text = "PLC Configuration"
                GroupBox2.Text = "Model Configuration"
            ElseIf node.Tag = "TASK" Then
                Dim intchildnode As Integer
                intchildnode = (node.GetNodeCount(True))
                If intchildnode <> 0 Then
                    DeleteConfigurationItemsFromGroupBox()
                    DeleteConfigurationItemsFromModelGroupBox()
                    cmdSave_Main.Visible = False
                    cmdSave_Model.Visible = False
                    cmdDelete_Main.Visible = False
                    cmdDelete_Model.Visible = False
                    GroupBox1.Tag = "TASK"
                    GroupBox1.Text = "Task Configuration"
                    GroupBox2.Text = "Model Configuration"
                    Exit Sub
                End If

                SplitContainer1.Visible = True
                SplitContainer2.Visible = True

                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()

                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = True
                cmdSave_Model.Enabled = True

                cmdDelete_Main.Visible = True
                cmdDelete_Main.Enabled = True

                node.Expand()
                strLastTreePath = node.FullPath
                UpdatePending = 1
                NumericUpDownTaskInstance.Value = 1
                GetTaskStructureFromDatabase(node.Text, 0)
                GetTaskConfigurationFromDatabase(0)

                GroupBox1.Tag = "TASK"

                If NumericUpDownTaskInstance.Visible = True Then
                    GetModelStructureFromDatabase(node.Text, 0)
                    GetModelConfigurationFromDatabase(0)
                Else
                    GetModelStructureFromDatabase(node.Text, 99)
                    GetModelConfigurationFromDatabase(0)
                End If
                UpdatePending = 0
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                currentTaskItemNumber = 1
                GroupBox1.Text = "Task Configuration"
                GroupBox2.Text = "Model Configuration"
                'SequenceMenuItem.Enabled = True
            ElseIf InStr(node.Tag, "TASK") Then
                Dim TaskInfo() As String
                TaskInfo = Split(node.Tag, "|")

                SplitContainer1.Visible = True
                SplitContainer2.Visible = True

                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()

                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = True
                cmdSave_Model.Enabled = True

                cmdDelete_Main.Visible = True
                cmdDelete_Main.Enabled = True

                UpdatePending = 1
                NumericUpDownTaskInstance.Minimum = 0
                NumericUpDownTaskInstance.Maximum = node.Parent.GetNodeCount(True)

                NumericUpDownTaskInstance.Value = TaskInfo(1)
                GetTaskStructureFromDatabase(node.Name, TaskInfo(1) - 1, True)
                GetTaskConfigurationFromDatabase(TaskInfo(1) - 1, True)

                GroupBox1.Tag = "TASK"
                If NumericUpDownTaskInstance.Visible = True Then
                    GetModelStructureFromDatabase(node.Name, TaskInfo(1) - 1)
                    GetModelConfigurationFromDatabase(0)
                Else
                    GetModelStructureFromDatabase(node.Name, 99)
                    GetModelConfigurationFromDatabase(0)
                End If
                UpdatePending = 0
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                currentTaskItemNumber = TaskInfo(1)
                GroupBox1.Text = "Task Configuration"
                GroupBox2.Text = "Model Configuration"
                'SequenceMenuItem.Enabled = True
            ElseIf node.Tag = "MASTERFILE" Then
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = False
                cmdSave_Model.Visible = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "MASTERFILE"
                GroupBox1.Text = "Masterfile Configuration"
                GroupBox2.Text = "Model Configuration"
            ElseIf node.Tag = "STATION" Then
                SplitContainer1.Visible = True
                SplitContainer2.Visible = True
                'SplitContainer2.Panel2.Hide()
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = True
                cmdSave_Model.Enabled = True
                cmdDelete_Main.Visible = True
                cmdDelete_Main.Enabled = True
                GroupBox1.Tag = "STATION"
                GetStationStructureFromDatabase(node.Text)
                GetStationConfigurationFromDatabase()
                GetModelStructureFromDatabase(node.Text, -1)
                GetModelConfigurationFromDatabase(0)
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                GroupBox1.Text = "Station Configuration"
                GroupBox2.Text = "Model Configuration"
                SequenceMenuItem.Enabled = True
                CheckUserLevel()
            ElseIf node.Tag = "SECTION" Then
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = False
                cmdSave_Model.Visible = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "SECTION"
                GroupBox1.Text = "Section Configuration"
                GroupBox2.Text = "Model Configuration"
            ElseIf node.Tag = "AREA" Then
                SplitContainer1.Visible = True
                SplitContainer2.Visible = True
                'SplitContainer2.Panel2.Hide()
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = False
                cmdSave_Model.Enabled = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "AREA"
                GetAreaStructureFromDatabase(node.Text)
                GetAreaConfigurationFromDatabase()
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                GroupBox1.Text = "Area Configuration"
                GroupBox2.Text = "Model Configuration"
            ElseIf node.Tag = "PLANT" Then
                SplitContainer1.Visible = True
                SplitContainer2.Visible = True
                'SplitContainer2.Panel2.Hide()
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = False
                cmdSave_Main.Enabled = False
                cmdSave_Model.Visible = False
                cmdSave_Model.Enabled = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "PLANT"
                GroupBox1.Text = "Plant Configuration"
                GroupBox2.Text = "Model Configuration"
            End If

            If strUserName = "Operator" Then
                For Each ctrl In GroupBox1.Controls
                    If Not TypeOf ctrl Is Button Then
                        ctrl.Enabled = False
                    End If

                Next

                For Each ctrl In GroupBox2.Controls

                    If Not TypeOf ctrl Is Button Then
                        ctrl.Enabled = False
                    End If

                Next

                cmdDelete_Main.Enabled = False
                cmdSave_Main.Enabled = False
                cbo_Models.Enabled = True
                cmdDelete_Model.Enabled = False
                cmdSave_Model.Enabled = False
            End If

        Catch ex As Exception
            Log_Anything("tvwProject_AfterSelect - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub tvwProject_AfterSelect(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles tvwProject.AfterSelect
        Try
            SequenceMenuItem.Enabled = False
            DownloadTaskConfigurationToolStripMenuItem.Enabled = False
            DownloadModelInformationToolStripMenuItem.Enabled = False
            DownloadStationConfigurationToolStripMenuItem.Enabled = False
            DownloadAllConfigurationToolStripMenuItem.Enabled = False
            UploadModelConfigurationToolStripMenuItem.Enabled = False
            UploadProjectConfigurationToolStripMenuItem.Enabled = False
            UploadAllConfigurationToolStripMenuItem.Enabled = False

            If e.Node.Tag = "PLC" Then
                SplitContainer1.Visible = True
                SplitContainer2.Visible = True
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = False
                cmdSave_Model.Enabled = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "PLC"
                GetPLCStructureFromDatabase(e.Node.Text)
                GetPLCConfigurationFromDatabase()
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                GroupBox1.Text = "PLC Configuration"
                GroupBox2.Text = "Model Configuration"
            ElseIf e.Node.Tag = "TASK" Then
                Dim intchildnode As Integer
                intchildnode = (e.Node.GetNodeCount(True))
                If intchildnode <> 0 Then
                    DeleteConfigurationItemsFromGroupBox()
                    DeleteConfigurationItemsFromModelGroupBox()
                    cmdSave_Main.Visible = False
                    cmdSave_Model.Visible = False
                    cmdDelete_Main.Visible = False
                    cmdDelete_Model.Visible = False
                    GroupBox1.Tag = "TASK"
                    GroupBox1.Text = "Task Configuration"
                    GroupBox2.Text = "Model Configuration"
                    Exit Sub
                End If

                SplitContainer1.Visible = True
                SplitContainer2.Visible = True

                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()

                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = True
                cmdSave_Model.Enabled = True

                cmdDelete_Main.Visible = True
                cmdDelete_Main.Enabled = True

                e.Node.Expand()
                strLastTreePath = e.Node.FullPath
                UpdatePending = 1
                NumericUpDownTaskInstance.Value = 1
                GetTaskStructureFromDatabase(e.Node.Text, 0)
                GetTaskConfigurationFromDatabase(0)

                GroupBox1.Tag = "TASK"

                If NumericUpDownTaskInstance.Visible = True Then
                    GetModelStructureFromDatabase(e.Node.Text, 0)
                    GetModelConfigurationFromDatabase(0)
                Else
                    GetModelStructureFromDatabase(e.Node.Text, 99)
                    GetModelConfigurationFromDatabase(0)
                End If
                UpdatePending = 0
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                currentTaskItemNumber = 1
                GroupBox1.Text = "Task Configuration"
                GroupBox2.Text = "Model Configuration"
                'SequenceMenuItem.Enabled = True
            ElseIf InStr(e.Node.Tag, "TASK") Then
                Dim TaskInfo() As String
                TaskInfo = Split(e.Node.Tag, "|")

                SplitContainer1.Visible = True
                SplitContainer2.Visible = True

                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()

                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = True
                cmdSave_Model.Enabled = True

                cmdDelete_Main.Visible = True
                cmdDelete_Main.Enabled = True

                UpdatePending = 1
                NumericUpDownTaskInstance.Minimum = 0
                NumericUpDownTaskInstance.Maximum = e.Node.Parent.GetNodeCount(True)

                NumericUpDownTaskInstance.Value = TaskInfo(1)
                GetTaskStructureFromDatabase(e.Node.Name, TaskInfo(1) - 1, True)
                GetTaskConfigurationFromDatabase(TaskInfo(1) - 1, True)

                GroupBox1.Tag = "TASK"
                If NumericUpDownTaskInstance.Visible = True Then
                    GetModelStructureFromDatabase(e.Node.Name, TaskInfo(1) - 1)
                    GetModelConfigurationFromDatabase(0)
                Else
                    GetModelStructureFromDatabase(e.Node.Name, 99)
                    GetModelConfigurationFromDatabase(0)
                End If
                UpdatePending = 0
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                currentTaskItemNumber = TaskInfo(1)
                GroupBox1.Text = "Task Configuration"
                GroupBox2.Text = "Model Configuration"
                'SequenceMenuItem.Enabled = True
            ElseIf e.Node.Tag = "MASTERFILE" Then
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = False
                cmdSave_Model.Visible = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "MASTERFILE"
                GroupBox1.Text = "Masterfile Configuration"
                GroupBox2.Text = "Model Configuration"
            ElseIf e.Node.Tag = "STATION" Then
                SplitContainer1.Visible = True
                SplitContainer2.Visible = True
                'SplitContainer2.Panel2.Hide()
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = True
                cmdSave_Model.Enabled = True
                cmdDelete_Main.Visible = True
                cmdDelete_Main.Enabled = True
                GroupBox1.Tag = "STATION"
                GetStationStructureFromDatabase(e.Node.Text)
                GetStationConfigurationFromDatabase()
                GetModelStructureFromDatabase(e.Node.Text, -1)
                GetModelConfigurationFromDatabase(0)
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                GroupBox1.Text = "Station Configuration"
                GroupBox2.Text = "Model Configuration"
                SequenceMenuItem.Enabled = True
                CheckUserLevel()
            ElseIf e.Node.Tag = "SECTION" Then
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = False
                cmdSave_Model.Visible = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "SECTION"
                GroupBox1.Text = "Section Configuration"
                GroupBox2.Text = "Model Configuration"
            ElseIf e.Node.Tag = "AREA" Then
                SplitContainer1.Visible = True
                SplitContainer2.Visible = True
                'SplitContainer2.Panel2.Hide()
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = True
                cmdSave_Main.Enabled = True
                cmdSave_Model.Visible = False
                cmdSave_Model.Enabled = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "AREA"
                GetAreaStructureFromDatabase(e.Node.Text)
                GetAreaConfigurationFromDatabase()
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                GroupBox1.Text = "Area Configuration"
                GroupBox2.Text = "Model Configuration"
            ElseIf e.Node.Tag = "PLANT" Then
                SplitContainer1.Visible = True
                SplitContainer2.Visible = True
                'SplitContainer2.Panel2.Hide()
                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()
                cmdSave_Main.Visible = False
                cmdSave_Main.Enabled = False
                cmdSave_Model.Visible = False
                cmdSave_Model.Enabled = False
                cmdDelete_Main.Visible = False
                cmdDelete_Model.Visible = False
                GroupBox1.Tag = "PLANT"
                GroupBox1.Text = "Plant Configuration"
                GroupBox2.Text = "Model Configuration"
            End If

            If strUserName = "Operator" Then
                For Each ctrl In GroupBox1.Controls
                    If Not TypeOf ctrl Is Button Then
                        ctrl.Enabled = False
                    End If

                Next

                For Each ctrl In GroupBox2.Controls

                    If Not TypeOf ctrl Is Button Then
                        ctrl.Enabled = False
                    End If

                Next

                cmdDelete_Main.Enabled = False
                cmdSave_Main.Enabled = False
                cbo_Models.Enabled = True
                cmdDelete_Model.Enabled = False
                cmdSave_Model.Enabled = False
            End If

        Catch ex As Exception
            Log_Anything("tvwProject_AfterSelect - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub txt_TaskNumber_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaskNumber.KeyPress
        If e.KeyChar = vbCr Then
            Dim i As Integer
            Dim strModelArray() As String

            Try
                If tvwProject.Nodes.Count = 0 Then Exit Sub

                If tvwProject.SelectedNode.Tag = "AREA" Then
                    If bFirstTimeAreaIsLoaded = False Then

                        strModelsString = ""
                        For i = 1 To intNumberOfModelsVisible
                            For Each ctrl In GroupBox1.Controls
                                If ctrl.name = "txt_" & strAreaConfigurationName & "_" & i Then
                                    If strModelsString = "" Then
                                        strModelsString = ctrl.text
                                    Else
                                        strModelsString = strModelsString & "," & ctrl.text
                                    End If
                                    Exit For
                                End If
                            Next
                        Next

                        strModelArray = Split(strModelsString, ",")

                        If txt_TaskNumber.Text < strModelArray.Length Then
                            MsgBox("The number of configured Models exceeds the number you are trying to change to. Please delete the Models before lowering the No. of Models.", MsgBoxStyle.Information, Application.ProductName)
                            txt_TaskNumber.Text = strModelArray.Length
                            Exit Sub
                        ElseIf txt_TaskNumber.Text = strModelArray.Length Then
                            Exit Sub
                        End If

                        'Add Empty Model Text Boxes
                        AddEmptyModels()

                        For i = 0 To UBound(strModelArray)
                            For Each ctrl In GroupBox1.Controls
                                If ctrl.name = "txt_" & strAreaConfigurationName & "_" & i + 1 Then
                                    ctrl.text = strModelArray(i)
                                    Exit For
                                End If
                            Next
                        Next

                        Dim intModelName As Integer
                        For Each ctrl In GroupBox1.Controls
                            intModelName = InStr(ctrl.Name, "txt_ModelID")
                            If intModelName <> 0 Then
                                ctrl.contextmenustrip = ContextCutCopyPaste
                            End If
                        Next
                    Else
                        bFirstTimeAreaIsLoaded = False
                    End If
                End If
            Catch ex As Exception
                Log_Anything("txt_TaskNumber_TextChanged - " & GetExceptionInfo(ex))
            End Try
        End If

    End Sub

    Private Sub AddEmptyModels()
        Dim i As Integer
        Try

            DeleteConfigurationItemsFromGroupBox()
            lbl_TaskType_lbl.Visible = True
            lbl_TaskType_lbl.Text = "Area Name:"
            lbl_TaskType.Visible = True
            lbl_TaskType.Text = tvwProject.SelectedNode.Text

            txt_TaskNumber.Visible = True
            'txt_TaskNumber.Text = 1
            txt_TaskNumber_lbl.Visible = True
            txt_TaskNumber_lbl.Text = "No. Of Models:"


            txt_TaskName_lbl.Visible = False
            txt_TaskName.Visible = False

            chk_TaskBypassed.Checked = False
            chk_TaskBypassed.Visible = False
            taskItemNumber = 1

            If IsNumeric(txt_TaskNumber.Text) Then
                For i = 1 To txt_TaskNumber.Text
                    AddConfigurationItemToGroupBox(strAreaConfigurationType, strAreaConfigurationName, strAreaConfigurationName & "_" & i, taskItemNumber & " - " & strAreaModelDescription, strAreaConfigurationPosition, "", "", "")
                Next
            End If
            intNumberOfModelsVisible = txt_TaskNumber.Text
        Catch ex As Exception
            Log_Anything("AddEmptyModels - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If GroupBox1.Tag = "AREA" Then
            Dim bAreaConfigurationISOk As Boolean
            bAreaConfigurationISOk = CheckAreaConfiguration()
            If bAreaConfigurationISOk Then
                SetAreaConfigurationToDatabase()
            End If
        ElseIf GroupBox1.Tag = "STATION" Then
            Dim bStationConfigurationISOk As Boolean = True
            'bStationConfigurationISOk = CheckAreaConfiguration()
            If bStationConfigurationISOk Then
                SetStationConfigurationToDatabase()
            End If
        ElseIf GroupBox1.Tag = "TASK" Then
            Dim bTaskConfigurationISOk As Boolean = True
            'bStationConfigurationISOk = CheckAreaConfiguration()
            If bTaskConfigurationISOk Then
                If NumericUpDownTaskInstance.Visible = True Then
                    SetTaskConfigurationToDatabase(NumericUpDownTaskInstance.Value)
                    SetModelConfigurationToDatabase(cbo_Models.SelectedIndex, NumericUpDownTaskInstance.Value - 1)
                Else
                    SetTaskConfigurationToDatabase(-1)
                    SetModelConfigurationToDatabase(-1, -1)
                End If
            End If
        ElseIf GroupBox1.Tag = "PLC" Then
            Dim bPLCConfigurationISOk As Boolean = True
            'bStationConfigurationISOk = CheckAreaConfiguration()
            If bPLCConfigurationISOk Then
                SetPLCConfigurationToDatabase()
            End If
        End If
    End Sub

    Private Sub Form2_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        CheckUserLevel()
    End Sub

    Private Sub Form2_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            AskToSave()
            PowertrainProjectFile = ""
            tvwProject.Nodes.Clear()
            DeleteConfigurationItemsFromGroupBox()
            DeleteConfigurationItemsFromModelGroupBox()
            SplitContainer1.Visible = True
            SplitContainer2.Visible = True
            tvwProject.Nodes.Clear()
            intNumberOfModelsVisible = 0
            strAreaConfigurationName = ""
            strAreaConfigurationPosition = ""
            strAreaConfigurationType = ""
            strAreaModelDescription = ""
            cmdSave_Main.Visible = False
            cmdSave_Model.Visible = False
            ReDim InitialValueArrayGB1(0)
            ReDim InitialValueArrayGB2(0)
            Form5.Show()
        Catch ex As Exception
            Log_Anything("Form2_FormClosing - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub Form2_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Me.Text = CommonController.GenerateProductionTitle()

        HelpProvider1.HelpNamespace = strHelpPath

        SplitContainer1.Visible = True
        SplitContainer2.Visible = True
        'SplitContainer2.Panel2.Hide()
        tvwProject.Nodes.Clear()
        intNumberOfModelsVisible = 0
        strAreaConfigurationName = ""
        strAreaConfigurationPosition = ""
        strAreaConfigurationType = ""
        strAreaModelDescription = ""
        If intGlobalDescriptionToUse = 0 Then
            intGlobalDescriptionToUse = 5
        End If
        SetupPreviousFilesMenu()

        'Init performance analysis controller'
        PerformanceAnalysisController.InitPerformanceAnalysisController()
    End Sub


    Private Sub NumericUpDownTaskInstance_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDownTaskInstance.ValueChanged
        'Put in code to move to Instance of Task being dispayed. Remember that the Instance Number being displayed is 1 greater that the actual TAG Offset [0]-[7] = 1-8
        Dim Response As Integer

        If UpdatePending = 0 Then
            If IsArray(InitialValueArrayGB1) Then
                If UBound(InitialValueArrayGB1) > 0 Then
                    If ISDirtyGB1() Then
                        Response = MsgBox(GroupBox1.Tag & " Values have Changed. Would you like to save your work?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                        If Response = 6 Then
                            SetTaskConfigurationToDatabase(currentTaskItemNumber - 1)
                            'cmdSave_Main.PerformClick()
                        End If
                    End If
                End If
            End If

            If IsArray(InitialValueArrayGB2) Then
                If UBound(InitialValueArrayGB2) > 0 Then
                    If ISDirtyGB2() Then
                        Response = MsgBox("Model Values have Changed. Would you like to save your work?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                        If Response = 6 Then
                            cmdSave_Model.PerformClick()
                        End If
                    End If
                End If
            End If

            ReDim InitialValueArrayGB1(0)
            ReDim InitialValueArrayGB2(0)

            currentTaskItemNumber = NumericUpDownTaskInstance.Value

            If NumericUpDownTaskInstance.Value > MaxNumberOfTasks Then
                NumericUpDownTaskInstance.Value = 1
                currentTaskItemNumber = NumericUpDownTaskInstance.Value
                Exit Sub
            ElseIf NumericUpDownTaskInstance.Value < 1 Then
                NumericUpDownTaskInstance.Value = MaxNumberOfTasks
                currentTaskItemNumber = NumericUpDownTaskInstance.Value
                Exit Sub
            End If



            UpdatePending = 1
            DeleteConfigurationItemsFromGroupBox()
            GetTaskStructureFromDatabase(tvwProject.SelectedNode.Text, NumericUpDownTaskInstance.Value - 1)
            GetTaskConfigurationFromDatabase(NumericUpDownTaskInstance.Value - 1)
            GetModelStructureFromDatabase(tvwProject.SelectedNode.Text, NumericUpDownTaskInstance.Value - 1)
            GetModelConfigurationFromDatabase(NumericUpDownTaskInstance.Value - 1)
            SaveInitialValuesOfGroupBox1()
            SaveInitialValuesOfGroupBox2()
            bShowIndexForm = True
            UpdatePending = 0
        End If

    End Sub

    Private Sub PreviousFileOpen_01_Click(sender As System.Object, e As System.EventArgs) Handles PreviousFileOpen_01.Click
        Dim strFileOK As String

        strFileOK = Dir(My.Settings.LastConfigFile01)
        If strFileOK <> "" Then
            If PowertrainProjectFile <> "" Then
                AskToSave()
            End If
            PowertrainProjectFile = My.Settings.LastConfigFile01
            If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                PowertrainProjectFile = PowertrainProjectFile & ".ppf"
            End If
            KeepLastFileNameOpened(PowertrainProjectFile)


            'ChangeProject()
            'Init all controllers after open one ppf, 
            'Must be this time to init, because we use the 'PowertrainProjectFile' to open the access db
            InitAllControllers()

            'Get the Plant Data that is saved in the Database
            GetPlantDataFromDatabase()
            bISDirty = False
            tvwProject.SelectedNode = tvwProject.Nodes(0)
            'Set the Forms Title
            SetFormTitle()

        Else
            MsgBox("File " & My.Settings.LastConfigFile01 & " does not exist.", MsgBoxStyle.Information, Application.ProductName)
            My.Settings.LastConfigFile01 = My.Settings.LastConfigFile02
            My.Settings.LastConfigFile02 = My.Settings.LastConfigFile03
            My.Settings.LastConfigFile03 = My.Settings.LastConfigFile04
            My.Settings.LastConfigFile04 = ""
            My.Settings.Save()
            SetupPreviousFilesMenu()
        End If

    End Sub

    Private Sub PreviousFileOpen_02_Click(sender As System.Object, e As System.EventArgs) Handles PreviousFileOpen_02.Click
        Dim strFileOK As String

        strFileOK = Dir(My.Settings.LastConfigFile02)
        If strFileOK <> "" Then
            If PowertrainProjectFile <> "" Then
                AskToSave()
            End If
            PowertrainProjectFile = My.Settings.LastConfigFile02
            If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                PowertrainProjectFile = PowertrainProjectFile & ".ppf"
            End If
            KeepLastFileNameOpened(PowertrainProjectFile)

            'Init all controllers after open one ppf, 
            'Must be this time to init, because we use the 'PowertrainProjectFile' to open the access db
            InitAllControllers()

            'ChangeProject()
            'Get the Plant Data that is saved in the Database
            GetPlantDataFromDatabase()
            bISDirty = False
            tvwProject.SelectedNode = tvwProject.Nodes(0)
            'Set the Forms Title
            SetFormTitle()
        Else
            MsgBox("File " & My.Settings.LastConfigFile02 & " does not exist.", MsgBoxStyle.Information, Application.ProductName)
            My.Settings.LastConfigFile02 = My.Settings.LastConfigFile03
            My.Settings.LastConfigFile03 = My.Settings.LastConfigFile04
            My.Settings.LastConfigFile04 = ""
            My.Settings.Save()
            SetupPreviousFilesMenu()
        End If
    End Sub

    Private Sub PreviousFileOpen_03_Click(sender As System.Object, e As System.EventArgs) Handles PreviousFileOpen_03.Click
        Dim strFileOK As String

        strFileOK = Dir(My.Settings.LastConfigFile03)
        If strFileOK <> "" Then
            If PowertrainProjectFile <> "" Then
                AskToSave()
            End If
            PowertrainProjectFile = My.Settings.LastConfigFile03
            If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                PowertrainProjectFile = PowertrainProjectFile & ".ppf"
            End If
            KeepLastFileNameOpened(PowertrainProjectFile)

            'Init all controllers after open one ppf, 
            'Must be this time to init, because we use the 'PowertrainProjectFile' to open the access db
            InitAllControllers()
            'ChangeProject()
            'Get the Plant Data that is saved in the Database
            GetPlantDataFromDatabase()
            bISDirty = False
            tvwProject.SelectedNode = tvwProject.Nodes(0)
            'Set the Forms Title
            SetFormTitle()
        Else
            MsgBox("File " & My.Settings.LastConfigFile03 & " does not exist.", MsgBoxStyle.Information, Application.ProductName)
            My.Settings.LastConfigFile03 = My.Settings.LastConfigFile04
            My.Settings.LastConfigFile04 = ""
            My.Settings.Save()
            SetupPreviousFilesMenu()
        End If
    End Sub

    Private Sub PreviousFileOpen_04_Click(sender As System.Object, e As System.EventArgs) Handles PreviousFileOpen_04.Click
        Dim strFileOK As String

        strFileOK = Dir(My.Settings.LastConfigFile04)
        If strFileOK <> "" Then
            If PowertrainProjectFile <> "" Then
                AskToSave()
            End If
            PowertrainProjectFile = My.Settings.LastConfigFile04
            If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                PowertrainProjectFile = PowertrainProjectFile & ".ppf"
            End If
            KeepLastFileNameOpened(PowertrainProjectFile)

            'Init all controllers after open one ppf, 
            'Must be this time to init, because we use the 'PowertrainProjectFile' to open the access db
            InitAllControllers()
            'ChangeProject()
            'Get the Plant Data that is saved in the Database
            GetPlantDataFromDatabase()
            bISDirty = False
            tvwProject.SelectedNode = tvwProject.Nodes(0)
            'Set the Forms Title
            SetFormTitle()
        Else
            MsgBox("File " & My.Settings.LastConfigFile04 & " does not exist.", MsgBoxStyle.Information, Application.ProductName)
            My.Settings.LastConfigFile04 = ""
            My.Settings.Save()
            SetupPreviousFilesMenu()
        End If
    End Sub

    Private Sub cmdSave_Main_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave_Main.Click
        If GroupBox1.Tag = "AREA" Then
            Dim bAreaConfigurationISOk As Boolean
            bAreaConfigurationISOk = CheckAreaConfiguration()
            If bAreaConfigurationISOk Then
                SetAreaConfigurationToDatabase()
            End If
            ReDim InitialValueArrayGB1(0)
            SaveInitialValuesOfGroupBox1()
        ElseIf GroupBox1.Tag = "STATION" Then
            Dim bStationConfigurationISOk As Boolean = True
            'bStationConfigurationISOk = CheckAreaConfiguration()
            If bStationConfigurationISOk Then
                SetStationConfigurationToDatabase()
            End If
            ReDim InitialValueArrayGB1(0)
            SaveInitialValuesOfGroupBox1()

        ElseIf GroupBox1.Tag = "TASK" Then
            Dim bTaskConfigurationISOk As Boolean = True
            'bStationConfigurationISOk = CheckAreaConfiguration()
            If MaxNumberOfTasks = 1 Then
                If bTaskConfigurationISOk Then
                    SetTaskConfigurationToDatabase(-1)
                End If
            Else
                If bTaskConfigurationISOk Then
                    SetTaskConfigurationToDatabase(NumericUpDownTaskInstance.Value - 1)
                End If
            End If

            ReDim InitialValueArrayGB1(0)
            SaveInitialValuesOfGroupBox1()
        ElseIf GroupBox1.Tag = "PLC" Then
            Dim bPLCConfigurationISOk As Boolean = True
            'bStationConfigurationISOk = CheckAreaConfiguration()
            If bPLCConfigurationISOk Then
                SetPLCConfigurationToDatabase()
            End If
            ReDim InitialValueArrayGB1(0)
            SaveInitialValuesOfGroupBox1()
        End If
        VerifyProjectFile(tvwProject.SelectedNode.FullPath)
    End Sub

    Private Sub cmdDelete_Main_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelete_Main.Click
        If GroupBox1.Tag = "AREA" Then
            'Dim bAreaConfigurationISOk As Boolean
            'bAreaConfigurationISOk = CheckAreaConfiguration()
            'If bAreaConfigurationISOk Then
            '    SetAreaConfigurationToDatabase()
            'End If
            'ReDim InitialValueArrayGB1(0)
            'SaveInitialValuesOfGroupBox1()
        ElseIf GroupBox1.Tag = "STATION" Then
            DeleteStationConfiguration()

        ElseIf GroupBox1.Tag = "TASK" Then
            Dim bTaskConfigurationISOk As Boolean = True
            'bStationConfigurationISOk = CheckAreaConfiguration()
            If MaxNumberOfTasks = 1 Then
                If bTaskConfigurationISOk Then
                    DeleteTaskConfiguration(-1)
                End If
            Else
                If bTaskConfigurationISOk Then
                    DeleteTaskConfiguration(NumericUpDownTaskInstance.Value - 1)
                End If
            End If

            ReDim InitialValueArrayGB1(0)
            SaveInitialValuesOfGroupBox1()
        ElseIf GroupBox1.Tag = "PLC" Then
            'Dim bPLCConfigurationISOk As Boolean = True
            ''bStationConfigurationISOk = CheckAreaConfiguration()
            'If bPLCConfigurationISOk Then
            '    SetPLCConfigurationToDatabase()
            'End If
            'ReDim InitialValueArrayGB1(0)
            'SaveInitialValuesOfGroupBox1()
        End If
        VerifyProjectFile(tvwProject.SelectedNode.FullPath)
    End Sub

    Private Sub cmdSave_Model_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave_Model.Click
        If NumericUpDownTaskInstance.Visible = True Then
            SetModelConfigurationToDatabase(cbo_Models.SelectedIndex, NumericUpDownTaskInstance.Value - 1)
        Else
            SetModelConfigurationToDatabase(cbo_Models.SelectedIndex, -1)
        End If
        ReDim InitialValueArrayGB2(0)
        SaveInitialValuesOfGroupBox2()
        'VerifyProjectFile(tvwProject.SelectedNode.FullPath)
    End Sub

    Private Sub cmdDelete_Model_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelete_Model.Click
        'If NumericUpDownTaskInstance.Visible = True Then
        '    SetModelConfigurationToDatabase(cbo_Models.SelectedIndex, NumericUpDownTaskInstance.Value - 1)
        'Else
        '    SetModelConfigurationToDatabase(cbo_Models.SelectedIndex, -1)
        'End If
        'ReDim InitialValueArrayGB2(0)
        'SaveInitialValuesOfGroupBox2()
        'VerifyProjectFile()
    End Sub

    Private Sub cbo_Models_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_Models.SelectedIndexChanged
        'GetModelStructureFromDatabase(tvwProject.SelectedNode.Text, cbo_Models.SelectedIndex)
        Dim intPosition As Integer
        Dim Response As Integer

        If IsArray(InitialValueArrayGB2) Then
            If UBound(InitialValueArrayGB2) > 0 Then
                If ISDirtyGB2() And strUserName <> "Operator" Then
                    Response = MsgBox("Model Values have Changed. Would you like to save your work?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                    If Response = 6 Then
                        If NumericUpDownTaskInstance.Visible = True Then
                            SetModelConfigurationToDatabase(InitialValueArrayGB2(0), NumericUpDownTaskInstance.Value - 1)
                        Else
                            SetModelConfigurationToDatabase(InitialValueArrayGB2(0), -1)
                        End If
                    End If
                End If
            End If
        End If


        If UpdatePending = 0 Then
            For Each ctrl In GroupBox2.Controls
                If TypeOf ctrl Is TextBox Then
                    ctrl.text = ""
                ElseIf TypeOf ctrl Is ComboBox Then
                    intPosition = InStr(ctrl.Tag, "BASE")
                    If intPosition = 0 Then
                        ctrl.text = ""
                    End If
                ElseIf TypeOf ctrl Is CheckBox Then
                    ctrl.checked = False
                ElseIf TypeOf ctrl Is Button Then
                    'ctrl.AccessibleName = ""
                    ctrl.AccessibleDescription = ""
                End If
            Next
            GetModelConfigurationFromDatabase(cbo_Models.SelectedIndex)
            SaveInitialValuesOfGroupBox2()
        End If

    End Sub

    Private Sub SaveInitialValuesOfGroupBox1()
        Dim i As Integer

        Try
            ReDim InitialValueArrayGB1(0)
            For Each ctrl In GroupBox1.Controls
                If TypeOf ctrl Is TextBox Then
                    ReDim Preserve InitialValueArrayGB1(i)
                    InitialValueArrayGB1(i) = ctrl.Text
                    i += 1
                ElseIf TypeOf ctrl Is ComboBox Then
                    ReDim Preserve InitialValueArrayGB1(i)
                    InitialValueArrayGB1(i) = ctrl.text
                    i += 1
                ElseIf TypeOf ctrl Is CheckBox Then
                    ReDim Preserve InitialValueArrayGB1(i)
                    InitialValueArrayGB1(i) = ctrl.Checked
                    i += 1
                ElseIf TypeOf ctrl Is Button Then
                    ReDim Preserve InitialValueArrayGB1(i)
                    InitialValueArrayGB1(i) = ctrl.AccessibleDescription
                    i += 1
                End If
            Next
        Catch ex As Exception
            Log_Anything("SaveInitialValuesOfGroupBoxes - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub SaveInitialValuesOfGroupBox2()
        Dim i As Integer

        Try
            ReDim InitialValueArrayGB2(0)
            i += 1
            For Each ctrl In GroupBox2.Controls
                InitialValueArrayGB2(0) = cbo_Models.SelectedIndex
                If TypeOf ctrl Is TextBox Then
                    ReDim Preserve InitialValueArrayGB2(i)
                    InitialValueArrayGB2(i) = ctrl.Text
                    i += 1
                ElseIf TypeOf ctrl Is ComboBox And ctrl.Name <> "cbo_Models" Then
                    ReDim Preserve InitialValueArrayGB2(i)
                    InitialValueArrayGB2(i) = ctrl.text
                    i += 1
                ElseIf TypeOf ctrl Is CheckBox Then
                    ReDim Preserve InitialValueArrayGB2(i)
                    InitialValueArrayGB2(i) = ctrl.Checked
                    i += 1
                ElseIf TypeOf ctrl Is Button Then
                    ReDim Preserve InitialValueArrayGB2(i)
                    InitialValueArrayGB2(i) = ctrl.AccessibleDescription
                    i += 1
                End If
            Next
        Catch ex As Exception
            Log_Anything("SaveInitialValuesOfGroupBoxes - " & GetExceptionInfo(ex))
        End Try
    End Sub


    Private Function ISDirtyGB1() As Boolean
        Dim i As Integer

        Try
            For Each ctrl In GroupBox1.Controls
                If TypeOf ctrl Is TextBox Then
                    If InitialValueArrayGB1(i) <> ctrl.Text Then
                        ISDirtyGB1 = True
                        Exit Function
                    End If
                    i += 1
                ElseIf TypeOf ctrl Is ComboBox Then
                    If InitialValueArrayGB1(i) <> ctrl.Text Then
                        ISDirtyGB1 = True
                        Exit Function
                    End If
                    i += 1
                ElseIf TypeOf ctrl Is CheckBox Then
                    If InitialValueArrayGB1(i) <> ctrl.checked Then
                        ISDirtyGB1 = True
                        Exit Function
                    End If
                    i += 1
                ElseIf TypeOf ctrl Is Button Then
                    If InitialValueArrayGB1(i) <> ctrl.AccessibleDescription Then
                        ISDirtyGB1 = True
                        Exit Function
                    End If
                    i += 1
                End If
            Next
            ISDirtyGB1 = False
        Catch ex As Exception
            Log_Anything("ISDirtyGB1 - " & GetExceptionInfo(ex))
            ISDirtyGB1 = True
        End Try
    End Function

    Private Function ISDirtyGB2() As Boolean
        Dim i As Integer

        Try
            i += 1
            For Each ctrl In GroupBox2.Controls
                If TypeOf ctrl Is TextBox Then
                    If InitialValueArrayGB2(i) <> ctrl.Text Then
                        ISDirtyGB2 = True
                        Exit Function
                    End If
                    i += 1
                ElseIf TypeOf ctrl Is ComboBox And ctrl.Name <> "cbo_Models" Then
                    If InitialValueArrayGB2(i) <> ctrl.Text Then
                        ISDirtyGB2 = True
                        Exit Function
                    End If
                    i += 1
                ElseIf TypeOf ctrl Is CheckBox Then
                    If InitialValueArrayGB2(i) <> ctrl.checked Then
                        ISDirtyGB2 = True
                        Exit Function
                    End If
                    i += 1
                ElseIf TypeOf ctrl Is Button Then
                    If InitialValueArrayGB2(i) <> ctrl.AccessibleDescription Then
                        ISDirtyGB2 = True
                        Exit Function
                    End If
                    i += 1
                End If
            Next
            ISDirtyGB2 = False
        Catch ex As Exception
            Log_Anything("ISDirtyGB2 - " & GetExceptionInfo(ex))
            ISDirtyGB2 = True
        End Try
    End Function

    Private Sub VerifyProjectToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles VerifyProjectToolStripMenuItem.Click
        VerifyProjectFile(tvwProject.SelectedNode.FullPath)
    End Sub

    ''' <summary>
    ''' Verify Project File -- According to selected tree path
    ''' Such As:
    ''' PowertrainPlan\Area
    ''' PowertrainPlan\Area\Section\Station\Plc
    ''' PowertrainPlan\Area\Section\Station\MasterFile\Task\TaskInstance
    ''' </summary>
    ''' <param name="strTreePath"></param>
    ''' <remarks></remarks>
    Private Sub VerifyProjectFile(Optional ByVal strTreePath As String = "")
        Dim treePathArray = Split(strTreePath, "\")
        Try
            Dim intNumberOfNodes = tvwProject.GetNodeCount(True)
            ShowProgressPopUp(intNumberOfNodes, UiDisplayConstant.VerifyProjectFile)

            If treePathArray.Length < 3 Then
                VerifyProjectFileFromTreeTopNode()
            Else
                VerifyProjectFileAccordingToSelectedTreePath(treePathArray)
            End If
            CloseProgressPopUp()
            Exit Sub
        Catch ex As Exception
            Log_Anything("VerifyProjectFile - " & GetExceptionInfo(ex))
            CloseProgressPopUp()
        End Try

    End Sub

    ''' <summary>
    ''' Verify project file according to selected tree path
    ''' </summary>
    ''' <param name="treePathArray"></param>
    ''' <remarks></remarks>
    Private Sub VerifyProjectFileAccordingToSelectedTreePath(treePathArray As String())
        Dim treeNodeHier = New TreeNodeHierarchy(treePathArray(1), treePathArray(2), treePathArray(3))
        Dim selectedTreeNode = tvwProject.SelectedNode

        If selectedTreeNode.Tag.Equals(TreeNodeTagConstant.STATION) Then
            VerifyProjectFileAccordingToSelectedStationNode(treeNodeHier, selectedTreeNode)
        ElseIf selectedTreeNode.Tag.Equals(TreeNodeTagConstant.TASK) Then
            VerifyProjectFileAccordingToSelectedTaskNode(treePathArray, selectedTreeNode)
        ElseIf selectedTreeNode.Tag.ToString().Contains(TreeNodeTagConstant.TASKInstancePrefix) Then
            VerifyProjectFileAccordingToSelectedTaskInstanceNode(treePathArray, selectedTreeNode)
        End If
    End Sub

    Private Sub VerifyProjectFileAccordingToSelectedStationNode(treeNodeHier As TreeNodeHierarchy, selectedNode As TreeNode)
        If String.IsNullOrWhiteSpace(treeNodeHier.StationName) Then
            Log_Anything("VerifyProjectFile - I Dont know how we got here. We called Verfiy Project but the Treeview Path does not contain Station information." & selectedNode.Text)
            Exit Sub
        End If

        Dim stationConfigList = stationConfigController.GetStationConfigurationBySection(treeNodeHier.AreaName, treeNodeHier.SectionName)
        Dim stationValResult = stationConfigController.VerifyStationConfiguration(stationConfigList, treeNodeHier.StationName)
        Dim intStationAccept = stationsController.GetAccept(treeNodeHier.StationName, treeNodeHier.SectionName, treeNodeHier.AreaName)

        If stationValResult = StationConfigValidateResult.Valid Then
            selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
            selectedNode.ImageIndex = TreeNodeImageIndexConstant.ValidNode
        Else
            If intStationAccept = 0 Then
                If stationValResult = StationConfigValidateResult.Invalid Then
                    selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
                    selectedNode.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
                ElseIf stationValResult = StationConfigValidateResult.NotFound Then
                    selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                    selectedNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                End If
            ElseIf intStationAccept = 1 Then
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.ManualAcceptNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.ManualAcceptNode
            End If
        End If
    End Sub

    Private Sub VerifyProjectFileAccordingToSelectedTaskNode(treePathArray As String(), selectedNode As TreeNode)

        If treePathArray.Length < 6 Then
            Log_Anything("VerifyProjectFile - I Dont know how we got here. We called Verfiy Project but the Treeview Path does not contain Task information." & selectedNode.Text)
            Exit Sub
        End If

        Dim treeNodeHier = New TreeNodeHierarchy(treePathArray(1), treePathArray(2), treePathArray(3))
        Dim taskConfigList = taskConfigController.GetTaskConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)
        Dim taskValResult = TaskConfigValidateResult.Valid

        If treePathArray.Length = 6 Then
            taskValResult = taskConfigController.VerifyTaskConfiguration(taskConfigList, selectedNode.Name)
        End If

        Select Case taskValResult
            Case TaskConfigValidateResult.Valid
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.ValidNode
            Case TaskConfigValidateResult.Invalid
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
            Case TaskConfigValidateResult.NotFound
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundNode
            Case TaskConfigValidateResult.NotFoundValidationField
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundValidationFieldNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundValidationFieldNode
        End Select
    End Sub

    Private Sub VerifyProjectFileAccordingToSelectedTaskInstanceNode(treePathArray As String(), selectedNode As TreeNode)

        If treePathArray.Length < 7 Then
            Log_Anything("VerifyProjectFile - I Dont know how we got here. We called Verfiy Project but the Treeview Path does not contain Task information." & selectedNode.Text)
            Exit Sub
        End If

        Dim treeNodeHier = New TreeNodeHierarchy(treePathArray(1), treePathArray(2), treePathArray(3))
        Dim taskConfigList = taskConfigController.GetTaskConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)
        Dim taskInstanceValResult = TaskConfigValidateResult.Valid

        Dim taskTagArray = Split(selectedNode.Tag, "|")
        If taskTagArray.Length = 2 Then
            taskInstanceValResult = taskConfigController.VerifyTaskConfiguration(taskConfigList, selectedNode.Name, taskTagArray(1) - 1)
        End If

        Select Case taskInstanceValResult
            Case TaskConfigValidateResult.Valid
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.ValidNode
                selectedNode.Parent.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
                selectedNode.Parent.ImageIndex = TreeNodeImageIndexConstant.ValidNode
            Case TaskConfigValidateResult.Invalid
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
                selectedNode.Parent.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
                selectedNode.Parent.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
            Case TaskConfigValidateResult.NotFound
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                selectedNode.Parent.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                selectedNode.Parent.ImageIndex = TreeNodeImageIndexConstant.NotFoundNode
            Case TaskConfigValidateResult.NotFoundValidationField
                selectedNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundValidationFieldNode
                selectedNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundValidationFieldNode
                'why???
                selectedNode.Parent.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
                selectedNode.Parent.ImageIndex = TreeNodeImageIndexConstant.ValidNode
        End Select
    End Sub


    ''' <summary>
    ''' Verify project from the tree top node
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub VerifyProjectFileFromTreeTopNode()
        For Each MainNode In tvwProject.Nodes
            UpdateProgressPopUp()
            For Each AreaNode In MainNode.Nodes
                UpdateProgressPopUp()
                VerifyProjectFileAreaNode(AreaNode)
            Next
        Next
    End Sub

    Private Sub VerifyProjectFileAreaNode(ByRef areaNode As TreeNode)
        Dim valResult = areaConfigController.ValidateAreaConfiguration(areaNode.Text)
        If valResult = False Then
            areaNode.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
            areaNode.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
        Else
            areaNode.SelectedImageIndex = TreeNodeImageIndexConstant.Area
            areaNode.ImageIndex = TreeNodeImageIndexConstant.Area
        End If

        For Each sectionNode In areaNode.Nodes
            UpdateProgressPopUp()
            Dim treeNodeHier = New TreeNodeHierarchy(areaNode.Text, CType(sectionNode, TreeNode).Text)
            VerifyProjectFileSectionNode(treeNodeHier, sectionNode)
        Next
    End Sub

    Private Sub VerifyProjectFileSectionNode(treeNodeHier As TreeNodeHierarchy, ByRef sectionNode As TreeNode)
        Dim stationConfigList = stationConfigController.GetStationConfigurationBySection(treeNodeHier.AreaName, treeNodeHier.SectionName)
        For Each stationNode In sectionNode.Nodes
            UpdateProgressPopUp()
            treeNodeHier.StationName = CType(stationNode, TreeNode).Text
            VerifyProjectFileStationNode(treeNodeHier, stationNode, stationConfigList)
        Next
    End Sub

    Private Sub VerifyProjectFileStationNode(treeNodeHier As TreeNodeHierarchy, ByRef stationNode As TreeNode, stationConfigList As List(Of StationConfiguration))
        Dim stationValResult = stationConfigController.VerifyStationConfiguration(stationConfigList, stationNode.Text)
        Dim intStationAccept = stationsController.GetAccept(treeNodeHier.StationName, treeNodeHier.SectionName, treeNodeHier.AreaName)

        If stationValResult = StationConfigValidateResult.Valid Then
            stationNode.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
            stationNode.ImageIndex = TreeNodeImageIndexConstant.ValidNode
        Else
            If intStationAccept = 0 Then
                If stationValResult = StationConfigValidateResult.Invalid Then
                    stationNode.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
                    stationNode.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
                ElseIf stationValResult = StationConfigValidateResult.NotFound Then
                    stationNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                    stationNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                End If
            ElseIf intStationAccept = 1 Then
                stationNode.SelectedImageIndex = TreeNodeImageIndexConstant.ManualAcceptNode
                stationNode.ImageIndex = TreeNodeImageIndexConstant.ManualAcceptNode
            End If
        End If

        For Each masterFileNode In stationNode.Nodes
            UpdateProgressPopUp()
            If CType(masterFileNode, TreeNode).Tag.Equals(TreeNodeTagConstant.MASTERFILE) Then
                VerifyProjectFileMasterFileNode(treeNodeHier, masterFileNode)
            End If
        Next
    End Sub

    Private Sub VerifyProjectFileMasterFileNode(treeNodeHier As TreeNodeHierarchy, ByRef masterFileNode As TreeNode)
        For Each taskNode In masterFileNode.Nodes
            UpdateProgressPopUp()
            Dim taskConfigList = taskConfigController.GetTaskConfigurationByStation(treeNodeHier.AreaName, treeNodeHier.SectionName, treeNodeHier.StationName)
            VerifyProjectFileTaskNode(treeNodeHier, taskNode, taskConfigList)
        Next
    End Sub

    Private Sub VerifyProjectFileTaskNode(treeNodeHier As TreeNodeHierarchy, ByRef taskNode As TreeNode, taskConfigList As List(Of TaskConfiguration))
        If taskNode.Nodes.Count = 0 Then
            Dim taskValResult = taskConfigController.VerifyTaskConfiguration(taskConfigList, taskNode.Name)
            Dim intStationAccept = stationsController.GetAccept(treeNodeHier.StationName, treeNodeHier.SectionName, treeNodeHier.AreaName)

            If taskValResult = TaskConfigValidateResult.Valid Then
                taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
                taskNode.ImageIndex = TreeNodeImageIndexConstant.ValidNode
            Else
                If intStationAccept = 0 Then
                    If taskValResult = TaskConfigValidateResult.Invalid Then
                        taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
                        taskNode.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
                    ElseIf taskValResult = TaskConfigValidateResult.NotFound Then
                        taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                        taskNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                    ElseIf taskValResult = TaskConfigValidateResult.NotFoundValidationField Then
                        taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundValidationFieldNode
                        taskNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundValidationFieldNode
                    End If
                ElseIf intStationAccept = 1 Then
                    taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.ManualAcceptNode
                    taskNode.ImageIndex = TreeNodeImageIndexConstant.ManualAcceptNode
                End If
            End If
        Else
            Dim i = 0
            For Each taskInstanceNode In taskNode.Nodes
                UpdateProgressPopUp()
                VerifyProjectFileTaskInstanceNode(treeNodeHier, taskNode, taskInstanceNode, taskConfigList, i)
                i += 1
            Next
        End If
    End Sub

    Private Sub VerifyProjectFileTaskInstanceNode(treeNodeHier As TreeNodeHierarchy, ByRef taskNode As TreeNode, ByRef taskInstanceNode As TreeNode, taskConfigList As List(Of TaskConfiguration), taskInstance As Integer)
        Dim taskValResult = taskConfigController.VerifyTaskConfiguration(taskConfigList, taskInstanceNode.Name, taskInstance)
        Dim intStationAccept = stationsController.GetAccept(treeNodeHier.StationName, treeNodeHier.SectionName, treeNodeHier.AreaName)

        If taskValResult = TaskConfigValidateResult.Valid Then
            taskInstanceNode.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
            taskInstanceNode.ImageIndex = TreeNodeImageIndexConstant.ValidNode
            taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
            taskNode.ImageIndex = TreeNodeImageIndexConstant.ValidNode
        Else
            If intStationAccept = 0 Then
                If taskValResult = TaskConfigValidateResult.Invalid Then
                    taskInstanceNode.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
                    taskInstanceNode.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
                    taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.InvalidNode
                    taskNode.ImageIndex = TreeNodeImageIndexConstant.InvalidNode
                ElseIf taskValResult = TaskConfigValidateResult.NotFound Then
                    taskInstanceNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                    taskInstanceNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                    taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                    taskNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundNode
                ElseIf taskValResult = TaskConfigValidateResult.NotFoundValidationField Then
                    taskInstanceNode.SelectedImageIndex = TreeNodeImageIndexConstant.NotFoundValidationFieldNode
                    taskInstanceNode.ImageIndex = TreeNodeImageIndexConstant.NotFoundValidationFieldNode
                    'why???
                    taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.ValidNode
                    taskNode.ImageIndex = TreeNodeImageIndexConstant.ValidNode
                End If
            ElseIf intStationAccept = 1 Then
                taskNode.SelectedImageIndex = TreeNodeImageIndexConstant.ManualAcceptNode
                taskNode.ImageIndex = TreeNodeImageIndexConstant.ManualAcceptNode
            End If
        End If
    End Sub

    Private Sub GetGraphicData(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String)
        Dim Table_ As String = "Tasks_Configuration"
        Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
        Dim ds As New DataSet
        Dim ds2 As New DataSet
        Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
        Dim query As String
        Dim query2 As String
        Dim intForwardPosition As Integer = 0
        Dim intReversePosition As Integer = 0
        Dim strCheckString As String = ""
        Dim intStartSequencePos As Integer
        Dim intStopSequencePos As Integer
        Dim intEndIndexPos As Integer
        Dim strStartSequence As String = 0
        Dim strStopSequence As String = 0
        Dim strTaskName_User As String = ""
        Dim strMultipleStart As String = ""
        Dim strMultipleStop As String = ""
        Dim bMultipleStartStop As Boolean
        Dim strIndexStartValues() As String
        Dim strIndexStopValues() As String
        Dim strSingleStartValue() As String
        Dim strSingleStopValue() As String



        'Open Connection to Database
        Try
            cnn.Open()
            query = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND (Member_Name Like '%.Number' OR Member_Name Like '%.Task_Number' OR Member_Name Like '%.TaskNumber') ORDER BY Val(Member_Value)"

            Dim cmd As New OleDbCommand(query, cnn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)
            Dim t1 As DataTable = ds.Tables(Table_)
            Dim row As DataRow
            Dim i As Integer


            ReDim strTaskArray(0)
            ReDim intTaskStartArray(0)
            ReDim intTaskStopArray(0)

            Dim cmd2 As New OleDbCommand
            Dim da2 As New OleDbDataAdapter
            Dim t2 As DataTable
            Dim row2 As DataRow

            For Each row In t1.Rows
                intForwardPosition = InStr(row(6).ToString, ".")
                intReversePosition = InStrRev(row(6).ToString, ".")
                If intForwardPosition = intReversePosition Then
                    ReDim Preserve strTaskArray(i)
                    ReDim Preserve intTaskStartArray(i)
                    ReDim Preserve intTaskStopArray(i)
                    query2 = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & row(4).ToString & "' AND Task_Instance = " & row(5).ToString & " AND (Member_Name Like '%.Start_Sequence%' OR Member_Name Like '%.Stop_Sequence%') AND Member_Value <> '0' ORDER BY Val(Member_Value)"
                    cmd2 = New OleDbCommand(query2, cnn)
                    da2 = New OleDbDataAdapter(cmd2)
                    da2.Fill(ds2, Table_)
                    t2 = ds2.Tables(Table_)

                    strStartSequence = -1
                    strStopSequence = -1
                    strMultipleStart = ""
                    strMultipleStop = ""
                    bMultipleStartStop = False
                    For Each row2 In t2.Rows

                        intStartSequencePos = InStr(row2(6).ToString, "Start_Sequence[")
                        intStopSequencePos = InStr(row2(6).ToString, "Stop_Sequence[")
                        intEndIndexPos = InStrRev(row2(6).ToString, "]")
                        If intStartSequencePos > 0 And intEndIndexPos > 0 Then
                            bMultipleStartStop = True
                            If row2(7).ToString <> "0" Then
                                If strMultipleStart = "" Then
                                    strMultipleStart = Mid(row2(6).ToString, intStartSequencePos + 15, intEndIndexPos - (intStartSequencePos + 15)) & "," & row2(7).ToString
                                Else
                                    strMultipleStart = strMultipleStart & "|" & Mid(row2(6).ToString, intStartSequencePos + 15, intEndIndexPos - (intStartSequencePos + 15)) & "," & row2(7).ToString
                                End If
                            End If
                        End If

                        If intStopSequencePos > 0 And intEndIndexPos > 0 Then
                            bMultipleStartStop = True
                            If row2(7).ToString <> "0" Then
                                If strMultipleStop = "" Then
                                    strMultipleStop = Mid(row2(6).ToString, intStopSequencePos + 14, intEndIndexPos - (intStopSequencePos + 14)) & "," & row2(7).ToString
                                Else
                                    strMultipleStop = strMultipleStop & "|" & Mid(row2(6).ToString, intStopSequencePos + 14, intEndIndexPos - (intStopSequencePos + 14)) & "," & row2(7).ToString
                                End If
                            End If
                        End If

                        If bMultipleStartStop = False Then
                            intStartSequencePos = InStr(row2(6).ToString, "Start_Sequence")
                            intStopSequencePos = InStr(row2(6).ToString, "Stop_Sequence")
                            If intStartSequencePos > 0 Then
                                strStartSequence = row2(7).ToString
                            End If
                            If intStopSequencePos > 0 Then
                                strStopSequence = row2(7).ToString
                            End If
                        End If

                    Next

                    t2.Clear()
                    da2.Dispose()
                    cmd2.Dispose()

                    query2 = "SELECT * FROM " & Table_ & " WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & row(4).ToString & "' AND Task_Instance = " & row(5).ToString & " AND Member_Name = 'TaskName_User'"

                    cmd2 = New OleDbCommand(query2, cnn)
                    da2 = New OleDbDataAdapter(cmd2)
                    da2.Fill(ds2, Table_)
                    t2 = ds2.Tables(Table_)

                    For Each row2 In t2.Rows
                        strTaskName_User = row2(7).ToString
                    Next



                    If bMultipleStartStop = False Then
                        If IsNumeric(strStartSequence) Then
                            intTaskStartArray(i) = strStartSequence
                        Else
                            intTaskStartArray(i) = -2
                            strStartSequence = -2
                        End If

                        If IsNumeric(strStopSequence) Then
                            intTaskStopArray(i) = strStopSequence
                        Else
                            intTaskStopArray(i) = -2
                            strStopSequence = -2
                        End If
                        strTaskArray(i) = row(4).ToString & "," & row(5).ToString & "," & strStartSequence & "," & strStopSequence & "," & strTaskName_User
                    Else
                        strIndexStartValues = Split(strMultipleStart, "|")
                        strIndexStopValues = Split(strMultipleStop, "|")
                        For k = 0 To UBound(strIndexStartValues)
                            If k > 0 Then
                                i += 1
                                ReDim Preserve strTaskArray(i)
                                ReDim Preserve intTaskStartArray(i)
                                ReDim Preserve intTaskStopArray(i)
                            End If
                            strSingleStartValue = Split(strIndexStartValues(k), ",")
                            strSingleStopValue = Split(strIndexStopValues(k), ",")

                            If IsNumeric(strSingleStartValue(1)) Then
                                intTaskStartArray(i) = strSingleStartValue(1)
                            Else
                                intTaskStartArray(i) = -2
                                strSingleStartValue(1) = -2
                            End If

                            If IsNumeric(strSingleStopValue(1)) Then
                                intTaskStopArray(i) = strSingleStopValue(1)
                            Else
                                intTaskStopArray(i) = -2
                                strSingleStopValue(1) = -2
                            End If

                            strTaskArray(i) = row(4).ToString & "," & row(5).ToString & "|" & strSingleStartValue(0) & "," & strSingleStartValue(1) & "," & strSingleStopValue(1) & "," & strTaskName_User
                        Next
                    End If

                    t2.Clear()
                    da2.Dispose()
                    cmd2.Dispose()

                    i += 1
                End If
            Next
            Array.Sort(intTaskStartArray, strTaskArray)
            Array.Sort(intTaskStopArray)
            cnn.Close()
        Catch ex As Exception
            Log_Anything("GetGraphicData - " & GetExceptionInfo(ex))
        End Try

    End Sub


    Private Sub FillSequenceGraphic()
        Dim intLastStartColumn As Integer
        Dim intLastStopColumn As Integer
        Dim strSingleTaskInfoArray() As String
        Dim i, j As Integer
        Dim strMultipleInstances() As String
        Dim intMultipleInstancesPosition As String

        Try

            dgvSequenceGraphic.Rows.Clear()
            dgvSequenceGraphic.Columns.Clear()

            intLastStartColumn = intTaskStartArray(UBound(intTaskStartArray))
            intLastStopColumn = intTaskStopArray(UBound(intTaskStopArray))

            If intLastStartColumn > intLastStopColumn Then
                dgvSequenceGraphic.ColumnCount = intLastStartColumn + 1
            Else
                dgvSequenceGraphic.ColumnCount = intLastStopColumn + 1
            End If

            dgvSequenceGraphic.Columns(0).HeaderText = "Task Name"
            dgvSequenceGraphic.Columns(0).Width = 400

            For i = 1 To dgvSequenceGraphic.ColumnCount - 1
                dgvSequenceGraphic.Columns(i).HeaderText = i
                dgvSequenceGraphic.Columns(i).Width = 35
            Next

            For i = 0 To UBound(strTaskArray)
                If intTaskStartArray(i) > -1 And intTaskStopArray(i) > -1 Then
                    strSingleTaskInfoArray = Split(strTaskArray(i), ",")
                    dgvSequenceGraphic.Rows.Add()
                    intMultipleInstancesPosition = InStr(strSingleTaskInfoArray(1), "|")
                    If intMultipleInstancesPosition > 0 Then
                        strMultipleInstances = Split(strSingleTaskInfoArray(1), "|")
                        If strMultipleInstances(0) > -1 Then
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(0).Value = strSingleTaskInfoArray(0) & "-[" & strMultipleInstances(0) + 1 & "][" & strMultipleInstances(1) + 1 & "]"
                        Else
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(0).Value = strSingleTaskInfoArray(0)
                        End If
                    Else
                        If strSingleTaskInfoArray(1) > -1 Then
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(0).Value = strSingleTaskInfoArray(0) & "-[" & strSingleTaskInfoArray(1) + 1 & "]"
                        Else
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(0).Value = strSingleTaskInfoArray(0)
                        End If
                    End If


                    For j = strSingleTaskInfoArray(2) To strSingleTaskInfoArray(3)
                        dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(j).Style.BackColor = Color.YellowGreen
                    Next
                End If
            Next

            dgvSequenceGraphic.Rows.Add()

            For i = 0 To UBound(strTaskArray)
                If intTaskStartArray(i) < 0 Or intTaskStopArray(i) < 0 Then
                    strSingleTaskInfoArray = Split(strTaskArray(i), ",")
                    dgvSequenceGraphic.Rows.Add()
                    intMultipleInstancesPosition = InStr(strSingleTaskInfoArray(1), "|")
                    If intMultipleInstancesPosition > 0 Then
                        strMultipleInstances = Split(strSingleTaskInfoArray(1), "|")
                        If strMultipleInstances(0) > -1 Then
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(0).Value = strSingleTaskInfoArray(0) & "-[" & strMultipleInstances(0) + 1 & "][" & strMultipleInstances(1) + 1 & "]"
                        Else
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(0).Value = strSingleTaskInfoArray(0)
                        End If
                    Else
                        If strSingleTaskInfoArray(1) > -1 Then
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(0).Value = strSingleTaskInfoArray(0) & "-[" & strSingleTaskInfoArray(1) + 1 & "]"
                        Else
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(0).Value = strSingleTaskInfoArray(0)
                        End If
                    End If


                    For j = strSingleTaskInfoArray(2) To strSingleTaskInfoArray(3)
                        If j >= 0 Then
                            dgvSequenceGraphic.Rows(dgvSequenceGraphic.Rows.Count - 1).Cells(j).Style.BackColor = Color.Red
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            Log_Anything("FillSequenceGraphic - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub ShowProgressPopUp(progressMaxNumber As Integer)
        Form7.Show()
        Form7.Top = (Me.Height / 2) - (Form7.Height * 2)
        Form7.Left = (Me.Width / 2) - (Form7.Width / 2)
        Form7.tmrProgress.Enabled = False
        Form7.ProgressBar1.Maximum = progressMaxNumber
    End Sub

    Private Sub ShowProgressPopUp(progressMaxNumber As Integer, msg As String)
        Form7.ProgressBar1.Maximum = progressMaxNumber
        Form7.ProgressBar1.Minimum = 0
        Form7.tmrProgress.Enabled = False
        Form7.lblInformation.Text = msg
        Form7.Show()
    End Sub

    Private Sub UpdateProgressPopUp(Optional ByVal addedCount As Integer = 1)
        Form7.ProgressBar1.Value += addedCount
        Form7.Refresh()
    End Sub

    Private Sub CloseProgressPopUp()
        Form7.Close()
    End Sub

    Private Sub DownloadModelInformationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DownloadModelInformationToolStripMenuItem.Click
        Try
            Dim selectedNodeHierarchy = GetSelectedTreeNodeHierarchy()

            Dim downloadCount = areaConfigController.GetDownloadAreaConfigurationCount(selectedNodeHierarchy)
            downloadCount += modelConfigController.GetDownloadModelConfigurationCount(selectedNodeHierarchy)
            ShowProgressPopUp(downloadCount)

            'Performance Analysis'
            PerformanceAnalysisController.SharedStartPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadModel)
            Dim fileObj = plcConfigController.GetPythonFileObj(selectedNodeHierarchy)
            areaConfigController.DownloadAreaConfiguration(fileObj, selectedNodeHierarchy)
            modelConfigController.DownloadModelConfiguration(fileObj, selectedNodeHierarchy)
            'Performance Analysis'
            PerformanceAnalysisController.SharedEndPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadModel)

            'to-do: show message according to which one is failed. download model id or model configuratin

            CloseProgressPopUp()
            MsgBox("All Data Downloaded Successfully.", MsgBoxStyle.Information, Application.ProductName)
        Catch ex As Exception
            CloseProgressPopUp()
            MsgBox("Model Data Failed to Download." & ex.Message, MsgBoxStyle.Critical, Application.ProductName)
            Log_Anything("DownloadModelInformationToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub SequenceMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SequenceMenuItem.Click

        If tvwProject.SelectedNode.Tag <> "STATION" Then
            MsgBox("Please Select a Station from the Project Tree before generating the Sequence Graphic", vbExclamation, Application.ProductName)
            Exit Sub
        End If

        ShowSequenceGraphic()
    End Sub

    Private Sub tvwProject_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles tvwProject.MouseClick
        Try
            With contextMenuStrip1
                .Items(0).Visible = False
                .Items(1).Visible = False
                .Items(2).Visible = False
                .Items(3).Visible = False
                .Items(4).Visible = False
            End With


            If e.Button = Windows.Forms.MouseButtons.Right And strUserName <> "Operator" Then
                With contextMenuStrip1

                    tvwProject.SelectedNode = tvwProject.GetNodeAt(e.X, e.Y)

                    .Items(0).Visible = False
                    .Items(1).Visible = False
                    .Items(2).Visible = False
                    .Items(3).Visible = False
                    .Items(4).Visible = False

                    If tvwProject.SelectedNode.Tag = "PLANT" Then
                        .Items(0).Text = "Verify Project"
                        '.Items(1).Text = "Add Area"
                        .Items(0).Visible = True
                        '.Items(1).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "AREA" Then
                        .Items(0).Text = "Verify Project"
                        '.Items(1).Text = "Add Section"
                        '.Items(2).Text = "Remove Area"
                        .Items(0).Visible = True
                        '.Items(1).Visible = True
                        '.Items(2).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "SECTION" Then
                        .Items(0).Text = "Verify Project"
                        '.Items(1).Text = "Add Station"
                        '.Items(2).Text = "Remove Section"
                        .Items(0).Visible = True
                        '.Items(1).Visible = True
                        '.Items(2).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "STATION" Then
                        .Items(0).Text = "Verify Project"
                        .Items(1).Text = "Show Sequence Graphic"
                        .Items(2).Text = "Delete Station Configuration"
                        .Items(3).Text = Form2Constant.OrderChange
                        .Items(4).Text = Form2Constant.AcceptStation
                        .Items(0).Visible = True
                        .Items(1).Visible = True
                        .Items(2).Visible = True
                        .Items(3).Visible = True
                        .Items(4).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "MASTERFILE" Then
                        '.Items(0).Text = "Verify Project"
                        '.Items(0).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "TASK" Then
                        .Items(0).Text = Form2Constant.OrderChange
                        .Items(0).Visible = True
                        Dim intchildnode As Integer
                        intchildnode = (tvwProject.SelectedNode.GetNodeCount(True))
                        If intchildnode = 0 Then
                            .Items(1).Visible = True
                            .Items(1).Text = "Delete Task Configuration"
                        End If
                    ElseIf InStr(tvwProject.SelectedNode.Tag, "TASK|") Then
                        '.Items(0).Text = "Verify Project"
                        .Items(0).Visible = True
                        .Items(0).Text = "Delete Task Configuration"
                    ElseIf tvwProject.SelectedNode.Tag = "PLC" Then
                        '.Items(0).Text = "Verify Project"
                        '.Items(0).Visible = True
                    Else
                        'sender.handled = True
                        Exit Sub
                    End If

                    .Show(New Point(Cursor.Position.X, Cursor.Position.Y))
                End With

            End If
        Catch ex As Exception
            Log_Anything("tvwProject_MouseClick - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub mnuPos1_Click(sender As System.Object, e As System.EventArgs) Handles mnuPos1.Click
        Try
            If mnuPos1.Text = "Verify Project" Then
                VerifyProjectFile(tvwProject.SelectedNode.FullPath)
                Exit Sub
            ElseIf mnuPos1.Text = Form2Constant.OrderChange Then
                OrderChange.From8Text = tvwProject.SelectedNode.Text
                OrderChange.From2SelectNodeTag = tvwProject.SelectedNode
                OrderChange.Owner = Me
                OrderChange.Show()
            End If
        Catch ex As Exception
            Log_Anything("mnuPos1_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub mnuPos2_Click(sender As System.Object, e As System.EventArgs) Handles mnuPos2.Click
        Try
            If mnuPos2.Text = "Show Sequence Graphic" Then
                ShowSequenceGraphic()
            ElseIf mnuPos2.Text = "Delete Task Configuration" Then
                If NumericUpDownTaskInstance.Visible = True Then
                    DeleteTaskConfiguration(NumericUpDownTaskInstance.Value - 1)
                Else
                    DeleteTaskConfiguration(-1)
                End If
            End If
        Catch ex As Exception
            Log_Anything("mnuPos2_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub mnuPos3_Click(sender As System.Object, e As System.EventArgs) Handles mnuPos3.Click
        If mnuPos3.Text = "Delete Task Configuration" Then
            DeleteStationConfiguration()
        End If
    End Sub

    Private Sub MenuPosition4ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuPosition4ToolStripMenuItem.Click
        If MenuPosition4ToolStripMenuItem.Text = Form2Constant.OrderChange Then
            OrderChange.From8Text = tvwProject.SelectedNode.Tag
            OrderChange.From2SelectNodeTag = tvwProject.SelectedNode
            OrderChange.Owner = Me
            OrderChange.Show()
        End If
    End Sub

    Private Sub MenuPosition5ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuPosition5ToolStripMenuItem.Click
        If MenuPosition5ToolStripMenuItem.Text = Form2Constant.AcceptStation Then
            Dim stationName = tvwProject.SelectedNode.Text
            Dim sectionName = tvwProject.SelectedNode.Parent.Text
            Dim areaName = tvwProject.SelectedNode.Parent.Parent.Text
            stationsController.SetStationAccept(stationName, sectionName, areaName)
            VerifyProjectFile()
        End If
    End Sub

    Private Sub DeleteStationConfiguration()
        Try
            Dim Table_ As String = "Stations_Configuration"
            Dim query As String = "SELECT * FROM " & Table_
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim cmd As New OleDbCommand
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String


            Dim strValues As String = ""
            Dim nResult As Integer


            nResult = MsgBox("Are you sure you want to DELETE the Station Configuration from the Database?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Application.ProductName)
            If nResult = 6 Then


                strStationName = tvwProject.SelectedNode.Text
                strSectionName = tvwProject.SelectedNode.Parent.Text
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Text

                cnn.Open()

                query = "DELETE * FROM Stations_Configuration WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "'"
                cmd = New OleDbCommand(query, cnn)
                cmd.ExecuteNonQuery()
                cnn.Close()

                ReDim InitialValueArrayGB1(0)
                ReDim InitialValueArrayGB2(0)

                UpdatePending = 1

                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()

                GetStationStructureFromDatabase(strStationName)
                GetStationConfigurationFromDatabase()
                GetModelStructureFromDatabase(strStationName, -1)
                GetModelConfigurationFromDatabase(0)
                bShowIndexForm = True
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()

                bShowIndexForm = True
                UpdatePending = 0
            Else

            End If

        Catch ex As Exception
            Log_Anything("DeleteStationConfiguration - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub DeleteTaskConfiguration(ByVal intTaskInstance As Integer)
        Try
            Dim Table_ As String = "Tasks_Configuration"
            Dim query As String = "SELECT * FROM " & Table_
            Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim cmd As New OleDbCommand
            Dim ds As New DataSet
            Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
            Dim strAreaName As String
            Dim strSectionName As String
            Dim strStationName As String
            Dim strTaskName As String

            Dim strValues As String = ""
            Dim nResult As Integer


            nResult = MsgBox("Are you sure you want to DELETE the Task Configuration from the Database?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Application.ProductName)
            If nResult = 6 Then


                If tvwProject.SelectedNode.Parent.Tag <> "TASK" Then
                    strTaskName = tvwProject.SelectedNode.Text
                    strStationName = tvwProject.SelectedNode.Parent.Parent.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                Else
                    strTaskName = tvwProject.SelectedNode.Name
                    strStationName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
                    strSectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text
                    strAreaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Parent.Text
                End If

                cnn.Open()

                query = "DELETE * FROM Tasks_Configuration WHERE Area_Name = '" & strAreaName & "' AND Section_Name = '" & strSectionName & "' AND Station_Name = '" & strStationName & "' AND Task_Name = '" & strTaskName & "' AND TASK_Instance = " & intTaskInstance
                cmd = New OleDbCommand(query, cnn)
                cmd.ExecuteNonQuery()
                cnn.Close()

                ReDim InitialValueArrayGB1(0)
                ReDim InitialValueArrayGB2(0)

                UpdatePending = 1

                DeleteConfigurationItemsFromGroupBox()
                DeleteConfigurationItemsFromModelGroupBox()

                GetTaskStructureFromDatabase(tvwProject.SelectedNode.Name, intTaskInstance, True)
                GetTaskConfigurationFromDatabase(intTaskInstance, True)
                GetModelStructureFromDatabase(tvwProject.SelectedNode.Name, intTaskInstance)
                GetModelConfigurationFromDatabase(intTaskInstance)
                SaveInitialValuesOfGroupBox1()
                SaveInitialValuesOfGroupBox2()
                bShowIndexForm = True
                UpdatePending = 0
            Else

            End If

        Catch ex As Exception
            Log_Anything("DeleteTaskConfiguration - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub ShowSequenceGraphic()
        Dim strAreaName As String
        Dim strSectionName As String
        Dim strStationName As String

        If tvwProject.SelectedNode.Tag <> "STATION" Then
            MsgBox("Please Select a STATION from the Project Tree before generating the Sequence Graphic", vbExclamation, Application.ProductName)
            Exit Sub
        End If

        strStationName = tvwProject.SelectedNode.Text
        strSectionName = tvwProject.SelectedNode.Parent.Text
        strAreaName = tvwProject.SelectedNode.Parent.Parent.Text

        GroupBox1.Text = "Sequence Chart"
        SplitContainer2.Panel2Collapsed = True
        'GroupBox2.Text = "Sequence Control"

        GetGraphicData(strAreaName, strSectionName, strStationName)
        FillSequenceGraphic()
        dgvSequenceGraphic.Visible = True
    End Sub

    Private Sub DownloadAllConfigurationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DownloadAllConfigurationToolStripMenuItem.Click
        Try
            Dim selectedNodeHierarchy = GetSelectedTreeNodeHierarchy()

            'Performance Analysis'
            PerformanceAnalysisController.SharedStartPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadAll)

            Dim downloadCount = areaConfigController.GetDownloadAreaConfigurationCount(selectedNodeHierarchy)
            downloadCount += modelConfigController.GetDownloadModelConfigurationCount(selectedNodeHierarchy)
            downloadCount += taskConfigController.GetDownloadTaskConfigurationCount(selectedNodeHierarchy)
            downloadCount += stationConfigController.GetDownloadStationConfigurationCount(selectedNodeHierarchy)
            ShowProgressPopUp(downloadCount)

            Dim fileObj = plcConfigController.GetPythonFileObj(selectedNodeHierarchy)
            stationConfigController.DownloadStationConfiguration(fileObj, selectedNodeHierarchy)
            taskConfigController.DownloadTaskConfiguration(fileObj, selectedNodeHierarchy)
            areaConfigController.DownloadAreaConfiguration(fileObj, selectedNodeHierarchy)
            modelConfigController.DownloadModelConfiguration(fileObj, selectedNodeHierarchy)

            'to-do: show error message according to which download failed
            '
            '
            '

            'Performance Analysis'
            PerformanceAnalysisController.SharedEndPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadAll)

            CloseProgressPopUp()
            MsgBox("All Data Downloaded Successfully.", MsgBoxStyle.Information, Application.ProductName)
        Catch ex As Exception
            CloseProgressPopUp()
            MsgBox("Failed to Download." & ex.Message, MsgBoxStyle.Information, Application.ProductName)
            Log_Anything("DownloadAllConfigurationToolStripMenuItem - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub DownloadTaskConfigurationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DownloadTaskConfigurationToolStripMenuItem.Click
        Try
            Dim selectedNodeHierarchy = GetSelectedTreeNodeHierarchy()

            Dim downloadCount = taskConfigController.GetDownloadTaskConfigurationCount(selectedNodeHierarchy)
            ShowProgressPopUp(downloadCount)

            'Performance Analysis'
            PerformanceAnalysisController.SharedStartPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadTask)
            Dim fileObj = plcConfigController.GetPythonFileObj(selectedNodeHierarchy)
            taskConfigController.DownloadTaskConfiguration(fileObj, selectedNodeHierarchy)
            'Performance Analysis'
            PerformanceAnalysisController.SharedEndPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadTask)

            CloseProgressPopUp()
            MsgBox("All Data Downloaded Successfully.", MsgBoxStyle.Information, Application.ProductName)
        Catch ex As Exception
            CloseProgressPopUp()
            MsgBox("Task Data Failed to Download." & ex.Message, MsgBoxStyle.Critical, Application.ProductName)
            Log_Anything("DownloadTaskConfigurationToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub DownloadStationConfigurationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DownloadStationConfigurationToolStripMenuItem.Click
        Try
            Dim selectedNodeHierarchy = GetSelectedTreeNodeHierarchy()

            Dim downloadCount = stationConfigController.GetDownloadStationConfigurationCount(selectedNodeHierarchy)
            ShowProgressPopUp(downloadCount)

            'Performance Analysis'
            PerformanceAnalysisController.SharedStartPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadStation)
            Dim fileObj = plcConfigController.GetPythonFileObj(selectedNodeHierarchy)
            stationConfigController.DownloadStationConfiguration(fileObj, selectedNodeHierarchy)
            'Performance Analysis'
            PerformanceAnalysisController.SharedEndPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadStation)

            CloseProgressPopUp()
            MsgBox("All Data Downloaded Successfully.", MsgBoxStyle.Information, Application.ProductName)
        Catch ex As Exception
            CloseProgressPopUp()
            MsgBox("Station Data Failed to Download." & ex.Message, MsgBoxStyle.Critical, Application.ProductName)
            Log_Anything("DownloadStationConfigurationToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        AboutBox1.TextBoxDescription.Text = Me.Text
        AboutBox1.Show()
    End Sub

    Private Sub ViewHelpToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ViewHelpToolStripMenuItem.Click
        Help.ShowHelp(Me, HelpProvider1.HelpNamespace, HelpNavigator.TableOfContents)
    End Sub

    Private Sub ContextCutCopyPaste_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles ContextCutCopyPaste.Opening
        Dim strCallingTextBox As String
        strCallingTextBox = ContextCutCopyPaste.SourceControl.Name

        For Each ctrl In GroupBox1.Controls
            If ctrl.Name = strCallingTextBox Then
                ctrl.Select()
            End If
        Next
    End Sub

    Private Sub mnu_Copy_Click(sender As System.Object, e As System.EventArgs) Handles mnu_Copy.Click
        Dim intModelNumber As Integer
        Dim intUnderscorePosition As Integer
        Dim bCopyComplete As Boolean
        Dim strNewModelID As String = ""

        strNewModelID = InputBox("Please enter the Model ID for the new Model", Application.ProductName, ContextCutCopyPaste.SourceControl.Text)

        If strNewModelID <> "" Then
            intUnderscorePosition = InStr(ContextCutCopyPaste.SourceControl.Tag, "_")
            If intUnderscorePosition <> 0 Then
                intModelNumber = Mid(ContextCutCopyPaste.SourceControl.Tag, intUnderscorePosition + 1, Len(ContextCutCopyPaste.SourceControl.Tag.ToString) - intUnderscorePosition)
                bCopyComplete = CopyModelConfigurationToDatabase(ContextCutCopyPaste.SourceControl.Text, intModelNumber, txt_TaskNumber.Text.ToString + 1, strNewModelID, tvwProject.SelectedNode.Text)
                If bCopyComplete = True Then
                    DeleteConfigurationItemsFromGroupBox()
                    DeleteConfigurationItemsFromModelGroupBox()
                    GroupBox1.Tag = "AREA"
                    GetAreaStructureFromDatabase(tvwProject.SelectedNode.Text)
                    GetAreaConfigurationFromDatabase()
                    bShowIndexForm = True
                    SaveInitialValuesOfGroupBox1()
                    SaveInitialValuesOfGroupBox2()
                End If
            End If
        End If

    End Sub

    Private Sub mnu_Cut_Click(sender As System.Object, e As System.EventArgs) Handles mnu_Cut.Click
        Dim intResponse As Integer
        Dim intUnderscorePosition As Integer
        Dim intModelNumber As Integer
        Dim bDeleteComplete As Boolean

        intResponse = MsgBox("Are you sure you want to DELETE Model ID: " & ContextCutCopyPaste.SourceControl.Text & " ? All Model / Task associations will be DELETED as well.", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, Application.ProductName)
        If intResponse = 6 Then
            intUnderscorePosition = InStr(ContextCutCopyPaste.SourceControl.Tag, "_")
            If intUnderscorePosition <> 0 Then
                intModelNumber = Mid(ContextCutCopyPaste.SourceControl.Tag, intUnderscorePosition + 1, Len(ContextCutCopyPaste.SourceControl.Tag.ToString) - intUnderscorePosition)
                bDeleteComplete = DeleteModelConfigurationFromDatabase(ContextCutCopyPaste.SourceControl.Text, intModelNumber, tvwProject.SelectedNode.Text)
                If bDeleteComplete = True Then
                    DeleteConfigurationItemsFromGroupBox()
                    DeleteConfigurationItemsFromModelGroupBox()
                    GroupBox1.Tag = "AREA"
                    GetAreaStructureFromDatabase(tvwProject.SelectedNode.Text)
                    GetAreaConfigurationFromDatabase()
                    bShowIndexForm = True
                    SaveInitialValuesOfGroupBox1()
                    SaveInitialValuesOfGroupBox2()
                End If
            End If
        End If

    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim bFileSaved As Boolean
        Try
            If PowertrainProjectFile <> "" Then
                SaveFileDialog1.Title = "Save Current Project File to different Name"
                bFileSaved = SaveAsProjectFile()
                If bFileSaved = True Then
                    'Set the Forms Title
                    SetFormTitle()
                End If
            End If
        Catch ex As Exception
            Log_Anything("SaveAsToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Function SaveAsProjectFile() As Boolean
        Dim newPowerTrainProjectFile As String

        SaveFileDialog1.Filter = "Powertrain Project File (*.ppf)|*.ppf"
        SaveFileDialog1.InitialDirectory = My.Settings.ProjectFolder
        SaveFileDialog1.FileName = ""

        Dim result As DialogResult = SaveFileDialog1.ShowDialog()
        Try
            If result = DialogResult.OK Then
                newPowerTrainProjectFile = SaveFileDialog1.FileName
                If Mid(newPowerTrainProjectFile, Len(newPowerTrainProjectFile) - 3, 4) <> ".ppf" Then
                    newPowerTrainProjectFile = newPowerTrainProjectFile & ".ppf"
                End If

                FileCopy(PowertrainProjectFile, newPowerTrainProjectFile)
                PowertrainProjectFile = newPowerTrainProjectFile

                '***********************************************************************
                If GroupBox1.Tag = "AREA" Then
                    Dim bAreaConfigurationISOk As Boolean
                    bAreaConfigurationISOk = CheckAreaConfiguration()
                    If bAreaConfigurationISOk Then
                        SetAreaConfigurationToDatabase()
                    End If
                ElseIf GroupBox1.Tag = "STATION" Then
                    Dim bStationConfigurationISOk As Boolean = True
                    'bStationConfigurationISOk = CheckAreaConfiguration()
                    If bStationConfigurationISOk Then
                        SetStationConfigurationToDatabase()
                    End If
                ElseIf GroupBox1.Tag = "TASK" Then
                    Dim bTaskConfigurationISOk As Boolean = True
                    'bStationConfigurationISOk = CheckAreaConfiguration()
                    If bTaskConfigurationISOk Then
                        If NumericUpDownTaskInstance.Visible = True Then
                            SetTaskConfigurationToDatabase(NumericUpDownTaskInstance.Value)
                            SetModelConfigurationToDatabase(cbo_Models.SelectedIndex, NumericUpDownTaskInstance.Value - 1)
                        Else
                            SetTaskConfigurationToDatabase(-1)
                            SetModelConfigurationToDatabase(-1, -1)
                        End If
                    End If
                ElseIf GroupBox1.Tag = "PLC" Then
                    Dim bPLCConfigurationISOk As Boolean = True
                    'bStationConfigurationISOk = CheckAreaConfiguration()
                    If bPLCConfigurationISOk Then
                        SetPLCConfigurationToDatabase()
                    End If
                End If
                '***********************************************************************
                SaveAsProjectFile = True
            Else
                SaveAsProjectFile = False
            End If
        Catch ex As Exception
            Log_Anything("SaveAsProjectFile - " & GetExceptionInfo(ex))
            SaveAsProjectFile = False
        End Try
    End Function

    Private Sub ExpandTreeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExpandTreeToolStripMenuItem.Click
        tvwProject.ExpandAll()
    End Sub

    Private Sub CollapseTreeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CollapseTreeToolStripMenuItem.Click
        tvwProject.CollapseAll()
    End Sub

    Private Function GetSelectedTreeNodeHierarchy() As TreeNodeHierarchy
        Dim strStationName = tvwProject.SelectedNode.Text
        Dim strSectionName = tvwProject.SelectedNode.Parent.Text
        Dim strAreaName = tvwProject.SelectedNode.Parent.Parent.Text

        Return New TreeNodeHierarchy(strAreaName, strSectionName, strStationName)
    End Function

    Private Sub UploadProjectConfigurationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles UploadProjectConfigurationToolStripMenuItem.Click
        Try
            Dim selectedNodeHierarchy = GetSelectedTreeNodeHierarchy()

            Dim uploadCount = taskConfigController.GetDownloadTaskConfigurationCount(selectedNodeHierarchy)
            uploadCount += stationConfigController.GetDownloadStationConfigurationCount(selectedNodeHierarchy)
            ShowProgressPopUp(uploadCount)

            'Performance Analysis'
            PerformanceAnalysisController.SharedStartPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadTask)
            Dim fileObj = plcConfigController.GetPythonFileObj(selectedNodeHierarchy)
            taskConfigController.UploadTaskConfiguration(fileObj, selectedNodeHierarchy)
            stationConfigController.UploadStationConfiguration(fileObj, selectedNodeHierarchy)

            'Performance Analysis'
            PerformanceAnalysisController.SharedEndPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadTask)
            CloseProgressPopUp()
            MsgBox("All Data Uploaded Successfully.", MsgBoxStyle.Information, Application.ProductName)
            Reload(tvwProject.SelectedNode)
        Catch ex As Exception
            CloseProgressPopUp()
            MsgBox("Model Data Failed to Upload." & ex.Message, MsgBoxStyle.Critical, Application.ProductName)
            Log_Anything("UploadProjectConfigurationToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub UploadModelConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UploadModelConfigurationToolStripMenuItem.Click
        Try
            Dim selectedNodeHierarchy = GetSelectedTreeNodeHierarchy()

            Dim uploadCount = modelConfigController.GetDownloadModelConfigurationCount(selectedNodeHierarchy)
            uploadCount += areaConfigController.GetDownloadAreaConfigurationCount(selectedNodeHierarchy)
            ShowProgressPopUp(uploadCount)
            'Performance Analysis'
            PerformanceAnalysisController.SharedStartPerformanceAnalysis(Enums.PerformanceAnalysisType.UploadModel)
            Dim fileObj = plcConfigController.GetPythonFileObj(selectedNodeHierarchy)
            modelConfigController.UploadModelConfiguration(fileObj, selectedNodeHierarchy)
            areaConfigController.UploadAreaConfiguration(fileObj, selectedNodeHierarchy)
            'Performance Analysis'
            PerformanceAnalysisController.SharedEndPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadTask)
            CloseProgressPopUp()
            MsgBox("All Data Uploaded Successfully.", MsgBoxStyle.Information, Application.ProductName)
            Reload(tvwProject.SelectedNode)
        Catch ex As Exception
            CloseProgressPopUp()
            MsgBox("Model Data Failed to Upload." & ex.Message, MsgBoxStyle.Critical, Application.ProductName)
            Log_Anything("UploadModelConfigurationToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub UploadAllConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UploadAllConfigurationToolStripMenuItem.Click
        Try
            Dim selectedNodeHierarchy = GetSelectedTreeNodeHierarchy()

            'Performance Analysis'
            PerformanceAnalysisController.SharedStartPerformanceAnalysis(Enums.PerformanceAnalysisType.UploadAll)

            Dim uploadCount = areaConfigController.GetDownloadAreaConfigurationCount(selectedNodeHierarchy)
            uploadCount += modelConfigController.GetDownloadModelConfigurationCount(selectedNodeHierarchy)
            uploadCount += taskConfigController.GetDownloadTaskConfigurationCount(selectedNodeHierarchy)
            uploadCount += stationConfigController.GetDownloadStationConfigurationCount(selectedNodeHierarchy)
            ShowProgressPopUp(uploadCount)

            Dim fileObj = plcConfigController.GetPythonFileObj(selectedNodeHierarchy)
            stationConfigController.UploadStationConfiguration(fileObj, selectedNodeHierarchy)
            taskConfigController.UploadTaskConfiguration(fileObj, selectedNodeHierarchy)
            areaConfigController.UploadAreaConfiguration(fileObj, selectedNodeHierarchy)
            modelConfigController.UploadModelConfiguration(fileObj, selectedNodeHierarchy)

            'Performance Analysis'
            PerformanceAnalysisController.SharedEndPerformanceAnalysis(Enums.PerformanceAnalysisType.DownloadAll)

            CloseProgressPopUp()
            MsgBox("All Data Uploaded Successfully.", MsgBoxStyle.Information, Application.ProductName)
            Reload(tvwProject.SelectedNode)
        Catch ex As Exception
            CloseProgressPopUp()
            MsgBox("Failed to Upload." & ex.Message, MsgBoxStyle.Information, Application.ProductName)
            Log_Anything("UploadAllConfigurationToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub
End Class