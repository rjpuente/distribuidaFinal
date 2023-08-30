/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

/**
 *
 * @author rpuente
 */
public class Feria {

    public static final String CONTRINUYENTE_ESPECIAL = "Contribuyente especial # 194";
    public static final String AMBIENTE = "Ambiente: Produccion";
    public static final String CONTABILIDAD = "Obligado a llevar contabilidad: No";
    private String TELEFONO;
    public static final String CIUDAD = "Quito - Ecuador";
    public static final File FILE = new File("configuracionImpresoras.txt");
    public static final Integer apagar = 0;
    public static final Boolean imprimir = true;
    private String impresoraComprobantes;
    private boolean imprimiendo = true;
    public static final boolean EN_PRODUCCION = true;

    public Feria() {
        try {
            Scanner leer = new Scanner(FILE);
            while (leer.hasNextLine()) {
                String linea = leer.nextLine();
                Scanner delimitar = new Scanner(linea);
                /*
                 * Usando una expresion regular que valida que exista algo, antes o despues de una coma
                 */
                delimitar.useDelimiter("\\s*,\\s*");
                this.impresoraComprobantes = (delimitar.next());
            }
        } catch (FileNotFoundException fnfe) {
            fnfe.printStackTrace();
        }
    }

    public String getImpresoraComprobantes() {
        return impresoraComprobantes;
    }

    public void setImpresoraComprobantes(String impresoraComprobantes) {
        this.impresoraComprobantes = impresoraComprobantes;
    }

    public boolean isImprimiendo() {
        return imprimiendo;
    }

    public void setImprimiendo(boolean imprimiendo) {
        this.imprimiendo = imprimiendo;
    }

    public String getTELEFONO() {
        return TELEFONO;
    }

    public void setTELEFONO(String TELEFONO) {
        this.TELEFONO = TELEFONO;
    }

}
