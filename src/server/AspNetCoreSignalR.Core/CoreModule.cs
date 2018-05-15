using AspNetCoreSignalR.Common.Dependancies;
using AspNetCoreSignalR.Common.Events;
using AspNetCoreSignalR.Core.Chat;
using Autofac;
using System.Reflection;

namespace AspNetCoreSignalR.Core
{
    public class CoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var core = typeof(CoreModule).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(core)
                   .AssignableTo<IManager>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<DomainEvents>().As<IDomainEvents>();
            builder.RegisterType<ChatManager>().As<IChatManager>().SingleInstance();
        }
    }
}
