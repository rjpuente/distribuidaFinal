Module FuncionesGlobales
    Public Function cerosIzquierda(valor As String, longitud As Integer) As String
        Dim ret As String = valor

        Do While ret.Length < longitud
            ret = "0" & ret
        Loop
        Return ret
    End Function


    Public Function convertirTipoIdentificacionDynamics(valor As String) As String
        If valor = "R" Then
            Return TIPO_IDENTIFICACION_RUC
        ElseIf valor = "C" Then
            Return TIPO_IDENTIFICACION_CEDULA
        ElseIf valor = "O" Then
            Return TIPO_IDENTIFICACION_IDENTIFICACION_DEL_EXTERIOR
        ElseIf valor = "P" Then
            Return TIPO_IDENTIFICACION_PASAPORTE
        Else
            Return ""
        End If
    End Function

End Module
