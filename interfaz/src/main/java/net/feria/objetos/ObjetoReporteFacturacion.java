/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.objetos;

import java.io.Serializable;

/**
 *
 * @author Deth1
 */
public class ObjetoReporteFacturacion implements Serializable {

    private String numeroFactura;
    private String cliente;
    private String fecha;
    private Double valorSinIva;
    private Double valorFactura;
    private Double valorIva;

    public ObjetoReporteFacturacion() {
    }

    /*
     * Getter's y Setter's
     */
    public String getNumeroFactura() {
        return numeroFactura;
    }

    public void setNumeroFactura(String numeroFactura) {
        this.numeroFactura = numeroFactura;
    }

    public String getCliente() {
        return cliente;
    }

    public void setCliente(String cliente) {
        this.cliente = cliente;
    }

    public String getFecha() {
        return fecha;
    }

    public void setFecha(String fecha) {
        this.fecha = fecha;
    }

    public Double getValorFactura() {
        return valorFactura;
    }

    public void setValorFactura(Double valorFactura) {
        this.valorFactura = valorFactura;
    }

    public Double getValorIva() {
        return valorIva;
    }

    public void setValorIva(Double valorIva) {
        this.valorIva = valorIva;
    }

    public Double getValorSinIva() {
        return valorSinIva;
    }

    public void setValorSinIva(Double valorSinIva) {
        this.valorSinIva = valorSinIva;
    }

}
