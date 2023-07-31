﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace="http://tempuri.org", ConfigurationName="wsplanoSoap")]
public interface wsplanoSoap
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Procesar", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> ProcesarAsync(logininfo login, string file, int formato);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ObtenerLink", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> ObtenerLinkAsync(logininfo login, string tpomov, string folio, string tipo, string cedible);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getBoletaTicket", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> getBoletaTicketAsync(logininfo login, string ticket);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getXMLDte", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<getXMLDteResponse> getXMLDteAsync(getXMLDteRequest request);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AltNum", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> AltNumAsync(logininfo login, string folio, string tipo, string campo);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RegIP", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> RegIPAsync(logininfo login, string puerto);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getIP", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> getIPAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/EliminarDoc", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> EliminarDocAsync(logininfo login, string tpomov, string folio, string tipo);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AnularGuia", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> AnularGuiaAsync(logininfo login, string tpomov, string folio, string tipo);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Online", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> OnlineAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Version", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> VersionAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ProcesarWSMasivo", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> ProcesarWSMasivoAsync(logininfo login, string file, int formato);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ConsultarEstado", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> ConsultarEstadoAsync(logininfo login, int trackid);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ConsultaLibroVentas", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> ConsultaLibroVentasAsync(logininfo login, string fechadocumento, string tipolibro);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/LibroVentasDetallado", ReplyAction="*")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
    System.Threading.Tasks.Task<string> LibroVentasDetalladoAsync(logininfo login, string fechadocumento, string tipolibro);
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org")]
public partial class logininfo
{
    
    private string usuarioField;
    
    private string rutField;
    
    private string claveField;
    
    private string puertoField;
    
    private string incluyeLinkField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public string Usuario
    {
        get
        {
            return this.usuarioField;
        }
        set
        {
            this.usuarioField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    public string Rut
    {
        get
        {
            return this.rutField;
        }
        set
        {
            this.rutField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public string Clave
    {
        get
        {
            return this.claveField;
        }
        set
        {
            this.claveField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    public string Puerto
    {
        get
        {
            return this.puertoField;
        }
        set
        {
            this.puertoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Order=4)]
    public string IncluyeLink
    {
        get
        {
            return this.incluyeLinkField;
        }
        set
        {
            this.incluyeLinkField = value;
        }
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName="getXMLDte", WrapperNamespace="http://tempuri.org", IsWrapped=true)]
public partial class getXMLDteRequest
{
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org", Order=0)]
    public logininfo login;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org", Order=1)]
    public string tpomov;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org", Order=2)]
    public string folio;
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org", Order=3)]
    public string tipo;
    
    public getXMLDteRequest()
    {
    }
    
    public getXMLDteRequest(logininfo login, string tpomov, string folio, string tipo)
    {
        this.login = login;
        this.tpomov = tpomov;
        this.folio = folio;
        this.tipo = tipo;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName="getXMLDteResponse", WrapperNamespace="http://tempuri.org", IsWrapped=true)]
public partial class getXMLDteResponse
{
    
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org", Order=0)]
    [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
    public byte[] getXMLDteResult;
    
    public getXMLDteResponse()
    {
    }
    
    public getXMLDteResponse(byte[] getXMLDteResult)
    {
        this.getXMLDteResult = getXMLDteResult;
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public interface wsplanoSoapChannel : wsplanoSoap, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public partial class wsplanoSoapClient : System.ServiceModel.ClientBase<wsplanoSoap>, wsplanoSoap
{
    
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
    ct
    public wsplanoSoapClient(EndpointConfiguration endpointConfiguration) : 
            base(wsplanoSoapClient.GetBindingForEndpoint(endpointConfiguration), wsplanoSoapClient.GetEndpointAddress(endpointConfiguration))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public wsplanoSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
            base(wsplanoSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public wsplanoSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(wsplanoSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }
    
    public wsplanoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public System.Threading.Tasks.Task<string> ProcesarAsync(logininfo login, string file, int formato)
    {
        return base.Channel.ProcesarAsync(login, file, formato);
    }
    
    public System.Threading.Tasks.Task<string> ObtenerLinkAsync(logininfo login, string tpomov, string folio, string tipo, string cedible)
    {
        return base.Channel.ObtenerLinkAsync(login, tpomov, folio, tipo, cedible);
    }
    
    public System.Threading.Tasks.Task<string> getBoletaTicketAsync(logininfo login, string ticket)
    {
        return base.Channel.getBoletaTicketAsync(login, ticket);
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<getXMLDteResponse> wsplanoSoap.getXMLDteAsync(getXMLDteRequest request)
    {
        return base.Channel.getXMLDteAsync(request);
    }
    
    public System.Threading.Tasks.Task<getXMLDteResponse> getXMLDteAsync(logininfo login, string tpomov, string folio, string tipo)
    {
        getXMLDteRequest inValue = new getXMLDteRequest();
        inValue.login = login;
        inValue.tpomov = tpomov;
        inValue.folio = folio;
        inValue.tipo = tipo;
        return ((wsplanoSoap)(this)).getXMLDteAsync(inValue);
    }
    
    public System.Threading.Tasks.Task<string> AltNumAsync(logininfo login, string folio, string tipo, string campo)
    {
        return base.Channel.AltNumAsync(login, folio, tipo, campo);
    }
    
    public System.Threading.Tasks.Task<string> RegIPAsync(logininfo login, string puerto)
    {
        return base.Channel.RegIPAsync(login, puerto);
    }
    
    public System.Threading.Tasks.Task<string> getIPAsync()
    {
        return base.Channel.getIPAsync();
    }
    
    public System.Threading.Tasks.Task<string> EliminarDocAsync(logininfo login, string tpomov, string folio, string tipo)
    {
        return base.Channel.EliminarDocAsync(login, tpomov, folio, tipo);
    }
    
    public System.Threading.Tasks.Task<string> AnularGuiaAsync(logininfo login, string tpomov, string folio, string tipo)
    {
        return base.Channel.AnularGuiaAsync(login, tpomov, folio, tipo);
    }
    
    public System.Threading.Tasks.Task<string> OnlineAsync()
    {
        return base.Channel.OnlineAsync();
    }
    
    public System.Threading.Tasks.Task<string> VersionAsync()
    {
        return base.Channel.VersionAsync();
    }
    
    public System.Threading.Tasks.Task<string> ProcesarWSMasivoAsync(logininfo login, string file, int formato)
    {
        return base.Channel.ProcesarWSMasivoAsync(login, file, formato);
    }
    
    public System.Threading.Tasks.Task<string> ConsultarEstadoAsync(logininfo login, int trackid)
    {
        return base.Channel.ConsultarEstadoAsync(login, trackid);
    }
    
    public System.Threading.Tasks.Task<string> ConsultaLibroVentasAsync(logininfo login, string fechadocumento, string tipolibro)
    {
        return base.Channel.ConsultaLibroVentasAsync(login, fechadocumento, tipolibro);
    }
    
    public System.Threading.Tasks.Task<string> LibroVentasDetalladoAsync(logininfo login, string fechadocumento, string tipolibro)
    {
        return base.Channel.LibroVentasDetalladoAsync(login, fechadocumento, tipolibro);
    }
    
    public virtual System.Threading.Tasks.Task OpenAsync()
    {
        return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
    }
    
    private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.wsplanoSoap))
        {
            System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
            result.MaxBufferSize = int.MaxValue;
            result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            result.MaxReceivedMessageSize = int.MaxValue;
            result.AllowCookies = true;
            return result;
        }
        if ((endpointConfiguration == EndpointConfiguration.wsplanoSoap12))
        {
            System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
            System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
            textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
            result.Elements.Add(textBindingElement);
            System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
            httpBindingElement.AllowCookies = true;
            httpBindingElement.MaxBufferSize = int.MaxValue;
            httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
            result.Elements.Add(httpBindingElement);
            return result;
        }
        throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
    }
    
    private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.wsplanoSoap))
        {
            return new System.ServiceModel.EndpointAddress("http://ws.facturacion.cl/WSDS/wsplano.asmx");
        }
        if ((endpointConfiguration == EndpointConfiguration.wsplanoSoap12))
        {
            return new System.ServiceModel.EndpointAddress("http://ws.facturacion.cl/WSDS/wsplano.asmx");
        }
        throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
    }
    
    public enum EndpointConfiguration
    {
        
        wsplanoSoap,
        
        wsplanoSoap12,
    }
}
