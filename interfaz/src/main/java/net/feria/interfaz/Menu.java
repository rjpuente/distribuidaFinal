/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.interfaz;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.GridLayout;
import javax.swing.BoxLayout;
import javax.swing.JPanel;

/**
 *
 * @author Deth1
 */
public class Menu extends javax.swing.JFrame {

    /**
     * Creates new form Menu
     */
    public Menu() {
        initComponents();
        this.setLocationRelativeTo(null);
        this.setResizable(false);
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
        jButtonCrearFacturaRuc = new javax.swing.JButton();
        jButtonCrearFacturaRise = new javax.swing.JButton();
        jButtonCrearCliente = new javax.swing.JButton();
        jButtonCrearProducto = new javax.swing.JButton();
        jButtonCambiarDireccionRuc = new javax.swing.JButton();
        jButtonCrearUsuario = new javax.swing.JButton();
        jButtonConfiguracionImpresora = new javax.swing.JButton();
        jButton1 = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

        jPanel1.setBorder(javax.swing.BorderFactory.createEtchedBorder());

        jButtonCrearFacturaRuc.setFont(new java.awt.Font("Dialog", 1, 24)); // NOI18N
        jButtonCrearFacturaRuc.setText("Crear factura RUC");
        jButtonCrearFacturaRuc.setBorder(new javax.swing.border.SoftBevelBorder(javax.swing.border.BevelBorder.RAISED));
        jButtonCrearFacturaRuc.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonCrearFacturaRuc.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonCrearFacturaRucActionPerformed(evt);
            }
        });

        jButtonCrearFacturaRise.setFont(new java.awt.Font("Dialog", 1, 24)); // NOI18N
        jButtonCrearFacturaRise.setText("Crear Factura RIMPE");
        jButtonCrearFacturaRise.setBorder(new javax.swing.border.SoftBevelBorder(javax.swing.border.BevelBorder.RAISED));
        jButtonCrearFacturaRise.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonCrearFacturaRise.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonCrearFacturaRiseActionPerformed(evt);
            }
        });

        jButtonCrearCliente.setFont(new java.awt.Font("Dialog", 1, 24)); // NOI18N
        jButtonCrearCliente.setText("Crear cliente");
        jButtonCrearCliente.setBorder(new javax.swing.border.SoftBevelBorder(javax.swing.border.BevelBorder.RAISED));
        jButtonCrearCliente.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonCrearCliente.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonCrearClienteActionPerformed(evt);
            }
        });

        jButtonCrearProducto.setFont(new java.awt.Font("Dialog", 1, 24)); // NOI18N
        jButtonCrearProducto.setText("Crear producto");
        jButtonCrearProducto.setBorder(new javax.swing.border.SoftBevelBorder(javax.swing.border.BevelBorder.RAISED));
        jButtonCrearProducto.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonCrearProductoActionPerformed(evt);
            }
        });

        jButtonCambiarDireccionRuc.setFont(new java.awt.Font("Dialog", 1, 24)); // NOI18N
        jButtonCambiarDireccionRuc.setText("Cambiar dirección RUC");
        jButtonCambiarDireccionRuc.setBorder(new javax.swing.border.SoftBevelBorder(javax.swing.border.BevelBorder.RAISED));
        jButtonCambiarDireccionRuc.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonCambiarDireccionRuc.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonCambiarDireccionRucActionPerformed(evt);
            }
        });

        jButtonCrearUsuario.setFont(new java.awt.Font("Dialog", 1, 24)); // NOI18N
        jButtonCrearUsuario.setText("Crear usuario");
        jButtonCrearUsuario.setBorder(new javax.swing.border.SoftBevelBorder(javax.swing.border.BevelBorder.RAISED));
        jButtonCrearUsuario.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonCrearUsuario.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonCrearUsuarioActionPerformed(evt);
            }
        });

        jButtonConfiguracionImpresora.setFont(new java.awt.Font("Dialog", 0, 18)); // NOI18N
        jButtonConfiguracionImpresora.setText("Configurar Impresora");
        jButtonConfiguracionImpresora.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonConfiguracionImpresoraActionPerformed(evt);
            }
        });

        jButton1.setFont(new java.awt.Font("Dialog", 1, 18)); // NOI18N
        jButton1.setText("Reporte Facturación");
        jButton1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(48, 48, 48)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(jButtonConfiguracionImpresora, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(jButtonCrearProducto, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(jButtonCrearUsuario, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(jButtonCrearFacturaRuc, javax.swing.GroupLayout.DEFAULT_SIZE, 288, Short.MAX_VALUE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 142, Short.MAX_VALUE)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                        .addComponent(jButtonCambiarDireccionRuc, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, 302, Short.MAX_VALUE)
                        .addComponent(jButtonCrearCliente, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, 302, Short.MAX_VALUE)
                        .addComponent(jButton1, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                    .addComponent(jButtonCrearFacturaRise, javax.swing.GroupLayout.PREFERRED_SIZE, 292, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(49, 49, 49))
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(63, 63, 63)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jButtonCrearFacturaRuc, javax.swing.GroupLayout.PREFERRED_SIZE, 91, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jButtonCrearFacturaRise, javax.swing.GroupLayout.PREFERRED_SIZE, 91, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(58, 58, 58)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jButtonCrearProducto, javax.swing.GroupLayout.PREFERRED_SIZE, 91, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jButtonCrearCliente, javax.swing.GroupLayout.PREFERRED_SIZE, 91, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(47, 47, 47)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jButtonCambiarDireccionRuc, javax.swing.GroupLayout.PREFERRED_SIZE, 96, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jButtonCrearUsuario, javax.swing.GroupLayout.PREFERRED_SIZE, 96, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 41, Short.MAX_VALUE)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(jButtonConfiguracionImpresora, javax.swing.GroupLayout.DEFAULT_SIZE, 42, Short.MAX_VALUE)
                    .addComponent(jButton1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
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

    private void jButtonCrearFacturaRucActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonCrearFacturaRucActionPerformed
        CrearFacturaRuc crearFacturaRuc = new CrearFacturaRuc();
        crearFacturaRuc.show();
        this.dispose();
    }//GEN-LAST:event_jButtonCrearFacturaRucActionPerformed

    private void jButtonCrearFacturaRiseActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonCrearFacturaRiseActionPerformed
        CrearFacturaRIMPE crearFacturaRise = new CrearFacturaRIMPE();
        crearFacturaRise.show();
        this.dispose();
    }//GEN-LAST:event_jButtonCrearFacturaRiseActionPerformed

    private void jButtonCrearProductoActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonCrearProductoActionPerformed
        CrearProducto crearProducto = new CrearProducto();
        crearProducto.show();
        this.dispose();
    }//GEN-LAST:event_jButtonCrearProductoActionPerformed

    private void jButtonCrearClienteActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonCrearClienteActionPerformed
        CrearCliente crearCliente = new CrearCliente();
        crearCliente.show();
        this.dispose();
    }//GEN-LAST:event_jButtonCrearClienteActionPerformed

    private void jButtonCrearUsuarioActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonCrearUsuarioActionPerformed
        CrearUsuario crearUsuario = new CrearUsuario();
        crearUsuario.show();
        this.dispose();
    }//GEN-LAST:event_jButtonCrearUsuarioActionPerformed

    private void jButtonCambiarDireccionRucActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonCambiarDireccionRucActionPerformed
        CambiarDireccionRuc cambiarDireccionRuc = new CambiarDireccionRuc();
        cambiarDireccionRuc.show();
        this.dispose();
    }//GEN-LAST:event_jButtonCambiarDireccionRucActionPerformed

    private void jButtonConfiguracionImpresoraActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonConfiguracionImpresoraActionPerformed
        Impresora impresora = new Impresora();
        impresora.show();
        this.dispose();
    }//GEN-LAST:event_jButtonConfiguracionImpresoraActionPerformed

    private void jButton1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ActionPerformed
        ReporteFacturacion reporteFacturacion = new ReporteFacturacion();
        reporteFacturacion.show();
        this.dispose();
    }//GEN-LAST:event_jButton1ActionPerformed

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
            java.util.logging.Logger.getLogger(Menu.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(Menu.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(Menu.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(Menu.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new Menu().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton jButton1;
    private javax.swing.JButton jButtonCambiarDireccionRuc;
    private javax.swing.JButton jButtonConfiguracionImpresora;
    private javax.swing.JButton jButtonCrearCliente;
    private javax.swing.JButton jButtonCrearFacturaRise;
    private javax.swing.JButton jButtonCrearFacturaRuc;
    private javax.swing.JButton jButtonCrearProducto;
    private javax.swing.JButton jButtonCrearUsuario;
    private javax.swing.JPanel jPanel1;
    // End of variables declaration//GEN-END:variables
}
