/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.entity;

import java.io.Serializable;
import java.math.BigDecimal;
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
@Table(name = "item")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Item.findAll", query = "SELECT i FROM Item i"),
    @NamedQuery(name = "Item.findByCodigoItem", query = "SELECT i FROM Item i WHERE i.codigoItem = :codigoItem"),
    @NamedQuery(name = "Item.findByNombreItem", query = "SELECT i FROM Item i WHERE i.nombreItem = :nombreItem"),
    @NamedQuery(name = "Item.findByPrecioItem", query = "SELECT i FROM Item i WHERE i.precioItem = :precioItem")})
public class Item implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "codigo_item")
    private Integer codigoItem;
    @Basic(optional = false)
    @Column(name = "nombre_item")
    private String nombreItem;
    // @Max(value=?)  @Min(value=?)//if you know range of your decimal fields consider using these annotations to enforce field validation
    @Basic(optional = false)
    @Column(name = "precio_item")
    private BigDecimal precioItem;
    @OneToMany(cascade = CascadeType.ALL, mappedBy = "codigoItem")
    private Collection<DetalleFactura> detalleFacturaCollection;

    public Item() {
    }

    public Item(Integer codigoItem) {
        this.codigoItem = codigoItem;
    }

    public Item(Integer codigoItem, String nombreItem, BigDecimal precioItem) {
        this.codigoItem = codigoItem;
        this.nombreItem = nombreItem;
        this.precioItem = precioItem;
    }

    public Integer getCodigoItem() {
        return codigoItem;
    }

    public void setCodigoItem(Integer codigoItem) {
        this.codigoItem = codigoItem;
    }

    public String getNombreItem() {
        return nombreItem;
    }

    public void setNombreItem(String nombreItem) {
        this.nombreItem = nombreItem;
    }

    public BigDecimal getPrecioItem() {
        return precioItem;
    }

    public void setPrecioItem(BigDecimal precioItem) {
        this.precioItem = precioItem;
    }

    @XmlTransient
    public Collection<DetalleFactura> getDetalleFacturaCollection() {
        return detalleFacturaCollection;
    }

    public void setDetalleFacturaCollection(Collection<DetalleFactura> detalleFacturaCollection) {
        this.detalleFacturaCollection = detalleFacturaCollection;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (codigoItem != null ? codigoItem.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Item)) {
            return false;
        }
        Item other = (Item) object;
        if ((this.codigoItem == null && other.codigoItem != null) || (this.codigoItem != null && !this.codigoItem.equals(other.codigoItem))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "net.feria.entity.Item[ codigoItem=" + codigoItem + " ]";
    }
    
}
