Public Class ControladorArchivoXML
    Inherits ControladorGenerico

    Public datos As New RegistroArchivoXML
    Private comando As New Npgsql.NpgsqlCommand
    Private prmNumeroComprobante As New Npgsql.NpgsqlParameter("@numero_comprobante", SqlDbType.VarChar)
    Private prmArchivo As New SqlClient.SqlParameter("@archivo", SqlDbType.VarBinary)

    Public Sub New()
        sqlServer.conectar()
        nombreTabla = "REMECO.DBO.ARCHIVO_XML"
        listaCampos = "numeroComprobante,archivo"
        campoClave = "numeroComprobante"

        comando.Connection = sqlServer.enlace
        comando.Parameters.Add(prmNumeroComprobante)
        comando.Parameters.Add(prmArchivo)

        asignarParametros()
    End Sub

    Public Overrides Sub actualizar()
        comando.CommandText = "UPDATE " & nombreTabla & " SET " &
            "archivo=@archivo " &
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
        comando.Parameters.Remove(prmArchivo)
        comando.CommandText = "INSERT INTO " & nombreTabla & " (" & listaCampos & ") VALUES " &
            "(@numero_comprobante,0x)"
        asignarParametros()
        comando.ExecuteNonQuery()
        comando.Parameters.Add(prmArchivo)
    End Sub

    Public Overrides Sub obtener(valor As String)
        comando.CommandText = "SELECT " & listaCampos & " FROM " & nombreTabla & " WHERE " & campoClave & "=@numero_comprobante"
        comando.Parameters("@numero_comprobante").Value = valor
        comando.Parameters.Remove(prmArchivo)

        Using aux As Npgsql.NpgsqlDataReader = comando.ExecuteReader()
            If aux.Read Then
                existe = True
                datos.numeroComprobante = aux("numeroComprobante")
                datos.archivo = aux("archivo")
            Else
                existe = False
                datos = New RegistroArchivoXML
            End If
        End Using

        comando.Parameters.Add(prmArchivo)
    End Sub

    Public Sub obtenerSiguienteLibre()
        comando.CommandText = "SELECT TOP 1 " & listaCampos & " FROM " & nombreTabla & " WHERE BORRAME='' ORDER BY numeroComprobante"

        Using aux As Npgsql.NpgsqlDataReader = comando.ExecuteReader()
            If aux.Read Then
                existe = True
                datos.numeroComprobante = aux("numeroComprobante")
                datos.archivo = aux("archivo")
            Else
                existe = False
                datos = New RegistroArchivoXML
            End If
        End Using
    End Sub

    Private Sub asignarParametros()
        prmNumeroComprobante.Value = datos.numeroComprobante
        If Not datos.archivo Is Nothing Then
            prmArchivo.Value = datos.archivo
        End If
    End Sub
End Class
