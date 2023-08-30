Public Class RideRetencion
    Inherits RIDEGenerico

    Public Class LineaRetencion
        Public factura As String
        Public baseImponible As String
        Public codigo As String
        Public impuesto As String
        Public porcentaje As Double
        Public precioUnitario As Double
        Public valorRetenido As Double
        Public taxCode As String
    End Class

    Public numeroComprobanteRetenido As String
    Public releaseDate As Date
    Public tipoComprobanteRetenido As String
    Public concepto As String = ""

    Public Sub New(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, numeroComprobanteDynamics As String, tipoEmision As Integer, tipoAmbiente As Integer)
        MyBase.New(configuracion, numeroComprobanteDynamics, TIPO_DE_COMPROBANTE_COMPROBANTE_DE_RETENCION, tipoEmision, tipoAmbiente)
        Me.obtenerDocumento()
        Me.inicializarDocumento()
        Me.generar()
    End Sub

    Public Overrides Sub generar()
        Dim totalRetenido As Double = 0

        textoNegrita("CONTRIBUYENTE:", fila, 1)
        textoNormal(razonSocialTercero, fila, 8)
        textoNegrita("R.U.C.:", fila, 26)
        textoNormal(identificacionTercero, fila, 29)
        siguienteFila()

        textoNegrita("CONCEPTO:", fila, 1)
        textoNormal(concepto, fila, 7)
        siguienteFila()
        siguienteFila()

        'Detalles
        textoNegrita("COMPROBANTE", fila, 1)
        textoNegrita("BASE IMPONIBLE", fila, 7)
        textoNegrita("CODIGO", fila, 15)
        textoNegrita("IMPUESTO", fila, 23)
        textoNegrita("% RETENIDO", fila, 31)
        textoNegrita("VALOR RETENIDO", fila, 39)
        siguienteFila()

        Dim consulta As String = "SELECT LECRETTYPEENUM,RETPERCENT,RETTAXCODE,TAXBASE,RETVALUE,TAXCODE FROM LECRETDETAILTABLE WHERE RETNUM='" & numeroComprobanteDynamics & "'"
        Dim tabla As DataTable = sqlServer.consultar(consulta)

        For Each registro As DataRow In tabla.Rows
            Dim detalle As New LineaRetencion
            detalle.baseImponible = registro("TAXBASE")
            detalle.factura = numeroComprobanteRetenido
            detalle.porcentaje = registro("RETPERCENT")
            detalle.valorRetenido = registro("RETVALUE")
            detalle.codigo = registro("RETTAXCODE")
            detalle.taxCode = registro("TAXCODE")
            If registro("LECRETTYPEENUM") = 1 Then
                'IVA
                detalle.impuesto = "RETENCIÓN IVA"
            Else
                'RENTA
                detalle.impuesto = "RETENCIÓN IMPUESTO RENTA"
            End If

            'Si el porcentaje usado es '275' cambiar a '2.75'
            If detalle.porcentaje.Equals("275") Then
                detalle.porcentaje = "2.75"
            End If

            If detalle.porcentaje = 0D Then
                'Determinar el porcentaje
                'Tratar de conseguir el porcentaje por otros medios
                consulta = "SELECT ABS(TAXVALUE) VALOR FROM TAXDATA WHERE DATAAREAID='REM' AND TAXCODE='" & detalle.taxCode & "' ORDER BY RECID DESC"
                Dim tablaTaxData As DataTable = sqlServer.consultar(consulta)
                If tablaTaxData.Rows.Count > 0 Then
                    detalle.porcentaje = tablaTaxData.Rows(0)("VALOR")
                Else
                    detalle.porcentaje = 0
                End If
            End If

            textoDetalle(detalle.factura, fila, 1)
            textoNumeros(FormatNumber(detalle.baseImponible, 2), 10, fila, 7)
            textoDetalle(detalle.codigo, fila, 16)
            textoDetalle(detalle.impuesto, fila, 21)
            textoNumeros(FormatNumber(detalle.porcentaje, 2), 3, fila, 33)
            textoNumeros(FormatNumber(detalle.valorRetenido, 2), 10, fila, 40)
            siguienteFila()

            totalRetenido += detalle.valorRetenido
        Next

        siguienteFila()
        textoNegrita("Total retenido:", fila, 33)
        textoNumerosNegrita(FormatNumber(totalRetenido, 2), 10, fila, 40)
        siguienteFila()

        dibujarLogotipoFinDocumento()
        guardar()
    End Sub

    Function aproximarPorcentaje(p As Double) As Double
        Dim dMinima As Double = 9999999D
        Dim pSeleccionado As Double = 999999D

        compararDiferencia(pSeleccionado, p, 0D, dMinima)
        compararDiferencia(pSeleccionado, p, 1D, dMinima)
        compararDiferencia(pSeleccionado, p, 1.5D, dMinima)
        compararDiferencia(pSeleccionado, p, 1.75D, dMinima)
        compararDiferencia(pSeleccionado, p, 2D, dMinima)
        compararDiferencia(pSeleccionado, p, 2.75D, dMinima)
        compararDiferencia(pSeleccionado, p, 3D, dMinima)
        compararDiferencia(pSeleccionado, p, 8D, dMinima)
        compararDiferencia(pSeleccionado, p, 10D, dMinima)
        compararDiferencia(pSeleccionado, p, 15D, dMinima)
        compararDiferencia(pSeleccionado, p, 20D, dMinima)
        compararDiferencia(pSeleccionado, p, 25D, dMinima)
        compararDiferencia(pSeleccionado, p, 30D, dMinima)
        compararDiferencia(pSeleccionado, p, 35D, dMinima)
        compararDiferencia(pSeleccionado, p, 40D, dMinima)
        compararDiferencia(pSeleccionado, p, 45D, dMinima)
        compararDiferencia(pSeleccionado, p, 50D, dMinima)
        compararDiferencia(pSeleccionado, p, 55D, dMinima)
        compararDiferencia(pSeleccionado, p, 60D, dMinima)
        compararDiferencia(pSeleccionado, p, 65D, dMinima)
        compararDiferencia(pSeleccionado, p, 70D, dMinima)
        compararDiferencia(pSeleccionado, p, 75D, dMinima)
        compararDiferencia(pSeleccionado, p, 80D, dMinima)
        compararDiferencia(pSeleccionado, p, 85D, dMinima)
        compararDiferencia(pSeleccionado, p, 90D, dMinima)
        compararDiferencia(pSeleccionado, p, 95D, dMinima)
        compararDiferencia(pSeleccionado, p, 100D, dMinima)
        Return pSeleccionado
    End Function

    Sub compararDiferencia(ByRef pSeleccionado As Double, p As Double, pComparado As Double, ByRef dMinima As Double)
        Dim dComparada As Double = obtenerDiferencia(p, pComparado)
        If dComparada < dMinima Then
            dMinima = dComparada
            pSeleccionado = pComparado
        End If
    End Sub

    Function obtenerDiferencia(p1 As Double, p2 As Double) As Double
        Return Math.Abs(p1 - p2)
    End Function

    Public Overrides Sub obtenerDocumento()
        Dim tabla As DataTable
        Dim registro As DataRow
        Dim consulta As String
        Dim codigoProveedor As String
        Dim recIdLJT As Long

        consulta = "SELECT ACCOUNTNUM,RETPERSONID RUC,VOUCHERTYPECODE,RELEASEDATE,TRANSDATE,INVNUM,LEDGERJOURTRANSRECID FROM LECRETHEADERTABLE WHERE DATAAREAID='REM' AND RETNUM='" & numeroComprobanteDynamics & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            codigoProveedor = registro("ACCOUNTNUM")
            identificacionTercero = registro("RUC")
            tipoComprobanteRetenido = registro("VOUCHERTYPECODE")
            fechaEmision = registro("TRANSDATE")
            numeroComprobanteRetenido = registro("INVNUM")
            releaseDate = registro("RELEASEDATE")
            recIdLJT = registro("LEDGERJOURTRANSRECID")

            consulta = "SELECT SRIIDTYPE,SOCIALNAME FROM VENDDATATABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoProveedor & "'"
            tabla = sqlServer.consultar(consulta)
            If tabla.Rows.Count > 0 Then
                registro = tabla.Rows(0)
                tipoIdentificacionTercero = "01"
                razonSocialTercero = registro("SOCIALNAME")
            End If

            consulta = "SELECT TXT FROM LEDGERJOURNALTRANS WHERE DATAAREAID='REM' AND RECID=" & recIdLJT.ToString
            tabla = sqlServer.consultar(consulta)
            If tabla.Rows.Count > 0 Then
                registro = tabla.Rows(0)
                concepto = registro("TXT")
            End If
        End If
    End Sub
End Class
