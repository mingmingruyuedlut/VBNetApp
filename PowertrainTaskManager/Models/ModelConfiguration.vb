
Namespace Models
    Public Class ModelConfiguration
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property StationName As String
        Public Property TaskName As String
        Public Property TaskInstance As String
        Public Property MemberName As String
        Public Property MemberValue As String
        Public Property ModelInstance As String
        Public Property MemberType As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal area As String, ByVal section As String, ByVal station As String, ByVal taskname As String, ByVal taskInstance As String, ByVal memberName As String, ByVal memberValue As String, ByVal modelInstance As String, ByVal memberType As String)
            AreaName = area
            SectionName = section
            StationName = station
            Me.TaskName = taskname
            Me.TaskInstance = taskInstance
            Me.MemberName = memberName
            Me.MemberValue = memberValue
            Me.ModelInstance = modelInstance
            Me.MemberType = memberType
        End Sub
    End Class
End Namespace

