Public Class RegistroComprobanteElectronico
    Public numeroComprobante As String = ""
    Public tipoComprobante As String = ""
    Public claveAcceso As String = ""
    Public ruc As String = ""
    Public fechaInicioProceso As Date = CDate("01/01/1900")
    Public fechaGeneracionXML As Date = CDate("01/01/1900")
    Public fechaFirmaXML As Date = CDate("01/01/1900")
    Public fechaContingencia As Date = CDate("01/01/1900")
    Public numeroAutorizacionContingencia As String = ""
    Public fechaAutorizacion As Date = CDate("01/01/1900")
    Public numeroAutorizacionSRI As String = ""
    Public numeroIntentosTransicionAutorizacion As Integer = 0
    Public transicionAutorizacion As Integer = 0
    Public mensajeErrorAutorizacion As String = ""
    Public fechaObtencionEmail As Date = CDate("01/01/1900")
    Public fechaNotificacionCliente As Date = CDate("01/01/1900")
    Public numeroIntentosTransicionNotificacion As Integer = 0
    Public transicionNotificacion As Integer = 0
    Public mensajeErrorNotificacion As String = ""
    Public fechaPrimeraDescargaXML As Date = CDate("01/01/1900")
    Public fechaPrimeraDescargaRIDE As Date = CDate("01/01/1900")
    Public fechaUltimaDescargaXML As Date = CDate("01/01/1900")
    Public fechaUltimaDescargaRIDE As Date = CDate("01/01/1900")
    Public tipoEmision As Integer = TIPO_DE_EMISION_NORMAL
End Class
