
Namespace Models
    Public Class Plant
        Public Property Id As Integer
        Public Property PlantName As String

        Public Sub New(ByVal id As Integer, ByVal plantName As String)
            Me.Id = id
            Me.PlantName = plantName
        End Sub

        Public Sub New(plantName As String)
            Me.PlantName = plantName
        End Sub

        Public Sub New()

        End Sub
    End Class
End Namespace
