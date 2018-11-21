
Namespace Models
    Public Class MenuRenameAction
        Public Property SelectedTreeNodeTag As String
        Public Property AreaName As String
        Public Property NewAreaName As String
        Public Property SectionName As String
        Public Property NewSectionName As String
        Public Property StationName As String
        Public Property NewStationName As String
        Public Sub New()

        End Sub

        Public Sub New(tag As String, areaName As String, newAreaName As String)
            SelectedTreeNodeTag = tag
            Me.AreaName = areaName
            Me.NewAreaName = newAreaName
        End Sub

        Public Sub New(tag As String, areaName As String, sectionName As String, newSectionName As String)
            SelectedTreeNodeTag = tag
            Me.AreaName = areaName
            Me.SectionName = sectionName
            Me.NewSectionName = newSectionName
        End Sub

        Public Sub New(tag As String, areaName As String, sectionName As String, stationName As String, newStationName As String)
            SelectedTreeNodeTag = tag
            Me.AreaName = areaName
            Me.SectionName = sectionName
            Me.StationName = stationName
            Me.NewStationName = newStationName
        End Sub
    End Class
End Namespace
