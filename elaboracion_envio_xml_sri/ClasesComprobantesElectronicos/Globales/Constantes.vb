﻿Module Constantes
    Public Const TIPO_DE_AMBIENTE_PRUEBAS As Integer = 1
    Public Const TIPO_DE_AMBIENTE_PRODUCCION As Integer = 2

    Public Const TIPO_DE_EMISION_NORMAL = 1
    Public Const TIPO_DE_EMISION_INDISPONIBILIDAD_SISTEMA = 2

    Public Const TIPO_DE_COMPROBANTE_FACTURA = "01"
    Public Const TIPO_DE_COMPROBANTE_NOTA_DE_CREDITO = "04"
    Public Const TIPO_DE_COMPROBANTE_NOTA_DE_DEBITO = "05"
    Public Const TIPO_DE_COMPROBANTE_GUIA_DE_REMISION = "06"
	Public Const TIPO_DE_COMPROBANTE_COMPROBANTE_DE_RETENCION = "07"
	Public Const TIPO_DE_COMPROBANTE_LIQUIDACION_COMPRA = "03"

	Public Const TIPO_IDENTIFICACION_RUC = "04"
    Public Const TIPO_IDENTIFICACION_CEDULA = "05"
    Public Const TIPO_IDENTIFICACION_PASAPORTE = "06"
    Public Const TIPO_IDENTIFICACION_VENTA_A_CONSUMIDOR_FINAL = "07"
    Public Const TIPO_IDENTIFICACION_IDENTIFICACION_DEL_EXTERIOR = "08"
    Public Const TIPO_IDENTIFICACION_PLACA = "09"

    Public Const CODIGO_IMPUESTO_IVA = 2
    Public Const CODIGO_IMPUESTO_ICE = 3
    Public Const CODIGO_IMPUESTO_IRBPNR = 5

    Public Const TARIFA_IVA_0 = 0
    Public Const TARIFA_IVA_12 = 2
    Public Const TARIFA_IVA_14 = 3

    Public Const IMPUESTO_POR_RETENER_RENTA = 1
    Public Const IMPUESTO_POR_RETENER_IVA = 2
    Public Const IMPUESTO_POR_RETENER_ISD = 6

    Public Const PORCENTAJE_RETENCION_IVA_10 = 9
    Public Const PORCENTAJE_RETENCION_IVA_20 = 10
    Public Const PORCENTAJE_RETENCION_IVA_30 = 1
    Public Const PORCENTAJE_RETENCION_IVA_50 = 11
    Public Const PORCENTAJE_RETENCION_IVA_70 = 2
    Public Const PORCENTAJE_RETENCION_IVA_100 = 3
    Public Const PORCENTAJE_RETENCION_IVA_0 = 7

    'Transiciones 
    Public Const TRANSICION_PROCESO_INICIADO = 0
    Public Const TRANSICION_XML_GENERADO_POR_FIRMAR = 7
    Public Const TRANSICION_XML_GENERADO = 3
    Public Const TRANSICION_XML_ACEPTADO = 4
    Public Const TRANSICION_NUMERO_AUTORIZACION = 5
    Public Const TRANSICION_RIDE_AUTORIZADO = 6
    Public Const TRANSICION_LISTO_PARA_ENVIAR = 6
End Module