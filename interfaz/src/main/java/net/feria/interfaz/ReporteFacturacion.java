/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.interfaz;

import com.toedter.calendar.JDateChooser;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.FileOutputStream;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import javax.swing.GroupLayout;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import net.feria.controlador.EntityManagerFactoryObject;
import net.feria.ejb.ComprobanteElectronicoJpaController;
import net.feria.ejb.FacturasCabeceraJpaController;
import net.feria.entity.ComprobanteElectronico;
import net.feria.entity.FacturasCabecera;
import net.feria.objetos.ObjetoReporteFacturacion;
import org.apache.poi.ss.usermodel.BuiltinFormats;
import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.CellStyle;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;

/**
 *
 * @author Deth1
 */
public class ReporteFacturacion extends javax.swing.JFrame {

    private ComprobanteElectronicoJpaController comprobanteElectronicoJpaController = new ComprobanteElectronicoJpaController(EntityManagerFactoryObject.emf);
    private FacturasCabeceraJpaController facturasCabeceraJpaController = new FacturasCabeceraJpaController(EntityManagerFactoryObject.emf);

    private List<ComprobanteElectronico> listaComprobantes;
    private List<ObjetoReporteFacturacion> listaReporteFacturacion;

    /**
     * Creates new form ReporteFacturacion
     */
    public ReporteFacturacion() {
        initComponents();
        this.setLocationRelativeTo(null);
        initDatePicker();

    }

    private void initDatePicker() {
        // Crear y configurar el primer date picker
        datePickerDesde.setBounds(150, 30, 200, 30);
        // Crear y configurar el segundo date picker
        datePickerHasta.setBounds(150, 70, 200, 30);
        // Crear y configurar los labels
        labelDesde.setBounds(40, 30, 100, 30);
        labelHasta.setBounds(40, 70, 100, 30);
        // Crear y configurar los botones
        botonConsultar.setBounds(150, 110, 100, 30);
        botonRegresar.setBounds(260, 110, 100, 30);

        botonRegresar.setFont(new Font("Arial", Font.BOLD, 22)); // Tamaño 22 y en negrita
        botonConsultar.setFont(new Font("Arial", Font.BOLD, 22)); // Tamaño 22 y en negrita
        labelHasta.setFont(new Font("Arial", Font.BOLD, 22)); // Tamaño 22 y en negrita
        labelDesde.setFont(new Font("Arial", Font.BOLD, 22)); // Tamaño 22 y en negrita
        datePickerHasta.setFont(new Font("Arial", Font.BOLD, 22)); // Tamaño 22 y en negrita
        datePickerDesde.setFont(new Font("Arial", Font.BOLD, 22)); // Tamaño 22 y en negrita

        botonConsultar.addActionListener(new ActionListener() {

            @Override
            public void actionPerformed(ActionEvent e) {

                listaReporteFacturacion = new ArrayList<>();

                Date fechaDesde = datePickerDesde.getDate();
                Date fechaHasta = datePickerHasta.getDate();

                listaComprobantes = comprobanteElectronicoJpaController.obtenerFacturasPorFecha(fechaDesde, fechaHasta);

                for (ComprobanteElectronico comprobante : listaComprobantes) {
                    ObjetoReporteFacturacion objetoReporteFacturacion = new ObjetoReporteFacturacion();
                    objetoReporteFacturacion.setNumeroFactura(comprobante.getNumeroComprobante());
                    objetoReporteFacturacion.setFecha(comprobante.getFechaFirmaXml().toString());
                    FacturasCabecera facturasCabecera = new FacturasCabecera();
                    facturasCabecera = facturasCabeceraJpaController.obtenerPorNumeroFactura(comprobante.getNumeroComprobante());
                    objetoReporteFacturacion.setCliente(facturasCabecera.getCodigoCliente().getNombre());
                    objetoReporteFacturacion.setValorSinIva(facturasCabecera.getValorTotalFactura().doubleValue() / 1.12);
                    objetoReporteFacturacion.setValorFactura(facturasCabecera.getValorTotalFactura().doubleValue());
                    objetoReporteFacturacion.setValorIva(facturasCabecera.getIva().doubleValue());

                    listaReporteFacturacion.add(objetoReporteFacturacion);
                }

                generarExcel();
                JOptionPane.showMessageDialog(null, "Excel generado en C:/Reportes");
                Menu menu = new Menu();
                menu.show();
                ReporteFacturacion.this.dispose();

            }
        });

        botonRegresar.addActionListener(new ActionListener() {

            @Override
            public void actionPerformed(ActionEvent e) {
                Menu menu = new Menu();
                menu.show();
                ReporteFacturacion.this.dispose();
            }
        });

        // Agregar los componentes al panel
        jPanel1.add(labelDesde);
        jPanel1.add(datePickerDesde);
        jPanel1.add(labelHasta);
        jPanel1.add(datePickerHasta);
        jPanel1.add(botonConsultar);
        jPanel1.add(botonRegresar);

        // Configurar el layout GroupLayout
        GroupLayout jPanelLayout = new GroupLayout(jPanel1);
        jPanel1.setLayout(jPanelLayout);
        jPanelLayout.setHorizontalGroup(
                jPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                .addGroup(jPanelLayout.createSequentialGroup()
                        .addContainerGap(40, Short.MAX_VALUE)
                        .addGroup(jPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                .addComponent(labelDesde)
                                .addComponent(labelHasta))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(jPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                                .addComponent(datePickerDesde, javax.swing.GroupLayout.DEFAULT_SIZE, 200, Short.MAX_VALUE)
                                .addComponent(datePickerHasta, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                        .addContainerGap(40, Short.MAX_VALUE))
                .addGroup(jPanelLayout.createSequentialGroup()
                        .addGap(150, 150, 150)
                        .addComponent(botonConsultar)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(botonRegresar)
                        .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        jPanelLayout.setVerticalGroup(
                jPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                .addGroup(jPanelLayout.createSequentialGroup()
                        .addContainerGap(30, Short.MAX_VALUE)
                        .addGroup(jPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                                .addComponent(datePickerDesde, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                                .addComponent(labelDesde, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(jPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                                .addComponent(datePickerHasta, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                                .addComponent(labelHasta, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                        .addGap(18, 18, 18)
                        .addGroup(jPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                                .addComponent(botonConsultar)
                                .addComponent(botonRegresar))
                        .addContainerGap(30, Short.MAX_VALUE))
        );

        pack();
    }

    private void generarExcel() {
        String[] titulos = {"Numero factura", "Cliente", "Fecha", "Valor sin Iva", "IVA", "Valor Total"};
        try (Workbook workbook = new XSSFWorkbook()) {
            Sheet sheet = workbook.createSheet("Reporte facturación");

            // Encabezado
            Row headerRow = sheet.createRow(0);
            for (int i = 0; i < titulos.length; i++) {
                Cell cell = headerRow.createCell(i);
                cell.setCellValue(titulos[i]);
            }

            // Contenido
            int rowNum = 1;
            DecimalFormat decimalFormat = new DecimalFormat("#.00");

            CellStyle numericStyle = workbook.createCellStyle();
            numericStyle.setDataFormat((short) BuiltinFormats.getBuiltinFormat("#,##0.00"));

            CellStyle dateStyle = workbook.createCellStyle();
            dateStyle.setDataFormat((short) 14); // Formato "día mes año" (14)

            for (ObjetoReporteFacturacion objeto : listaReporteFacturacion) {
                Row row = sheet.createRow(rowNum++);

                row.createCell(0).setCellValue(objeto.getNumeroFactura());
                row.createCell(1).setCellValue(objeto.getCliente());

                Cell fechaCell = row.createCell(2);
                fechaCell.setCellValue(objeto.getFecha());
                fechaCell.setCellStyle(dateStyle); // Aplicar estilo de fecha

                // Formatear celdas como números
                Cell valorSinIvaCell = row.createCell(3);
                valorSinIvaCell.setCellValue(objeto.getValorSinIva());
                valorSinIvaCell.setCellStyle(numericStyle);

                Cell valorTotalCell = row.createCell(4);
                valorTotalCell.setCellValue(objeto.getValorIva());
                valorTotalCell.setCellStyle(numericStyle);

                Cell ivaCell = row.createCell(5);
                ivaCell.setCellValue(objeto.getValorFactura());
                ivaCell.setCellStyle(numericStyle);
            }

            // Ajustar el ancho de las columnas automáticamente
            for (int i = 0; i < titulos.length; i++) {
                sheet.autoSizeColumn(i);
            }

            // Guardar el archivo Excel
            try (FileOutputStream fileOut = new FileOutputStream("C:/Reportes/reporteFacturacion.xlsx")) {
                workbook.write(fileOut);
            }
            System.out.println("Archivo Excel generado exitosamente.");
        } catch (Exception e) {
            System.out.println("Error al exportar a Excel: " + e.getMessage());
        }
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

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 500, Short.MAX_VALUE)
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 320, Short.MAX_VALUE)
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
            .addGroup(layout.createSequentialGroup()
                .addComponent(jPanel1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, Short.MAX_VALUE))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

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
            java.util.logging.Logger.getLogger(ReporteFacturacion.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(ReporteFacturacion.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(ReporteFacturacion.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(ReporteFacturacion.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new ReporteFacturacion().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JPanel jPanel1;
    // End of variables declaration//GEN-END:variables
    private JDateChooser datePickerDesde = new JDateChooser();
    private JDateChooser datePickerHasta = new JDateChooser();
    private JLabel labelDesde = new JLabel("Desde:");
    JLabel labelHasta = new JLabel("Hasta:");
    JButton botonConsultar = new JButton("Consultar");
    JButton botonRegresar = new JButton("Regresar");

}
