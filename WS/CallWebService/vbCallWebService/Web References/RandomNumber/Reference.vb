﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.573
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
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
'Microsoft.VSDesigner generó automáticamente este código fuente, versión=1.1.4322.573.
'
Namespace RandomNumber
    
    '<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="GeneratorSoap", [Namespace]:="http://fernandolucas.info/")>  _
    Public Class Generator
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        '<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = "http://www.golemproject.com/Apps/96/Generator.asmx"
        End Sub
        
        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://fernandolucas.info/GenerateRandom", RequestNamespace:="http://fernandolucas.info/", ResponseNamespace:="http://fernandolucas.info/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GenerateRandom(ByVal ID As String, ByVal Min As Integer, ByVal Max As Integer, ByVal Count As Integer) As Integer()
            Dim results() As Object = Me.Invoke("GenerateRandom", New Object() {ID, Min, Max, Count})
            Return CType(results(0),Integer())
        End Function
        
        '<remarks/>
        Public Function BeginGenerateRandom(ByVal ID As String, ByVal Min As Integer, ByVal Max As Integer, ByVal Count As Integer, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("GenerateRandom", New Object() {ID, Min, Max, Count}, callback, asyncState)
        End Function
        
        '<remarks/>
        Public Function EndGenerateRandom(ByVal asyncResult As System.IAsyncResult) As Integer()
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0),Integer())
        End Function
        
        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://fernandolucas.info/GenerateRandomNormalized", RequestNamespace:="http://fernandolucas.info/", ResponseNamespace:="http://fernandolucas.info/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GenerateRandomNormalized() As Single
            Dim results() As Object = Me.Invoke("GenerateRandomNormalized", New Object(-1) {})
            Return CType(results(0),Single)
        End Function
        
        '<remarks/>
        Public Function BeginGenerateRandomNormalized(ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("GenerateRandomNormalized", New Object(-1) {}, callback, asyncState)
        End Function
        
        '<remarks/>
        Public Function EndGenerateRandomNormalized(ByVal asyncResult As System.IAsyncResult) As Single
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0),Single)
        End Function
        
        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://fernandolucas.info/GenerateRandomDotOrg", RequestNamespace:="http://fernandolucas.info/", ResponseNamespace:="http://fernandolucas.info/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GenerateRandomDotOrg(ByVal Min As Integer, ByVal Max As Integer, ByVal Count As Integer) As Integer()
            Dim results() As Object = Me.Invoke("GenerateRandomDotOrg", New Object() {Min, Max, Count})
            Return CType(results(0),Integer())
        End Function
        
        '<remarks/>
        Public Function BeginGenerateRandomDotOrg(ByVal Min As Integer, ByVal Max As Integer, ByVal Count As Integer, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("GenerateRandomDotOrg", New Object() {Min, Max, Count}, callback, asyncState)
        End Function
        
        '<remarks/>
        Public Function EndGenerateRandomDotOrg(ByVal asyncResult As System.IAsyncResult) As Integer()
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0),Integer())
        End Function
        
        '<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://fernandolucas.info/GeneratePseudoRandom", RequestNamespace:="http://fernandolucas.info/", ResponseNamespace:="http://fernandolucas.info/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function GeneratePseudoRandom(ByVal Min As Integer, ByVal Max As Integer, ByVal Count As Integer) As Integer()
            Dim results() As Object = Me.Invoke("GeneratePseudoRandom", New Object() {Min, Max, Count})
            Return CType(results(0),Integer())
        End Function
        
        '<remarks/>
        Public Function BeginGeneratePseudoRandom(ByVal Min As Integer, ByVal Max As Integer, ByVal Count As Integer, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("GeneratePseudoRandom", New Object() {Min, Max, Count}, callback, asyncState)
        End Function
        
        '<remarks/>
        Public Function EndGeneratePseudoRandom(ByVal asyncResult As System.IAsyncResult) As Integer()
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0),Integer())
        End Function
    End Class
End Namespace
