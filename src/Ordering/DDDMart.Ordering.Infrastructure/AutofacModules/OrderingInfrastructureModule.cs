using Autofac;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DDDMart.Ordering.Infrastructure.AutofacModules
{
    public class OrderingInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<OrderingContext>()
                    .UseSqlite(connection)
                    .Options;

            builder.RegisterType<OrderingContext>()
                .AsSelf()
                .InstancePerRequest()
                .InstancePerLifetimeScope()
                .WithParameter(new NamedParameter("options", options));

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(e => e.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .InstancePerLifetimeScope();
        }
    }
}
