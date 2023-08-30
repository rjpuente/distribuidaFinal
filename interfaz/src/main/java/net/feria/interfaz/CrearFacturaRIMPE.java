/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.interfaz;

import java.awt.Dimension;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import javax.swing.JOptionPane;
import javax.swing.table.DefaultTableModel;
import net.feria.controlador.EntityManagerFactoryObject;
import net.feria.ejb.ClienteJpaController;
import net.feria.ejb.ComprobanteElectronicoJpaController;
import net.feria.ejb.DetalleFacturaJpaController;
import net.feria.ejb.FacturasCabeceraJpaController;
import net.feria.ejb.FormaPagoJpaController;
import net.feria.ejb.ItemJpaController;
import net.feria.entity.Cliente;
import net.feria.entity.ComprobanteElectronico;
import net.feria.entity.DetalleFactura;
import net.feria.entity.FacturasCabecera;
import net.feria.entity.FormaPago;
import net.feria.entity.Item;

/**
 *
 * @author Deth1
 */
public class CrearFacturaRIMPE extends javax.swing.JFrame {
    
    private final ClienteJpaController clienteJpaController = new ClienteJpaController(EntityManagerFactoryObject.emf);
    private final ItemJpaController itemJpaController = new ItemJpaController(EntityManagerFactoryObject.emf);
    private final FacturasCabeceraJpaController facturasCabeceraJpaController = new FacturasCabeceraJpaController(EntityManagerFactoryObject.emf);
    private final DetalleFacturaJpaController detalleFacturaJpaController = new DetalleFacturaJpaController(EntityManagerFactoryObject.emf);
    private final ComprobanteElectronicoJpaController comprobanteElectronicoJpaController = new ComprobanteElectronicoJpaController(EntityManagerFactoryObject.emf);
    private final FormaPagoJpaController formaPagoJpaController = new FormaPagoJpaController(EntityManagerFactoryObject.emf);
    
    private static final Double IVA_PORCENTAHE = 1.12;
    private static final String INICIO_FACTURA = "001001";
    private static final String FORMATO_FACTURA = "%09d";
    private List<Cliente> listaClientes;
    private List<Item> listaItems;
    private List<DetalleFactura> listaProductosFactura;
    private List<FormaPago> listaFormasPago;
    private FacturasCabecera facturaCabecera;

    /**
     * Creates new form CrearFacturaRuc
     */
    public CrearFacturaRIMPE() {
        initComponents();
        this.setLocationRelativeTo(null);
        this.setSize(new Dimension(850, 750));
        llenarListaClientes();
        llenarListaProductos();
        llenarListaFormasPagos();
        inicializarTablaProductos();
        inicializarTablaDetalleFactura();
    }
    
    private void inicializarTablaProductos() {
        String[] columnas = {"Nombre", "Cantidad", "Precio", "Total Unitario"};
        DefaultTableModel model = new DefaultTableModel(columnas, 0);
        
        jTableItems.setModel(model);
        
    }
    
    private void inicializarTablaDetalleFactura() {
        listaProductosFactura = new ArrayList<>();
    }
    
    private void llenarListaClientes() {
        listaClientes = clienteJpaController.findClienteEntities();
        
        listaClientes.stream().forEach((cliente) -> {
            this.jComboBoxClientes.addItem(cliente.getNombre());
        });
    }
    
    private void llenarListaProductos() {
        listaItems = itemJpaController.findItemEntities();
        
        listaItems.forEach((item) -> {
            jComboBoxProductos.addItem(item.getNombreItem());
        });
    }
    
    private void llenarListaFormasPagos() {
        listaFormasPago = formaPagoJpaController.findFormaPagoEntities();
        
        listaFormasPago.forEach((pago) -> {
            jComboBoxFormasPago.addItem(pago.getDescripcion());
        });
    }
    
    private void obtenerCabeceraFactura() {
        Cliente cliente = listaClientes.stream()
                .filter(c -> c.getNombre().equals(jComboBoxClientes.getSelectedItem().toString()))
                .findFirst()
                .orElse(null);
        
        FormaPago formaPago = listaFormasPago.stream()
                .filter(f -> f.getDescripcion().equals(jComboBoxFormasPago.getSelectedItem().toString()))
                .findFirst()
                .orElse(null);
        
        if (cliente != null && formaPago != null) {
            facturaCabecera = new FacturasCabecera();
            facturaCabecera.setCodigoCliente(cliente);
            facturaCabecera.setNumeroFactura(obtenerNumeroFactura());
            facturaCabecera.setFechaFactura(new Date());
            facturaCabecera.setFechaFactura(new Date());
            facturaCabecera.setFormaPago(formaPago);
        }
        
    }
    
    private String obtenerNumeroFactura() {
        return "F_" + completarFacturaConCeros(facturasCabeceraJpaController.obtenerUltimoNumeroFacttura());
    }
    
    private String completarFacturaConCeros(String numeroFactura) {
        
        int numeroFacturaInt = Integer.parseInt(numeroFactura);
        String numeroFacturaFormateado = String.format(FORMATO_FACTURA, numeroFacturaInt);
        
        return INICIO_FACTURA + numeroFacturaFormateado;
        
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jPanel1 = new javax.swing.JPanel();
        jLabel2 = new javax.swing.JLabel();
        jComboBoxClientes = new javax.swing.JComboBox();
        jButtonNuevoCliente = new javax.swing.JButton();
        jComboBoxProductos = new javax.swing.JComboBox();
        jLabel1 = new javax.swing.JLabel();
        jTextFieldCantidad = new javax.swing.JTextField();
        jLabel3 = new javax.swing.JLabel();
        jButtonAgregar = new javax.swing.JButton();
        jScrollPaneItems = new javax.swing.JScrollPane();
        jTableItems = new javax.swing.JTable();
        jButtonGenerarFactura = new javax.swing.JButton();
        jLabel4 = new javax.swing.JLabel();
        jComboBoxFormasPago = new javax.swing.JComboBox();
        jButtonRegresar = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

        jPanel1.setBorder(javax.swing.BorderFactory.createEtchedBorder());

        jLabel2.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jLabel2.setText("Cliente");

        jComboBoxClientes.setFont(new java.awt.Font("Dialog", 0, 18)); // NOI18N

        jButtonNuevoCliente.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jButtonNuevoCliente.setText("Nuevo Cliente");
        jButtonNuevoCliente.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonNuevoClienteActionPerformed(evt);
            }
        });

        jComboBoxProductos.setFont(new java.awt.Font("Dialog", 0, 18)); // NOI18N

        jLabel1.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jLabel1.setText("Productos");

        jTextFieldCantidad.setFont(new java.awt.Font("Dialog", 0, 18)); // NOI18N
        jTextFieldCantidad.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jTextFieldCantidadActionPerformed(evt);
            }
        });

        jLabel3.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jLabel3.setText("Cantidad");

        jButtonAgregar.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jButtonAgregar.setText("Agregar");
        jButtonAgregar.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonAgregarActionPerformed(evt);
            }
        });

        jTableItems.setAutoCreateRowSorter(true);
        jTableItems.setFont(new java.awt.Font("Dialog", 1, 14)); // NOI18N
        jTableItems.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null}
            },
            new String [] {
                "Title 1", "Title 2", "Title 3", "Title 4"
            }
        ));
        jScrollPaneItems.setViewportView(jTableItems);

        jButtonGenerarFactura.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jButtonGenerarFactura.setText("Generar factura");
        jButtonGenerarFactura.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonGenerarFacturaActionPerformed(evt);
            }
        });

        jLabel4.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jLabel4.setText("Metodo de pago");

        jComboBoxFormasPago.setFont(new java.awt.Font("Dialog", 0, 18)); // NOI18N

        jButtonRegresar.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jButtonRegresar.setText("Regresar");
        jButtonRegresar.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonRegresarActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(93, 93, 93)
                .addComponent(jButtonGenerarFactura)
                .addGap(78, 78, 78)
                .addComponent(jButtonRegresar, javax.swing.GroupLayout.PREFERRED_SIZE, 125, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, Short.MAX_VALUE))
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(46, 46, 46)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jLabel4)
                            .addComponent(jLabel2, javax.swing.GroupLayout.PREFERRED_SIZE, 88, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addGap(0, 0, Short.MAX_VALUE))
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                            .addComponent(jScrollPaneItems, javax.swing.GroupLayout.Alignment.LEADING)
                            .addGroup(jPanel1Layout.createSequentialGroup()
                                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                                    .addComponent(jComboBoxClientes, javax.swing.GroupLayout.PREFERRED_SIZE, 246, javax.swing.GroupLayout.PREFERRED_SIZE)
                                    .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                                        .addGroup(javax.swing.GroupLayout.Alignment.LEADING, jPanel1Layout.createSequentialGroup()
                                            .addComponent(jLabel1)
                                            .addGap(0, 0, Short.MAX_VALUE))
                                        .addComponent(jComboBoxProductos, javax.swing.GroupLayout.Alignment.LEADING, 0, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                                        .addComponent(jComboBoxFormasPago, javax.swing.GroupLayout.Alignment.LEADING, 0, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)))
                                .addGap(18, 18, 18)
                                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel1Layout.createSequentialGroup()
                                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                            .addComponent(jTextFieldCantidad, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.PREFERRED_SIZE, 77, javax.swing.GroupLayout.PREFERRED_SIZE)
                                            .addComponent(jLabel3, javax.swing.GroupLayout.Alignment.TRAILING))
                                        .addGap(27, 27, 27)
                                        .addComponent(jButtonAgregar))
                                    .addComponent(jButtonNuevoCliente, javax.swing.GroupLayout.Alignment.TRAILING))))
                        .addContainerGap(111, Short.MAX_VALUE))))
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(24, 24, 24)
                .addComponent(jLabel2)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jComboBoxClientes)
                    .addComponent(jButtonNuevoCliente, javax.swing.GroupLayout.DEFAULT_SIZE, 45, Short.MAX_VALUE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addComponent(jLabel4)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addComponent(jComboBoxFormasPago, javax.swing.GroupLayout.PREFERRED_SIZE, 36, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel1)
                    .addComponent(jLabel3))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jComboBoxProductos, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jTextFieldCantidad, javax.swing.GroupLayout.PREFERRED_SIZE, 34, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jButtonAgregar, javax.swing.GroupLayout.PREFERRED_SIZE, 35, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(26, 26, 26)
                .addComponent(jScrollPaneItems, javax.swing.GroupLayout.PREFERRED_SIZE, 207, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(18, 18, 18)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jButtonGenerarFactura, javax.swing.GroupLayout.PREFERRED_SIZE, 42, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jButtonRegresar, javax.swing.GroupLayout.PREFERRED_SIZE, 41, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addContainerGap())
        );

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addComponent(jPanel1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButtonAgregarActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonAgregarActionPerformed
        String nombreItem = jComboBoxProductos.getSelectedItem().toString();
        try {
            Integer cantidad = Integer.parseInt(jTextFieldCantidad.getText());
            Item item = listaItems.stream()
                    .filter(i -> i.getNombreItem().equals(nombreItem))
                    .findFirst()
                    .orElse(null);
            
            DetalleFactura detalleFactura = new DetalleFactura();
            
            detalleFactura.setCodigoItem(item);
            detalleFactura.setCantidad(cantidad);
            
            listaProductosFactura.add(detalleFactura);
            
            actualizarListaItems(detalleFactura);
            
        } catch (Exception e) {
            JOptionPane.showMessageDialog(null, "Debe ingresar solo números");
        }
    }//GEN-LAST:event_jButtonAgregarActionPerformed

    private void jTextFieldCantidadActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jTextFieldCantidadActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_jTextFieldCantidadActionPerformed

    private void jButtonNuevoClienteActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonNuevoClienteActionPerformed
        CrearCliente crearCliente = new CrearCliente();
        crearCliente.show();
        this.dispose();
    }//GEN-LAST:event_jButtonNuevoClienteActionPerformed

    private void jButtonGenerarFacturaActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonGenerarFacturaActionPerformed
        
        if (!listaProductosFactura.isEmpty()) {
            obtenerCabeceraFactura();
            Double totalSinIva = 0d;
            totalSinIva = listaProductosFactura.stream()
                    .map((item) -> item.getCantidad() * item.getCodigoItem().getPrecioItem().doubleValue()).reduce(totalSinIva, (accumulator, _item) -> accumulator + _item);
            
            facturaCabecera.setValorTotalFactura(BigDecimal.valueOf(totalSinIva));
            facturaCabecera.setIva(BigDecimal.valueOf(0));
            
            facturaCabecera = facturasCabeceraJpaController.create(facturaCabecera);
            
            listaProductosFactura.stream().map((item) -> {
                item.setCodigoFactura(facturaCabecera);
                return item;
            }).forEach((item) -> {
                detalleFacturaJpaController.create(item);
            });
            
            IniciarProcesoComprobanteElectronico();
            
            JOptionPane.showMessageDialog(null, "Factura ingresada exitosamente");
            
            Menu menu = new Menu();
            menu.show();
            this.dispose();
        } else {
            JOptionPane.showMessageDialog(null, "Debe agregar al menos un producto");
        }
        

    }//GEN-LAST:event_jButtonGenerarFacturaActionPerformed

    private void jButtonRegresarActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonRegresarActionPerformed
        Menu menu = new Menu();
        menu.show();
        this.dispose();
    }//GEN-LAST:event_jButtonRegresarActionPerformed
    
    private void IniciarProcesoComprobanteElectronico() {
        ComprobanteElectronico comprobanteElectronico = new ComprobanteElectronico();
        
        comprobanteElectronico.setNumeroComprobante(facturaCabecera.getNumeroFactura());
        comprobanteElectronico.setClaveAcceso(generarClaveAcceso());
        comprobanteElectronico.setRuc(facturaCabecera.getCodigoCliente().getNumeroCedula());
        comprobanteElectronico.setFechaInicioProceso(new Date());
        comprobanteElectronico.setFechaGeneracionXml(new Date());
        comprobanteElectronico.setFechaFirmaXml(new Date());
        comprobanteElectronico.setFechaAutorizacion(new Date());
        comprobanteElectronico.setNumeroAutorizacionSri("");
        comprobanteElectronico.setNumeroIntentosTransicionAutorizacion(0);
        comprobanteElectronico.setTransicionAutorizacion(0);
        comprobanteElectronico.setMensajeErrorAutorizacion("");
        comprobanteElectronico.setAnulado(0);
        comprobanteElectronico.setFirmado(false);
        
        try {
            comprobanteElectronicoJpaController.create(comprobanteElectronico);
        } catch (Exception ex) {
            JOptionPane.showMessageDialog(null, "Número de factura ya ocupado");
        }
        
    }
    
    private String generarClaveAcceso() {
        String tipoComprobante = "01";
        String tipoAmbiente = "2";
        String tipoEmision = "1";

        // 01234567890123456
        // F_001002000023314
        String establecimiento = facturaCabecera.getNumeroFactura().substring(2, 5);
        String puntoEmision = facturaCabecera.getNumeroFactura().substring(5, 8);
        String secuencial = facturaCabecera.getNumeroFactura().substring(8, 15);
        String cadenaAleatoria = facturaCabecera.getNumeroFactura().substring(9, 17);
        
        Calendar cal = Calendar.getInstance();
        cal.setTime(facturaCabecera.getFechaFactura());
        Integer anio = cal.get(Calendar.YEAR);
        Integer mes = cal.get(Calendar.MONTH) + 1;
        Integer dia = cal.get(Calendar.DAY_OF_MONTH);
        
        String ret = "";
        ret += cerosIzquierda(dia.toString(), 2);
        ret += cerosIzquierda(mes.toString(), 2);
        ret += cerosIzquierda(anio.toString(), 4);
        ret += cerosIzquierda(tipoComprobante, 2);
        ret += cerosIzquierda(facturaCabecera.getCodigoCliente().getNumeroCedula(), 13);
        ret += cerosIzquierda(tipoAmbiente, 1);
        ret += cerosIzquierda(establecimiento, 3);
        ret += cerosIzquierda(puntoEmision, 3);
        ret += cerosIzquierda(secuencial, 9);
        ret += cerosIzquierda(cadenaAleatoria, 8);
        ret += cerosIzquierda(tipoEmision, 1);
        ret += cerosIzquierda(digitoVerificador(ret).toString(), 1);
        return ret;
    }
    
    private String cerosIzquierda(String valor, int longitud) {
        String ret = valor;
        while (ret.length() < longitud) {
            ret = "0" + ret;
        }
        return ret;
    }
    
    private Integer digitoVerificador(String cadena) {
        Integer factor = 2;
        Integer valorCalculado;
        Integer sumatoria = 0;
        Integer residuo11;
        Integer diferencia11;
        
        for (int i = cadena.length() - 1; i >= 0; i--) {
            Integer digito = Integer.parseInt(cadena.substring(i, i + 1));
            valorCalculado = digito * factor;
            sumatoria += valorCalculado;
            factor += 1;
            if (factor.equals(8)) {
                factor = 2;
            }
        }
        residuo11 = sumatoria % 11;
        diferencia11 = 11 - residuo11;
        if (diferencia11.equals(10)) {
            diferencia11 = 1;
        } else if (diferencia11.equals(11)) {
            diferencia11 = 0;
        }
        return diferencia11;
    }
    
    private void actualizarListaItems(DetalleFactura detalle) {
        DefaultTableModel model = (DefaultTableModel) jTableItems.getModel();
        Double precioTotalItem = detalle.getCantidad() * detalle.getCodigoItem().getPrecioItem().doubleValue();
        Object[] fila = {detalle.getCodigoItem().getNombreItem(), detalle.getCantidad(),
            detalle.getCodigoItem().getPrecioItem(), (precioTotalItem)};
        model.addRow(fila);
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(CrearFacturaRIMPE.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(CrearFacturaRIMPE.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(CrearFacturaRIMPE.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(CrearFacturaRIMPE.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new CrearFacturaRIMPE().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton jButtonAgregar;
    private javax.swing.JButton jButtonGenerarFactura;
    private javax.swing.JButton jButtonNuevoCliente;
    private javax.swing.JButton jButtonRegresar;
    private javax.swing.JComboBox jComboBoxClientes;
    private javax.swing.JComboBox jComboBoxFormasPago;
    private javax.swing.JComboBox jComboBoxProductos;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JScrollPane jScrollPaneItems;
    private javax.swing.JTable jTableItems;
    private javax.swing.JTextField jTextFieldCantidad;
    // End of variables declaration//GEN-END:variables
}
