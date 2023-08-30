Public Class ProcesadorComprobantes

    Public Const MAXIMO_NUMERO_INTENTOS_TRANSICION_AUTORIZACION = 2

    Public Class EstadoProceso
        Public ok As Boolean = True
        Public mensaje As String = ""
        Public numeroEtapa As Integer = 0
        Public errorServicioWeb As Boolean = False

        Public ReadOnly Property etapa As String
            Get
                Select Case numeroEtapa
                    Case TRANSICION_PROCESO_INICIADO
                        Return "Proceso iniciado"
                    Case TRANSICION_XML_GENERADO
                        Return "XML generado"
                    Case TRANSICION_XML_ACEPTADO
                        Return "XML aceptado"
                    Case TRANSICION_NUMERO_AUTORIZACION
                        Return "Número autorización obtenido"
                    Case TRANSICION_RIDE_AUTORIZADO
                        Return "RIDE autorizado"
                    Case Else
                        Return "Otro"
                End Select
            End Get
        End Property
    End Class

    Private configuracion As ClasesComprobantesElectronicos.ConfiguracionActual
    Private listaDestinatariosProblemasSistemas As New List(Of String)
    Private listaDestinatariosProblemasLogistica As New List(Of String)

    Public Sub New(ByVal configuracionActual As ClasesComprobantesElectronicos.ConfiguracionActual)
        configuracion = configuracionActual
        sqlServer = New ConectorSQLServer(configuracion)
        sqlServer.conectar()
        listaDestinatariosProblemasSistemas.Add(configuracion.destinatarioProblemasSistemas)
        listaDestinatariosProblemasLogistica.Add(configuracion.destinatarioProblemasLogistica)
    End Sub
    Public Sub procesarComprobantes()
        Dim consulta As String
        Dim aux As DataTable
        Dim registro As DataRow
        Dim numeroComprobante As String

        sqlServer.conectar()

        consulta = "SELECT numero_comprobante FROM COMPROBANTE_ELECTRONICO WHERE anulado=0 and transicion_autorizacion<" & TRANSICION_RIDE_AUTORIZADO
        aux = sqlServer.consultar(consulta)
        For Each registro In aux.Rows
            numeroComprobante = registro("numero_comprobante")
            procesarComprobante(numeroComprobante)
        Next
    End Sub

    Private Sub procesarComprobante(ByVal numeroComprobante As String)
        Dim comprobante As New ControladorComprobanteElectronico
        Dim retAutorizacion As New EstadoProceso
        Dim retNofificacion As New EstadoProceso

        comprobante.obtener(numeroComprobante)
        If comprobante.existe Then
            With comprobante.datos
                'Solo procesar si no se ha superado el número de intentos de transicionAutorizacion
                If .numeroIntentosTransicionAutorizacion < MAXIMO_NUMERO_INTENTOS_TRANSICION_AUTORIZACION Then
                    'Procesar el comprobante
                    If .transicionAutorizacion = TRANSICION_PROCESO_INICIADO Then
                        retAutorizacion = generarXMLNormal(numeroComprobante)
                    ElseIf .transicionAutorizacion = TRANSICION_XML_GENERADO Then
                        If .numeroIntentosTransicionAutorizacion < configuracion.maximoIntentosEntreTransiciones Or True Then 'No pasar nunca a contingencia
                            retAutorizacion = transmitirXML(numeroComprobante)
                        Else
                            retAutorizacion = transmitirXML(numeroComprobante)
                        End If
                    ElseIf .transicionAutorizacion = TRANSICION_XML_ACEPTADO Then
                        retAutorizacion = obtenerNumeroAutorizacion(numeroComprobante)
                    End If
                Else
                    'No seguir intentando
                    retAutorizacion.mensaje = .mensajeErrorAutorizacion
                    retAutorizacion.ok = False
                    .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 1
                    comprobante.actualizar()
                End If
            End With
        Else

        End If

    End Sub

    Public Function generarXMLNormal(ByVal numeroComprobante) As EstadoProceso
        Dim comprobante As New ControladorComprobanteElectronico
        Dim ret As New EstadoProceso
        Dim claveAcceso As String = ""

        comprobante.obtener(numeroComprobante)

        Try
            Dim archivo As New ArchivoXMLFactura(configuracion, TIPO_DE_EMISION_NORMAL, configuracion.tipoAmbienteActual, numeroComprobante)
            archivo.generar()
            claveAcceso = archivo.claveAccesoGenerada
        Catch ex As Exception
            ret.ok = False
            ret.mensaje = ex.Message
        End Try


        If ret.ok Then
            Try
                'Transición
                With comprobante.datos
                    .claveAcceso = claveAcceso
                    .fechaGeneracionXML = Now
                    .fechaFirmaXML = Now
                    .numeroIntentosTransicionAutorizacion = 0
                    .mensajeErrorAutorizacion = ""
                    .transicionAutorizacion = TRANSICION_XML_GENERADO_POR_FIRMAR
                End With
                comprobante.actualizar()
            Catch ex As Exception
                ret.ok = False
                ret.mensaje = ex.Message

                Try
                    With comprobante.datos
                        .claveAcceso = ""
                        .fechaGeneracionXML = CDate("01/01/1900")
                        .fechaFirmaXML = CDate("01/01/1900")
                        .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 1
                        .mensajeErrorAutorizacion = ret.mensaje
                    End With
                    comprobante.actualizar()
                Catch ex2 As Exception

                End Try
            End Try
        Else
            Try
                With comprobante.datos
                    .claveAcceso = ""
                    .fechaGeneracionXML = CDate("01/01/1900")
                    .fechaFirmaXML = CDate("01/01/1900")
                    .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 1
                    .mensajeErrorAutorizacion = ret.mensaje
                End With
                comprobante.actualizar()
            Catch ex As Exception

            End Try
        End If
        Return ret
    End Function

    Private Function generarRIDENormal(numeroComprobante) As EstadoProceso
        Dim comprobante As New ControladorComprobanteElectronico
        Dim ret As New EstadoProceso
        Dim claveAcceso As String = ""

        comprobante.obtener(numeroComprobante)
        Select Case comprobante.datos.tipoComprobante
            Case TIPO_DE_COMPROBANTE_FACTURA
                Try
                    Dim archivo As New RIDEFactura(configuracion, numeroComprobante, TIPO_DE_EMISION_NORMAL, configuracion.tipoAmbienteActual)
                Catch ex As Exception
                    ret.ok = False
                    ret.mensaje = ex.Message
                End Try
            Case TIPO_DE_COMPROBANTE_GUIA_DE_REMISION
                Try
                    Dim archivo As New RideGuiaRemision(configuracion, numeroComprobante, TIPO_DE_EMISION_NORMAL, configuracion.tipoAmbienteActual)
                Catch ex As Exception
                    ret.ok = False
                    ret.mensaje = ex.Message
                End Try
            Case TIPO_DE_COMPROBANTE_NOTA_DE_CREDITO
                Try
                    Dim archivo As New RideNotaCredito(configuracion, numeroComprobante, TIPO_DE_EMISION_NORMAL, configuracion.tipoAmbienteActual)
                Catch ex As Exception
                    ret.ok = False
                    ret.mensaje = ex.Message
                End Try
			Case TIPO_DE_COMPROBANTE_COMPROBANTE_DE_RETENCION
				Try
					Dim archivo As New RideRetencion(configuracion, numeroComprobante, TIPO_DE_EMISION_NORMAL, configuracion.tipoAmbienteActual)
				Catch ex As Exception
					ret.ok = False
					ret.mensaje = ex.Message
				End Try
			Case TIPO_DE_COMPROBANTE_LIQUIDACION_COMPRA
				Try
					Dim archivo As New RideLiquidacionCompra(configuracion, numeroComprobante, TIPO_DE_EMISION_NORMAL, configuracion.tipoAmbienteActual)
				Catch ex As Exception
					ret.ok = False
					ret.mensaje = ex.Message
				End Try
		End Select

        If ret.ok Then
            Try
                'Transición
                With comprobante.datos
                    .numeroIntentosTransicionAutorizacion = 0
                    .mensajeErrorAutorizacion = ""
                    .transicionAutorizacion = TRANSICION_RIDE_AUTORIZADO
                End With
                comprobante.actualizar()
            Catch ex As Exception
                ret.ok = False
                ret.mensaje = ex.Message

                Try
                    With comprobante.datos
                        '.claveAcceso = "" 'Ya no debe cambiar la clave de acceso
                        .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 1
                        .mensajeErrorAutorizacion = ret.mensaje
                    End With
                    comprobante.actualizar()
                Catch ex2 As Exception

                End Try
            End Try
        Else
            Try
                With comprobante.datos
                    .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 1
                    .mensajeErrorAutorizacion = ret.mensaje
                End With
                comprobante.actualizar()
            Catch ex As Exception

            End Try
        End If
        Return ret
    End Function

    Private Function transmitirXML(numeroComprobante) As EstadoProceso
        Dim comprobante As New ControladorComprobanteElectronico
        Dim ret As New EstadoProceso
        Dim claveAcceso As String = ""
        Dim respuestaSRI As New TransmisorComprobante.RespuestaSRI

        comprobante.obtener(numeroComprobante)
        Try
            respuestaSRI = TransmisorComprobante.transmitir(configuracion, numeroComprobante & ".xml")
            If Not respuestaSRI.autorizado Then
                ret.ok = False
                ret.mensaje = respuestaSRI.mensajeError
                ret.errorServicioWeb = respuestaSRI.errorServicioWeb
            End If
        Catch ex As Exception
            ret.ok = False
            ret.mensaje = ex.Message
        End Try

        If ret.ok Then
            Try
                'Transición
                With comprobante.datos
                    .numeroIntentosTransicionAutorizacion = 0
                    .mensajeErrorAutorizacion = ""
                    .transicionAutorizacion = TRANSICION_XML_ACEPTADO
                End With
                comprobante.actualizar()
            Catch ex As Exception
                ret.ok = False
                ret.mensaje = ex.Message

                Try
                    With comprobante.datos
                        '.claveAcceso = "" 'Ya no debe cambiar la clave de acceso
                        .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 1
                        .mensajeErrorAutorizacion = ret.mensaje
                    End With
                    comprobante.actualizar()
                Catch ex2 As Exception

                End Try
            End Try
        Else
            With comprobante.datos
                If ret.errorServicioWeb Then
                    'Seguir intentando indefinidamente
                    .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion
                    .mensajeErrorAutorizacion = ret.mensaje
                Else
                    'Detener el procesamiento
                    .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 20
                    .mensajeErrorAutorizacion = ret.mensaje
                End If
            End With
            Try
                comprobante.actualizar()
            Catch ex As Exception

            End Try
        End If
        Return ret
    End Function

    Private Function obtenerNumeroAutorizacion(numeroComprobante) As EstadoProceso
        Dim comprobante As New ControladorComprobanteElectronico
        Dim ret As New EstadoProceso
        Dim claveAcceso As String = ""
        Dim respuestaSRI As New TransmisorComprobante.RespuestaSRI

        comprobante.obtener(numeroComprobante)
        Try
            respuestaSRI = TransmisorComprobante.obtenerNumeroAutorizacion(configuracion, numeroComprobante, comprobante.datos.claveAcceso)
            ret.errorServicioWeb = respuestaSRI.errorServicioWeb
            If Not respuestaSRI.autorizado Then
                ret.ok = False
                ret.mensaje = respuestaSRI.mensajeError
            End If
        Catch ex As Exception
            ret.ok = False
            ret.mensaje = ex.Message
        End Try

        If ret.ok Then
            If respuestaSRI.numeroAutorizacion.Length > 0 Then
                Try
                    'Transición
                    With comprobante.datos
                        .numeroIntentosTransicionAutorizacion = 0
                        .mensajeErrorAutorizacion = ""
                        .fechaAutorizacion = Now
                        .numeroAutorizacionSRI = respuestaSRI.numeroAutorizacion
                        .transicionAutorizacion = TRANSICION_NUMERO_AUTORIZACION
                    End With
                    comprobante.actualizar()
                Catch ex As Exception
                    ret.ok = False
                    ret.mensaje = ex.Message

                    Try
                        With comprobante.datos
                            '.claveAcceso = "" 'Ya no debe cambiar la clave de acceso
                            .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 1
                            .mensajeErrorAutorizacion = ret.mensaje
                        End With
                        comprobante.actualizar()
                    Catch ex2 As Exception

                    End Try
                End Try
            Else
                Try
                    ret.ok = True
                    ret.mensaje = "Número de autorización en blanco"

                    'Si la autorización está en blanco quiere decir que la clave de acceso no fue registrada correctamente
                    'Hay que volver a transmitir
                    With comprobante.datos
                        .numeroIntentosTransicionAutorizacion = 0
                        .mensajeErrorAutorizacion = ret.mensaje
                        .transicionAutorizacion = TRANSICION_XML_GENERADO
                    End With
                    comprobante.actualizar()
                Catch ex2 As Exception

                End Try
            End If
        Else
            With comprobante.datos
                If ret.errorServicioWeb Then
                    'Seguir intentando
                    .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion
                    .mensajeErrorAutorizacion = ret.mensaje
                Else
                    'Detener procesamiento
                    .numeroIntentosTransicionAutorizacion = .numeroIntentosTransicionAutorizacion + 20
                    .mensajeErrorAutorizacion = ret.mensaje
                End If
            End With
            Try
                comprobante.actualizar()
            Catch ex As Exception

            End Try
        End If
        Return ret
    End Function


    Private Function notificarCliente(numeroComprobante As String, tipoComprobante As String) As EstadoProceso
        Dim comprobante As New ControladorComprobanteElectronico
        Dim contribuyente As New ControladorContribuyente
        Dim ret As New EstadoProceso
        Dim nombreContribuyente As String
        Dim nombreComprobante As String

        Try
            comprobante.obtener(numeroComprobante)
            contribuyente.obtener(comprobante.datos.ruc)

            Select Case comprobante.datos.tipoComprobante
                Case TIPO_DE_COMPROBANTE_FACTURA
                    nombreContribuyente = "Cliente"
                    nombreComprobante = "Factura"
                Case TIPO_DE_COMPROBANTE_GUIA_DE_REMISION
                    nombreContribuyente = "Cliente"
                    nombreComprobante = "Guía de remisión"
                Case TIPO_DE_COMPROBANTE_NOTA_DE_CREDITO
                    nombreContribuyente = "Cliente"
                    nombreComprobante = "Nota de crédito"
				Case TIPO_DE_COMPROBANTE_COMPROBANTE_DE_RETENCION
					nombreContribuyente = "Proveedor"
					nombreComprobante = "Comprobante de retención"
				Case TIPO_DE_COMPROBANTE_LIQUIDACION_COMPRA
					nombreContribuyente = "Proveedor"
					nombreComprobante = "Liquidación de compra"
				Case Else
                    nombreContribuyente = ""
                    nombreComprobante = ""
            End Select
        Catch ex As Exception
            ret.ok = False
            ret.mensaje = ex.Message
        End Try

        If ret.ok Then
            Try
                'Transición
                With comprobante.datos
                    .fechaNotificacionCliente = Now
                    .numeroIntentosTransicionNotificacion = 0
                    .mensajeErrorNotificacion = ""
                    .transicionNotificacion = TRANSICION_LISTO_PARA_ENVIAR
                End With
                comprobante.actualizar()
            Catch ex As Exception
                ret.ok = False
                ret.mensaje = ex.Message

                Try
                    With comprobante.datos
                        '.claveAcceso = "" 'Ya no debe cambiar la clave de acceso
                        .numeroIntentosTransicionNotificacion = .numeroIntentosTransicionNotificacion + 1
                        .mensajeErrorAutorizacion = ret.mensaje
                    End With
                    comprobante.actualizar()
                Catch ex2 As Exception

                End Try
            End Try
        Else
            Try
                With comprobante.datos
                    .numeroIntentosTransicionNotificacion = .numeroIntentosTransicionNotificacion + 1
                    .mensajeErrorNotificacion = ret.mensaje
                End With
                comprobante.actualizar()
            Catch ex As Exception

            End Try
        End If
        Return ret
    End Function


    Private Function guardarXML(numeroComprobante As String) As Boolean
        Dim archivoXML As New ControladorArchivoXML

        Try
            archivoXML.obtener(numeroComprobante)
            If Not archivoXML.existe Then
                archivoXML.datos.numeroComprobante = numeroComprobante
                archivoXML.insertar()
            End If
            archivoXML.datos.archivo = System.IO.File.ReadAllBytes(configuracion.ubicacionTemporalXMLFirmados.Replace("\\", "\") & "\" & numeroComprobante & ".xml")
            archivoXML.actualizar()
            Try
                'Eliminar los XML pues ya fueron guardados en la base de datos
                If System.IO.File.Exists(configuracion.ubicacionTemporalXMLFirmados.Replace("\\", "\") & "\" & numeroComprobante & ".xml") Then
                    System.IO.File.Delete(configuracion.ubicacionTemporalXMLFirmados.Replace("\\", "\") & "\" & numeroComprobante & ".xml")
                End If
            Catch ex As Exception

            End Try

            Try
                'Eliminar los XML pues ya fueron guardados en la base de datos
                If System.IO.File.Exists(configuracion.ubicacionArchivosSinFirma.Replace("\\", "\") & "\" & numeroComprobante & ".xml") Then
                    System.IO.File.Delete(configuracion.ubicacionArchivosSinFirma.Replace("\\", "\") & "\" & numeroComprobante & ".xml")
                End If
            Catch ex As Exception

            End Try


            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


End Class
