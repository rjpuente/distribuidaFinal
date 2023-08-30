Imports Npgsql

Public Class ConectorSQLServer
    Private comando As New NpgsqlCommand
    Public enlace As New NpgsqlConnection
    Private commandBuilder As New NpgsqlCommandBuilder

    Public mensajeError As Exception
    Public hayError As Boolean

    Sub New(configuracion As ClasesComprobantesElectronicos.ConfiguracionActual)
        Dim csb As New NpgsqlConnectionStringBuilder

        csb.Host = configuracion.servidorSQLServer
        csb.Username = configuracion.usuarioSQLServer
        csb.Password = configuracion.contrasenaSQLServer
        csb.Database = configuracion.baseSQLServer
        csb.Port = configuracion.puertoSqlServer
        enlace.ConnectionString = csb.ConnectionString
        comando.Connection = enlace
        hayError = False
    End Sub

    Sub conectar()
        Try
            enlace.Open()
            hayError = False
        Catch ex As Exception
            hayError = True
            mensajeError = ex
        End Try
    End Sub

    Public Sub ejecutar(ByVal sentencia As String)
        Try
            comando.CommandText = sentencia
            comando.ExecuteNonQuery()
            hayError = False
        Catch ex As Exception
            hayError = True
            mensajeError = ex
        End Try
    End Sub

    Public Function obtenerDataSet(ByVal consulta As String) As DataSet
        Dim adapter As New NpgsqlDataAdapter()

        obtenerDataSet = New DataSet
        adapter.SelectCommand = New NpgsqlCommand(consulta, enlace)
        adapter.Fill(obtenerDataSet)
    End Function

    Public Sub desconectar()
        enlace.Close()
    End Sub

    Public Function consultar(ByVal consulta As String) As DataTable
        Dim ds As DataSet

        Try
            ds = obtenerDataSet(consulta)
            Return ds.Tables(0)
            hayError = False
        Catch ex As Exception
            hayError = True
            mensajeError = ex
            consultar = Nothing
        End Try
    End Function
End Class
