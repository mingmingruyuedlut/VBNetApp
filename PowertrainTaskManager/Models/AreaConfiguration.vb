
Namespace Models
    Public Class AreaConfiguration
        Public Property AreaName As String
        Public Property ModelNumber As String
        Public Property ModelName As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal area As String, ByVal modelNumber As String, ByVal modelName As String)
            AreaName = area
            Me.ModelNumber = modelNumber
            Me.ModelName = modelName
        End Sub
    End Class

    Public Class AreaConfigurationGroup
        Public Property ModelName As String
        Public Property AreaConfigurationList As List(Of AreaConfiguration)

        Public Sub New()
        End Sub

        'This is for area configuration validation
        Public Sub New(ByVal modelName As String, ByVal areaConfigList As List(Of AreaConfiguration))
            Me.ModelName = modelName
            Me.AreaConfigurationList = areaConfigList
        End Sub
    End Class
End Namespace

