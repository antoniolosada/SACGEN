﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=1.1.4322.573.
// 
namespace CallWebService.RandomNumber {
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="GeneratorSoap", Namespace="http://fernandolucas.info/")]
    public class Generator : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        /// <remarks/>
        public Generator() {
            this.Url = "http://www.golemproject.com/Apps/96/Generator.asmx";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://fernandolucas.info/GenerateRandom", RequestNamespace="http://fernandolucas.info/", ResponseNamespace="http://fernandolucas.info/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int[] GenerateRandom(string ID, int Min, int Max, int Count) {
            object[] results = this.Invoke("GenerateRandom", new object[] {
                        ID,
                        Min,
                        Max,
                        Count});
            return ((int[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGenerateRandom(string ID, int Min, int Max, int Count, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GenerateRandom", new object[] {
                        ID,
                        Min,
                        Max,
                        Count}, callback, asyncState);
        }
        
        /// <remarks/>
        public int[] EndGenerateRandom(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((int[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://fernandolucas.info/GenerateRandomNormalized", RequestNamespace="http://fernandolucas.info/", ResponseNamespace="http://fernandolucas.info/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Single GenerateRandomNormalized() {
            object[] results = this.Invoke("GenerateRandomNormalized", new object[0]);
            return ((System.Single)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGenerateRandomNormalized(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GenerateRandomNormalized", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public System.Single EndGenerateRandomNormalized(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Single)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://fernandolucas.info/GenerateRandomDotOrg", RequestNamespace="http://fernandolucas.info/", ResponseNamespace="http://fernandolucas.info/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int[] GenerateRandomDotOrg(int Min, int Max, int Count) {
            object[] results = this.Invoke("GenerateRandomDotOrg", new object[] {
                        Min,
                        Max,
                        Count});
            return ((int[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGenerateRandomDotOrg(int Min, int Max, int Count, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GenerateRandomDotOrg", new object[] {
                        Min,
                        Max,
                        Count}, callback, asyncState);
        }
        
        /// <remarks/>
        public int[] EndGenerateRandomDotOrg(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((int[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://fernandolucas.info/GeneratePseudoRandom", RequestNamespace="http://fernandolucas.info/", ResponseNamespace="http://fernandolucas.info/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int[] GeneratePseudoRandom(int Min, int Max, int Count) {
            object[] results = this.Invoke("GeneratePseudoRandom", new object[] {
                        Min,
                        Max,
                        Count});
            return ((int[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGeneratePseudoRandom(int Min, int Max, int Count, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GeneratePseudoRandom", new object[] {
                        Min,
                        Max,
                        Count}, callback, asyncState);
        }
        
        /// <remarks/>
        public int[] EndGeneratePseudoRandom(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((int[])(results[0]));
        }
    }
}
