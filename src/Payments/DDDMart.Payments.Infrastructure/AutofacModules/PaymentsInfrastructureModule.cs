using Autofac;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DDDMart.Payments.Infrastructure.AutofacModules
{
    public class PaymentsInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<PaymentsContext>()
                    .UseSqlite(connection)
                    .Options;

            builder.RegisterType<PaymentsContext>()
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
