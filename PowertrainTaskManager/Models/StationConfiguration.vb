
Namespace Models
    Public Class StationConfiguration
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property StationName As String
        Public Property MemberName As String
        Public Property MemberValue As String
        Public Property MemberType As String
        Public Property BaseTag As String

        Public Sub New ()
        End Sub

        Public Sub New(ByVal area As String, ByVal section As String, ByVal station As String, ByVal memberName As String, ByVal memberValue As String, ByVal memberType As String, baseTag As String)
            AreaName = area
            SectionName = section
            StationName = station
            Me.MemberName = memberName
            Me.MemberValue = memberValue
            Me.MemberType = memberType
            Me.BaseTag = baseTag
        End Sub
    End Class
End Namespace
