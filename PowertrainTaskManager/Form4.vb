' Compile this example using the following command line:
' vbc basicsplitcontainer.vb /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:System.dll /r:System.Data.dll
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports Microsoft.VisualBasic
Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms
Imports Powertrain_Task_Manager.Controllers

Public Class Form4
    Inherits System.Windows.Forms.Form
    Public strMasterFile As String
    Public strPlantName As String

    Dim muControllerMaster As MemoryUsageController

    Public Sub New()
        InitializeComponent()
        InitAllControllers()
    End Sub 'New

    Private Sub InitializeComponent()
        Me.DataSet1 = New System.Data.DataSet()
        Me.ReportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer()
        CType(Me.DataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataSet1
        '
        Me.DataSet1.DataSetName = "NewDataSet"
        '
        'ReportViewer1
        '
        Me.ReportViewer1.LocalReport.DisplayName = "Test"
        Me.ReportViewer1.LocalReport.ReportEmbeddedResource = ""
        Me.ReportViewer1.LocalReport.ReportPath = "Report1.rdlc"
        Me.ReportViewer1.Location = New System.Drawing.Point(51, 12)
        Me.ReportViewer1.Name = "ReportViewer1"
        Me.ReportViewer1.Size = New System.Drawing.Size(967, 620)
        Me.ReportViewer1.TabIndex = 0
        '
        'Form4
        '
        Me.ClientSize = New System.Drawing.Size(1071, 691)
        Me.Controls.Add(Me.ReportViewer1)
        Me.Name = "Form4"
        Me.Text = "PPB - Task Manager - Reporting"
        CType(Me.DataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Sub InitAllControllers()
        'Init controllers used in this form
        muControllerMaster = New MemoryUsageController()
        muControllerMaster.InitMaster()
    End Sub

    <STAThread()> _
    Shared Sub Main()
        Application.Run(New Form1())
    End Sub

    Private Sub Form4_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Form1.Show()
        Form1.Refresh()
        'Me.Close()
    End Sub 'Main

    Private Sub Form4_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            Dim muDataTable = muControllerMaster.GetMemoryUsageDataTable()

            With ReportViewer1
                .LocalReport.DataSources.Clear()
                .ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
                .LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("dsMemoryUsage", muDataTable))
            End With

            Dim rpPlantName As New ReportParameter()
            Dim rpWarningPercent As New ReportParameter()
            Dim rpErrorPercent As New ReportParameter

            Dim warningPercent As String
            Dim errorPercent As String


            rpPlantName.Name = "PlantName"
            rpPlantName.Values.Add(strPlantName)

            warningPercent = My.Settings.MemoryWarning
            errorPercent = My.Settings.MemoryFault

            rpWarningPercent.Name = "WarningPercent"
            rpWarningPercent.Values.Add(warningPercent)

            rpErrorPercent.Name = "ErrorPercent"
            rpErrorPercent.Values.Add(errorPercent)

            'Set the report parameters for the report
            Dim parameters() As ReportParameter = {rpPlantName, rpWarningPercent, rpErrorPercent}

            ReportViewer1.LocalReport.SetParameters(parameters)

            'Refresh the report
            Me.ReportViewer1.RefreshReport()

        Catch ex As Exception
            Log_Anything("Form4_Load - " & GetExceptionInfo(ex))
        End Try
    End Sub
End Class 'Form4
