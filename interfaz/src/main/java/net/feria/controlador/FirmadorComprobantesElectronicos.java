/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.controlador;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimerTask;
import net.feria.Feria;
import net.feria.TipoIdentificacion;
import net.feria.ejb.ComprobanteElectronicoJpaController;
import net.feria.ejb.FacturasCabeceraJpaController;
import net.feria.ejb.InformacionFiscalJpaController;
import net.feria.entity.ComprobanteElectronico;
import net.feria.entity.DetalleFactura;
import net.feria.entity.FacturasCabecera;
import net.feria.entity.InformacionFiscal;
import net.feria.objetos.ObjetoDetalleFactura;
import net.feria.objetos.ObjetoFactura;

/**
 *
 * @author Deth1
 */
public class FirmadorComprobantesElectronicos extends TimerTask {

    private final ComprobanteElectronicoJpaController comprobanteElectronicoJpaController = new ComprobanteElectronicoJpaController(EntityManagerFactoryObject.emf);

    private static final String PATH_FIRMADOR = "C:\\Comprobantes\\Firmador\\FirmadoXML.jar";
    private static final String PATH_COMPROBANTES_SIN_FIRMA = "C:\\Comprobantes\\XML\\";
    private static final String PATH_FIRMA_ELECTRONICA = "C:\\Comprobantes\\Firmador\\firma.p12";
    private static final String CONTRASENA_FIRMA_ELECTRONICA = "clave_firma";
    private static final String PATH_COMPROBANTES_CON_FIRMA = "C:\\Comprobantes\\XMLFirmados\\";
    private static final SimpleDateFormat formatoISO = new SimpleDateFormat("yyyy-MM-dd");
    public static final int IZQUIERDA = 1;
    public static final int DERECHA = 2;

    private List<ComprobanteElectronico> listaComprobantesPorFirmar;

    @Override
    public void run() {
        verificarComprobantesFirmar();
    }

    private void verificarComprobantesFirmar() {
        listaComprobantesPorFirmar = comprobanteElectronicoJpaController.obtenerComprobantesPorFirmar();

        listaComprobantesPorFirmar.forEach((comprobante) -> {
            if (!firmar(comprobante)) {
                System.out.println("No se pudo firmar el comprobante " + comprobante.getNumeroComprobante());;
            }
        });
    }

    private boolean firmar(ComprobanteElectronico comprobante) {
        String nombreArchivo = comprobante.getNumeroComprobante() + ".xml";
        String inputXmlPath = PATH_COMPROBANTES_SIN_FIRMA + "\\" + nombreArchivo;
        String comando = "";
        comando += "java.exe -jar " + PATH_FIRMADOR;
        comando += " " + inputXmlPath;
        comando += " " + PATH_FIRMA_ELECTRONICA;
        comando += " " + CONTRASENA_FIRMA_ELECTRONICA;
        comando += " " + PATH_COMPROBANTES_CON_FIRMA;
        comando += " " + nombreArchivo;

        try {
            Process proc = Runtime.getRuntime().exec(comando);
            InputStream in = proc.getInputStream();
            InputStream err = proc.getErrorStream();
            String respuestaNormal = convertStreamToString(in);
            String respuestaError = convertStreamToString(err);
            if (!respuestaNormal.equals("")) {
                comprobante.setTransicionAutorizacion(3);
                comprobante.setFirmado(true);
                try {
                    comprobanteElectronicoJpaController.edit(comprobante);
                } catch (Exception ex) {
                    System.out.println(ex.getMessage());
                }
                return true;
            } else if (!respuestaError.equals("")) {
                return false;
            } else {
                return false;
            }
        } catch (IOException ex) {
            System.out.println(ex.getMessage());
            return false;
        }
    }

    private String convertStreamToString(InputStream is) {
        BufferedReader reader = new BufferedReader(new InputStreamReader(is));
        StringBuilder sb = new StringBuilder();

        String line = null;
        try {
            while ((line = reader.readLine()) != null) {
                sb.append(line).append('\n');
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                is.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        return sb.toString();
    }

    

}
