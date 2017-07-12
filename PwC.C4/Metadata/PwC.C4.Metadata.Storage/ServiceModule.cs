using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;


namespace PwC.C4.Metadata.Storage
{
    internal class ServiceModule : NinjectModule
    {
        private string _connName = null;
        private string _entityName = null;
        private string _searchProvider = null;

        public ServiceModule(string connectName, string entityName = null, string searchProvider = null)
        {
            _connName = connectName;
            _entityName = entityName;
            _searchProvider = searchProvider;
        }

        private LogWrapper log = new LogWrapper();

        public override void Load()
        {
            try
            {
                var storage = AppSettings.Instance.GetStorage();
                switch (storage.ToLower())
                {
                    case "mongodb":
                        Bind<IEntityService>()
                            .To<MongoDb.Service.EntityService>()
                            .WithConstructorArgument("connectName", _connName)
                            .WithConstructorArgument("entityName", _entityName)
                            .WithConstructorArgument("searchProvider", _searchProvider);
                        Bind<IAttachmentService>()
                            .To<MongoDb.Service.AttachmentService>()
                            .WithConstructorArgument("connectName", _connName)
                            .WithConstructorArgument("entityName", _entityName);
                        break;
                    case "mssql":
                        Bind<IEntityService>()
                            .To<Mssql.Service.EntityService>()
                            .WithConstructorArgument("connectName", _connName)
                            .WithConstructorArgument("entityName", _entityName)
                            .WithConstructorArgument("searchProvider", _searchProvider);
                        Bind<IAttachmentService>()
                            .To<Mssql.Service.AttachmentService>()
                            .WithConstructorArgument("connectName", _connName)
                            .WithConstructorArgument("entityName", _entityName);
                        break;
                }
            }
            catch (Exception ee)
            {
                log.Error("Init ninject error", ee);
            }
        }
    }
}
