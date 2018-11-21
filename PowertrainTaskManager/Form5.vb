Public Class Form5

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        Form2.Hide()
        Form1.Show()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Hide()
        Form1.Hide()
        Form2.Show()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Application.Exit()
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Me.Hide()
        Form3.CalledFromForm1 = False
        Form3.CalledFromMainForm = True
        Form3.Show()
    End Sub

    Private Sub Form5_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        If My.Settings.DisableArchMgr = True Then
            Button1.Visible = False
        Else
            Button1.Visible = True
        End If

        If My.Settings.DisableProdTaskMgr = True Then
            Button2.Visible = False
        Else
            Button2.Visible = True
        End If
    End Sub

    Private Sub Form5_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cboUser.Text = "Operator"
        strUserName = "Operator"
        txtPassword.Text = ""

        If My.Settings.DisableArchMgr = True Then
            Button1.Visible = False
        End If

        If My.Settings.DisableProdTaskMgr = True Then
            Button2.Visible = False
        End If

        If System.Diagnostics.Debugger.IsAttached Then
            Button1.Enabled = True
            Button2.Enabled = True
            Button4.Enabled = True
            strUserName = "Administrator"
        End If

        intGlobalDescriptionToUse = My.Settings.GlobalDescriptionFieldToUse
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click

        If Button5.Text = "Login" Then
            Button1.Enabled = False
            Button2.Enabled = False
            Button4.Enabled = True

            strUserName = "Operator"

            If cboUser.Text = "Administrator" Then
                If txtPassword.Text = My.Settings.AdministratorPassword Then
                    Button1.Enabled = True
                    Button2.Enabled = True
                    'Button4.Enabled = True
                    strUserName = "Administrator"
                    Button5.Text = "Logout"
                    cboUser.Enabled = False
                    txtPassword.Enabled = False
                Else
                    MsgBox("Incorrect Password entered for user.", MsgBoxStyle.Information, Application.ProductName)
                End If
            ElseIf cboUser.Text = "Maintenance" Then
                If txtPassword.Text = My.Settings.MaintenancePassword Then
                    Button1.Enabled = True
                    Button2.Enabled = True
                    strUserName = "Maintenance"
                    Button5.Text = "Logout"
                    cboUser.Enabled = False
                    txtPassword.Enabled = False
                Else
                    MsgBox("Incorrect Password entered for user.", MsgBoxStyle.Information, Application.ProductName)
                End If
            ElseIf cboUser.Text = "Operator" Then
                If txtPassword.Text = My.Settings.OperatorPassword Then
                    Button1.Enabled = True
                    Button2.Enabled = True
                    strUserName = "Operator"
                    Button5.Text = "Logout"
                    cboUser.Enabled = False
                    txtPassword.Enabled = False
                Else
                    MsgBox("Incorrect Password entered for user.", MsgBoxStyle.Information, Application.ProductName)
                End If
            End If
        Else
            Button1.Enabled = False
            Button2.Enabled = False
            'Button4.Enabled = False
            cboUser.Enabled = True
            txtPassword.Enabled = True
            cboUser.Text = "Operator"
            strUserName = "Operator"
            txtPassword.Text = ""
            Button5.Text = "Login"
        End If

    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        'ImportDataTypesToXMLFile("C:\Temp\XMLTest\ProductionImport.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        'ImportAOIsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        'ImportTagsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        'ImportModulesToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        'ImportProgramsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        'ImportTasksToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X", "Test_Task")

        ImportDataTypesToXMLFile("C:\Temp\XMLTest\ProductionImport.L5X", "C:\Temp\XMLTest\Vision_Task.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportAOIsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Vision_Task.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportTagsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Vision_Task.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportModulesToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Vision_Task.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportProgramsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Vision_Task.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportTasksToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Vision_Task.L5X", "C:\Temp\XMLTest\MergedFile.L5X", "Vision_Task")

        ImportDataTypesToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportAOIsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportTagsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportModulesToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportProgramsToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X")
        ImportTasksToXMLFile("C:\Temp\XMLTest\MergedFile.L5X", "C:\Temp\XMLTest\Test_Task_CTS_V5.1_20160527.L5X", "C:\Temp\XMLTest\MergedFile.L5X", "Test_Task")


        MsgBox("Complete")
    End Sub
End Class