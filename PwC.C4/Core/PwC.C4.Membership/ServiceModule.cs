using System;
using Ninject.Modules;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership.Config;

namespace PwC.C4.Membership
{
    internal class ServiceModule : NinjectModule
    {
        private string _appCode;
        public ServiceModule(string appCode)
        {
            _appCode = appCode;

        }

        private LogWrapper log = new LogWrapper();

        public override void Load()
        {
            try
            {
                var storage = AppSettings.Instance.GetAuthenticateProvider();
                switch (storage.ToLower())
                {
                    case "applicationcenter":
                        Bind<IUserProvider>()
                            .To<ApplicationCenter.UserProvider>()
                            .WithConstructorArgument("appCode", _appCode);
                        break;
                    case "vprofile":
                        Bind<IUserProvider>()
                            .To<vProfile.UserProvider>()
                            .WithConstructorArgument("appCode", _appCode);
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
