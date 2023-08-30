/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.entity;

import java.io.Serializable;
import java.math.BigDecimal;
import java.util.Collection;
import java.util.Date;
import javax.persistence.Basic;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.NamedQueries;
import javax.persistence.NamedQuery;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.persistence.Temporal;
import javax.persistence.TemporalType;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlTransient;

/**
 *
 * @author Deth1
 */
@Entity
@Table(name = "facturas_cabecera")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "FacturasCabecera.findAll", query = "SELECT f FROM FacturasCabecera f"),
    @NamedQuery(name = "FacturasCabecera.findByCodigoFactura", query = "SELECT f FROM FacturasCabecera f WHERE f.codigoFactura = :codigoFactura"),
    @NamedQuery(name = "FacturasCabecera.findByNumeroFactura", query = "SELECT f FROM FacturasCabecera f WHERE f.numeroFactura = :numeroFactura"),
    @NamedQuery(name = "FacturasCabecera.findByFechaFactura", query = "SELECT f FROM FacturasCabecera f WHERE f.fechaFactura = :fechaFactura"),
    @NamedQuery(name = "FacturasCabecera.findByValorTotalFactura", query = "SELECT f FROM FacturasCabecera f WHERE f.valorTotalFactura = :valorTotalFactura"),
    @NamedQuery(name = "FacturasCabecera.findByValorDescuento", query = "SELECT f FROM FacturasCabecera f WHERE f.valorDescuento = :valorDescuento"),
    @NamedQuery(name = "FacturasCabecera.findByIva", query = "SELECT f FROM FacturasCabecera f WHERE f.iva = :iva")})
public class FacturasCabecera implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Basic(optional = false)
    @Column(name = "codigo_factura")
    private Integer codigoFactura;
    @Basic(optional = false)
    @Column(name = "numero_factura")
    private String numeroFactura;
    @Basic(optional = false)
    @Column(name = "fecha_factura")
    @Temporal(TemporalType.DATE)
    private Date fechaFactura;
    // @Max(value=?)  @Min(value=?)//if you know range of your decimal fields consider using these annotations to enforce field validation
    @Column(name = "valor_total_factura")
    private BigDecimal valorTotalFactura;
    @Column(name = "valor_descuento")
    private BigDecimal valorDescuento;
    @Column(name = "iva")
    private BigDecimal iva;
    @OneToMany(cascade = CascadeType.ALL, mappedBy = "codigoFactura")
    private Collection<DetalleFactura> detalleFacturaCollection;
    @JoinColumn(name = "codigo_cliente", referencedColumnName = "codigo_cliente")
    @ManyToOne(optional = false)
    private Cliente codigoCliente;
    @JoinColumn(name = "forma_pago", referencedColumnName = "codigo_forma_pago")
    @ManyToOne(optional = false)
    private FormaPago formaPago;

    public FacturasCabecera() {
    }

    public FacturasCabecera(Integer codigoFactura) {
        this.codigoFactura = codigoFactura;
    }

    public FacturasCabecera(Integer codigoFactura, String numeroFactura, Date fechaFactura) {
        this.codigoFactura = codigoFactura;
        this.numeroFactura = numeroFactura;
        this.fechaFactura = fechaFactura;
    }

    public Integer getCodigoFactura() {
        return codigoFactura;
    }

    public void setCodigoFactura(Integer codigoFactura) {
        this.codigoFactura = codigoFactura;
    }

    public String getNumeroFactura() {
        return numeroFactura;
    }

    public void setNumeroFactura(String numeroFactura) {
        this.numeroFactura = numeroFactura;
    }

    public Date getFechaFactura() {
        return fechaFactura;
    }

    public void setFechaFactura(Date fechaFactura) {
        this.fechaFactura = fechaFactura;
    }

    public BigDecimal getValorTotalFactura() {
        return valorTotalFactura;
    }

    public void setValorTotalFactura(BigDecimal valorTotalFactura) {
        this.valorTotalFactura = valorTotalFactura;
    }

    public BigDecimal getValorDescuento() {
        return valorDescuento;
    }

    public void setValorDescuento(BigDecimal valorDescuento) {
        this.valorDescuento = valorDescuento;
    }

    public BigDecimal getIva() {
        return iva;
    }

    public void setIva(BigDecimal iva) {
        this.iva = iva;
    }

    @XmlTransient
    public Collection<DetalleFactura> getDetalleFacturaCollection() {
        return detalleFacturaCollection;
    }

    public void setDetalleFacturaCollection(Collection<DetalleFactura> detalleFacturaCollection) {
        this.detalleFacturaCollection = detalleFacturaCollection;
    }

    public Cliente getCodigoCliente() {
        return codigoCliente;
    }

    public void setCodigoCliente(Cliente codigoCliente) {
        this.codigoCliente = codigoCliente;
    }

    public FormaPago getFormaPago() {
        return formaPago;
    }

    public void setFormaPago(FormaPago formaPago) {
        this.formaPago = formaPago;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (codigoFactura != null ? codigoFactura.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof FacturasCabecera)) {
            return false;
        }
        FacturasCabecera other = (FacturasCabecera) object;
        if ((this.codigoFactura == null && other.codigoFactura != null) || (this.codigoFactura != null && !this.codigoFactura.equals(other.codigoFactura))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "net.feria.entity.FacturasCabecera[ codigoFactura=" + codigoFactura + " ]";
    }
    
}
