Public Class RideNotaCredito
    Inherits RIDEGenerico

    Public Class LineaNotaCredito
        Public codigo As String
        Public descripcion As String
        Public serie As String
        Public cantidad As Long
        Public unidad As String
        Public precioUnitario As Double
        Public descuento As Double
        Public valorTotal As Double
    End Class

    Public baseImponible0 As Double
	Public baseImponible12 As Double
	Public baseImponible14 As Double
    Public codigoCliente As String
    Public fechaDocumentoModificado As Date
    Public iva0 As Double
	Public iva12 As Double
	Public iva14 As Double
    Public motivoModificacion As String
    Public numeroDocumentoModificado As String
    Public observaciones As String
    Public porcentajeDescuentoCabecera As Double
    Public salesId As String
    Public telefono As String
    Public tipoDocumentoModificado As String
    Public totalDescuento As Double
    Public totalSinImpuestos As Double
    Public valorModificacion As Double
    Public vendedor As String
    Public voucherNotaCredito As String
    Public direccionCliente As String
    Public descuentoPie As Double

    Public Sub New(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, numeroComprobanteDynamics As String, tipoEmision As Integer, tipoAmbiente As Integer)
        MyBase.New(configuracion, numeroComprobanteDynamics, TIPO_DE_COMPROBANTE_NOTA_DE_CREDITO, tipoEmision, tipoAmbiente)
        Me.obtenerDocumento()
        Me.inicializarDocumento()
        Me.generar()
    End Sub

    Public Overrides Sub generar()
        textoNegrita("CLIENTE:", fila, 1)
        textoNormal(razonSocialTercero, fila, 4)
        textoNegrita("R.U.C.:", fila, 26)
        textoNormal(identificacionTercero, fila, 29)
        siguienteFila()

        textoNegrita("CÓDIGO:", fila, 1)
        textoNormal(codigoCliente, fila, 4)
        textoNegrita("TELÉFONO:", fila, 21)
        textoNormal(telefono, fila, 25)
        siguienteFila()

        textoNegrita("DOCUMENTO MODIFICADO:", fila, 1)
        textoNormal(numeroDocumentoModificado, fila, 10)
        textoNegrita("EMISIÓN DOCUMENTO MODIFICADO:", fila, 21)
        textoNormal(Format(fechaDocumentoModificado, "dd/MM/yyyy"), fila, 33)
        siguienteFila()

        textoNegrita("RAZÓN DE MODIFICACIÓN:", fila, 1)
        textoNormal(motivoModificacion, fila, 10)
        siguienteFila()

        textoNegrita("DIRECCIÓN:", fila, 1)
        Try
            direccionCliente = direccionCliente.Substring(0, 90)
        Catch ex As Exception

        End Try
        textoNormal(direccionCliente, fila, 5)
        siguienteFila()

        textoNegrita("OBSERVACIONES:", fila, 1)
        textoNormal(observaciones, fila, 7)
        siguienteFila()
        siguienteFila()

        'Detalles
        textoNegrita("CÓDIGO", fila, 1)
        textoNegrita("DESCRIPCIÓN", fila, 7)
        textoNegrita("CANTIDAD", fila, 23)
        textoNegrita("UNIDAD", fila, 27)
        textoNegrita("P. UNITARIO", fila, 31)
        textoNegrita("DSCTO.", fila, 36)
        textoNegrita("TOTAL", fila, 42)
        siguienteFila()

        Dim consulta As String = "SELECT * FROM CUSTINVOICETRANS WHERE INVOICEID='" & numeroComprobanteDynamics & "'"
        Dim tabla As DataTable = sqlServer.consultar(consulta)

        For Each registro As DataRow In tabla.Rows

            Dim detalle As New LineaNotaCredito
            detalle.codigo = registro("ITEMID")
            detalle.descripcion = registro("NAME")
            detalle.cantidad = Math.Abs(registro("QTY"))
            detalle.precioUnitario = Math.Abs(registro("SALESPRICE"))
            detalle.descuento = Math.Abs(registro("DISCPERCENT"))
            detalle.valorTotal = Math.Abs(registro("LINEAMOUNT"))
            detalle.unidad = registro("SALESUNIT")

            Dim inventDimId As String = registro("INVENTDIMID")
            consulta = "SELECT INVENTSERIALID FROM INVENTDIM WHERE DATAAREAID='REM' AND INVENTDIMID='" & inventDimId & "'"
            Dim inventDim As DataTable = sqlServer.consultar(consulta)
            If inventDim.Rows.Count > 0 Then
                detalle.serie = inventDim.Rows(0)("INVENTSERIALID")
            Else
                detalle.serie = ""
            End If

            textoDetalle(detalle.codigo, fila, 1)
            textoDetalle(detalle.descripcion, fila, 7)
            textoNumeros(FormatNumber(detalle.cantidad, 0), 7, fila, 22)
            textoDetalle(detalle.unidad, fila, 27)
            textoNumeros(FormatNumber(detalle.precioUnitario, 2), 10, fila, 30)
            textoNumeros(FormatNumber(detalle.descuento, 2), 6, fila, 36)
            textoNumeros(FormatNumber(detalle.valorTotal, 2), 10, fila, 40)
            siguienteFila()
            If detalle.serie.Length > 0 Then
                textoDetalle("S/N: " & detalle.serie, fila, 7)
                siguienteFila()
            End If
        Next

        siguienteFila()
		textoNormal("Valor:", fila, 33)
		textoNumeros(FormatNumber(totalSinImpuestos - descuentoPie, 2), 10, fila, 40)
		siguienteFila()

		textoNormal("Descuento " & FormatNumber(porcentajeDescuentoCabecera * 100, 1) & "%:", fila, 33)
		textoNumeros(FormatNumber(-descuentoPie, 2), 10, fila, 40)
		siguienteFila()

		textoNormal("Subtotal:", fila, 33)
		textoNumeros(FormatNumber(baseImponible0 + baseImponible12 + baseImponible14, 2), 10, fila, 40)
		siguienteFila()

		textoNormal("Tarifa 0%:", fila, 33)
		textoNumeros(FormatNumber(baseImponible0, 2), 10, fila, 40)
		siguienteFila()

		Dim porcentajeIva As Double = 100 * (iva12 + iva14) / (baseImponible12 + baseImponible14)

		textoNormal("Tarifa " & FormatNumber(porcentajeIva, 0) & "%:", fila, 33)
		textoNumeros(FormatNumber(baseImponible12 + baseImponible14, 2), 10, fila, 40)
		siguienteFila()

		textoNormal("IVA " & FormatNumber(porcentajeIva, 0) & "%:", fila, 33)
		textoNumeros(FormatNumber(iva12 + iva14, 2), 10, fila, 40)
		siguienteFila()

		textoNegrita("TOTAL:", fila, 33)
		textoNumerosNegrita(FormatNumber(totalSinImpuestos + iva12 + iva14, 2), 10, fila, 40)
		siguienteFila()

        dibujarLogotipoFinDocumento()
        guardar()
    End Sub

    Public Overrides Sub obtenerDocumento()
        Dim tabla As DataTable
        Dim consulta As String = ""
        Dim registro As DataRow
        Dim recIdCustInvoiceJour As Long

        'Datos notaCredito
        consulta = consulta & "SELECT * FROM CUSTINVOICEJOUR A  WHERE "
        consulta = consulta & "A.DATAAREAID='REM' AND "
        consulta = consulta & "A.INVOICEID='" & numeroComprobanteDynamics & "'"

        tabla = sqlServer.consultar(consulta)
        registro = tabla.Rows(0)
        fechaEmision = registro("INVOICEDATE")
        If registro("MODDOCTYPE") = "18" Then
            tipoDocumentoModificado = "01"
        Else
            tipoDocumentoModificado = registro("MODDOCTYPE")
        End If
        numeroDocumentoModificado = registro("MODIFIEDDOCNUM")
        salesId = registro("SALESID")
        voucherNotaCredito = registro("LEDGERVOUCHER")
        codigoCliente = registro("INVOICEACCOUNT")
        recIdCustInvoiceJour = registro("RECID")
        totalSinImpuestos = Math.Abs(registro("SALESBALANCE") - registro("ENDDISC"))
        valorModificacion = Math.Abs(registro("INVOICEAMOUNT"))
        If registro("SALESBALANCE") <> 0 Then
            porcentajeDescuentoCabecera = registro("ENDDISC") / registro("SALESBALANCE")
            descuentoPie = registro("ENDDISC")
        Else
            porcentajeDescuentoCabecera = 0
        End If

        totalDescuento = Math.Abs(registro("SUMLINEDISC") + registro("ENDDISC"))

        'Fecha del documento modificado
        consulta = ""
        consulta = consulta & "SELECT INVOICEDATE FROM CUSTINVOICEJOUR WHERE DATAAREAID='REM' AND INVOICEID='" & numeroDocumentoModificado & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            fechaDocumentoModificado = registro("INVOICEDATE")
        Else
            fechaDocumentoModificado = CDate("01/01/1900")
        End If

        'Datos fiscales cliente
        consulta = ""
        consulta = consulta & "SELECT * FROM CUSTDATATABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoCliente & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            Try
                tipoIdentificacionTercero = convertirTipoIdentificacionDynamics(registro("SRIIDTYPE"))
                If tipoIdentificacionTercero = TIPO_IDENTIFICACION_IDENTIFICACION_DEL_EXTERIOR Or tipoIdentificacionTercero = TIPO_IDENTIFICACION_VENTA_A_CONSUMIDOR_FINAL Then
                    identificacionTercero = "9999999999999"
                Else
                    identificacionTercero = registro("IDENTIFICATIONNUMBER")
                End If
                razonSocialTercero = registro("SOCIALNAME")
            Catch ex As Exception

            End Try
        Else

        End If

        'Datos cliente
        consulta = ""
        consulta = consulta & "SELECT * FROM CUSTTABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoCliente & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            Try
                telefono = registro("PHONE")
            Catch ex As Exception

            End Try
        Else

        End If

        'IVA
        consulta = ""
        consulta = consulta & "SELECT * FROM TAXTRANS A,TAXTABLE B,TAXDATA C WHERE "
        consulta = consulta & "A.DATAAREAID=B.DATAAREAID AND A.TAXCODE=B.TAXCODE AND "
        consulta = consulta & "C.DATAAREAID=B.DATAAREAID AND C.TAXCODE=B.TAXCODE AND "
        consulta = consulta & "A.DATAAREAID='REM' AND "
        consulta = consulta & "(B.TIPOIMPUESTO=3 OR B.TIPOIMPUESTO=4) AND " 'IVA
        consulta = consulta & "C.TAXFROMDATE<='" & Format(fechaEmision, "dd/MM/yyyy") & "' AND C.TAXTODATE>='" & Format(fechaEmision, "dd/MM/yyyy") & "' AND "
        consulta = consulta & "A.VOUCHER='" & voucherNotaCredito & "'"
        tabla = sqlServer.consultar(consulta)
        For Each registro In tabla.Rows
            Try
                If registro("TAXVALUE") = 12 Then
					baseImponible12 = baseImponible12 + registro("SOURCEBASEAMOUNTCUR")
					iva12 = iva12 + registro("SOURCETAXAMOUNTCUR")
				ElseIf registro("TAXVALUE") = 14 Then
                    baseImponible14 = baseImponible14 + registro("SOURCEBASEAMOUNTCUR")
                    iva14 = iva14 + registro("SOURCETAXAMOUNTCUR")
                ElseIf registro("TAXVALUE") = 0 Then
                    baseImponible0 = baseImponible0 + Math.Abs(registro("SOURCEBASEAMOUNTCUR"))
                    iva0 = registro("SOURCETAXAMOUNTCUR")
                End If
            Catch ex As Exception

            End Try
        Next

        'SalesTable
        consulta = "SELECT * FROM SALESTABLE WHERE DATAAREAID='REM' AND SALESID='" & salesId & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            observaciones = tabla.Rows(0)("CUSTOMERREF")
            motivoModificacion = tabla.Rows(0)("MODIFICATIONMOTIVE")
            If tabla.Rows(0)("COMISIONISTAEXCLUSIVO").ToString.Length > 0 Then
                vendedor = tabla.Rows(0)("COMISIONISTAEXCLUSIVO")
            Else
                vendedor = tabla.Rows(0)("DLVTERM")
            End If
        Else
            observaciones = ""
            motivoModificacion = "DEVOLUCION"
            vendedor = ""
        End If

        'SalesTable
        consulta = "SELECT * FROM SALESTABLE WHERE DATAAREAID='REM' AND SALESID='" & salesId & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            observaciones = tabla.Rows(0)("CUSTOMERREF")
            motivoModificacion = tabla.Rows(0)("MODIFICATIONMOTIVE")
            direccionCliente = tabla.Rows(0)("DELIVERYADDRESS")
            If tabla.Rows(0)("COMISIONISTAEXCLUSIVO").ToString.Length > 0 Then
                vendedor = tabla.Rows(0)("COMISIONISTAEXCLUSIVO")
            Else
                vendedor = tabla.Rows(0)("DLVTERM")
            End If
        Else
            observaciones = ""
            motivoModificacion = "DEVOLUCION"
            direccionCliente = ""
            vendedor = ""
        End If

        'Motivo de modificación
        consulta = "SELECT DESCRIPTION FROM MODIFICATIONMOTIVE WHERE DATAAREAID='REM' AND ID='" & motivoModificacion & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            motivoModificacion = tabla.Rows(0)("DESCRIPTION")
        End If
    End Sub
End Class
