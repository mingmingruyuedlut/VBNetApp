Public Class Form6
    Dim _strControlString As String
    Dim _intControlValue As Integer
    Dim _intControlPosition As Integer
    Dim _bControlIsCheckbox As Boolean
    Dim _tmpTagNameAndDescription() As String
    Dim _intControlMark As Integer

    Private Sub btnClose_Click(sender As System.Object, e As EventArgs) Handles btnClose.Click
        _strControlString = ""
        _intControlValue = 0
        _intControlPosition = 0
        _bControlIsCheckbox = False
        For Each ctrl In Controls
            If ctrl.Tag <> "BASE" Then
                If TypeOf ctrl Is TextBox Then
                    If _strControlString = "" Then
                        _strControlString = ctrl.Tag & "," & ctrl.Text
                    Else
                        _strControlString = _strControlString & "|" & ctrl.Tag & "," & ctrl.Text
                    End If
                ElseIf TypeOf ctrl Is CheckBox Then
                    _bControlIsCheckbox = True
                    If _strControlString = "" Then
                        _strControlString = ctrl.Tag & "," & ctrl.checked
                    Else
                        _strControlString = _strControlString & "|" & ctrl.Tag & "," & ctrl.checked
                    End If

                    If _intControlPosition = 0 Then
                        If ctrl.Checked = True Then
                            _intControlMark = 1
                        Else
                            _intControlMark = 0
                        End If
                    ElseIf _intControlPosition = 1 Then
                        If ctrl.Checked = True Then
                            _intControlValue = 1
                        Else
                            _intControlValue = 0
                        End If
                    Else
                        If ctrl.Checked = True Then
                            _intControlValue = _intControlValue + (2 ^ (_intControlPosition - 1))
                        End If
                    End If
                    _intControlPosition += 1
                End If
            End If
        Next
        If _intControlMark = 1 Then
            _intControlValue = -_intControlValue
        End If


        For Each ctrl In Form2.GroupBox1.Controls
            If ctrl.Tag = Tag Then
                If _bControlIsCheckbox = False Then
                    ctrl.AccessibleDescription = _strControlString
                Else
                    _tmpTagNameAndDescription = Split(ctrl.Tag, "|")
                    ctrl.AccessibleDescription = _tmpTagNameAndDescription(0) & "," & _intControlValue
                End If
                Exit For
            End If
        Next

        For Each ctrl In Form2.GroupBox2.Controls
            If ctrl.Tag = Tag Then
                If _bControlIsCheckbox = False Then
                    ctrl.AccessibleDescription = _strControlString
                Else
                    _tmpTagNameAndDescription = Split(ctrl.Tag, "|")
                    ctrl.AccessibleDescription = _tmpTagNameAndDescription(0) & "," & _intControlValue
                End If
                Exit For
            End If
        Next

        'MsgBox(Me.Width)
        Close()
    End Sub

    Private Sub Form6_Load(sender As System.Object, e As EventArgs) Handles MyBase.Load
        Left = Form2.SplitContainer2.Left '+ (Form2.Width / 2)
        Top = Form2.SplitContainer2.Top '+ (Form2.Height / 2) - Me.Height
    End Sub
End Class