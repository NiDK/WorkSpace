using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PwC.C4.Configuration.WcfSettings;
using PwC.C4.Dfs.Common;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.ReceiveService
{
    public partial class ReceiveService : ServiceBase
    {
        static readonly LogWrapper Log = new LogWrapper();
        static ServiceHost _host = null;
        static FileRepositoryService _service = null;

        public ReceiveService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var serviceConfig = DfsServerConfig.Instance;
                _service = new FileRepositoryService {RepositoryDirectory = serviceConfig.ServerRootFolder};
                WcfSetting wcf = WcfSettings.Instance.GetWcfSetting("C4DfsServerService");
                _host = new ServiceHost(_service);
                _host.AddServiceEndpoint(typeof (IFileRepositoryService), wcf.Binding, wcf.Endpoint.Uri);

                if (_host.State != CommunicationState.Opening)
                    _host.Open();
            }
            catch (Exception ee)
            {
                Log.Error("OnStart error", ee);
            }


        }



        protected override void OnStop()
        {
        }
    }
}
