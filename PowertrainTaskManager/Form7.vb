Public Class Form7

    Private Sub tmrProgress_Tick(sender As System.Object, e As System.EventArgs) Handles tmrProgress.Tick
        Dim LastTimeValue As Integer
        LastTimeValue = ProgressBar1.Value

        LastTimeValue += 1
        If LastTimeValue > ProgressBar1.Maximum Then
            ProgressBar1.Value = 0
        Else
            ProgressBar1.Value += 1
        End If

    End Sub

    Private Sub Form7_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        tmrProgress.Enabled = True
    End Sub

    Private Sub lblInformation_Click(sender As System.Object, e As System.EventArgs) Handles lblInformation.Click

    End Sub
End Class