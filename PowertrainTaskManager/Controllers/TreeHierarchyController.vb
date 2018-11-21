
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories
Namespace Controllers
    Public Class TreeHierarchyController
        Public Shared Function GenerateTreeNodeArea(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod = GenerateTreeNode(hierDetail.AreaName, hierDetail.AreaName, TreeNodeTagConstant.AREA, TreeNodeImageIndexConstant.Area, TreeNodeImageIndexConstant.Area)
            Return nod
        End Function

        Public Shared Function GenerateTreeNodeSection(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod = GenerateTreeNode(hierDetail.SectionName, hierDetail.SectionName, TreeNodeTagConstant.SECTION, TreeNodeImageIndexConstant.Section, TreeNodeImageIndexConstant.Section)
            Return nod
        End Function

        Public Shared Function GenerateTreeNodeStation(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod = GenerateTreeNode(hierDetail.StationName, hierDetail.StationName, TreeNodeTagConstant.STATION, TreeNodeImageIndexConstant.Station, TreeNodeImageIndexConstant.Station)
            Return nod
        End Function

        Public Shared Function GenerateTreeNodeMasterFile(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod As New TreeNode()
            nod.Name = hierDetail.MasterFileName
            nod.Text = "_" & hierDetail.MasterFileName
            nod.Tag = TreeNodeTagConstant.MASTERFILE
            nod.ImageIndex = TreeNodeImageIndexConstant.MasterFile
            nod.SelectedImageIndex = TreeNodeImageIndexConstant.MasterFile
            Return nod
        End Function

        Public Shared Function GenerateTreeNodeMasterFileAttribute(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod = GenerateTreeNode(hierDetail.MasterFileAttributeName, hierDetail.MasterFileAttributeName, TreeNodeTagConstant.MASTERFILEATTRIBUTE, TreeNodeImageIndexConstant.MasterFileAttribute, TreeNodeImageIndexConstant.MasterFileAttribute)
            Return nod
        End Function

        Public Shared Function GenerateTreeNodeTask(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod = GenerateTreeNode(hierDetail.TaskName, hierDetail.TaskName, TreeNodeTagConstant.TASK, TreeNodeImageIndexConstant.Task, TreeNodeImageIndexConstant.Task)
            Return nod
        End Function

        Public Shared Function GenerateTreeNodeTaskInstance(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod As New TreeNode()
            Dim strInstance As String

            If hierDetail.TaskInstance < 10 Then
                strInstance = "0" & hierDetail.TaskInstance
            Else
                strInstance = hierDetail.TaskInstance
            End If

            nod.Name = hierDetail.TaskName
            nod.Text = strInstance & "-" & hierDetail.TaskName
            nod.Tag = "TASK|" & hierDetail.TaskInstance
            nod.ImageIndex = TreeNodeImageIndexConstant.Task
            nod.SelectedImageIndex = TreeNodeImageIndexConstant.Task
            Return nod
        End Function

        Public Shared Function GenerateTreeNodePlc(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod As New TreeNode()
            nod.Name = hierDetail.PlcName
            nod.Text = "_" & hierDetail.PlcName
            nod.Tag = TreeNodeTagConstant.PLC
            nod.ImageIndex = TreeNodeImageIndexConstant.Plc
            nod.SelectedImageIndex = TreeNodeImageIndexConstant.Plc
            Return nod
        End Function

        Public Shared Function GenerateTreeNodePlcAttribute(hierDetail As TreeNodeHierarchyValDetail) As TreeNode
            Dim nod = GenerateTreeNode(hierDetail.PlcAttributeName, hierDetail.PlcAttributeName, TreeNodeTagConstant.PLCAttribute, TreeNodeImageIndexConstant.PlcAttribute, TreeNodeImageIndexConstant.PlcAttribute)
            Return nod
        End Function

        Public Shared Function GenerateTreeNode(name As String, text As String, tag As String, imgIndex As Integer, selectedImgIndex As Integer) As TreeNode
            Dim nod As New TreeNode()
            nod.Name = name
            nod.Text = text
            nod.Tag = tag
            nod.ImageIndex = imgIndex
            nod.SelectedImageIndex = selectedImgIndex
            Return nod
        End Function



        Public Shared Function ValidateTreeHierarchy(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = True
            Select Case treeNodeVal.TreeNodeHierType
                Case TreeNodeHierarchyType.Area
                    valResult = ValidateTreeHierarchyArea(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeNodeHierarchyType.Section
                    valResult = ValidateTreeHierarchySection(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeNodeHierarchyType.Station
                    valResult = ValidateTreeHierarchyStation(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeNodeHierarchyType.MasterFile
                    valResult = ValidateTreeHierarchyMasterFile(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeNodeHierarchyType.MasterFileAttribute
                    valResult = ValidateTreeHierarchyMasterFileAttribute(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeNodeHierarchyType.Task
                    valResult = ValidateTreeHierarchyTask(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeNodeHierarchyType.TaskInstance
                    valResult = ValidateTreeHierarchyTaskInstance(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeNodeHierarchyType.Plc
                    valResult = ValidateTreeHierarchyPlc(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeNodeHierarchyType.PlcAttribute
                    valResult = ValidateTreeHierarchyPlcAttribute(treeNodeVal, treeViewObj, valResult, parentNode)
            End Select
            Return valResult
        End Function

        ''' <summary>
        ''' Validate tree hierarchy area node
        ''' </summary>
        ''' <param name="treeNodeVal">tree node hierarchy validation</param>
        ''' <param name="treeViewObj">tree view obj</param>
        ''' <param name="valMsg">validation message</param>
        ''' <returns></returns>
        Public Shared Function ValidateTreeHierarchyArea(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim areaNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.AreaName, False).FirstOrDefault()
            If areaNode IsNot Nothing
                valMsg = "Area Name already defined"
                Return False
            End If

            parentNode = treeViewObj.Nodes(0)
            Return True
        End Function

        ''' <summary>
        ''' Validate tree hierarchy section node
        ''' </summary>
        ''' <param name="treeNodeVal">tree node hierarchy validation</param>
        ''' <param name="treeViewObj">tree view obj</param>
        ''' <param name="valMsg">validation message</param>
        ''' <returns></returns>
        Public Shared Function ValidateTreeHierarchySection(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim areaNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.AreaName, False).FirstOrDefault()
            If areaNode Is Nothing
                Return False
            End If
            Dim sectionNode = areaNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.SectionName, False).FirstOrDefault()
            If sectionNode IsNot Nothing
                valMsg = "Section Name already defined for this Area"
                Return False
            End If

            parentNode = areaNode
            Return True
        End Function

        ''' <summary>
        ''' Validate tree hierarchy station node
        ''' </summary>
        ''' <param name="treeNodeVal">tree node hierarchy validation</param>
        ''' <param name="treeViewObj">tree view obj</param>
        ''' <param name="valMsg">validation message</param>
        ''' <returns></returns>
        Public Shared Function ValidateTreeHierarchyStation(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim areaNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.AreaName, False).FirstOrDefault()
            If areaNode Is Nothing
                Return False
            End If
            Dim sectionNode = areaNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.SectionName, False).FirstOrDefault()
            If sectionNode Is Nothing
                Return False
            End If
            Dim stationNode = sectionNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.StationName, False).FirstOrDefault()
            If stationNode IsNot Nothing
                valMsg = "Station Name already defined for this Area\Section"
                Return False
            End If

            parentNode = sectionNode
            Return True
        End Function

        Public Shared Function ValidateTreeHierarchyMasterFile(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = True
            Select Case treeNodeVal.TreType
                Case TreeType.ProjectFileTree
                    valResult = ValidateTreeHierarchyMasterFileOfProjectTree(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeType.MasterFileTree
                    valResult = ValidateTreeHierarchyMasterFileOfMasterFileTree(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeType.PlcTree
                    valResult = ValidateTreeHierarchyMasterFileOfProjectTree(treeNodeVal, treeViewObj, valResult, parentNode)
            End Select
            Return valResult
        End Function

        Public Shared Function ValidateTreeHierarchyMasterFileOfProjectTree(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim areaNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.AreaName, False).FirstOrDefault()
            If areaNode Is Nothing Then
                Return False
            End If
            Dim sectionNode = areaNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.SectionName, False).FirstOrDefault()
            If sectionNode Is Nothing Then
                Return False
            End If
            Dim stationNode = sectionNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.StationName, False).FirstOrDefault()
            If stationNode Is Nothing Then
                Return False
            End If

            parentNode = stationNode
            Return True
        End Function

        Public Shared Function ValidateTreeHierarchyMasterFileOfMasterFileTree(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim mfNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.MasterFileName, False).FirstOrDefault()
            If mfNode IsNot Nothing Then
                valMsg = "Master File Name already defined"
                Return False
            End If

            parentNode = treeViewObj.Nodes(0)
            Return True
        End Function

        Public Shared Function ValidateTreeHierarchyMasterFileAttribute(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim mfNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.MasterFileName, False).FirstOrDefault()
            If mfNode Is Nothing Then
                Return False
            End If

            Dim mfAttrNode = mfNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.MasterFileAttributeName, False).FirstOrDefault()
            If mfAttrNode IsNot Nothing Then
                valMsg = "Attribute Name already defined for this Master File"
                Return False
            End If

            parentNode = mfNode
            Return True
        End Function


        Public Shared Function ValidateTreeHierarchyTask(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = True
            Select Case treeNodeVal.TreType
                Case TreeType.ProjectFileTree
                    valResult = ValidateTreeHierarchyTaskOfProjectTree(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeType.MasterFileTree
                    valResult = ValidateTreeHierarchyTaskOfMasterFileTree(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeType.PlcTree
                    valResult = ValidateTreeHierarchyTaskOfProjectTree(treeNodeVal, treeViewObj, valResult, parentNode)
            End Select
            Return valResult
        End Function

        Public Shared Function ValidateTreeHierarchyTaskOfProjectTree(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim areaNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.AreaName, False).FirstOrDefault()
            If areaNode Is Nothing Then
                Return False
            End If
            Dim sectionNode = areaNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.SectionName, False).FirstOrDefault()
            If sectionNode Is Nothing Then
                Return False
            End If
            Dim stationNode = sectionNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.StationName, False).FirstOrDefault()
            If stationNode Is Nothing Then
                Return False
            End If
            Dim sameNameMasterFileNodes = stationNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.MasterFileName, False)
            If sameNameMasterFileNodes Is Nothing Then
                Return False
            End If
            Dim masterFileNode As TreeNode = sameNameMasterFileNodes.Cast(Of Object).FirstOrDefault(Function(x) x.Tag.Equals(TreeNodeTagConstant.MASTERFILE))
            If masterFileNode Is Nothing Then
                Return False
            End If
            Dim taskNode = masterFileNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.TaskName, False).FirstOrDefault()
            If taskNode IsNot Nothing Then
                valMsg = "Task Name already defined for this Area\Section\Station\MasterFile"
                Return False
            End If

            parentNode = masterFileNode
            Return True
        End Function

        Public Shared Function ValidateTreeHierarchyTaskOfMasterFileTree(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim mfNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.MasterFileName, False).FirstOrDefault()
            If mfNode Is Nothing Then
                Return False
            End If

            Dim taskNode = mfNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.TaskName, False).FirstOrDefault()
            If taskNode IsNot Nothing Then
                valMsg = "Task Name already defined"
                Return False
            End If

            parentNode = mfNode
            Return True
        End Function

        Public Shared Function ValidateTreeHierarchyTaskInstance(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim areaNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.AreaName, False).FirstOrDefault()
            If areaNode Is Nothing
                Return False
            End If
            Dim sectionNode = areaNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.SectionName, False).FirstOrDefault()
            If sectionNode Is Nothing
                Return False
            End If
            Dim stationNode = sectionNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.StationName, False).FirstOrDefault()
            If stationNode Is Nothing
                Return False
            End If
            Dim sameNameMasterFileNodes = stationNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.MasterFileName, False)
            If sameNameMasterFileNodes Is Nothing
                Return False
            End If
            Dim masterFileNode As TreeNode = sameNameMasterFileNodes.Cast(Of Object).FirstOrDefault(Function(x) x.Tag.Equals(TreeNodeTagConstant.MASTERFILE))
            If masterFileNode Is Nothing
                Return False
            End If
            Dim taskNode = masterFileNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.TaskName, False).FirstOrDefault()
            If taskNode Is Nothing
                Return False
            End If
            For Each taskInstanceNode In taskNode.Nodes
                If taskInstanceNode.Tag.Equals("TASK|" & treeNodeVal.TreeNodeHierValDetail.TaskInstance)Then
                    valMsg = "Task Instance Name already defined for this Area\Section\Station\MasterFile\Task"
                    Return False
                End If
            Next

            parentNode = taskNode
            Return True
        End Function

        Public Shared Function ValidateTreeHierarchyPlc(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = True
            Select Case treeNodeVal.TreType
                Case TreeType.ProjectFileTree
                    valResult = ValidateTreeHierarchyPlcOfProjectTree(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeType.MasterFileTree
                    valResult = ValidateTreeHierarchyPlcOfProjectTree(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeType.PlcTree
                    valResult = ValidateTreeHierarchyPlcOfPlcTree(treeNodeVal, treeViewObj, valResult, parentNode)
            End Select
            Return valResult
        End Function

        Public Shared Function ValidateTreeHierarchyPlcOfProjectTree(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim areaNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.AreaName, False).FirstOrDefault()
            If areaNode Is Nothing
                Return False
            End If
            Dim sectionNode = areaNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.SectionName, False).FirstOrDefault()
            If sectionNode Is Nothing
                Return False
            End If
            Dim stationNode = sectionNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.StationName, False).FirstOrDefault()
            If stationNode Is Nothing
                Return False
            End If
            Dim plcNode = stationNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.PlcName, False).FirstOrDefault()
            If plcNode IsNot Nothing
                valMsg = "PLC Name already defined for this Area\Section\Station"
                Return False
            End If

            parentNode = stationNode
            Return True
        End Function

        Public Shared Function ValidateTreeHierarchyPlcOfPlcTree(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim plcNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.PlcName, False).FirstOrDefault()
            If plcNode IsNot Nothing AndAlso plcNode.Tag = TreeNodeTagConstant.PLC Then
                valMsg = "PLC Name already defined"
                Return False
            End If

            parentNode = treeViewObj.Nodes(0)
            Return True
        End Function

        Public Shared Function ValidateTreeHierarchyPlcAttribute(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = True
            Select Case treeNodeVal.TreType
                Case TreeType.ProjectFileTree
                Case TreeType.MasterFileTree
                    valResult = ValidateTreeHierarchyPlcAttributeOfProjectTree(treeNodeVal, treeViewObj, valResult, parentNode)
                Case TreeType.PlcTree
                    valResult = ValidateTreeHierarchyPlcAttributeOfPlcTree(treeNodeVal, treeViewObj, valResult, parentNode)
            End Select
            Return valResult
        End Function

        Public Shared Function ValidateTreeHierarchyPlcAttributeOfProjectTree(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim areaNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.AreaName, False).FirstOrDefault()
            If areaNode Is Nothing
                Return False
            End If
            Dim sectionNode = areaNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.SectionName, False).FirstOrDefault()
            If sectionNode Is Nothing
                Return False
            End If
            Dim stationNode = sectionNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.StationName, False).FirstOrDefault()
            If stationNode Is Nothing
                Return False
            End If
            Dim plcNode = stationNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.PlcName, False).FirstOrDefault()
            If plcNode Is Nothing
                Return False
            End If
            Dim plcAttrNode = plcNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.PlcAttributeName, False).FirstOrDefault()
            If plcAttrNode IsNot Nothing
                valMsg = "PLC Name already defined for this Area\Section\Station"
                Return False
            End If

            parentNode = plcNode
            Return True
        End Function
        
        Public Shared Function ValidateTreeHierarchyPlcAttributeOfPlcTree(treeNodeVal As TreeNodeHierarchyValidation, ByRef treeViewObj As TreeView, ByRef valMsg As String, ByRef parentNode As TreeNode) As Boolean
            Dim valResult = ValidateTreeNodeHierarchyDetail(treeNodeVal.TreeNodeHierValDetail, valMsg)
            If valResult = False Then
                Return False
            End If

            Dim plcNode = treeViewObj.Nodes(0).Nodes.Find(treeNodeVal.TreeNodeHierValDetail.PlcName, False).FirstOrDefault()
            If plcNode Is Nothing
                Return False
            End If
            Dim plcAttrNode = plcNode.Nodes.Find(treeNodeVal.TreeNodeHierValDetail.PlcAttributeName, False).FirstOrDefault()
            If plcAttrNode IsNot Nothing Then
                valMsg = "Attribute Name already defined for this PLC"
                Return False
            End If
            
            parentNode = plcNode
            Return True
        End Function


        ''' <summary>
        ''' Validate tree node hierarchy details, the node is empty or not
        ''' </summary>
        ''' <param name="hierarchyValidation">hierarchy validation</param>
        ''' <param name="valMsg">validation message</param>
        ''' <returns></returns>
        Public Shared Function ValidateTreeNodeHierarchyDetail(ByVal hierarchyValidation As TreeNodeHierarchyValDetail, ByRef valMsg As String) As Boolean
            If String.IsNullOrWhiteSpace(hierarchyValidation.AreaName) Then
                valMsg = "Area Name cannot be empty"
                Return False
            End If

            If String.IsNullOrWhiteSpace(hierarchyValidation.SectionName) Then
                valMsg = "Section Name cannot be empty"
                Return False
            End If

            If String.IsNullOrWhiteSpace(hierarchyValidation.StationName) Then
                valMsg = "Station Name cannot be empty"
                Return False
            End If

            If String.IsNullOrWhiteSpace(hierarchyValidation.TaskName) Then
                valMsg = "Task Name cannot be empty"
                Return False
            End If

            If String.IsNullOrWhiteSpace(hierarchyValidation.MasterFileName) Then
                valMsg = "Master File Name cannot be empty"
                Return False
            End If

            If String.IsNullOrWhiteSpace(hierarchyValidation.MasterFileAttributeName) Then
                valMsg = "Master File Attribute Name cannot be empty"
                Return False
            End If

            If String.IsNullOrWhiteSpace(hierarchyValidation.PlcName) Then
                valMsg = "Plc Name cannot be empty"
                Return False
            End If

            If String.IsNullOrWhiteSpace(hierarchyValidation.PlcAttributeName) Then
                valMsg = "Plc Attribute Name cannot be empty"
                Return False
            End If

            Return True
        End Function

    End Class
End Namespace
