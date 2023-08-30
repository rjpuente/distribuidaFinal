Imports ClasesComprobantesElectronicos

Module Module1

    Sub Main()

        Dim configuracion As New ClasesComprobantesElectronicos.ConfiguracionActual

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

        Dim sincronizador As New ProcesadorComprobantes(configuracion)

        sincronizador.procesarComprobantes()
        'sincronizador.generarXMLNormal("F_001002000000006")


        'Try
        ''sincronizador.procesarComprobantes()
        ''Catch ex2 As Exception
        ''Dim tb As String
        ''tb = ex2.Message

        ''End Try
        'sincronizador.procesarComprobantes()

        'Dim sqlServer2 As New ConectorSQLServer(configuracion)
        'notificarClientes(sqlServer2)

        'Dim archivo2 As New RideGuiaRemision(configuracion, "GR_001002000085685", 1, configuracion.tipoAmbienteActual)

        'Dim archivo As New ArchivoXMLComprobanteRetencion(configuracion, 1, configuracion.tipoAmbienteActual, "001002000013312")
        'archivo.generar()

        'Dim archivo As New RideRetencion(configuracion, "001002000013389", 1, configuracion.tipoAmbienteActual)

    End Sub

    Sub notificarClientes(ByVal sqlServer2 As ConectorSQLServer)
        Dim rsContribuyentes As DataTable = sqlServer2.consultar("SELECT * FROM REMECO.DBO.CONTRIBUYENTE")
        For Each contribuyente As DataRow In rsContribuyentes.Rows
            enviarNotificacion("Envío de retenciones", contribuyente("ruc"), sqlServer2)
        Next
    End Sub

    Sub enviarNotificacion(ByVal asunto As String, ByVal ruc As String, ByVal sqlServer2 As ConectorSQLServer)
        Dim rsContribuyente As DataTable = sqlServer2.consultar("SELECT * FROM REMECO.DBO.CONTRIBUYENTE WHERE RUC='" & ruc & "'")
        If rsContribuyente.Rows.Count > 0 Then
            Dim contribuyente As DataRow = rsContribuyente.Rows(0)
            Dim notificacion As New Notificacion(asunto)
            notificacion.agregarParrafo("Estimados Clientes:")
            notificacion.agregarParrafo("El presente comunicado es para informarles que las Retenciones emitidas por ustedes en el período fiscal 2016 serán recibidas por Remeco hasta el  16 de Enero del 2017.")
            notificacion.agregarParrafo("Recuerden que nos las pueden hacer llegar por fax al número 022437168 ext. 229, vía correo electrónico a la dirección: comprobanteselectronicos@remeco.net o entregándolas a nuestros Asesores Comerciales.")
            notificacion.agregarParrafo("Muchas Gracias")
            Dim email As String = ""
            Try
                email = contribuyente("EMAIL")
            Catch ex As Exception

            End Try
            If email.Length > 0 Then
                notificacion.agregarListaSeparada(email, ";")
                notificacion.ordenarEnvio()
            End If
        End If
    End Sub
End Module
