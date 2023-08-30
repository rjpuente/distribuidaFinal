Public Class ControladorArchivoRIDE
    Inherits ControladorGenerico

    Public datos As New RegistroArchivoRIDE
    Private comando As New Npgsql.NpgsqlCommand
    Private prmNumeroComprobante As New SqlClient.SqlParameter("@numeroComprobante", SqlDbType.VarChar)
    Private prmArchivo As New SqlClient.SqlParameter("@archivo", SqlDbType.VarBinary)

    Public Sub New()
        sqlServer.conectar()
        nombreTabla = "REMECO.DBO.ARCHIVO_RIDE"
        listaCampos = "numeroComprobante,archivo"
        campoClave = "numeroComprobante"

        comando.Connection = sqlServer.enlace
        comando.Parameters.Add(prmNumeroComprobante)
        comando.Parameters.Add(prmArchivo)

        asignarParametros()
    End Sub

    Public Overrides Sub actualizar()
        comando.CommandText = "UPDATE " & nombreTabla & " SET " & _
            "archivo=@archivo " & _
            "WHERE " & campoClave & "=@numeroComprobante"

        asignarParametros()

        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub eliminar()
        comando.CommandText = "DELETE FROM " & nombreTabla & " WHERE " & campoClave & "=@numeroComprobante"
        comando.Parameters("@numeroComprobante").Value = datos.numeroComprobante
        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub insertar()
        comando.Parameters.Remove(prmArchivo)
        comando.CommandText = "INSERT INTO " & nombreTabla & " (" & listaCampos & ") VALUES " & _
            "(@numeroComprobante,0x)"
        asignarParametros()
        comando.ExecuteNonQuery()
        comando.Parameters.Add(prmArchivo)
    End Sub

    Public Overrides Sub obtener(valor As String)
        comando.CommandText = "SELECT " & listaCampos & " FROM " & nombreTabla & " WHERE " & campoClave & "=@numeroComprobante"
        comando.Parameters("@numeroComprobante").Value = valor
        comando.Parameters.Remove(prmArchivo)

        Using aux As Npgsql.NpgsqlDataReader = comando.ExecuteReader()
            If aux.Read Then
                existe = True
                datos.numeroComprobante = aux("numeroComprobante")
                datos.archivo = aux("archivo")
            Else
                existe = False
                datos = New RegistroArchivoRIDE
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
                datos = New RegistroArchivoRIDE
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
