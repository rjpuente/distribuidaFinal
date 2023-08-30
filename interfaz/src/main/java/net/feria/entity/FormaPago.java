/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.entity;

import java.io.Serializable;
import java.util.Collection;
import javax.persistence.Basic;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.NamedQueries;
import javax.persistence.NamedQuery;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlTransient;

/**
 *
 * @author Deth1
 */
@Entity
@Table(name = "forma_pago")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "FormaPago.findAll", query = "SELECT f FROM FormaPago f"),
    @NamedQuery(name = "FormaPago.findByCodigoFormaPago", query = "SELECT f FROM FormaPago f WHERE f.codigoFormaPago = :codigoFormaPago"),
    @NamedQuery(name = "FormaPago.findByCodigoSriFormaPago", query = "SELECT f FROM FormaPago f WHERE f.codigoSriFormaPago = :codigoSriFormaPago"),
    @NamedQuery(name = "FormaPago.findByDescripcion", query = "SELECT f FROM FormaPago f WHERE f.descripcion = :descripcion")})
public class FormaPago implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "codigo_forma_pago")
    private Integer codigoFormaPago;
    @Basic(optional = false)
    @Column(name = "codigo_sri_forma_pago")
    private String codigoSriFormaPago;
    @Basic(optional = false)
    @Column(name = "descripcion")
    private String descripcion;
    @OneToMany(cascade = CascadeType.ALL, mappedBy = "formaPago")
    private Collection<FacturasCabecera> facturasCabeceraCollection;

    public FormaPago() {
    }

    public FormaPago(Integer codigoFormaPago) {
        this.codigoFormaPago = codigoFormaPago;
    }

    public FormaPago(Integer codigoFormaPago, String codigoSriFormaPago, String descripcion) {
        this.codigoFormaPago = codigoFormaPago;
        this.codigoSriFormaPago = codigoSriFormaPago;
        this.descripcion = descripcion;
    }

    public Integer getCodigoFormaPago() {
        return codigoFormaPago;
    }

    public void setCodigoFormaPago(Integer codigoFormaPago) {
        this.codigoFormaPago = codigoFormaPago;
    }

    public String getCodigoSriFormaPago() {
        return codigoSriFormaPago;
    }

    public void setCodigoSriFormaPago(String codigoSriFormaPago) {
        this.codigoSriFormaPago = codigoSriFormaPago;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    @XmlTransient
    public Collection<FacturasCabecera> getFacturasCabeceraCollection() {
        return facturasCabeceraCollection;
    }

    public void setFacturasCabeceraCollection(Collection<FacturasCabecera> facturasCabeceraCollection) {
        this.facturasCabeceraCollection = facturasCabeceraCollection;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (codigoFormaPago != null ? codigoFormaPago.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof FormaPago)) {
            return false;
        }
        FormaPago other = (FormaPago) object;
        if ((this.codigoFormaPago == null && other.codigoFormaPago != null) || (this.codigoFormaPago != null && !this.codigoFormaPago.equals(other.codigoFormaPago))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "net.feria.entity.FormaPago[ codigoFormaPago=" + codigoFormaPago + " ]";
    }
    
}
