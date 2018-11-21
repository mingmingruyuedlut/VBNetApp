
Imports Powertrain_Task_Manager.Constants

Namespace Controllers
    ''' <summary>
    ''' Record some commom functions in this controller
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommonController
        Public Shared Function GetStringTypeLength(ByVal memberType As String) As Integer
            Dim startPosition = memberType.IndexOf("[", StringComparison.OrdinalIgnoreCase)
            Dim endPosition = memberType.IndexOf("]", StringComparison.OrdinalIgnoreCase)
            Dim typeLength = memberType.Substring(startPosition + 1, endPosition - startPosition - 1)
            Return Integer.Parse(typeLength)
        End Function

        Public Shared Function GetStationChildCount(ByVal memberType As String) As Integer
            Dim startPosition = memberType.IndexOf("(", StringComparison.OrdinalIgnoreCase)
            Dim endPosition = memberType.IndexOf(")", StringComparison.OrdinalIgnoreCase)
            Dim typeLength = 0
            If startPosition > 0 And startPosition > 0 Then
                typeLength = memberType.Substring(startPosition + 1, endPosition - startPosition - 1)
            End If
            Return Integer.Parse(typeLength)
        End Function

        Public Shared Function SupplementMemberValue(ByVal memberValue As String, ByVal memberLength As Integer) As String
            Dim memberValueLength = memberValue.Length
            If memberValueLength >= memberLength Then
                Return memberValue
            End If

            For i = memberValueLength To memberLength - 1
                memberValue = memberValue & " "
            Next
            Return memberValue
        End Function

        Public Shared Function GetDownloadMemberName(ByVal memberName As String) As String
            Dim memberNameArray = Split(memberName, ".")
            If UBound(memberNameArray) <= 0 Then
                Return String.Empty
            End If
            Dim memberNameFirstPart = memberNameArray(0)
            Dim memberNameSecondPart = memberNameArray(1)

            If memberNameFirstPart = memberNameSecondPart Then
                Return memberNameFirstPart
            Else
                Return memberName
            End If
        End Function

        Public Shared Function GetDownloadMemberName(ByVal memberName As String, ByVal baseTag As String) As String
            Dim memberNameArray = Split(memberName, ".")
            If UBound(memberNameArray) <= 0 Then
                Return String.Empty
            End If
            Dim memberNameFirstPart = memberNameArray(0)
            Dim memberNameSecondPart = memberNameArray(1)

            If memberNameFirstPart = memberNameSecondPart Then
                If (String.IsNullOrWhiteSpace(baseTag)) Then
                    Return memberNameFirstPart
                Else
                    Return baseTag & Form2Constant.Dot & memberNameFirstPart
                End If
            Else
                If (String.IsNullOrWhiteSpace(baseTag)) Then
                    Return memberName
                Else
                    Return baseTag & Form2Constant.Dot & memberName
                End If
            End If
        End Function

        Public Shared Function GetDownloadMemberValue(ByVal memberValue As String) As String
            Dim memValue = memberValue
            If (memValue = Form2Constant.True1) Then
                Return 1
            ElseIf (memValue = Form2Constant.False1) Then
                Return 0
            End If
            Return memValue
        End Function

        Public Shared Sub DownloadStringTypeMember(ByRef fileObj As Object, ByVal memberName As String, ByVal memberValue As String, ByVal memberLength As Integer)
            For i = 0 To memberLength - 1
                Dim tagValue = Asc(Mid(memberValue, i + 1, 1))
                Dim tagName = memberName & ".Data[" & i & "]"
                PythonController.DownloadTag(fileObj, tagName, tagValue)
            Next
        End Sub

        Public Shared Sub DownloadStringTypeLength(ByRef fileObj As Object, ByVal memberName As String, ByVal memberLength As Integer)
            Dim tagName = memberName & ".Len"
            Dim tagValue = memberLength
            PythonController.DownloadTag(fileObj, tagName, tagValue)
        End Sub

        Public Shared Function GenerateProductionTitle(Optional filePath As String = "") As String
            Dim title = String.Concat(ProductConstant.ProductVersion, " ", GetVersion(), " ", ProductConstant.ProductCompany)
            If String.IsNullOrWhiteSpace(filePath) Then
                Return title
            End If

            Dim lastSlashIndex = filePath.LastIndexOf("\", StringComparison.OrdinalIgnoreCase)
            Dim fileName = filePath.Substring(lastSlashIndex + 1)
            title = fileName & " - " & title
            Return title
        End Function

        Public Shared Function GetVersion() As String
            If (Deployment.Application.ApplicationDeployment.IsNetworkDeployed) Then
                Dim ver = Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion
                Dim verStr = String.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision)
                Return verStr
            Else
                Return ProductConstant.NotPublished
            End If
        End Function
    End Class
End Namespace


