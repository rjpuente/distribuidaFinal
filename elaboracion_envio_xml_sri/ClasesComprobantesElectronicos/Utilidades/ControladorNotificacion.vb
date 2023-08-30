Public Class ControladorNotificacion
    Inherits ControladorGenerico

    Public datos As New RegistroNotificacion
    Private comando As New Npgsql.NpgsqlCommand

    Public Sub New()
        sqlServer.conectar()
        nombreTabla = "REMECO.DBO.NOTIFICACION"
        listaCampos = "ntfCodigo,ntfTexto,ntfEnviar,ntfAsunto,ntfFecha"
        campoClave = "ntfCodigo"

        comando.Connection = sqlServer.enlace
        comando.Parameters.Add("@ntfCodigo", SqlDbType.BigInt)
        comando.Parameters.Add("@ntfTexto", SqlDbType.VarChar)
        comando.Parameters.Add("@ntfEnviar", SqlDbType.SmallInt)
        comando.Parameters.Add("@ntfAsunto", SqlDbType.VarChar)
        comando.Parameters.Add("@ntfFecha", SqlDbType.DateTime)

        asignarParametros()

        datos.ntfCodigo = determinarNtfCodigo()
    End Sub

    Public Overrides Sub actualizar()
        comando.CommandText = "UPDATE " & nombreTabla & " SET " & _
            "ntfCodigo=@ntfCodigo," & _
            "ntfTexto=@ntfTexto," & _
            "ntfEnviar=@ntfEnviar," & _
            "ntfAsunto=@ntfAsunto," & _
            "ntfFecha=@ntfFecha " & _
            "WHERE " & campoClave & "=@ntfCodigo"

        asignarParametros()

        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub eliminar()
        comando.CommandText = "DELETE FROM " & nombreTabla & " WHERE " & campoClave & "=@ntfCodigo"
        comando.Parameters("@ntfCodigo").Value = datos.ntfCodigo
        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub insertar()
        comando.CommandText = "INSERT INTO " & nombreTabla & " (" & listaCampos & ") VALUES " & _
            "(@ntfCodigo,@ntfTexto,@ntfEnviar,@ntfAsunto,@ntfFecha)"
        asignarParametros()
        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub obtener(valor As String)
        comando.CommandText = "SELECT " & listaCampos & " FROM " & nombreTabla & " WHERE " & campoClave & "=@ntfCodigo"
        comando.Parameters("@ntfCodigo").Value = valor

        Using aux As Npgsql.NpgsqlDataReader = comando.ExecuteReader()
            If aux.Read Then
                existe = True
                datos.ntfCodigo = aux("ntfCodigo")
                datos.ntfTexto = aux("ntfTexto")
                datos.ntfEnviar = aux("ntfEnviar")
                datos.ntfAsunto = aux("ntfAsunto")
                datos.ntfFecha = aux("ntfFecha")
            Else
                existe = False
                datos = New RegistroNotificacion
            End If
        End Using
    End Sub

    Private Sub asignarParametros()
        comando.Parameters("@ntfCodigo").Value = datos.ntfCodigo
        comando.Parameters("@ntfTexto").Value = datos.ntfTexto
        comando.Parameters("@ntfEnviar").Value = datos.ntfEnviar
        comando.Parameters("@ntfAsunto").Value = datos.ntfAsunto
        comando.Parameters("@ntfFecha").Value = datos.ntfFecha
    End Sub

    Private Function determinarNtfCodigo()
        Dim valor As Long

        Randomize()

        valor = Int(Rnd() * 999999999999)

        Return valor
    End Function

    Public Function obtenerDestinatarios() As List(Of String)
        Dim ret As New List(Of String)
        Dim aux As DataTable

        aux = sqlServer.consultar("SELECT DSTEMAIL FROM REMECO.DBO.DESTINATARIO WHERE NTFCODIGO=" & Me.datos.ntfCodigo.ToString)
        For Each registro As DataRow In aux.Rows
            ret.Add(registro("DSTEMAIL"))
        Next
        Return ret
    End Function
End Class
