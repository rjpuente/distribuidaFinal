<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de componentes
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de componentes requiere el siguiente procedimiento
    'Se puede modificar usando el Diseñador de componentes.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.ServiceProcessInstaller1 = New System.ServiceProcess.ServiceProcessInstaller()
		Me.Instalador = New System.ServiceProcess.ServiceInstaller()
		'
		'ServiceProcessInstaller1
		'
		Me.ServiceProcessInstaller1.Password = Nothing
		Me.ServiceProcessInstaller1.Username = Nothing
		'
		'Instalador
		'
		Me.Instalador.Description = "Generación de comprobantes electrónicos"
		Me.Instalador.DisplayName = "Generación de comprobantes electrónicos"
		Me.Instalador.ServiceName = "Servicio de generación de comprobantes electrónicos"
		Me.Instalador.StartType = System.ServiceProcess.ServiceStartMode.Automatic
		'
		'ProjectInstaller
		'
		Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.ServiceProcessInstaller1, Me.Instalador})

	End Sub
	Friend WithEvents ServiceProcessInstaller1 As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents Instalador As System.ServiceProcess.ServiceInstaller

End Class
