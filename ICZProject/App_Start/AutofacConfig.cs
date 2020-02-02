using Autofac;
using Autofac.Integration.Mvc;
using ICZProject.Services;
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

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<XmlService>().As<IFileService>().InstancePerLifetimeScope().PropertiesAutowired();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}