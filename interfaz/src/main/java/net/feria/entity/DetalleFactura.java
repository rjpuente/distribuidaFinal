/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.entity;

import java.io.Serializable;
import java.math.BigDecimal;
import javax.persistence.Basic;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.NamedQueries;
import javax.persistence.NamedQuery;
import javax.persistence.Table;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Deth1
 */
@Entity
@Table(name = "detalle_factura")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "DetalleFactura.findAll", query = "SELECT d FROM DetalleFactura d"),
    @NamedQuery(name = "DetalleFactura.findByCodigoDetalleFactura", query = "SELECT d FROM DetalleFactura d WHERE d.codigoDetalleFactura = :codigoDetalleFactura"),
    @NamedQuery(name = "DetalleFactura.findByDescuentoUnitario", query = "SELECT d FROM DetalleFactura d WHERE d.descuentoUnitario = :descuentoUnitario"),
    @NamedQuery(name = "DetalleFactura.findByCantidad", query = "SELECT d FROM DetalleFactura d WHERE d.cantidad = :cantidad")})
public class DetalleFactura implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "codigo_detalle_factura")
    private Integer codigoDetalleFactura;
    // @Max(value=?)  @Min(value=?)//if you know range of your decimal fields consider using these annotations to enforce field validation
    @Column(name = "descuento_unitario")
    private BigDecimal descuentoUnitario;
    @Basic(optional = false)
    @Column(name = "cantidad")
    private int cantidad;
    @JoinColumn(name = "codigo_factura", referencedColumnName = "codigo_factura")
    @ManyToOne(optional = false)
    private FacturasCabecera codigoFactura;
    @JoinColumn(name = "codigo_item", referencedColumnName = "codigo_item")
    @ManyToOne(optional = false)
    private Item codigoItem;

    public DetalleFactura() {
    }

    public DetalleFactura(Integer codigoDetalleFactura) {
        this.codigoDetalleFactura = codigoDetalleFactura;
    }

    public DetalleFactura(Integer codigoDetalleFactura, int cantidad) {
        this.codigoDetalleFactura = codigoDetalleFactura;
        this.cantidad = cantidad;
    }

    public Integer getCodigoDetalleFactura() {
        return codigoDetalleFactura;
    }

    public void setCodigoDetalleFactura(Integer codigoDetalleFactura) {
        this.codigoDetalleFactura = codigoDetalleFactura;
    }

    public BigDecimal getDescuentoUnitario() {
        return descuentoUnitario;
    }

    public void setDescuentoUnitario(BigDecimal descuentoUnitario) {
        this.descuentoUnitario = descuentoUnitario;
    }

    public int getCantidad() {
        return cantidad;
    }

    public void setCantidad(int cantidad) {
        this.cantidad = cantidad;
    }

    public FacturasCabecera getCodigoFactura() {
        return codigoFactura;
    }

    public void setCodigoFactura(FacturasCabecera codigoFactura) {
        this.codigoFactura = codigoFactura;
    }

    public Item getCodigoItem() {
        return codigoItem;
    }

    public void setCodigoItem(Item codigoItem) {
        this.codigoItem = codigoItem;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (codigoDetalleFactura != null ? codigoDetalleFactura.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof DetalleFactura)) {
            return false;
        }
        DetalleFactura other = (DetalleFactura) object;
        if ((this.codigoDetalleFactura == null && other.codigoDetalleFactura != null) || (this.codigoDetalleFactura != null && !this.codigoDetalleFactura.equals(other.codigoDetalleFactura))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "net.feria.entity.DetalleFactura[ codigoDetalleFactura=" + codigoDetalleFactura + " ]";
    }
    
}
