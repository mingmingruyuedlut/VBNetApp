Imports System.Xml
Imports System.IO
Imports System.Text

Module XML_Import

    Public Sub XML_Parser()
        Try
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            Dim member_nodelist As XmlNodeList
            Dim m_node As XmlNode
            Dim member_node As XmlNode

            'Create the XML Document
            m_xmld = New XmlDocument()
            'Load the Xml file
            m_xmld.Load("C:\Temp\Test.l5x")
            'Get the list of name nodes 
            m_nodelist = m_xmld.SelectNodes("/RSLogix5000Content/Controller/DataTypes/DataType")
            'Loop through the nodes
            For Each m_node In m_nodelist
                'Get the Gender Attribute Value
                Dim DataTypeName = m_node.Attributes.GetNamedItem("Name").Value
                'Get the firstName Element Value

                If DataTypeName = "Config" Then
                    MsgBox("Found It")
                    member_nodelist = m_node.SelectNodes("Members/Member")
                    For Each member_node In member_nodelist
                        Dim MemberName = member_node.Attributes.GetNamedItem("Name").Value
                        If InStr(MemberName, "_Task") Then
                            MsgBox(MemberName)
                        End If
                    Next
                End If

                'Dim firstNameValue = m_node.ChildNodes.Item(0).InnerText
                'Get the lastName Element Value
                'Dim lastNameValue = m_node.ChildNodes.Item(1).InnerText
                'Write Result to the Console
                'Console.Write("Gender: " & genderAttribute _
                '  & " FirstName: " & firstNameValue & " LastName: " _
                '  & lastNameValue)
                'Console.Write(vbCrLf)
            Next
        Catch errorVariable As Exception
            'Error trapping
            Console.Write(errorVariable.ToString())
        End Try

    End Sub

End Module
