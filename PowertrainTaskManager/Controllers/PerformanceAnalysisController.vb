
Imports System.IO
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models

Namespace Controllers
    ''' <summary>
    ''' Generate .csv file to analysis the function performance
    ''' IsPerformanceAnalysis is the swith
    ''' Set it to 'True' will do the performance analysis
    ''' Set it to 'False' won't do the performance analysis
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PerformanceAnalysisController
        Private Shared IsPerformanceAnalysis As Boolean = False
        Private Shared paController As PerformanceAnalysisController
        Private Shared filePath As String = "C:\\PPB\\PerformanceAnalysis\\"
        Property fileName As String
        Property paModelList As Dictionary(Of PerformanceAnalysisType, PerformanceAnalysis)

        Public Sub New(ByVal fileName As String)
            Me.fileName = fileName
            Me.paModelList = New Dictionary(Of PerformanceAnalysisType, PerformanceAnalysis)
            If Not (Directory.Exists(filePath)) Then
                Directory.CreateDirectory(filePath)
            End If
        End Sub

        ''' <summary>
        ''' Star performance analysis
        ''' </summary>
        ''' <param name="paType">performance analysis type</param>
        Public Sub StartPerformanceAnalysis(ByVal paType As PerformanceAnalysisType)
            If (Me.paModelList Is Nothing) Then
                Return
            End If
            Try
                If (paModelList.ContainsKey(paType)) Then
                    paModelList.Remove(paType)
                    Return
                End If
                Dim paModel = New PerformanceAnalysis(paType)
                paModelList.Add(paType, paModel)
                WriteStartInformation(paType)
            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' End performance analysis
        ''' </summary>
        ''' <param name="paType">performance analysis type</param>
        Public Sub EndPerformanceAnalysis(ByVal paType As PerformanceAnalysisType)
            If (Me.paModelList Is Nothing) Then
                Return
            End If
            Try
                If Not (paModelList.ContainsKey(paType)) Then
                    Return
                End If

                paModelList(paType).endDateTime = Date.Now
                WriteEndInformation(paType)
                paModelList.Remove(paType)
            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Write start information
        ''' </summary>
        ''' <param name="paType">performance analysis type</param>
        Public Sub WriteStartInformation(ByVal paType As PerformanceAnalysisType)
            Try
                If Not (IsPerformanceAnalysis) Then
                    Return
                End If

                Using fs As New FileStream(filePath + fileName, FileMode.Append, FileAccess.Write)
                    Dim sw As New StreamWriter(fs)
                    sw.WriteLine("[{0}], Start at, {1}", paType.ToString(), GetDateTimeString(paModelList(paType).startDateTime))
                    sw.Close()
                End Using
            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Write end information
        ''' </summary>
        ''' <param name="paType">performance analysis type</param>
        Public Sub WriteEndInformation(ByVal paType As PerformanceAnalysisType)
            Try
                If Not (IsPerformanceAnalysis) Then
                    Return
                End If

                Using fs As New FileStream(filePath + fileName, FileMode.Append, FileAccess.Write)
                    Dim sw As New StreamWriter(fs)
                    sw.WriteLine("[{0}], End at, {1}", paType.ToString(), GetDateTimeString(paModelList(paType).endDateTime))
                    sw.Close()
                End Using
            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Init performance analysis controller with default file name
        ''' </summary>
        Public Shared Sub InitPerformanceAnalysisController()
            If (IsPerformanceAnalysis) Then
                Dim strFileName As String = Date.Now.ToString("yyyyMMddHHmmss") + ".csv"
                InitPerformanceAnalysisController(strFileName)
            End If
        End Sub

        ''' <summary>
        ''' Init performance analysis controller with file name
        ''' </summary>
        ''' <param name="fileName">file name</param>
        Public Shared Sub InitPerformanceAnalysisController(ByVal fileName As String)
            If Not (IsPerformanceAnalysis) Then
                Return
            End If

            If (paController Is Nothing) Then
                paController = New PerformanceAnalysisController(fileName)
            End If
        End Sub

        ''' <summary>
        ''' Get performance analysis controller
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetPerformanceAnalysisController() As PerformanceAnalysisController
            Return paController
        End Function

        ''' <summary>
        ''' Start performance analysis (shared sub) ****
        ''' </summary>
        ''' <param name="paType">performance analysis type</param>
        Public Shared Sub SharedStartPerformanceAnalysis(ByVal paType As PerformanceAnalysisType)
            'to-do, Start to performance analysis
            If (GetPerformanceAnalysisController() IsNot Nothing) Then
                GetPerformanceAnalysisController().StartPerformanceAnalysis(paType)
            End If
        End Sub

        ''' <summary>
        ''' End performance analysis (shared sub) ****
        ''' </summary>
        ''' <param name="paType">performance analysis type</param>
        Public Shared Sub SharedEndPerformanceAnalysis(ByVal paType As PerformanceAnalysisType)
            'to-do, End to performance analysis
            If (GetPerformanceAnalysisController() IsNot Nothing) Then
                GetPerformanceAnalysisController().EndPerformanceAnalysis(paType)
            End If
        End Sub

        ''' <summary>
        ''' Get the date string with target format
        ''' </summary>
        ''' <param name="dt">date</param>
        ''' <returns></returns>
        Public Shared Function GetDateTimeString(ByVal dt As Date) As String
            Return dt.ToString("yyyy-MM-dd HH:mm:ss fff")
        End Function

    End Class
End Namespace
