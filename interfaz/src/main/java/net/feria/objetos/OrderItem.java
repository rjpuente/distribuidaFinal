/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.objetos;

/**
 *
 * @author rpuente
 */
public class OrderItem {

    char[] temp = new char[]{' '};

    public OrderItem(char delimit) {
        temp = new char[]{delimit};
    }

    public String getItemCantidad(String orderItem) {
        String[] delimitado = orderItem.split("" + temp);
        return delimitado[0];
    }

    public String getItemNombre(String orderItem) {
        String[] delimitado = orderItem.split("" + temp);
        return delimitado[1];
    }

    public String getItemPrecio(String orderItem) {
        String[] delimitado = orderItem.split("" + temp);
        return delimitado[2];
    }

    public String getItemTotal(String orderItem) {
        String[] delimitado = orderItem.split("" + temp);
        return delimitado[3];
    }

    public String generaItem(String cantidad, String nombre, String precio, String total) {
        return cantidad + temp[0] + nombre + temp[0] + precio + temp[0] + total;
    }
}
