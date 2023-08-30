/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.objetos;


import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.print.PageFormat;
import java.awt.print.Printable;
import static java.awt.print.Printable.NO_SUCH_PAGE;
import static java.awt.print.Printable.PAGE_EXISTS;
import java.awt.print.PrinterException;
import java.io.Serializable;
import java.util.StringTokenizer;
import javax.print.PrintService;
import javax.print.PrintServiceLookup;

/**
 *
 * @author rpuente
 */
public class Impresion implements Printable, Serializable {

    private String texto;
    private StringTokenizer lineasDeTexto;
    private int totalLineas;
    private int[] paginas; //numero de paginas que se necesitaran para imprimir el texto
    private String[] textoLineas; //Lineas de texto a imprimir en cada hoja

    public Impresion() {
    }   
    

    public Impresion(String texto) {
        this.texto = texto;
        this.lineasDeTexto = new StringTokenizer(texto, "\n", true);;
        this.totalLineas = lineasDeTexto.countTokens();
        this.paginas = paginas;
        this.textoLineas = textoLineas;
    }

    @Override
    public int print(Graphics g, PageFormat pf, int pageIndex) throws PrinterException {
        Font font = new Font("Arial", Font.BOLD, 12);
        FontMetrics metrics = g.getFontMetrics(font);
        int altoDeLinea = metrics.getHeight();

        //Calcula el número de lineas por pagina y el total de páginas
        if (paginas == null) {
            initTextoLineas();
            //Calcula las lineas que caben en cada pagina, dividiendo la altura imprimible entre la altura de linea de texto
            int lineasPorPagina = (int) (pf.getWidth() / altoDeLinea);
            //Calcula el número de paginas, diviendo el total de lineas entre el número de lineas por página
            int numeroPaginas = (textoLineas.length - 1) / lineasPorPagina;
            paginas = new int[numeroPaginas];
            for (int i = 0; i < numeroPaginas; i++) {
                paginas[i] = (i + 1) * lineasPorPagina;
            }
        }
        //Si se recibe un indice de página mayor que el total de páginas calculadas entonces 
        //retorna NO_SUCH_PAGE para indicar que tal pagina no existe 
        if (pageIndex > paginas.length) {
            return NO_SUCH_PAGE;
        }
        /*Por lo regular cuando dibujamos algun objeto lo coloca en la coordenada (0,0), esta coordenada 
         * se encuentra fuera del área imprimible, por tal motivo se debe trasladar la posicion de las lineas de texto
         * según el área imprimible del eje X y el eje Y 
         */

        Graphics2D g2d = (Graphics2D) g;
        g2d.translate(pf.getImageableX(), pf.getImageableY());
        
        /*Dibujamos cada línea de texto en cada página,
         * se aumenta a la posición 'y' la altura de la línea a cada línea de texto para evitar la saturación de texto 
         */

        int y = 0;
        int start = (pageIndex == 0) ? 0 : paginas[pageIndex - 1];
        int end = (pageIndex == paginas.length) ? textoLineas.length : paginas[pageIndex];
        for (int line = start; line < end; line++) {
            y += altoDeLinea;
            g.drawString(textoLineas[line], 0, y);
        }
        /* Retorna PAGE_EXISTS para indicar al invocador que esta página es parte del documento impreso
         */
        return PAGE_EXISTS;
    }

    private void initTextoLineas() {
        if (textoLineas == null) {
            int numLineas = totalLineas;
            textoLineas = new String[numLineas];
            //Se llena el arreglo que contiene todas las lineas de texto
            while (lineasDeTexto.hasMoreTokens()) {
                for (int i = 0; i < numLineas; i++) {
                    textoLineas[i] = lineasDeTexto.nextToken();
                }
            }
        }
    }

    

    public PrintService encontrarImrpesora(String nombreImpresora) {
        PrintService[] printServices = PrintServiceLookup.lookupPrintServices(null, null);
        for (PrintService printService : printServices) {

            if (printService.getName().trim().equals(nombreImpresora)) {
                return printService;
            }
        }
        return null;
    }

    /*
    Getter´s y Setter´s
     */
    public String getTexto() {
        return texto;
    }

    public void setTexto(String texto) {
        this.texto = texto;
    }

    public StringTokenizer getLineasDeTexto() {
        return lineasDeTexto;
    }

    public void setLineasDeTexto(StringTokenizer lineasDeTexto) {
        this.lineasDeTexto = lineasDeTexto;
    }

    public int getTotalLineas() {
        return totalLineas;
    }

    public void setTotalLineas(int totalLineas) {
        this.totalLineas = totalLineas;
    }

    public int[] getPaginas() {
        return paginas;
    }

    public void setPaginas(int[] paginas) {
        this.paginas = paginas;
    }

    public String[] getTextoLineas() {
        return textoLineas;
    }

    public void setTextoLineas(String[] textoLineas) {
        this.textoLineas = textoLineas;
    }

}
