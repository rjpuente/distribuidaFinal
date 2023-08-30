/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.entity;

import java.io.Serializable;
import javax.persistence.Basic;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.NamedQueries;
import javax.persistence.NamedQuery;
import javax.persistence.Table;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Deth1
 */
@Entity
@Table(name = "informacion_fiscal")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "InformacionFiscal.findAll", query = "SELECT i FROM InformacionFiscal i"),
    @NamedQuery(name = "InformacionFiscal.findByCodigoInformacionFiscal", query = "SELECT i FROM InformacionFiscal i WHERE i.codigoInformacionFiscal = :codigoInformacionFiscal"),
    @NamedQuery(name = "InformacionFiscal.findByNombreComercial", query = "SELECT i FROM InformacionFiscal i WHERE i.nombreComercial = :nombreComercial"),
    @NamedQuery(name = "InformacionFiscal.findByRuc", query = "SELECT i FROM InformacionFiscal i WHERE i.ruc = :ruc"),
    @NamedQuery(name = "InformacionFiscal.findByDireccion", query = "SELECT i FROM InformacionFiscal i WHERE i.direccion = :direccion"),
    @NamedQuery(name = "InformacionFiscal.findByObligadoLlevarContabilidad", query = "SELECT i FROM InformacionFiscal i WHERE i.obligadoLlevarContabilidad = :obligadoLlevarContabilidad"),
    @NamedQuery(name = "InformacionFiscal.findByTipoContribuyente", query = "SELECT i FROM InformacionFiscal i WHERE i.tipoContribuyente = :tipoContribuyente"),
    @NamedQuery(name = "InformacionFiscal.findByNombreSocial", query = "SELECT i FROM InformacionFiscal i WHERE i.nombreSocial = :nombreSocial")})
public class InformacionFiscal implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "codigo_informacion_fiscal")
    private Integer codigoInformacionFiscal;
    @Basic(optional = false)
    @Column(name = "nombre_comercial")
    private String nombreComercial;
    @Basic(optional = false)
    @Column(name = "ruc")
    private String ruc;
    @Basic(optional = false)
    @Column(name = "direccion")
    private String direccion;
    @Basic(optional = false)
    @Column(name = "obligado_llevar_contabilidad")
    private Character obligadoLlevarContabilidad;
    @Basic(optional = false)
    @Column(name = "tipo_contribuyente")
    private Character tipoContribuyente;
    @Basic(optional = false)
    @Column(name = "nombre_social")
    private String nombreSocial;

    public InformacionFiscal() {
    }

    public InformacionFiscal(Integer codigoInformacionFiscal) {
        this.codigoInformacionFiscal = codigoInformacionFiscal;
    }

    public InformacionFiscal(Integer codigoInformacionFiscal, String nombreComercial, String ruc, String direccion, Character obligadoLlevarContabilidad, Character tipoContribuyente, String nombreSocial) {
        this.codigoInformacionFiscal = codigoInformacionFiscal;
        this.nombreComercial = nombreComercial;
        this.ruc = ruc;
        this.direccion = direccion;
        this.obligadoLlevarContabilidad = obligadoLlevarContabilidad;
        this.tipoContribuyente = tipoContribuyente;
        this.nombreSocial = nombreSocial;
    }

    public Integer getCodigoInformacionFiscal() {
        return codigoInformacionFiscal;
    }

    public void setCodigoInformacionFiscal(Integer codigoInformacionFiscal) {
        this.codigoInformacionFiscal = codigoInformacionFiscal;
    }

    public String getNombreComercial() {
        return nombreComercial;
    }

    public void setNombreComercial(String nombreComercial) {
        this.nombreComercial = nombreComercial;
    }

    public String getRuc() {
        return ruc;
    }

    public void setRuc(String ruc) {
        this.ruc = ruc;
    }

    public String getDireccion() {
        return direccion;
    }

    public void setDireccion(String direccion) {
        this.direccion = direccion;
    }

    public Character getObligadoLlevarContabilidad() {
        return obligadoLlevarContabilidad;
    }

    public void setObligadoLlevarContabilidad(Character obligadoLlevarContabilidad) {
        this.obligadoLlevarContabilidad = obligadoLlevarContabilidad;
    }

    public Character getTipoContribuyente() {
        return tipoContribuyente;
    }

    public void setTipoContribuyente(Character tipoContribuyente) {
        this.tipoContribuyente = tipoContribuyente;
    }

    public String getNombreSocial() {
        return nombreSocial;
    }

    public void setNombreSocial(String nombreSocial) {
        this.nombreSocial = nombreSocial;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (codigoInformacionFiscal != null ? codigoInformacionFiscal.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof InformacionFiscal)) {
            return false;
        }
        InformacionFiscal other = (InformacionFiscal) object;
        if ((this.codigoInformacionFiscal == null && other.codigoInformacionFiscal != null) || (this.codigoInformacionFiscal != null && !this.codigoInformacionFiscal.equals(other.codigoInformacionFiscal))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "net.feria.entity.InformacionFiscal[ codigoInformacionFiscal=" + codigoInformacionFiscal + " ]";
    }
    
}
