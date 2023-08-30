Public Class ControladorContribuyente
    Inherits ControladorGenerico

    Public datos As New RegistroContribuyente
    Private comando As New Npgsql.NpgsqlCommand

    Public Sub New()
        sqlServer.conectar()
        nombreTabla = "cliente"
        listaCampos = "numero_cedula, nombre, codigo_cliente, '' AS codigoProveedor, correo, '' AS enlace "
        campoClave = "numero_cedula"

        comando.Connection = sqlServer.enlace
        comando.Parameters.Add("@numero_cedula", SqlDbType.VarChar)
        comando.Parameters.Add("@nombre", SqlDbType.VarChar)
        comando.Parameters.Add("@codigo_cliente", SqlDbType.VarChar)
        comando.Parameters.Add("@codigoProveedor", SqlDbType.VarChar)
        comando.Parameters.Add("@correo", SqlDbType.VarChar)
        comando.Parameters.Add("@enlace", SqlDbType.VarChar)

        asignarParametros()
    End Sub

    Public Overrides Sub actualizar()
        comando.CommandText = "UPDATE " & nombreTabla & " SET " &
            "nombre=@nombre," &
            "codigo_cliente=@codigo_cliente," &
            "correo=@correo," &
            "WHERE " & campoClave & "=@numero_cedula"

        asignarParametros()

        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub eliminar()
        comando.CommandText = "DELETE FROM " & nombreTabla & " WHERE " & campoClave & "=@ruc"
        comando.Parameters("@ruc").Value = datos.ruc
        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub insertar()
        comando.CommandText = "INSERT INTO " & nombreTabla & " (" & listaCampos & ") VALUES " &
            "(@numero_cedula,@nombre,@codigo_cliente,@correo)"
        asignarParametros()
        comando.ExecuteNonQuery()
    End Sub

    Public Overrides Sub obtener(valor As String)
        comando.CommandText = "SELECT " & listaCampos & " FROM " & nombreTabla & " WHERE " & campoClave & "=@numero_cedula"
        comando.Parameters("@numero_cedula").Value = valor

        Using aux As Npgsql.NpgsqlDataReader = comando.ExecuteReader()
            If aux.Read Then
                existe = True
                datos.ruc = aux("numero_cedula")
                datos.razonSocial = aux("nombre")
                datos.codigoCliente = aux("codigo_cliente")
                datos.codigoProveedor = aux("codigoProveedor")
                datos.email = aux("correo")
                datos.enlace = aux("enlace")
            Else
                existe = False
                datos = New RegistroContribuyente
            End If
        End Using
    End Sub

    Private Sub asignarParametros()
        comando.Parameters("@numero_cedula").Value = datos.ruc
        If datos.razonSocial.Length <= 100 Then
            comando.Parameters("@nombre").Value = datos.razonSocial
        Else
            comando.Parameters("@nombre").Value = datos.razonSocial.Substring(0, 100)
        End If
        comando.Parameters("@codigo_cliente").Value = datos.codigoCliente
        comando.Parameters("@correo").Value = datos.email
    End Sub

    Public Sub obtenerDesdeCodigoCliente(valor As String)
        comando.CommandText = "SELECT " & listaCampos & " FROM " & nombreTabla & " WHERE codigoCliente=@codigoCliente"
        comando.Parameters("@codigoCliente").Value = valor

        Using aux As Npgsql.NpgsqlDataReader = comando.ExecuteReader()
            If aux.Read Then
                existe = True
                datos.ruc = aux("numero_cedula")
                datos.razonSocial = aux("nombre")
                datos.codigoCliente = aux("codigo_cliente")
                datos.codigoProveedor = aux("codigoProveedor")
                datos.email = aux("correo")
                datos.enlace = aux("enlace")
            Else
                existe = False
                datos = New RegistroContribuyente
            End If
        End Using
    End Sub

    Public Sub obtenerDesdeEnlace(valor As String)
        comando.CommandText = "SELECT " & listaCampos & " FROM " & nombreTabla & " WHERE enlace=@enlace"
        comando.Parameters("@enlace").Value = valor

        Using aux As Npgsql.NpgsqlDataReader = comando.ExecuteReader()
            If aux.Read Then
                existe = True
                datos.ruc = aux("numero_cedula")
                datos.razonSocial = aux("nombre")
                datos.codigoCliente = aux("codigo_cliente")
                datos.codigoProveedor = aux("codigoProveedor")
                datos.email = aux("correo")
                datos.enlace = aux("enlace")
            Else
                existe = False
                datos = New RegistroContribuyente
            End If
        End Using
    End Sub
End Class
