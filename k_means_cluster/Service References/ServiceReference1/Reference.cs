﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace k_means_cluster.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IDataBase")]
    public interface IDataBase {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataBase/getPriceArray", ReplyAction="http://tempuri.org/IDataBase/getPriceArrayResponse")]
        decimal[] getPriceArray(string ticker, System.DateTime start, System.DateTime end);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataBase/getPriceArray", ReplyAction="http://tempuri.org/IDataBase/getPriceArrayResponse")]
        System.Threading.Tasks.Task<decimal[]> getPriceArrayAsync(string ticker, System.DateTime start, System.DateTime end);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDataBaseChannel : k_means_cluster.ServiceReference1.IDataBase, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataBaseClient : System.ServiceModel.ClientBase<k_means_cluster.ServiceReference1.IDataBase>, k_means_cluster.ServiceReference1.IDataBase {
        
        public DataBaseClient() {
        }
        
        public DataBaseClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DataBaseClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataBaseClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataBaseClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public decimal[] getPriceArray(string ticker, System.DateTime start, System.DateTime end) {
            return base.Channel.getPriceArray(ticker, start, end);
        }
        
        public System.Threading.Tasks.Task<decimal[]> getPriceArrayAsync(string ticker, System.DateTime start, System.DateTime end) {
            return base.Channel.getPriceArrayAsync(ticker, start, end);
        }
    }
}