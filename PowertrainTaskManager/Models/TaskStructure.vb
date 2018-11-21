
Namespace Models
    Public Class TaskStructure
        Public Property Id As Integer
        Public Property TaskName As String
        Public Property MemberName As String
        Public Property MemberType As String
        Public Property MemberOrder As Integer
        Public Property MemberDescription1 As String
        Public Property MemberDescription2 As String
        Public Property MemberDescription3 As String
        Public Property MemberValues As String
        Public Property TaskXrefName As String
        Public Property Visible As Integer
        Public Property Global1 As Integer
        Public Property Base As Integer
        Public Property MaxLength As Integer
        Public Property MinValue As Integer
        Public Property MaxValue As Integer
        Public Property ExclusionString As String
        Public Property Version As String
        Public Sub New(ByVal id As Integer, ByVal taskName As String, ByVal memberName As String, ByVal memberType As String, ByVal memberOrder As String, ByVal memberDescription1 As String, ByVal memberDescription2 As String, ByVal memberDescription3 As String, ByVal memberValues As String, ByVal taskXrefName As String, ByVal visible As Integer, ByVal global1 As Integer, ByVal base As Integer, ByVal maxLength As Integer, ByVal minValue As Integer, ByVal maxValue As Integer, ByVal exclusionString As String, ByVal version As String)
            Me.Id = id
            Me.TaskName = taskName
            Me.MemberName = memberName
            Me.MemberType = memberType
            Me.MemberOrder = memberOrder
            Me.MemberDescription1 = memberDescription1
            Me.MemberDescription2 = memberDescription2
            Me.MemberDescription3 = memberDescription3
            Me.MemberValues = memberValues
            Me.TaskXrefName = taskXrefName
            Me.Visible = visible
            Me.Global1 = global1
            Me.Base = base
            Me.MaxLength = maxLength
            Me.MinValue = minValue
            Me.MaxValue = maxValue
            Me.ExclusionString = exclusionString
            Me.Version = version
        End Sub
        Public Sub New()

        End Sub
    End Class
End Namespace
