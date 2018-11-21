
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories
Namespace Controllers
    Public Class StationStructureController
        Property StationStructureControllerRep As StationStructureRepository

        Public Sub New(ByVal stationStuctureRep As StationStructureRepository)
            StationStructureControllerRep = stationStuctureRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            StationStructureControllerRep = New StationStructureRepository(dbSqlHelper)
        End Sub

        Public Function GetListStationStructure() As List(Of StationStructure)
            Dim stationListStructure
            stationListStructure = StationStructureControllerRep.GetListStationStructure(StationStructureControllerRep.GetStationStructureDatable())
            Return stationListStructure
        End Function

        Public Function GetListStationStructureByMemberGroup(ByVal strMemberGroup) As List(Of StationStructure)
            Dim stationListStructure
            stationListStructure = StationStructureControllerRep.GetListStationStructure(StationStructureControllerRep.GetStationStructureDatableByMemberGroup(strMemberGroup))
            Return stationListStructure
        End Function

        Public Function GetListString(ByVal listStationStructure As List(Of StationStructure)) As List(Of String)
            Dim listString As List(Of String) = New List(Of String)
            For i = 0 To listStationStructure.Count - 1
                Dim description As String
                If listStationStructure(i).MemberDescription1 = "" Then
                    description = listStationStructure(i).MemberName
                Else
                    description = listStationStructure(i).MemberDescription1
                End If
                If listStationStructure(i).MemberName = StructureConstant.Space Then
                    listString.Add(OrderChangeConstant.Space)
                Else
                    listString.Add(description)
                End If
            Next
            Return listString
        End Function

        Public Sub DeleteStationStructureByBase()
            StationStructureControllerRep.DeleteStationStructureByBase()
        End Sub

        Public Sub InsertStationStructure(ByVal parent As String, ByVal order As Integer)
            StationStructureControllerRep.InsertStationStructure(parent, order)
        End Sub

        Public Sub UpdateStationStructureMemberOrderbyMemberName(ByVal memberName As String, ByVal order As Integer)
            StationStructureControllerRep.UpdateStationStructureMemberOrderbyMemberName(memberName, order)
        End Sub

        Public Function GetParentByMemberName(ByVal memberName As String) As String
            Dim parent
            parent = StationStructureControllerRep.GetParentByMemberName(memberName)
            Return parent
        End Function
    End Class
End Namespace
