
Namespace Models
    Public Class TaskConfiguration
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property StationName As String
        Public Property TaskName As String
        Public Property TaskInstance As Integer
        Public Property MemberName As String
        Public Property MemberValue As String
        Public Property MemberType As String
        Public Property BaseTag As String

        Public Sub New ()
        End Sub

        'Used for menu action on project tree of 'Acthitecture Manager' page
        Public Sub New(area As String, section As String, station As String, taskName As String)
            AreaName = area
            SectionName = section
            StationName = station
            Me.TaskName = taskName
        End Sub

        Public Sub New(ByVal area As String, ByVal section As String, ByVal station As String, ByVal taskName As String, ByVal taskInstance As Integer, ByVal memberName As String, ByVal memberValue As String, ByVal memberType As String, ByVal baseTag As String)
            AreaName = area
            SectionName = section
            StationName = station
            Me.TaskName = taskName
            Me.TaskInstance = taskInstance
            Me.MemberName = memberName
            Me.MemberValue = memberValue
            Me.MemberType = memberType
            Me.BaseTag = baseTag
        End Sub
    End Class
End Namespace

