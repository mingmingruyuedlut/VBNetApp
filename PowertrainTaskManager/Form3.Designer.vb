<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtLogFolder = New System.Windows.Forms.TextBox()
        Me.txtDatabase = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSetLoggingFolder = New System.Windows.Forms.Button()
        Me.btnSetDatabase = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtWarningPercent = New System.Windows.Forms.TextBox()
        Me.txtFaultPercent = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtPLCMemoryAWS = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPLCMemoryMWS = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboLanguageField = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtAdministratorPassword = New System.Windows.Forms.TextBox()
        Me.txtMaintenancePassword = New System.Windows.Forms.TextBox()
        Me.txtOperatorPassword = New System.Windows.Forms.TextBox()
        Me.btnSetL5XFolder = New System.Windows.Forms.Button()
        Me.txtL5X = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtL5XBaseFileName = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.chkDisableArchMgr = New System.Windows.Forms.CheckBox()
        Me.chkDisableProdTaskMgr = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(82, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Log Folder:"
        '
        'txtLogFolder
        '
        Me.txtLogFolder.Location = New System.Drawing.Point(151, 35)
        Me.txtLogFolder.Name = "txtLogFolder"
        Me.txtLogFolder.Size = New System.Drawing.Size(797, 20)
        Me.txtLogFolder.TabIndex = 1
        '
        'txtDatabase
        '
        Me.txtDatabase.Location = New System.Drawing.Point(151, 61)
        Me.txtDatabase.Name = "txtDatabase"
        Me.txtDatabase.Size = New System.Drawing.Size(798, 20)
        Me.txtDatabase.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(86, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Database:"
        '
        'btnSetLoggingFolder
        '
        Me.btnSetLoggingFolder.Location = New System.Drawing.Point(957, 34)
        Me.btnSetLoggingFolder.Name = "btnSetLoggingFolder"
        Me.btnSetLoggingFolder.Size = New System.Drawing.Size(28, 21)
        Me.btnSetLoggingFolder.TabIndex = 6
        Me.btnSetLoggingFolder.Text = "..."
        Me.btnSetLoggingFolder.UseVisualStyleBackColor = True
        '
        'btnSetDatabase
        '
        Me.btnSetDatabase.Location = New System.Drawing.Point(957, 60)
        Me.btnSetDatabase.Name = "btnSetDatabase"
        Me.btnSetDatabase.Size = New System.Drawing.Size(28, 21)
        Me.btnSetDatabase.TabIndex = 7
        Me.btnSetDatabase.Text = "..."
        Me.btnSetDatabase.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(892, 373)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(93, 25)
        Me.btnExit.TabIndex = 9
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 157)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(135, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Memory Usage Warning %:"
        '
        'txtWarningPercent
        '
        Me.txtWarningPercent.Location = New System.Drawing.Point(154, 157)
        Me.txtWarningPercent.Name = "txtWarningPercent"
        Me.txtWarningPercent.Size = New System.Drawing.Size(57, 20)
        Me.txtWarningPercent.TabIndex = 13
        '
        'txtFaultPercent
        '
        Me.txtFaultPercent.Location = New System.Drawing.Point(154, 182)
        Me.txtFaultPercent.Name = "txtFaultPercent"
        Me.txtFaultPercent.Size = New System.Drawing.Size(57, 20)
        Me.txtFaultPercent.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 182)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(118, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Memory Usage Fault %:"
        '
        'txtPLCMemoryAWS
        '
        Me.txtPLCMemoryAWS.Location = New System.Drawing.Point(539, 182)
        Me.txtPLCMemoryAWS.Name = "txtPLCMemoryAWS"
        Me.txtPLCMemoryAWS.Size = New System.Drawing.Size(77, 20)
        Me.txtPLCMemoryAWS.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(297, 185)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(236, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "PLC Memory Reserved (Auto Station) Actual KB:"
        '
        'txtPLCMemoryMWS
        '
        Me.txtPLCMemoryMWS.Location = New System.Drawing.Point(539, 157)
        Me.txtPLCMemoryMWS.Name = "txtPLCMemoryMWS"
        Me.txtPLCMemoryMWS.Size = New System.Drawing.Size(77, 20)
        Me.txtPLCMemoryMWS.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(283, 160)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(249, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "PLC Memory Reserved (Manual Station) Actual KB:"
        '
        'cboLanguageField
        '
        Me.cboLanguageField.FormattingEnabled = True
        Me.cboLanguageField.Items.AddRange(New Object() {"Language 1", "Language 2", "Language 3"})
        Me.cboLanguageField.Location = New System.Drawing.Point(154, 208)
        Me.cboLanguageField.Name = "cboLanguageField"
        Me.cboLanguageField.Size = New System.Drawing.Size(121, 21)
        Me.cboLanguageField.TabIndex = 21
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 212)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(139, 13)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Language Description Field:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(30, 242)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(119, 13)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Administrator Password:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(28, 270)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(121, 13)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "Maintenance Password:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(49, 293)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(100, 13)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "Operator Password:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdministratorPassword
        '
        Me.txtAdministratorPassword.Location = New System.Drawing.Point(154, 239)
        Me.txtAdministratorPassword.Name = "txtAdministratorPassword"
        Me.txtAdministratorPassword.Size = New System.Drawing.Size(182, 20)
        Me.txtAdministratorPassword.TabIndex = 26
        '
        'txtMaintenancePassword
        '
        Me.txtMaintenancePassword.Location = New System.Drawing.Point(154, 265)
        Me.txtMaintenancePassword.Name = "txtMaintenancePassword"
        Me.txtMaintenancePassword.Size = New System.Drawing.Size(182, 20)
        Me.txtMaintenancePassword.TabIndex = 27
        '
        'txtOperatorPassword
        '
        Me.txtOperatorPassword.Location = New System.Drawing.Point(154, 291)
        Me.txtOperatorPassword.Name = "txtOperatorPassword"
        Me.txtOperatorPassword.Size = New System.Drawing.Size(182, 20)
        Me.txtOperatorPassword.TabIndex = 28
        '
        'btnSetL5XFolder
        '
        Me.btnSetL5XFolder.Location = New System.Drawing.Point(957, 86)
        Me.btnSetL5XFolder.Name = "btnSetL5XFolder"
        Me.btnSetL5XFolder.Size = New System.Drawing.Size(28, 21)
        Me.btnSetL5XFolder.TabIndex = 31
        Me.btnSetL5XFolder.Text = "..."
        Me.btnSetL5XFolder.UseVisualStyleBackColor = True
        '
        'txtL5X
        '
        Me.txtL5X.Location = New System.Drawing.Point(151, 87)
        Me.txtL5X.Name = "txtL5X"
        Me.txtL5X.Size = New System.Drawing.Size(798, 20)
        Me.txtL5X.TabIndex = 30
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(81, 90)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(61, 13)
        Me.Label11.TabIndex = 29
        Me.Label11.Text = "L5X Folder:"
        '
        'txtL5XBaseFileName
        '
        Me.txtL5XBaseFileName.Location = New System.Drawing.Point(151, 113)
        Me.txtL5XBaseFileName.Name = "txtL5XBaseFileName"
        Me.txtL5XBaseFileName.Size = New System.Drawing.Size(185, 20)
        Me.txtL5XBaseFileName.TabIndex = 33
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(41, 116)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(101, 13)
        Me.Label12.TabIndex = 32
        Me.Label12.Text = "L5X Base Filename:"
        '
        'chkDisableArchMgr
        '
        Me.chkDisableArchMgr.AutoSize = True
        Me.chkDisableArchMgr.Location = New System.Drawing.Point(151, 355)
        Me.chkDisableArchMgr.Name = "chkDisableArchMgr"
        Me.chkDisableArchMgr.Size = New System.Drawing.Size(410, 17)
        Me.chkDisableArchMgr.TabIndex = 34
        Me.chkDisableArchMgr.Text = "Disable Architecture Manager (This will remove the selection from the Main Menu)"
        Me.chkDisableArchMgr.UseVisualStyleBackColor = True
        Me.chkDisableArchMgr.Visible = False
        '
        'chkDisableProdTaskMgr
        '
        Me.chkDisableProdTaskMgr.AutoSize = True
        Me.chkDisableProdTaskMgr.Location = New System.Drawing.Point(151, 378)
        Me.chkDisableProdTaskMgr.Name = "chkDisableProdTaskMgr"
        Me.chkDisableProdTaskMgr.Size = New System.Drawing.Size(446, 17)
        Me.chkDisableProdTaskMgr.TabIndex = 35
        Me.chkDisableProdTaskMgr.Text = "Disable Production Task Configurator (This will remove the selection from the Mai" & _
    "n Menu)"
        Me.chkDisableProdTaskMgr.UseVisualStyleBackColor = True
        Me.chkDisableProdTaskMgr.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(783, 373)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(93, 25)
        Me.Button1.TabIndex = 36
        Me.Button1.Text = "Save"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(997, 406)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.chkDisableProdTaskMgr)
        Me.Controls.Add(Me.chkDisableArchMgr)
        Me.Controls.Add(Me.txtL5XBaseFileName)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.btnSetL5XFolder)
        Me.Controls.Add(Me.txtL5X)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtOperatorPassword)
        Me.Controls.Add(Me.txtMaintenancePassword)
        Me.Controls.Add(Me.txtAdministratorPassword)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cboLanguageField)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtPLCMemoryMWS)
        Me.Controls.Add(Me.txtPLCMemoryAWS)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtFaultPercent)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtWarningPercent)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnSetDatabase)
        Me.Controls.Add(Me.btnSetLoggingFolder)
        Me.Controls.Add(Me.txtDatabase)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtLogFolder)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form3"
        Me.Text = "System Settings"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtLogFolder As System.Windows.Forms.TextBox
    Friend WithEvents txtDatabase As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSetLoggingFolder As System.Windows.Forms.Button
    Friend WithEvents btnSetDatabase As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtWarningPercent As System.Windows.Forms.TextBox
    Friend WithEvents txtFaultPercent As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtPLCMemoryAWS As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPLCMemoryMWS As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cboLanguageField As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtAdministratorPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtMaintenancePassword As System.Windows.Forms.TextBox
    Friend WithEvents txtOperatorPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnSetL5XFolder As System.Windows.Forms.Button
    Friend WithEvents txtL5X As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtL5XBaseFileName As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents chkDisableArchMgr As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisableProdTaskMgr As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
