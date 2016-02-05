using System;
using System.Reflection;
using System.Web.Http;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Serilog;
using Serilog.Enrichers;
using Serilog.Formatting.Json;
using Serilog.Sinks.IOFile;

namespace ExtendedApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CreateLogger();
            var config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            var contextLogger = Log.Logger.ForContext<Startup>();
            contextLogger.Debug("starting webapi {DomainId}", AppDomain.CurrentDomain.Id);
            config.MessageHandlers.Add(new RequestResponseLoggingHandler());

            app.UseNinjectMiddleware(CreateKernel)
                .UseNinjectWebApi(config);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.LiterateConsole(outputTemplate: "{Timestamp:u} [{Level}] {SourceContext}:: {Message}{NewLine}{Exception}")
                .WriteTo.File("c:\\tmp\\ExtendedApi_Log.txt", outputTemplate: "{Timestamp:u} [{Level}] {SourceContext}:: {Message}{NewLine}{Exception}")
                .WriteTo.Sink(new FileSink(@"c:\\tmp\\ExtendedApi_Log.json", new JsonFormatter(false, null, true), null))
                .Enrich.WithProperty("App", "ExtendedApi")
                .Enrich.With(new ThreadIdEnricher(), new MachineNameEnricher())
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
