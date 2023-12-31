﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.42000
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
'
Namespace ec.gob.sri.celcer.autorizacionPruebas
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="AutorizacionComprobantesOfflineServiceSoapBinding", [Namespace]:="http://ec.gob.sri.ws.autorizacion"),  _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(mensaje())),  _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(autorizacion()))>  _
    Partial Public Class AutorizacionComprobantesOfflineService
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private autorizacionComprobanteOperationCompleted As System.Threading.SendOrPostCallback
        
        Private autorizacionComprobanteLoteOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.ClasesComprobantesElectronicos.My.MySettings.Default.ClasesComprobantesElectronicos_ec_gob_sri_celcer_autorizacionPruebas_AutorizacionComprobantesService
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event autorizacionComprobanteCompleted As autorizacionComprobanteCompletedEventHandler
        
        '''<remarks/>
        Public Event autorizacionComprobanteLoteCompleted As autorizacionComprobanteLoteCompletedEventHandler

        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://ec.gob.sri.ws.autorizacion", ResponseNamespace:="http://ec.gob.sri.ws.autorizacion", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>
        Public Function autorizacionComprobante(<System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal claveAccesoComprobante As String) As <System.Xml.Serialization.XmlElementAttribute("RespuestaAutorizacionComprobante", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> Object()
            Return Me.Invoke("autorizacionComprobante", New Object() {claveAccesoComprobante})
        End Function

        '''<remarks/>
        Public Overloads Sub autorizacionComprobanteAsync(ByVal claveAccesoComprobante As String)
            Me.autorizacionComprobanteAsync(claveAccesoComprobante, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub autorizacionComprobanteAsync(ByVal claveAccesoComprobante As String, ByVal userState As Object)
            If (Me.autorizacionComprobanteOperationCompleted Is Nothing) Then
                Me.autorizacionComprobanteOperationCompleted = AddressOf Me.OnautorizacionComprobanteOperationCompleted
            End If
            Me.InvokeAsync("autorizacionComprobante", New Object() {claveAccesoComprobante}, Me.autorizacionComprobanteOperationCompleted, userState)
        End Sub
        
        Private Sub OnautorizacionComprobanteOperationCompleted(ByVal arg As Object)
            If (Not (Me.autorizacionComprobanteCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent autorizacionComprobanteCompleted(Me, New autorizacionComprobanteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://ec.gob.sri.ws.autorizacion", ResponseNamespace:="http://ec.gob.sri.ws.autorizacion", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function autorizacionComprobanteLote(<System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal claveAccesoLote As String) As <System.Xml.Serialization.XmlElementAttribute("RespuestaAutorizacionLote", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> respuestaLote
            Dim results() As Object = Me.Invoke("autorizacionComprobanteLote", New Object() {claveAccesoLote})
            Return CType(results(0),respuestaLote)
        End Function
        
        '''<remarks/>
        Public Overloads Sub autorizacionComprobanteLoteAsync(ByVal claveAccesoLote As String)
            Me.autorizacionComprobanteLoteAsync(claveAccesoLote, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub autorizacionComprobanteLoteAsync(ByVal claveAccesoLote As String, ByVal userState As Object)
            If (Me.autorizacionComprobanteLoteOperationCompleted Is Nothing) Then
                Me.autorizacionComprobanteLoteOperationCompleted = AddressOf Me.OnautorizacionComprobanteLoteOperationCompleted
            End If
            Me.InvokeAsync("autorizacionComprobanteLote", New Object() {claveAccesoLote}, Me.autorizacionComprobanteLoteOperationCompleted, userState)
        End Sub
        
        Private Sub OnautorizacionComprobanteLoteOperationCompleted(ByVal arg As Object)
            If (Not (Me.autorizacionComprobanteLoteCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent autorizacionComprobanteLoteCompleted(Me, New autorizacionComprobanteLoteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://ec.gob.sri.ws.autorizacion")>  _
    Partial Public Class respuestaComprobante
        
        Private claveAccesoConsultadaField As String
        
        Private numeroComprobantesField As String
        
        Private autorizacionesField() As autorizacion
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property claveAccesoConsultada() As String
            Get
                Return Me.claveAccesoConsultadaField
            End Get
            Set
                Me.claveAccesoConsultadaField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property numeroComprobantes() As String
            Get
                Return Me.numeroComprobantesField
            End Get
            Set
                Me.numeroComprobantesField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified),  _
         System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=false)>  _
        Public Property autorizaciones() As autorizacion()
            Get
                Return Me.autorizacionesField
            End Get
            Set
                Me.autorizacionesField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://ec.gob.sri.ws.autorizacion")>  _
    Partial Public Class autorizacion
        
        Private estadoField As String
        
        Private numeroAutorizacionField As String
        
        Private fechaAutorizacionField As Date
        
        Private fechaAutorizacionFieldSpecified As Boolean
        
        Private ambienteField As String
        
        Private comprobanteField As String
        
        Private mensajesField() As mensaje
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property estado() As String
            Get
                Return Me.estadoField
            End Get
            Set
                Me.estadoField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property numeroAutorizacion() As String
            Get
                Return Me.numeroAutorizacionField
            End Get
            Set
                Me.numeroAutorizacionField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property fechaAutorizacion() As Date
            Get
                Return Me.fechaAutorizacionField
            End Get
            Set
                Me.fechaAutorizacionField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property fechaAutorizacionSpecified() As Boolean
            Get
                Return Me.fechaAutorizacionFieldSpecified
            End Get
            Set
                Me.fechaAutorizacionFieldSpecified = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property ambiente() As String
            Get
                Return Me.ambienteField
            End Get
            Set
                Me.ambienteField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property comprobante() As String
            Get
                Return Me.comprobanteField
            End Get
            Set
                Me.comprobanteField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified),  _
         System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=false)>  _
        Public Property mensajes() As mensaje()
            Get
                Return Me.mensajesField
            End Get
            Set
                Me.mensajesField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://ec.gob.sri.ws.autorizacion")>  _
    Partial Public Class mensaje
        
        Private identificadorField As String
        
        Private mensaje1Field As String
        
        Private informacionAdicionalField As String
        
        Private tipoField As String
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property identificador() As String
            Get
                Return Me.identificadorField
            End Get
            Set
                Me.identificadorField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("mensaje", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property mensaje1() As String
            Get
                Return Me.mensaje1Field
            End Get
            Set
                Me.mensaje1Field = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property informacionAdicional() As String
            Get
                Return Me.informacionAdicionalField
            End Get
            Set
                Me.informacionAdicionalField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property tipo() As String
            Get
                Return Me.tipoField
            End Get
            Set
                Me.tipoField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://ec.gob.sri.ws.autorizacion")>  _
    Partial Public Class respuestaLote
        
        Private claveAccesoLoteConsultadaField As String
        
        Private numeroComprobantesLoteField As String
        
        Private autorizacionesField() As autorizacion
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property claveAccesoLoteConsultada() As String
            Get
                Return Me.claveAccesoLoteConsultadaField
            End Get
            Set
                Me.claveAccesoLoteConsultadaField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property numeroComprobantesLote() As String
            Get
                Return Me.numeroComprobantesLoteField
            End Get
            Set
                Me.numeroComprobantesLoteField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified),  _
         System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=false)>  _
        Public Property autorizaciones() As autorizacion()
            Get
                Return Me.autorizacionesField
            End Get
            Set
                Me.autorizacionesField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")>  _
    Public Delegate Sub autorizacionComprobanteCompletedEventHandler(ByVal sender As Object, ByVal e As autorizacionComprobanteCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class autorizacionComprobanteCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As respuestaComprobante
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),respuestaComprobante)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")>  _
    Public Delegate Sub autorizacionComprobanteLoteCompletedEventHandler(ByVal sender As Object, ByVal e As autorizacionComprobanteLoteCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class autorizacionComprobanteLoteCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As respuestaLote
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),respuestaLote)
            End Get
        End Property
    End Class
End Namespace
