Public Class RIDEFactura
    Inherits RIDEGenerico

    Public Class LineaFactura
        Public codigo As String
        Public descripcion As String
        Public serie As String
        Public cantidad As Long
        Public unidad As String
        Public precioUnitario As Double
        Public descuento As Double
        Public valorTotal As Double
        Public esServicio As Boolean
    End Class

    Public direccionCliente As String
    Public telefonoCliente As String
    Public codigoCliente As String
    Public codigoSeguimiento As String
    Public observaciones As String
    Public vencimientos As String
	Public baseImponible12 As Double = 0
	Public baseImponible14 As Double = 0
    Public baseImponible0 As Double = 0
    Public porcentajeDescuentoPie As Double
    Public descuentoPie As Double
	Public iva12 As Double = 0
	Public iva14 As Double = 0
    Public valorTotal As Double
    Public numeroPedido As String
    Public porcentajeDescuentoCabecera As Double
    Public salesId As String
    Public totalDescuento As Double
    Public totalSinImpuestos As Double
    Public vencimiento1 As String
    Public vencimiento2 As String
    Public vencimiento3 As String
    Public vencimiento4 As String
    Public vendedor As String
    Public voucherFactura As String
    Public recIdCustInvoiceJour As Long
    Public recIdCustPaymSched As Long
    Public registradoPaginaWeb As Boolean

    Public Sub New(ByVal configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, ByVal numeroComprobanteDynamics As String, ByVal tipoEmision As Integer, ByVal tipoAmbiente As Integer)
        MyBase.New(configuracion, numeroComprobanteDynamics, TIPO_DE_COMPROBANTE_FACTURA, tipoEmision, tipoAmbiente)
        Me.obtenerDocumento()
        Me.inicializarDocumento()
        Me.generar()
    End Sub

    Public Overrides Sub generar()
        Dim totalVentasBienes As Double = 0

        textoNegrita("CLIENTE:", fila, 1)
        textoNormal(razonSocialTercero, fila, 4)
        textoNegrita("R.U.C.:", fila, 26)
        textoNormal(identificacionTercero, fila, 29)
        siguienteFila()

        textoNegrita("CÓDIGO:", fila, 1)
        textoNormal(codigoCliente, fila, 4)
        textoNegrita("TELÉFONO:", fila, 26)
        textoNormal(telefonoCliente, fila, 30)
        siguienteFila()

        textoNegrita("DIRECCIÓN:", fila, 1)
        Try
            direccionCliente = direccionCliente.Substring(0, 90)
        Catch ex As Exception

        End Try
        textoNormal(direccionCliente, fila, 5)
        textoNegrita("COD. SEGUIMIENTO:", fila, 26)
        textoNormal(codigoSeguimiento, fila, 33)
        siguienteFila()

        textoNegrita("OBSERVACIONES:", fila, 1)
        textoNormal(observaciones, fila, 7)
        siguienteFila()

        textoNegrita("VENCIMIENTOS:", fila, 1)
        textoNormal(vencimiento1 & "   " & vencimiento2 & "   " & vencimiento3 & "   " & vencimiento4, fila, 7)
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

        Dim consulta As String = "SELECT * FROM CUSTINVOICETRANS A LEFT JOIN INVENTTABLE B ON A.DATAAREAID=B.DATAAREAID AND A.ITEMID=B.ITEMID WHERE A.DATAAREAID='REM' AND INVOICEID='" & numeroComprobanteDynamics & "'"
        Dim tabla As DataTable = sqlServer.consultar(consulta)

        For Each registro As DataRow In tabla.Rows

            Dim detalle As New LineaFactura
            detalle.codigo = registro("ITEMID")
            detalle.descripcion = registro("NAME")
            detalle.cantidad = Math.Abs(registro("QTY"))
            detalle.precioUnitario = Math.Abs(registro("SALESPRICE"))
            detalle.descuento = Math.Abs(registro("DISCPERCENT"))
            detalle.valorTotal = Math.Abs(registro("LINEAMOUNT"))
            detalle.unidad = registro("SALESUNIT")
            Dim itemGroupId As String
            Dim taxItemGroup As String
            If IsDBNull(registro("ITEMGROUPID")) Then
                itemGroupId = ""
                detalle.esServicio = True
            Else
                itemGroupId = registro("ITEMGROUPID")
                taxItemGroup = registro("TAXITEMGROUP")
                If taxItemGroup.Length > 0 Then
                    If taxItemGroup.Substring(0, 2) = "VB" Then
                        detalle.esServicio = False
                    Else
                        detalle.esServicio = True
                    End If
                Else
                    detalle.esServicio = True
                End If
            End If

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

            If Not detalle.esServicio And Not itemGroupId = "29" Then
                totalVentasBienes = totalVentasBienes + detalle.valorTotal
            End If
        Next

		siguienteFila()
		textoNormal("Valor:", fila, 33)
		textoNumeros(FormatNumber(totalSinImpuestos + descuentoPie, 2), 10, fila, 40)
		siguienteFila()

		textoNormal("Descuento " & FormatNumber(porcentajeDescuentoCabecera * 100, 1) & "%:", fila, 33)
		textoNumeros(FormatNumber(descuentoPie, 2), 10, fila, 40)
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
        siguienteFila()

        'Formas de pago
        textoNegrita("FORMA DE PAGO", fila, 1)
        textoNegrita("VALOR", fila, 16)
        textoNegrita("PLAZO", fila, 21)
        textoNegrita("TIEMPO", fila, 26)
        siguienteFila()

        consulta = "SELECT * FROM CUSTPAYMSCHED WHERE DATAAREAID='REM' AND EXTTABLEID=62 AND EXTRECID=" & recIdCustInvoiceJour.ToString
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            recIdCustPaymSched = tabla.Rows(0)("RECID")
            consulta = "SELECT * FROM CUSTPAYMSCHEDLINE WHERE DATAAREAID='REM' AND PARENTRECID=" & recIdCustPaymSched.ToString
            tabla = sqlServer.consultar(consulta)

            For Each registro In tabla.Rows
                Dim fechaVencimiento As Date
                Dim valorVencimiento As Double
                fechaVencimiento = registro("DUEDATE")
                valorVencimiento = registro("DUEAMOUNT")
                Dim dias As Integer = fechaVencimiento.Subtract(fechaEmision).Days

                textoDetalle("OTROS CON UTILIZACION DEL SISTEMA FINANCIERO", fila, 1)
                textoNumeros(FormatNumber(valorVencimiento, 2), 10, fila, 16)
                textoNumeros(FormatNumber(dias, 0), 7, fila, 21)
                textoDetalle("Días", fila, 26)
                siguienteFila()
            Next
        Else
            textoDetalle("OTROS CON UTILIZACION DEL SISTEMA FINANCIERO", fila, 1)
            textoNumeros(FormatNumber(totalSinImpuestos + iva12 + iva14, 2), 10, fila, 16)
            textoNumeros(FormatNumber(0, 0), 7, fila, 21)
            textoDetalle("Días", fila, 26)
            siguienteFila()
        End If

        'Plan de fidelización Distribuciones
        Dim planFidelizacionDesde As Date = New Date(2018, 4, 1)
        Dim planFidelizacionHasta As Date = New Date(2018, 9, 30)
        consulta = "SELECT POLITICACOMERCIAL FROM CUSTTABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoCliente & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            Dim politicaComercial As String = tabla.Rows(0)("POLITICACOMERCIAL")
            If (politicaComercial = "FERR" Or politicaComercial = "FERRPREM" Or politicaComercial = "MAYO" Or politicaComercial = "MAYOPREM") And fechaEmision >= planFidelizacionDesde And fechaEmision <= planFidelizacionHasta And registradoPaginaWeb Then
                Dim numeroPuntos = totalVentasBienes \ 10
                siguienteFila()
                siguienteFila()
                textoNegrita("GRACIAS POR SU COMPRA", fila, 10)
                siguienteFila()
                textoNegrita("EN ESTA FACTURA USTED ACUMULÓ " & Trim(CStr(numeroPuntos)) & " PUNTOS!", fila, 10)
                siguienteFila()
            End If
        End If

        dibujarLogotipoFinDocumento()
        guardar()
    End Sub

    Public Overrides Sub obtenerDocumento()
        Dim tabla As DataTable
        Dim consulta As String = ""
        Dim registro As DataRow
        Dim i As Integer
        Dim fechaVencimiento As Date
        Dim valorVencimiento As Double

        'Datos factura
        consulta = consulta & "SELECT * FROM CUSTINVOICEJOUR A  WHERE "
        consulta = consulta & "A.DATAAREAID='REM' AND "
        consulta = consulta & "A.INVOICEID='" & numeroComprobanteDynamics & "'"

        tabla = sqlServer.consultar(consulta)
        registro = tabla.Rows(0)
        fechaEmision = registro("INVOICEDATE")
        salesId = registro("SALESID")
        voucherFactura = registro("LEDGERVOUCHER")
        codigoCliente = registro("INVOICEACCOUNT")
        recIdCustInvoiceJour = registro("RECID")
        totalSinImpuestos = Math.Abs(registro("SALESBALANCE") - registro("ENDDISC"))
        If registro("SALESBALANCE") <> 0 Then
            porcentajeDescuentoCabecera = registro("ENDDISC") / registro("SALESBALANCE")
            descuentoPie = registro("ENDDISC")
        Else
            porcentajeDescuentoCabecera = 0
        End If

        totalDescuento = Math.Abs(registro("SUMLINEDISC") + registro("ENDDISC"))

        'Guía de remisión
        consulta = ""
        consulta = consulta & "SELECT NUM FROM LECSALEGUIAREMTABLE WHERE DATAAREAID='REM' AND SALESID='" & salesId & "' AND DELIVERYINITDATE='" & Format(fechaEmision, "dd/MM/yyyy") & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            numeroGuiaRemision = registro("NUM")
            'GR_006001000029230
            '012345678901234567
            Try
                numeroGuiaRemision = numeroGuiaRemision.Substring(3, 15)
            Catch ex As Exception
                numeroGuiaRemision = ""
            End Try
        Else
            numeroGuiaRemision = ""
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
                telefonoCliente = registro("PHONE")
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
        consulta = consulta & "A.VOUCHER='" & voucherFactura & "'"
        tabla = sqlServer.consultar(consulta)
        For Each registro In tabla.Rows
            Try
                If registro("TAXVALUE") = 12 Then
                    baseImponible12 -= registro("SOURCEBASEAMOUNTCUR")
                    iva12 -= registro("SOURCETAXAMOUNTCUR")
                ElseIf registro("TAXVALUE") = 14 Then
                    baseImponible14 -= registro("SOURCEBASEAMOUNTCUR")
                    iva14 -= registro("SOURCETAXAMOUNTCUR")
                ElseIf registro("TAXVALUE") = 0 Then
                    baseImponible0 -= registro("SOURCEBASEAMOUNTCUR")
                End If
            Catch ex As Exception

            End Try
        Next

        'SalesTable
        consulta = "SELECT * FROM SALESTABLE WHERE DATAAREAID='REM' AND SALESID='" & salesId & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            observaciones = tabla.Rows(0)("CUSTOMERREF")
            numeroPedido = tabla.Rows(0)("PURCHORDERFORMNUM")
            direccionCliente = tabla.Rows(0)("DELIVERYADDRESS")
            If tabla.Rows(0)("COMISIONISTAEXCLUSIVO").ToString.Length > 0 Then
                vendedor = tabla.Rows(0)("COMISIONISTAEXCLUSIVO")
            Else
                vendedor = tabla.Rows(0)("DLVTERM")
            End If
        Else
            observaciones = "SIN ORDEN DE VENTA"
            numeroPedido = ""
            direccionCliente = ""
            vendedor = ""
        End If

        'Cambiar la dirección de entrega por la dirección principal
        consulta = "SELECT ADDRESS FROM FSR_LIBRETA_DIRECCIONES_V4 WHERE ACCOUNTNUM='" & codigoCliente & "' AND ISPRIMARY=1"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            Try
                direccionCliente = registro("ADDRESS")
            Catch ex As Exception

            End Try
        Else

        End If

        'Vencimientos
        consulta = "SELECT * FROM CUSTPAYMSCHED WHERE DATAAREAID='REM' AND EXTTABLEID=62 AND EXTRECID=" & recIdCustInvoiceJour.ToString
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            recIdCustPaymSched = tabla.Rows(0)("RECID")
            consulta = "SELECT * FROM CUSTPAYMSCHEDLINE WHERE DATAAREAID='REM' AND PARENTRECID=" & recIdCustPaymSched.ToString
            tabla = sqlServer.consultar(consulta)
            i = 1
            For Each registro In tabla.Rows
                fechaVencimiento = registro("DUEDATE")
                valorVencimiento = registro("DUEAMOUNT")
                If i = 1 Then
                    vencimiento1 = Format(fechaVencimiento, "dd/MM/yyyy") & "   " & FormatNumber(valorVencimiento, 2)
                ElseIf i = 2 Then
                    vencimiento2 = Format(fechaVencimiento, "dd/MM/yyyy") & "   " & FormatNumber(valorVencimiento, 2)
                ElseIf i = 3 Then
                    vencimiento3 = Format(fechaVencimiento, "dd/MM/yyyy") & "   " & FormatNumber(valorVencimiento, 2)
                ElseIf i = 4 Then
                    vencimiento4 = Format(fechaVencimiento, "dd/MM/yyyy") & "   " & FormatNumber(valorVencimiento, 2)
                End If
                i += 1
            Next
        End If

        'Registro en página web
        consulta = "SELECT * FROM REMECO.DBO.CONTRIBUYENTE WHERE RUC='" & identificacionTercero & "' AND ESTADO='R'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registradoPaginaWeb = True
        Else
            registradoPaginaWeb = False
        End If

        'Cambiar la dirección de entrega por la dirección principal
        consulta = "SELECT RMSCODIGO FROM SALESDELIVERYDATA WHERE INVOICENUMBER='" & numeroComprobanteDynamics & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            Try
                Dim codigoRemision As Long
                codigoRemision = registro("RMSCODIGO")
                codigoSeguimiento = "89253" & codigoRemision.ToString("D7")
            Catch ex As Exception
                codigoSeguimiento = "N/A"
            End Try
        Else
            codigoSeguimiento = "N/A"
        End If
    End Sub
End Class
