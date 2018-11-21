<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OrderChange
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
        Me.BtnGenerateSplitLine = New System.Windows.Forms.Button()
        Me.BtnLoadListBoxData = New System.Windows.Forms.Button()
        Me.listBox1 = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'BtnGenerateSplitLine
        '
        Me.BtnGenerateSplitLine.Location = New System.Drawing.Point(222, 404)
        Me.BtnGenerateSplitLine.Name = "BtnGenerateSplitLine"
        Me.BtnGenerateSplitLine.Size = New System.Drawing.Size(112, 23)
        Me.BtnGenerateSplitLine.TabIndex = 5
        Me.BtnGenerateSplitLine.Text = "Generate Split  Line"
        Me.BtnGenerateSplitLine.UseVisualStyleBackColor = True
        '
        'BtnLoadListBoxData
        '
        Me.BtnLoadListBoxData.Location = New System.Drawing.Point(89, 404)
        Me.BtnLoadListBoxData.Name = "BtnLoadListBoxData"
        Me.BtnLoadListBoxData.Size = New System.Drawing.Size(109, 23)
        Me.BtnLoadListBoxData.TabIndex = 4
        Me.BtnLoadListBoxData.Text = "Save"
        Me.BtnLoadListBoxData.UseVisualStyleBackColor = True
        '
        'listBox1
        '
        Me.listBox1.AllowDrop = True
        Me.listBox1.FormattingEnabled = True
        Me.listBox1.Location = New System.Drawing.Point(89, 28)
        Me.listBox1.Name = "listBox1"
        Me.listBox1.ScrollAlwaysVisible = True
        Me.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.listBox1.Size = New System.Drawing.Size(245, 329)
        Me.listBox1.TabIndex = 3
        '
        'OrderChange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 454)
        Me.Controls.Add(Me.BtnGenerateSplitLine)
        Me.Controls.Add(Me.BtnLoadListBoxData)
        Me.Controls.Add(Me.listBox1)
        Me.Name = "OrderChange"
        Me.Text = "OrderChange"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents BtnGenerateSplitLine As System.Windows.Forms.Button
    Private WithEvents BtnLoadListBoxData As System.Windows.Forms.Button
    Private WithEvents listBox1 As System.Windows.Forms.ListBox
End Class
