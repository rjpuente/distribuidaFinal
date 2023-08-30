/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.controlador;

import javax.persistence.Persistence;

/**
 *
 * @author Deth1
 */
public class EntityManagerFactoryObject {

    public static final javax.persistence.EntityManagerFactory emf = Persistence.createEntityManagerFactory("net.feria_facturacion_feria_jar_1.0-SNAPSHOTPU");

}
