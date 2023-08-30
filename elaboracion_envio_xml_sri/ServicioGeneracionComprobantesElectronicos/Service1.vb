Imports ClasesComprobantesElectronicos

Public Class ServicioGeneracionComprobantesElectronicos
    Private cronometro As System.Threading.Timer
    Private procesando As Boolean = False
    Private configuracion As New ConfiguracionActual

    Protected Overrides Sub OnStart(ByVal args() As String)
        configuracion.baseSQLServer = "feria"
        configuracion.contrasenaSQLServer = "admin"
        configuracion.establecimientoMatriz = "001"
        configuracion.fechaArranqueEmisionElectronica = CDate("01/08/2023")
        configuracion.maximoIntentosEntreTransiciones = 10
        configuracion.numeroMaximoLineasRide = 50
        configuracion.servidorSQLServer = "localhost"
        configuracion.puertoSqlServer = "5100"
        configuracion.tamanoFuenteCodigoBarras = 24
        configuracion.tamanoFuenteDetalle = 8
        configuracion.tamanoFuenteEstandar = 8
        configuracion.tamanoFuenteNumeros = 8
        configuracion.tamanoGridRide = 13
        configuracion.tipoAmbienteActual = 2
        configuracion.tipoFuenteCodigoBarras = "BC C39 2 to 1 Narrow"
        configuracion.tipoFuenteDetalle = "Arial Narrow"
        configuracion.tipoFuenteEstandar = "Arial"
        configuracion.tipoFuenteNumeros = "Lucida Console"
        configuracion.ubicacionArchivosSinFirma = "C:\\Comprobantes\\XML"
        configuracion.ubicacionTemporalXML = "C:\\Comprobantes\\XML"
        configuracion.ubicacionTemporalXMLFirmados = "C:\\Comprobantes\\XMLFirmados"
        configuracion.usuarioSQLServer = "admin"

        inicializarEventLog()
        Dim oCallback As New System.Threading.TimerCallback(AddressOf OnTimedEvent)
        cronometro = New System.Threading.Timer(oCallback, Nothing, 60 * 1000, 0)

        log.WriteEntry("Iniciando servicio")
        log.WriteEntry("Configuración SQL Server: Servidor: " & configuracion.servidorSQLServer & ", Base: " & configuracion.baseSQLServer & ", Usuario: " & configuracion.usuarioSQLServer & ", Contrasena: " & configuracion.contrasenaSQLServer)
        log.WriteEntry("Configuración firma electrónica: firma: " & configuracion.ubicacionFirmaElectronica & ", contraseña: " & configuracion.contrasenaFirmaElectronica & ", firmador java: " & configuracion.ubicacionFirmadorJava)
        log.WriteEntry("Ubicación archivos XML: " & configuracion.ubicacionArchivosSinFirma & " " & configuracion.ubicacionTemporalXML)
        log.WriteEntry("Ubicación archivos firmados: " & configuracion.ubicacionTemporalXMLFirmados)
        log.WriteEntry("Ubicación RIDES: " & configuracion.ubicacionTemporalRides)
        log.WriteEntry("Ubicación logo Remeco: " & configuracion.ubicacionLogoRemeco)
        log.WriteEntry("Ubicación logo 1800 Remeco: " & configuracion.ubicacionLogo1800Remeco)
        log.WriteEntry("Tipo de letra estándar: " & configuracion.tipoFuenteEstandar & " " & configuracion.tamanoFuenteEstandar)
        log.WriteEntry("Tipo de letra detalle: " & configuracion.tipoFuenteDetalle & " " & configuracion.tamanoFuenteDetalle)
        log.WriteEntry("Tipo de letra números: " & configuracion.tipoFuenteNumeros & " " & configuracion.tamanoFuenteNumeros)
        log.WriteEntry("Tipo de letra código de barras: " & configuracion.tipoFuenteCodigoBarras & " " & configuracion.tamanoFuenteCodigoBarras)
        log.WriteEntry("Contribuyente especial: " & configuracion.codigoContribuyenteEspecial & ", Establecimiento matriz: " & configuracion.establecimientoMatriz & ", Fecha arranque emisión electrónica: " & Format(configuracion.fechaArranqueEmisionElectronica, "dd/MM/yyyy"))
        log.WriteEntry("Destinatarios alertas: Sistemas: " & configuracion.destinatarioProblemasSistemas & ", Logística: " & configuracion.destinatarioProblemasLogistica & "")
        log.WriteEntry("Tipo de ambiente actual: " & configuracion.tipoAmbienteActual)
        log.WriteEntry("RIDE: Máximo líneas: " & configuracion.numeroMaximoLineasRide & ", Tamaño GRID: " & configuracion.tamanoGridRide)
        log.WriteEntry("Intentos antes de alertar: " & configuracion.maximoIntentosEntreTransiciones)
        log.WriteEntry("Frecuencia de ejecución servicio: Cada " & 60 & " segundos")
    End Sub

    Protected Overrides Sub OnStop()
        ' Agregue el código aquí para realizar cualquier anulación necesaria para detener el servicio.
    End Sub

    Public Sub inicializarEventLog()
        Try
            System.Diagnostics.EventLog.CreateEventSource("Procesamiento de comprobantes electronicos", "Feria")
        Catch ex As Exception
            'Ya existe
        End Try
        log.Log = "Feria"
        log.Source = "Procesamiento de comprobantes electronicos"
    End Sub

    Private Sub OnTimedEvent(ByVal state As Object)
        Try
            procesarTareas()
        Catch ex As Exception
            log.WriteEntry("Error en Timer " & ex.Message, System.Diagnostics.EventLogEntryType.Error)
        Finally
            cronometro.Change(60 * 1000, 0)
        End Try
    End Sub

    Public Sub procesarTareas()
        If Not procesando Then
            procesando = True

            Try
                Dim sincronizador As New ProcesadorComprobantes(configuracion)
                Try
                    sincronizador.procesarComprobantes()
                Catch ex3 As Exception
                    log.WriteEntry("Error al procesar comprobantes " & ex3.Message, System.Diagnostics.EventLogEntryType.Error)
                End Try

            Catch ex As Exception
                log.WriteEntry("Error general " & ex.Message, System.Diagnostics.EventLogEntryType.Error)
            End Try

            procesando = False
        End If
    End Sub
End Class
