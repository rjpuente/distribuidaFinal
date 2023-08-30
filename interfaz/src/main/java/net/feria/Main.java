package net.feria;

import java.util.Date;
import java.util.Timer;
import net.feria.controlador.FirmadorComprobantesElectronicos;
import net.feria.interfaz.Login;

public class Main {
    public static void main(String[] args) {
        // Crea una instancia de Timer
        Timer timer = new Timer();
        int delay = 0; // Retardo inicial (en milisegundos)
        int periodo = 10000; // Periodo de ejecución (en milisegundos, en este caso 10 segundos)
        
        FirmadorComprobantesElectronicos firmadorComprobantesElectronicos = new FirmadorComprobantesElectronicos();
        
        timer.scheduleAtFixedRate(firmadorComprobantesElectronicos, new Date(), periodo);
        
        Login login = new Login();
        login.show();
    }
}