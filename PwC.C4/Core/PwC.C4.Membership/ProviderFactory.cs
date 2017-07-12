using Ninject;

namespace PwC.C4.Membership
{

    public static class ProviderFactory 
    {
        public static T GetProvider<T>(string appCode)
            where T : class
        {
            
            using (var kernel = new StandardKernel(new ServiceModule(appCode)))
            {
                var provider = kernel.Get<T>();
                return provider;
            }
        }


    }
}
