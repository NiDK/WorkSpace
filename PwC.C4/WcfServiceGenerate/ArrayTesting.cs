﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IArrayTesting")]
public interface IArrayTesting
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IArrayTesting/DoWork", ReplyAction="http://tempuri.org/IArrayTesting/DoWorkResponse")]
    void DoWork();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IArrayTesting/DoWork", ReplyAction="http://tempuri.org/IArrayTesting/DoWorkResponse")]
    System.Threading.Tasks.Task DoWorkAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IArrayTesting/ConnectTesting", ReplyAction="http://tempuri.org/IArrayTesting/ConnectTestingResponse")]
    int ConnectTesting();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IArrayTesting/ConnectTesting", ReplyAction="http://tempuri.org/IArrayTesting/ConnectTestingResponse")]
    System.Threading.Tasks.Task<int> ConnectTestingAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IArrayTesting/ObjectArrayTransfer", ReplyAction="http://tempuri.org/IArrayTesting/ObjectArrayTransferResponse")]
    System.Collections.Generic.Dictionary<string, object> ObjectArrayTransfer();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IArrayTesting/ObjectArrayTransfer", ReplyAction="http://tempuri.org/IArrayTesting/ObjectArrayTransferResponse")]
    System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, object>> ObjectArrayTransferAsync();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IArrayTestingChannel : IArrayTesting, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class ArrayTestingClient : System.ServiceModel.ClientBase<IArrayTesting>, IArrayTesting
{
    
    public ArrayTestingClient()
    {
    }
    
    public ArrayTestingClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public ArrayTestingClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public ArrayTestingClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public ArrayTestingClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public void DoWork()
    {
        base.Channel.DoWork();
    }
    
    public System.Threading.Tasks.Task DoWorkAsync()
    {
        return base.Channel.DoWorkAsync();
    }
    
    public int ConnectTesting()
    {
        return base.Channel.ConnectTesting();
    }
    
    public System.Threading.Tasks.Task<int> ConnectTestingAsync()
    {
        return base.Channel.ConnectTestingAsync();
    }
    
    public System.Collections.Generic.Dictionary<string, object> ObjectArrayTransfer()
    {
        return base.Channel.ObjectArrayTransfer();
    }
    
    public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, object>> ObjectArrayTransferAsync()
    {
        return base.Channel.ObjectArrayTransferAsync();
    }
}
