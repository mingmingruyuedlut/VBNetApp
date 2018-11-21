
Namespace Constants
    Public Class ConfigurationConstant
        Public Const DotNumber As String = ".Number"
        Public Const DotTask_Number As String = ".Task_Number"
        Public Const DotTaskNumber As String = ".TaskNumber"

        Public Const StationTaskNumber As String = "Station_Task_Number.Station_Task_Number"
        Public Const StationTaskNumberNew As String = "StationConfig.Station_Task_Number"
    End Class

    Public Class DbTableNameConstant
        Public Const StationsConfiguration As String = "Stations_Configuration"
        Public Const TasksConfiguration As String = "Tasks_Configuration"
    End Class

    Public Class StationsConfigurationColumnConstant
        Public Const AreaName As String = "Area_Name"
        Public Const SectionName As String = "Section_Name"
        Public Const StationName As String = "Station_Name"
        Public Const MemberName As String = "Member_Name"
        Public Const MemberValue As String = "Member_Value"
        Public Const MemberType As String = "Member_type"
        Public Const BaseTag As String = "Base_Tag"
    End Class

    Public Class StationsColumnConstant
        Public Const Id As String = "ID"
        Public Const AreaName As String = "Area_Name"
        Public Const SectionName As String = "Section_Name"
        Public Const StationName As String = "Station_Name"
        Public Const StationType As String = "Station_Type"
        Public Const PlcType As String = "PLC_Type"
        Public Const MasterFileName As String = "MasterFile_Name"
        Public Const MasterFileRevision As String = "MasterFile_Revision"
        Public Const ModelAffiliation As String = "ModelAffiliation"
        Public Const Accept As String = "Accept"
    End Class

    Public Class TasksConfigurationColumnConstant
        Public Const AreaName As String = "Area_Name"
        Public Const SectionName As String = "Section_Name"
        Public Const StationName As String = "Station_Name"
        Public Const TaskName As String = "Task_Name"
        Public Const TaskInstance As String = "Task_Instance"
        Public Const MemberName As String = "Member_Name"
        Public Const MemberValue As String = "Member_Value"
        Public Const MemberType As String = "Member_type"
        Public Const BaseTag As String = "Base_Tag"
    End Class

    Public Class TasksColumnConstant
        Public Const Id As String = "ID"
        Public Const AreaName As String = "Area_Name"
        Public Const SectionName As String = "Section_Name"
        Public Const StationName As String = "Station_Name"
        Public Const TaskName As String = "Task_Name"
        Public Const MasterFileName As String = "MasterFile_Name"
        Public Const ModelAffiliation As String = "ModelAffiliation"
        Public Const TaskMemory As String = "Task_Memory"
        Public Const TaskMemoryPlus As String = "Task_MemoryPLUS"
        Public Const TaskNodes As String = "Task_Nodes"
        Public Const TaskConnection As String = "Task_Connection"
        Public Const MaxNoOfInstances As String = "MaxNoOfInstances"
    End Class

    Public Class MasterFileColumnConstant
        Public Const MasterFileName As String = "MasterFile_Name"
        Public Const MasterFileRevision As String = "MasterFile_Revision"
        Public Const MasterFileApplicationNotes As String = "MasterFile_Application_Notes"
        Public Const OverheadMemoryBase As String = "Overhead_Memory_Base"
        Public Const OverheadMemoryPlus As String = "Overhead_Memory_PLUS"
    End Class

    Public Class MasterFilesTaskColumnConstant
        Public Const MasterFileName As String = "MasterFile_Name"
        Public Const TaskName As String = "Task_Name"
        Public Const MemoryUsed As String = "Memory_Used"
        Public Const Version As String = "Version"
        Public Const MultiStation As String = "Multi-Station"
        Public Const MaxNoOfInstances As String = "MaxNoOfInstances"
        Public Const ModelAffiliation As String = "ModelAffiliation"
        Public Const L5XFileName As String = "L5XFileName"
    End Class

    Public Class ModelConfigurationColumnConstant
        Public Const AreaName As String = "Area_Name"
        Public Const SectionName As String = "Section_Name"
        Public Const StationName As String = "Station_Name"
        Public Const TaskName As String = "Task_Name"
        Public Const TaskInstance As String = "Task_Instance"
        Public Const MemberName As String = "Member_Name"
        Public Const MemberValue As String = "Member_Value"
        Public Const MemberType As String = "Member_type"
        Public Const ModelInstance As String = "Model_Instance"
    End Class

    Public Class PlcsConfigurationColumnConstant
        Public Const AreaName As String = "Area_Name"
        Public Const SectionName As String = "Section_Name"
        Public Const StationName As String = "Station_Name"
        Public Const PlcName As String = "PLC_Name"
        Public Const MemberName As String = "Member_Name"
        Public Const MemberValue As String = "Member_Value"

        Public Const Address1 As String = "PLC.IP Address[1]"
        Public Const Address2 As String = "PLC.IP Address[2]"
        Public Const Address3 As String = "PLC.IP Address[3]"
        Public Const Address4 As String = "PLC.IP Address[4]"
        Public Const Slot As String = "PLC.PLC Slot"
    End Class

    Public Class PlcInformationColumnConstant
        Public Const ProcessorType As String = "Processor_Type"
        Public Const ApplicationNotes As String = "Application_Notes"
        Public Const TotalBytesAvailable As String = "Total_Bytes_Available"
        Public Const MaxNodes As String = "Max_Nodes"
        Public Const MaxConnections As String = "Max_Connections"
    End Class

    Public Class AreasConfigurationColumnConstant
        Public Const AreaName As String = "Area_Name"
        Public Const ModelNumber As String = "Model_Number"
        Public Const ModelName As String = "Model_Name"
    End Class

    Public Class MemoryUsageColumnConstant
        Public Const AreaName As String = "Area_Name"
        Public Const SectionName As String = "Section_Name"
        Public Const StationName As String = "Station_Name"
        Public Const PlcType As String = "PLCType"
        Public Const TotalMem As String = "Total_Mem"
        Public Const TotalMemRsvd As String = "Total_Mem_Rsvd"
        Public Const MemAvailable As String = "Mem_Available"
        Public Const MemUsed As String = "Mem_Used"
        Public Const PercentAvailable As String = "Percent_Available"
        Public Const PercentUsed As String = "Percent_Used"
    End Class

    Public Class SectionConfigurationColumnConstant
        Public Const AreaName As String = "Area_Name"
        Public Const SectionName As String = "Section_Name"
        Public Const MemberName As String = "Member_Name"
        Public Const MemberValue As String = "Member_Value"
    End Class

    Public Class AreaStructureConstant
        Public Const ModelID As String = "ModelID"
    End Class

    Public Class OrderChangeConstant
        Public Const Show As String = "Save is OK"
        Public Const Station As String = "STATION"
        Public Const StationConfig As String = "StationConfig"
        Public Const Space As String = "-------"
    End Class

    Public Class StructureConstant
        Public Const Space As String = "SPACE"
        Public Const Parent As String = "Parent"
    End Class

    Public Class Form2Constant
        Public Const OrderChange As String = "Order Change"
        Public Const AcceptStation As String = "Accept Station"
        Public Const Config As String = "Config"
        Public Const Dot As String = "."
        Public Const LogixService As String = ".\Services\LogixService.py"
        Public Const True1 As String = "True"
        Public Const False1 As String = "False"
    End Class

    Public Class TreeNodeTagConstant
        Public Const PLANT As String = "PLANT"
        Public Const AREA As String = "AREA"
        Public Const SECTION As String = "SECTION"
        Public Const STATION As String = "STATION"
        Public Const TASK As String = "TASK"
        Public Const TASKInstancePrefix As String = "TASK|"
        Public Const MASTERFILE As String = "MASTERFILE"
        Public Const MASTERFILEATTRIBUTE As String = "MASTERFILEATTRIBUTE"
        Public Const PLC As String = "PLC"
        Public Const PLCAttribute As String = "PLCAttribute"
    End Class

    Public Class TreeNodeImageIndexConstant
        Public Const Plant As Integer = 0
        Public Const Area As Integer = 1
        Public Const Section As Integer = 2
        Public Const Station As Integer = 3
        Public Const Task As Integer = 5
        Public Const MasterFileSub As Integer = 10
        Public Const MasterFile As Integer = 12
        Public Const MasterFileAttribute As Integer = 16
        Public Const Plc As Integer = 15
        Public Const PlcAttribute As Integer = 16

        Public Const InvalidNode As Integer = 17
        Public Const ValidNode As Integer = 19
        Public Const NotFoundNode As Integer = 18
        Public Const NotFoundValidationFieldNode As Integer = 9
        Public Const ManualAcceptNode As Integer = 20
    End Class

    Public Class StationTypeConstant
        Public Const Auto As String = "Auto"
        Public Const Manual As String = "Manual"
    End Class

    Public Class PlcMemoryGridViewHeaderConstant
        Public Const No As String = "No."
        Public Const AreaName As String = "Area Name"
        Public Const SectionName As String = "Section Name"
        Public Const StationName As String = "Station Name"
        Public Const StationType As String = "Station Type"
        Public Const PlcType As String = "PLC Type"
        Public Const TotalPlcMem As String = "Total PLC Mem"
        Public Const TotalRsvdMem As String = "Total Rsvd Mem"
        Public Const MemAvailable As String = "Mem Available"
        Public Const MemUsed As String = "Mem Used"
    End Class

    Public Class PlcMemoryGdvColumnNameConstant
        Public Const No As String = "No"
        Public Const AreaName As String = "AreaName"
        Public Const SectionName As String = "SectionName"
        Public Const StationName As String = "StationName"
        Public Const StationType As String = "StationType"
        Public Const PlcType As String = "PLCType"
        Public Const TotalPlcMem As String = "TotalPLCMem"
        Public Const TotalRsvdMem As String = "TotalRsvdMem"
        Public Const MemAvailable As String = "MemAvailable"
        Public Const MemUsed As String = "MemUsed"
    End Class
End Namespace
