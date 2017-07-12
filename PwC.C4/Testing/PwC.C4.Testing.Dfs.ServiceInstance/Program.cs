using System;
using System.ServiceModel;
using PwC.C4.Configuration.WcfSettings;
using PwC.C4.Dfs.Common;
using PwC.C4.Dfs.Common.Config;

namespace PwC.C4.Testing.Dfs.ServiceInstance
{
    class Program
    {
        static ServiceHost _host = null;
        static FileRepositoryService _service = null;

        static void Main(string[] args)
        {
            var dfsConfig = PwC.C4.Dfs.Common.Config.DfsServerConfig.Instance.ServerRootFolder;
            _service = new FileRepositoryService();
            _service.RepositoryDirectory = dfsConfig;

            _service.FileRequested += new FileEventHandler(Service_FileRequested);
            _service.FileUploaded += new FileEventHandler(Service_FileUploaded);
            _service.FileDeleted += new FileEventHandler(Service_FileDeleted);

            //_host = new ServiceHost(_service);
            //_host.Faulted += new EventHandler(Host_Faulted);
            var serviceConfig = DfsServerConfig.Instance;
            _service = new FileRepositoryService { RepositoryDirectory = serviceConfig.ServerRootFolder };
            WcfSetting wcf = WcfSettings.Instance.GetWcfSetting("C4DfsServerService");
            _host = new ServiceHost(_service);
            _host.AddServiceEndpoint(typeof(IFileRepositoryService), wcf.Binding, wcf.Endpoint.Uri);

           
            try
            {
                if (_host.State != CommunicationState.Opening)
                {
                    _host.Open();
                    Console.WriteLine("Press a key to close the service");
                    Console.ReadKey();
                }

            }
            finally
            {
                _host.Close();
            }
        }

        static void Host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Host faulted; reinitialising host");
            _host.Abort();
        }

        static void Service_FileRequested(object sender, FileEventArgs e)
        {
            Console.WriteLine(string.Format("File access\t{0}\t{1}", e.VirtualPath, DateTime.Now));
        }

        static void Service_FileUploaded(object sender, FileEventArgs e)
        {
            Console.WriteLine(string.Format("File upload\t{0}\t{1}", e.VirtualPath, DateTime.Now));
        }

        static void Service_FileDeleted(object sender, FileEventArgs e)
        {
            Console.WriteLine(string.Format("File deleted\t{0}\t{1}", e.VirtualPath, DateTime.Now));
        }
    }
}
