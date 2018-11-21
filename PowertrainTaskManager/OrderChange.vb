Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Controllers
Public Class OrderChange
    Dim _allDataItems As List(Of String) = New List(Of String)
    Public From8Text As String
    Public From2SelectNodeTag

    Private Sub BtnGenerateSplitLine_Click(sender As Object, e As EventArgs) Handles BtnGenerateSplitLine.Click
        _allDataItems.Add(OrderChangeConstant.Space)
        BtnLoadListBoxData_Click()
    End Sub

    Private Sub BtnLoadListBoxData_Click()
        If (listBox1.Items.Count > 0) Then
            listBox1.Items.Clear()
        End If
        For i = 0 To _allDataItems.Count - 1
            If _allDataItems(i) <> Nothing Then
                listBox1.Items.Add(_allDataItems(i))
            End If
        Next
    End Sub

    Private Sub listBox1_DragDrop(sender As Object, e As DragEventArgs) Handles listBox1.DragDrop
        Dim point As Point
        point = listBox1.PointToClient(New Point(e.X, e.Y))
        Dim index As Integer
        index = listBox1.IndexFromPoint(point)
        If (index < 0) Then
            index = listBox1.Items.Count - 1
        End If
        Dim data = listBox1.SelectedItem
        AjustItemOrder(listBox1.SelectedIndex, index)
        listBox1.Items.RemoveAt(listBox1.SelectedIndex)
        listBox1.Items.Insert(index, data)
    End Sub

    Private Sub AjustItemOrder(ByRef sourceIndex As Integer, ByRef destinationIndex As Integer)
        Dim source = _allDataItems(sourceIndex)
        _allDataItems.RemoveAt(sourceIndex)
        _allDataItems.Insert(destinationIndex, source)
    End Sub

    Private Sub GetAllDataItems()
        If (From8Text <> Nothing) Then
            If (From8Text = OrderChangeConstant.Station) Then
                GetStation_Structures()
            Else
                GetTask_Structures(From8Text)
            End If
        End If
    End Sub

    Private Sub GetStation_Structures()
        Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
        Dim scdbSqlHelper = New SqlHelper(dbConnStr)
        Dim stationStructureController = New StationStructureController(scdbSqlHelper)

        Dim listStationStructure As List(Of StationStructure)
        ListStationStructure = stationStructureController.GetListStationStructure
        _allDataItems.Clear()
        _allDataItems = stationStructureController.GetListString(ListStationStructure)
    End Sub

    Private Sub GetTask_Structures(ByVal strTaskName As String)
        Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
        Dim scdbSqlHelper = New SqlHelper(dbConnStr)
        Dim taskStructureController = New TaskStructureController(scdbSqlHelper)
        Dim strTaskConfigName As String = GetTaskConfigName(strTaskName)
        Dim listTaskStructure As List(Of TaskStructure)
        ListTaskStructure = taskStructureController.GetListTaskStructure(strTaskConfigName)
        _allDataItems.Clear()
        _allDataItems = taskStructureController.GetListString(ListTaskStructure)
    End Sub

    Private Function GetTaskConfigName(ByVal strTaskName As String) As String
        Dim strTaskConfigName As String
        Dim intTaskStringPosition
        intTaskStringPosition = InStr(strTaskName, "_Task")
        If intTaskStringPosition <> 0 Then
            strTaskConfigName = Mid(strTaskName, 1, intTaskStringPosition + 4)
        Else
            strTaskConfigName = strTaskName
        End If

        If intTaskStringPosition = 0 Then
            intTaskStringPosition = InStr(strTaskName, " ")
            If intTaskStringPosition <> 0 Then
                strTaskConfigName = Mid(strTaskName, 1, intTaskStringPosition - 1)
                strTaskConfigName = strTaskConfigName & "_Task"
            Else
                strTaskConfigName = strTaskConfigName & "_Task"
            End If
        End If
        Return strTaskConfigName
    End Function

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getAllDataItems()
        BtnLoadListBoxData_Click()
    End Sub

    Private Sub listBox1_DragOver(sender As Object, e As DragEventArgs) Handles listBox1.DragOver
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub listBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles listBox1.MouseDown
        If (listBox1.SelectedItem = Nothing) Then
            Exit Sub
        End If
        listBox1.DoDragDrop(listBox1.SelectedItem, DragDropEffects.Move)
    End Sub

    Private Sub BtnLoadListBoxData_Click(sender As Object, e As EventArgs) Handles BtnLoadListBoxData.Click
        If (From8Text = OrderChangeConstant.Station) Then
            UpdateStationStructure()
        Else
            UpdateTaskStructure(From8Text)
        End If
    End Sub

    Private Sub UpdateStationStructure()
        If (_allDataItems.Count > 0) Then
            Dim taskName As String = OrderChangeConstant.StationConfig
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim scdbSqlHelper = New SqlHelper(dbConnStr)
            Dim stationStructureController = New StationStructureController(scdbSqlHelper)
            stationStructureController.DeleteStationStructureByBase()
            Dim listStationStructure = stationStructureController.GetListStationStructure()
            For i = 0 To _allDataItems.Count - 1
                Dim memberName = _allDataItems(i)
                If (memberName = OrderChangeConstant.Space) Then
                    stationStructureController.InsertStationStructure(taskName, i + 1)
                Else
                    For j = 0 To listStationStructure.Count
                        If (memberName = listStationStructure(j).MemberDescription1 Or memberName = listStationStructure(j).MemberName) Then
                            If (listStationStructure(j).MemberOrder = i + 1) Then
                                taskName = listStationStructure(j).Parent
                                Exit For
                            Else
                                stationStructureController.UpdateStationStructureMemberOrderbyMemberName(memberName, i + 1)
                                taskName = listStationStructure(j).Parent
                                Exit For
                            End If
                        End If
                    Next
                End If
            Next
        End If
        MessageBox.Show(OrderChangeConstant.Show)
    End Sub

    Private Sub UpdateTaskStructure(ByVal strTaskName As String)
        If (_allDataItems.Count > 0) Then
            Dim strTaskConfigName As String = GetTaskConfigName(strTaskName)
            Dim taskName As String = strTaskConfigName
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Dim scdbSqlHelper = New SqlHelper(dbConnStr)
            Dim taskStructureController = New TaskStructureController(scdbSqlHelper)
            taskStructureController.DeleteTaskStructureSpace(strTaskConfigName)
            Dim listTaskStructure = taskStructureController.GetListTaskStructure(strTaskConfigName)
            For i = 0 To _allDataItems.Count - 1
                Dim memberName = _allDataItems(i)
                If (memberName = OrderChangeConstant.Space) Then
                    taskStructureController.InsertTaskStructureSpace(strTaskConfigName, taskName, i + 1)
                Else
                    For j = 0 To listTaskStructure.Count
                        If (memberName = listTaskStructure(j).MemberDescription1 Or memberName = listTaskStructure(j).MemberName) Then
                            taskName = listTaskStructure(j).TaskName
                            Exit For
                        Else
                            taskStructureController.UpdateTaskStructureMemberOrderbyMemberName(memberName, i + 1, strTaskConfigName)
                            taskName = listTaskStructure(j).TaskName
                            Exit For
                        End If
                    Next
                End If
            Next
        End If
        MessageBox.Show(OrderChangeConstant.Show)
    End Sub

    Private Sub OrderChange_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim formfu As Form2
        formfu = Owner
        formfu.Reload(From2SelectNodeTag)
    End Sub
End Class