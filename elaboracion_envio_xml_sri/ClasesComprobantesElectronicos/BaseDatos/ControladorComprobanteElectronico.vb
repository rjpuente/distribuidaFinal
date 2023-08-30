Public Class ControladorComprobanteElectronico
    Inherits ControladorGenerico

    Public datos As New RegistroComprobanteElectronico
    Private comando As New Npgsql.NpgsqlCommand

    Public Sub New()
        sqlServer.conectar()
        nombreTabla = "comprobante_electronico"
        listaCampos = "numero_comprobante, clave_acceso, ruc, fecha_inicio_proceso, fecha_generacion_xml, fecha_firma_xml, fecha_autorizacion, numero_autorizacion_sri, numero_intentos_transicion_autorizacion, transicion_autorizacion, mensaje_error_autorizacion"
        campoClave = "numero_comprobante"

        comando.Connection = sqlServer.enlace
        comando.Parameters.Add("@numero_comprobante", NpgsqlTypes.NpgsqlDbType.Varchar)
        comando.Parameters.Add("@clave_acceso", NpgsqlTypes.NpgsqlDbType.Varchar)
        comando.Parameters.Add("@ruc", NpgsqlTypes.NpgsqlDbType.Varchar)
        comando.Parameters.Add("@fecha_inicio_proceso", NpgsqlTypes.NpgsqlDbType.Date)
        comando.Parameters.Add("@fecha_generacion_xml", NpgsqlTypes.NpgsqlDbType.Date)
        comando.Parameters.Add("@fecha_firma_xml", NpgsqlTypes.NpgsqlDbType.Date)
        comando.Parameters.Add("@fecha_autorizacion", NpgsqlTypes.NpgsqlDbType.Date)
        comando.Parameters.Add("@numero_autorizacion_sri", NpgsqlTypes.NpgsqlDbType.Varchar)
        comando.Parameters.Add("@numero_intentos_transicion_autorizacion", NpgsqlTypes.NpgsqlDbType.Integer)
        comando.Parameters.Add("@transicion_autorizacion", NpgsqlTypes.NpgsqlDbType.Integer)
        comando.Parameters.Add("@mensaje_error_autorizacion", NpgsqlTypes.NpgsqlDbType.Varchar)

        asignarParametros()
    End Sub

    Public Overrides Sub actualizar()
        comando.CommandText = "UPDATE " & nombreTabla & " SET " &
            "clave_acceso=@clave_acceso," &
            "ruc=@ruc," &
            "fecha_inicio_proceso=@fecha_inicio_proceso," &
            "fecha_generacion_xml=@fecha_generacion_xml," &
            "fecha_firma_xml=@fecha_firma_xml," &
            "fecha_autorizacion=@fecha_autorizacion," &
            "numero_autorizacion_sri=@numero_autorizacion_sri," &
            "numero_intentos_transicion_autorizacion=@numero_intentos_transicion_autorizacion," &
            "transicion_autorizacion=@transicion_autorizacion," &
            "mensaje_error_autorizacion=@mensaje_error_autorizacion " &
            "WHERE " & campoClave & "=@numero_comprobante"

        asignarParametros()

        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub eliminar()
        comando.CommandText = "DELETE FROM " & nombreTabla & " WHERE " & campoClave & "=@numero_comprobante"
        comando.Parameters("@numero_comprobante").Value = datos.numeroComprobante
        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub insertar()
        comando.CommandText = "INSERT INTO " & nombreTabla & " (" & listaCampos & ") VALUES " &
            "(@numero_comprobante,@clave_acceso,@ruc,@fecha_inicio_proceso,@fecha_generacion_xml,@fecha_firma_xml,@fecha_autorizacion,@numero_autorizacion_sri,@numero_intentos_transicion_autorizacion,@transicion_autorizacion,@mensaje_error_autorizacion)"
        asignarParametros()
        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub obtener(valor As String)
        comando.CommandText = "SELECT " & listaCampos & " FROM " & nombreTabla & " WHERE " & campoClave & "=@numero_comprobante"
        comando.Parameters("@numero_comprobante").Value = valor

        Using aux As Npgsql.NpgsqlDataReader = comando.ExecuteReader()
            If aux.Read Then
                existe = True
                datos.numeroComprobante = aux("numero_comprobante")
                datos.claveAcceso = aux("clave_acceso")
                datos.ruc = aux("ruc")
                datos.fechaInicioProceso = aux("fecha_inicio_proceso")
                datos.fechaGeneracionXML = aux("fecha_generacion_xml")
                datos.fechaFirmaXML = aux("fecha_firma_xml")
                datos.fechaAutorizacion = aux("fecha_autorizacion")
                datos.numeroAutorizacionSRI = aux("numero_autorizacion_sri")
                datos.numeroIntentosTransicionAutorizacion = aux("numero_intentos_transicion_autorizacion")
                datos.transicionAutorizacion = aux("transicion_autorizacion")
                datos.mensajeErrorAutorizacion = aux("mensaje_error_autorizacion")
            Else
                existe = False
                datos = New RegistroComprobanteElectronico
            End If
        End Using
    End Sub

    Private Sub asignarParametros()
        comando.Parameters("@numero_comprobante").Value = datos.numeroComprobante
        comando.Parameters("@clave_acceso").Value = datos.claveAcceso
        comando.Parameters("@ruc").Value = datos.ruc
        comando.Parameters("@fecha_inicio_proceso").Value = datos.fechaInicioProceso
        comando.Parameters("@fecha_generacion_xml").Value = datos.fechaGeneracionXML
        comando.Parameters("@fecha_firma_xml").Value = datos.fechaFirmaXML
        comando.Parameters("@fecha_autorizacion").Value = datos.fechaAutorizacion
        comando.Parameters("@numero_autorizacion_sri").Value = datos.numeroAutorizacionSRI
        comando.Parameters("@numero_intentos_transicion_autorizacion").Value = datos.numeroIntentosTransicionAutorizacion
        comando.Parameters("@transicion_autorizacion").Value = datos.transicionAutorizacion
        If datos.mensajeErrorAutorizacion.Length > 300 Then
            comando.Parameters("@mensaje_error_autorizacion").Value = datos.mensajeErrorAutorizacion.Substring(0, 300)
        Else
            comando.Parameters("@mensaje_error_autorizacion").Value = datos.mensajeErrorAutorizacion
        End If

    End Sub
End Class
