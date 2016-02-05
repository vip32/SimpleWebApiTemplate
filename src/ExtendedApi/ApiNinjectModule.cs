using ExtendedApi.Providers;
using Ninject.Modules;
using Serilog;

namespace ExtendedApi
{
    public class ApiNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IMessageProvider>().To<MessageProvider>();
        }
    }
}