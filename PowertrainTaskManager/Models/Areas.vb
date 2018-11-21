
Namespace Models
    Public Class Areas
        Public Property Id As Integer
        Public Property AreaName As String
        
        Public Sub New(ByVal id As Integer, ByVal areaName As String)
            Me.Id = id
            Me.AreaName = areaName
        End Sub

        Public Sub New(areaName As String)
            Me.AreaName = areaName
        End Sub

        Public Sub New()

        End Sub
    End Class
End Namespace
