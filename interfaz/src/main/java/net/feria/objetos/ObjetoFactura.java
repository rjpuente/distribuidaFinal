/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.objetos;

import java.io.Serializable;
import java.util.Date;
import java.util.List;

/**
 *
 * @author Deth1
 */
public class ObjetoFactura implements Serializable {

    private String numeroFactura;
    //datos del cliente
    private String codigoCliente;
    private String numeroCliente;
    private String direccion;
    private Date fecha;
    private String numeroAutorizacion;
    private List<ObjetoDetalleFactura> detalles;
    private Double costoTotalSinIva;
    private Double costoTotal;
    private Double descuentoCabecera;
    private Double descuentoPie;
    private Double totalDescuento;
    private String identificacionTercero;
    private String razonSocialTercero;
    private Double baseImponible12;
    private Double baseImponible0;
    private Double iva12;
    private String codigoSeguimiento;
    private Double totalVentasBienes;

    public ObjetoFactura() {
    }
    
    public Double getCostoTotal() {
        return (costoTotalSinIva + iva12);
    }

    /*
     Getter´s y Setter´s
     */
    public String getNumeroFactura() {
        return numeroFactura;
    }

    public void setNumeroFactura(String numeroFactura) {
        this.numeroFactura = numeroFactura;
    }

    public List<ObjetoDetalleFactura> getDetalles() {
        return detalles;
    }

    public void setDetalles(List<ObjetoDetalleFactura> detalles) {
        this.detalles = detalles;
    }

    public String getNumeroCliente() {
        return numeroCliente;
    }

    public void setNumeroCliente(String numeroCliente) {
        this.numeroCliente = numeroCliente;
    }

    public Double getCostoTotalSinIva() {
        return costoTotalSinIva;
    }

    public void setCostoTotalSinIva(Double costoTotalSinIva) {
        this.costoTotalSinIva = costoTotalSinIva;
    }

    public void setCostoTotal(Double costoTotal) {
        this.costoTotal = costoTotal;
    }

    public String getDireccion() {
        return direccion;
    }

    public void setDireccion(String direccion) {
        this.direccion = direccion;
    }

    public Date getFecha() {
        return fecha;
    }

    public void setFecha(Date fecha) {
        this.fecha = fecha;
    }

    public String getNumeroAutorizacion() {
        return numeroAutorizacion;
    }

    public void setNumeroAutorizacion(String numeroAutorizacion) {
        this.numeroAutorizacion = numeroAutorizacion;
    }

    public String getCodigoCliente() {
        return codigoCliente;
    }

    public void setCodigoCliente(String codigoCliente) {
        this.codigoCliente = codigoCliente;
    }

    public Double getDescuentoCabecera() {
        return descuentoCabecera;
    }

    public void setDescuentoCabecera(Double descuentoCabecera) {
        this.descuentoCabecera = descuentoCabecera;
    }

    public Double getDescuentoPie() {
        return descuentoPie;
    }

    public void setDescuentoPie(Double descuentoPie) {
        this.descuentoPie = descuentoPie;
    }

    public Double getTotalDescuento() {
        return totalDescuento;
    }

    public void setTotalDescuento(Double totalDescuento) {
        this.totalDescuento = totalDescuento;
    }

    public String getIdentificacionTercero() {
        return identificacionTercero;
    }

    public void setIdentificacionTercero(String identificacionTercero) {
        this.identificacionTercero = identificacionTercero;
    }

    public String getRazonSocialTercero() {
        return razonSocialTercero;
    }

    public void setRazonSocialTercero(String razonSocialTercero) {
        this.razonSocialTercero = razonSocialTercero;
    }

    public Double getBaseImponible12() {
        return baseImponible12;
    }

    public void setBaseImponible12(Double baseImponible12) {
        this.baseImponible12 = baseImponible12;
    }

    public Double getBaseImponible0() {
        return baseImponible0;
    }

    public void setBaseImponible0(Double baseImponible0) {
        this.baseImponible0 = baseImponible0;
    }

    public Double getIva12() {
        return iva12;
    }

    public void setIva12(Double iva12) {
        this.iva12 = iva12;
    }

    public String getCodigoSeguimiento() {
        return codigoSeguimiento;
    }

    public void setCodigoSeguimiento(String codigoSeguimiento) {
        this.codigoSeguimiento = codigoSeguimiento;
    }

    public Double getTotalVentasBienes() {
        return totalVentasBienes;
    }

    public void setTotalVentasBienes(Double totalVentasBienes) {
        this.totalVentasBienes = totalVentasBienes;
    }
}
