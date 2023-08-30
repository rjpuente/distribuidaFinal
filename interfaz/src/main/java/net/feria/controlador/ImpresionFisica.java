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
import net.feria.Feria;
import net.feria.TipoIdentificacion;
import static net.feria.controlador.FirmadorComprobantesElectronicos.DERECHA;
import static net.feria.controlador.FirmadorComprobantesElectronicos.IZQUIERDA;
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
public class ImpresionFisica {
    
    private final FacturasCabeceraJpaController facturasCabeceraJpaController = new FacturasCabeceraJpaController(EntityManagerFactoryObject.emf);
    private final InformacionFiscalJpaController informacionFiscalJpaController = new InformacionFiscalJpaController(EntityManagerFactoryObject.emf);
    private final ComprobanteElectronicoJpaController comprobanteElectronicoJpaController = new ComprobanteElectronicoJpaController(EntityManagerFactoryObject.emf);
    
    private static final SimpleDateFormat formatoISO = new SimpleDateFormat("yyyy-MM-dd");
    public static final int IZQUIERDA = 1;
    public static final int DERECHA = 2;
    
    
    private List<ComprobanteElectronico> listaComprobantesPorImprimir;
    private final InformacionFiscal informacionFiscal = informacionFiscalJpaController.findInformacionFiscal(1);

    public void imprimirComprobantes() {
        listaComprobantesPorImprimir = comprobanteElectronicoJpaController.obtenerComprobantesPorImprimir();

        listaComprobantesPorImprimir.forEach((comprobante) -> {
            imprimir(comprobante);
        });
    }

    public void imprimir(ComprobanteElectronico comprobanteElectronico) {
        NumberFormat nf = NumberFormat.getNumberInstance(Locale.ENGLISH);
        DecimalFormat form = (DecimalFormat) nf;
        form.applyPattern("#,##0.00");

        Feria feria = new Feria();

        ObjetoFactura objetoFactura = new ObjetoFactura();
        objetoFactura.setNumeroFactura(comprobanteElectronico.getNumeroComprobante());
        objetoFactura.setNumeroAutorizacion(comprobanteElectronico.getClaveAcceso());
        objetoFactura.setFecha(comprobanteElectronico.getFechaAutorizacion());

        FacturasCabecera facturasCabecera = facturasCabeceraJpaController.obtenerPorNumeroFactura(comprobanteElectronico
                .getNumeroComprobante());
        objetoFactura.setCodigoCliente(facturasCabecera.getCodigoCliente().getCodigoCliente().toString());
        objetoFactura.setCostoTotalSinIva(facturasCabecera.getValorTotalFactura()
                .doubleValue() - facturasCabecera.getValorDescuento()
                .doubleValue() - facturasCabecera.getIva().doubleValue());
        objetoFactura.setTotalDescuento(facturasCabecera.getValorDescuento().doubleValue());

        String tipoIdentificacion = TipoIdentificacion.convertirTipoIdentificacionDynamics(facturasCabecera.getCodigoCliente().getTipoIdentificacion());

        if (tipoIdentificacion.equals(TipoIdentificacion.TIPO_IDENTIFICACION_IDENTIFICACION_DEL_EXTERIOR) || tipoIdentificacion.equals(TipoIdentificacion.TIPO_IDENTIFICACION_VENTA_A_CONSUMIDOR_FINAL)) {
            objetoFactura.setIdentificacionTercero("9999999999999");
        } else {
            objetoFactura.setIdentificacionTercero(facturasCabecera.getCodigoCliente().getNumeroCedula());
        }

        objetoFactura.setRazonSocialTercero(facturasCabecera.getCodigoCliente().getNombre());
        objetoFactura.setNumeroCliente(facturasCabecera.getCodigoCliente().getTelefono());

        Double baseImponible12 = 0d;
        Double baseImponible0 = 0d;
        Double iva12 = 0d;

        Double valorIvaAuxiliar = facturasCabecera.getIva().doubleValue();

        if (valorIvaAuxiliar != 0) {
            iva12 = valorIvaAuxiliar;
            baseImponible12 = objetoFactura.getCostoTotalSinIva();
        } else {
            baseImponible0 = objetoFactura.getCostoTotalSinIva();
        }

        objetoFactura.setBaseImponible12(baseImponible12);
        objetoFactura.setIva12(iva12);
        objetoFactura.setBaseImponible0(baseImponible0);

        objetoFactura.setDireccion(facturasCabecera.getCodigoCliente().getDireccion());

        objetoFactura.setDetalles(obtenerDetallesDocumento(new ArrayList<>(facturasCabecera.getDetalleFacturaCollection())));

        imprimirFactura(objetoFactura, feria.getImpresoraComprobantes());
    }

    private List<ObjetoDetalleFactura> obtenerDetallesDocumento(List<DetalleFactura> listaDetalles) {
        List<ObjetoDetalleFactura> listaDetallesObjeto = new ArrayList<>();
        //obtencion de campos extras
        listaDetalles.forEach((detalle) -> {
            ObjetoDetalleFactura objetoDetalleFactura = new ObjetoDetalleFactura();
            objetoDetalleFactura.setDescripccion(detalle.getCodigoItem().getNombreItem());
            objetoDetalleFactura.setCodigoItem(detalle.getCodigoItem().getCodigoItem().toString());
            objetoDetalleFactura.setCantidad(detalle.getCantidad());
            objetoDetalleFactura.setPrecioUnitario((detalle.getCodigoItem().getPrecioItem().doubleValue() / 1.12));
            objetoDetalleFactura.setValorTotal(objetoDetalleFactura.getPrecioUnitario() * objetoDetalleFactura.getCantidad());

            listaDetallesObjeto.add(objetoDetalleFactura);
        });

        return listaDetallesObjeto;
    }

    private void imprimirFactura(ObjetoFactura objetoFactura, String nombreImpresora) {

        Ticket ticket = new Ticket();
        ticket = generarCabecera(ticket, objetoFactura);
        ticket = generarSubCabecera(ticket, objetoFactura);
        ticket = generarItems(ticket, objetoFactura);
        ticket = generarPie(ticket, objetoFactura);
        ticket = generarFin(ticket, objetoFactura);
        try {
            ticket.imprimirDocumento(nombreImpresora);
        } catch (Exception ex) {
            ticket.imprimirDocumento(nombreImpresora);
        }
    }

    private Ticket generarCabecera(Ticket ticket, ObjetoFactura objetoFactura) {
        Date fecha = new Date();
        ticket.addCabecera(ticket.centrar(informacionFiscal.getNombreComercial()));
        ticket.addCabecera(ticket.darEspacio());
        ticket.addCabecera(ticket.centrar(informacionFiscal.getRuc()));
        ticket.addCabecera(ticket.darEspacio());
        ticket.addCabecera(ticket.centrar(informacionFiscal.getDireccion()));
        ticket.addCabecera(ticket.darEspacio());
        ticket.addCabecera(ticket.centrar(Feria.CIUDAD));
        ticket.addCabecera(ticket.darEspacio());
        ticket.addCabecera(ticket.centrar(Feria.CONTABILIDAD));
        ticket.addCabecera(ticket.darEspacio());
        ticket.addCabecera(ticket.centrar(Feria.AMBIENTE));
        ticket.addCabecera(ticket.darEspacio());
        ticket.addCabecera(ticket.centrar(fecha.toString()));
        ticket.addCabecera(ticket.darEspacio());
        ticket.addCabecera(ticket.dibujarLinea(40));
        ticket.addCabecera(ticket.darEspacio());
        ticket.addCabecera(ticket.darEspacio());

        return ticket;
    }

    private Ticket generarSubCabecera(Ticket ticket, ObjetoFactura objetoFactura) {
        ticket.addSubCabecera("Factura: " + objetoFactura.getNumeroFactura());
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera("Cliente: " + objetoFactura.getRazonSocialTercero());
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera("CC/RUC: " + objetoFactura.getIdentificacionTercero());
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera("Fecha emision: " + formatoISO.format(objetoFactura.getFecha()));
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera("Direccion: " + objetoFactura.getDireccion());
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera("Tlf: " + objetoFactura.getNumeroCliente());
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera("Cod. Autorizacion: " + objetoFactura.getNumeroAutorizacion());
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera(ticket.dibujarLinea(40));
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera("CANT.  PRODUCTO   V.UNIT         V.TOTAL");
        ticket.addSubCabecera(ticket.darEspacio());
        ticket.addSubCabecera(ticket.dibujarLinea(40));
        ticket.addSubCabecera(ticket.darEspacio());

        return ticket;
    }

    private Ticket generarItems(Ticket ticket, ObjetoFactura objetoFactura) {
        for (int i = 0; i < objetoFactura.getDetalles().size(); i++) {
            Integer cantidad = objetoFactura.getDetalles().get(i).getCantidad();
            List<String> lineasItem = generarCadenaItem(cantidad, objetoFactura.getDetalles().get(i).getCodigoItem(), objetoFactura.getDetalles().get(i).getDescripccion(), objetoFactura.getDetalles().get(i).getPrecioUnitario(), objetoFactura.getDetalles().get(i).getValorTotal());
            ticket.addItemSimple(lineasItem);
            ticket.darEspacio();
            ticket.darEspacio();
        }
        ticket.addItem(ticket.dibujarLinea(40), "", "", "");
        ticket.addTotal(ticket.darEspacio());

        return ticket;
    }

    private List<String> generarCadenaItem(Integer cantidad, String codigoItem, String descripcion, Double unitario, Double total) {
        List<String> lineasDescripcion = descomponerDescripcion(descripcion, codigoItem, 33);
        List<String> ret = new ArrayList<>();
        String linea1 = cadenaAlineada(IZQUIERDA, cantidad.toString(), 6)
                + cadenaAlineada(IZQUIERDA, " ", 1)
                + cadenaAlineada(IZQUIERDA, lineasDescripcion.get(0), 33);
        ret.add(linea1);
        for (int i = 1; i < lineasDescripcion.size(); i++) {
            String linea2 = cadenaAlineada(IZQUIERDA, "", 6)
                    + cadenaAlineada(IZQUIERDA, " ", 1)
                    + cadenaAlineada(IZQUIERDA, lineasDescripcion.get(i), 33);
            ret.add(linea2);
        }
        String linea3 = cadenaAlineada(IZQUIERDA, "", 15)
                + cadenaAlineada(DERECHA, formatearDolares(unitario), 9)
                + cadenaAlineada(IZQUIERDA, " ", 2)
                + cadenaAlineada(DERECHA, formatearDolares(total), 14);
        ret.add(linea3);
        return ret;
    }

    String formatearDolares(double dolares) {
        DecimalFormat formato = new DecimalFormat("#,##0.00");
        return formato.format(dolares);
    }

    String formatearDescuento(double descuento) {
        DecimalFormat formato = new DecimalFormat("#0.0");
        return formato.format(descuento);
    }

    String formatearPorcentaje(double porcentaje) {
        DecimalFormat formato = new DecimalFormat("0");
        return formato.format(porcentaje);
    }

    private String cadenaAlineada(int alineacion, String cadena, int longitud) {
        if (cadena.length() >= longitud) {
            if (alineacion == IZQUIERDA) {
                return cadena.substring(0, longitud);
            } else if (alineacion == DERECHA) {
                return cadena.substring(cadena.length() - longitud, cadena.length());
            }
        } else {
            String espacios = cadenaEspacios(longitud - cadena.length());
            if (alineacion == IZQUIERDA) {
                return cadena + espacios;
            } else if (alineacion == DERECHA) {
                return espacios + cadena;
            }
        }
        return null;
    }

    private List<String> descomponerDescripcion(String descripcion, String codigoItem, int longitud) {
        List<String> lineas = new ArrayList<>();
        String cadena = descripcion + " (" + codigoItem + ")";
        if (cadena.length() <= longitud) {
            lineas.add(cadenaAlineada(IZQUIERDA, cadena, longitud));
        } else {
            int siguienteInicio = 0;
            String cadenaRestante = cadena.substring(siguienteInicio, cadena.length());
            while (cadenaRestante.length() > longitud) {
                lineas.add(cadenaAlineada(IZQUIERDA, cadenaRestante.substring(0, longitud), longitud));
                siguienteInicio += longitud;
                cadenaRestante = cadena.substring(siguienteInicio, cadena.length());
            }
            lineas.add(cadenaAlineada(IZQUIERDA, cadenaRestante, longitud));
        }
        return lineas;
    }

    private String cadenaEspacios(int longitud) {
        String ret = "";
        for (int i = 1; i <= longitud; i++) {
            ret += " ";
        }
        return ret;
    }

    private Ticket generarPie(Ticket ticket, ObjetoFactura objetoFactura) {
        Double porcentajeIvaN = (100 * (objetoFactura.getIva12()) / (objetoFactura.getBaseImponible12()));

        ticket.addTotal(cadenaAlineada(IZQUIERDA, "", 9) + cadenaAlineada(IZQUIERDA, "Valor:", 17) + cadenaAlineada(DERECHA, formatearDolares(objetoFactura.getCostoTotalSinIva()), 14));        
        ticket.addTotal(ticket.darEspacio());
        ticket.addTotal(cadenaAlineada(IZQUIERDA, "", 9) + cadenaAlineada(IZQUIERDA, "Subtotal:", 17) + cadenaAlineada(DERECHA, formatearDolares(objetoFactura.getBaseImponible0() + objetoFactura.getBaseImponible12()), 14));
        ticket.addTotal(ticket.darEspacio());
        ticket.addTotal(cadenaAlineada(IZQUIERDA, "", 9) + cadenaAlineada(IZQUIERDA, "Tarifa 0%:", 17) + cadenaAlineada(DERECHA, formatearDolares(objetoFactura.getBaseImponible0()), 14));
        ticket.addTotal(ticket.darEspacio());
        ticket.addTotal(cadenaAlineada(IZQUIERDA, "", 9) + cadenaAlineada(IZQUIERDA, "Tarifa " + formatearPorcentaje(porcentajeIvaN) + "%:", 17) + cadenaAlineada(DERECHA, formatearDolares(objetoFactura.getBaseImponible12()), 14));
        ticket.addTotal(ticket.darEspacio());
        ticket.addTotal(cadenaAlineada(IZQUIERDA, "", 9) + cadenaAlineada(IZQUIERDA, "IVA " + formatearPorcentaje(porcentajeIvaN) + "%: ", 17) + cadenaAlineada(DERECHA, formatearDolares(objetoFactura.getIva12()), 14));
        ticket.addTotal(ticket.darEspacio());
        ticket.addTotal(cadenaAlineada(IZQUIERDA, "", 9) + cadenaAlineada(IZQUIERDA, "TOTAL: " + formatearPorcentaje(porcentajeIvaN) + "%:", 17) + cadenaAlineada(DERECHA, formatearDolares(objetoFactura.getCostoTotal()), 14));
        ticket.addTotal(ticket.darEspacio());
        ticket.addTotal(ticket.dibujarLinea(40));
        ticket.addTotal(ticket.darEspacio());
        return ticket;
    }

    private Ticket generarFin(Ticket ticket, ObjetoFactura objetoFactura) {
        ticket.addPieLinea("Forma de Pago     Valor   Plazo   Tiempo");
        ticket.addPieLinea(ticket.darEspacio());
        Date fechaVencimiento = new Date();
        Double valorVencimiento = objetoFactura.getCostoTotal();

        String linea = "";

        String forma = "Otros con utili.";
        String texto = "Dias";

        String valor = valorVencimiento.toString();
        if (valor.length() < 8) {
            int v = 8 - valor.length();
            String val = "";
            for (int i = 0; i < v; i++) {
                val += " ";
            }
            valor = val + valor;
        }

        linea = forma + valor + 0 + "   " + texto;
        ticket.addPieLinea(linea);
        ticket.addPieLinea(ticket.darEspacio());
        ticket.addPieLinea(ticket.darEspacio());
        ticket.addPieLinea(ticket.centrar("GRACIAS POR SU COMPRA"));
        ticket.addPieLinea(ticket.darEspacio());
        return ticket;
    }

    private String contarEspaciosFinal(String pie) {
        if (pie.length() < 32) {
            int p = 32 - pie.length();
            String texto = "";
            for (int i = 0; i < p; i++) {
                texto += " ";
            }
            texto = texto + pie;
            return texto;
        }
        return pie;
    }

    private String agregarValorPie(String texto) {
        if (texto.length() < 8) {
            int v = 8 - texto.length();
            String val = "";
            for (int i = 0; i < v; i++) {
                val += " ";
            }
            texto = val + texto;
            return texto;
        }
        return texto;
    }

    private Integer convertirDias(Long mili) {
        Long segundos = mili / 1000;
        Long minutos = segundos / 60;
        Long horas = minutos / 60;
        Long diasL = (horas / 24);
        Integer dias = diasL.intValue();
        return dias;
    }
}
