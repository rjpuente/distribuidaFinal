/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria;

/**
 *
 * @author Deth1
 */
public class TipoIdentificacion {

    public static final char RUC = 'R';
    public static final char CEDULA = 'C';
    public static final char IDENTIFICACION_EXTERIOR = 'O';
    public static final char PASAPORTE = 'P';

    public static final String TIPO_IDENTIFICACION_RUC = "04";
    public static final String TIPO_IDENTIFICACION_CEDULA = "05";
    public static final String TIPO_IDENTIFICACION_PASAPORTE = "06";
    public static final String TIPO_IDENTIFICACION_VENTA_A_CONSUMIDOR_FINAL = "07";
    public static final String TIPO_IDENTIFICACION_IDENTIFICACION_DEL_EXTERIOR = "08";
    public static final String TIPO_IDENTIFICACION_PLACA = "09";

    private char codigo;

    public TipoIdentificacion(char codigo) {
        this.codigo = codigo;
    }

    public static String convertirTipoIdentificacionDynamics(char codigo) {
        if (codigo == RUC) {
            return TIPO_IDENTIFICACION_RUC;
        } else if (codigo == CEDULA) {
            return TIPO_IDENTIFICACION_CEDULA;
        } else if (codigo == IDENTIFICACION_EXTERIOR) {
            return TIPO_IDENTIFICACION_IDENTIFICACION_DEL_EXTERIOR;
        } else if (codigo == PASAPORTE) {
            return TIPO_IDENTIFICACION_PASAPORTE;
        } else {
            return "";
        }
    }

    /*
     Getter´s y Setter´s
     */
    public char getCodigo() {
        return codigo;
    }

    public void setCodigo(char codigo) {
        this.codigo = codigo;
    }

}
