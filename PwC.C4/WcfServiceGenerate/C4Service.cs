﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Runtime.Serialization.ContractNamespaceAttribute("http://schemas.datacontract.org/2004/07/PwC.C4.Metadata.Metadata", ClrNamespace="PwC.C4.Metadata.Metadata1")]

namespace System.Dynamic
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DynamicObject", Namespace="http://schemas.datacontract.org/2004/07/System.Dynamic")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(PwC.C4.Metadata.Metadata1.DynamicMetadata))]
    public partial class DynamicObject : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
}
namespace PwC.C4.Metadata.Metadata1
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DynamicMetadata", Namespace="http://schemas.datacontract.org/2004/07/PwC.C4.Metadata.Metadata")]
    public partial class DynamicMetadata : System.Dynamic.DynamicObject
    {
        
        private bool IsTranslatoredk__BackingFieldField;
        
        private System.Collections.Generic.Dictionary<string, object> Propertiesk__BackingFieldField;
        
        private System.Guid UniqueIdk__BackingFieldField;
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<IsTranslatored>k__BackingField", IsRequired=true)]
        public bool IsTranslatoredk__BackingField
        {
            get
            {
                return this.IsTranslatoredk__BackingFieldField;
            }
            set
            {
                this.IsTranslatoredk__BackingFieldField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Properties>k__BackingField", IsRequired=true)]
        public System.Collections.Generic.Dictionary<string, object> Propertiesk__BackingField
        {
            get
            {
                return this.Propertiesk__BackingFieldField;
            }
            set
            {
                this.Propertiesk__BackingFieldField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<UniqueId>k__BackingField", IsRequired=true)]
        public System.Guid UniqueIdk__BackingField
        {
            get
            {
                return this.UniqueIdk__BackingFieldField;
            }
            set
            {
                this.UniqueIdk__BackingFieldField = value;
            }
        }
    }
}
namespace PwC.C4.ServiceWCF.Models.ApiModel
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DataCommonApiModelOfDynamicMetadatazs_ScFYER", Namespace="http://schemas.datacontract.org/2004/07/PwC.C4.ServiceWCF.Models.ApiModel")]
    public partial class DataCommonApiModelOfDynamicMetadatazs_ScFYER : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string DataIdField;
        
        private string JsonField;
        
        private PwC.C4.Metadata.Metadata1.DynamicMetadata ModelField;
        
        private System.Collections.Generic.Dictionary<string, string> PropertiesField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataId
        {
            get
            {
                return this.DataIdField;
            }
            set
            {
                this.DataIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Json
        {
            get
            {
                return this.JsonField;
            }
            set
            {
                this.JsonField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public PwC.C4.Metadata.Metadata1.DynamicMetadata Model
        {
            get
            {
                return this.ModelField;
            }
            set
            {
                this.ModelField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.Dictionary<string, string> Properties
        {
            get
            {
                return this.PropertiesField;
            }
            set
            {
                this.PropertiesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FileModel", Namespace="http://schemas.datacontract.org/2004/07/PwC.C4.ServiceWCF.Models.ApiModel")]
    public partial class FileModel : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string FileExtNameField;
        
        private string FileNameField;
        
        private System.IO.MemoryStream FileStreamField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FileExtName
        {
            get
            {
                return this.FileExtNameField;
            }
            set
            {
                this.FileExtNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FileName
        {
            get
            {
                return this.FileNameField;
            }
            set
            {
                this.FileNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.IO.MemoryStream FileStream
        {
            get
            {
                return this.FileStreamField;
            }
            set
            {
                this.FileStreamField = value;
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IC4Service")]
public interface IC4Service
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetData", ReplyAction="http://tempuri.org/IC4Service/GetDataResponse")]
    string GetData(int value);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetData", ReplyAction="http://tempuri.org/IC4Service/GetDataResponse")]
    System.Threading.Tasks.Task<string> GetDataAsync(int value);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetEntity", ReplyAction="http://tempuri.org/IC4Service/GetEntityResponse")]
    PwC.C4.Metadata.Metadata1.DynamicMetadata GetEntity(string dataId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetEntity", ReplyAction="http://tempuri.org/IC4Service/GetEntityResponse")]
    System.Threading.Tasks.Task<PwC.C4.Metadata.Metadata1.DynamicMetadata> GetEntityAsync(string dataId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/SaveFoundation", ReplyAction="http://tempuri.org/IC4Service/SaveFoundationResponse")]
    int SaveFoundation(PwC.C4.ServiceWCF.Models.ApiModel.DataCommonApiModelOfDynamicMetadatazs_ScFYER data, string staffId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/SaveFoundation", ReplyAction="http://tempuri.org/IC4Service/SaveFoundationResponse")]
    System.Threading.Tasks.Task<int> SaveFoundationAsync(PwC.C4.ServiceWCF.Models.ApiModel.DataCommonApiModelOfDynamicMetadatazs_ScFYER data, string staffId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetAnotherEntity", ReplyAction="http://tempuri.org/IC4Service/GetAnotherEntityResponse")]
    System.Collections.Generic.Dictionary<string, object> GetAnotherEntity(string dataId, string entityname);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetAnotherEntity", ReplyAction="http://tempuri.org/IC4Service/GetAnotherEntityResponse")]
    System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, object>> GetAnotherEntityAsync(string dataId, string entityname);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/DownloadContentFile", ReplyAction="http://tempuri.org/IC4Service/DownloadContentFileResponse")]
    System.Tuple<string, string, System.IO.MemoryStream> DownloadContentFile(string fileid);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/DownloadContentFile", ReplyAction="http://tempuri.org/IC4Service/DownloadContentFileResponse")]
    System.Threading.Tasks.Task<System.Tuple<string, string, System.IO.MemoryStream>> DownloadContentFileAsync(string fileid);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetLinkTrackingUrl", ReplyAction="http://tempuri.org/IC4Service/GetLinkTrackingUrlResponse")]
    string GetLinkTrackingUrl(string notesUrl);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetLinkTrackingUrl", ReplyAction="http://tempuri.org/IC4Service/GetLinkTrackingUrlResponse")]
    System.Threading.Tasks.Task<string> GetLinkTrackingUrlAsync(string notesUrl);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/DownloadFile", ReplyAction="http://tempuri.org/IC4Service/DownloadFileResponse")]
    System.Tuple<string, string, System.IO.MemoryStream> DownloadFile(string fileId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/DownloadFile", ReplyAction="http://tempuri.org/IC4Service/DownloadFileResponse")]
    System.Threading.Tasks.Task<System.Tuple<string, string, System.IO.MemoryStream>> DownloadFileAsync(string fileId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/UploadFile", ReplyAction="http://tempuri.org/IC4Service/UploadFileResponse")]
    string UploadFile(PwC.C4.ServiceWCF.Models.ApiModel.FileModel file);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/UploadFile", ReplyAction="http://tempuri.org/IC4Service/UploadFileResponse")]
    System.Threading.Tasks.Task<string> UploadFileAsync(PwC.C4.ServiceWCF.Models.ApiModel.FileModel file);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/SaveEditHistory", ReplyAction="http://tempuri.org/IC4Service/SaveEditHistoryResponse")]
    int SaveEditHistory(PwC.C4.ServiceWCF.Models.ApiModel.DataCommonApiModelOfDynamicMetadatazs_ScFYER data, string staffId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/SaveEditHistory", ReplyAction="http://tempuri.org/IC4Service/SaveEditHistoryResponse")]
    System.Threading.Tasks.Task<int> SaveEditHistoryAsync(PwC.C4.ServiceWCF.Models.ApiModel.DataCommonApiModelOfDynamicMetadatazs_ScFYER data, string staffId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetEditHistory", ReplyAction="http://tempuri.org/IC4Service/GetEditHistoryResponse")]
    System.Collections.Generic.List<PwC.C4.Metadata.Metadata1.DynamicMetadata> GetEditHistory(string dataId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IC4Service/GetEditHistory", ReplyAction="http://tempuri.org/IC4Service/GetEditHistoryResponse")]
    System.Threading.Tasks.Task<System.Collections.Generic.List<PwC.C4.Metadata.Metadata1.DynamicMetadata>> GetEditHistoryAsync(string dataId);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IC4ServiceChannel : IC4Service, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class C4ServiceClient : System.ServiceModel.ClientBase<IC4Service>, IC4Service
{
    
    public C4ServiceClient()
    {
    }
    
    public C4ServiceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public C4ServiceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public C4ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public C4ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string GetData(int value)
    {
        return base.Channel.GetData(value);
    }
    
    public System.Threading.Tasks.Task<string> GetDataAsync(int value)
    {
        return base.Channel.GetDataAsync(value);
    }
    
    public PwC.C4.Metadata.Metadata1.DynamicMetadata GetEntity(string dataId)
    {
        return base.Channel.GetEntity(dataId);
    }
    
    public System.Threading.Tasks.Task<PwC.C4.Metadata.Metadata1.DynamicMetadata> GetEntityAsync(string dataId)
    {
        return base.Channel.GetEntityAsync(dataId);
    }
    
    public int SaveFoundation(PwC.C4.ServiceWCF.Models.ApiModel.DataCommonApiModelOfDynamicMetadatazs_ScFYER data, string staffId)
    {
        return base.Channel.SaveFoundation(data, staffId);
    }
    
    public System.Threading.Tasks.Task<int> SaveFoundationAsync(PwC.C4.ServiceWCF.Models.ApiModel.DataCommonApiModelOfDynamicMetadatazs_ScFYER data, string staffId)
    {
        return base.Channel.SaveFoundationAsync(data, staffId);
    }
    
    public System.Collections.Generic.Dictionary<string, object> GetAnotherEntity(string dataId, string entityname)
    {
        return base.Channel.GetAnotherEntity(dataId, entityname);
    }
    
    public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, object>> GetAnotherEntityAsync(string dataId, string entityname)
    {
        return base.Channel.GetAnotherEntityAsync(dataId, entityname);
    }
    
    public System.Tuple<string, string, System.IO.MemoryStream> DownloadContentFile(string fileid)
    {
        return base.Channel.DownloadContentFile(fileid);
    }
    
    public System.Threading.Tasks.Task<System.Tuple<string, string, System.IO.MemoryStream>> DownloadContentFileAsync(string fileid)
    {
        return base.Channel.DownloadContentFileAsync(fileid);
    }
    
    public string GetLinkTrackingUrl(string notesUrl)
    {
        return base.Channel.GetLinkTrackingUrl(notesUrl);
    }
    
    public System.Threading.Tasks.Task<string> GetLinkTrackingUrlAsync(string notesUrl)
    {
        return base.Channel.GetLinkTrackingUrlAsync(notesUrl);
    }
    
    public System.Tuple<string, string, System.IO.MemoryStream> DownloadFile(string fileId)
    {
        return base.Channel.DownloadFile(fileId);
    }
    
    public System.Threading.Tasks.Task<System.Tuple<string, string, System.IO.MemoryStream>> DownloadFileAsync(string fileId)
    {
        return base.Channel.DownloadFileAsync(fileId);
    }
    
    public string UploadFile(PwC.C4.ServiceWCF.Models.ApiModel.FileModel file)
    {
        return base.Channel.UploadFile(file);
    }
    
    public System.Threading.Tasks.Task<string> UploadFileAsync(PwC.C4.ServiceWCF.Models.ApiModel.FileModel file)
    {
        return base.Channel.UploadFileAsync(file);
    }
    
    public int SaveEditHistory(PwC.C4.ServiceWCF.Models.ApiModel.DataCommonApiModelOfDynamicMetadatazs_ScFYER data, string staffId)
    {
        return base.Channel.SaveEditHistory(data, staffId);
    }
    
    public System.Threading.Tasks.Task<int> SaveEditHistoryAsync(PwC.C4.ServiceWCF.Models.ApiModel.DataCommonApiModelOfDynamicMetadatazs_ScFYER data, string staffId)
    {
        return base.Channel.SaveEditHistoryAsync(data, staffId);
    }
    
    public System.Collections.Generic.List<PwC.C4.Metadata.Metadata1.DynamicMetadata> GetEditHistory(string dataId)
    {
        return base.Channel.GetEditHistory(dataId);
    }
    
    public System.Threading.Tasks.Task<System.Collections.Generic.List<PwC.C4.Metadata.Metadata1.DynamicMetadata>> GetEditHistoryAsync(string dataId)
    {
        return base.Channel.GetEditHistoryAsync(dataId);
    }
}
