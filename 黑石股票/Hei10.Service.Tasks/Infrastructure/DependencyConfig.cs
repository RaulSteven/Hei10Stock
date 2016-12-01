using Autofac;
using Hei10.Core.Cache;
using Hei10.Domain.Entityframework;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;

namespace Hei10.Service.Tasks.Infrastructure
{
    public class DependencyConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DbFactory>().As<IDbFactory>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>))
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(AttachmentRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            builder.RegisterAssemblyTypes(typeof(UserRoleSvc).Assembly)
                .Where(t => t.Name.EndsWith("Svc"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            builder.RegisterType<MemoryCacheManager>()
                .As<ICacheManager>()
                .SingleInstance()
                .PropertiesAutowired();

            builder.RegisterType<SysConfigRepository>()
                .As<ISysConfigRepository>()
                .InstancePerLifetimeScope();

            var container = builder.Build();
            Container = container;
        }

        public static IContainer Container { get; private set; }
    }
}