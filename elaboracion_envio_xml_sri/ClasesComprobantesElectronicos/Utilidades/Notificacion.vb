Public Class Notificacion
    Private contenido As String
    Private listaDestinatarios As New List(Of String)
    Private asunto As String

    Public Sub New(asunto As String)
        Me.asunto = asunto
    End Sub

    Public Sub agregarTituloReporte(valor As String)
        contenido = contenido & "<p class='tituloReporte'>" & valor & "</p>"
    End Sub

    Public Sub agregarTituloSeccion(valor As String)
        contenido = contenido & "<p class='seccionReporte'>" & valor & "</p>"
    End Sub

    Public Sub agregarParrafo(valor As String)
        contenido = contenido & "<p class='normal'>" & valor & "</p>"
    End Sub

    Public Sub agregarHTMLPuro(valor As String)
        contenido = contenido & valor
    End Sub

    Public Sub agregarDestinatario(email As String)
        listaDestinatarios.Add(email)
    End Sub

    Public Sub agregarListaSeparada(lista As String, separador As String)
        Dim arreglo() As String

        arreglo = lista.Split(";")
        For i As Integer = 0 To arreglo.Length - 1
            If arreglo(i).Length > 0 Then
                agregarDestinatario(arreglo(i))
            End If
        Next
    End Sub

    Public Sub ordenarEnvio()
        Dim ntfCodigo As Long
        Dim clsNotificacion As New ClaseNotificacion
        ntfCodigo = clsNotificacion.crearNueva(contenido, asunto)
        If ntfCodigo > 0 Then
            For Each destinatario As String In listaDestinatarios
                clsNotificacion.agregarDestinatario(ntfCodigo, destinatario)
            Next
        End If
        clsNotificacion.ordenarEnvio(ntfCodigo)
    End Sub

    Public Sub agregarHipervinculo(texto As String, direccion As String)
        contenido = contenido & "<a href='" & direccion & "'>" & texto & "</a>"
    End Sub
End Class
