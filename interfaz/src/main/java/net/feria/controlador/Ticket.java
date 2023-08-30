/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.controlador;


import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import javax.print.Doc;
import javax.print.DocFlavor;
import javax.print.DocPrintJob;
import javax.print.PrintService;
import javax.print.SimpleDoc;
import javax.swing.JOptionPane;
import net.feria.Feria;
import net.feria.objetos.Impresion;
import net.feria.objetos.OrderItem;

/**
 *
 * @author rpuente
 */
public class Ticket {
    
    private static List<String> cabezaLineas = new ArrayList<>();
    private static List<String> subCabezaLineas = new ArrayList<>();
    private static List<String> items = new ArrayList();
    private static List<String> totales = new ArrayList<>();
    private static List<String> lineasPie = new ArrayList<>();
    
    public static void addCabecera(String linea) {
        cabezaLineas.add(linea);
    }
    
    public static void addSubCabecera(String linea) {
        subCabezaLineas.add(linea);
    }
    
    public static void addItem(String cantidad, String item, String precio, String total) {
        OrderItem nuevoItem = new OrderItem(' ');
        items.add(nuevoItem.generaItem(cantidad, item, precio, total));
    }
    
    public static void addItemSimple(List<String> lineasItem) {
        for (String linea : lineasItem) {
            items.add(linea);
            items.add(darEspacio());
        }
        items.add(darEspacio());
    }
    
    public static void addItemCodigoSerie(String numeroSerie) {
        items.add(numeroSerie);
    }
    
    public static void addTotal(String linea) {
        totales.add(linea);
    }
    
    public static void addPieLinea(String linea) {
        lineasPie.add(linea);
    }
    
    public static String dibujarLinea(int valor) {
        String raya = "";
        
        for (int i = 0; i < valor; i++) {
            raya += "-";
        }
        return raya;
    }
    
    public static String centrar(String texto) {
        String centro = "";
        String blanco = " ";
        int palabraMitad = texto.length() / 2;
        int centroInferior = 20 - palabraMitad;
        int centroSuperior = 21 + palabraMitad;
        int j = 0;
        for (int i = 0; i < 40; i++) {
            if (i < centroInferior) {
                centro += blanco;
            } else if (i >= centroSuperior) {
                centro += blanco;
            } else {
                try {
                    centro += texto.charAt(j);
                    j++;
                } catch (Exception ex) {
                    centro += blanco;
                }
            }
        }
        return centro;
    }
    
    public static String darEspacio() {
        return "\n";
    }
    
    public static void setFormato(FileWriter pw, int formato) {
        try {
            char[] ESC_CUT_PAPER = new char[]{0x1B, '!', (char) formato};
            pw.write(ESC_CUT_PAPER);
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }
    
    public static void imprmirDocumento(String impresora, boolean abrir) {
        try {
            FileWriter imp = new FileWriter(impresora);
            char[] Caracter = new char[]{0x1B, 'R', 18};
            imp.write(Caracter);
            for (int cabecera = 0; cabecera < cabezaLineas.size(); cabecera++) {
                imp.write(cabezaLineas.get(cabecera));
            }
            for (int subCabecera = 0; subCabecera < subCabezaLineas.size(); subCabecera++) {
                imp.write(subCabezaLineas.get(subCabecera));
            }
            for (int item = 0; item < items.size(); item++) {
                imp.write(items.get(item));
            }
            for (int total = 0; total < totales.size(); total++) {
                imp.write(totales.get(total));
            }
            for (int pie = 0; pie < lineasPie.size(); pie++) {
                imp.write(lineasPie.get(pie));
            }
            for (int u = 0; u <= 10; u++) {
                imp.write("\n");
            }

            //Corte de papel 
            char[] CORTAR_PAPEL = new char[]{0x1B, 'm'};
            imp.write(CORTAR_PAPEL);
            
            imp.close();

            //Limpieza de listas
            limpiar();
            
        } catch (IOException ioe) {
            ioe.printStackTrace();
            JOptionPane.showMessageDialog(null, "Error al impimir: \n" + ioe.getMessage());
            limpiar();
        }
    }
    
    public static void imprimirDocumento(String nombreImpresora) {        
        try {
            Feria feria = new Feria();
            Impresion i = new Impresion();
            String cadena = "";
            for (int cabecera = 0; cabecera < cabezaLineas.size(); cabecera++) {
                cadena += cabezaLineas.get(cabecera);
            }
            for (int subCabecera = 0; subCabecera < subCabezaLineas.size(); subCabecera++) {
                cadena += subCabezaLineas.get(subCabecera);
            }
            for (int item = 0; item < items.size(); item++) {
                cadena += items.get(item);
            }
            for (int total = 0; total < totales.size(); total++) {
                cadena += totales.get(total);
            }
            for (int pie = 0; pie < lineasPie.size(); pie++) {
                cadena += lineasPie.get(pie);
            }
            for (int u = 0; u <= 5; u++) {
                cadena += "\n";
            }
            
            if (Feria.EN_PRODUCCION) {
                DocFlavor flavor = DocFlavor.BYTE_ARRAY.AUTOSENSE;
                PrintService impresora;
                try {
                    if (nombreImpresora.equals(feria.getImpresoraComprobantes())) {
                        impresora = i.encontrarImrpesora(nombreImpresora);
                        DocPrintJob pj = impresora.createPrintJob();
                        byte[] bytes = cadena.getBytes();
                        Doc doc = new SimpleDoc(bytes, flavor, null);
                        try {
                            pj.print(doc, null);
                            limpiar();
                        } catch (Exception ex) {
                            ex.printStackTrace();
                            limpiar();
                        }
                    }else{
                        nombreImpresora = feria.getImpresoraComprobantes();
                        imprimirDocumento(nombreImpresora);
                    }
                } catch (Exception ex) {
                    ex.printStackTrace();
                }
                
            } else {
                System.out.println(cadena);
            }
        } catch (Exception ex) {
            imprimirDocumento(nombreImpresora);
        }
        
    }
    
    private static void limpiar() {
        cabezaLineas.removeAll(cabezaLineas);
        subCabezaLineas.removeAll(subCabezaLineas);
        items.removeAll(items);
        totales.removeAll(totales);
        lineasPie.removeAll(lineasPie);
    }
}
