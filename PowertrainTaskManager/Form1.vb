Imports System.Data.OleDb
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Controllers
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models

Public Class Form1
    Public SystemColor As Color
    Public bISDirty As Boolean = False

    Public Const origFormX As Single = 1360
    Public Const origFormY As Single = 760
    Dim strNodeBeforeEdit As TreeNode

    Dim strAreaName As String
    Dim strSectionName As String
    Dim strStationName As String
    Dim _renameMenuAction As MenuRenameAction = New MenuRenameAction()

    Dim taskController As TaskController
    Dim taskConfigController As TaskConfigurationController
    Dim areaController As AreasController
    Dim areaConfigController As AreaConfigurationController
    Dim plcInfoControllerMaster As PlcInformationController
    Dim plcConfigController As PlcConfigurationController
    Dim stationsController As StationController
    Dim stationConfigController As StationConfigurationController
    Dim plantController As PlantController
    Dim sectionController As SectionsController
    Dim sectionCofigController As SectionConfigurationController
    Dim mfTaskControllerMaster As MasterFilesTaskController
    Dim mfControllerMaster As MasterFileController
    Dim muControllerMaster As MemoryUsageController

    Private Sub Form1_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        CheckUserLevel()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            AskToSave()
            tvwProject.Nodes.Clear()
            dgvPLCMemory.Rows.Clear()
            Form5.Show()
        Catch ex As Exception
            Log_Anything("Form1_FormClosing - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            'Set the Title for the Form
            SetFormTitle()

            Me.Width = origFormX
            Me.Height = origFormY

            HelpProvider1.HelpNamespace = strHelpPath

            SystemColor = lblSystemCheck.BackColor
            'Setup the Datagrid properties
            SetupDataGridView()

            'Setup Tree Properties
            tvwProject.AllowDrop = True
            tvwProject.HideSelection = False
            tvwProject.ImageList = ImageList1

            ' start off by adding a base treeview node
            Dim mainNode = TreeHierarchyController.GenerateTreeNode(UiDisplayConstant.MainNode, UiDisplayConstant.PowertrainPlant, TreeNodeTagConstant.PLANT, TreeNodeImageIndexConstant.Plant, TreeNodeImageIndexConstant.Plant)
            Me.tvwProject.Nodes.Add(mainNode)
            tvwProject.Sort()

            Dim plcMainNode = TreeHierarchyController.GenerateTreeNode(UiDisplayConstant.MainNode, UiDisplayConstant.Processors, String.Empty, TreeNodeImageIndexConstant.Plc, TreeNodeImageIndexConstant.Plc)
            Me.tvwPLCs.Nodes.Add(plcMainNode)
            tvwPLCs.Sort()

            Dim mfMainNode = TreeHierarchyController.GenerateTreeNode(UiDisplayConstant.MainNode, UiDisplayConstant.MasterFiles, String.Empty, TreeNodeImageIndexConstant.MasterFile, TreeNodeImageIndexConstant.MasterFile)
            Me.tvwMasterTasks.Nodes.Add(mfMainNode)
            tvwMasterTasks.Sort()

            'Init all controllers - mainly for plcInfoControllerMaster
            InitAllControllers()

            'Check to make sure that the System Settings have been set up.
            CheckSystemSettings()

            tvwProject.CollapseAll()

            tvwMasterTasks.Nodes(0).Expand()
            tvwPLCs.Nodes(0).Expand()
            'tvwProject.Nodes(0).Expand()

            SetupPreviousFilesMenu()

            CheckUserLevel()

        Catch ex As Exception
            Log_Anything("Form1_Load - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub CheckUserLevel()
        If strUserName = "Operator" Or strUserName = "Maintenance" Then
            tvwPLCs.Enabled = False
            tvwMasterTasks.Enabled = False
            tvwProject.Enabled = True
            NewToolStripMenuItem.Enabled = False
            SaveAsToolStripMenuItem.Enabled = False
            SaveToolStripMenuItem.Enabled = False
        Else
            tvwPLCs.Enabled = True
            tvwMasterTasks.Enabled = True
            tvwProject.Enabled = True
            NewToolStripMenuItem.Enabled = True
            SaveAsToolStripMenuItem.Enabled = True
            SaveToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub Form1_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel

    End Sub

    Private Sub Form1_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        Me.SplitContainer1.Width = Me.Width * 0.3
        Me.SplitContainer2.Width = Me.Width * 0.3
        Me.SplitContainer3.Width = Me.Width * 0.4

        Me.SplitContainer3.Location = New Point(Me.SplitContainer1.Width + 6, 24)
        Me.SplitContainer2.Location = New Point(Me.SplitContainer1.Width + 6 + Me.SplitContainer3.Width + 6, 24)
        Me.Refresh()
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        GC.WaitForPendingFinalizers()
        GC.Collect()
        Me.Dispose()
    End Sub

    ''' <summary>
    ''' Init all controllers used in this form
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitAllControllers()
        'Init controllers used in this form
        taskController = New TaskController()
        taskConfigController = New TaskConfigurationController()
        areaController = New AreasController()
        areaConfigController = New AreaConfigurationController()
        stationsController = New StationController()
        stationConfigController = New StationConfigurationController()
        plcInfoControllerMaster = New PlcInformationController()
        plcConfigController = New PlcConfigurationController()
        plantController = New PlantController()
        sectionController = New SectionsController()
        sectionCofigController = New SectionConfigurationController()
        mfTaskControllerMaster = New MasterFilesTaskController()
        mfControllerMaster = New MasterFileController()
        muControllerMaster = New MemoryUsageController()
        taskController.Init()
        taskConfigController.Init()
        areaController.Init()
        areaConfigController.Init()
        stationsController.Init()
        stationConfigController.Init()
        plcInfoControllerMaster.InitMaster()
        plcConfigController.Init()
        plantController.Init()
        sectionController.Init()
        sectionCofigController.Init()
        mfTaskControllerMaster.InitMaster()
        mfControllerMaster.InitMaster()
        muControllerMaster.InitMaster()
    End Sub

    Private Function DealWithMenuRemoveAction(menuAction As String) As Boolean
        If menuAction.Equals(MenuActionConstant.RemoveArea) Then
            Return RemoveAreaConfiguration()
        ElseIf menuAction.Equals(MenuActionConstant.RemoveSection) Then
            Return RemoveSectionConfiguration()
        ElseIf menuAction.Equals(MenuActionConstant.RemoveStation) Then
            Return RemoveStationConfiguration()
        ElseIf menuAction.Equals(MenuActionConstant.RemoveTask) Then
            Return RemoveTaskConfiguration()
        End If
        Return False
    End Function

    ''' <summary>
    ''' Remove task configuration according to project file tree view
    ''' </summary>
    ''' <returns>The flag to remove</returns>
    ''' <remarks></remarks>
    Private Function RemoveTaskConfiguration() As Boolean
        Dim taskName = tvwProject.SelectedNode.Text
        Dim stationName = tvwProject.SelectedNode.Parent.Parent.Text
        Dim sectionName = tvwProject.SelectedNode.Parent.Parent.Parent.Text
        Dim areaName = tvwProject.SelectedNode.Parent.Parent.Parent.Parent.Text

        Dim taskConig = New TaskConfiguration(areaName, sectionName, stationName, taskName)
        Dim configDataExists = taskConfigController.CheckTaskConfigurationExisted(taskConig)
        If configDataExists Then
            Dim responseResult = MsgBox("There is existing data that is configured for this TASK. If you remove this TASK it will be deleted.", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, Application.ProductName)
            If responseResult = MsgBoxResult.Yes Then
                taskConfigController.DeleteTaskConfiguration(areaName, sectionName, stationName, taskName)
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function

    Private Function RemoveStationConfiguration() As Boolean
        Dim stationName = tvwProject.SelectedNode.Text
        Dim sectionName = tvwProject.SelectedNode.Parent.Text
        Dim areaName = tvwProject.SelectedNode.Parent.Parent.Text

        Dim configDataExists = stationConfigController.CheckStationConfigurationExisted(areaName, sectionName, stationName)
        If configDataExists Then
            Dim responseResult = MsgBox("There is existing data that is configured for this STATION. If you remove this STATION it will be deleted.", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, Application.ProductName)
            If responseResult = MsgBoxResult.Yes Then
                stationConfigController.DeleteStationConfiguration(areaName, sectionName, stationName)
                taskConfigController.DeleteTaskConfiguration(areaName, sectionName, stationName)
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function

    Private Function RemoveSectionConfiguration() As Boolean
        Dim sectionName = tvwProject.SelectedNode.Text
        Dim areaName = tvwProject.SelectedNode.Parent.Text

        Dim configDataExists = sectionCofigController.CheckSectionConfigurationExisted(areaName, sectionName)
        If configDataExists Then
            Dim responseResult = MsgBox("There is existing data that is configured for this SECTION. If you remove this SECTION it will be deleted.", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, Application.ProductName)
            If responseResult = MsgBoxResult.Yes Then
                sectionCofigController.DeleteSectionConfiguration(areaName, sectionName)
                stationConfigController.DeleteStationConfiguration(areaName, sectionName)
                taskConfigController.DeleteTaskConfiguration(areaName, sectionName)
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function

    Private Function RemoveAreaConfiguration() As Boolean
        Dim areaName = tvwProject.SelectedNode.Text

        Dim configDataExists = areaConfigController.CheckAreaConfigurationExisted(areaName)
        If configDataExists = False Then
            configDataExists = stationConfigController.CheckStationConfigurationExisted(areaName) 'why ???
        End If
        If configDataExists Then
            Dim responseResult = MsgBox("There is existing data that is configured for this AREA. If you remove this AREA it will be deleted.", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, Application.ProductName)
            If responseResult = MsgBoxResult.Yes Then
                areaConfigController.DeleteAreaConfiguration(areaName)
                sectionCofigController.DeleteSectionConfiguration(areaName)
                stationConfigController.DeleteStationConfiguration(areaName)
                taskConfigController.DeleteTaskConfiguration(areaName)
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function


    Private Sub RecordOriginalMenuRenameActionData()
        tvwProject.LabelEdit = True
        _renameMenuAction = New MenuRenameAction()

        Dim selectedTreeNode = tvwProject.SelectedNode
        If selectedTreeNode.Tag.Equals(TreeNodeTagConstant.AREA) Then
            _renameMenuAction.SelectedTreeNodeTag = TreeNodeTagConstant.AREA
            _renameMenuAction.AreaName = tvwProject.SelectedNode.Text
        ElseIf selectedTreeNode.Tag.Equals(TreeNodeTagConstant.SECTION) Then
            _renameMenuAction.SelectedTreeNodeTag = TreeNodeTagConstant.SECTION
            _renameMenuAction.SectionName = tvwProject.SelectedNode.Text
            _renameMenuAction.AreaName = tvwProject.SelectedNode.Parent.Text
        ElseIf selectedTreeNode.Tag.Equals(TreeNodeTagConstant.STATION) Then
            _renameMenuAction.SelectedTreeNodeTag = TreeNodeTagConstant.STATION
            _renameMenuAction.StationName = tvwProject.SelectedNode.Text
            _renameMenuAction.SectionName = tvwProject.SelectedNode.Parent.Text
            _renameMenuAction.AreaName = tvwProject.SelectedNode.Parent.Parent.Text
        End If
        tvwProject.SelectedNode.BeginEdit()
    End Sub

    Private Sub DealWithMenuRenameAction(newValue As String)
        If _renameMenuAction.SelectedTreeNodeTag.Equals(TreeNodeTagConstant.AREA) Then
            _renameMenuAction.NewSectionName = newValue
            'update
            areaConfigController.UpdateAreaConfiguration(_renameMenuAction.AreaName, newValue)
            sectionCofigController.UpdateSectionConfiguration(_renameMenuAction.AreaName, newValue)
            stationConfigController.UpdateStationConfiguration(_renameMenuAction.AreaName, newValue)
            taskConfigController.UpdateTaskConfiguration(_renameMenuAction.AreaName, newValue)

        ElseIf _renameMenuAction.SelectedTreeNodeTag.Equals(TreeNodeTagConstant.SECTION) Then
            _renameMenuAction.NewSectionName = newValue
            'update
            sectionCofigController.UpdateSectionConfiguration(_renameMenuAction.AreaName, _renameMenuAction.SectionName, newValue)
            stationConfigController.UpdateStationConfiguration(_renameMenuAction.AreaName, _renameMenuAction.SectionName, newValue)
            taskConfigController.UpdateTaskConfiguration(_renameMenuAction.AreaName, _renameMenuAction.SectionName, newValue)
        ElseIf _renameMenuAction.SelectedTreeNodeTag.Equals(TreeNodeTagConstant.STATION) Then
            _renameMenuAction.NewStationName = newValue
            'update
            stationConfigController.UpdateStationConfiguration(_renameMenuAction.AreaName, _renameMenuAction.SectionName, _renameMenuAction.StationName, newValue)
            taskConfigController.UpdateTaskConfiguration(_renameMenuAction.AreaName, _renameMenuAction.SectionName, _renameMenuAction.StationName, newValue)
        End If
    End Sub

    Private Sub DealWithMenuAddAction(menuAction As String)
        Dim strKey = tvwProject.SelectedNode.FullPath
        Dim strPath = Split(strKey, "\")

        If menuAction.Equals(MenuActionConstant.AddArea) Then
            AddAreaToTree(DefaultValueConstant.NewArea)
        ElseIf menuAction.Equals(MenuActionConstant.AddSection) Then
            AddSectionToTree(strPath(1), DefaultValueConstant.NewSection)
        ElseIf menuAction.Equals(MenuActionConstant.AddStationAuto) Then
            AddStationToTree(strPath(1), strPath(2), DefaultValueConstant.NewStation, StationTypeConstant.Auto)
        ElseIf menuAction.Equals(MenuActionConstant.AddStationManual) Then
            AddStationToTree(strPath(1), strPath(2), DefaultValueConstant.NewStation, StationTypeConstant.Manual)
        End If
    End Sub

    Private Sub DealWithMenuChangeAction(menuAction As String)
        If menuAction.Equals(MenuActionConstant.ChangeToManualStation) Then
            tvwProject.SelectedNode.ToolTipText = StationTypeConstant.Manual
            CheckAllNodesForMemoryUsage()
            bISDirty = True
        End If

        If menuAction.Equals(MenuActionConstant.ChangeToAutoStation) Then
            tvwProject.SelectedNode.ToolTipText = StationTypeConstant.Auto
            CheckAllNodesForMemoryUsage()
            bISDirty = True
        End If
    End Sub

    Private Sub DealWithMenuCopyAction()
        Dim stationTreeNodeHier = GetSelectedStationTreeNodeHierarchy()
        Dim newStationName = stationsController.CalculateStationNameForCopy(stationTreeNodeHier)
        'add station to project file db
        Dim currentStation = stationsController.GetStationModel(stationTreeNodeHier)
        If currentStation IsNot Nothing Then
            currentStation.StationName = newStationName
            stationsController.InsertStation(currentStation)
        End If
        'add station configuration to project file db
        Dim stationConfigList = stationConfigController.GetStationConfiguration(stationTreeNodeHier)
        stationConfigList.ForEach(Sub(x) x.StationName = newStationName)
        stationConfigController.InsertStationConfiguration(stationConfigList)

        'add task to project file db
        Dim taskList = taskController.GetTaskList(stationTreeNodeHier)
        taskList.ForEach(Sub(x) x.StationName = newStationName)
        taskController.InsertTask(taskList)

        'add task configuration to project file db
        Dim taskConfigList = taskConfigController.GetTaskConfiguration(stationTreeNodeHier)
        taskConfigList.ForEach(Sub(x) x.StationName = newStationName)
        taskConfigController.InsertTaskConfiguration(taskConfigList)

        'add plc configuration to project file db
        Dim plcConfigList = plcConfigController.GetPlcConfiguration(stationTreeNodeHier)
        plcConfigList.ForEach(Sub(x) x.StationName = newStationName)
        plcConfigController.InsertPlcConfiguration(plcConfigList)
    End Sub

    Private Function GetSelectedStationTreeNodeHierarchy() As TreeNodeHierarchy
        Dim stationName = tvwProject.SelectedNode.Text
        Dim sectionName = tvwProject.SelectedNode.Parent.Text
        Dim areaName = tvwProject.SelectedNode.Parent.Parent.Text

        Return New TreeNodeHierarchy(areaName, sectionName, stationName)
    End Function
    
    Private Sub mnuPos1_Click(sender As System.Object, e As System.EventArgs) Handles mnuPos1.Click
        Try
            Dim menuAction = mnuPos1.Text

            If menuAction.Equals(MenuActionConstant.Rename) Then
                RecordOriginalMenuRenameActionData()
            End If

            If menuAction.Contains("Remove") Then
                Dim bOktoRemove = DealWithMenuRemoveAction(menuAction)
                If bOktoRemove = True Then
                    tvwProject.SelectedNode.Remove()
                    CheckAllNodesForMemoryUsage()
                    ColorPLCListBasedOnMemory()
                End If
            End If
        Catch ex As Exception
            Log_Anything("mnuPos1_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub mnuPos2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPos2.Click
        Try
            Dim menuAction = mnuPos2.Text

            'rename action
            If menuAction.Equals(MenuActionConstant.Rename) Then
                RecordOriginalMenuRenameActionData()
            End If

            'remove action
            If menuAction.Contains("Remove") Then
                Dim bOktoRemove = DealWithMenuRemoveAction(menuAction)
                If bOktoRemove = True Then
                    tvwProject.SelectedNode.Remove()
                    CheckAllNodesForMemoryUsage()
                End If
            End If

            'add action
            If menuAction.Contains("Add") Then
                DealWithMenuAddAction(menuAction)
            End If
        Catch ex As Exception
            Log_Anything("mnuPos2_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub mnuPos3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPos3.Click
        Try
            Dim menuAction = mnuPos3.Text

            If menuAction.Contains("Remove") Then
                Dim bOktoRemove = DealWithMenuRemoveAction(menuAction)
                If bOktoRemove = True Then
                    tvwProject.SelectedNode.Remove()
                    CheckAllNodesForMemoryUsage()
                End If

                bISDirty = True
            End If

            If menuAction.Contains("Add") Then
                DealWithMenuAddAction(menuAction)
            End If

            DealWithMenuChangeAction(menuAction)
        Catch ex As Exception
            Log_Anything("mnuPos2_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub mnuPos4_Click(sender As System.Object, e As System.EventArgs) Handles mnuPos4.Click
        Try
            Dim menuAction = mnuPos4.Text

            If menuAction.Contains("Remove") Then
                Dim bOktoRemove = DealWithMenuRemoveAction(menuAction)
                If bOktoRemove = True Then
                    tvwProject.SelectedNode.Remove()
                    CheckAllNodesForMemoryUsage()
                End If

                bISDirty = True
            End If

            If menuAction.Equals(MenuActionConstant.GenerateBaseProgram) Then
                GenerateBasePLCProgram()
            End If


        Catch ex As Exception
            Log_Anything("mnuPos2_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub mnuPos5_Click(sender As System.Object, e As System.EventArgs) Handles mnuPos5.Click
        Dim menuAction = mnuPos5.Text
        If menuAction.Equals(MenuActionConstant.CopyStation) Then
            Dim strWhereClause As String
            If PowertrainProjectFile <> "" Then
                'delete orginal data
                DeleteOriginalData()
                'save the new data
                InsertPlantData()
            End If

            If tvwProject.SelectedNode.Tag.Equals(TreeNodeTagConstant.STATION) Then

                'strAreaNameBeingCopied = tvwProject.SelectedNode.Parent.Parent.Text
                'strSectionNameBeingCopied = tvwProject.SelectedNode.Parent.Text
                'strStationNameBeingCopied = tvwProject.SelectedNode.Text

                'strWhereClause = "AREA_NAME = '" & strAreaNameBeingCopied & "' AND SECTION_NAME = '" & strSectionNameBeingCopied & "' AND STATION_NAME = '" & strStationNameBeingCopied & "'"

                DealWithMenuCopyAction()
                'CopyDatabaseForConfiguredGroup("STATION", strWhereClause, strStationNameBeingCopied)
                GetPlantDataFromDatabase()
                CheckAllNodesForMemoryUsage()

            End If
        End If
    End Sub

    Private Sub tvwProject_BeforeLabelEdit(sender As Object, e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tvwProject.BeforeLabelEdit
        strNodeBeforeEdit = e.Node
    End Sub

    Private Sub tvwProject_AfterLabelEdit(sender As Object, e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tvwProject.AfterLabelEdit
        Try
            tvwProject.LabelEdit = False

            If Not String.IsNullOrWhiteSpace(_renameMenuAction.SelectedTreeNodeTag) Then
                DealWithMenuRenameAction(e.Label)
            End If

            e.Node.Text = e.Label
            e.Node.Name = e.Label
            bISDirty = True
            CheckAllNodesForMemoryUsage()
            tvwProject.Sort()
        Catch ex As Exception
            Log_Anything("tvwProject_AfterLabelEdit - " & GetExceptionInfo(ex))
            Throw ex
        End Try
    End Sub

    Private Sub tvwProject_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwProject.AfterSelect
        Try
            Dim strKey As String
            'Dim strPathArray() As String
            txtName.Text = ""
            txtParentName.Text = ""
            txtText.Text = ""
            txtTag.Text = ""

            txtName.Text = tvwProject.SelectedNode.Name.ToString()
            txtText.Text = tvwProject.SelectedNode.Text.ToString()
            txtTag.Text = tvwProject.SelectedNode.Tag.ToString()
            If tvwProject.SelectedNode.Tag <> "PLANT" Then
                txtParentName.Text = tvwProject.SelectedNode.Parent.Text.ToString()
            End If
            strKey = tvwProject.SelectedNode.FullPath

            If txtTag.Text = "STATION" Then
                strStationName = tvwProject.SelectedNode.Name.ToString
                strSectionName = tvwProject.SelectedNode.Parent.Text.ToString
                strAreaName = tvwProject.SelectedNode.Parent.Parent.Text.ToString
                ColorPLCListBasedOnMemory()
            End If

        Catch ex As Exception
            Log_Anything("treeView1_AfterSelect - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub tvwProject_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles tvwProject.MouseClick
        Try
            Dim strStationType As String

            With contextMenuStrip1
                .Items(0).Visible = False
                .Items(1).Visible = False
                .Items(2).Visible = False
                .Items(3).Visible = False
            End With

            If e.Button = Windows.Forms.MouseButtons.Right And strUserName = "Administrator" Then
                With contextMenuStrip1

                    tvwProject.SelectedNode = tvwProject.GetNodeAt(e.X, e.Y)

                    .Items(0).Visible = False
                    .Items(1).Visible = False
                    .Items(2).Visible = False
                    .Items(3).Visible = False
                    .Items(4).Visible = False

                    If tvwProject.SelectedNode.Tag = "PLANT" Then
                        .Items(0).Text = "Rename"
                        .Items(1).Text = "Add Area"
                        .Items(0).Visible = True
                        .Items(1).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "AREA" Then
                        .Items(0).Text = "Rename"
                        .Items(1).Text = "Add Section"
                        .Items(2).Text = "Remove Area"
                        .Items(0).Visible = True
                        .Items(1).Visible = True
                        .Items(2).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "SECTION" Then
                        .Items(0).Text = "Rename"
                        .Items(1).Text = "Add Station (Auto)"
                        .Items(2).Text = "Add Station (Manual)"
                        .Items(3).Text = "Remove Section"
                        .Items(0).Visible = True
                        .Items(1).Visible = True
                        .Items(2).Visible = True
                        .Items(3).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "STATION" Then
                        .Items(0).Text = "Rename"
                        .Items(1).Text = "Remove Station"
                        If tvwProject.SelectedNode.ToolTipText = "Auto" Then
                            strStationType = "Manual"
                        Else
                            strStationType = "Auto"
                        End If
                        .Items(2).Text = "Change to " & strStationType & " Station"
                        .Items(3).Text = "Generate Base Program (L5X)"
                        .Items(4).Text = "Copy Station"
                        .Items(0).Visible = True
                        .Items(1).Visible = True
                        .Items(2).Visible = True
                        .Items(3).Visible = True
                        .Items(4).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "MASTERFILE" Then
                        .Items(0).Text = "Remove MasterFile"
                        .Items(0).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "TASK" Then
                        .Items(0).Text = "Remove Task"
                        .Items(0).Visible = True
                    ElseIf tvwProject.SelectedNode.Tag = "PLC" Then
                        .Items(0).Text = "Remove PLC"
                        .Items(0).Visible = True
                    Else
                        sender.handled = True
                        Exit Sub
                    End If

                    '.Show(New Point(Cursor.Position.X, Cursor.Position.Y))
                End With

            End If
        Catch ex As Exception
            Log_Anything("tvwProject_MouseClick - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub tvwProject_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles tvwProject.DragDrop
        Try
            Dim msgResponse As Integer
            Dim bMasterFileUpdated As Boolean
            Dim strPath() As String

            'Check that there is a TreeNode being dragged
            If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = False Then Exit Sub

            'Get the TreeView raising the event (incase multiple on form)
            Dim selectedTreeview As TreeView = CType(sender, TreeView)

            'Get the TreeNode being dragged
            Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)
            Dim cloneNode As TreeNode = dropNode.Clone
            'The target node should be selected from the DragOver event
            Dim targetNode As TreeNode = selectedTreeview.SelectedNode

            If cloneNode.Tag.ToString = "PLC" And targetNode.Tag.ToString <> "STATION" Then
                MsgBox("PLC's can only be added to Stations")
                Exit Sub
            ElseIf cloneNode.Tag.ToString = "PLC" And targetNode.Tag.ToString = "STATION" Then
                For Each nod In targetNode.Nodes
                    If nod.tag = "PLC" Then
                        msgResponse = MsgBox("PLC " & nod.Name & " already exists. Overwrite with " & cloneNode.Name & " ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Application.ProductName)
                        If msgResponse = 6 Then
                            nod.remove()
                        Else
                            Exit Sub
                        End If
                        Exit For
                    End If
                Next
            End If

            If dropNode.Tag.ToString = "TASK" And (targetNode.Tag.ToString <> "MASTERFILE" And targetNode.Tag.ToString <> "STATION" And targetNode.Tag.ToString <> "PLC") Then
                MsgBox("Tasks can only be added to MasterFile")
                Exit Sub
            End If

            If dropNode.Tag.ToString = "STATION" And targetNode.Tag.ToString <> "SECTION" Then
                MsgBox("Stations can only be added to Sections")
                Exit Sub
            End If

            If dropNode.Tag.ToString = "SECTION" And targetNode.Tag.ToString <> "AREA" Then
                MsgBox("Sections can only be added to Areas")
                Exit Sub
            End If

            If dropNode.Tag.ToString = "AREA" And targetNode.Tag.ToString <> "PLANT" Then
                MsgBox("Areas can only be added to Main Plant")
                Exit Sub
            End If

            Dim strDropKey As String = dropNode.FullPath
            Dim strDropPath() As String = Split(strDropKey, "\")


            'Remove the drop node from its current location
            If dropNode.Tag.ToString <> "PLC" And (dropNode.Tag.ToString <> "TASK" And dropNode.Parent.Tag <> "MASTERFILE") Then
                dropNode.Remove()
            End If

            'If there is no targetNode add dropNode to the bottom of the TreeView root
            'nodes, otherwise add it to the end of the dropNode child nodes
            If dropNode.Tag <> "PLC" And (dropNode.Tag.ToString <> "TASK" And dropNode.Parent.Tag <> "MASTERFILE") Then
                If targetNode Is Nothing Then
                    selectedTreeview.Nodes.Add(dropNode)
                Else
                    targetNode.Nodes.Add(dropNode)
                End If
            ElseIf dropNode.Tag = "PLC" Then
                cloneNode.Text = "_" & cloneNode.Text
                If targetNode Is Nothing Then
                    selectedTreeview.Nodes.Add(cloneNode)
                Else
                    targetNode.Nodes.Add(cloneNode)
                End If
            ElseIf dropNode.Tag.ToString = "TASK" And dropNode.Parent.Tag = "MASTERFILE" Then
                Dim strKey As String = targetNode.FullPath

                Dim strMasterFileName As String
                strPath = Split(strKey, "\")
                If strDropPath(0) = tvwProject.Nodes(0).Text Then
                    strMasterFileName = Mid(dropNode.Parent.Text, 2, Len(dropNode.Parent.Text) - 1)
                    dropNode.Remove()
                Else
                    strMasterFileName = dropNode.Parent.Text
                End If
                bMasterFileUpdated = AddMasterFileToProjectTree(strPath(1), strPath(2), strPath(3), strMasterFileName)

                If bMasterFileUpdated = True Then
                    AddTaskToProjectTree(strPath(1), strPath(2), strPath(3), strMasterFileName, dropNode.Text)
                End If
                'add by chaoyang to fix drag base file from master to project
            ElseIf dropNode.Tag.ToString = "MASTERFILEATTRIBUTE" And dropNode.Parent.Tag = "MASTERFILE" Then
                Dim strKey As String = targetNode.FullPath

                Dim strMasterFileName As String
                strPath = Split(strKey, "\")
                If strDropPath(0) = tvwProject.Nodes(0).Text Then
                    strMasterFileName = Mid(dropNode.Parent.Text, 2, Len(dropNode.Parent.Text) - 1)
                    dropNode.Remove()
                Else
                    strMasterFileName = dropNode.Parent.Text
                End If
                bMasterFileUpdated = AddMasterFileToProjectTree(strPath(1), strPath(2), strPath(3), strMasterFileName)
                If bMasterFileUpdated = True Then
                    Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
                    Dim scdbSqlHelper = New SqlHelper(dbConnStr)
                    Dim masterFilesTaskController = New MasterFilesTaskController(scdbSqlHelper)
                    Dim listMasterFilesTask = masterFilesTaskController.GetMasterFilesTaskByStation(strMasterFileName)
                    For i = 0 To listMasterFilesTask.Count - 1
                        AddTaskToProjectTree(strPath(1), strPath(2), strPath(3), strMasterFileName, listMasterFilesTask(i).TaskName)
                    Next
                End If
            End If


            'Ensure the newly created node is visible to the user and select it
            If dropNode.Tag.ToString <> "PLC" And (dropNode.Tag.ToString <> "TASK") Then
                dropNode.EnsureVisible()
                selectedTreeview.SelectedNode = dropNode
            ElseIf dropNode.Tag.ToString = "PLC" Then
                cloneNode.EnsureVisible()
                selectedTreeview.SelectedNode = cloneNode
            ElseIf dropNode.Tag.ToString = "TASK" And bMasterFileUpdated Then
                For Each AreaNode In tvwProject.Nodes(0).Nodes
                    If AreaNode.Name = strPath(1) Then
                        For Each SectionNode In AreaNode.Nodes
                            If SectionNode.Name = strPath(2) Then
                                For Each StationNode In SectionNode.Nodes
                                    If StationNode.Name = strPath(3) Then
                                        For Each MasterFileNode In StationNode.Nodes
                                            If MasterFileNode.Tag = "MASTERFILE" Then
                                                MasterFileNode.Expand()
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next
            End If

            CheckAllNodesForMemoryUsage()
            ColorPLCListBasedOnMemory()
            bISDirty = True
        Catch ex As Exception
            'MsgBox("tvwProject_DragDrop - " & GetExceptionInfo(ex))
            Log_Anything("tvwProject_DragDrop - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub tvwProject_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles tvwProject.DragEnter
        Try
            'See if there is a TreeNode being dragged
            If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
                'TreeNode found allow move effect
                e.Effect = DragDropEffects.Move
            Else
                'No TreeNode found, prevent move
                e.Effect = DragDropEffects.None
            End If
        Catch ex As Exception
            Log_Anything("TreeView1_DragEnter - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub tvwProject_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles tvwProject.DragOver
        Try
            'Check that there is a TreeNode being dragged
            If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = False Then Exit Sub

            'Get the TreeView raising the event (incase multiple on form)
            Dim selectedTreeview As TreeView = CType(sender, TreeView)

            'As the mouse moves over nodes, provide feedback to the user
            'by highlighting the node that is the current drop target
            Dim pt As Point = CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
            Dim targetNode As TreeNode = selectedTreeview.GetNodeAt(pt)

            'See if the targetNode is currently selected, if so no need to validate again
            If Not (selectedTreeview Is targetNode) Then
                'Select the node currently under the cursor
                selectedTreeview.SelectedNode = targetNode

                'Check that the selected node is not the dropNode and also that it
                'is not a child of the dropNode and therefore an invalid target
                Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)
                Do Until targetNode Is Nothing
                    If targetNode Is dropNode Then
                        e.Effect = DragDropEffects.None
                        Exit Sub
                    End If
                    targetNode = targetNode.Parent
                Loop
            End If

            'Currently selected node is a suitable target, allow the move
            e.Effect = DragDropEffects.Move
        Catch ex As Exception
            Log_Anything("TreeView1_DragOver - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub TreeView1_ItemDrag(sender As Object, e As System.Windows.Forms.ItemDragEventArgs) Handles tvwMasterTasks.ItemDrag, tvwProject.ItemDrag, tvwPLCs.ItemDrag
        Try
            'Set the drag node and initiate the DragDrop
            'add by chaoyang to fix drag base file from master to project
            If e.Item.tag.ToString <> "MASTERFILE" Then
                DoDragDrop(e.Item, DragDropEffects.Move)
            End If
        Catch ex As Exception
            Log_Anything("TreeView1_ItemDrag - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub btnFindNode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNode.Click

        ClearBackColor()

        Try
            Dim tn() As TreeNode = tvwProject.Nodes(0).Nodes.Find(txtNodeSearch.Text, True)

            Dim i As Integer = 0

            For i = 0 To tn.Length
                tvwProject.SelectedNode = tn(i)
                tvwProject.SelectedNode.BackColor = Color.Yellow
            Next i

        Catch ex As Exception
            Log_Anything("btnFindNodeClick - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub btnNodeTextSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNodeTextSearch.Click
        Try
            ClearBackColor()
            FindByText()
        Catch ex As Exception
            Log_Anything("btnNodeTextSearch_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub btnNodeTagSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNodeTagSearch.Click
        Try
            ClearBackColor()
            FindByTag()
        Catch ex As Exception
            Log_Anything("btnNodeTagSearch_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub SystemSettingsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Try
            Form3.CalledFromForm1 = True
            Form3.CalledFromMainForm = False
            Form3.Show()
        Catch ex As Exception
            Log_Anything("SystemSettingsToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub tmrCheckSystem_Tick(sender As System.Object, e As System.EventArgs) Handles tmrCheckSystem.Tick
        Try
            If String.IsNullOrWhiteSpace(My.Settings.DatabasePath) Or String.IsNullOrWhiteSpace(My.Settings.LogPath) Then
                If lblSystemCheck.BackColor = Color.White Then
                    lblSystemCheck.BackColor = Color.Red
                    lblSystemCheck.ForeColor = Color.Black
                Else
                    lblSystemCheck.BackColor = Color.White
                    lblSystemCheck.ForeColor = Color.Red
                End If
                lblSystemCheck.Text = PromptMsgConstant.SystemSettingIsRequired
            Else
                lblSystemCheck.BackColor = SystemColor
                lblSystemCheck.Text = UiDisplayConstant.SystemOk
                tmrCheckSystem.Enabled = False
            End If
        Catch ex As Exception
            Log_Anything("tmrCheckSystem_Tick - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Try
            AskToSave()
            tvwProject.Nodes.Clear()
            dgvPLCMemory.Rows.Clear()
            Form5.Show()
            Me.Close()
        Catch ex As Exception
            Log_Anything("ExitToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Try
            If PowertrainProjectFile <> "" Then
                'Add Save Logic Here
                SetPlantDataToDatabase()
            End If
        Catch ex As Exception
            Log_Anything("SaveToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub dgvPLCMemory_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPLCMemory.CellClick
        Try
            Dim areaName = dgvPLCMemory.Rows(e.RowIndex).Cells(PlcMemoryGdvColumnNameConstant.AreaName).Value
            Dim sectionName = dgvPLCMemory.Rows(e.RowIndex).Cells(PlcMemoryGdvColumnNameConstant.SectionName).Value
            Dim stationName = dgvPLCMemory.Rows(e.RowIndex).Cells(PlcMemoryGdvColumnNameConstant.StationName).Value

            tvwProject.CollapseAll()

            Dim areaNode = tvwProject.Nodes(0).Nodes.Find(areaName, False).FirstOrDefault()
            If areaNode Is Nothing Then
                Exit Sub
            End If
            Dim sectionNode = areaNode.Nodes.Find(sectionName, False).FirstOrDefault()
            If sectionNode Is Nothing Then
                Exit Sub
            End If
            Dim stationNode = sectionNode.Nodes.Find(stationName, False).FirstOrDefault()
            If stationNode Is Nothing Then
                Exit Sub
            End If
            stationNode.EnsureVisible()
            tvwProject.SelectedNode = stationNode

            Dim plcNode = stationNode.Nodes.Cast(Of Object).FirstOrDefault(Function(x) x.Tag.Equals(TreeNodeTagConstant.PLC))
            If plcNode Is Nothing Then
                Exit Sub
            End If
            Dim plcTreeNode = CType(plcNode, TreeNode)
            plcTreeNode.EnsureVisible()
            tvwProject.SelectedNode = plcTreeNode
        Catch ex As Exception
            Log_Anything("dgvPLCMemory_CellClick - " & GetExceptionInfo(ex))
        End Try

    End Sub


    'recursively move through the treeview nodes
    'and reset backcolors to white
    Private Sub ClearBackColor()
        Try
            Dim nodes As TreeNodeCollection
            nodes = tvwProject.Nodes
            Dim n As TreeNode

            For Each n In nodes
                ClearRecursive(n)
            Next
        Catch ex As Exception
            Log_Anything("ClearBackColor - " & GetExceptionInfo(ex))
        End Try
    End Sub

    'called by ClearBackColor function
    Private Sub ClearRecursive(ByVal treeNode As TreeNode)
        Try
            Dim tn As TreeNode

            For Each tn In treeNode.Nodes

                tn.BackColor = Color.White
                ClearRecursive(tn)
            Next
        Catch ex As Exception
            Log_Anything("ClearRecursive - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub FindByText()
        Try
            Dim nodes As TreeNodeCollection = tvwProject.Nodes
            Dim n As TreeNode
            For Each n In nodes
                FindRecursive(n)
            Next
        Catch ex As Exception
            Log_Anything("FindByText - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub FindRecursive(ByVal tNode As TreeNode)
        Try
            Dim tn As TreeNode
            For Each tn In tNode.Nodes

                ' if the text properties match, color the item
                If tn.Text = Me.txtNodeTextSearch.Text Then
                    tn.BackColor = Color.Yellow
                End If

                FindRecursive(tn)
            Next
        Catch ex As Exception
            Log_Anything("FindRecursive - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub FindByTag()
        Try
            Dim nodes As TreeNodeCollection = tvwProject.Nodes
            Dim n As TreeNode
            For Each n In nodes
                FindRecursiveTag(n)
            Next
        Catch ex As Exception
            Log_Anything("FindByTag - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub FindRecursiveTag(ByVal tNode As TreeNode)
        Try
            Dim tn As TreeNode
            For Each tn In tNode.Nodes

                ' if the text properties match, color the item
                If tn.Tag.ToString() = Me.txtTagSearch.Text Then
                    tn.BackColor = Color.Yellow
                End If

                FindRecursiveTag(tn)
            Next
        Catch ex As Exception
            Log_Anything("FindRecursiveTag - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Public Sub SetFormTitle()
        Try
            'Add Form Title and Version
            Me.Text = CommonController.GenerateProductionTitle(PowertrainProjectFile)
            tvwProject.ExpandAll()
        Catch ex As Exception
            Log_Anything("SetFormTitle - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Public Sub CheckSystemSettings()
        Try
            tvwProject.Nodes.Clear()
            dgvPLCMemory.Rows.Clear()

            If String.IsNullOrWhiteSpace(My.Settings.LogPath) Or String.IsNullOrWhiteSpace(My.Settings.DatabasePath) Then
                tvwProject.Enabled = False
                tmrCheckSystem.Enabled = True
                Exit Sub
            Else
                tvwProject.Enabled = True
                lblSystemCheck.BackColor = SystemColor
                lblSystemCheck.Text = UiDisplayConstant.SystemOk
                lblSystemCheck.ForeColor = Color.Black
                tmrCheckSystem.Enabled = False
                GeneratePlcTreeViewNodes()
                GenerateMasterFilesTreeViewNodes()
                bISDirty = False
                tvwPLCs.Nodes(0).Expand()
                tvwMasterTasks.Nodes(0).Expand()
            End If
        Catch ex As Exception
            Log_Anything("CheckSystemSettings - " & GetExceptionInfo(ex))
        End Try

    End Sub

    ''' <summary>
    ''' Add plc and plc attribute node to plc tree view
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GeneratePlcTreeViewNodes()
        Try
            tvwPLCs.Nodes.Clear()
            Dim plcMainNode = TreeHierarchyController.GenerateTreeNode(UiDisplayConstant.MainNode, UiDisplayConstant.Processors, String.Empty, TreeNodeImageIndexConstant.Plc, TreeNodeImageIndexConstant.Plc)
            Me.tvwPLCs.Nodes.Add(plcMainNode)
            tvwPLCs.Sort()

            Dim plcInfoList = plcInfoControllerMaster.GetPlcInformation()
            For Each plcInfo In plcInfoList
                AddPLCToTree(plcInfo)
                AddPLCAttributeToTree(plcInfo.ProcessorType, "1. Total Memory - " & plcInfo.TotalBytesAvailable)
                AddPLCAttributeToTree(plcInfo.ProcessorType, "2. Max Nodes - " & plcInfo.MaxNodes)
                AddPLCAttributeToTree(plcInfo.ProcessorType, "3. Max Conns - " & plcInfo.MaxConnections)
                If Not String.IsNullOrWhiteSpace(plcInfo.ApplicationNotes) Then
                    AddPLCAttributeToTree(plcInfo.ProcessorType, "4. Application Notes - " & plcInfo.ApplicationNotes)
                End If
            Next
        Catch ex As Exception
            Log_Anything("GetPLCInformation - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub CheckAllNodesForMemoryUsage()
        Try
            Dim intTotalPLCMemory As Integer
            Dim intTotalReservedPLCMemory As Integer
            Dim intMaxConnections As Integer
            Dim intMaxNodes As Integer
            Dim strTempPLCArray() As String

            Dim intStationCount As Integer
            Dim intStationMemoryUsage As Integer
            Dim intStationConnections As Integer
            Dim intStationNodes As Integer
            Dim intMasterFileBaseUsage As Integer

            Dim strPLCName As String = "Not Selected"

            Dim strTempMasterFileInfoArray() As String
            Dim row As String() = New String() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"}

            intStationCount = 1
            dgvPLCMemory.Rows.Clear()

            For Each AreaNode In tvwProject.Nodes(0).Nodes
                For Each SectionNode In AreaNode.Nodes

                    For Each StationNode In SectionNode.Nodes
                        strPLCName = "Not Selected"
                        intTotalPLCMemory = 0
                        intTotalReservedPLCMemory = 0
                        Dim plcNode = CType(StationNode, TreeNode).Nodes.Cast(Of Object).FirstOrDefault(Function(x) x.Tag.Equals(TreeNodeTagConstant.PLC))
                        If plcNode IsNot Nothing Then
                            intStationMemoryUsage = 0
                            intStationConnections = 0
                            intStationNodes = 0
                            Dim plcInfo = plcInfoControllerMaster.GetPlcInformationByProcessorType(plcNode.Name)
                            If plcInfo IsNot Nothing Then
                                strPLCName = plcNode.Name
                                intTotalPLCMemory = plcInfo.TotalBytesAvailable
                                intMaxNodes = plcInfo.MaxNodes
                                intMaxConnections = plcInfo.MaxConnections
                                'intTotalReservedPLCMemory = (intTotalPLCMemory * (My.Settings.PLCMemoryPercentToReserve / 100))
                                If StationNode.ToolTipText = "Auto" Then
                                    intTotalReservedPLCMemory = My.Settings.PLCMemoryToReserveAWS
                                Else
                                    intTotalReservedPLCMemory = My.Settings.PLCMemoryToReserveMWS
                                End If
                            End If
                        End If

                        intMasterFileBaseUsage = 0
                        For Each MasterFileNode In StationNode.Nodes
                            If MasterFileNode.Tag = "MASTERFILE" Then
                                Dim mfName = MasterFileNode.Name
                                Dim mf = mfControllerMaster.GetMasterFileByName(mfName)
                                intMasterFileBaseUsage = mf.OverheadMemoryBase
                            End If
                        Next

                        row = New String() {intStationCount, AreaNode.Name, SectionNode.Name, StationNode.Name, StationNode.ToolTipText, strPLCName, intTotalPLCMemory, intTotalReservedPLCMemory, intTotalPLCMemory - intMasterFileBaseUsage - intTotalReservedPLCMemory, intMasterFileBaseUsage}
                        dgvPLCMemory.Rows.Add(row)
                        dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(8).Style.BackColor = Color.YellowGreen

                        If intStationMemoryUsage + intMasterFileBaseUsage >= intTotalPLCMemory * (My.Settings.MemoryFault / 100) Then
                            StationNode.backColor = Color.Red
                            dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(8).Style.BackColor = Color.Red
                            For cellNo = 0 To 9
                                dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(cellNo).Style.BackColor = Color.Red
                            Next
                        ElseIf intStationMemoryUsage + intMasterFileBaseUsage >= intTotalPLCMemory * (My.Settings.MemoryWarning / 100) Then
                            StationNode.backColor = Color.Yellow
                            dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(8).Style.BackColor = Color.Yellow
                            For cellNo = 0 To 9
                                dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(cellNo).Style.BackColor = Color.Yellow
                            Next
                        Else
                            StationNode.backColor = Color.White
                            dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(8).Style.BackColor = Color.YellowGreen
                            For cellNo = 0 To 9
                                dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(cellNo).Style.BackColor = Color.YellowGreen
                            Next
                        End If

                        For Each MasterFileNode In StationNode.Nodes
                            If MasterFileNode.Tag = "MASTERFILE" Then
                                Dim mfName = MasterFileNode.Name
                                Dim mfTaskList = mfTaskControllerMaster.GetMasterFileTasksByMfNameAndMultiStation(mfName, 0)

                                For Each TaskNode In MasterFileNode.Nodes
                                    If TaskNode.Tag = "TASK" Then
                                        Dim mfTask = mfTaskList.FirstOrDefault(Function(x) x.TaskName.Equals(TaskNode.Name))
                                        If mfTask IsNot Nothing Then
                                            intStationMemoryUsage = mfTask.MemoryUsed + intStationMemoryUsage
                                            intStationConnections = mfTask.Version + intStationConnections 'Why???
                                            intStationNodes = mfTask.MultiStation + intStationNodes

                                            dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(8).Value = intTotalPLCMemory - (intStationMemoryUsage + intMasterFileBaseUsage)
                                            dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(9).Value = intStationMemoryUsage + intMasterFileBaseUsage

                                            If intStationMemoryUsage + intMasterFileBaseUsage >= intTotalPLCMemory * (My.Settings.MemoryFault / 100) Then
                                                StationNode.backColor = Color.Red
                                                For cellNo = 0 To 9
                                                    dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(cellNo).Style.BackColor = Color.Red
                                                Next
                                            ElseIf intStationMemoryUsage + intMasterFileBaseUsage >= intTotalPLCMemory * (My.Settings.MemoryWarning / 100) Then
                                                StationNode.backColor = Color.Yellow
                                                For cellNo = 0 To 9
                                                    dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(cellNo).Style.BackColor = Color.Yellow
                                                Next
                                            Else
                                                StationNode.backColor = Color.White
                                                For cellNo = 0 To 9
                                                    dgvPLCMemory.Rows(dgvPLCMemory.Rows.Count - 2).Cells(cellNo).Style.BackColor = Color.YellowGreen
                                                Next
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        Next
                        intStationCount += 1
                    Next
                Next
            Next
        Catch ex As Exception
            Log_Anything("CheckAllNodesForMemoryUsage - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub SetupDataGridView()
        Try
            dgvPLCMemory.ColumnCount = 10

            dgvPLCMemory.Columns(0).HeaderText = PlcMemoryGridViewHeaderConstant.No
            dgvPLCMemory.Columns(1).HeaderText = PlcMemoryGridViewHeaderConstant.AreaName
            dgvPLCMemory.Columns(2).HeaderText = PlcMemoryGridViewHeaderConstant.SectionName
            dgvPLCMemory.Columns(3).HeaderText = PlcMemoryGridViewHeaderConstant.StationName
            dgvPLCMemory.Columns(4).HeaderText = PlcMemoryGridViewHeaderConstant.StationType
            dgvPLCMemory.Columns(5).HeaderText = PlcMemoryGridViewHeaderConstant.PlcType
            dgvPLCMemory.Columns(6).HeaderText = PlcMemoryGridViewHeaderConstant.TotalPlcMem
            dgvPLCMemory.Columns(7).HeaderText = PlcMemoryGridViewHeaderConstant.TotalRsvdMem
            dgvPLCMemory.Columns(8).HeaderText = PlcMemoryGridViewHeaderConstant.MemAvailable
            dgvPLCMemory.Columns(9).HeaderText = PlcMemoryGridViewHeaderConstant.MemUsed

            dgvPLCMemory.Columns(0).Name = PlcMemoryGdvColumnNameConstant.No
            dgvPLCMemory.Columns(1).Name = PlcMemoryGdvColumnNameConstant.AreaName
            dgvPLCMemory.Columns(2).Name = PlcMemoryGdvColumnNameConstant.SectionName
            dgvPLCMemory.Columns(3).Name = PlcMemoryGdvColumnNameConstant.StationName
            dgvPLCMemory.Columns(4).Name = PlcMemoryGdvColumnNameConstant.StationType
            dgvPLCMemory.Columns(5).Name = PlcMemoryGdvColumnNameConstant.PlcType
            dgvPLCMemory.Columns(6).Name = PlcMemoryGdvColumnNameConstant.TotalPlcMem
            dgvPLCMemory.Columns(7).Name = PlcMemoryGdvColumnNameConstant.TotalRsvdMem
            dgvPLCMemory.Columns(8).Name = PlcMemoryGdvColumnNameConstant.MemAvailable
            dgvPLCMemory.Columns(9).Name = PlcMemoryGdvColumnNameConstant.MemUsed

            dgvPLCMemory.Columns(0).Width = 40
            dgvPLCMemory.Columns(1).Width = 100
            dgvPLCMemory.Columns(2).Width = 100
            dgvPLCMemory.Columns(3).Width = 100
            dgvPLCMemory.Columns(4).Width = 100
            dgvPLCMemory.Columns(5).Width = 100
            dgvPLCMemory.Columns(6).Width = 75
            dgvPLCMemory.Columns(7).Width = 75
            dgvPLCMemory.Columns(8).Width = 75
            dgvPLCMemory.Columns(9).Width = 75

        Catch ex As Exception
            Log_Anything("SetupDatagridView - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub GetPlantDataFromDatabase()

        Try
            tvwProject.Nodes.Clear()
            Dim plantName = plantController.GetPlantName()
            Dim mainNode = TreeHierarchyController.GenerateTreeNode(UiDisplayConstant.MainNode, plantName, TreeNodeTagConstant.PLANT, TreeNodeImageIndexConstant.Plant, TreeNodeImageIndexConstant.Plant)
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
                AddStationToTree(station.AreaName, station.SectionName, station.StationName, station.StationType)
                If Not String.IsNullOrWhiteSpace(station.PlcType) Then
                    AddPLCToProjectTree(station.AreaName, station.SectionName, station.StationName, station.PlcType)

                    Dim plcInfo = plcInfoControllerMaster.GetPlcInformationByProcessorType(station.PlcType)
                    If plcInfo IsNot Nothing Then
                        AddPLCAttributeToProjectTree(station.AreaName, station.SectionName, station.StationName, station.PlcType, "1. Total Memory - " & plcInfo.TotalBytesAvailable)
                        AddPLCAttributeToProjectTree(station.AreaName, station.SectionName, station.StationName, station.PlcType, "2. Max Nodes - " & plcInfo.MaxNodes)
                        AddPLCAttributeToProjectTree(station.AreaName, station.SectionName, station.StationName, station.PlcType, "3. Max Conns - " & plcInfo.MaxConnections)
                    End If

                    If Not String.IsNullOrWhiteSpace(station.MasterFileName) Then
                        AddMasterFileToProjectTree(station.AreaName, station.SectionName, station.StationName, station.MasterFileName)
                    End If
                End If
            Next

            Dim taskList = taskController.GetListTasks()
            For Each task In taskList
                AddTaskToProjectTree(task.AreaName, task.SectionName, task.StationName, task.MasterFileName, task.TaskName)
            Next

            tvwProject.Refresh()
            Me.Refresh()

        Catch ex As Exception
            Log_Anything("GetPlantDataFromDatabase - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub SetPlantDataToDatabase()

        Try
            'delete orginal data
            DeleteOriginalData()
            'save the new data
            InsertPlantData()

            GetPlantDataFromDatabase()
            CheckAllNodesForMemoryUsage()
            tvwProject.ExpandAll()
            'tvwProject.Nodes(0).Expand()
            bISDirty = False
        Catch ex As Exception
            Log_Anything("SetPlantDataToDatabase - " & GetExceptionInfo(ex))
        End Try
    End Sub

    ''' <summary>
    ''' Delete original data. This way is not suitable for some special cases. If the next step 'save' throw exception, original data is lost already.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteOriginalData()
        plantController.DeleteAllPlant()
        areaController.DeleteAllAreas()
        sectionController.DeleteAllSections()
        stationsController.DeleteAllStations()
        taskController.DeleteAllTasks()
    End Sub

    ''' <summary>
    ''' Save the plant data. Contains: Plant, Area, Section, Station and Task
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InsertPlantData()
        Dim plantTreeNode = tvwProject.Nodes(0)
        Dim plantObj = New Plant(plantTreeNode.Text)
        plantController.InsertPlant(plantObj)

        For Each areaTreeNode In plantTreeNode.Nodes
            Dim areaObj = New Areas(CType(areaTreeNode, TreeNode).Text)
            InsertAreaData(areaTreeNode, areaObj)
        Next
    End Sub

    Private Sub InsertAreaData(areaTreeNode As TreeNode, areaObj As Areas)
        areaController.InsertAreas(areaObj)

        For Each sectionTreeNode In areaTreeNode.Nodes
            Dim sectionObj = New SectionModel(areaTreeNode.Text, CType(sectionTreeNode, TreeNode).Text)
            InsertSectionData(sectionTreeNode, sectionObj)
        Next
    End Sub

    Private Sub InsertSectionData(sectionTreeNode As TreeNode, sectionObj As SectionModel)
        sectionController.InsertSection(sectionObj)

        For Each stationTreeNode In sectionTreeNode.Nodes
            Dim plcNode = CType(stationTreeNode, TreeNode).Nodes.Cast(Of Object).FirstOrDefault(Function(x) CType(x, TreeNode).Tag.Equals(TreeNodeTagConstant.PLC))
            Dim plcName = IIf(plcNode Is Nothing, String.Empty, CType(plcNode, TreeNode).Name)
            Dim masterFileNode = CType(stationTreeNode, TreeNode).Nodes.Cast(Of Object).FirstOrDefault(Function(x) CType(x, TreeNode).Tag.Equals(TreeNodeTagConstant.MASTERFILE))
            Dim masterFileName = IIf(masterFileNode Is Nothing, String.Empty, CType(masterFileNode, TreeNode).Name)

            Dim stationObj = New StationModel()
            stationObj.AreaName = sectionObj.AreaName
            stationObj.SectionName = sectionObj.SectionName
            stationObj.StationName = CType(stationTreeNode, TreeNode).Text
            stationObj.StationType = CType(stationTreeNode, TreeNode).ToolTipText
            stationObj.PlcType = plcName
            stationObj.MasterFileName = masterFileName
            'If the string type field is null, can't insert it to db
            stationObj.MasterFileRevision = String.Empty
            stationObj.ModelAffiliation = String.Empty

            InsertStationData(stationTreeNode, stationObj)
        Next
    End Sub

    Private Sub InsertStationData(stationTreeNode As TreeNode, stationObj As StationModel)
        stationsController.InsertStation(stationObj)

        Dim masterFileNode = stationTreeNode.Nodes.Cast(Of Object).FirstOrDefault(Function(x) CType(x, TreeNode).Tag.Equals(TreeNodeTagConstant.MASTERFILE))
        If masterFileNode Is Nothing Then
            Exit Sub
        End If
        Dim taskNodeList = CType(masterFileNode, TreeNode).Nodes.Cast(Of Object).Where(Function(x) CType(x, TreeNode).Tag.Equals(TreeNodeTagConstant.TASK)).ToList()
        Dim mfTaskList = mfTaskControllerMaster.GetMasterFileTasks()

        For Each taskTreeNode In taskNodeList
            Dim taskObj = New TaskModel()
            taskObj.AreaName = stationObj.AreaName
            taskObj.SectionName = stationObj.SectionName
            taskObj.StationName = stationObj.StationName
            taskObj.MasterFileName = CType(masterFileNode, TreeNode).Name
            taskObj.TaskName = CType(taskTreeNode, TreeNode).Name

            Dim mfTask = mfTaskList.FirstOrDefault(Function(x) ("_" & x.MasterFileName).Equals(CType(masterFileNode, TreeNode).Text) AndAlso
                                                               x.TaskName.Equals(CType(taskTreeNode, TreeNode).Name))
            If mfTask IsNot Nothing Then
                taskObj.TaskMemory = mfTask.MemoryUsed
                taskObj.TaskMemoryPlus = mfTask.Version
                taskObj.MaxNoOfInstances = mfTask.MaxNoOfInstances
                taskObj.ModelAffiliation = mfTask.ModelAffiliation
            Else
                'If string type field is null, can't insert it to db
                taskObj.TaskMemory = String.Empty
                taskObj.ModelAffiliation = String.Empty
            End If

            InsertTaskData(taskObj)
        Next
    End Sub

    Private Sub InsertTaskData(taskObj As TaskModel)
        taskController.InsertTask(taskObj)
    End Sub

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

        Catch ex As Exception
            Log_Anything("AddSectionToTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub AddStationToTree(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strStationType As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName, strSectionName, strStationName)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.Station, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodeStation(hierarchyValidationDetail)
            If String.IsNullOrWhiteSpace(strStationType) Then strStationType = StationTypeConstant.Auto
            nod.ToolTipText = strStationType

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

    Private Sub AddPLCAttributeToProjectTree(ByVal strAreaName As String, ByVal strSectionName As String, ByVal strStationName As String, ByVal strPLCName As String, ByVal strPLCAttributeName As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(strAreaName, strSectionName, strStationName, DefaultValueConstant.TreeNodeHierarchy, DefaultValueConstant.TreeNodeHierarchy, strPLCName, strPLCAttributeName)
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.ProjectFileTree, TreeNodeHierarchyType.PlcAttribute, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodePlcAttribute(hierarchyValidationDetail)

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
            Log_Anything("AddPLCAttributeToProjectTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub AddPLCToTree(ByVal plcInfo As PlcInformation)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(True)
            hierarchyValidationDetail.PlcName = plcInfo.ProcessorType
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.PlcTree, TreeNodeHierarchyType.Plc, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodePlc(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwPLCs, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
            tvwPLCs.Sort()

        Catch ex As Exception
            Log_Anything("AddPLCToTree - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub AddPLCAttributeToTree(ByVal strPLCName As String, strPLCItem As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(True)
            hierarchyValidationDetail.PlcName = strPLCName
            hierarchyValidationDetail.PlcAttributeName = strPLCItem
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.PlcTree, TreeNodeHierarchyType.PlcAttribute, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodePlcAttribute(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwPLCs, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)

        Catch ex As Exception
            Log_Anything("AddPLCAttributeToTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    ''' <summary>
    ''' Generate master file tree view nodes
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GenerateMasterFilesTreeViewNodes()
        Try
            tvwMasterTasks.Nodes.Clear()
            Dim mfMainNode = TreeHierarchyController.GenerateTreeNode(UiDisplayConstant.MainNode, UiDisplayConstant.MasterFiles, String.Empty, TreeNodeImageIndexConstant.MasterFile, TreeNodeImageIndexConstant.MasterFile)
            tvwMasterTasks.Nodes.Add(mfMainNode)
            tvwMasterTasks.Sort()

            Dim mfList = mfControllerMaster.GetMasterFiles()
            For Each mf In mfList
                AddMasterFileToMasterFileTree(mf.MasterFileName)
                AddMasterFileAttributeToMasterFileTree(mf.MasterFileName, "1. Version:=" & mf.MasterFileRevision)
                AddMasterFileAttributeToMasterFileTree(mf.MasterFileName, "2. Base Memory Usage:=" & mf.OverheadMemoryBase)

                Dim mfTaskList = mfTaskControllerMaster.GetMasterFileTasksByMfNameAndMultiStation(mf.MasterFileName, 0)
                For Each mfTask In mfTaskList
                    AddTaskToMasterFileTree(mfTask.MasterFileName, mfTask.TaskName)
                Next
            Next
        Catch ex As Exception
            Log_Anything("GetMasterFiles - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub AddMasterFileToMasterFileTree(mfName As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(True)
            hierarchyValidationDetail.MasterFileName = mfName
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.MasterFileTree, TreeNodeHierarchyType.MasterFile, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNode(mfName, mfName, TreeNodeTagConstant.MASTERFILE, TreeNodeImageIndexConstant.MasterFileSub, TreeNodeImageIndexConstant.MasterFileSub)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwMasterTasks, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
        Catch ex As Exception
            Log_Anything("AddMasterFileToMasterFileTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub AddTaskToMasterFileTree(mfName As String, taskName As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(True)
            hierarchyValidationDetail.MasterFileName = mfName
            hierarchyValidationDetail.TaskName = taskName
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.MasterFileTree, TreeNodeHierarchyType.Task, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNode(taskName, taskName, TreeNodeTagConstant.TASK, TreeNodeImageIndexConstant.Task, TreeNodeImageIndexConstant.Task)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwMasterTasks, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)
        Catch ex As Exception
            Log_Anything("AddTaskToMasterFileTree - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub AddMasterFileAttributeToMasterFileTree(strMasterFileName As String, strMasterFileAttribute As String)
        Try
            Dim hierarchyValidationDetail = New TreeNodeHierarchyValDetail(True)
            hierarchyValidationDetail.MasterFileName = strMasterFileName
            hierarchyValidationDetail.MasterFileAttributeName = strMasterFileAttribute
            Dim hierarchyValidation = New TreeNodeHierarchyValidation(TreeType.MasterFileTree, TreeNodeHierarchyType.MasterFileAttribute, hierarchyValidationDetail)

            'generate node
            Dim nod = TreeHierarchyController.GenerateTreeNodeMasterFileAttribute(hierarchyValidationDetail)

            'validate
            Dim valMsg = String.Empty
            Dim parentNode = New TreeNode()
            Dim valResult = TreeHierarchyController.ValidateTreeHierarchy(hierarchyValidation, tvwMasterTasks, valMsg, parentNode)
            If valResult = False Then
                If Not String.IsNullOrWhiteSpace(valMsg) Then
                    MsgBox(valMsg, MsgBoxStyle.Critical, Application.ProductName)
                End If
                Exit Sub
            End If

            'add node
            parentNode.Nodes.Add(nod)

        Catch ex As Exception
            Log_Anything("AddMasterFileAttributeToMasterFileTree - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub MemoryUsageToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MemoryUsageToolStripMenuItem.Click
        Try
            CheckAllNodesForMemoryUsage()
            FillDataTableWithMemoryInformation()
            Form4.strPlantName = tvwProject.Nodes(0).Text
            Form4.ReportViewer1.RefreshReport()
            Form4.Show()
        Catch ex As Exception
            Log_Anything("MemoryUsageToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub FillDataTableWithMemoryInformation()
        Try
            'delete the original memory usages
            muControllerMaster.DeleteMemoryUsages()

            'insert the current memory usages
            Dim muList = New List(Of MemoryUsage)
            For i = 0 To dgvPLCMemory.RowCount - 2
                Dim areaName = dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.AreaName).Value.ToString() 'try to use name to get the value
                Dim sectionName = dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.SectionName).Value.ToString()
                Dim stationName = dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.StationName).Value.ToString()
                Dim plcType = dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.PlcType).Value.ToString()
                Dim totalMem = Convert.ToInt32(dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.TotalPlcMem).Value.ToString())
                Dim totalMemRsvd = Convert.ToInt32(dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.TotalRsvdMem).Value.ToString())
                Dim memAvailable = Convert.ToInt32(dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.MemAvailable).Value.ToString())
                Dim memUsed = Convert.ToInt32(dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.MemUsed).Value.ToString())

                Dim totalPlcReserverdMemory = IIf(dgvPLCMemory.Rows(i).Cells(PlcMemoryGdvColumnNameConstant.StationType).Value.ToString().Equals(StationTypeConstant.Auto), My.Settings.PLCMemoryToReserveAWS, My.Settings.PLCMemoryToReserveMWS)
                Dim plcMemoryAfterReserved = (totalMem - totalPlcReserverdMemory)
                Dim totalPlcPercentAvailable = (memAvailable / plcMemoryAfterReserved) * 100
                Dim totalPlcPercentUsed = 100 - totalPlcPercentAvailable

                Dim muObj = New MemoryUsage(areaName, sectionName, stationName, plcType, totalMem, totalMemRsvd, memAvailable, memUsed, totalPlcPercentAvailable, totalPlcPercentUsed)
                muList.Add(muObj)
            Next
            muControllerMaster.InsertMemoryUsages(muList)
        Catch ex As Exception
            Log_Anything("FillDataTableWithMemoryInformation - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Function OpenProjectFile() As Boolean

        OpenFileDialog1.Filter = "Powertrain Project File (*.ppf)|*.ppf"
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
                SetPlantDataToDatabase()

                SaveAsProjectFile = True
            Else
                SaveAsProjectFile = False
            End If
        Catch ex As Exception
            Log_Anything("SaveAsProjectFile - " & GetExceptionInfo(ex))
            SaveAsProjectFile = False
        End Try
    End Function

    Private Sub AskToSave()
        Try
            Dim nResult As DialogResult

            If bISDirty = True Then
                nResult = MsgBox("Changes have been made and not saved. " & vbCrLf & "Do you want to save your changes?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                If nResult = 6 Then
                    SetPlantDataToDatabase()
                End If
                bISDirty = False
            End If
        Catch ex As Exception
            Log_Anything("AskToSave - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub ExpandTreeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExpandTreeToolStripMenuItem.Click
        tvwProject.ExpandAll()
    End Sub

    Private Sub CollapseTreeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CollapseTreeToolStripMenuItem.Click
        tvwProject.CollapseAll()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Try
            Dim bFileOpen As Boolean
            OpenFileDialog1.Title = "Open Exisiting XXX Powertrain Project File"
            OpenFileDialog1.CheckFileExists = True
            bFileOpen = OpenProjectFile()
            If bFileOpen Then
                'Get the Plant Data that is saved in the Database
                GetPlantDataFromDatabase()
                bISDirty = False
                tvwProject.SelectedNode = tvwProject.Nodes(0)
                'Run the Memory Check Routine to check all memory usage
                CheckAllNodesForMemoryUsage()
                'Set the Forms Title
                SetFormTitle()
            End If
        Catch ex As Exception
            Log_Anything("OpenToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Dim bFileOpen As Boolean
        Dim strFileExists As String
        Dim Response As Integer
        Try

            OpenFileDialog1.Title = "Create NEW XXX Powertrain Project File"
            OpenFileDialog1.CheckFileExists = False
            'OpenFileDialog1.
            bFileOpen = OpenProjectFile()
            If bFileOpen Then
                strFileExists = Dir(PowertrainProjectFile)
                If strFileExists = "" Then
                    FileCopy(My.Settings.DatabasePath, PowertrainProjectFile)
                Else
                    Response = MsgBox("File already exists. OK to Overwrite?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Application.ProductName)
                    If Response = 6 Then
                        FileCopy(My.Settings.DatabasePath, PowertrainProjectFile)
                    Else
                        Exit Sub
                    End If
                End If

                'Set the Forms Title
                SetFormTitle()
                'Get the Plant Data that is saved in the Database
                GetPlantDataFromDatabase()
                bISDirty = False
                tvwProject.SelectedNode = tvwProject.Nodes(0)
                'Run the Memory Check Routine to check all memory usage
                CheckAllNodesForMemoryUsage()
            End If
        Catch ex As Exception
            Log_Anything("NewToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub CloseProjectToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CloseProjectToolStripMenuItem.Click
        Try
            If PowertrainProjectFile <> "" Then
                AskToSave()
            End If
            PowertrainProjectFile = ""
            tvwProject.Nodes.Clear()
            dgvPLCMemory.Rows.Clear()
        Catch ex As Exception
            Log_Anything("CloseProjectToolStripMenuItem_Click - " & GetExceptionInfo(ex))
        End Try
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

    Private Sub KeepLastFileNameOpened(strLastFileName As String)
        Try
            If strLastFileName = My.Settings.LastTaskFile01 Then

            ElseIf strLastFileName = My.Settings.LastTaskFile02 Then
                My.Settings.LastTaskFile02 = My.Settings.LastTaskFile01
                My.Settings.LastTaskFile01 = strLastFileName

            ElseIf strLastFileName = My.Settings.LastTaskFile03 Then
                My.Settings.LastTaskFile03 = My.Settings.LastTaskFile02
                My.Settings.LastTaskFile02 = My.Settings.LastTaskFile01
                My.Settings.LastTaskFile01 = strLastFileName

            ElseIf strLastFileName = My.Settings.LastTaskFile04 Then
                My.Settings.LastTaskFile04 = My.Settings.LastTaskFile03
                My.Settings.LastTaskFile03 = My.Settings.LastTaskFile02
                My.Settings.LastTaskFile02 = My.Settings.LastTaskFile01
                My.Settings.LastTaskFile01 = strLastFileName

            Else
                My.Settings.LastTaskFile04 = My.Settings.LastTaskFile03
                My.Settings.LastTaskFile03 = My.Settings.LastTaskFile02
                My.Settings.LastTaskFile02 = My.Settings.LastTaskFile01
                My.Settings.LastTaskFile01 = strLastFileName
            End If

            My.Settings.Save()
            SetupPreviousFilesMenu()
        Catch ex As Exception
            Log_Anything("KeepLastFileNameOpened - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub SetupPreviousFilesMenu()
        Dim intPosition As Integer

        Try
            PreviousFileOpen_01.Visible = False
            PreviousFileOpen_02.Visible = False
            PreviousFileOpen_03.Visible = False
            PreviousFileOpen_04.Visible = False

            If My.Settings.LastTaskFile01 <> "" Then
                PreviousFileOpen_01.Visible = True
                intPosition = InStrRev(My.Settings.LastTaskFile01, "\")
                PreviousFileOpen_01.Text = Mid(My.Settings.LastTaskFile01, intPosition + 1, Len(My.Settings.LastTaskFile01) - intPosition)
                PreviousFileOpen_01.Tag = My.Settings.LastTaskFile01
            End If


            If My.Settings.LastTaskFile02 <> "" Then
                PreviousFileOpen_02.Visible = True
                intPosition = InStrRev(My.Settings.LastTaskFile02, "\")
                PreviousFileOpen_02.Text = Mid(My.Settings.LastTaskFile02, intPosition + 1, Len(My.Settings.LastTaskFile02) - intPosition)
                PreviousFileOpen_02.Tag = My.Settings.LastTaskFile02
            End If

            If My.Settings.LastTaskFile03 <> "" Then
                PreviousFileOpen_03.Visible = True
                intPosition = InStrRev(My.Settings.LastTaskFile03, "\")
                PreviousFileOpen_03.Text = Mid(My.Settings.LastTaskFile03, intPosition + 1, Len(My.Settings.LastTaskFile03) - intPosition)
                PreviousFileOpen_03.Tag = My.Settings.LastTaskFile03
            End If


            If My.Settings.LastTaskFile04 <> "" Then
                PreviousFileOpen_04.Visible = True
                intPosition = InStrRev(My.Settings.LastTaskFile04, "\")
                PreviousFileOpen_04.Text = Mid(My.Settings.LastTaskFile04, intPosition + 1, Len(My.Settings.LastTaskFile04) - intPosition)
                PreviousFileOpen_04.Tag = My.Settings.LastTaskFile04
            End If
        Catch ex As Exception
            Log_Anything("SetupPreviousFilesMenu - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub PreviousFileOpen_01_Click(sender As System.Object, e As System.EventArgs) Handles PreviousFileOpen_01.Click
        Dim strFileOK As String

        Try
            strFileOK = Dir(My.Settings.LastTaskFile01)
            If strFileOK <> "" Then
                If PowertrainProjectFile <> "" Then
                    AskToSave()
                End If
                PowertrainProjectFile = My.Settings.LastTaskFile01
                If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                    PowertrainProjectFile = PowertrainProjectFile & ".ppf"
                End If
                KeepLastFileNameOpened(PowertrainProjectFile)

                'Init all controllers according to current project file
                InitAllControllers()

                'Get the Plant Data that is saved in the Database
                GetPlantDataFromDatabase()
                bISDirty = False
                tvwProject.SelectedNode = tvwProject.Nodes(0)
                'Run the Memory Check Routine to check all memory usage
                CheckAllNodesForMemoryUsage()
                'Set the Forms Title
                SetFormTitle()
            Else
                MsgBox("File " & My.Settings.LastTaskFile01 & " does not exist.", MsgBoxStyle.Information, Application.ProductName)
                My.Settings.LastTaskFile01 = My.Settings.LastTaskFile02
                My.Settings.LastTaskFile02 = My.Settings.LastTaskFile03
                My.Settings.LastTaskFile03 = My.Settings.LastTaskFile04
                My.Settings.LastTaskFile04 = ""
                My.Settings.Save()
                SetupPreviousFilesMenu()
            End If
        Catch ex As Exception
            Log_Anything("PreviousFileOpen_01_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub PreviousFileOpen_02_Click(sender As System.Object, e As System.EventArgs) Handles PreviousFileOpen_02.Click
        Dim strFileOK As String

        Try

            strFileOK = Dir(My.Settings.LastTaskFile02)
            If strFileOK <> "" Then
                If PowertrainProjectFile <> "" Then
                    AskToSave()
                End If
                PowertrainProjectFile = My.Settings.LastTaskFile02
                If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                    PowertrainProjectFile = PowertrainProjectFile & ".ppf"
                End If
                KeepLastFileNameOpened(PowertrainProjectFile)

                'Init all controllers according to current project file
                InitAllControllers()

                'Get the Plant Data that is saved in the Database
                GetPlantDataFromDatabase()
                bISDirty = False
                tvwProject.SelectedNode = tvwProject.Nodes(0)
                'Run the Memory Check Routine to check all memory usage
                CheckAllNodesForMemoryUsage()
                'Set the Forms Title
                SetFormTitle()
            Else
                MsgBox("File " & My.Settings.LastTaskFile02 & " does not exist.", MsgBoxStyle.Information, Application.ProductName)
                My.Settings.LastTaskFile02 = My.Settings.LastTaskFile03
                My.Settings.LastTaskFile03 = My.Settings.LastTaskFile04
                My.Settings.LastTaskFile04 = ""
                My.Settings.Save()
                SetupPreviousFilesMenu()
            End If
        Catch ex As Exception
            Log_Anything("PreviousFileOpen_02_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub PreviousFileOpen_03_Click(sender As System.Object, e As System.EventArgs) Handles PreviousFileOpen_03.Click
        Dim strFileOK As String

        Try
            strFileOK = Dir(My.Settings.LastTaskFile03)
            If strFileOK <> "" Then
                If PowertrainProjectFile <> "" Then
                    AskToSave()
                End If
                PowertrainProjectFile = My.Settings.LastTaskFile03
                If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                    PowertrainProjectFile = PowertrainProjectFile & ".ppf"
                End If
                KeepLastFileNameOpened(PowertrainProjectFile)

                'Init all controllers according to current project file
                InitAllControllers()

                'Get the Plant Data that is saved in the Database
                GetPlantDataFromDatabase()
                bISDirty = False
                tvwProject.SelectedNode = tvwProject.Nodes(0)
                'Run the Memory Check Routine to check all memory usage
                CheckAllNodesForMemoryUsage()
                'Set the Forms Title
                SetFormTitle()
            Else
                MsgBox("File " & My.Settings.LastTaskFile03 & " does not exist.", MsgBoxStyle.Information, Application.ProductName)
                My.Settings.LastTaskFile03 = My.Settings.LastTaskFile04
                My.Settings.LastTaskFile04 = ""
                My.Settings.Save()
                SetupPreviousFilesMenu()
            End If
        Catch ex As Exception
            Log_Anything("PreviousFileOpen_03_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub PreviousFileOpen_04_Click(sender As System.Object, e As System.EventArgs) Handles PreviousFileOpen_04.Click
        Dim strFileOK As String

        Try
            strFileOK = Dir(My.Settings.LastTaskFile04)
            If strFileOK <> "" Then
                If PowertrainProjectFile <> "" Then
                    AskToSave()
                End If
                PowertrainProjectFile = My.Settings.LastTaskFile04
                If Mid(PowertrainProjectFile, Len(PowertrainProjectFile) - 3, 4) <> ".ppf" Then
                    PowertrainProjectFile = PowertrainProjectFile & ".ppf"
                End If
                KeepLastFileNameOpened(PowertrainProjectFile)

                'Init all controllers according to current project file
                InitAllControllers()

                'Get the Plant Data that is saved in the Database
                GetPlantDataFromDatabase()
                bISDirty = False
                tvwProject.SelectedNode = tvwProject.Nodes(0)
                'Run the Memory Check Routine to check all memory usage
                CheckAllNodesForMemoryUsage()
                'Set the Forms Title
                SetFormTitle()
            Else
                MsgBox("File " & My.Settings.LastTaskFile04 & " does not exist.", MsgBoxStyle.Information, Application.ProductName)
                My.Settings.LastTaskFile04 = ""
                My.Settings.Save()
                SetupPreviousFilesMenu()
            End If
        Catch ex As Exception
            Log_Anything("PreviousFileOpen_04_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub ColorPLCListBasedOnMemory()
        Try
            For Each plcNode In tvwPLCs.Nodes(0).Nodes
                plcNode.Backcolor = Color.White
            Next

            Dim treeNodeHier = New TreeNodeHierarchy(strAreaName, strSectionName, strStationName)
            Dim muList = muControllerMaster.GetMemoryUsages(treeNodeHier)
            For Each mu In muList
                Dim intMemoryUsed = mu.MemUsed
                For Each plcNode In tvwPLCs.Nodes(0).Nodes
                    Dim intPlcMemory = Convert.ToInt32(plcNode.Nodes(0).Text.ToString().Replace("1. Total Memory - ", ""))
                    If intPlcMemory < intMemoryUsed Then
                        plcNode.Backcolor = Color.Red
                    ElseIf intMemoryUsed > intPlcMemory * (My.Settings.MemoryWarning / 100) Then
                        plcNode.BackColor = Color.Yellow
                    Else
                        plcNode.BackColor = Color.YellowGreen
                    End If
                Next
            Next
        Catch ex As Exception
            Log_Anything("ColorPLCListBasedOnMemory - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        AboutBox1.TextBoxDescription.Text = Me.Text
        AboutBox1.Show()
    End Sub

    Private Sub ViewHelpToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ViewHelpToolStripMenuItem.Click
        Help.ShowHelp(Me, HelpProvider1.HelpNamespace, HelpNavigator.TableOfContents)
    End Sub

    Private Sub GenerateBasePLCProgram()
        Dim strMasterFile As String = ""
        Dim strTasks() As String
        Dim i As Integer
        Dim intMultiStation As Integer = 0

        Try

            ReDim strTasks(0)
            For Each Node In tvwProject.SelectedNode.Nodes
                If Node.Tag = "MASTERFILE" Then
                    strMasterFile = Node.text
                    For Each TaskNode In Node.Nodes
                        ReDim Preserve strTasks(i)
                        strTasks(i) = TaskNode.Text
                        i += 1
                    Next
                    Exit For
                End If
            Next

            If Mid(strMasterFile, 1, 1) = "_" Then
                strMasterFile = Mid(strMasterFile, 2, Len(strMasterFile) - 1)
            End If

            If strMasterFile <> "" And i > 0 Then

                Dim Table_ As String = "MasterFile_Tasks"
                Dim query As String
                Dim ACCDBConnString_ As String = My.Settings.DBConnectionString & My.Settings.DatabasePath & ";"
                Dim ds As New DataSet
                Using cnn As New OleDbConnection(ACCDBConnString_) 'Using to make sure db connection is disposed
                    'Dim cnn As OleDbConnection = New OleDbConnection(ACCDBConnString_)
                    Dim strMasterFileTaskL5K() As String

                    cnn.Open()
                    Dim cmd As New OleDbCommand
                    Dim da As New OleDbDataAdapter

                    Dim t1 As DataTable
                    Dim row As DataRow
                    Dim j As Integer = 0
                    ReDim strMasterFileTaskL5K(0)
                    Dim bProductionFileLoaded As Boolean
                    Dim bOneTaskLoaded As Boolean
                    Dim bL5XFileFound As Boolean
                    Dim strL5XFile As String
                    Dim strListOfL5XFilesMissing As String = ""
                    Dim strListOfTasksWithoutL5XFilesAssociated As String = ""

                    For j = 0 To UBound(strTasks)
                        query = "SELECT * FROM " & Table_ & " WHERE MasterFile_Name = '" & strMasterFile & "' AND [Multi-Station] = " & intMultiStation & " AND Task_Name = '" & strTasks(j) & "'"
                        cmd = New OleDbCommand(query, cnn)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(ds, Table_)
                        t1 = ds.Tables(Table_)
                        ReDim Preserve strMasterFileTaskL5K(j)
                        For Each row In t1.Rows
                            If Not IsDBNull(row(8)) Then
                                If row(8) <> "" Then
                                    strMasterFileTaskL5K(j) = UCase(row(8).ToString)
                                Else
                                    strMasterFileTaskL5K(j) = "NONE"
                                End If
                            Else
                                strMasterFileTaskL5K(j) = "NONE"
                            End If

                        Next
                    Next

                    Dim strL5XFolder As String = ""
                    Dim strMergedFile As String = tvwProject.SelectedNode.Text & "_BaseFile.L5X"
                    If My.Settings.L5XFolder <> "" Then
                        strL5XFolder = My.Settings.L5XFolder & "\"
                    Else
                        MsgBox("No L5X Folder has been setup. Please goto the Settings screen and select a folder where the L5X files reside.", MsgBoxStyle.Information, Application.ProductName)
                        Exit Sub
                    End If

                    strL5XFile = Dir(strL5XFolder & My.Settings.L5XBaseFileName)
                    If strL5XFile = "" Then
                        MsgBox(strL5XFile & " does not exist. PLC File generation cannot continue.", MsgBoxStyle.Critical, Application.ProductName)
                        Exit Sub
                    End If

                    Form7.ProgressBar1.Maximum = UBound(strMasterFileTaskL5K) + 1
                    Form7.ProgressBar1.Minimum = 0
                    Form7.tmrProgress.Enabled = False
                    Form7.Show()
                    Form7.tmrProgress.Enabled = False
                    Form7.lblInformation.Text = "Generating PLC File - Please wait"

                    For j = 0 To UBound(strMasterFileTaskL5K)
                        Form7.ProgressBar1.Value += 1
                        Form7.Refresh()

                        If strMasterFileTaskL5K(j) <> "NONE" And strMasterFileTaskL5K(j) <> "" Then

                            strL5XFile = Dir(strL5XFolder & strMasterFileTaskL5K(j))
                            If strL5XFile <> "" Or strMasterFileTaskL5K(j) = "DEFAULT" Then
                                bL5XFileFound = True
                            Else
                                bL5XFileFound = False
                            End If

                            If bL5XFileFound = True Then

                                If bProductionFileLoaded = False Then
                                    If strMasterFileTaskL5K(j) <> "DEFAULT" Then
                                        ImportDataTypesToXMLFile(strL5XFolder & My.Settings.L5XBaseFileName, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile)
                                    Else
                                        ImportDataTypesToXMLFile(strL5XFolder & My.Settings.L5XBaseFileName, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, True)
                                    End If
                                    bProductionFileLoaded = True
                                ElseIf bProductionFileLoaded = True Then
                                    If strMasterFileTaskL5K(j) <> "DEFAULT" Then
                                        ImportDataTypesToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile)
                                    Else
                                        ImportDataTypesToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, True)
                                    End If
                                End If

                                If strMasterFileTaskL5K(j) <> "DEFAULT" Then
                                    ImportAOIsToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile)
                                    ImportTagsToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile)
                                    ImportModulesToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile)
                                    ImportProgramsToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile)
                                Else
                                    ImportAOIsToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, True)
                                    ImportTagsToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, True)
                                    ImportModulesToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, True)
                                    ImportProgramsToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, True)
                                End If


                                If TaskScheduleName <> "" Then
                                    If strMasterFileTaskL5K(j) <> "DEFAULT" Then
                                        ImportTasksToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, TaskScheduleName)
                                    Else
                                        ImportTasksToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, TaskScheduleName, False)
                                    End If
                                Else
                                    If strMasterFileTaskL5K(j) <> "DEFAULT" Then
                                        ImportTasksToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, "Unknown_Name" & j)
                                    Else
                                        ImportTasksToXMLFile(strL5XFolder & strMergedFile, strL5XFolder & strMasterFileTaskL5K(j), strL5XFolder & strMergedFile, "Unknown_Name" & j, False)
                                    End If
                                End If

                            End If

                            bOneTaskLoaded = True

                            If bL5XFileFound = False Then
                                If strListOfL5XFilesMissing = "" Then
                                    strListOfL5XFilesMissing = strMasterFileTaskL5K(j)
                                Else
                                    strListOfL5XFilesMissing = strListOfL5XFilesMissing & "," & vbCrLf & strMasterFileTaskL5K(j)
                                End If
                            End If

                        Else
                            If strListOfTasksWithoutL5XFilesAssociated = "" Then
                                strListOfTasksWithoutL5XFilesAssociated = strTasks(j)
                            Else
                                strListOfTasksWithoutL5XFilesAssociated = strListOfTasksWithoutL5XFilesAssociated & "," & strTasks(j)
                            End If
                        End If
                    Next


                    If bOneTaskLoaded Then
                        If strListOfL5XFilesMissing = "" Then
                            If strListOfTasksWithoutL5XFilesAssociated = "" Then
                                MsgBox("PLC File has been generated. Output file is named - " & strL5XFolder & strMergedFile, MsgBoxStyle.Information, Application.ProductName)
                            Else
                                MsgBox("PLC File has been generated. Output file is named - " & strL5XFolder & strMergedFile & vbCrLf & "There are MasterFile Tasks that do not have an associated L5X filename in the Database." & vbCrLf & "These Tasks are " & vbCrLf & strListOfTasksWithoutL5XFilesAssociated, MsgBoxStyle.Information, Application.ProductName)
                            End If
                        Else
                            If strListOfTasksWithoutL5XFilesAssociated = "" Then
                                MsgBox("PLC File has been generated. Output file is named - " & strL5XFolder & strMergedFile & vbCrLf & "There were missing L5X Files." & vbCrLf & "The missing files are " & vbCrLf & strListOfL5XFilesMissing, MsgBoxStyle.Information, Application.ProductName)
                            Else
                                MsgBox("PLC File has been generated. Output file is named - " & strL5XFolder & strMergedFile & vbCrLf & "There were missing L5X Files. " & vbCrLf & "The missing files are " & vbCrLf & strListOfL5XFilesMissing & vbCrLf & "Also there are MasterFile Tasks that do not have an associated L5X filename in the Database." & vbCrLf & "These Tasks are " & vbCrLf & strListOfTasksWithoutL5XFilesAssociated, MsgBoxStyle.Information, Application.ProductName)
                            End If
                        End If
                    Else
                        If strListOfTasksWithoutL5XFilesAssociated = "" Then
                            MsgBox("PLC File has not been generated. Master Database does not have L5X files associated with selected Tasks.", MsgBoxStyle.Information, Application.ProductName)
                        Else
                            MsgBox("PLC File has not been generated. There are MasterFile Tasks that do not have an associated L5X filename in the Database. " & vbCrLf & "These Tasks are " & vbCrLf & strListOfTasksWithoutL5XFilesAssociated, MsgBoxStyle.Information, Application.ProductName)
                        End If
                    End If
                    Form7.Close()
                End Using
            Else
                MsgBox("PLC File cannot be generated without first selecting a Master file and associated Tasks for the station.", MsgBoxStyle.Exclamation, Application.ProductName)
            End If

        Catch ex As Exception
            Log_Anything("GetTaskInformation - " & GetExceptionInfo(ex))
        End Try
    End Sub


End Class
