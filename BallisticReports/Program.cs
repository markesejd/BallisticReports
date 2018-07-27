using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BallisticReports.Database;
using BallisticReports.DataImporters;
using log4net;
using Microsoft.Owin.Cors;
using Neo4jClient;
using Owin;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.Owin;

namespace BallisticReports
{
    public class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            var builder = new ContainerBuilder();

            builder.Register(context => NeoServerConfiguration.GetConfiguration(new Uri("http://localhost:7474/db/data"), "neo4j", "falcons"))
                .SingleInstance();

            builder.RegisterType<GraphClientFactory>()
                .As<IGraphClientFactory>()
                .SingleInstance();

            builder.Register(cc => new WriteDatabase(cc.Resolve<IGraphClientFactory>()))
                .As<IWriteDb>()
                .SingleInstance();


            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var scheduledImports = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var assemblyImports = (from t in assembly.GetTypes()
                    where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ScheduledImport))
                    select t).ToList();
                scheduledImports.AddRange(assemblyImports);
            }

            foreach (var scheduledImport in scheduledImports)
            {
                builder.RegisterType(scheduledImport)
                    .Named<IScheduledImport>(scheduledImport.Name)
                    .As<IScheduledImport>()
                    .SingleInstance();
            }


            builder.Register(c => new DataImporter(c.Resolve<IEnumerable<IScheduledImport>>())).As<DataImporter>().SingleInstance();

            var container = builder.Build();

            HostFactory.Run(x =>
            {
                x.UseAutofacContainer(container);
                x.Service<DataImporter>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());

                    s.OwinEndpoint(app =>
                    {
                        app.Domain = "localhost";
                        app.Port = 45678;
                        app.ConfigureAppBuilder(appBuilder =>
                        {
                            appBuilder.UseCors(CorsOptions.AllowAll);
                            appBuilder.UseAutofacMiddleware(container);
                            appBuilder.UseNancy(bt => bt.Bootstrapper = new NancyBootstrapper(container));
                        });
                    });
                });

                x.RunAsPrompt();
                x.SetDescription("Ballistic Reports UI");
                x.SetDisplayName("BallisticReports");
                x.SetServiceName("BallisticReports");
                x.StartAutomatically();
            });
        }
    }
}
