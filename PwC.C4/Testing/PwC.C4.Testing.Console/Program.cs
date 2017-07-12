using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PwC.C4.Configuration.Data;
using PwC.C4.Metadata.Config;
using Thrift.Protocol;
using Thrift.Transport;

namespace PwC.C4.Testing.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ////for (var n = 0; n < 100; n++)
            ////{
            ////    //var coll = ConnectionStringProvider.GetConnectionString("dbconn.C4DataKeeper");
            ////    //System.Console.WriteLine("Current connectstring is :" + coll);
            ////    var list = MetadataSettings.Instance.GetEntityShortNameMapping("Entity_AppCode");
            ////    Thread.Sleep(30000);
            ////}
            //var program = new SqlDependencyNotification();
            //program.RegisterDependencyUsingDefaultQueue();
            //System.Console.ReadLine();
            //program.Dispose();
            //TTransport transport = new TSocket("localhost", 1234);
            //TProtocol protocol = new TBinaryProtocol(transport);
            //Hello.Client client = new Hello.Client(protocol);
            //transport.Open();
            //Console.WriteLine("Client calls .....");
            //while (true)
            //{
            //    String str = Console.ReadLine();

            //    Console.WriteLine(client.helloInt(Convert.ToInt32(str)));

            //}
            //transport.Close();

            //Console.ReadLine();
            for (int n = 0; n < 1000; n++)
            {
                var common = ConnectionStringProvider.GetConnectionString("dbconn.C4DataKeeper");
                System.Console.WriteLine(common);
                Thread.Sleep(500);
            }
            System.Console.ReadKey();
        }
    }
}
