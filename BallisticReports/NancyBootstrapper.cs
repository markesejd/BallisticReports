using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;

namespace BallisticReports
{
    public class NancyBootstrapper : AutofacNancyBootstrapper
    {
        private readonly ILifetimeScope _appScope;
        public NancyBootstrapper(ILifetimeScope container)
        {
            _appScope = container;
        }
        protected override ILifetimeScope GetApplicationContainer()
        {
            return _appScope;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during application startup.
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) => ctx.Response
                .WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS")
                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type, Authorization"));
        }
    }
}
