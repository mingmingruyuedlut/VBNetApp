
Namespace Models
    Public Class PlcConfiguration
        Public Property AreaName As String
        Public Property SectionName As String
        Public Property StationName As String
        Public Property PlcName As String
        Public Property MemberName As String
        Public Property MemberValue As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal area As String, ByVal section As String, ByVal station As String, ByVal plc As String, ByVal memberName As String, ByVal memberValue As String)
            AreaName = area
            SectionName = section
            StationName = station
            PlcName = plc
            Me.MemberName = memberName
            Me.MemberValue = memberValue
        End Sub
    End Class

    Public Class PlcAddressConfiguration
        Public Property IpAddressFirstPart As String
        Public Property IpAddressSecondPart As String
        Public Property IpAddressThirdPart As String
        Public Property IpAddressFourthPart As String
        Public Property IpAddress As String
        Public Property Slot As Int32
        Public Property IpAddressAndSlot As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal ipFirstPart As String, ByVal ipSecondPart As String, ByVal ipThirdPart As String, ByVal ipFourthPart As String, ByVal slot As Int32)
            IpAddressFirstPart = ipFirstPart
            IpAddressSecondPart = ipSecondPart
            IpAddressThirdPart = ipThirdPart
            IpAddressFourthPart = ipFourthPart
            IpAddress = IpAddressFirstPart + "." + IpAddressSecondPart + "." + IpAddressThirdPart + "." + IpAddressFourthPart
            Slot = slot
            IpAddressAndSlot = IpAddress + "," + Slot.ToString()
        End Sub
    End Class
End Namespace

