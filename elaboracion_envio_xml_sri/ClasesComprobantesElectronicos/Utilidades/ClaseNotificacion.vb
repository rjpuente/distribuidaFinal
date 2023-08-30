Public Class ClaseNotificacion
    Public Function crearNueva(ntfTexto As String, ntfAsunto As String) As Long
        Try
            Dim notificacion As New ControladorNotificacion
            notificacion.datos.ntfTexto = ntfTexto
            notificacion.datos.ntfAsunto = ntfAsunto
            notificacion.datos.ntfEnviar = 0
            notificacion.datos.ntfFecha = Now
            notificacion.insertar()
            Return notificacion.datos.ntfCodigo
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function agregarDestinatario(ntfCodigo As Long, dstEmail As String) As Boolean
        Dim destinatario As New ControladorDestinatario
        destinatario.datos.dstEmail = dstEmail
        destinatario.datos.ntfCodigo = ntfCodigo
        destinatario.insertar()
        Return True
    End Function

    Public Function ordenarEnvio(ntfCodigo As Long) As Boolean
        Try
            Dim notificacion As New ControladorNotificacion
            notificacion.obtener(ntfCodigo)
            notificacion.datos.ntfEnviar = 1
            notificacion.actualizar()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function eliminar(ntfCodigo As Long) As Boolean
        Try
            Dim notificacion As New ControladorNotificacion
            notificacion.datos.ntfCodigo = ntfCodigo
            notificacion.eliminar()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
