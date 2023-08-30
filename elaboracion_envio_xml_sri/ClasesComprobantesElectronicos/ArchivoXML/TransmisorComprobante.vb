Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Reflection

Public Class TransmisorComprobante
    Public Class RespuestaSRI
        Public numeroAutorizacion As String = ""
        Public autorizado As Boolean = False
        Public huboError As Boolean = False
        Public mensajeError As String = ""
        Public errorServicioWeb As Boolean
    End Class

    Public Shared Function transmitir(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, archivoXML As String) As RespuestaSRI
        'Obtener la clave de acceso del archivo
        Dim enClave As Boolean = False
        Dim servicioRecepcionPruebasSRI As New ec.gob.sri.celcer.recepcionPruebas.RecepcionComprobantesOfflineService
        Dim servicioRecepcionProduccionSRI As New ec.gob.sri.cel.recepcionProduccion.RecepcionComprobantesOfflineService
        Dim respuestaRecepcion As Object()
        Dim estado As String

        Try
            Dim data() As Byte = File.ReadAllBytes(configuracion.ubicacionTemporalXMLFirmados.Replace("\\", "\") & "\" & archivoXML)
            If configuracion.tipoAmbienteActual = TIPO_DE_AMBIENTE_PRODUCCION Then
                respuestaRecepcion = servicioRecepcionProduccionSRI.validarComprobante(data)
            Else
                respuestaRecepcion = servicioRecepcionPruebasSRI.validarComprobante(data)
            End If

            estado = obtenerEstado(respuestaRecepcion(0))
            If estado = "DEVUELTA" Then
                Dim respuesta As New RespuestaSRI
                respuesta.autorizado = False
                respuesta.huboError = True
                respuesta.numeroAutorizacion = ""
                respuesta.mensajeError = obtenerMensajeError(respuestaRecepcion(0))
                respuesta.errorServicioWeb = False
                Return respuesta
            ElseIf estado = "RECIBIDA" Then
                Dim respuesta As New RespuestaSRI
                respuesta.autorizado = True
                respuesta.huboError = False
                respuesta.numeroAutorizacion = ""
                respuesta.mensajeError = ""
                respuesta.errorServicioWeb = False
                Return respuesta
            Else
                Dim respuesta As New RespuestaSRI
                respuesta.autorizado = False
                respuesta.huboError = True
                respuesta.numeroAutorizacion = ""
                respuesta.mensajeError = "Respuesta desconocida SRI"
                respuesta.errorServicioWeb = False
                Return respuesta
            End If
            Return New RespuestaSRI
        Catch ex As Exception
            Dim respuesta As New RespuestaSRI
            respuesta.autorizado = False
            respuesta.huboError = True
            respuesta.numeroAutorizacion = ""
            respuesta.mensajeError = "Error al validar XML SRI: " & ex.Message
            respuesta.errorServicioWeb = True
            Return respuesta
        End Try
    End Function

    Public Shared Function obtenerNumeroAutorizacion(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, numeroComprobante As String, claveAccesoComprobante As String) As RespuestaSRI
        Dim servicioAutorizacionPruebasSRI As New ec.gob.sri.celcer.autorizacionPruebas.AutorizacionComprobantesOfflineService
        Dim servicioAutorizacionProduccionSRI As New ec.gob.sri.cel.autorizacionProduccion.AutorizacionComprobantesOfflineService
        Dim respuestaAutorizacion As Object()
        Dim respuesta As New RespuestaSRI
        Dim textoAutorizacion As String

        Try
            If configuracion.tipoAmbienteActual = TIPO_DE_AMBIENTE_PRODUCCION Then
                respuestaAutorizacion = servicioAutorizacionProduccionSRI.autorizacionComprobante(claveAccesoComprobante)
            Else
                respuestaAutorizacion = servicioAutorizacionPruebasSRI.autorizacionComprobante(claveAccesoComprobante)
            End If

            textoAutorizacion = obtenerTextoAutorizacion(respuestaAutorizacion, numeroComprobante, configuracion)
            If textoAutorizacion = "" Then
                respuesta.autorizado = True
                respuesta.huboError = False
                respuesta.numeroAutorizacion = ""
                respuesta.mensajeError = "Autorizado anteriormente"
                respuesta.errorServicioWeb = False
            ElseIf textoAutorizacion.Substring(0, "AUTORIZADO".Length) = "AUTORIZADO" Then
                respuesta.autorizado = True
                respuesta.huboError = False
                respuesta.numeroAutorizacion = textoAutorizacion.Substring(10, 49)
                respuesta.mensajeError = ""
                respuesta.errorServicioWeb = False
            ElseIf textoAutorizacion.Substring(0, "NO AUTORIZADO".Length) = "NO AUTORIZADO" Then
                respuesta.autorizado = False
                respuesta.huboError = True
                respuesta.numeroAutorizacion = ""
                respuesta.mensajeError = obtenerMensajeError(textoAutorizacion, claveAccesoComprobante.Substring(8, 2))
                respuesta.errorServicioWeb = False
            End If
            Return respuesta
        Catch ex As Exception
            respuesta.autorizado = False
            respuesta.huboError = True
            respuesta.numeroAutorizacion = ""
            respuesta.mensajeError = "Error al obtener número de autorización SRI: " & ex.Message
            respuesta.errorServicioWeb = True
            Return respuesta
        End Try
    End Function

    Private Shared Function obtenerEstado(respuesta As Object) As String
        Dim objetoEstado As Object
        Try
            Dim objetoInterno As Object = respuesta(0)
            objetoEstado = objetoInterno(0)
            Return objetoEstado.InnerText
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Shared Function obtenerMensajeError(respuesta As Object) As String
        Dim objetoEstado As Object
        Try
            Dim objetoInterno As Object = respuesta(0)
            objetoEstado = objetoInterno(1)
            Try
                Return objetoEstado.InnerText.ToString.Substring(49, objetoEstado.InnerText.ToString.Length - 49)
            Catch ex As Exception
                Try
                    Return objetoEstado.InnerText
                Catch ex2 As Exception
                    Return "Devuelto por SRI"
                End Try
            End Try
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Shared Function obtenerTextoAutorizacion(respuesta As Object, numeroComprobante As String, configuracionActual As ClasesComprobantesElectronicos.ConfiguracionActual) As String
        Dim objetoAutorizaciones As Object
        Dim objetoAutorizacion As Object
        Dim ret As String = ""
        Dim numeroAutorizacion As String
        Dim estado As String
        Dim configuracion As ClasesComprobantesElectronicos.ConfiguracionActual

        configuracion = configuracionActual

        Try
            Dim objetoInterno As Object = respuesta(0)
            objetoInterno = objetoInterno(0)
            objetoAutorizaciones = objetoInterno(2)
            For i = 0 To objetoAutorizaciones.ChildNodes.Count - 1
                objetoAutorizacion = objetoAutorizaciones.ChildNodes.ItemOf(i)

                Dim doc As New XmlDocument()
                doc.LoadXml(objetoAutorizacion.OuterXML)

                estado = doc.SelectSingleNode("/autorizacion/estado").InnerText

                If estado = "AUTORIZADO" Then
                    numeroAutorizacion = doc.SelectSingleNode("/autorizacion/numeroAutorizacion").InnerText
                    doc.Save(configuracion.ubicacionTemporalXMLFirmados.Replace("\\", "\") & "\" & numeroComprobante & ".xml")
                    Return estado & numeroAutorizacion
                End If
            Next
            Return objetoAutorizaciones.InnerText
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Shared Function obtenerTextoAutorizacionAnterior(respuesta As Object) As String
        Dim objetoAutorizaciones As Object
        Dim objetoAutorizacion As Object
        Dim ret As String = ""
        Try
            Dim objetoInterno As Object = respuesta(0)
            objetoInterno = objetoInterno(0)
            objetoAutorizaciones = objetoInterno(2)
            For i = 0 To objetoAutorizaciones.ChildNodes.Count - 1
                objetoAutorizacion = objetoAutorizaciones.ChildNodes.ItemOf(i)
                ret = objetoAutorizacion.InnerText
                If ret.Length >= 49 + "AUTORIZADO".Length + 37 Then
                    If ret.Substring(49, "AUTORIZADO".Length) = "AUTORIZADO" Then
                        Return ret.Substring(49)
                    End If
                End If
            Next
            Return objetoAutorizaciones.InnerText
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Shared Function obtenerMensajeError(texto As String, tipoComprobante As String) As String
        Dim finComprobante As String

        Select Case tipoComprobante
            Case TIPO_DE_COMPROBANTE_FACTURA
                finComprobante = "factura"
            Case TIPO_DE_COMPROBANTE_GUIA_DE_REMISION
                finComprobante = "guiaRemision"
            Case TIPO_DE_COMPROBANTE_NOTA_DE_CREDITO
                finComprobante = "notaCredito"
			Case TIPO_DE_COMPROBANTE_COMPROBANTE_DE_RETENCION
				finComprobante = "comprobanteRetencion"
			Case TIPO_DE_COMPROBANTE_LIQUIDACION_COMPRA
				finComprobante = "liquidacionCompra"
			Case Else
                finComprobante = "ñdsklfhñasdklfh"
        End Select
        finComprobante = "</" & finComprobante & ">"
        If InStr(texto, finComprobante) > 0 Then
            Return texto.Substring(InStr(texto, finComprobante) + finComprobante.Length - 1, texto.Length - InStr(texto, finComprobante) - finComprobante.Length + 1)
        Else
            Return "No se detectó el mensaje de error"
        End If
    End Function
End Class