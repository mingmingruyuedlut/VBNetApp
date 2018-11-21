
Namespace Models
    Public Class SectionModel
        Public Property Id As Integer
        Public Property AreaName As String
        Public Property SectionName As String

        Public Sub New()

        End Sub

        Public Sub New(areaName As String, sectionName As String)
            Me.AreaName = areaName
            Me.SectionName = sectionName
        End Sub

        Public Sub New(id As Integer, areaName As String, sectionName As String)
            Me.Id = id
            Me.AreaName = areaName
            Me.SectionName = sectionName
        End Sub
    End Class
End Namespace

