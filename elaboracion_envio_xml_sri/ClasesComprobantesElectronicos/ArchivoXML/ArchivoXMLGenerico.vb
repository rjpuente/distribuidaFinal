Imports System.Xml

Public MustInherit Class ArchivoXMLGenerico
    Public configuracion As ClasesComprobantesElectronicos.ConfiguracionActual
    Private lecCompFisInfTable As DataTable
    Public archivoXML As XmlTextWriter
    Dim tipoEmision As Integer
    Public fechaEmision As Date
    Dim tipoComprobante As String
    Dim ruc As String
    Dim razonSocial As String
    Dim nombreComercial As String
    Dim tipoAmbiente As Integer
    Public tipoIdentificacionTercero As String
    Public razonSocialTercero As String
    Public identificacionTercero As String
    Public establecimiento As String
    Public puntoEmision As String
    Public secuencial As Long
    Dim direccionMatriz As String
    Public direccionEstablecimiento As String
    Public numeroComprobanteDynamics As String
    Public mensajeError As String = ""
    Public hayError As Boolean = False
    Public claveAccesoGenerada As String

    Const CARACTERES_NO_PERMITIDOS = Chr(9) & Chr(13) & Chr(10)
    Const MAX_LONGITUD_RAZON_SOCIAL = 300
    Const MAX_LONGITUD_NOMBRE_COMERCIAL = 300
    Const MAX_LONGITUD_RUC = 13
    Const MAX_LONGITUD_CLAVE_ACCESO = 49

    Public MustOverride Sub obtenerEncabezadoDocumento()
    Public MustOverride Sub generar()

    Public Sub New(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual, tipoEmision As Integer, tipoComprobante As String, tipoAmbiente As Integer, numeroComprobante As String)
        sqlServer = New ConectorSQLServer(configuracion)
        Me.configuracion = configuracion
        Me.tipoEmision = tipoEmision
        Me.tipoComprobante = tipoComprobante
        Me.tipoAmbiente = tipoAmbiente
        Me.numeroComprobanteDynamics = numeroComprobante
        desglosarNumeroComprobante()
        obtenerInformacionFiscal()
        archivoXML = New XmlTextWriter(configuracion.ubicacionTemporalXML.Replace("\\", "\") & "\" & numeroComprobanteDynamics & ".xml", System.Text.Encoding.UTF8)
        archivoXML.Formatting = Formatting.Indented
        archivoXML.Indentation = 2
    End Sub

    Private Sub crearArchivo(nombreArchivo As String)
        archivoXML = New XmlTextWriter(nombreArchivo, System.Text.Encoding.UTF8)
    End Sub


    Private Sub obtenerInformacionFiscal()
        Dim consulta As String = "SELECT ruc, nombre_social, nombre_comercial, direccion FROM informacion_fiscal"
        Dim registro As DataRow

        lecCompFisInfTable = sqlServer.consultar(consulta)
        registro = lecCompFisInfTable.Rows(0)
        ruc = registro("ruc")
        razonSocial = registro("nombre_social")
        nombreComercial = registro("nombre_comercial")
        direccionMatriz = registro("direccion")
        direccionEstablecimiento = registro("direccion")
    End Sub
    Public Sub tagInfoTributaria()
        With archivoXML
            .WriteStartElement("infoTributaria")

            tagAmbiente()
            tagTipoEmision()
            tagRazonSocial()
            tagNombreComercial()
            tagRuc()
            tagClaveAcceso()
            tagCodDoc()
            tagEstab()
            tagPtoEmi()
            tagSecuencial()
            tagDirMatriz()

            .WriteEndElement()
        End With
    End Sub

    Public Sub tagAmbiente()
        With archivoXML
            .WriteStartElement("ambiente")
            .WriteString(tipoAmbiente)
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagTipoEmision()
        With archivoXML
            .WriteStartElement("tipoEmision")
            .WriteString(tipoEmision)
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagRazonSocial()
        With archivoXML
            .WriteStartElement("razonSocial")
            .WriteString(cadenaCaracteres(razonSocial, MAX_LONGITUD_RAZON_SOCIAL))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagNombreComercial()
        With archivoXML
            .WriteStartElement("nombreComercial")
            .WriteString(cadenaCaracteres(nombreComercial, MAX_LONGITUD_NOMBRE_COMERCIAL))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagRuc()
        With archivoXML
            .WriteStartElement("ruc")
            .WriteString(cadenaCaracteres(ruc, MAX_LONGITUD_RUC))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagClaveAcceso()
        With archivoXML
            .WriteStartElement("claveAcceso")
            .WriteString(cadenaCaracteres(claveAcceso(), 49))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagCodDoc()
        With archivoXML
            .WriteStartElement("codDoc")
            .WriteString(cadenaCaracteres(tipoComprobante, 2))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagEstab()
        With archivoXML
            .WriteStartElement("estab")
            .WriteString(cadenaCaracteres(establecimiento, 3))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagPtoEmi()
        With archivoXML
            .WriteStartElement("ptoEmi")
            .WriteString(cadenaCaracteres(puntoEmision, 3))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagSecuencial()
        With archivoXML
            .WriteStartElement("secuencial")
            .WriteString(cerosIzquierda(secuencial.ToString, 9))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDirMatriz()
        With archivoXML
            .WriteStartElement("dirMatriz")
            .WriteString(cadenaCaracteres(direccionMatriz, 300))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagDirEstablecimiento()
        With archivoXML
            .WriteStartElement("dirEstablecimiento")
            .WriteString(cadenaCaracteres(direccionEstablecimiento, 300))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagObligadoContabilidad()
        With archivoXML
            .WriteStartElement("obligadoContabilidad")
            .WriteString(cadenaCaracteres("NO", 2))
            .WriteEndElement()
        End With
    End Sub

    Public Sub tagContribuyenteEspecial()
        With archivoXML
            .WriteStartElement("contribuyenteEspecial")
            .WriteString(cadenaCaracteres(configuracion.codigoContribuyenteEspecial, 5))
            .WriteEndElement()
        End With
    End Sub

    Function cadenaCaracteres(valor As String, longitudMaxima As Integer) As String
        Dim cadena As String = ""
        Dim i As Integer

        'Eliminar caracteres no permitidos
        For i = 0 To valor.Length - 1
            If Not InStr(CARACTERES_NO_PERMITIDOS, valor.Substring(i, 1)) > 0 Then
                cadena = cadena & valor.Substring(i, 1)
            End If
        Next

        If cadena.Length > longitudMaxima Then
            Return cadena.Substring(0, longitudMaxima - 1)
        Else
            Return cadena
        End If
    End Function

    Function claveAcceso() As String
        Dim ret As String = ""

        If tipoEmision = TIPO_DE_EMISION_NORMAL Then
            ret = ret & cerosIzquierda(fechaEmision.Day.ToString, 2)
            ret = ret & cerosIzquierda(fechaEmision.Month.ToString, 2)
            ret = ret & cerosIzquierda(fechaEmision.Year.ToString, 4)
            ret = ret & cerosIzquierda(tipoComprobante, 2)
            ret = ret & cerosIzquierda(ruc, 13)
            ret = ret & cerosIzquierda(tipoAmbiente.ToString, 1)
            ret = ret & cerosIzquierda(establecimiento, 3)
            ret = ret & cerosIzquierda(puntoEmision, 3)
            ret = ret & cerosIzquierda(secuencial.ToString, 9)
            ret = ret & cerosIzquierda(secuencial.ToString, 8)
			ret = ret & cerosIzquierda(tipoEmision.ToString, 1)
            ret = ret & cerosIzquierda(digitoVerificador(ret).ToString, 1)
        End If
        claveAccesoGenerada = ret
        Return ret
    End Function

    Function digitoVerificador(cadena As String) As Integer
        Dim factor As Integer = 2
        Dim valorCalculado As Integer
        Dim sumatoria As Integer = 0
        Dim residuo11 As Integer
        Dim diferencia11 As Integer

        For i As Integer = cadena.Length - 1 To 0 Step -1
            valorCalculado = CInt(cadena.Substring(i, 1)) * factor
            sumatoria += valorCalculado
            factor += 1
            If factor = 8 Then factor = 2
        Next
        residuo11 = sumatoria Mod 11
        diferencia11 = 11 - residuo11
        If diferencia11 = 10 Then
            diferencia11 = 1
        ElseIf diferencia11 = 11 Then
            diferencia11 = 0
        End If
        Return diferencia11
    End Function

	'Function cadenaAleatoriaNumeros(longitud As Integer) As String
	'Dim ret As String = ""
	'Dim numero As Integer
	'
	'Randomize()
	'For i As Integer = 1 To longitud
	'       numero = Int(Rnd() * 10)
	'      ret = ret & numero.ToString
	'Next
	'Return ret
	'End Function

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
End Class
