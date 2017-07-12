using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;


namespace PwC.C4.Metadata.Storage
{

    public static class ProviderFactory 
    {
        public static T GetProvider<T>(string connectName, string entityName = null,string searchProvider=null)
            where T : class
        {
            
            using (var kernel = new StandardKernel(new ServiceModule(connectName, entityName, searchProvider)))
            {
                var provider = kernel.Get<T>();
                return provider;
            }
        }


    }
}
