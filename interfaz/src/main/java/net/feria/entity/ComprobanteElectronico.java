/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.entity;

import java.io.Serializable;
import java.util.Date;
import javax.persistence.Basic;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.NamedQueries;
import javax.persistence.NamedQuery;
import javax.persistence.Table;
import javax.persistence.Temporal;
import javax.persistence.TemporalType;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Deth1
 */
@Entity
@Table(name = "comprobante_electronico")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "ComprobanteElectronico.findAll", query = "SELECT c FROM ComprobanteElectronico c"),
    @NamedQuery(name = "ComprobanteElectronico.findByNumeroComprobante", query = "SELECT c FROM ComprobanteElectronico c WHERE c.numeroComprobante = :numeroComprobante"),
    @NamedQuery(name = "ComprobanteElectronico.findByClaveAcceso", query = "SELECT c FROM ComprobanteElectronico c WHERE c.claveAcceso = :claveAcceso"),
    @NamedQuery(name = "ComprobanteElectronico.findByRuc", query = "SELECT c FROM ComprobanteElectronico c WHERE c.ruc = :ruc"),
    @NamedQuery(name = "ComprobanteElectronico.findByFechaInicioProceso", query = "SELECT c FROM ComprobanteElectronico c WHERE c.fechaInicioProceso = :fechaInicioProceso"),
    @NamedQuery(name = "ComprobanteElectronico.findByFechaGeneracionXml", query = "SELECT c FROM ComprobanteElectronico c WHERE c.fechaGeneracionXml = :fechaGeneracionXml"),
    @NamedQuery(name = "ComprobanteElectronico.findByFechaFirmaXml", query = "SELECT c FROM ComprobanteElectronico c WHERE c.fechaFirmaXml = :fechaFirmaXml"),
    @NamedQuery(name = "ComprobanteElectronico.findByFechaAutorizacion", query = "SELECT c FROM ComprobanteElectronico c WHERE c.fechaAutorizacion = :fechaAutorizacion"),
    @NamedQuery(name = "ComprobanteElectronico.findByNumeroAutorizacionSri", query = "SELECT c FROM ComprobanteElectronico c WHERE c.numeroAutorizacionSri = :numeroAutorizacionSri"),
    @NamedQuery(name = "ComprobanteElectronico.findByNumeroIntentosTransicionAutorizacion", query = "SELECT c FROM ComprobanteElectronico c WHERE c.numeroIntentosTransicionAutorizacion = :numeroIntentosTransicionAutorizacion"),
    @NamedQuery(name = "ComprobanteElectronico.findByTransicionAutorizacion", query = "SELECT c FROM ComprobanteElectronico c WHERE c.transicionAutorizacion = :transicionAutorizacion"),
    @NamedQuery(name = "ComprobanteElectronico.findByMensajeErrorAutorizacion", query = "SELECT c FROM ComprobanteElectronico c WHERE c.mensajeErrorAutorizacion = :mensajeErrorAutorizacion"),
    @NamedQuery(name = "ComprobanteElectronico.findByAnulado", query = "SELECT c FROM ComprobanteElectronico c WHERE c.anulado = :anulado"),
    @NamedQuery(name = "ComprobanteElectronico.findByFirmado", query = "SELECT c FROM ComprobanteElectronico c WHERE c.firmado = :firmado")})
public class ComprobanteElectronico implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @Basic(optional = false)
    @Column(name = "numero_comprobante")
    private String numeroComprobante;
    @Column(name = "clave_acceso")
    private String claveAcceso;
    @Basic(optional = false)
    @Column(name = "ruc")
    private String ruc;
    @Column(name = "fecha_inicio_proceso")
    @Temporal(TemporalType.DATE)
    private Date fechaInicioProceso;
    @Column(name = "fecha_generacion_xml")
    @Temporal(TemporalType.DATE)
    private Date fechaGeneracionXml;
    @Column(name = "fecha_firma_xml")
    @Temporal(TemporalType.DATE)
    private Date fechaFirmaXml;
    @Column(name = "fecha_autorizacion")
    @Temporal(TemporalType.DATE)
    private Date fechaAutorizacion;
    @Column(name = "numero_autorizacion_sri")
    private String numeroAutorizacionSri;
    @Column(name = "numero_intentos_transicion_autorizacion")
    private Integer numeroIntentosTransicionAutorizacion;
    @Column(name = "transicion_autorizacion")
    private Integer transicionAutorizacion;
    @Column(name = "mensaje_error_autorizacion")
    private String mensajeErrorAutorizacion;
    @Column(name = "anulado")
    private Integer anulado;
    @Column(name = "firmado")
    private Boolean firmado;

    public ComprobanteElectronico() {
    }

    public ComprobanteElectronico(String numeroComprobante) {
        this.numeroComprobante = numeroComprobante;
    }

    public ComprobanteElectronico(String numeroComprobante, String ruc) {
        this.numeroComprobante = numeroComprobante;
        this.ruc = ruc;
    }

    public String getNumeroComprobante() {
        return numeroComprobante;
    }

    public void setNumeroComprobante(String numeroComprobante) {
        this.numeroComprobante = numeroComprobante;
    }

    public String getClaveAcceso() {
        return claveAcceso;
    }

    public void setClaveAcceso(String claveAcceso) {
        this.claveAcceso = claveAcceso;
    }

    public String getRuc() {
        return ruc;
    }

    public void setRuc(String ruc) {
        this.ruc = ruc;
    }

    public Date getFechaInicioProceso() {
        return fechaInicioProceso;
    }

    public void setFechaInicioProceso(Date fechaInicioProceso) {
        this.fechaInicioProceso = fechaInicioProceso;
    }

    public Date getFechaGeneracionXml() {
        return fechaGeneracionXml;
    }

    public void setFechaGeneracionXml(Date fechaGeneracionXml) {
        this.fechaGeneracionXml = fechaGeneracionXml;
    }

    public Date getFechaFirmaXml() {
        return fechaFirmaXml;
    }

    public void setFechaFirmaXml(Date fechaFirmaXml) {
        this.fechaFirmaXml = fechaFirmaXml;
    }

    public Date getFechaAutorizacion() {
        return fechaAutorizacion;
    }

    public void setFechaAutorizacion(Date fechaAutorizacion) {
        this.fechaAutorizacion = fechaAutorizacion;
    }

    public String getNumeroAutorizacionSri() {
        return numeroAutorizacionSri;
    }

    public void setNumeroAutorizacionSri(String numeroAutorizacionSri) {
        this.numeroAutorizacionSri = numeroAutorizacionSri;
    }

    public Integer getNumeroIntentosTransicionAutorizacion() {
        return numeroIntentosTransicionAutorizacion;
    }

    public void setNumeroIntentosTransicionAutorizacion(Integer numeroIntentosTransicionAutorizacion) {
        this.numeroIntentosTransicionAutorizacion = numeroIntentosTransicionAutorizacion;
    }

    public Integer getTransicionAutorizacion() {
        return transicionAutorizacion;
    }

    public void setTransicionAutorizacion(Integer transicionAutorizacion) {
        this.transicionAutorizacion = transicionAutorizacion;
    }

    public String getMensajeErrorAutorizacion() {
        return mensajeErrorAutorizacion;
    }

    public void setMensajeErrorAutorizacion(String mensajeErrorAutorizacion) {
        this.mensajeErrorAutorizacion = mensajeErrorAutorizacion;
    }

    public Integer getAnulado() {
        return anulado;
    }

    public void setAnulado(Integer anulado) {
        this.anulado = anulado;
    }

    public Boolean getFirmado() {
        return firmado;
    }

    public void setFirmado(Boolean firmado) {
        this.firmado = firmado;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (numeroComprobante != null ? numeroComprobante.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof ComprobanteElectronico)) {
            return false;
        }
        ComprobanteElectronico other = (ComprobanteElectronico) object;
        if ((this.numeroComprobante == null && other.numeroComprobante != null) || (this.numeroComprobante != null && !this.numeroComprobante.equals(other.numeroComprobante))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "net.feria.entity.ComprobanteElectronico[ numeroComprobante=" + numeroComprobante + " ]";
    }
    
}
