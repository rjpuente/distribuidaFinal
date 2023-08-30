Public Class ControladorDestinatario
    Public nombreTabla As String
    Public listaCampos As String
    Public existe As Boolean = False

    Public datos As New RegistroDestinatario
    Private comando As New Npgsql.NpgsqlCommand

    Public Sub New()
        sqlServer.conectar()
        nombreTabla = "REMECO.DBO.DESTINATARIO"
        listaCampos = "ntfCodigo,dstEmail"

        comando.Connection = sqlServer.enlace
        comando.Parameters.Add("@ntfCodigo", SqlDbType.BigInt)
        comando.Parameters.Add("@dstEmail", SqlDbType.VarChar)

        asignarParametros()
    End Sub

    Public Sub insertar()
        comando.CommandText = "INSERT INTO " & nombreTabla & " (" & listaCampos & ") VALUES " & _
            "(@ntfCodigo,@dstEmail)"
        asignarParametros()
        comando.ExecuteNonQuery()
    End Sub

    Private Sub asignarParametros()
        comando.Parameters("@ntfCodigo").Value = datos.ntfCodigo
        comando.Parameters("@dstEmail").Value = datos.dstEmail
    End Sub
End Class
