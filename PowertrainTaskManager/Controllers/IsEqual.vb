
Imports Powertrain_Task_Manager.Models

Namespace Controllers
    Public Class IsEqual

        Public Sub New()

        End Sub

        Public Function IsEqual(ByVal list1 As List(Of AreaStructure), ByVal list2 As List(Of AreaStructure)) As Boolean
            If list1.Count <> list2.Count Then
                Return False
            Else
                For i = 0 To list1.Count - 1
                    If list1(i).Base = list2(i).Base And list1(i).ExclusionString = list2(i).ExclusionString And list1(i).Global1 = list2(i).Global1 And list1(i).MemberDescription3 = list2(i).MemberDescription3 And list1(i).MemberDescription1 = list2(i).MemberDescription1 And list1(i).MemberDescription2 = list2(i).MemberDescription2 And list1(i).MaxLength = list2(i).MaxLength And list1(i).MaxValue = list2(i).MaxValue And list1(i).TaskXrefName = list2(i).TaskXrefName And list1(i).MemberName = list2(i).MemberName And list1(i).MemberOrder = list2(i).MemberOrder And list1(i).MemberType = list2(i).MemberType And list1(i).MemberValues = list2(i).MemberValues And list1(i).MinValue = list2(i).MinValue And list1(i).Parent = list2(i).Parent And list1(i).TaskXrefName = list2(i).TaskXrefName And list1(i).Version = list2(i).Version And list1(i).Visible = list2(i).Visible Then
                    Else
                        Return False
                    End If
                Next
                Return True
            End If
        End Function
        Public Function IsEqual(ByVal list1 As List(Of StationStructure), ByVal list2 As List(Of StationStructure)) As Boolean
            If list1.Count <> list2.Count Then
                Return False
            Else
                For i = 0 To list1.Count - 1
                    If list1(i).Base = list2(i).Base And list1(i).ExclusionString = list2(i).ExclusionString And list1(i).Global1 = list2(i).Global1 And list1(i).MemberDescription3 = list2(i).MemberDescription3 And list1(i).MemberDescription1 = list2(i).MemberDescription1 And list1(i).MemberDescription2 = list2(i).MemberDescription2 And list1(i).MaxLength = list2(i).MaxLength And list1(i).MaxValue = list2(i).MaxValue And list1(i).MemberGroup = list2(i).MemberGroup And list1(i).MemberName = list2(i).MemberName And list1(i).MemberOrder = list2(i).MemberOrder And list1(i).MemberType = list2(i).MemberType And list1(i).MemberValues = list2(i).MemberValues And list1(i).MinValue = list2(i).MinValue And list1(i).Parent = list2(i).Parent And list1(i).TaskXrefName = list2(i).TaskXrefName And list1(i).Version = list2(i).Version And list1(i).Visible = list2(i).Visible Then
                    Else
                        Return False
                    End If
                Next
                Return True
            End If
        End Function
        Public Function IsEqual(ByVal list1 As List(Of TaskStructure), ByVal list2 As List(Of TaskStructure)) As Boolean
            If list1.Count <> list2.Count Then
                Return False
            Else
                For i = 0 To list1.Count - 1
                    If list1(i).Base = list2(i).Base And list1(i).ExclusionString = list2(i).ExclusionString And list1(i).Global1 = list2(i).Global1 And list1(i).MemberDescription3 = list2(i).MemberDescription3 And list1(i).MemberDescription1 = list2(i).MemberDescription1 And list1(i).MemberDescription2 = list2(i).MemberDescription2 And list1(i).MaxLength = list2(i).MaxLength And list1(i).MaxValue = list2(i).MaxValue And list1(i).TaskXrefName = list2(i).TaskXrefName And list1(i).MemberName = list2(i).MemberName And list1(i).MemberOrder = list2(i).MemberOrder And list1(i).MemberType = list2(i).MemberType And list1(i).MemberValues = list2(i).MemberValues And list1(i).MinValue = list2(i).MinValue And list1(i).TaskXrefName = list2(i).TaskXrefName And list1(i).Version = list2(i).Version And list1(i).Visible = list2(i).Visible Then
                    Else
                        Return False
                    End If
                Next
                Return True
            End If
        End Function
    End Class
End Namespace

