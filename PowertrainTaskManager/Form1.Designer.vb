<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPos1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPos2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPos3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPos4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPos5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnNodeTagSearch = New System.Windows.Forms.Button()
        Me.txtTagSearch = New System.Windows.Forms.TextBox()
        Me.label4 = New System.Windows.Forms.Label()
        Me.gbxSearchByText = New System.Windows.Forms.GroupBox()
        Me.btnNodeTextSearch = New System.Windows.Forms.Button()
        Me.txtNodeTextSearch = New System.Windows.Forms.TextBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.gbxNodeSearch = New System.Windows.Forms.GroupBox()
        Me.btnFindNode = New System.Windows.Forms.Button()
        Me.txtNodeSearch = New System.Windows.Forms.TextBox()
        Me.label5 = New System.Windows.Forms.Label()
        Me.gbxNodeInfo = New System.Windows.Forms.GroupBox()
        Me.txtTag = New System.Windows.Forms.TextBox()
        Me.txtText = New System.Windows.Forms.TextBox()
        Me.label6 = New System.Windows.Forms.Label()
        Me.label7 = New System.Windows.Forms.Label()
        Me.txtParentName = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.ReportsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MemoryUsageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewHelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblSystemCheck = New System.Windows.Forms.Label()
        Me.tmrCheckSystem = New System.Windows.Forms.Timer(Me.components)
        Me.dgvPLCMemory = New System.Windows.Forms.DataGridView()
        Me.tvwMasterTasks = New System.Windows.Forms.TreeView()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tvwProject = New System.Windows.Forms.TreeView()
        Me.tvwPLCs = New System.Windows.Forms.TreeView()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.contextMenuStrip1.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.gbxSearchByText.SuspendLayout()
        Me.gbxNodeSearch.SuspendLayout()
        Me.gbxNodeInfo.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.dgvPLCMemory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.SuspendLayout()
        '
        'contextMenuStrip1
        '
        Me.contextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPos1, Me.mnuPos2, Me.mnuPos3, Me.mnuPos4, Me.mnuPos5})
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
        'mnuPos4
        '
        Me.mnuPos4.Name = "mnuPos4"
        Me.mnuPos4.Size = New System.Drawing.Size(160, 22)
        Me.mnuPos4.Text = "Menu Position 4"
        '
        'mnuPos5
        '
        Me.mnuPos5.Name = "mnuPos5"
        Me.mnuPos5.Size = New System.Drawing.Size(160, 22)
        Me.mnuPos5.Text = "Menu Position 5"
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
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.btnNodeTagSearch)
        Me.groupBox1.Controls.Add(Me.txtTagSearch)
        Me.groupBox1.Controls.Add(Me.label4)
        Me.groupBox1.Location = New System.Drawing.Point(540, 888)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(368, 86)
        Me.groupBox1.TabIndex = 7
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Node Search (By Tag)"
        Me.groupBox1.Visible = False
        '
        'btnNodeTagSearch
        '
        Me.btnNodeTagSearch.Location = New System.Drawing.Point(228, 55)
        Me.btnNodeTagSearch.Name = "btnNodeTagSearch"
        Me.btnNodeTagSearch.Size = New System.Drawing.Size(75, 23)
        Me.btnNodeTagSearch.TabIndex = 7
        Me.btnNodeTagSearch.Text = "Find"
        Me.btnNodeTagSearch.UseVisualStyleBackColor = True
        '
        'txtTagSearch
        '
        Me.txtTagSearch.Location = New System.Drawing.Point(97, 28)
        Me.txtTagSearch.Name = "txtTagSearch"
        Me.txtTagSearch.Size = New System.Drawing.Size(207, 20)
        Me.txtTagSearch.TabIndex = 6
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(64, 31)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(29, 13)
        Me.label4.TabIndex = 5
        Me.label4.Text = "Tag:"
        '
        'gbxSearchByText
        '
        Me.gbxSearchByText.Controls.Add(Me.btnNodeTextSearch)
        Me.gbxSearchByText.Controls.Add(Me.txtNodeTextSearch)
        Me.gbxSearchByText.Controls.Add(Me.label3)
        Me.gbxSearchByText.Location = New System.Drawing.Point(923, 793)
        Me.gbxSearchByText.Name = "gbxSearchByText"
        Me.gbxSearchByText.Size = New System.Drawing.Size(368, 89)
        Me.gbxSearchByText.TabIndex = 6
        Me.gbxSearchByText.TabStop = False
        Me.gbxSearchByText.Text = "Node Search (By Text)"
        Me.gbxSearchByText.Visible = False
        '
        'btnNodeTextSearch
        '
        Me.btnNodeTextSearch.Location = New System.Drawing.Point(228, 55)
        Me.btnNodeTextSearch.Name = "btnNodeTextSearch"
        Me.btnNodeTextSearch.Size = New System.Drawing.Size(75, 23)
        Me.btnNodeTextSearch.TabIndex = 7
        Me.btnNodeTextSearch.Text = "Find"
        Me.btnNodeTextSearch.UseVisualStyleBackColor = True
        '
        'txtNodeTextSearch
        '
        Me.txtNodeTextSearch.Location = New System.Drawing.Point(97, 28)
        Me.txtNodeTextSearch.Name = "txtNodeTextSearch"
        Me.txtNodeTextSearch.Size = New System.Drawing.Size(207, 20)
        Me.txtNodeTextSearch.TabIndex = 6
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(64, 31)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(31, 13)
        Me.label3.TabIndex = 5
        Me.label3.Text = "Text:"
        '
        'gbxNodeSearch
        '
        Me.gbxNodeSearch.Controls.Add(Me.btnFindNode)
        Me.gbxNodeSearch.Controls.Add(Me.txtNodeSearch)
        Me.gbxNodeSearch.Controls.Add(Me.label5)
        Me.gbxNodeSearch.Location = New System.Drawing.Point(540, 793)
        Me.gbxNodeSearch.Name = "gbxNodeSearch"
        Me.gbxNodeSearch.Size = New System.Drawing.Size(368, 89)
        Me.gbxNodeSearch.TabIndex = 5
        Me.gbxNodeSearch.TabStop = False
        Me.gbxNodeSearch.Text = "Node Search (By Name)"
        Me.gbxNodeSearch.Visible = False
        '
        'btnFindNode
        '
        Me.btnFindNode.Location = New System.Drawing.Point(228, 55)
        Me.btnFindNode.Name = "btnFindNode"
        Me.btnFindNode.Size = New System.Drawing.Size(75, 23)
        Me.btnFindNode.TabIndex = 7
        Me.btnFindNode.Text = "Find"
        Me.btnFindNode.UseVisualStyleBackColor = True
        '
        'txtNodeSearch
        '
        Me.txtNodeSearch.Location = New System.Drawing.Point(97, 28)
        Me.txtNodeSearch.Name = "txtNodeSearch"
        Me.txtNodeSearch.Size = New System.Drawing.Size(207, 20)
        Me.txtNodeSearch.TabIndex = 6
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(57, 32)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(38, 13)
        Me.label5.TabIndex = 5
        Me.label5.Text = "Name:"
        '
        'gbxNodeInfo
        '
        Me.gbxNodeInfo.Controls.Add(Me.txtTag)
        Me.gbxNodeInfo.Controls.Add(Me.txtText)
        Me.gbxNodeInfo.Controls.Add(Me.label6)
        Me.gbxNodeInfo.Controls.Add(Me.label7)
        Me.gbxNodeInfo.Controls.Add(Me.txtParentName)
        Me.gbxNodeInfo.Controls.Add(Me.txtName)
        Me.gbxNodeInfo.Controls.Add(Me.label2)
        Me.gbxNodeInfo.Controls.Add(Me.label1)
        Me.gbxNodeInfo.Location = New System.Drawing.Point(923, 888)
        Me.gbxNodeInfo.Name = "gbxNodeInfo"
        Me.gbxNodeInfo.Size = New System.Drawing.Size(368, 132)
        Me.gbxNodeInfo.TabIndex = 4
        Me.gbxNodeInfo.TabStop = False
        Me.gbxNodeInfo.Text = "Selected Node Information"
        Me.gbxNodeInfo.Visible = False
        '
        'txtTag
        '
        Me.txtTag.Location = New System.Drawing.Point(97, 104)
        Me.txtTag.Name = "txtTag"
        Me.txtTag.Size = New System.Drawing.Size(236, 20)
        Me.txtTag.TabIndex = 9
        '
        'txtText
        '
        Me.txtText.Location = New System.Drawing.Point(97, 77)
        Me.txtText.Name = "txtText"
        Me.txtText.Size = New System.Drawing.Size(236, 20)
        Me.txtText.TabIndex = 8
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(66, 108)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(29, 13)
        Me.label6.TabIndex = 7
        Me.label6.Text = "Tag:"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(64, 81)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(31, 13)
        Me.label7.TabIndex = 6
        Me.label7.Text = "Text:"
        '
        'txtParentName
        '
        Me.txtParentName.Location = New System.Drawing.Point(97, 51)
        Me.txtParentName.Name = "txtParentName"
        Me.txtParentName.Size = New System.Drawing.Size(236, 20)
        Me.txtParentName.TabIndex = 5
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(97, 24)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(236, 20)
        Me.txtName.TabIndex = 4
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(23, 55)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(72, 13)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Parent Name:"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(57, 28)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(38, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Name:"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ReportsToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1362, 24)
        Me.MenuStrip1.TabIndex = 18
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.CloseProjectToolStripMenuItem, Me.ToolStripSeparator1, Me.PreviousFileOpen_01, Me.PreviousFileOpen_02, Me.PreviousFileOpen_03, Me.PreviousFileOpen_04, Me.ToolStripSeparator2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.NewToolStripMenuItem.Text = "New Project"
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
        'ReportsToolStripMenuItem
        '
        Me.ReportsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MemoryUsageToolStripMenuItem})
        Me.ReportsToolStripMenuItem.Name = "ReportsToolStripMenuItem"
        Me.ReportsToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.ReportsToolStripMenuItem.Text = "Reports"
        '
        'MemoryUsageToolStripMenuItem
        '
        Me.MemoryUsageToolStripMenuItem.Name = "MemoryUsageToolStripMenuItem"
        Me.MemoryUsageToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.MemoryUsageToolStripMenuItem.Text = "Memory Usage"
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
        'lblSystemCheck
        '
        Me.lblSystemCheck.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSystemCheck.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSystemCheck.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemCheck.Location = New System.Drawing.Point(0, 0)
        Me.lblSystemCheck.Name = "lblSystemCheck"
        Me.lblSystemCheck.Size = New System.Drawing.Size(579, 30)
        Me.lblSystemCheck.TabIndex = 19
        Me.lblSystemCheck.Text = "System OK"
        Me.lblSystemCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tmrCheckSystem
        '
        Me.tmrCheckSystem.Interval = 500
        '
        'dgvPLCMemory
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPLCMemory.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvPLCMemory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPLCMemory.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvPLCMemory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPLCMemory.Location = New System.Drawing.Point(0, 0)
        Me.dgvPLCMemory.Name = "dgvPLCMemory"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPLCMemory.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvPLCMemory.Size = New System.Drawing.Size(579, 682)
        Me.dgvPLCMemory.TabIndex = 27
        '
        'tvwMasterTasks
        '
        Me.tvwMasterTasks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwMasterTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwMasterTasks.HotTracking = True
        Me.tvwMasterTasks.ImageIndex = 0
        Me.tvwMasterTasks.ImageList = Me.ImageList1
        Me.tvwMasterTasks.Location = New System.Drawing.Point(0, 0)
        Me.tvwMasterTasks.Name = "tvwMasterTasks"
        Me.tvwMasterTasks.SelectedImageIndex = 0
        Me.tvwMasterTasks.ShowNodeToolTips = True
        Me.tvwMasterTasks.Size = New System.Drawing.Size(390, 410)
        Me.tvwMasterTasks.TabIndex = 31
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tvwProject)
        Me.SplitContainer1.Panel2Collapsed = True
        Me.SplitContainer1.Size = New System.Drawing.Size(379, 718)
        Me.SplitContainer1.SplitterDistance = 548
        Me.SplitContainer1.TabIndex = 32
        '
        'tvwProject
        '
        Me.tvwProject.ContextMenuStrip = Me.contextMenuStrip1
        Me.tvwProject.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwProject.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwProject.HotTracking = True
        Me.tvwProject.ImageIndex = 0
        Me.tvwProject.ImageList = Me.ImageList1
        Me.tvwProject.Location = New System.Drawing.Point(0, 0)
        Me.tvwProject.Name = "tvwProject"
        Me.tvwProject.SelectedImageIndex = 0
        Me.tvwProject.ShowNodeToolTips = True
        Me.tvwProject.Size = New System.Drawing.Size(377, 716)
        Me.tvwProject.TabIndex = 1
        '
        'tvwPLCs
        '
        Me.tvwPLCs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwPLCs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwPLCs.HotTracking = True
        Me.tvwPLCs.ImageIndex = 0
        Me.tvwPLCs.ImageList = Me.ImageList1
        Me.tvwPLCs.Location = New System.Drawing.Point(0, 0)
        Me.tvwPLCs.Name = "tvwPLCs"
        Me.tvwPLCs.SelectedImageIndex = 0
        Me.tvwPLCs.ShowNodeToolTips = True
        Me.tvwPLCs.Size = New System.Drawing.Size(390, 300)
        Me.tvwPLCs.TabIndex = 35
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer2.Location = New System.Drawing.Point(970, 24)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.tvwPLCs)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.tvwMasterTasks)
        Me.SplitContainer2.Size = New System.Drawing.Size(392, 718)
        Me.SplitContainer2.SplitterDistance = 302
        Me.SplitContainer2.TabIndex = 33
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer3.IsSplitterFixed = True
        Me.SplitContainer3.Location = New System.Drawing.Point(385, 24)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.lblSystemCheck)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.dgvPLCMemory)
        Me.SplitContainer3.Size = New System.Drawing.Size(579, 716)
        Me.SplitContainer3.SplitterDistance = 30
        Me.SplitContainer3.TabIndex = 34
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "ppf"
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1362, 742)
        Me.Controls.Add(Me.SplitContainer3)
        Me.Controls.Add(Me.SplitContainer2)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.gbxSearchByText)
        Me.Controls.Add(Me.gbxNodeSearch)
        Me.Controls.Add(Me.gbxNodeInfo)
        Me.HelpProvider1.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.HelpProvider1.SetShowHelp(Me, True)
        Me.Text = "Architecture Manager"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.contextMenuStrip1.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.gbxSearchByText.ResumeLayout(False)
        Me.gbxSearchByText.PerformLayout()
        Me.gbxNodeSearch.ResumeLayout(False)
        Me.gbxNodeSearch.PerformLayout()
        Me.gbxNodeInfo.ResumeLayout(False)
        Me.gbxNodeInfo.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.dgvPLCMemory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents btnNodeTagSearch As System.Windows.Forms.Button
    Private WithEvents txtTagSearch As System.Windows.Forms.TextBox
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents gbxSearchByText As System.Windows.Forms.GroupBox
    Private WithEvents btnNodeTextSearch As System.Windows.Forms.Button
    Private WithEvents txtNodeTextSearch As System.Windows.Forms.TextBox
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents gbxNodeSearch As System.Windows.Forms.GroupBox
    Private WithEvents btnFindNode As System.Windows.Forms.Button
    Private WithEvents txtNodeSearch As System.Windows.Forms.TextBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents gbxNodeInfo As System.Windows.Forms.GroupBox
    Private WithEvents txtTag As System.Windows.Forms.TextBox
    Private WithEvents txtText As System.Windows.Forms.TextBox
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents txtParentName As System.Windows.Forms.TextBox
    Private WithEvents txtName As System.Windows.Forms.TextBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblSystemCheck As System.Windows.Forms.Label
    Friend WithEvents tmrCheckSystem As System.Windows.Forms.Timer
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents contextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Private WithEvents mnuPos2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dgvPLCMemory As System.Windows.Forms.DataGridView
    Friend WithEvents tvwMasterTasks As System.Windows.Forms.TreeView
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents mnuPos1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents mnuPos3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tvwProject As System.Windows.Forms.TreeView
    Friend WithEvents tvwPLCs As System.Windows.Forms.TreeView
    Friend WithEvents ReportsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MemoryUsageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExpandTreeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CollapseTreeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PreviousFileOpen_01 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PreviousFileOpen_02 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PreviousFileOpen_03 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PreviousFileOpen_04 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ViewHelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents mnuPos4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPos5 As System.Windows.Forms.ToolStripMenuItem

End Class
