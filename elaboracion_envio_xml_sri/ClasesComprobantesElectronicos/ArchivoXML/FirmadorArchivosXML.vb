Imports System.IO

Public Class FirmadorArchivosXML
    'Public Shared Sub firmar(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, archivoXML As String)
    '    Dim cadenaSalida As String = ""
    '    Dim processinfo As New ProcessStartInfo()
    '    processinfo.WorkingDirectory = "C:\"
    '    processinfo.FileName = "java.exe"
    '    processinfo.Arguments = "-jar " & configuracion.ubicacionFirmadorJava & " " & configuracion.ubicacionArchivosSinFirma & "\\" & archivoXML & " " & configuracion.ubicacionFirmaElectronica & " " & configuracion.contrasenaFirmaElectronica & " " & configuracion.ubicacionTemporalXMLFirmados & " " & archivoXML
    '    Dim myProcess As New Process()
    '    processinfo.UseShellExecute = False
    '    processinfo.RedirectStandardOutput = True
    '    myProcess.StartInfo = processinfo
    '    myProcess.Start()
    '    Dim myStreamReader As StreamReader = myProcess.StandardOutput
    '    ' Read the standard output of the spawned process. 
    '    Do
    '        cadenaSalida = cadenaSalida & myStreamReader.ReadLine()
    '    Loop Until (myStreamReader.EndOfStream)
    '    myProcess.WaitForExit()
    '    myProcess.Close()
    'End Sub
End Class
