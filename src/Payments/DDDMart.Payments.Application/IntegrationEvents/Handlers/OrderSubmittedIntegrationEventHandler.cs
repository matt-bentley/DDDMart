using DDDMart.Application;
using DDDMart.Payments.Core.Invoices.Entities;
using DDDMart.Payments.Core.Invoices.Repositories;
using DDDMart.Payments.Core.Invoices.ValueObjects;
using Microsoft.Extensions.Logging;

namespace DDDMart.Payments.Application.IntegrationEvents.Handlers
{
    public class OrderSubmittedIntegrationEventHandler : IIntegrationEventHandler<OrderSubmittedIntegrationEvent>
    {
        private readonly ILogger<OrderSubmittedIntegrationEventHandler> _logger;
        private readonly IInvoicesRepository _invoicesRepository;

        public OrderSubmittedIntegrationEventHandler(ILogger<OrderSubmittedIntegrationEventHandler> logger,
            IInvoicesRepository invoicesRepository)
        {
            _logger = logger;
            _invoicesRepository = invoicesRepository;
        }

        public async Task HandleAsync(OrderSubmittedIntegrationEvent @event)
        {
            _logger.LogInformation("Generating invoice for order {order}", @event.Id);
            var invoice = Invoice.Generate(@event.CustomerId, @event.Id, @event.PaymentMethodId, InvoiceAddress.Create(@event.PaymentStreet, @event.PaymentCity, @event.PaymentState, @event.PaymentCountry, @event.PaymentZipCode), @event.TotalPrice);
            await _invoicesRepository.InsertAsync(invoice);
            await _invoicesRepository.UnitOfWork.CommitAsync();
        }
    }
}
