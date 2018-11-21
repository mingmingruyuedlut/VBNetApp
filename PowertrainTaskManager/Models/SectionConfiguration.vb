
Namespace Models
    Public Class SectionConfiguration
        Public Property Id As Integer
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property MemberName As String
        Public Property MemberValue As String

        Public Sub New()

        End Sub

        Public Sub New(areaName As String, sectionName As String, memberName As String, memberValue As String)
            Me.AreaName = areaName
            Me.SectionName = sectionName
            Me.MemberName = memberName
            Me.MemberValue = memberValue
        End Sub
    End Class
End Namespace
