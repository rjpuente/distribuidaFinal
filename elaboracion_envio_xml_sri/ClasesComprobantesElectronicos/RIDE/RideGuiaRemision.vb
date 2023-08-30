Public Class RideGuiaRemision
    Inherits RIDEGenerico

    Public Class LineaGuiaRemision
        Public codigo As String
        Public descripcion As String
        Public cantidad As Long
        Public unidad As String
        Public serie As String
    End Class

    Public codigoCliente As String
    Public codigoTransportista As String
    Public direccionEntrega As String
    Public fechaFactura As Date
    Public fechaFinTraslado As Date
    Public fechaInicioTraslado As Date
    Public motivoTraslado As String
    Public numeroFactura As String
    Public placaVehiculo As String
    Public razonSocialTransportista As String
    Public rucTransportista As String
    Public salesId As String
    Public tipoIdentificacionTransportista As String
    Public ciudadEntrega As String

    Public Sub New(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, numeroComprobanteDynamics As String, tipoEmision As Integer, tipoAmbiente As Integer)
        MyBase.New(configuracion, numeroComprobanteDynamics, TIPO_DE_COMPROBANTE_GUIA_DE_REMISION, tipoEmision, tipoAmbiente)
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

        textoNegrita("TELÉFONO:", fila, 1)
        textoNormal(telefonoTercero, fila, 6)
        textoNegrita("FACTURA:", fila, 26)
        textoNormal(numeroFactura, fila, 30)
        siguienteFila()
        siguienteFila()

        textoNegrita("FECHA INICIO TRASLADO:", fila, 1)
        textoNormal(Format(fechaInicioTraslado, "dd/MM/yyyy"), fila, 10)
        textoNegrita("FECHA FIN TRASLADO:", fila, 14)
        textoNormal(Format(fechaFinTraslado, "dd/MM/yyyy"), fila, 22)
        textoNegrita("MOTIVO DE TRASLADO:", fila, 26)
        textoNormal(motivoTraslado, fila, 34)
        siguienteFila()

        textoNegrita("PUNTO DE PARTIDA:", fila, 1)
        textoNormal("Quito - " & direccionEstablecimiento, fila, 9)
        siguienteFila()

        textoNegrita("PUNTO DE LLEGADA:", fila, 1)
        If direccionEntrega.Length > 70 Then direccionEntrega = direccionEntrega.Substring(0, 70)
        textoNormal(ciudadEntrega & " - " & direccionEntrega, fila, 9)
        siguienteFila()
        siguienteFila()

        textoNegrita("TRANSPORTISTA:", fila, 1)
        textoNormal(razonSocialTransportista, fila, 8)
        textoNegrita("R.U.C.:", fila, 20)
        textoNormal(rucTransportista, fila, 23)
        textoNegrita("PLACA:", fila, 29)
        textoNormal(placaVehiculo, fila, 32)
        siguienteFila()
        siguienteFila()

        'Detalles
        textoNegrita("CÓDIGO", fila, 1)
        textoNegrita("DESCRIPCIÓN", fila, 7)
        textoNegrita("CANTIDAD", fila, 28)
        textoNegrita("UNIDAD", fila, 32)
        siguienteFila()

        Dim consulta As String = "SELECT * FROM CUSTPACKINGSLIPTRANS WHERE DATAAREAID='REM' AND PACKINGSLIPID='" & numeroComprobanteDynamics & "'"
        Dim tabla As DataTable = sqlServer.consultar(consulta)

        For Each registro As DataRow In tabla.Rows

            Dim detalle As New LineaGuiaRemision
            detalle.codigo = registro("ITEMID")
            detalle.descripcion = registro("NAME")
            detalle.cantidad = Math.Abs(registro("QTY"))
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
            textoNumeros(FormatNumber(detalle.cantidad, 0), 7, fila, 27)
            textoDetalle(detalle.unidad, fila, 32)
            siguienteFila()
            If detalle.serie.Length > 0 Then
                textoDetalle("S/N: " & detalle.serie, fila, 7)
                siguienteFila()
            End If
        Next

        dibujarLogotipoFinDocumento()
        guardar()
    End Sub

    Public Overrides Sub obtenerDocumento()
        Dim consulta As String
        Dim registro As DataRow
        Dim tabla As DataTable

        consulta = "SELECT SALESID,NUM,CARRIERID,DELIVERYINITDATE,DELIVERYENDDATE,VEHICLEPLATE,CUSTID,DELIVERYENDPLACE,DELIVERYMOTIVE,DUIFUE,SALEDOCID FROM LECSALEGUIAREMTABLE WHERE DATAAREAID='REM' AND NUM='" & numeroComprobanteDynamics & "'"
        tabla = sqlServer.consultar(consulta)
        If tabla.Rows.Count > 0 Then
            registro = tabla.Rows(0)
            codigoTransportista = registro("CARRIERID")
            salesId = registro("SALESID")
            codigoCliente = registro("CUSTID")
            fechaInicioTraslado = registro("DELIVERYINITDATE")
            fechaFinTraslado = registro("DELIVERYENDDATE")
            placaVehiculo = registro("VEHICLEPLATE")
            direccionEntrega = registro("DELIVERYENDPLACE")
            motivoTraslado = "VENTA - " & registro("DELIVERYMOTIVE")
            fechaEmision = fechaInicioTraslado

            consulta = "SELECT SOCIALNAME,SRIIDTYPE,IDENTIFICATIONNUMBER FROM VENDDATATABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoTransportista & "'"
            tabla = sqlServer.consultar(consulta)
            If tabla.Rows.Count > 0 Then
                registro = tabla.Rows(0)
                razonSocialTransportista = registro("SOCIALNAME")
                rucTransportista = registro("IDENTIFICATIONNUMBER")
                tipoIdentificacionTransportista = convertirTipoIdentificacionDynamics(registro("SRIIDTYPE"))
            End If

            consulta = "SELECT INVOICEID,INVOICEDATE,DLVCOUNTY FROM CUSTINVOICEJOUR WHERE DATAAREAID='REM' AND INVOICEDATE<='" & Format(fechaInicioTraslado, "dd/MM/yyyy") & "' AND SALESID='" & salesId & "' ORDER BY INVOICEDATE DESC"
            tabla = sqlServer.consultar(consulta)
            If tabla.Rows.Count > 0 Then
                registro = tabla.Rows(0)
                numeroFactura = registro("INVOICEID")
                fechaFactura = registro("INVOICEDATE")
                ciudadEntrega = registro("DLVCOUNTY")
            Else
                numeroFactura = ""
            End If

            consulta = "SELECT NAME FROM ADDRESSCOUNTY WHERE DATAAREAID='REM' AND COUNTRYREGIONID='ECU' AND COUNTYID='" & ciudadEntrega & "'"
            tabla = sqlServer.consultar(consulta)
            If tabla.Rows.Count > 0 Then
                registro = tabla.Rows(0)
                ciudadEntrega = registro("NAME")
            End If

            consulta = "SELECT IDENTIFICATIONNUMBER,SOCIALNAME FROM CUSTDATATABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoCliente & "'"
            tabla = sqlServer.consultar(consulta)
            If tabla.Rows.Count > 0 Then
                registro = tabla.Rows(0)
                identificacionTercero = registro("IDENTIFICATIONNUMBER")
                razonSocialTercero = registro("SOCIALNAME")
            End If

            consulta = "SELECT PHONE FROM CUSTTABLE WHERE DATAAREAID='REM' AND ACCOUNTNUM='" & codigoCliente & "'"
            tabla = sqlServer.consultar(consulta)
            If tabla.Rows.Count > 0 Then
                registro = tabla.Rows(0)
                telefonoTercero = registro("PHONE")
            End If
        End If
    End Sub
End Class
