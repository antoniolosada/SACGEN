﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.269
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace WS
    
    '''<comentarios/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://servizos.dxfprcpsw/")>  _
    Partial Public Class Exception
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private messageField As String
        
        '''<comentarios/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, Order:=0)>  _
        Public Property message() As String
            Get
                Return Me.messageField
            End Get
            Set
                Me.messageField = value
                Me.RaisePropertyChanged("message")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute([Namespace]:="http://servizos.dxfprcpsw/", ConfigurationName:="WS.SafefpWS")>  _
    Public Interface SafefpWS
        
        'CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        <System.ServiceModel.OperationContractAttribute(Action:="", ReplyAction:="*"),  _
         System.ServiceModel.FaultContractAttribute(GetType(WS.Exception), Action:="", Name:="Exception"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function procesarQuery(ByVal request As WS.procesarQueryRequest) As <System.ServiceModel.MessageParameterAttribute(Name:="return")> WS.procesarQueryResponse
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="procesarQuery", WrapperNamespace:="http://servizos.dxfprcpsw/", IsWrapped:=true)>  _
    Partial Public Class procesarQueryRequest
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://servizos.dxfprcpsw/", Order:=0),  _
         System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public arg0 As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://servizos.dxfprcpsw/", Order:=1),  _
         System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public arg1 As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://servizos.dxfprcpsw/", Order:=2),  _
         System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public arg2 As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://servizos.dxfprcpsw/", Order:=3),  _
         System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public arg3 As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal arg0 As String, ByVal arg1 As String, ByVal arg2 As String, ByVal arg3 As String)
            MyBase.New
            Me.arg0 = arg0
            Me.arg1 = arg1
            Me.arg2 = arg2
            Me.arg3 = arg3
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="procesarQueryResponse", WrapperNamespace:="http://servizos.dxfprcpsw/", IsWrapped:=true)>  _
    Partial Public Class procesarQueryResponse
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://servizos.dxfprcpsw/", Order:=0),  _
         System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public [return] As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal [return] As String)
            MyBase.New
            Me.[return] = [return]
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface SafefpWSChannel
        Inherits WS.SafefpWS, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class SafefpWSClient
        Inherits System.ServiceModel.ClientBase(Of WS.SafefpWS)
        Implements WS.SafefpWS
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Function WS_SafefpWS_procesarQuery(ByVal request As WS.procesarQueryRequest) As WS.procesarQueryResponse Implements WS.SafefpWS.procesarQuery
            Return MyBase.Channel.procesarQuery(request)
        End Function
        
        Public Function procesarQuery(ByVal arg0 As String, ByVal arg1 As String, ByVal arg2 As String, ByVal arg3 As String) As String
            Dim inValue As WS.procesarQueryRequest = New WS.procesarQueryRequest()
            inValue.arg0 = arg0
            inValue.arg1 = arg1
            inValue.arg2 = arg2
            inValue.arg3 = arg3
            Dim retVal As WS.procesarQueryResponse = CType(Me,WS.SafefpWS).procesarQuery(inValue)
            Return retVal.[return]
        End Function
    End Class
End Namespace
