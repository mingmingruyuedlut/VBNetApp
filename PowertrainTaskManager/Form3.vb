Imports System.Data.OleDb

Public Class Form3
    Public Property CalledFromForm1 As Boolean = False
    Public Property CalledFromMainForm As Boolean = False
    Public Property intLanguageField As Integer

    Private Sub Form3_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        CheckUserLevel()
    End Sub

    Private Sub Form3_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Dim bNeedToSave As Boolean
            Dim nResult As Integer

            bNeedToSave = CheckIfIsDirty()

            If bNeedToSave = True Then
                nResult = MsgBox("System Settings have been modified. Do you want to save?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                If nResult = 6 Then
                    My.Settings.LogPath = txtLogFolder.Text
                    My.Settings.DatabasePath = txtDatabase.Text
                    My.Settings.L5XFolder = txtL5X.Text
                    My.Settings.L5XBaseFileName = txtL5XBaseFileName.Text

                    If txtPLCMemoryAWS.Text < 0 Then
                        MsgBox("PLC Memory Reserved CANNOT be less than 0. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                        txtPLCMemoryAWS.Text = 0
                    End If

                    If txtPLCMemoryMWS.Text < 0 Then
                        MsgBox("PLC Memory Reserved CANNOT be less than 0. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                        txtPLCMemoryMWS.Text = 0
                    End If

                    If txtWarningPercent.Text < 0 Or txtWarningPercent.Text > 100 Then
                        MsgBox("PLC Memory Warning % CANNOT be less than 0 or greater than 100. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                        txtWarningPercent.Text = 0
                    End If

                    If txtFaultPercent.Text < 0 Or txtFaultPercent.Text > 100 Then
                        MsgBox("PLC Memory Fault % CANNOT be less than 0 or greater than 100. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                        txtFaultPercent.Text = 0
                    End If

                    My.Settings.PLCMemoryToReserveAWS = txtPLCMemoryAWS.Text
                    My.Settings.PLCMemoryToReserveMWS = txtPLCMemoryMWS.Text
                    My.Settings.GlobalDescriptionFieldToUse = intLanguageField
                    My.Settings.MemoryWarning = txtWarningPercent.Text
                    My.Settings.MemoryFault = txtFaultPercent.Text

                    My.Settings.AdministratorPassword = txtAdministratorPassword.Text
                    My.Settings.MaintenancePassword = txtMaintenancePassword.Text
                    My.Settings.OperatorPassword = txtOperatorPassword.Text

                    My.Settings.Save()

                    If CalledFromForm1 = True Then
                        Form1.CheckSystemSettings()
                    End If

                    If CalledFromMainForm = True Then
                        Form5.Show()
                    End If
                End If
            End If

            intGlobalDescriptionToUse = intLanguageField

            If CalledFromMainForm = True Then
                Form5.Show()
            End If
        Catch ex As Exception
            Log_Anything("Form3_FormClosing - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub Form3_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            txtLogFolder.Text = My.Settings.LogPath
            txtDatabase.Text = My.Settings.DatabasePath
            txtL5X.Text = My.Settings.L5XFolder
            txtL5XBaseFileName.Text = My.Settings.L5XBaseFileName

            txtPLCMemoryAWS.Text = My.Settings.PLCMemoryToReserveAWS
            txtPLCMemoryMWS.Text = My.Settings.PLCMemoryToReserveMWS

            txtWarningPercent.Text = My.Settings.MemoryWarning
            txtFaultPercent.Text = My.Settings.MemoryFault

            If My.Settings.GlobalDescriptionFieldToUse = 5 Then
                cboLanguageField.Text = "Language 1"
                intLanguageField = 5
            ElseIf My.Settings.GlobalDescriptionFieldToUse = 6 Then
                cboLanguageField.Text = "Language 2"
                intLanguageField = 6
            ElseIf My.Settings.GlobalDescriptionFieldToUse = 7 Then
                cboLanguageField.Text = "Language 3"
                intLanguageField = 7
            Else
                cboLanguageField.Text = "Language 1"
                intLanguageField = 5
            End If

            txtAdministratorPassword.Text = My.Settings.AdministratorPassword
            txtMaintenancePassword.Text = My.Settings.MaintenancePassword
            txtOperatorPassword.Text = My.Settings.OperatorPassword

            chkDisableArchMgr.Checked = My.Settings.DisableArchMgr
            chkDisableProdTaskMgr.Checked = My.Settings.DisableProdTaskMgr

            CheckUserLevel()
        Catch ex As Exception
            Log_Anything("Form3_Load - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub CheckUserLevel()
        If strUserName = "Operator" Or strUserName = "Maintenance" Then
            txtL5X.Enabled = False
            txtL5XBaseFileName.Enabled = False
            txtWarningPercent.Enabled = False
            txtFaultPercent.Enabled = False
            txtPLCMemoryMWS.Enabled = False
            txtPLCMemoryAWS.Enabled = False
            cboLanguageField.Enabled = False
            txtAdministratorPassword.PasswordChar = "*"
            txtMaintenancePassword.PasswordChar = "*"
            txtOperatorPassword.PasswordChar = "*"
            txtAdministratorPassword.Enabled = False
            txtMaintenancePassword.Enabled = False
            txtOperatorPassword.Enabled = False
            chkDisableArchMgr.Visible = False
            chkDisableProdTaskMgr.Visible = False
        ElseIf strUserName = "Administrator" Then
            txtL5X.Enabled = True
            txtL5XBaseFileName.Enabled = True
            txtWarningPercent.Enabled = True
            txtFaultPercent.Enabled = True
            txtPLCMemoryMWS.Enabled = True
            txtPLCMemoryAWS.Enabled = True
            cboLanguageField.Enabled = True
            txtAdministratorPassword.PasswordChar = ""
            txtMaintenancePassword.PasswordChar = ""
            txtOperatorPassword.PasswordChar = ""
            txtAdministratorPassword.Enabled = True
            txtMaintenancePassword.Enabled = True
            txtOperatorPassword.Enabled = True
            chkDisableArchMgr.Visible = True
            chkDisableProdTaskMgr.Visible = True
        End If
    End Sub


    Private Sub btnExit_Click(sender As System.Object, e As System.EventArgs) Handles btnExit.Click
        Try
            Dim bNeedToSave As Boolean
            Dim nResult As Integer

            bNeedToSave = CheckIfIsDirty()

            If bNeedToSave = True Then
                nResult = MsgBox("System Settings have been modified. Do you want to save?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Application.ProductName)
                If nResult = 6 Then
                    My.Settings.LogPath = txtLogFolder.Text
                    My.Settings.DatabasePath = txtDatabase.Text
                    My.Settings.L5XFolder = txtL5X.Text
                    My.Settings.L5XBaseFileName = txtL5XBaseFileName.Text

                    If txtPLCMemoryAWS.Text < 0 Then
                        MsgBox("PLC Memory Reserved CANNOT be less than 0. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                        txtPLCMemoryAWS.Text = 0
                    End If

                    If txtPLCMemoryMWS.Text < 0 Then
                        MsgBox("PLC Memory Reserved CANNOT be less than 0. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                        txtPLCMemoryMWS.Text = 0
                    End If

                    If txtWarningPercent.Text < 0 Or txtWarningPercent.Text > 100 Then
                        MsgBox("PLC Memory Warning % CANNOT be less than 0 or greater than 100. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                        txtWarningPercent.Text = 0
                    End If

                    If txtFaultPercent.Text < 0 Or txtFaultPercent.Text > 100 Then
                        MsgBox("PLC Memory Fault % CANNOT be less than 0 or greater than 100. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                        txtFaultPercent.Text = 0
                    End If

                    My.Settings.PLCMemoryToReserveAWS = txtPLCMemoryAWS.Text
                    My.Settings.PLCMemoryToReserveMWS = txtPLCMemoryMWS.Text
                    My.Settings.GlobalDescriptionFieldToUse = intLanguageField
                    My.Settings.MemoryWarning = txtWarningPercent.Text
                    My.Settings.MemoryFault = txtFaultPercent.Text
                    My.Settings.AdministratorPassword = txtAdministratorPassword.Text
                    My.Settings.MaintenancePassword = txtMaintenancePassword.Text
                    My.Settings.OperatorPassword = txtOperatorPassword.Text

                    My.Settings.DisableArchMgr = chkDisableArchMgr.Checked
                    My.Settings.DisableProdTaskMgr = chkDisableProdTaskMgr.Checked

                    My.Settings.Save()

                    If CalledFromForm1 = True Then
                        Form1.CheckSystemSettings()
                    End If

                    If CalledFromMainForm = True Then
                        If My.Settings.DisableArchMgr = True Then
                            Form5.Button1.Visible = False
                        Else
                            Form5.Button1.Visible = True
                        End If

                        If My.Settings.DisableProdTaskMgr = True Then
                            Form5.Button2.Visible = False
                        Else
                            Form5.Button2.Visible = True
                        End If
                        Form5.Show()
                    End If
                    Else
                        txtLogFolder.Text = My.Settings.LogPath
                        txtDatabase.Text = My.Settings.DatabasePath

                        txtPLCMemoryAWS.Text = My.Settings.PLCMemoryToReserveAWS
                        txtPLCMemoryMWS.Text = My.Settings.PLCMemoryToReserveMWS

                        txtWarningPercent.Text = My.Settings.MemoryWarning
                        txtFaultPercent.Text = My.Settings.MemoryFault

                        If My.Settings.GlobalDescriptionFieldToUse = 5 Then
                            cboLanguageField.Text = "Language 1"
                            intLanguageField = 5
                        ElseIf My.Settings.GlobalDescriptionFieldToUse = 6 Then
                            cboLanguageField.Text = "Language 2"
                            intLanguageField = 6
                        ElseIf My.Settings.GlobalDescriptionFieldToUse = 7 Then
                            cboLanguageField.Text = "Language 3"
                            intLanguageField = 7
                        Else
                            cboLanguageField.Text = "Language 1"
                            intLanguageField = 5
                        End If

                        txtAdministratorPassword.Text = My.Settings.AdministratorPassword
                        txtMaintenancePassword.Text = My.Settings.MaintenancePassword
                        txtOperatorPassword.Text = My.Settings.OperatorPassword

                        chkDisableArchMgr.Checked = My.Settings.DisableArchMgr
                        chkDisableProdTaskMgr.Checked = My.Settings.DisableProdTaskMgr

                    End If
                End If

                intGlobalDescriptionToUse = intLanguageField

            If CalledFromMainForm = True Then
                If My.Settings.DisableArchMgr = True Then
                    Form5.Button1.Visible = False
                Else
                    Form5.Button1.Visible = True
                End If

                If My.Settings.DisableProdTaskMgr = True Then
                    Form5.Button2.Visible = False
                Else
                    Form5.Button2.Visible = True
                End If
                Form5.Show()
            End If

                Me.Close()
        Catch ex As Exception
            Log_Anything("btnExit_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Function CheckIfIsDirty() As Boolean
        Try
            If My.Settings.LogPath <> txtLogFolder.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.DatabasePath <> txtDatabase.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.L5XFolder <> txtL5X.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.L5XBaseFileName <> txtL5XBaseFileName.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.PLCMemoryToReserveAWS <> txtPLCMemoryAWS.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.PLCMemoryToReserveMWS <> txtPLCMemoryMWS.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.GlobalDescriptionFieldToUse <> intLanguageField Then
                CheckIfIsDirty = True
                Exit Function
            End If

            'If chkMultiStation.Checked = True And My.Settings.MultiStation = 0 Then
            '    CheckIfIsDirty = True
            '    Exit Function
            'End If

            'If chkMultiStation.Checked = False And My.Settings.MultiStation = 1 Then
            '    CheckIfIsDirty = True
            '    Exit Function
            'End If

            If My.Settings.AdministratorPassword <> txtAdministratorPassword.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.MaintenancePassword <> txtMaintenancePassword.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.OperatorPassword <> txtOperatorPassword.Text Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.DisableArchMgr <> chkDisableArchMgr.Checked Then
                CheckIfIsDirty = True
                Exit Function
            End If

            If My.Settings.DisableProdTaskMgr <> chkDisableProdTaskMgr.Checked Then
                CheckIfIsDirty = True
                Exit Function
            End If

            CheckIfIsDirty = False
        Catch ex As Exception
            Log_Anything("CheckIfIsDirty - " & GetExceptionInfo(ex))
            CheckIfIsDirty = True
        End Try
    End Function

    Private Sub btnSetDatabase_Click(sender As System.Object, e As System.EventArgs) Handles btnSetDatabase.Click
        Dim bDBSet As Boolean
        bDBSet = GetDatabaseFile()
    End Sub

    Private Function GetDatabaseFile() As Boolean
        Dim intBackSlashLocation As Integer
        Dim strFileName As String
        Dim strFileLocation As String

        OpenFileDialog1.Filter = "Powertrain Data File (*.pmf)|*.pmf"
        OpenFileDialog1.InitialDirectory = My.Settings.DatabasePath
        OpenFileDialog1.FileName = My.Settings.DatabasePath
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()
        Try
            If result = DialogResult.OK Then
                strFileName = UCase(OpenFileDialog1.FileName)
                intBackSlashLocation = InStrRev(strFileName, "\")
                If intBackSlashLocation <> 0 Then
                    strFileName = Mid(strFileName, intBackSlashLocation + 1, Len(strFileName) - intBackSlashLocation)
                End If
                strFileLocation = Replace(UCase(OpenFileDialog1.FileName), UCase(strFileName), "")
                'My.Settings.DatabasePath = OpenFileDialog1.FileName
                txtDatabase.Text = OpenFileDialog1.FileName
                GetDatabaseFile = True
            Else
                GetDatabaseFile = False
            End If
        Catch ex As Exception
            Log_Anything("GetDataFile - " & GetExceptionInfo(ex))
            GetDatabaseFile = False
        End Try
    End Function

    Private Sub btnSetLoggingFolder_Click(sender As System.Object, e As System.EventArgs) Handles btnSetLoggingFolder.Click
        If My.Settings.LogPath = "" Then
            FolderBrowserDialog1.SelectedPath = "C:\"
        Else
            FolderBrowserDialog1.SelectedPath = My.Settings.LogPath
        End If

        FolderBrowserDialog1.ShowNewFolderButton = True

        Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()
        Try
            If result = DialogResult.OK Then
                txtLogFolder.Text = FolderBrowserDialog1.SelectedPath
            End If
        Catch ex As Exception
            Log_Anything("btnSetLoggingFolder_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub


    Private Sub cboLanguageField_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboLanguageField.SelectedIndexChanged
        If cboLanguageField.Text = "Language 1" Then
            intLanguageField = 5
        ElseIf cboLanguageField.Text = "Language 2" Then
            intLanguageField = 6
        ElseIf cboLanguageField.Text = "Language 3" Then
            intLanguageField = 7
        End If
    End Sub

    Private Sub txtPLCMemoryAWS_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPLCMemoryAWS.TextChanged
        If Not IsNumeric(txtPLCMemoryAWS.Text) Then
            MsgBox("PLC Memory Reserved MUST be numeric. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
            txtPLCMemoryAWS.Text = 0
        End If

        If txtPLCMemoryAWS.Text < 0 Then
            MsgBox("PLC Memory Reserved CANNOT be less than 0. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
            txtPLCMemoryAWS.Text = 0
        End If
    End Sub

    Private Sub txtPLCMemoryMWS_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPLCMemoryMWS.TextChanged
        If Not IsNumeric(txtPLCMemoryMWS.Text) Then
            MsgBox("PLC Memory Reserved MUST be numeric. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
            txtPLCMemoryMWS.Text = 0
        End If

        If txtPLCMemoryMWS.Text < 0 Then
            MsgBox("PLC Memory Reserved CANNOT be less than 0. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
            txtPLCMemoryMWS.Text = 0
        End If
    End Sub

    Private Sub btnSetL5XFolder_Click(sender As System.Object, e As System.EventArgs) Handles btnSetL5XFolder.Click
        If My.Settings.L5XFolder = "" Then
            FolderBrowserDialog1.SelectedPath = "C:\"
        Else
            FolderBrowserDialog1.SelectedPath = My.Settings.L5XFolder
        End If

        FolderBrowserDialog1.ShowNewFolderButton = True

        Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()
        Try
            If result = DialogResult.OK Then
                txtL5X.Text = FolderBrowserDialog1.SelectedPath
            End If
        Catch ex As Exception
            Log_Anything("btnSetL5XFolder_Click - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim bNeedToSave As Boolean

        bNeedToSave = CheckIfIsDirty()
        If bNeedToSave = True Then
            My.Settings.LogPath = txtLogFolder.Text
            My.Settings.DatabasePath = txtDatabase.Text
            My.Settings.L5XFolder = txtL5X.Text
            My.Settings.L5XBaseFileName = txtL5XBaseFileName.Text

            If txtPLCMemoryAWS.Text < 0 Then
                MsgBox("PLC Memory Reserved CANNOT be less than 0. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                txtPLCMemoryAWS.Text = 0
            End If

            If txtPLCMemoryMWS.Text < 0 Then
                MsgBox("PLC Memory Reserved CANNOT be less than 0. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                txtPLCMemoryMWS.Text = 0
            End If

            If txtWarningPercent.Text < 0 Or txtWarningPercent.Text > 100 Then
                MsgBox("PLC Memory Warning % CANNOT be less than 0 or greater than 100. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                txtWarningPercent.Text = 0
            End If

            If txtFaultPercent.Text < 0 Or txtFaultPercent.Text > 100 Then
                MsgBox("PLC Memory Fault % CANNOT be less than 0 or greater than 100. Resetting to 0", MsgBoxStyle.Critical, Application.ProductName)
                txtFaultPercent.Text = 0
            End If

            My.Settings.PLCMemoryToReserveAWS = txtPLCMemoryAWS.Text
            My.Settings.PLCMemoryToReserveMWS = txtPLCMemoryMWS.Text
            My.Settings.GlobalDescriptionFieldToUse = intLanguageField
            My.Settings.MemoryWarning = txtWarningPercent.Text
            My.Settings.MemoryFault = txtFaultPercent.Text

            My.Settings.AdministratorPassword = txtAdministratorPassword.Text
            My.Settings.MaintenancePassword = txtMaintenancePassword.Text
            My.Settings.OperatorPassword = txtOperatorPassword.Text

            My.Settings.Save()

        End If
        MsgBox("Save Success")
    End Sub
End Class