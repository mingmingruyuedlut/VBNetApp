
Namespace Models
    Public Class MemoryUsage
        Public Property Id As Integer
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property StationName As String
        Public Property PlcType As String
        Public Property TotalMem As Integer
        Public Property TotalMemRsvd As Integer
        Public Property MemAvailable As Integer
        Public Property MemUsed As Integer
        Public Property PercentAvailable As Integer
        Public Property PercentUsed As Integer

        Public Sub New()

        End Sub

        Public Sub New(areaName As String, sectionName As String, stationName As String, plcType As String, totalMem As Integer, totalMemRsvd As Integer, memAvailable As Integer, memUsed As Integer, percentAvailable As Integer, percentUsed As Integer)
            Me.AreaName = areaName
            Me.SectionName = sectionName
            Me.StationName = stationName
            Me.PlcType = plcType
            Me.TotalMem = totalMem
            Me.TotalMemRsvd = totalMemRsvd
            Me.MemAvailable = memAvailable
            Me.MemUsed = memUsed
            Me.PercentAvailable = percentAvailable
            Me.PercentUsed = percentUsed
        End Sub
    End Class
End Namespace
