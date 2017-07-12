using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Serialization;

namespace PwC.C4.Configuration.WcfSettings
{
    internal static class Options
    {
        public static Dictionary<string, SecurityMode> SecurityModes = new Dictionary<string, SecurityMode>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"None", SecurityMode.None},
            {"Message", SecurityMode.Message},
            {"Transport", SecurityMode.Transport},
            {"TransportWithMessageCredential", SecurityMode.TransportWithMessageCredential}
        };
        public static Dictionary<string, TransferMode> TransferModes = new Dictionary<string, TransferMode>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"Buffered", TransferMode.Buffered},
            {"Streamed", TransferMode.Streamed},
            {"StreamedRequest", TransferMode.StreamedRequest},
            {"StreamedResponse", TransferMode.StreamedResponse}
        };

    }



    [XmlRoot("WcfSettings")]
    public class WcfSettings : BaseConfig<WcfSettings>
    {

        [XmlElement("WcfSetting")]
        public List<WcfNode> Wcfs { get; set; }

        public WcfSetting GetWcfSetting(string name)
        {
            var wcf = new WcfSetting();
            var node = this.Wcfs.FirstOrDefault(c => c.Name == name);
            if (node?.Endpoint?.AddressStr != null)
            {
                var se = System.ServiceModel.SecurityMode.None;
                if (!string.IsNullOrEmpty(node.Binding.SecurityMode))
                {
                    se = Options.SecurityModes[node.Binding.SecurityMode];
                }
                var tr = System.ServiceModel.TransferMode.Buffered;
                if (!string.IsNullOrEmpty(node.Binding.TransferMode))
                {
                    tr = Options.TransferModes[node.Binding.TransferMode];
                }
                var isHttps = node.Endpoint.AddressStr.StartsWith("https");


                var ts = TimeSpan.Parse(node.Binding.TimeOut);
                var basicHttpBinding = new BasicHttpBinding((BasicHttpSecurityMode) se)
                {
                    TransferMode = tr,
                    MaxReceivedMessageSize = Int32.MaxValue,
                    OpenTimeout = ts,
                    CloseTimeout = ts,
                    ReceiveTimeout = ts,
                    SendTimeout = ts,
                    MaxBufferPoolSize = 2147483647,
                    MaxBufferSize = 2147483647,
                    ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                    {
                        MaxArrayLength = 2147483647,
                        MaxBytesPerRead = 2147483647,
                        MaxNameTableCharCount = 2147483647,
                        MaxDepth = 2147483647,
                        MaxStringContentLength = 2147483647
                    }
                };
                if (isHttps)
                {
                    basicHttpBinding.Security = new BasicHttpSecurity
                    {
                        Mode = BasicHttpSecurityMode.Transport,
                        Transport = new HttpTransportSecurity()
                        {
                            ClientCredentialType = HttpClientCredentialType.None
                        }
                    };
                }
                switch (node.Binding.Type.ToLower())
                {
                    case "basichttpbinding":
                        wcf.Binding = basicHttpBinding;
                        break;
                    case "wshttpbinding":
                        var wsHttpBinding = new WSHttpBinding(se)
                        {
                            MaxReceivedMessageSize = node.Binding.MaxReceivedMessageSize,
                            OpenTimeout = ts,
                            CloseTimeout = ts,
                            ReceiveTimeout = ts,
                            SendTimeout = ts
                        };
                        wcf.Binding = wsHttpBinding;
                        break;
                    case "nettcpbinding":
                        var tcp = new NetTcpBinding(se)
                        {
                            MaxReceivedMessageSize = node.Binding.MaxReceivedMessageSize,
                            CloseTimeout = ts,
                            OpenTimeout = ts,
                            ReceiveTimeout = ts,
                            SendTimeout = ts,
                            TransferMode = tr,
                            MaxBufferPoolSize = 2147483647,
                            MaxBufferSize = 2147483647,
                            ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                            {
                                MaxArrayLength = 2147483647,
                                MaxBytesPerRead = 2147483647,
                                MaxNameTableCharCount = 2147483647,
                                MaxDepth = 2147483647,
                                MaxStringContentLength = 2147483647
                            }
                        };
                        wcf.Binding = tcp;
                        break;
                    case "nethttpbinding":
                        var netHttpBinding = new NetHttpBinding((BasicHttpSecurityMode) se)
                        {
                            TransferMode = tr,
                            MaxReceivedMessageSize = node.Binding.MaxReceivedMessageSize,
                            OpenTimeout = ts,
                            CloseTimeout = ts,
                            ReceiveTimeout = ts,
                            SendTimeout = ts
                        };
                        wcf.Binding = netHttpBinding;
                        break;
                    case "nethttpsbinding":
                        var netHttpsBinding = new NetHttpsBinding((BasicHttpsSecurityMode) se)
                        {
                            TransferMode = tr,
                            MaxReceivedMessageSize = node.Binding.MaxReceivedMessageSize,
                            OpenTimeout = ts,
                            CloseTimeout = ts,
                            ReceiveTimeout = ts,
                            SendTimeout = ts
                        };
                        break;
                    default:
                        wcf.Binding = basicHttpBinding;
                        break;
                }
                wcf.Endpoint = new EndpointAddress(node.Endpoint.AddressStr);
            }

            return wcf;
        }
    }

    public class WcfSetting
    {
        public Binding Binding { get; set; }

        public EndpointAddress Endpoint { get; set; }
    }

    public class WcfNode
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("binding")]
        public BindingNode Binding { get; set; }

        [XmlElement("endpoint")]
        public EndpointNode Endpoint { get; set; }

    }

    public class BindingNode
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("securityMode")]
        public string SecurityMode { get; set; }
        [XmlAttribute("transferMode")]
        public string TransferMode { get; set; }
        [XmlAttribute("maxReceivedMessageSize")]
        public long MaxReceivedMessageSize { get; set; }
        [XmlAttribute("timeOut")]
        public string TimeOut { get; set; }
        [XmlAttribute("maxConnections")]
        public int MaxConnections { get; set; }
    }

    public class EndpointNode
    {
        [XmlAttribute("address")]
        public string AddressStr { get; set; }
    }
}

