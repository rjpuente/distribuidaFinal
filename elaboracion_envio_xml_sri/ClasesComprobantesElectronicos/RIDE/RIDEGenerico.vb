Imports PdfSharp.Pdf
Imports PdfSharp.Drawing
Imports PdfSharp

Public MustInherit Class RIDEGenerico
    Private configuracion As ClasesComprobantesElectronicos.ConfiguracionActual
    Private lecCompFisInfTable As DataTable
    Public documento As PdfDocument
    Public paginaActual As PdfPage
    Public numeroComprobanteDynamics As String
    Public gfx As XGraphics
    Public fila As Integer

    'Contenido RIDE Genérico
    Public ruc As String
    Public numeroAutorizacion As String
    Public fechaHoraAutorizacion As Date
    Public emision As String
    Public claveAcceso As String
    Public direccionMatriz As String
    Public direccionEstablecimiento As String
    Public obligadoLlevarContabilidad As String
    Public identificacionTercero As String
    Public razonSocialTercero As String
    Public fechaEmision As Date
    Public tipoIdentificacionTercero As String
    Public tipoEmision As Integer
    Public tipoAmbiente As Integer
    Public establecimiento As String
    Public nombreComercial As String
    Public puntoEmision As String
    Public razonSocial As String
    Public secuencial As Long
    Public tipoComprobante As String
    Public numeroGuiaRemision As String = ""
    Public telefonoTercero As String

    Private fuenteEstandarNormal As XFont
    Private fuenteDetalle As XFont
    Private fuenteEstandarNegrita As XFont
    Private fuenteCodigoBarras As XFont
    Private fuenteNumeros As XFont
    Private fuenteNumerosNegrita As XFont

    Public MustOverride Sub obtenerDocumento()
    Public MustOverride Sub generar()

	Public Sub New(ByVal configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, ByVal numeroComprobanteDynamics As String, ByVal tipoComprobante As String, ByVal tipoEmision As Integer, ByVal tipoAmbiente As Integer)
		' Opciones para fuentes embebidas
		Dim opcionesFuentes As New XPdfFontOptions(PdfFontEncoding.Unicode)

		fuenteEstandarNormal = New XFont(configuracion.tipoFuenteEstandar, configuracion.tamanoFuenteEstandar, XFontStyle.Regular, opcionesFuentes)
		fuenteEstandarNegrita = New XFont(configuracion.tipoFuenteEstandar, configuracion.tamanoFuenteEstandar, XFontStyle.Bold, opcionesFuentes)
		fuenteDetalle = New XFont(configuracion.tipoFuenteDetalle, configuracion.tamanoFuenteDetalle, XFontStyle.Regular, opcionesFuentes)
		fuenteCodigoBarras = New XFont(configuracion.tipoFuenteCodigoBarras, configuracion.tamanoFuenteCodigoBarras, XFontStyle.Regular, opcionesFuentes)
		fuenteNumeros = New XFont(configuracion.tipoFuenteNumeros, configuracion.tamanoFuenteNumeros, XFontStyle.Regular, opcionesFuentes)
		fuenteNumerosNegrita = New XFont(configuracion.tipoFuenteNumeros, configuracion.tamanoFuenteNumeros, XFontStyle.Bold, opcionesFuentes)

		Me.configuracion = configuracion
		Me.numeroComprobanteDynamics = numeroComprobanteDynamics
		Me.tipoComprobante = tipoComprobante
		Me.tipoAmbiente = tipoAmbiente
		Me.tipoEmision = tipoEmision
		desglosarNumeroComprobante()
		obtenerInformacionFiscal()
		obtenerDireccionMatriz()
		obtenerDatosEstablecimiento()
		obtenerDatosAutorizacion()
	End Sub

	Sub inicializarDocumento()
        documento = New PdfDocument
        documento.Info.Author = "Representaciones Metalmecánicas C.A."
        documento.Info.Keywords = "Comprobante electrónico"
        agregarPagina()
        cabeceraEstandar()
    End Sub

    Sub cabeceraEstandar()
        Dim cadena As String

        textoNegrita("RAZÓN SOCIAL:", fila, 1)
        textoNormal(Me.razonSocial, fila, 7)
        siguienteFila()

        textoNegrita("R.U.C.:", fila, 1)
        textoNormal(Me.ruc, fila, 4)
        textoNegrita(tipoComprobanteCadena() & " " & Me.establecimiento & "-" & Me.puntoEmision & "-" & cerosIzquierda(Me.secuencial, 9), fila, 26)
        siguienteFila()

        textoNegrita("DIRECCIÓN MATRIZ:", fila, 1)
        textoNormal(Me.direccionMatriz, fila, 8)
        textoNegrita("FECHA DE EMISIÓN:", fila, 26)
        textoNormal(Format(Me.fechaEmision, "dd/MM/yyyy"), fila, 33)
        siguienteFila()

        textoNegrita("CONTRIBUYENTE ESPECIAL:", fila, 1)
        textoNormal(configuracion.codigoContribuyenteEspecial, fila, 10)
        textoNegrita("OBLIGADO CONTABILIDAD:", fila, 26)
        textoNormal("SÍ", fila, 35)
        siguienteFila()

        textoNegrita("AMBIENTE:", fila, 1)
        If tipoAmbiente = TIPO_DE_AMBIENTE_PRODUCCION Then
            cadena = "PRODUCCION"
        Else
            cadena = "PRUEBAS"
        End If
        textoNormal(cadena, fila, 5)
        textoNegrita("EMISIÓN:", fila, 26)
        If tipoEmision = TIPO_DE_EMISION_NORMAL Then
            cadena = "NORMAL"
        Else
            cadena = "CONTINGENCIA"
        End If
        textoNormal(cadena, fila, 30)
        siguienteFila()

        textoNegrita("FECHA Y HORA DE AUTORIZACIÓN:", fila, 1)
        textoNormal(Format(fechaHoraAutorizacion, "dd/MM/yyyy HH:mm:ss"), fila, 12)
        If numeroGuiaRemision.Length > 0 Then
            textoNegrita("GUÍA DE REMISIÓN:", fila, 26)
            textoNormal(numeroGuiaRemision.Substring(0, 3) & "-" & numeroGuiaRemision.Substring(3, 3) & "-" & numeroGuiaRemision.Substring(6, 9), fila, 33)
        End If
        siguienteFila()

        textoNegrita("NÚMERO DE AUTORIZACIÓN:", fila, 1)
        textoNormal(numeroAutorizacion, fila, 11)
        siguienteFila()

        textoNegrita("CLAVE DE ACCESO:", fila, 1)
        textoNormal(claveAcceso, fila, 8)
        siguienteFila()
        siguienteFila()

        textoCodigoBarras("*" & claveAcceso & "*", fila, 8)
        siguienteFila()
        siguienteFila()
    End Sub

    Sub siguienteFila()
        fila += 1
        If fila > configuracion.numeroMaximoLineasRide Then
            agregarPagina()
        End If
    End Sub

    Sub textoNormal(texto As String, valorFila As Integer, valorColumna As Integer)
        gfx.DrawString(texto, fuenteEstandarNormal, XBrushes.Black, coordenadaColumna(valorColumna), coordenadaFila(valorFila))
    End Sub

    Sub textoDetalle(texto As String, valorFila As Integer, valorColumna As Integer)
        gfx.DrawString(texto, fuenteDetalle, XBrushes.Black, coordenadaColumna(valorColumna), coordenadaFila(valorFila))
    End Sub

    Sub textoNegrita(texto As String, valorFila As Integer, valorColumna As Integer)
        gfx.DrawString(texto, fuenteEstandarNegrita, XBrushes.Black, coordenadaColumna(valorColumna), coordenadaFila(valorFila))
    End Sub

    Sub textoCodigoBarras(texto As String, valorFila As Integer, valorColumna As Integer)
        gfx.DrawString(texto, fuenteCodigoBarras, XBrushes.Black, coordenadaColumna(valorColumna), coordenadaFila(valorFila))
    End Sub

    Sub textoNumeros(texto As String, longitud As Integer, valorFila As Integer, valorColumna As Integer)
        Dim cadena As String = ""
        If texto.Length < longitud Then
            For i = 1 To longitud - texto.Length
                cadena = cadena & " "
            Next
        End If
        cadena = cadena & texto
        gfx.DrawString(cadena, fuenteNumeros, XBrushes.Black, coordenadaColumna(valorColumna), coordenadaFila(valorFila))
    End Sub

    Sub textoNumerosNegrita(texto As String, longitud As Integer, valorFila As Integer, valorColumna As Integer)
        Dim cadena As String = ""
        If texto.Length < longitud Then
            For i = 1 To longitud - texto.Length
                cadena = cadena & " "
            Next
        End If
        cadena = cadena & texto
        gfx.DrawString(cadena, fuenteNumerosNegrita, XBrushes.Black, coordenadaColumna(valorColumna), coordenadaFila(valorFila))
    End Sub

    Sub dibujarLogotipoInicioPagina()
        'Logotipo al inicio de la página
        Dim topLogo As XImage = XImage.FromFile(configuracion.ubicacionLogoRemeco)
        gfx = XGraphics.FromPdfPage(paginaActual)
        gfx.DrawImage(topLogo, coordenadaColumna(17), coordenadaFila(fila))
        siguienteFila()
        siguienteFila()
        siguienteFila()
        siguienteFila()
        siguienteFila()
        siguienteFila()
    End Sub

    Sub dibujarLogotipoFinDocumento()
        'Logotipo al final del documento
        Dim bottomLogo As XImage = XImage.FromFile(configuracion.ubicacionLogo1800Remeco)
        gfx.DrawImage(bottomLogo, coordenadaColumna(14), coordenadaFila(fila))
    End Sub

    Private Sub obtenerInformacionFiscal()
        Dim consulta As String = "SELECT * FROM LECCOMPFISINFTABLE WHERE DATAAREAID='REM'"
        Dim registro As DataRow

        lecCompFisInfTable = sqlServer.consultar(consulta)
        registro = lecCompFisInfTable.Rows(0)
        ruc = registro("RUC")
        razonSocial = registro("SOCIALNAME")
        nombreComercial = registro("COMMERCIALNAME")
    End Sub

    Private Sub obtenerDireccionMatriz()
        Dim consulta As String = "SELECT * FROM OPENINGTABLEELECTRONIC WHERE DATAAREAID='REM' AND OPENINGID='" & configuracion.establecimientoMatriz & "'"
        Dim registro As DataRow
        Dim tabla As DataTable

        tabla = sqlServer.consultar(consulta)
        registro = tabla.Rows(0)
        direccionMatriz = registro("ADDRESS")
    End Sub

    Private Sub obtenerDatosEstablecimiento()
        Dim consulta As String = "SELECT * FROM OPENINGTABLEELECTRONIC WHERE DATAAREAID='REM' AND OPENINGID='" & establecimiento & "'"
        Dim registro As DataRow
        Dim tabla As DataTable

        tabla = sqlServer.consultar(consulta)
        registro = tabla.Rows(0)
        direccionEstablecimiento = registro("ADDRESS")
    End Sub

    Private Sub obtenerDatosAutorizacion()
        Dim consulta As String = "SELECT * FROM REMECO.DBO.COMPROBANTE_ELECTRONICO WHERE numeroComprobante='" & numeroComprobanteDynamics & "'"
        Dim registro As DataRow
        Dim tabla As DataTable

        tabla = sqlServer.consultar(consulta)
        registro = tabla.Rows(0)
        claveAcceso = registro("claveAcceso")
        If registro("numeroAutorizacionSRI").ToString.Length = 0 Then
            fechaHoraAutorizacion = registro("fechaContingencia")
            numeroAutorizacion = registro("numeroAutorizacionContingencia")
        Else
            fechaHoraAutorizacion = registro("fechaAutorizacion")
            numeroAutorizacion = registro("numeroAutorizacionSRI")
        End If
    End Sub

    Public Sub desglosarNumeroComprobante()
		If tipoComprobante = TIPO_DE_COMPROBANTE_FACTURA Then
			'F_006001000029317
			'01234567890123456
			establecimiento = numeroComprobanteDynamics.Substring(2, 3)
			puntoEmision = numeroComprobanteDynamics.Substring(5, 3)
			secuencial = numeroComprobanteDynamics.Substring(8, 9)
		ElseIf tipoComprobante = TIPO_DE_COMPROBANTE_GUIA_DE_REMISION Then
			'GR_006001000029317
			'01234567890123456
			establecimiento = numeroComprobanteDynamics.Substring(3, 3)
			puntoEmision = numeroComprobanteDynamics.Substring(6, 3)
			secuencial = numeroComprobanteDynamics.Substring(9, 9)
		ElseIf tipoComprobante = TIPO_DE_COMPROBANTE_NOTA_DE_CREDITO Then
			'NC_006001000029317
			'01234567890123456
			establecimiento = numeroComprobanteDynamics.Substring(3, 3)
			puntoEmision = numeroComprobanteDynamics.Substring(6, 3)
			secuencial = numeroComprobanteDynamics.Substring(9, 9)
		ElseIf tipoComprobante = TIPO_DE_COMPROBANTE_COMPROBANTE_DE_RETENCION Then
			'006001000029317
			'01234567890123456
			establecimiento = numeroComprobanteDynamics.Substring(0, 3)
			puntoEmision = numeroComprobanteDynamics.Substring(3, 3)
			secuencial = numeroComprobanteDynamics.Substring(6, 9)
		ElseIf tipoComprobante = TIPO_DE_COMPROBANTE_LIQUIDACION_COMPRA Then
			'LC_006001000029317
			'01234567890123456
			establecimiento = numeroComprobanteDynamics.Substring(3, 3)
			puntoEmision = numeroComprobanteDynamics.Substring(6, 3)
			secuencial = numeroComprobanteDynamics.Substring(9, 9)
		End If
    End Sub

    Public Sub agregarPagina()
        paginaActual = documento.AddPage
        paginaActual.Size = PageSize.A4
        paginaActual.Orientation = PageOrientation.Portrait
        fila = 1
        dibujarLogotipoInicioPagina()
    End Sub

    Public Sub guardar()
        documento.Save(configuracion.ubicacionTemporalRides & "\" & numeroComprobanteDynamics & ".PDF")
    End Sub

    Private Function coordenadaFila(valorFila As Integer) As Double
        Return valorFila * configuracion.tamanoGridRide
    End Function

    Private Function coordenadaColumna(valorColumna As Integer) As Double
        Return valorColumna * configuracion.tamanoGridRide
    End Function

    Private Function tipoComprobanteCadena() As String
        Select Case tipoComprobante
            Case TIPO_DE_COMPROBANTE_FACTURA
                Return "FACTURA:"
            Case TIPO_DE_COMPROBANTE_GUIA_DE_REMISION
                Return "GUIA DE REMISION:"
			Case TIPO_DE_COMPROBANTE_COMPROBANTE_DE_RETENCION
				Return "COMPROBANTE DE RETENCIÓN:"
            Case TIPO_DE_COMPROBANTE_NOTA_DE_CREDITO
				Return "NOTA DE CRÉDITO:"
			Case TIPO_DE_COMPROBANTE_LIQUIDACION_COMPRA
				Return "LIQUIDACIÓN DE COMPRA:"
			Case Else
                Return ""
        End Select
    End Function
End Class
