Imports System.Data.OleDb
Imports System.Diagnostics
Imports System.Xml
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Controllers

Module Module1

    Public PLCArray() As String
    Public PowertrainProjectFile As String
    Public intGlobalDescriptionToUse As Integer
    Public strAreaModelDescription As String
    Public strAreaConfigurationType As String
    Public strAreaConfigurationName As String
    Public strAreaConfigurationPosition As String
    Public strUserName As String = "Operator"

    Public TagsNode As String = "/RSLogix5000Content/Controller/Tags"
    Public ModulesNode As String = "/RSLogix5000Content/Controller/Modules"
    Public ProgramsNode As String = "/RSLogix5000Content/Controller/Programs"
    Public TasksNode As String = "/RSLogix5000Content/Controller/Tasks"
    Public DataTypesNode As String = "/RSLogix5000Content/Controller/DataTypes"
    Public AOIsNodes As String = "/RSLogix5000Content/Controller/AddOnInstructionDefinitions"
    Public TaskScheduleName As String = ""

    Public strHelpPath As String = System.IO.Path.Combine(Application.StartupPath, "PPB Help.chm")

    Public Sub Log_Anything(ByVal strAnything As String)
        'This code is used for Debug purposes. When called it will log whatever to a logfile called Anything.Log
        Try
            Dim intFileNumber As Integer
            Dim strLogPath As String

            intFileNumber = FreeFile()
            strLogPath = My.Settings.LogPath
            FileOpen(intFileNumber, strLogPath & "\Anything.log", OpenMode.Append)
            Print(intFileNumber, Now() & " " & strAnything & vbCrLf)
            FileClose(intFileNumber)
            Debug.Print("ERROR-" & strAnything)
        Catch ex As Exception
            MsgBox(ex.Message)
            Debug.Print("ERROR-" & strAnything)
        End Try
    End Sub

    Public Sub Log_Anything_Andrew(ByVal methodException As String, Optional ByVal otherInfo As String = "")
        Try
            Dim intFileNumber As Integer
            Dim strLogPath As String
            Dim ST As New StackTrace(True)
            Dim SF As New StackFrame
            Dim logString As String
            Dim indent As String = Chr(9) & Chr(9) & Chr(9) 'these are just tabs for formatting

            Dim methodName As String
            Dim lineNumber As String
            Dim OrigNumber As String

            methodName = ST.GetFrame(1).GetMethod.Name
            OrigNumber = ST.GetFrame(0).GetFileLineNumber()
            lineNumber = ST.GetFrame(1).GetFileLineNumber()


            intFileNumber = FreeFile()
            strLogPath = My.Settings.LogPath
            FileOpen(intFileNumber, strLogPath & "\Anything.log", OpenMode.Append)

            logString = methodName & " LineNumber: " & lineNumber
            PrintLine(intFileNumber, Now() & "," & vbTab & logString & " Error - " & methodException)
            If otherInfo <> "" Then
                PrintLine(intFileNumber, indent & "*" & otherInfo)
            End If
            FileClose(intFileNumber)

            Debug.Print("ERROR-" & logString & " Error - " & methodException)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox("Error writing to Logfile - Please check to make sure the path is correct. Path = " & "C:\temp\", MsgBoxStyle.Critical)
        End Try

    End Sub

    Public Function GetExceptionInfo(ex As Exception) As String
        Dim Result As String
        Dim hr As Integer = Runtime.InteropServices.Marshal.GetHRForException(ex)
        Result = ex.GetType.ToString & "(0x" & hr.ToString("X8") & "): " & ex.Message & Environment.NewLine & ex.StackTrace & Environment.NewLine
        Dim st As StackTrace = New StackTrace(ex, True)
        For Each sf As StackFrame In st.GetFrames
            If sf.GetFileLineNumber() > 0 Then
                Result &= "Line:" & sf.GetFileLineNumber() & " Filename: " & IO.Path.GetFileName(sf.GetFileName) & Environment.NewLine
            End If
        Next
        Return Result
    End Function

    Public Sub ImportDataTypesToXMLFile(ByVal strMasterDataTypeFileName As String, ByVal strNonMasterDataTypeFileName As String, ByVal strMergedFileName As String, Optional ByVal isDefault As Boolean = False)
        Dim startNode As Xml.XmlNode
        Dim MasterNode As Xml.XmlNode
        Dim xChildNode As Xml.XmlNode
        Dim xMasterChildNode As Xml.XmlNode
        Dim tempMasterDTXMLDoc As Xml.XmlDocument
        Dim tempNonMasterDTXMLDoc As Xml.XmlDocument
        Dim DataTypeName As String
        Dim MasterDataTypeName As String
        Dim bolDataTypeFound As Boolean

        Try

            tempMasterDTXMLDoc = New Xml.XmlDocument
            tempMasterDTXMLDoc.Load(strMasterDataTypeFileName)
            If isDefault = False Then

                tempNonMasterDTXMLDoc = New Xml.XmlDocument
                tempNonMasterDTXMLDoc.Load(strNonMasterDataTypeFileName)

                'Update existing Data Types that are in the original file
                startNode = tempNonMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/DataTypes")
                MasterNode = tempMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/DataTypes")

                If startNode Is Nothing Then
                    Exit Sub
                End If

                For Each xChildNode In startNode.ChildNodes
                    DataTypeName = xChildNode.Attributes(0).InnerText
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(0).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            'startNode.ReplaceChild(xMasterChildNode, xChildNode)
                            Exit For
                        End If
                    Next
                Next

                'Add Data Types that are non existent in the original file
                For Each xChildNode In startNode.ChildNodes
                    bolDataTypeFound = False
                    DataTypeName = xChildNode.Attributes(0).InnerText
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(0).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            bolDataTypeFound = True
                            Exit For
                        End If
                    Next
                    If bolDataTypeFound = False Then
                        Dim ImportNode As XmlNode
                        ImportNode = tempMasterDTXMLDoc.ImportNode(xChildNode, True)
                        MasterNode.AppendChild(ImportNode)
                    End If
                Next
            End If

            tempMasterDTXMLDoc.Save(strMergedFileName)

            startNode = Nothing
            MasterNode = Nothing
            xChildNode = Nothing
            xMasterChildNode = Nothing
            tempNonMasterDTXMLDoc = Nothing
            tempMasterDTXMLDoc = Nothing
        Catch ex As Exception
            Log_Anything("ImportDataTypesToXMLFile - " & GetExceptionInfo(ex))
        End Try

    End Sub

    Public Sub ImportAOIsToXMLFile(ByVal strMasterDataTypeFileName As String, ByVal strNonMasterDataTypeFileName As String, ByVal strMergedFileName As String, Optional ByVal isDefault As Boolean = False)
        Dim startNode As Xml.XmlNode
        Dim MasterNode As Xml.XmlNode
        Dim xChildNode As Xml.XmlNode
        Dim xMasterChildNode As Xml.XmlNode
        Dim tempMasterDTXMLDoc As Xml.XmlDocument
        Dim tempNonMasterDTXMLDoc As Xml.XmlDocument
        Dim DataTypeName As String
        Dim MasterDataTypeName As String
        Dim bolDataTypeFound As Boolean

        Try

            tempMasterDTXMLDoc = New Xml.XmlDocument
            tempMasterDTXMLDoc.Load(strMasterDataTypeFileName)

            If isDefault = False Then
                tempNonMasterDTXMLDoc = New Xml.XmlDocument
                tempNonMasterDTXMLDoc.Load(strNonMasterDataTypeFileName)

                'Update existing Data Types that are in the original file
                startNode = tempNonMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/AddOnInstructionDefinitions")
                MasterNode = tempMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/AddOnInstructionDefinitions")

                If startNode Is Nothing Then
                    Exit Sub
                End If

                For Each xChildNode In startNode.ChildNodes
                    DataTypeName = xChildNode.Attributes(1).InnerText
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(1).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            'startNode.ReplaceChild(xMasterChildNode, xChildNode)
                            Exit For
                        End If
                    Next
                Next

                'Add Data Types that are non existent in the original file
                For Each xChildNode In startNode.ChildNodes
                    bolDataTypeFound = False
                    DataTypeName = xChildNode.Attributes(1).InnerText
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(1).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            bolDataTypeFound = True
                            Exit For
                        End If
                    Next
                    If bolDataTypeFound = False Then
                        Dim ImportNode As XmlNode
                        ImportNode = tempMasterDTXMLDoc.ImportNode(xChildNode, True)
                        MasterNode.AppendChild(ImportNode)
                    End If
                Next
            End If

            tempMasterDTXMLDoc.Save(strMergedFileName)

            startNode = Nothing
            MasterNode = Nothing
            xChildNode = Nothing
            xMasterChildNode = Nothing
            tempNonMasterDTXMLDoc = Nothing
            tempMasterDTXMLDoc = Nothing
        Catch ex As Exception
            Log_Anything("ImportAOIsToXMLFile - " & GetExceptionInfo(ex))
        End Try
    End Sub


    Public Sub ImportTagsToXMLFile(ByVal strMasterDataTypeFileName As String, ByVal strNonMasterDataTypeFileName As String, ByVal strMergedFileName As String, Optional ByVal isDefault As Boolean = False)
        Dim startNode As Xml.XmlNode
        Dim MasterNode As Xml.XmlNode
        Dim xChildNode As Xml.XmlNode
        Dim xMasterChildNode As Xml.XmlNode
        Dim tempMasterDTXMLDoc As Xml.XmlDocument
        Dim tempNonMasterDTXMLDoc As Xml.XmlDocument
        Dim DataTypeName As String
        Dim MasterDataTypeName As String
        Dim bolDataTypeFound As Boolean

        Try

            tempMasterDTXMLDoc = New Xml.XmlDocument
            tempMasterDTXMLDoc.Load(strMasterDataTypeFileName)

            If isDefault = False Then

                tempNonMasterDTXMLDoc = New Xml.XmlDocument
                tempNonMasterDTXMLDoc.Load(strNonMasterDataTypeFileName)

                'Update existing Data Types that are in the original file
                startNode = tempNonMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/Tags")
                MasterNode = tempMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/Tags")
                'For Each xChildNode In startNode.ChildNodes
                '    DataTypeName = xChildNode.Attributes(0).InnerText
                '    For Each xMasterChildNode In MasterNode.ChildNodes
                '        MasterDataTypeName = xMasterChildNode.Attributes(0).InnerText
                '        If DataTypeName = MasterDataTypeName Then
                '            'startNode.ReplaceChild(xMasterChildNode, xChildNode)
                '            Exit For
                '        End If
                '    Next
                'Next

                If startNode Is Nothing Then
                    Exit Sub
                End If

                'Add Data Types that are non existent in the original file
                For Each xChildNode In startNode.ChildNodes
                    bolDataTypeFound = False
                    DataTypeName = xChildNode.Attributes(0).InnerText
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(0).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            bolDataTypeFound = True
                            Exit For
                        End If
                    Next
                    If bolDataTypeFound = False Then
                        Dim ImportNode As XmlNode
                        ImportNode = tempMasterDTXMLDoc.ImportNode(xChildNode, True)
                        MasterNode.AppendChild(ImportNode)
                    End If
                Next

            End If

            tempMasterDTXMLDoc.Save(strMergedFileName)

            startNode = Nothing
            MasterNode = Nothing
            xChildNode = Nothing
            xMasterChildNode = Nothing

            tempNonMasterDTXMLDoc = Nothing
            tempMasterDTXMLDoc = Nothing
        Catch ex As Exception
            Log_Anything("ImportTagsToXMLFile - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Public Sub ImportModulesToXMLFile(ByVal strMasterDataTypeFileName As String, ByVal strNonMasterDataTypeFileName As String, ByVal strMergedFileName As String, Optional ByVal isDefault As Boolean = False)
        Dim startNode As Xml.XmlNode
        Dim MasterNode As Xml.XmlNode
        Dim xChildNode As Xml.XmlNode
        Dim xMasterChildNode As Xml.XmlNode
        Dim tempMasterDTXMLDoc As Xml.XmlDocument
        Dim tempNonMasterDTXMLDoc As Xml.XmlDocument
        Dim DataTypeName As String
        Dim MasterDataTypeName As String
        Dim bolDataTypeFound As Boolean

        Try

            tempMasterDTXMLDoc = New Xml.XmlDocument
            tempMasterDTXMLDoc.Load(strMasterDataTypeFileName)

            If isDefault = False Then
                tempNonMasterDTXMLDoc = New Xml.XmlDocument
                tempNonMasterDTXMLDoc.Load(strNonMasterDataTypeFileName)

                'Update existing Data Types that are in the original file
                startNode = tempNonMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/Modules")
                MasterNode = tempMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/Modules")

                If startNode Is Nothing Then
                    Exit Sub
                End If

                For Each xChildNode In startNode.ChildNodes
                    DataTypeName = xChildNode.Attributes(1).InnerText
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(1).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            'startNode.ReplaceChild(xMasterChildNode, xChildNode)
                            Exit For
                        End If
                    Next
                Next

                'Add Data Types that are non existent in the original file
                For Each xChildNode In startNode.ChildNodes
                    bolDataTypeFound = False
                    DataTypeName = xChildNode.Attributes(1).InnerText
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(1).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            bolDataTypeFound = True
                            Exit For
                        End If
                    Next
                    If bolDataTypeFound = False Then
                        Dim ImportNode As XmlNode
                        ImportNode = tempMasterDTXMLDoc.ImportNode(xChildNode, True)
                        MasterNode.AppendChild(ImportNode)
                    End If
                Next
            End If

            tempMasterDTXMLDoc.Save(strMergedFileName)

            startNode = Nothing
            MasterNode = Nothing
            xChildNode = Nothing
            xMasterChildNode = Nothing
            tempNonMasterDTXMLDoc = Nothing
            tempMasterDTXMLDoc = Nothing
        Catch ex As Exception
            Log_Anything("ImportModulesToXMLFile - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Public Sub ImportProgramsToXMLFile(ByVal strMasterDataTypeFileName As String, ByVal strNonMasterDataTypeFileName As String, ByVal strMergedFileName As String, Optional ByVal isDefault As Boolean = False)
        Dim startNode As Xml.XmlNode
        Dim MasterNode As Xml.XmlNode
        Dim xChildNode As Xml.XmlNode
        Dim xMasterChildNode As Xml.XmlNode
        Dim tempMasterDTXMLDoc As Xml.XmlDocument
        Dim tempNonMasterDTXMLDoc As Xml.XmlDocument
        Dim DataTypeName As String
        Dim MasterDataTypeName As String
        Dim bolDataTypeFound As Boolean

        Try
            TaskScheduleName = ""
            tempMasterDTXMLDoc = New Xml.XmlDocument
            tempMasterDTXMLDoc.Load(strMasterDataTypeFileName)

            If isDefault = False Then
                tempNonMasterDTXMLDoc = New Xml.XmlDocument
                tempNonMasterDTXMLDoc.Load(strNonMasterDataTypeFileName)

                'Update existing Data Types that are in the original file
                startNode = tempNonMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/Programs")
                MasterNode = tempMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/Programs")

                If startNode Is Nothing Then
                    Exit Sub
                End If

                For Each xChildNode In startNode.ChildNodes
                    DataTypeName = xChildNode.Attributes(1).InnerText
                    'If DataTypeName <> "" And Not bolDataTypeFound Then
                    '    bolDataTypeFound = True
                    '    TaskScheduleName = DataTypeName
                    'End If
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(1).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            'startNode.ReplaceChild(xMasterChildNode, xChildNode)
                            Exit For
                        End If
                    Next
                Next

                'Add Data Types that are non existent in the original file
                For Each xChildNode In startNode.ChildNodes
                    bolDataTypeFound = False
                    DataTypeName = xChildNode.Attributes(1).InnerText
                    If DataTypeName <> "" And TaskScheduleName = "" Then
                        TaskScheduleName = DataTypeName
                    End If
                    For Each xMasterChildNode In MasterNode.ChildNodes
                        MasterDataTypeName = xMasterChildNode.Attributes(1).InnerText
                        If DataTypeName = MasterDataTypeName Then
                            bolDataTypeFound = True
                            Exit For
                        End If
                    Next
                    If bolDataTypeFound = False Then
                        Dim ImportNode As XmlNode
                        ImportNode = tempMasterDTXMLDoc.ImportNode(xChildNode, True)
                        MasterNode.AppendChild(ImportNode)
                    End If
                Next
            End If

            tempMasterDTXMLDoc.Save(strMergedFileName)

            startNode = Nothing
            MasterNode = Nothing
            tempNonMasterDTXMLDoc = Nothing
            tempMasterDTXMLDoc = Nothing
        Catch ex As Exception
            Log_Anything("ImportProgramsToXMLFile - " & GetExceptionInfo(ex))
        End Try
    End Sub

    Public Sub ImportTasksToXMLFile(ByVal strMasterDataTypeFileName As String, ByVal strNonMasterDataTypeFileName As String, ByVal strMergedFileName As String, ByVal strTaskName As String, Optional ByVal isDefault As Boolean = False)
        Dim startNode As Xml.XmlNode
        Dim MasterNode As Xml.XmlNode
        Dim scheduledNode As Xml.XmlNode
        Dim xMasterChildNode As Xml.XmlNode
        Dim tempMasterDTXMLDoc As Xml.XmlDocument
        Dim tempNonMasterDTXMLDoc As Xml.XmlDocument
        Dim MasterDataTypeName As String
        Dim sNode As Xml.XmlNode
        Dim NewProgramNode As Xml.XmlNode

        Try

            tempMasterDTXMLDoc = New Xml.XmlDocument
            tempMasterDTXMLDoc.Load(strMasterDataTypeFileName)

            If isDefault = False Then
                tempNonMasterDTXMLDoc = New Xml.XmlDocument
                tempNonMasterDTXMLDoc.Load(strNonMasterDataTypeFileName)

                'Update existing Data Types that are in the original file
                MasterNode = tempMasterDTXMLDoc.DocumentElement.SelectSingleNode("/RSLogix5000Content/Controller/Tasks")
                For Each xMasterChildNode In MasterNode.ChildNodes
                    MasterDataTypeName = xMasterChildNode.Attributes(0).InnerText
                    If MasterDataTypeName = "MainTask" Then
                        scheduledNode = xMasterChildNode.ChildNodes(0)
                        For Each sNode In scheduledNode.ChildNodes
                            If sNode.Attributes(0).InnerText = "MainProgram" Then
                                NewProgramNode = sNode.Clone
                                NewProgramNode.Attributes(0).InnerText = strTaskName
                                Dim ImportNode As XmlNode
                                ImportNode = tempMasterDTXMLDoc.ImportNode(NewProgramNode, True)
                                scheduledNode.AppendChild(ImportNode)
                            End If
                        Next
                    End If
                Next
            End If

            tempMasterDTXMLDoc.Save(strMergedFileName)

            startNode = Nothing
            MasterNode = Nothing
            tempNonMasterDTXMLDoc = Nothing
            tempMasterDTXMLDoc = Nothing
        Catch ex As Exception
            Log_Anything("ImportTasksToXMLFile - " & GetExceptionInfo(ex))
        End Try
    End Sub
End Module
