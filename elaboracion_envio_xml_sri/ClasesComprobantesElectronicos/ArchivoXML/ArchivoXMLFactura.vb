Public Class ArchivoXMLFactura
    Inherits ArchivoXMLGenerico
    Private codigoCliente As String
    Private totalSinImpuestos As Double
    Private totalDescuento As Double
    Private baseImponible12 As Double
    Private iva12 As Double
    Private baseImponible0 As Double
    Private iva0 As Double
    Private voucherFactura As String
    Private porcentajeDescuentoCabecera As Double
    Private telefono As String
    Private direccionComprador As String
    Private formaPago As String

    Public Sub New(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, tipoEmision As Integer, tipoAmbiente As Integer, numeroComprobante As String)
        MyBase.New(configuracion, tipoEmision, TIPO_DE_COMPROBANTE_FACTURA, tipoAmbiente, numeroComprobante)
    End Sub

    Public Overrides Sub generar()
        Try
            obtenerEncabezadoDocumento()
            With archivoXML
                .WriteStartDocument(True)
                tagFactura()
                .WriteEndDocument()
                .Close()
            End With
        Catch ex As Exception
            hayError = True
            mensajeError = ex.Message
        End Try
    End Sub

    Private Sub tagFactura()
        With archivoXML
            .WriteStartElement("factura")
            .WriteAttributeString("id", "comprobante")
            .WriteAttributeString("version", "1.0.0")

            tagInfoTributaria()
            tagInfoFactura()
            tagDetalles()
            tagInfoAdicional()

            .WriteEndElement()
        End With
    End Sub

    Public Sub tagInfoFactura()
        With archivoXML
            .WriteStartElement("infoFactura")

            tagFechaEmision()
            tagDirEstablecimiento()
            tagObligadoContabilidad()
            tagTipoIdentificacionComprador()
            tagRazonSocialComprador()
            tagIdentificacionComprador()
            tagDireccionComprador()
            tagTotalSinImpuestos()
            tagTotalDescuento()
            tagTotalConImpuestos()
            tagPropina()
            tagImporteTotal()
            tagMoneda()
            tagPagos()

            .WriteEndElement()
        End With
    End Sub

    Public Sub tagFechaEmision()
        With archivoXML
            .WriteStartElement("fechaEmision")
            .WriteString(Format(fechaEmision, "dd/MM/yyyy"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagTipoIdentificacionComprador()
        With archivoXML
            .WriteStartElement("tipoIdentificacionComprador")
            .WriteString(cadenaCaracteres(tipoIdentificacionTercero, 2))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagRazonSocialComprador()
        With archivoXML
            .WriteStartElement("razonSocialComprador")
            .WriteString(cadenaCaracteres(razonSocialTercero, 300))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagIdentificacionComprador()
        With archivoXML
            .WriteStartElement("identificacionComprador")
            .WriteString(cadenaCaracteres(identificacionTercero, 20))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDireccionComprador()
        With archivoXML
            .WriteStartElement("direccionComprador")
            .WriteString(cadenaCaracteres(direccionComprador, 300))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagTotalSinImpuestos()
        With archivoXML
            .WriteStartElement("totalSinImpuestos")
            .WriteString(Format(totalSinImpuestos, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagTotalDescuento()
        With archivoXML
            .WriteStartElement("totalDescuento")
            .WriteString(Format(totalDescuento, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    'TODO Modificar para agregar el 14%
    Public Sub tagTotalConImpuestos()
        With archivoXML
            .WriteStartElement("totalConImpuestos")

            tagTotalImpuesto(CODIGO_IMPUESTO_IVA, TARIFA_IVA_12, baseImponible12, iva12, 12)
            tagTotalImpuesto(CODIGO_IMPUESTO_IVA, TARIFA_IVA_0, baseImponible0, iva0, 0)

            .WriteEndElement()
        End With
    End Sub

    Public Sub tagTotalImpuesto(codigo As Integer, codigoPorcentaje As Decimal, baseImponible As Decimal, valor As Decimal, tarifa As Integer)
        With archivoXML
            .WriteStartElement("totalImpuesto")

            tagCodigo(codigo)
            tagCodigoPorcentaje(codigoPorcentaje)
            tagBaseImponible(baseImponible)
            tagTarifa(tarifa)
            tagValor(valor)

            .WriteEndElement()
        End With

    End Sub

    Public Sub tagCodigo(codigo As Integer)
        With archivoXML
            .WriteStartElement("codigo")
            .WriteString(codigo.ToString)
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagCodigoPorcentaje(codigoPorcentaje As Integer)
        With archivoXML
            .WriteStartElement("codigoPorcentaje")
            .WriteString(codigoPorcentaje.ToString)
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagBaseImponible(valor As Decimal)
        With archivoXML
            .WriteStartElement("baseImponible")
            .WriteString(Format(valor, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagValor(valor As Decimal)
        With archivoXML
            .WriteStartElement("valor")
            .WriteString(Format(valor, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagPropina()
        With archivoXML
            .WriteStartElement("propina")
            .WriteString(Format(0, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagImporteTotal()
        With archivoXML
            .WriteStartElement("importeTotal")
            .WriteString(Format(totalSinImpuestos + iva12 + iva0, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagMoneda()
        With archivoXML
            .WriteStartElement("moneda")
            .WriteString(cadenaCaracteres("DOLAR", 15))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDetalles()
        Dim consulta As String
        Dim tabla As DataTable
        Dim detalle As ClaseValorDetalleFactura
        Dim inventDimId As String
        Dim inventDim As DataTable
        Dim inventTransId As String
        Dim taxTrans As DataTable


        consulta = ""
        consulta = consulta & "select i.codigo_item, "
        consulta = consulta & "i.nombre_item, "
        consulta = consulta & "a.cantidad, "
        consulta = consulta & "case when valor_descuento is null Then 0 else valor_descuento end as valor_descuento, "
        consulta = consulta & "(i.precio_item / 1.12) as precio_item, fc.iva "
        consulta = consulta & "from detalle_factura a "
        consulta = consulta & "join facturas_cabecera fc on a.codigo_factura = fc.codigo_factura "
        consulta = consulta & "join item i on a.codigo_item = i.codigo_item "
        consulta = consulta & "where fc.numero_factura = '" & numeroComprobanteDynamics & "';"


        tabla = sqlServer.consultar(consulta)
        With archivoXML
            .WriteStartElement("detalles")
            For Each registro As DataRow In tabla.Rows

                detalle = New ClaseValorDetalleFactura
                detalle.codigoPrincipal = registro("nombre_item")
                If detalle.codigoPrincipal.Length = 0 Then detalle.codigoPrincipal = registro("nombre_item")
                detalle.codigoAuxiliar = ""
                detalle.descripcion = registro("nombre_item")
                detalle.cantidad = Math.Abs(registro("cantidad"))
                detalle.precioUnitario = Math.Abs(registro("precio_item"))
                detalle.descuento = Math.Abs(registro("valor_descuento"))
                detalle.precioTotalSinImpuesto = Math.Abs(registro("precio_item"))
                Dim valorIva As Double = Math.Abs(registro("iva"))

                If valorIva <> 0 Then
                    detalle.iva12 = Math.Abs(registro("iva"))
                    detalle.baseImponible12 = totalSinImpuestos
                Else
                    detalle.iva0 = valorIva
                    detalle.baseImponible0 = totalSinImpuestos
                End If

                tagDetalle(detalle)
            Next
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagPagos()
        With archivoXML
            .WriteStartElement("pagos")
            tagPago(totalSinImpuestos + iva12, 0)
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDetalle(detalle As ClaseValorDetalleFactura)
        With archivoXML
            .WriteStartElement("detalle")
            tagCodigoPrincipal(detalle.codigoPrincipal)
            tagDescripcion(detalle.descripcion)
            tagCantidad(detalle.cantidad)
            tagPrecioUnitario(detalle.precioUnitario)
            tagDescuento(detalle.descuento)
            tagPrecioTotalSinImpuesto(detalle.precioTotalSinImpuesto)
            tagDetallesAdicionales(detalle.porcentajeDescuento)
            tagImpuestos(detalle)
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagPago(ByVal total As Decimal, ByVal plazo As Integer)
        With archivoXML
            .WriteStartElement("pago")
            tagFormaPago()
            tagTotal(total)
            tagPlazo(plazo)
            tagUnidadTiempo()
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagCodigoPrincipal(valor As String)
        With archivoXML
            .WriteStartElement("codigoPrincipal")
            .WriteString(cadenaCaracteres(valor, 25))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagFormaPago()
        With archivoXML
            .WriteStartElement("formaPago")
            .WriteString(cadenaCaracteres(formaPago, 2))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagTotal(ByVal valor As Decimal)
        With archivoXML
            .WriteStartElement("total")
            .WriteString(Format(valor, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagPlazo(ByVal valor As Integer)
        With archivoXML
            .WriteStartElement("plazo")
            .WriteString(Format(valor, "0"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagUnidadTiempo()
        With archivoXML
            .WriteStartElement("unidadTiempo")
            .WriteString(cadenaCaracteres("Días", 10))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagCodigoAuxiliar(valor As String)
        With archivoXML
            .WriteStartElement("codigoAuxiliar")
            .WriteString(cadenaCaracteres(valor, 25))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDescripcion(valor As String)
        With archivoXML
            .WriteStartElement("descripcion")
            .WriteString(cadenaCaracteres(valor, 300))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagCantidad(valor As Decimal)
        With archivoXML
            .WriteStartElement("cantidad")
            .WriteString(Format(valor, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagPrecioUnitario(valor As Decimal)
        With archivoXML
            .WriteStartElement("precioUnitario")
            .WriteString(Format(valor, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDescuento(valor As Decimal)
        With archivoXML
            .WriteStartElement("descuento")
            .WriteString(Format(valor, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagPrecioTotalSinImpuesto(valor As Decimal)
        With archivoXML
            .WriteStartElement("precioTotalSinImpuesto")
            .WriteString(Format(valor, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDetallesAdicionales(porcentajeDescuento As Decimal)
        With archivoXML
            If Not porcentajeDescuento = 0 Then
                .WriteStartElement("detallesAdicionales")
                If Not porcentajeDescuento = 0 Then
                    tagDetAdicional(porcentajeDescuento)
                End If
                .WriteEndElement()
            End If
        End With
    End Sub

    Public Sub tagDetAdicional(numeroSerie As String)
        With archivoXML
            .WriteStartElement("detAdicional")
            .WriteAttributeString("nombre", "numeroSerie")
            .WriteAttributeString("valor", numeroSerie)
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDetAdicional(porcentajeDescuento As Double)
        With archivoXML
            .WriteStartElement("detAdicional")
            .WriteAttributeString("nombre", "porcentajeDescuento")
            .WriteAttributeString("valor", Format(porcentajeDescuento, "0.00"))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagImpuestos(detalle As ClaseValorDetalleFactura)
        With archivoXML
            .WriteStartElement("impuestos")
            If Not detalle.baseImponible12 = 0 Then
                tagImpuesto(CODIGO_IMPUESTO_IVA, TARIFA_IVA_12, detalle.baseImponible12, detalle.iva12, 12)
            End If
            If Not detalle.baseImponible0 = 0 Then
                tagImpuesto(CODIGO_IMPUESTO_IVA, TARIFA_IVA_0, detalle.baseImponible0, detalle.iva0, 0)
            End If


            .WriteEndElement()
        End With
    End Sub

    Public Sub tagImpuesto(codigo As Integer, codigoPorcentaje As Decimal, baseImponible As Decimal, valor As Decimal, tarifa As Integer)
        With archivoXML
            .WriteStartElement("impuesto")

            tagCodigo(codigo)
            tagCodigoPorcentaje(codigoPorcentaje)
            tagTarifa(tarifa)
            tagBaseImponible(baseImponible)
            tagValor(valor)

            .WriteEndElement()
        End With

    End Sub

    Public Sub tagTarifa(valor As Integer)
        With archivoXML
            .WriteStartElement("tarifa")
            .WriteString(valor.ToString)
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagInfoAdicional()
        Dim cadena As String = " "

        cadena = cadena.Trim()
        With archivoXML
            .WriteStartElement("infoAdicional")
            tagCampoAdicional("datosCliente", codigoCliente & "||" & telefono) 'codCli||tlf||vend||numeroPedido||obs
            If Len(cadena) > 0 Then
                tagCampoAdicional("vencimientos", cadena) 'v1 v2 v3 v4
            End If
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagCampoAdicional(nombre As String, valor As String)
        With archivoXML
            .WriteStartElement("campoAdicional")
            .WriteAttributeString("nombre", nombre)
            .WriteString(valor)
            .WriteEndElement()
        End With
    End Sub

    Public Overrides Sub obtenerEncabezadoDocumento()
        Dim tabla As DataTable
        Dim consulta As String = ""
        Dim registro As DataRow
        Dim recIdCustPaymSched As Long
        Dim i As Integer
        Dim fechaVencimiento As Date
        Dim valorVencimiento As Double


        consulta = consulta & "SELECT a.numero_factura, "
        consulta = consulta & "a.fecha_factura, "
        consulta = consulta & "a.codigo_cliente, "
        consulta = consulta & "c.direccion, "
        consulta = consulta & "(a.valor_total_factura - a.iva) as valorSinIva, "
        consulta = consulta & "CASE "
        consulta = consulta & "WHEN valor_descuento IS NULL THEN 0 "
        consulta = consulta & "ELSE valor_descuento "
        consulta = consulta & "END as valor_descuento, a.iva, d.codigo_sri_forma_pago "
        consulta = consulta & "FROM facturas_cabecera a "
        consulta = consulta & "JOIN cliente c ON a.codigo_cliente = c.codigo_cliente "
        consulta = consulta & "JOIN forma_pago d ON d.codigo_forma_pago = a.forma_pago "
        consulta = consulta & "WHERE numero_factura = '" & numeroComprobanteDynamics & "';"



        tabla = sqlServer.consultar(consulta)
        registro = tabla.Rows(0)
        fechaEmision = registro("fecha_factura")
        voucherFactura = registro("numero_factura")
        codigoCliente = registro("codigo_cliente")
        direccionComprador = registro("direccion")
        totalSinImpuestos = Math.Abs(registro("valorSinIva") - registro("valor_descuento"))
        totalDescuento = Math.Abs(registro("valor_descuento"))
        formaPago = registro("codigo_sri_forma_pago")
        Dim valorIva As Double = Math.Abs(registro("iva"))

        If valorIva <> 0 Then
            iva12 = Math.Abs(registro("iva"))
            baseImponible12 = totalSinImpuestos
        Else
            iva0 = valorIva
            baseImponible0 = totalSinImpuestos
        End If



        'Datos fiscales cliente
        consulta = ""
        consulta = consulta & "SELECT numero_cedula, tipo_identificacion, telefono, nombre FROM cliente WHERE codigo_Cliente = '" & codigoCliente & "';"

        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            Try
                tipoIdentificacionTercero = convertirTipoIdentificacionDynamics(registro("tipo_identificacion"))
                If tipoIdentificacionTercero = TIPO_IDENTIFICACION_IDENTIFICACION_DEL_EXTERIOR Or tipoIdentificacionTercero = TIPO_IDENTIFICACION_VENTA_A_CONSUMIDOR_FINAL Then
                    identificacionTercero = "9999999999999"
                Else
                    identificacionTercero = registro("numero_cedula")
                End If
                razonSocialTercero = registro("nombre")
                telefono = registro("telefono")
            Catch ex As Exception

            End Try
        Else

        End If

    End Sub

End Class