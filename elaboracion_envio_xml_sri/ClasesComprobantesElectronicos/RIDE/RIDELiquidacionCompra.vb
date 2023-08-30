Public Class RIDELiquidacionCompra
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

	Public direccionProveedor As String
	Public codigoProveedor As String
	Public vencimientos As String
	Public baseImponible12 As Double = 0
	Public baseImponible14 As Double = 0
	Public baseImponible0 As Double = 0
	Public porcentajeDescuentoPie As Double
	Public iva12 As Double = 0
	Public iva14 As Double = 0
	Public iva0 As Double = 0
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
		MyBase.New(configuracion, numeroComprobanteDynamics, TIPO_DE_COMPROBANTE_LIQUIDACION_COMPRA, tipoEmision, tipoAmbiente)
		Me.obtenerDocumento()
		Me.inicializarDocumento()
		Me.generar()
	End Sub

	Public Overrides Sub generar()
		Dim totalVentasBienes As Double = 0

		textoNegrita("PROVEEDOR:", fila, 1)
		textoNormal(razonSocialTercero, fila, 6)
		textoNegrita("R.U.C.:", fila, 26)
		textoNormal(identificacionTercero, fila, 29)
		siguienteFila()

		Try
			direccionProveedor = direccionProveedor.Substring(0, 90)
		Catch ex As Exception

		End Try

		textoNegrita("DIRECCIÓN:", fila, 1)
		textoNormal(direccionProveedor, fila, 5)
		textoNegrita("CÓDIGO:", fila, 26)
		textoNormal(codigoProveedor, fila, 29)
		siguienteFila()
		siguienteFila()

		'Detalles
		textoNegrita("CÓDIGO", fila, 1)
		textoNegrita("DESCRIPCIÓN", fila, 7)
		textoNegrita("CANTIDAD", fila, 25)
		textoNegrita("P. UNITARIO", fila, 31)
		textoNegrita("DSCTO.", fila, 36)
		textoNegrita("TOTAL", fila, 42)
		siguienteFila()

		Dim consulta As String
		Dim tabla As DataTable
		Dim detalle As ClaseValorDetalleFactura
		Dim voucher As String

		consulta = "SELECT LEDGERVOUCHER FROM VENDINVOICEJOUR WHERE DATAAREAID='REM' AND INVOICEID='" & numeroComprobanteDynamics.Substring(3) & "' AND LEDGERVOUCHER LIKE 'CPLQC%'"
		tabla = sqlServer.consultar(consulta)
		If tabla.Rows.Count > 0 Then
			voucher = tabla.Rows(0)("LEDGERVOUCHER")
		Else
			voucher = ""
		End If

		consulta = "SELECT ACCOUNTTYPE,ACCOUNTNUM,TXT,AMOUNTCURDEBIT,AMOUNTCURCREDIT,OFFSETACCOUNT FROM LEDGERJOURNALTRANS WHERE DATAAREAID='REM' AND VOUCHER='" & voucher & "'"
		tabla = sqlServer.consultar(consulta)
		For Each registro As DataRow In tabla.Rows
			Dim accountType As Integer = registro("ACCOUNTTYPE")
			Dim accountNum As String = registro("ACCOUNTNUM").ToString.Trim
			Dim txt As String = registro("TXT").ToString.Trim
			If txt = "" Then
				txt = "NO ESPECIFICADO"
			End If
			Dim offsetAccount As String = registro("OFFSETACCOUNT").ToString.Trim
			Dim amountCurDebit As Double = registro("AMOUNTCURDEBIT")
			Dim amountCurCredit As Double = registro("AMOUNTCURCREDIT")
			Dim linea As LineaFactura
			Select Case accountType
				Case 2 'Proveedor
					If Not offsetAccount = "" Then 'Una sola línea
						linea = crearDetalle(offsetAccount, txt, amountCurCredit)
						desplegarLinea(linea)
					End If
				Case 0 'Cuenta contable (diferentes líneas)
					linea = crearDetalle(accountNum, txt, amountCurDebit)
					desplegarLinea(linea)
			End Select
		Next

		siguienteFila()
		textoNormal("Valor:", fila, 33)
		textoNumeros(FormatNumber(totalSinImpuestos, 2), 10, fila, 40)
		siguienteFila()

		textoNormal("Descuento " & FormatNumber(porcentajeDescuentoCabecera * 100, 1) & "%:", fila, 33)
		textoNumeros(FormatNumber(0, 2), 10, fila, 40)
		siguienteFila()

		textoNormal("Subtotal:", fila, 33)
		textoNumeros(FormatNumber(baseImponible0 + baseImponible12 + baseImponible14, 2), 10, fila, 40)
		siguienteFila()

		textoNormal("Tarifa 0%:", fila, 33)
		textoNumeros(FormatNumber(baseImponible0, 2), 10, fila, 40)
		siguienteFila()

		Dim porcentajeIva As Double
		If Math.Abs(baseImponible12) + Math.Abs(baseImponible14) > 0 Then
			porcentajeIva = 100 * (iva12 + iva14) / (baseImponible12 + baseImponible14)
		Else
			porcentajeIva = 12
		End If

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

		textoDetalle("OTROS CON UTILIZACION DEL SISTEMA FINANCIERO", fila, 1)
		textoNumeros(FormatNumber(totalSinImpuestos + iva12 + iva14, 2), 10, fila, 16)
		textoNumeros(FormatNumber(0, 0), 7, fila, 21)
		textoDetalle("Días", fila, 26)
		siguienteFila()

		dibujarLogotipoFinDocumento()
		guardar()
	End Sub

	Sub desplegarLinea(detalle As LineaFactura)
		textoDetalle(detalle.codigo, fila, 1)
		textoDetalle(detalle.descripcion, fila, 7)
		textoNumeros(FormatNumber(detalle.cantidad, 0), 7, fila, 24)
		textoDetalle(detalle.unidad, fila, 27)
		textoNumeros(FormatNumber(detalle.precioUnitario, 2), 10, fila, 30)
		textoNumeros(FormatNumber(detalle.descuento, 2), 6, fila, 36)
		textoNumeros(FormatNumber(detalle.valorTotal, 2), 10, fila, 40)
		siguienteFila()
		If detalle.serie.Length > 0 Then
			textoDetalle("S/N: " & detalle.serie, fila, 7)
			siguienteFila()
		End If
	End Sub

	Private Function crearDetalle(xCodigo As String, xDescripcion As String, xPrecio As Double) As LineaFactura
		Dim d As LineaFactura
		d = New LineaFactura
		d.codigo = xCodigo
		d.descripcion = xDescripcion
		d.cantidad = 1
		d.precioUnitario = xPrecio
		d.descuento = 0
		d.valorTotal = xPrecio
		d.unidad = ""
		d.esServicio = True
		d.serie = ""
		Return d
	End Function

	Public Overrides Sub obtenerDocumento()
		Dim tabla As DataTable
		Dim consulta As String = ""
		Dim registro As DataRow
		Dim recIdCustInvoiceJour As Long

		'Datos factura
		consulta = consulta & "SELECT * FROM VENDINVOICEJOUR A  WHERE "
		consulta = consulta & "A.DATAAREAID='REM' AND "
		consulta = consulta & "LEDGERVOUCHER LIKE 'CPLQC%' AND "
		consulta = consulta & "A.INVOICEID='" & numeroComprobanteDynamics.Substring(3) & "'"

		tabla = sqlServer.consultar(consulta)
		registro = tabla.Rows(0)
		fechaEmision = registro("INVOICEDATE")
		codigoProveedor = registro("INVOICEACCOUNT")
		totalSinImpuestos = Math.Abs(registro("SALESBALANCE") - registro("ENDDISC"))
		totalDescuento = Math.Abs(registro("SUMLINEDISC") + registro("ENDDISC"))

		voucherFactura = registro("LEDGERVOUCHER")
		recIdCustInvoiceJour = registro("RECID")

		'Datos fiscales proveedor
		consulta = ""
		consulta = consulta & "SELECT * FROM VENDDATATABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoProveedor & "'"
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

		'Datos proveedor
		consulta = ""
		consulta = consulta & "SELECT * FROM VENDTABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoProveedor & "'"
		tabla = sqlServer.consultar(consulta)
		If tabla.Rows.Count > 0 Then
			registro = tabla.Rows(0)
			Try
				direccionProveedor = registro("ADDRESS")
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
					baseImponible12 += registro("SOURCEBASEAMOUNTCUR")
					iva12 += registro("SOURCETAXAMOUNTCUR")
				ElseIf registro("TAXVALUE") = 14 Then
					baseImponible14 += registro("SOURCEBASEAMOUNTCUR")
					iva14 += registro("SOURCETAXAMOUNTCUR")
				ElseIf registro("TAXVALUE") = 0 Then
					baseImponible0 += registro("SOURCEBASEAMOUNTCUR")
					iva0 += registro("SOURCETAXAMOUNTCUR")
				End If
			Catch ex As Exception

			End Try
		Next
	End Sub
End Class
