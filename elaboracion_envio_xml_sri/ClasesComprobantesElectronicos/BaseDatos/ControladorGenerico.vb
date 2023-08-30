Public MustInherit Class ControladorGenerico
    Public nombreTabla As String
    Public listaCampos As String
    Public campoClave As String
    Public existe As Boolean = False

    Public MustOverride Sub insertar()
    Public MustOverride Sub obtener(valor As String)
    Public MustOverride Sub actualizar()
    Public MustOverride Sub eliminar()
End Class
