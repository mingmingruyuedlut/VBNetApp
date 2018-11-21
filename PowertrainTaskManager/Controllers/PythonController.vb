
Imports IronPython.Hosting
Imports Microsoft.Scripting.Hosting
Imports Powertrain_Task_Manager.Constants
Imports Powertrain_Task_Manager.Models

Namespace Controllers
    Public Class PythonController
        ''' <summary>
        ''' Get python file object without connection
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetPythonFileObj() As Object
            Dim fath = Form2Constant.LogixService
            Dim pyruntime As ScriptRuntime = Python.CreateRuntime()
            Dim fileObj As Object = pyruntime.UseFile(fath)
            Return fileObj
        End Function

        ''' <summary>
        ''' Get python file object with connection
        ''' </summary>
        ''' <param name="plcAddressConfig"></param>
        ''' <returns></returns>
        Public Shared Function GetPythonFileObj(ByVal plcAddressConfig As PlcAddressConfiguration) As Object
            Dim fileObj = GetPythonFileObj()
            GenerateConnectionInPython(fileObj, plcAddressConfig)
            Return fileObj
        End Function

        ''' <summary>
        ''' Generate the connection in python through plc address and slot
        ''' </summary>
        ''' <param name="fileObj"></param>
        ''' <param name="plcAddressConfig"></param>
        Public Shared Sub GenerateConnectionInPython(ByRef fileObj As Object, ByVal plcAddressConfig As PlcAddressConfiguration)
            Dim strIpAddress = plcAddressConfig.IpAddress
            Dim strPlcSlot = plcAddressConfig.Slot.ToString()
            fileObj.set_comn_tag(strIpAddress, strPlcSlot)
        End Sub

        Public Shared Sub DownloadTag(ByRef fileObj As Object, ByVal tagName As String, ByVal tagValue As String)
            fileObj.write_tag(tagName, tagValue)
        End Sub

        Public Shared Function UploadTag(ByRef fileObj As Object, ByVal tagName As String) As String
            Return fileObj.ex_read(tagName)
        End Function
    End Class
End Namespace

