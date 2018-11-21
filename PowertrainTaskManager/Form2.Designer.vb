<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.tvwProject = New System.Windows.Forms.TreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.dgvSequenceGraphic = New System.Windows.Forms.DataGridView()
        Me.cmdSave_Main = New System.Windows.Forms.Button()
        Me.lbl_TaskInstance = New System.Windows.Forms.Label()
        Me.NumericUpDownTaskInstance = New System.Windows.Forms.NumericUpDown()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lbl_TaskType_lbl = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_TaskName_lbl = New System.Windows.Forms.Label()
        Me.chk_TaskBypassed = New System.Windows.Forms.CheckBox()
        Me.txt_TaskNumber = New System.Windows.Forms.TextBox()
        Me.txt_TaskNumber_lbl = New System.Windows.Forms.Label()
        Me.txt_TaskName = New System.Windows.Forms.TextBox()
        Me.lbl_TaskType = New System.Windows.Forms.Label()
        Me.cmdDelete_Main = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmdDelete_Model = New System.Windows.Forms.Button()
        Me.cmdSave_Model = New System.Windows.Forms.Button()
        Me.chk_ModelEnabled = New System.Windows.Forms.CheckBox()
        Me.cbo_Models = New System.Windows.Forms.ComboBox()
        Me.lbl_Models_lbl = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.PreviousFileOpen_01 = New System.Windows.Forms.ToolStripMenuItem()
        Me.PreviousFileOpen_02 = New System.Windows.Forms.ToolStripMenuItem()
        Me.PreviousFileOpen_03 = New System.Windows.Forms.ToolStripMenuItem()
        Me.PreviousFileOpen_04 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExpandTreeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CollapseTreeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerificationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerifyProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.SequenceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.DownloadModelInformationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadTaskConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadStationConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadAllConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.UploadModelConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UploadProjectConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UploadAllConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewHelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPos1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPos2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPos3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPosition4ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPosition5ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmrRetry = New System.Windows.Forms.Timer(Me.components)
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.ContextCutCopyPaste = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnu_Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnu_Cut = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvSequenceGraphic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownTaskInstance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.contextMenuStrip1.SuspendLayout()
        Me.ContextCutCopyPaste.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvwProject
        '
        Me.tvwProject.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwProject.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwProject.HideSelection = False
        Me.tvwProject.HotTracking = True
        Me.tvwProject.ImageIndex = 0
        Me.tvwProject.ImageList = Me.ImageList1
        Me.tvwProject.Location = New System.Drawing.Point(0, 0)
        Me.tvwProject.Name = "tvwProject"
        Me.tvwProject.SelectedImageIndex = 0
        Me.tvwProject.ShowNodeToolTips = True
        Me.tvwProject.Size = New System.Drawing.Size(425, 718)
        Me.tvwProject.TabIndex = 2
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "chemical-factory-icon-53430.png")
        Me.ImageList1.Images.SetKeyName(1, "pd_group.gif")
        Me.ImageList1.Images.SetKeyName(2, "section.gif")
        Me.ImageList1.Images.SetKeyName(3, "area.gif")
        Me.ImageList1.Images.SetKeyName(4, "Pulte.GIF")
        Me.ImageList1.Images.SetKeyName(5, "device.gif")
        Me.ImageList1.Images.SetKeyName(6, "ControlDesk.gif")
        Me.ImageList1.Images.SetKeyName(7, "factory.jpg")
        Me.ImageList1.Images.SetKeyName(8, "factory-icon-53445.png")
        Me.ImageList1.Images.SetKeyName(9, "dev_noa.gif")
        Me.ImageList1.Images.SetKeyName(10, "folder.gif")
        Me.ImageList1.Images.SetKeyName(11, "open.gif")
        Me.ImageList1.Images.SetKeyName(12, "structure.gif")
        Me.ImageList1.Images.SetKeyName(13, "smallfail.gif")
        Me.ImageList1.Images.SetKeyName(14, "event.gif")
        Me.ImageList1.Images.SetKeyName(15, "1756Module.ico")
        Me.ImageList1.Images.SetKeyName(16, "blue-info-icon.jpg")
        Me.ImageList1.Images.SetKeyName(17, "exclamation_alert.png")
        Me.ImageList1.Images.SetKeyName(18, "red_cross_x_clip_art_7568.jpg")
        Me.ImageList1.Images.SetKeyName(19, "check-mark-hi.png")
        Me.ImageList1.Images.SetKeyName(20, "redright.jpg")
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 27)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tvwProject)
        Me.SplitContainer1.Panel2Collapsed = True
        Me.SplitContainer1.Size = New System.Drawing.Size(425, 718)
        Me.SplitContainer1.SplitterDistance = 129
        Me.SplitContainer1.TabIndex = 3
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer2.Location = New System.Drawing.Point(431, 27)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.AutoScroll = True
        Me.SplitContainer2.Panel1.Controls.Add(Me.GroupBox1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.AutoScroll = True
        Me.SplitContainer2.Panel2.Controls.Add(Me.GroupBox2)
        Me.SplitContainer2.Size = New System.Drawing.Size(931, 718)
        Me.SplitContainer2.SplitterDistance = 480
        Me.SplitContainer2.TabIndex = 4
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.Controls.Add(Me.dgvSequenceGraphic)
        Me.GroupBox1.Controls.Add(Me.cmdSave_Main)
        Me.GroupBox1.Controls.Add(Me.lbl_TaskInstance)
        Me.GroupBox1.Controls.Add(Me.NumericUpDownTaskInstance)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.lbl_TaskType_lbl)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txt_TaskName_lbl)
        Me.GroupBox1.Controls.Add(Me.chk_TaskBypassed)
        Me.GroupBox1.Controls.Add(Me.txt_TaskNumber)
        Me.GroupBox1.Controls.Add(Me.txt_TaskNumber_lbl)
        Me.GroupBox1.Controls.Add(Me.txt_TaskName)
        Me.GroupBox1.Controls.Add(Me.lbl_TaskType)
        Me.GroupBox1.Controls.Add(Me.cmdDelete_Main)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1037, 175)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Information / Control"
        '
        'dgvSequenceGraphic
        '
        Me.dgvSequenceGraphic.AllowUserToAddRows = False
        Me.dgvSequenceGraphic.AllowUserToDeleteRows = False
        Me.dgvSequenceGraphic.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSequenceGraphic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSequenceGraphic.Location = New System.Drawing.Point(3, 22)
        Me.dgvSequenceGraphic.Name = "dgvSequenceGraphic"
        Me.dgvSequenceGraphic.Size = New System.Drawing.Size(1031, 150)
        Me.dgvSequenceGraphic.TabIndex = 17
        Me.dgvSequenceGraphic.Tag = "BASE9999"
        Me.dgvSequenceGraphic.Visible = False
        '
        'cmdSave_Main
        '
        Me.cmdSave_Main.Enabled = False
        Me.cmdSave_Main.Location = New System.Drawing.Point(612, 66)
        Me.cmdSave_Main.Name = "cmdSave_Main"
        Me.cmdSave_Main.Size = New System.Drawing.Size(78, 26)
        Me.cmdSave_Main.TabIndex = 16
        Me.cmdSave_Main.Tag = "BASE6_0"
        Me.cmdSave_Main.Text = "SAVE"
        Me.cmdSave_Main.UseVisualStyleBackColor = True
        Me.cmdSave_Main.Visible = False
        '
        'lbl_TaskInstance
        '
        Me.lbl_TaskInstance.Location = New System.Drawing.Point(376, 66)
        Me.lbl_TaskInstance.Name = "lbl_TaskInstance"
        Me.lbl_TaskInstance.Size = New System.Drawing.Size(196, 20)
        Me.lbl_TaskInstance.TabIndex = 15
        Me.lbl_TaskInstance.Tag = "BASE5_0"
        Me.lbl_TaskInstance.Text = ":Task Instance"
        Me.lbl_TaskInstance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbl_TaskInstance.Visible = False
        '
        'NumericUpDownTaskInstance
        '
        Me.NumericUpDownTaskInstance.Location = New System.Drawing.Point(307, 64)
        Me.NumericUpDownTaskInstance.Name = "NumericUpDownTaskInstance"
        Me.NumericUpDownTaskInstance.Size = New System.Drawing.Size(63, 26)
        Me.NumericUpDownTaskInstance.TabIndex = 14
        Me.NumericUpDownTaskInstance.Tag = "BASE5"
        Me.NumericUpDownTaskInstance.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(589, 123)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(78, 26)
        Me.Button1.TabIndex = 13
        Me.Button1.Text = "EDIT"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'lbl_TaskType_lbl
        '
        Me.lbl_TaskType_lbl.Location = New System.Drawing.Point(9, 34)
        Me.lbl_TaskType_lbl.Name = "lbl_TaskType_lbl"
        Me.lbl_TaskType_lbl.Size = New System.Drawing.Size(200, 20)
        Me.lbl_TaskType_lbl.TabIndex = 12
        Me.lbl_TaskType_lbl.Tag = "BASE1_0"
        Me.lbl_TaskType_lbl.Text = "Task Type:"
        Me.lbl_TaskType_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_TaskType_lbl.Visible = False
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(505, 124)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(78, 26)
        Me.TextBox2.TabIndex = 11
        Me.TextBox2.Text = "0"
        Me.TextBox2.Visible = False
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(360, 126)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(200, 20)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Task Number:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label2.Visible = False
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(213, 124)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(78, 26)
        Me.TextBox1.TabIndex = 9
        Me.TextBox1.Text = "0"
        Me.TextBox1.Visible = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(9, 126)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(200, 20)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Task Number:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label1.Visible = False
        '
        'txt_TaskName_lbl
        '
        Me.txt_TaskName_lbl.AutoSize = True
        Me.txt_TaskName_lbl.Location = New System.Drawing.Point(575, 36)
        Me.txt_TaskName_lbl.Name = "txt_TaskName_lbl"
        Me.txt_TaskName_lbl.Size = New System.Drawing.Size(93, 20)
        Me.txt_TaskName_lbl.TabIndex = 3
        Me.txt_TaskName_lbl.Tag = "BASE2_0"
        Me.txt_TaskName_lbl.Text = "Task Name:"
        Me.txt_TaskName_lbl.Visible = False
        '
        'chk_TaskBypassed
        '
        Me.chk_TaskBypassed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chk_TaskBypassed.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_TaskBypassed.Location = New System.Drawing.Point(6, 96)
        Me.chk_TaskBypassed.Name = "chk_TaskBypassed"
        Me.chk_TaskBypassed.Size = New System.Drawing.Size(222, 24)
        Me.chk_TaskBypassed.TabIndex = 7
        Me.chk_TaskBypassed.Tag = "BASE4"
        Me.chk_TaskBypassed.Text = "Task Bypassed:"
        Me.chk_TaskBypassed.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chk_TaskBypassed.UseVisualStyleBackColor = True
        Me.chk_TaskBypassed.Visible = False
        '
        'txt_TaskNumber
        '
        Me.txt_TaskNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_TaskNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_TaskNumber.Location = New System.Drawing.Point(213, 64)
        Me.txt_TaskNumber.Name = "txt_TaskNumber"
        Me.txt_TaskNumber.Size = New System.Drawing.Size(78, 26)
        Me.txt_TaskNumber.TabIndex = 6
        Me.txt_TaskNumber.Tag = "BASE3"
        Me.txt_TaskNumber.Text = "0"
        Me.txt_TaskNumber.Visible = False
        '
        'txt_TaskNumber_lbl
        '
        Me.txt_TaskNumber_lbl.Location = New System.Drawing.Point(9, 66)
        Me.txt_TaskNumber_lbl.Name = "txt_TaskNumber_lbl"
        Me.txt_TaskNumber_lbl.Size = New System.Drawing.Size(200, 20)
        Me.txt_TaskNumber_lbl.TabIndex = 5
        Me.txt_TaskNumber_lbl.Tag = "BASE3_0"
        Me.txt_TaskNumber_lbl.Text = "Task Number:"
        Me.txt_TaskNumber_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.txt_TaskNumber_lbl.Visible = False
        '
        'txt_TaskName
        '
        Me.txt_TaskName.AccessibleName = "TaskName_User"
        Me.txt_TaskName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_TaskName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_TaskName.Location = New System.Drawing.Point(672, 34)
        Me.txt_TaskName.Name = "txt_TaskName"
        Me.txt_TaskName.Size = New System.Drawing.Size(359, 26)
        Me.txt_TaskName.TabIndex = 4
        Me.txt_TaskName.Tag = "BASE2"
        Me.txt_TaskName.Visible = False
        '
        'lbl_TaskType
        '
        Me.lbl_TaskType.AccessibleName = "TaskName_Actual"
        Me.lbl_TaskType.BackColor = System.Drawing.Color.White
        Me.lbl_TaskType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_TaskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_TaskType.Location = New System.Drawing.Point(213, 34)
        Me.lbl_TaskType.Name = "lbl_TaskType"
        Me.lbl_TaskType.Size = New System.Drawing.Size(359, 26)
        Me.lbl_TaskType.TabIndex = 0
        Me.lbl_TaskType.Tag = "BASE1"
        Me.lbl_TaskType.Visible = False
        '
        'cmdDelete_Main
        '
        Me.cmdDelete_Main.Enabled = False
        Me.cmdDelete_Main.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete_Main.Location = New System.Drawing.Point(696, 66)
        Me.cmdDelete_Main.Name = "cmdDelete_Main"
        Me.cmdDelete_Main.Size = New System.Drawing.Size(78, 26)
        Me.cmdDelete_Main.TabIndex = 18
        Me.cmdDelete_Main.Tag = "BASE7_0"
        Me.cmdDelete_Main.Text = "DELETE"
        Me.cmdDelete_Main.UseVisualStyleBackColor = True
        Me.cmdDelete_Main.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.AutoSize = True
        Me.GroupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox2.Controls.Add(Me.cmdDelete_Model)
        Me.GroupBox2.Controls.Add(Me.cmdSave_Model)
        Me.GroupBox2.Controls.Add(Me.chk_ModelEnabled)
        Me.GroupBox2.Controls.Add(Me.cbo_Models)
        Me.GroupBox2.Controls.Add(Me.lbl_Models_lbl)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(780, 116)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Model Configuration"
        '
        'cmdDelete_Model
        '
        Me.cmdDelete_Model.Enabled = False
        Me.cmdDelete_Model.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete_Model.Location = New System.Drawing.Point(696, 35)
        Me.cmdDelete_Model.Name = "cmdDelete_Model"
        Me.cmdDelete_Model.Size = New System.Drawing.Size(78, 26)
        Me.cmdDelete_Model.TabIndex = 19
        Me.cmdDelete_Model.Tag = "BASE7_0"
        Me.cmdDelete_Model.Text = "DELETE"
        Me.cmdDelete_Model.UseVisualStyleBackColor = True
        Me.cmdDelete_Model.Visible = False
        '
        'cmdSave_Model
        '
        Me.cmdSave_Model.Enabled = False
        Me.cmdSave_Model.Location = New System.Drawing.Point(612, 35)
        Me.cmdSave_Model.Name = "cmdSave_Model"
        Me.cmdSave_Model.Size = New System.Drawing.Size(78, 26)
        Me.cmdSave_Model.TabIndex = 17
        Me.cmdSave_Model.Tag = "BASE6_0"
        Me.cmdSave_Model.Text = "SAVE"
        Me.cmdSave_Model.UseVisualStyleBackColor = True
        Me.cmdSave_Model.Visible = False
        '
        'chk_ModelEnabled
        '
        Me.chk_ModelEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chk_ModelEnabled.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_ModelEnabled.Location = New System.Drawing.Point(6, 67)
        Me.chk_ModelEnabled.Name = "chk_ModelEnabled"
        Me.chk_ModelEnabled.Size = New System.Drawing.Size(222, 24)
        Me.chk_ModelEnabled.TabIndex = 16
        Me.chk_ModelEnabled.Tag = "BASE4"
        Me.chk_ModelEnabled.Text = "Enabled:"
        Me.chk_ModelEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chk_ModelEnabled.UseVisualStyleBackColor = True
        Me.chk_ModelEnabled.Visible = False
        '
        'cbo_Models
        '
        Me.cbo_Models.DisplayMember = "1,2,3,4,,"
        Me.cbo_Models.FormattingEnabled = True
        Me.cbo_Models.Location = New System.Drawing.Point(213, 33)
        Me.cbo_Models.Name = "cbo_Models"
        Me.cbo_Models.Size = New System.Drawing.Size(359, 28)
        Me.cbo_Models.TabIndex = 15
        Me.cbo_Models.Tag = "BASE1"
        Me.cbo_Models.Visible = False
        '
        'lbl_Models_lbl
        '
        Me.lbl_Models_lbl.Location = New System.Drawing.Point(9, 33)
        Me.lbl_Models_lbl.Name = "lbl_Models_lbl"
        Me.lbl_Models_lbl.Size = New System.Drawing.Size(200, 20)
        Me.lbl_Models_lbl.TabIndex = 14
        Me.lbl_Models_lbl.Tag = "BASE1_0"
        Me.lbl_Models_lbl.Text = "Models:"
        Me.lbl_Models_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_Models_lbl.Visible = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.VerificationToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1362, 24)
        Me.MenuStrip1.TabIndex = 19
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.CloseProjectToolStripMenuItem, Me.ToolStripSeparator1, Me.PreviousFileOpen_01, Me.PreviousFileOpen_02, Me.PreviousFileOpen_03, Me.PreviousFileOpen_04, Me.ToolStripSeparator2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OpenToolStripMenuItem.Text = "Open Project"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Enabled = False
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save As"
        '
        'CloseProjectToolStripMenuItem
        '
        Me.CloseProjectToolStripMenuItem.Name = "CloseProjectToolStripMenuItem"
        Me.CloseProjectToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.CloseProjectToolStripMenuItem.Text = "Close Project"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(180, 6)
        '
        'PreviousFileOpen_01
        '
        Me.PreviousFileOpen_01.Name = "PreviousFileOpen_01"
        Me.PreviousFileOpen_01.Size = New System.Drawing.Size(183, 22)
        Me.PreviousFileOpen_01.Text = "PreviousFileOpen_01"
        '
        'PreviousFileOpen_02
        '
        Me.PreviousFileOpen_02.Name = "PreviousFileOpen_02"
        Me.PreviousFileOpen_02.Size = New System.Drawing.Size(183, 22)
        Me.PreviousFileOpen_02.Text = "PreviousFileOpen_02"
        '
        'PreviousFileOpen_03
        '
        Me.PreviousFileOpen_03.Name = "PreviousFileOpen_03"
        Me.PreviousFileOpen_03.Size = New System.Drawing.Size(183, 22)
        Me.PreviousFileOpen_03.Text = "PreviousFileOpen_03"
        '
        'PreviousFileOpen_04
        '
        Me.PreviousFileOpen_04.Name = "PreviousFileOpen_04"
        Me.PreviousFileOpen_04.Size = New System.Drawing.Size(183, 22)
        Me.PreviousFileOpen_04.Text = "PreviousFileOpen_04"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(180, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExpandTreeToolStripMenuItem, Me.CollapseTreeToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.EditToolStripMenuItem.Text = "View"
        '
        'ExpandTreeToolStripMenuItem
        '
        Me.ExpandTreeToolStripMenuItem.Name = "ExpandTreeToolStripMenuItem"
        Me.ExpandTreeToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.ExpandTreeToolStripMenuItem.Text = "Expand Tree"
        '
        'CollapseTreeToolStripMenuItem
        '
        Me.CollapseTreeToolStripMenuItem.Name = "CollapseTreeToolStripMenuItem"
        Me.CollapseTreeToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.CollapseTreeToolStripMenuItem.Text = "Collapse Tree"
        '
        'VerificationToolStripMenuItem
        '
        Me.VerificationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VerifyProjectToolStripMenuItem, Me.ToolStripSeparator4, Me.SequenceMenuItem, Me.ToolStripSeparator5, Me.DownloadModelInformationToolStripMenuItem, Me.DownloadTaskConfigurationToolStripMenuItem, Me.DownloadStationConfigurationToolStripMenuItem, Me.DownloadAllConfigurationToolStripMenuItem, Me.ToolStripSeparator3, Me.UploadModelConfigurationToolStripMenuItem, Me.UploadProjectConfigurationToolStripMenuItem, Me.UploadAllConfigurationToolStripMenuItem})
        Me.VerificationToolStripMenuItem.Name = "VerificationToolStripMenuItem"
        Me.VerificationToolStripMenuItem.Size = New System.Drawing.Size(93, 20)
        Me.VerificationToolStripMenuItem.Text = "Configuration"
        '
        'VerifyProjectToolStripMenuItem
        '
        Me.VerifyProjectToolStripMenuItem.Name = "VerifyProjectToolStripMenuItem"
        Me.VerifyProjectToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.VerifyProjectToolStripMenuItem.Text = "Verify Project"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(242, 6)
        '
        'SequenceMenuItem
        '
        Me.SequenceMenuItem.Enabled = False
        Me.SequenceMenuItem.Name = "SequenceMenuItem"
        Me.SequenceMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.SequenceMenuItem.Text = "Show Sequence Graphic"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(242, 6)
        '
        'DownloadModelInformationToolStripMenuItem
        '
        Me.DownloadModelInformationToolStripMenuItem.Name = "DownloadModelInformationToolStripMenuItem"
        Me.DownloadModelInformationToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.DownloadModelInformationToolStripMenuItem.Text = "Download Model Configuration"
        '
        'DownloadTaskConfigurationToolStripMenuItem
        '
        Me.DownloadTaskConfigurationToolStripMenuItem.Name = "DownloadTaskConfigurationToolStripMenuItem"
        Me.DownloadTaskConfigurationToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.DownloadTaskConfigurationToolStripMenuItem.Text = "Download Task Configuration"
        '
        'DownloadStationConfigurationToolStripMenuItem
        '
        Me.DownloadStationConfigurationToolStripMenuItem.Name = "DownloadStationConfigurationToolStripMenuItem"
        Me.DownloadStationConfigurationToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.DownloadStationConfigurationToolStripMenuItem.Text = "Download Station Configuration"
        '
        'DownloadAllConfigurationToolStripMenuItem
        '
        Me.DownloadAllConfigurationToolStripMenuItem.Name = "DownloadAllConfigurationToolStripMenuItem"
        Me.DownloadAllConfigurationToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.DownloadAllConfigurationToolStripMenuItem.Text = "Download All Configuration"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(242, 6)
        '
        'UploadModelConfigurationToolStripMenuItem
        '
        Me.UploadModelConfigurationToolStripMenuItem.Name = "UploadModelConfigurationToolStripMenuItem"
        Me.UploadModelConfigurationToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.UploadModelConfigurationToolStripMenuItem.Text = "Upload Model Configuration"
        '
        'UploadProjectConfigurationToolStripMenuItem
        '
        Me.UploadProjectConfigurationToolStripMenuItem.Name = "UploadProjectConfigurationToolStripMenuItem"
        Me.UploadProjectConfigurationToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.UploadProjectConfigurationToolStripMenuItem.Text = "Upload Project Configuration"
        '
        'UploadAllConfigurationToolStripMenuItem
        '
        Me.UploadAllConfigurationToolStripMenuItem.Name = "UploadAllConfigurationToolStripMenuItem"
        Me.UploadAllConfigurationToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.UploadAllConfigurationToolStripMenuItem.Text = "Upload All Configuration"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewHelpToolStripMenuItem, Me.AboutToolStripMenuItem1})
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.AboutToolStripMenuItem.Text = "Help"
        '
        'ViewHelpToolStripMenuItem
        '
        Me.ViewHelpToolStripMenuItem.Name = "ViewHelpToolStripMenuItem"
        Me.ViewHelpToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.ViewHelpToolStripMenuItem.Text = "View Help"
        '
        'AboutToolStripMenuItem1
        '
        Me.AboutToolStripMenuItem1.Name = "AboutToolStripMenuItem1"
        Me.AboutToolStripMenuItem1.Size = New System.Drawing.Size(127, 22)
        Me.AboutToolStripMenuItem1.Text = "About"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "ppf"
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'contextMenuStrip1
        '
        Me.contextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPos1, Me.mnuPos2, Me.mnuPos3, Me.MenuPosition4ToolStripMenuItem, Me.MenuPosition5ToolStripMenuItem})
        Me.contextMenuStrip1.Name = "contextMenuStrip1"
        Me.contextMenuStrip1.Size = New System.Drawing.Size(161, 114)
        '
        'mnuPos1
        '
        Me.mnuPos1.Name = "mnuPos1"
        Me.mnuPos1.Size = New System.Drawing.Size(160, 22)
        Me.mnuPos1.Text = "Menu Position 1"
        '
        'mnuPos2
        '
        Me.mnuPos2.Name = "mnuPos2"
        Me.mnuPos2.Size = New System.Drawing.Size(160, 22)
        Me.mnuPos2.Text = "Menu Position 2"
        '
        'mnuPos3
        '
        Me.mnuPos3.Name = "mnuPos3"
        Me.mnuPos3.Size = New System.Drawing.Size(160, 22)
        Me.mnuPos3.Text = "Menu Position 3"
        '
        'MenuPosition4ToolStripMenuItem
        '
        Me.MenuPosition4ToolStripMenuItem.Name = "MenuPosition4ToolStripMenuItem"
        Me.MenuPosition4ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.MenuPosition4ToolStripMenuItem.Text = "Menu Position 4"
        '
        'MenuPosition5ToolStripMenuItem
        '
        Me.MenuPosition5ToolStripMenuItem.Name = "MenuPosition5ToolStripMenuItem"
        Me.MenuPosition5ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.MenuPosition5ToolStripMenuItem.Text = "Menu Position 5"
        '
        'ContextCutCopyPaste
        '
        Me.ContextCutCopyPaste.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnu_Copy, Me.mnu_Cut})
        Me.ContextCutCopyPaste.Name = "contextMenuStrip1"
        Me.ContextCutCopyPaste.Size = New System.Drawing.Size(162, 48)
        '
        'mnu_Copy
        '
        Me.mnu_Copy.Name = "mnu_Copy"
        Me.mnu_Copy.Size = New System.Drawing.Size(161, 22)
        Me.mnu_Copy.Text = "Duplicate Model"
        '
        'mnu_Cut
        '
        Me.mnu_Cut.Name = "mnu_Cut"
        Me.mnu_Cut.Size = New System.Drawing.Size(161, 22)
        Me.mnu_Cut.Text = "Delete Model"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1362, 742)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.SplitContainer2)
        Me.Controls.Add(Me.SplitContainer1)
        Me.HelpProvider1.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.TableOfContents)
        Me.Name = "Form2"
        Me.HelpProvider1.SetShowHelp(Me, true)
        Me.Text = "Production Task Configurator"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.SplitContainer1.Panel1.ResumeLayout(false)
        CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).EndInit
        Me.SplitContainer1.ResumeLayout(false)
        Me.SplitContainer2.Panel1.ResumeLayout(false)
        Me.SplitContainer2.Panel1.PerformLayout
        Me.SplitContainer2.Panel2.ResumeLayout(false)
        Me.SplitContainer2.Panel2.PerformLayout
        CType(Me.SplitContainer2,System.ComponentModel.ISupportInitialize).EndInit
        Me.SplitContainer2.ResumeLayout(false)
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        CType(Me.dgvSequenceGraphic,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.NumericUpDownTaskInstance,System.ComponentModel.ISupportInitialize).EndInit
        Me.GroupBox2.ResumeLayout(false)
        Me.MenuStrip1.ResumeLayout(false)
        Me.MenuStrip1.PerformLayout
        Me.contextMenuStrip1.ResumeLayout(false)
        Me.ContextCutCopyPaste.ResumeLayout(false)
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents tvwProject As System.Windows.Forms.TreeView
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExpandTreeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CollapseTreeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VerificationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VerifyProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chk_TaskBypassed As System.Windows.Forms.CheckBox
    Friend WithEvents txt_TaskNumber As System.Windows.Forms.TextBox
    Friend WithEvents txt_TaskNumber_lbl As System.Windows.Forms.Label
    Friend WithEvents txt_TaskName As System.Windows.Forms.TextBox
    Friend WithEvents txt_TaskName_lbl As System.Windows.Forms.Label
    Friend WithEvents lbl_TaskType As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_TaskType_lbl As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lbl_TaskInstance As System.Windows.Forms.Label
    Friend WithEvents NumericUpDownTaskInstance As System.Windows.Forms.NumericUpDown
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PreviousFileOpen_01 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PreviousFileOpen_02 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PreviousFileOpen_03 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PreviousFileOpen_04 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents chk_ModelEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents cbo_Models As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Models_lbl As System.Windows.Forms.Label
    Friend WithEvents DownloadModelInformationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DownloadTaskConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DownloadAllConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents UploadModelConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UploadProjectConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UploadAllConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdSave_Main As System.Windows.Forms.Button
    Friend WithEvents cmdSave_Model As System.Windows.Forms.Button
    Friend WithEvents dgvSequenceGraphic As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SequenceMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents contextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPos1 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnuPos2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPos3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPos4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DownloadStationConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrRetry As System.Windows.Forms.Timer
    Friend WithEvents cmdDelete_Main As System.Windows.Forms.Button
    Friend WithEvents cmdDelete_Model As System.Windows.Forms.Button
    Friend WithEvents ViewHelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Private WithEvents ContextCutCopyPaste As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnu_Cut As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mnu_Copy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuPosition4ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuPosition5ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
