
Imports Powertrain_Task_Manager.Common
Imports Powertrain_Task_Manager.Enums
Imports Powertrain_Task_Manager.Models
Imports Powertrain_Task_Manager.Repositories

Namespace Controllers
    Public Class StationController
        Public Function VerifyStationManualFlag(ByVal stationName As String) As Integer
            'to-do, check the station manual validation flag
            Return StationConfigValidateResult.Valid
        End Function

        Property StationsControllerRep As StationRepository

        Public Sub New()

        End Sub

        Public Sub New(ByVal stationsRep As StationRepository)
            StationsControllerRep = stationsRep
        End Sub

        Public Sub New(ByVal dbSqlHelper As SqlHelper)
            StationsControllerRep = New StationRepository(dbSqlHelper)
        End Sub

        Public Sub New(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            StationsControllerRep = New StationRepository(dbSqlHelper)
        End Sub

        Public Sub Init()
            Dim dbConnStr As String = My.Settings.DBConnectionString & PowertrainProjectFile & ";"
            Init(dbConnStr)
        End Sub

        Public Sub Init(ByVal dbConnStr As String)
            Dim dbSqlHelper = New SqlHelper(dbConnStr)
            StationsControllerRep = New StationRepository(dbSqlHelper)
        End Sub

        Public Function GetListStations(ByVal stationName As String, ByVal sectionName As String, ByVal areaName As String) As List(Of StationModel)
            Dim stationsList
            stationsList = StationsControllerRep.GetListStations(StationsControllerRep.GetStationsByMemberName(stationName, sectionName, areaName))
            Return stationsList
        End Function

        Public Function GetStationModel(stationTreeNodeHier As TreeNodeHierarchy) As StationModel
            Dim station = GetListStations(stationTreeNodeHier.StationName, stationTreeNodeHier.SectionName, stationTreeNodeHier.AreaName).FirstOrDefault()
            Return station
        End Function

        Public Function CalculateStationNameForCopy(stationTreeNodeHier As TreeNodeHierarchy) As String
            Dim stationNameLike = stationTreeNodeHier.StationName & "_"
            Dim stationList = StationsControllerRep.GetStationsLikeNameForCopy(stationTreeNodeHier.AreaName, stationTreeNodeHier.SectionName, stationNameLike)
            Dim stationIndexList = stationList.
                    Where(Function(x) StationNameSuffixIndexIsInteger(x.StationName, stationNameLike)).
                    Select(Function(x) Convert.ToInt32(x.StationName.Substring(stationNameLike.Length))).ToList()
            stationIndexList.Sort()
            Dim finalIndex = 1
            For Each index In stationIndexList
                If index = finalIndex Then
                    finalIndex += 1
                Else
                    Exit For
                End If
            Next

            Return stationNameLike & finalIndex
        End Function

        Private Function StationNameSuffixIndexIsInteger(stationName As String, prefixLike As String) As Boolean
            Dim suffixIndex = stationName.Substring(prefixLike.Length)
            Return IsNumeric(suffixIndex)
        End Function

        Public Function GetListStations() As List(Of StationModel)
            Dim stationsList
            stationsList = StationsControllerRep.GetListStations(StationsControllerRep.GetStations())
            Return stationsList
        End Function

        Public Sub InsertStation(stationList As List(Of StationModel))
            For Each station In stationList
                InsertStation(station)
            Next
        End Sub

        Public Sub InsertStation(station As StationModel)
            StationsControllerRep.InsertStation(station)
        End Sub

        Public Sub DeleteAllStations()
            StationsControllerRep.DeleteAllStations()
        End Sub

        Public Sub SetStationAccept(ByVal stationName As String, ByVal sectionName As String, ByVal areaName As String)
            StationsControllerRep.SetStationAccept(stationName, sectionName, areaName)
        End Sub

        Public Function GetAccept(stationName As String, sectionName As String, areaName As String) As Integer
            Dim stationList = GetListStations(stationName, sectionName, areaName)
            Return stationList(0).Accept
        End Function

        Public Function GetAccept(ByVal listStations As List(Of StationModel)) As Integer
            Return listStations(0).Accept
        End Function
    End Class
End Namespace
