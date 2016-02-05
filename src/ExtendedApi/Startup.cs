using System.Reflection;
using System.Web.Http;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace ExtendedApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            app.UseNinjectMiddleware(CreateKernel)
                .UseNinjectWebApi(config);
            //app.UseWebApi(config);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(new ApiNinjectModule()); // Assembly.GetExecutingAssembly()
            return kernel;
        }
    }
}
