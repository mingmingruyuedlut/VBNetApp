
Imports Powertrain_Task_Manager.Enums

Namespace Models
    Public Class PerformanceAnalysis
        Property paType As PerformanceAnalysisType
        Property startDateTime As Date
        Property endDateTime As Date

        Public Sub New(ByVal paType As PerformanceAnalysisType)
            Me.paType = paType
            Me.startDateTime = Date.Now
        End Sub
    End Class
End Namespace

