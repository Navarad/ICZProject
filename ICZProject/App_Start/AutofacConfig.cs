using Autofac;
using Autofac.Integration.Mvc;
using ICZProject.Services;
using Microsoft.Owin.Security;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICZProject.App_Start
{
    public static class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<XmlService>().As<IFileService>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<ConfigService>().As<IConfigService>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                  .WriteTo.RollingFile(
                    AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/Log-{Date}.txt")
                  .CreateLogger();
            }).SingleInstance();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}