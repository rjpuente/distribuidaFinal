Public Class ConfiguracionActual
    Dim xBaseSQLServer As String
    Dim xCodigoContribuyenteEspecial As String
    Dim xContrasenaFirmaElectronica As String
    Dim xContrasenaSQLServer As String
    Dim xDestinatarioProblemasLogistica As String
    Dim xDestinatarioProblemasSistemas As String
    Dim xEstablecimientoMatriz As String
    Dim xFechaArranqueEmisionElectronica As Date
    Dim xMaximoIntentosEntreTransiciones As Integer
    Dim xNumeroMaximoLineasRide As Integer
    Dim xServidorSQLServer As String
    Dim xTamanoFuenteCodigoBarras As Integer
    Dim xTamanoFuenteDetalle As Integer
    Dim xTamanoFuenteEstandar As Integer
    Dim xTamanoFuenteNumeros As Integer
    Dim xTamanoGridRide As Integer
    Dim xTipoAmbienteActual As Integer
    Dim xTipoFuenteCodigoBarras As String
    Dim xTipoFuenteDetalle As String
    Dim xTipoFuenteEstandar As String
    Dim xTipoFuenteNumeros As String
    Dim xUbicacionArchivosSinFirma As String
    Dim xUbicacionFirmadorJava As String
    Dim xUbicacionFirmaElectronica As String
    Dim xUbicacionLogo1800Remeco As String
    Dim xUbicacionLogoRemeco As String
    Dim xUbicacionTemporalRides As String
    Dim xUbicacionTemporalXML As String
    Dim xUbicacionTemporalXMLFirmados As String
    Dim xUsuarioSQLServer As String
    Dim xPuertoSqlSever As String

    Public Property baseSQLServer As String
        Get
            Return xBaseSQLServer
        End Get

        Set(value As String)
            xBaseSQLServer = value
        End Set
    End Property

    Public Property codigoContribuyenteEspecial As String
        Get
            Return xCodigoContribuyenteEspecial
        End Get

        Set(value As String)
            xCodigoContribuyenteEspecial = value
        End Set
    End Property

    Public Property contrasenaFirmaElectronica As String
        Get
            Return xContrasenaFirmaElectronica
        End Get

        Set(value As String)
            xContrasenaFirmaElectronica = value
        End Set
    End Property

    Public Property contrasenaSQLServer As String
        Get
            Return xContrasenaSQLServer
        End Get

        Set(value As String)
            xContrasenaSQLServer = value
        End Set
    End Property

    Public Property destinatarioProblemasLogistica As String
        Get
            Return xDestinatarioProblemasLogistica
        End Get

        Set(value As String)
            xDestinatarioProblemasLogistica = value
        End Set
    End Property

    Public Property destinatarioProblemasSistemas As String
        Get
            Return xDestinatarioProblemasSistemas
        End Get

        Set(value As String)
            xDestinatarioProblemasSistemas = value
        End Set
    End Property

    Public Property establecimientoMatriz As String
        Get
            Return xEstablecimientoMatriz
        End Get

        Set(value As String)
            xEstablecimientoMatriz = value
        End Set
    End Property

    Public Property fechaArranqueEmisionElectronica As Date
        Get
            Return xFechaArranqueEmisionElectronica
        End Get

        Set(value As Date)
            xFechaArranqueEmisionElectronica = value
        End Set
    End Property

    Public Property tamanoGridRide As String
        Get
            Return xTamanoGridRide
        End Get

        Set(value As String)
            xTamanoGridRide = value
        End Set
    End Property

    Public Property ubicacionLogo1800Remeco As String
        Get
            Return xUbicacionLogo1800Remeco
        End Get

        Set(value As String)
            xUbicacionLogo1800Remeco = value
        End Set
    End Property

    Public Property ubicacionLogoRemeco As String
        Get
            Return xUbicacionLogoRemeco
        End Get

        Set(value As String)
            xUbicacionLogoRemeco = value
        End Set
    End Property

    Public Property maximoIntentosEntreTransiciones As String
        Get
            Return xMaximoIntentosEntreTransiciones
        End Get

        Set(value As String)
            xMaximoIntentosEntreTransiciones = value
        End Set
    End Property

    Public Property numeroMaximoLineasRide As String
        Get
            Return xNumeroMaximoLineasRide
        End Get

        Set(value As String)
            xNumeroMaximoLineasRide = value
        End Set
    End Property

    Public Property ubicacionTemporalXML As String
        Get
            Return xUbicacionTemporalXML
        End Get

        Set(value As String)
            xUbicacionTemporalXML = value
        End Set
    End Property

    Public Property servidorSQLServer As String
        Get
            Return xServidorSQLServer
        End Get

        Set(value As String)
            xServidorSQLServer = value
        End Set
    End Property

    Public Property tamanoFuenteCodigoBarras As String
        Get
            Return xTamanoFuenteCodigoBarras
        End Get

        Set(value As String)
            xTamanoFuenteCodigoBarras = value
        End Set
    End Property

    Public Property tamanoFuenteDetalle As String
        Get
            Return xTamanoFuenteDetalle
        End Get

        Set(value As String)
            xTamanoFuenteDetalle = value
        End Set
    End Property

    Public Property tamanoFuenteEstandar As String
        Get
            Return xTamanoFuenteEstandar
        End Get

        Set(value As String)
            xTamanoFuenteEstandar = value
        End Set
    End Property

    Public Property tamanoFuenteNumeros As String
        Get
            Return xTamanoFuenteNumeros
        End Get

        Set(value As String)
            xTamanoFuenteNumeros = value
        End Set
    End Property

    Public Property ubicacionTemporalXMLFirmados As String
        Get
            Return xUbicacionTemporalXMLFirmados
        End Get

        Set(value As String)
            xUbicacionTemporalXMLFirmados = value
        End Set
    End Property

    Public Property tipoAmbienteActual As String
        Get
            Return xTipoAmbienteActual
        End Get

        Set(value As String)
            xTipoAmbienteActual = value
        End Set
    End Property

    Public Property tipoFuenteCodigoBarras As String
        Get
            Return xTipoFuenteCodigoBarras
        End Get

        Set(value As String)
            xTipoFuenteCodigoBarras = value
        End Set
    End Property

    Public Property tipoFuenteDetalle As String
        Get
            Return xTipoFuenteDetalle
        End Get

        Set(value As String)
            xTipoFuenteDetalle = value
        End Set
    End Property

    Public Property tipoFuenteEstandar As String
        Get
            Return xTipoFuenteEstandar
        End Get

        Set(value As String)
            xTipoFuenteEstandar = value
        End Set
    End Property

    Public Property tipoFuenteNumeros As String
        Get
            Return xTipoFuenteNumeros
        End Get

        Set(value As String)
            xTipoFuenteNumeros = value
        End Set
    End Property

    Public Property ubicacionArchivosSinFirma As String
        Get
            Return xUbicacionArchivosSinFirma
        End Get

        Set(value As String)
            xUbicacionArchivosSinFirma = value
        End Set
    End Property

    Public Property ubicacionFirmadorJava As String
        Get
            Return xUbicacionFirmadorJava
        End Get

        Set(value As String)
            xUbicacionFirmadorJava = value
        End Set
    End Property

    Public Property ubicacionFirmaElectronica As String
        Get
            Return xUbicacionFirmaElectronica
        End Get

        Set(value As String)
            xUbicacionFirmaElectronica = value
        End Set
    End Property

    Public Property ubicacionTemporalRides As String
        Get
            Return xUbicacionTemporalRides
        End Get

        Set(value As String)
            xUbicacionTemporalRides = value
        End Set
    End Property

    Public Property usuarioSQLServer As String
        Get
            Return xUsuarioSQLServer
        End Get

        Set(value As String)
            xUsuarioSQLServer = value
        End Set
    End Property

    Public Property puertoSqlServer As String
        Get
            Return xPuertoSqlSever
        End Get

        Set(value As String)
            xPuertoSqlSever = value
        End Set
    End Property
End Class
