using Autofac;
using Autofac.Extensions.DependencyInjection;
using DDDMart;
using DDDMart.Application.AutofacModules;
using DDDMart.Catalogue.Application.AutofacModules;
using DDDMart.Catalogue.Core.Entities;
using DDDMart.Catalogue.Infrastructure;
using DDDMart.Catalogue.Infrastructure.AutofacModules;
using DDDMart.Ordering.Application.AutofacModules;
using DDDMart.Ordering.Core.AutofacModules;
using DDDMart.Ordering.Infrastructure;
using DDDMart.Ordering.Infrastructure.AutofacModules;
using DDDMart.Payments.Application.AutofacModules;
using DDDMart.Payments.Infrastructure;
using DDDMart.Payments.Infrastructure.AutofacModules;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = Host.CreateDefaultBuilder()
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .UseSerilog((hostContext, loggingBuilder) =>
               {
                   loggingBuilder.MinimumLevel.Information()
                       .Enrich.FromLogContext()
                       .WriteTo.Console();
               })
               .ConfigureServices(services =>
               {
                   services.AddMediatR(typeof(Product).Assembly);
                   services.AddHostedService<OrderGenerator>();
                   services.AddHostedService<IntegrationEventsService>();
                   services.AddHostedService<IntegrationEventPublisher<OrderingContext>>();
                   services.AddHostedService<IntegrationEventPublisher<PaymentsContext>>();
                   services.AddHostedService<IntegrationEventPublisher<CatalogueContext>>();
                   services.AddHostedService<IntegrationEventsService>();
               })
               .ConfigureContainer<ContainerBuilder>(container =>
               {
                   container.RegisterModule(new CatalogueApplicationModule());
                   container.RegisterModule(new CatalogueInfrastructureModule());
                   container.RegisterModule(new OrderingApplicationModule());
                   container.RegisterModule(new OrderingCoreModule());
                   container.RegisterModule(new OrderingInfrastructureModule());
                   container.RegisterModule(new PaymentsApplicationModule());
                   container.RegisterModule(new PaymentsInfrastructureModule());
                   container.RegisterModule(new EventBusModule());
               })
               .Build();

await host.RunAsync();