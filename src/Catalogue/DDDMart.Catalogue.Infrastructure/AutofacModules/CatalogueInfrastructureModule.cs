using Autofac;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DDDMart.Catalogue.Infrastructure.AutofacModules
{
    public class CatalogueInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<CatalogueContext>()
                    .UseSqlite(connection)
                    .Options;

            builder.RegisterType<CatalogueContext>()
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
