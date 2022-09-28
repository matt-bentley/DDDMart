using DDDMart.Catalogue.Core.Products.Entities;
using DDDMart.Catalogue.Core.Products.Repositories;
using DDDMart.Catalogue.Core.Products.ValueObjects;
using DDDMart.Ordering.Core.Baskets.Entities;
using DDDMart.Ordering.Core.Baskets.Repositories;
using DDDMart.Ordering.Core.Common.ValueObjects;
using DDDMart.Ordering.Core.Orders.Entities;
using DDDMart.Ordering.Core.Orders.Factories;
using DDDMart.Ordering.Core.Orders.Repositories;
using DDDMart.Payments.Core.Invoices.Repositories;
using DDDMart.Payments.Core.PaymentMethods.Entities;
using DDDMart.Payments.Core.PaymentMethods.Repositories;
using DDDMart.Payments.Core.PaymentMethods.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DDDMart
{
    public class OrderGenerator : BackgroundService
    {
        private readonly ILogger<OrderGenerator> _logger;
        private readonly IProductsRepository _productsRepository;
        private readonly IBasketsRepository _basketsRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IPaymentMethodsRepository _paymentMethodsRepository;
        private readonly IInvoicesRepository _invoicesRepository;

        public OrderGenerator(ILogger<OrderGenerator> logger,
            IProductsRepository productsRepository,
            IBasketsRepository basketsRepository,
            IOrdersRepository ordersRepository,
            IPaymentMethodsRepository paymentMethodsRepository,
            IInvoicesRepository invoicesRepository)
        {
            _logger = logger;
            _productsRepository = productsRepository;
            _basketsRepository = basketsRepository;
            _ordersRepository = ordersRepository;
            _paymentMethodsRepository = paymentMethodsRepository;
            _invoicesRepository = invoicesRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Generating orders");
            await GenerateProductsAsync();

            var customerId = Guid.NewGuid();
            var basketId = await GenerateCustomerBasketAsync(customerId);
            await CheckoutAsync(basketId);
            var paymentMethodId = await SetupCustomerPaymentMethodAsync("Credit Card", PaymentType.CreditCard, customerId);
            var order = await _ordersRepository.GetAll(false).FirstOrDefaultAsync(e => e.BasketId == basketId);
            await CompleteOrderAsync(order, paymentMethodId);
            await PayInvoiceAsync(order.Id);
        }

        private async Task<Guid> GenerateCustomerBasketAsync(Guid customerId)
        {
            _logger.LogInformation("Generating Customer Basket for: {id}", customerId);
            var products = await _productsRepository.GetAll().ToListAsync();
            
            var basket = Basket.Create(customerId);
            AddProductToBasket(basket, products[0]);
            AddProductToBasket(basket, products[1]);
            AddProductToBasket(basket, products[2]);
            AddProductToBasket(basket, products[2]);
            AddProductToBasket(basket, products[3]);
            AddProductToBasket(basket, products[3]);
            AddProductToBasket(basket, products[3]);
            await _basketsRepository.InsertAsync(basket);
            await _basketsRepository.UnitOfWork.CommitAsync();
            return basket.Id;
        }

        private async Task CheckoutAsync(Guid basketId)
        {
            _logger.LogInformation("Checking-out Customer Basket: {id}", basketId);
            var basket = await _basketsRepository.GetByIdAsync(basketId);
            basket.Checkout();
            await _basketsRepository.UnitOfWork.CommitAsync();
        }

        private async Task<Guid> SetupCustomerPaymentMethodAsync(string name, PaymentType paymentType, Guid customerId)
        {
            _logger.LogInformation("Setting up {method} payment method for customer: {id}", paymentType.ToString(), customerId);
            var paymentMethod = PaymentMethod.Create(name, customerId, paymentType);
            await _paymentMethodsRepository.InsertAsync(paymentMethod);
            await _paymentMethodsRepository.UnitOfWork.CommitAsync();
            return paymentMethod.Id;
        }

        private async Task CompleteOrderAsync(Order order, Guid paymentMethodId)
        {
            _logger.LogInformation("Setting up delivery address for {order}", order.Id);
            var addressFactory = new AddressFactory();
            var shippingAddress = addressFactory.CreateShipping("4 Feather Lane", "City", "New York", "USA", "543534");
            order.UpdateShippingAddress(shippingAddress);
            _logger.LogInformation("Setting up payment method for {order}", order.Id);
            var paymentAddress = addressFactory.CreatePayment("4 Feather Lane", "City", "New York", "USA", "543534");
            order.UpdatePaymentMethod(paymentMethodId, paymentAddress);
            _logger.LogInformation("Submitting order {order}", order.Id);
            order.Submit();
            await _ordersRepository.UnitOfWork.CommitAsync();
        }

        private async Task PayInvoiceAsync(Guid orderId)
        {
            _logger.LogInformation("Searching for invoice for order {order}", orderId);
            var invoice = await _invoicesRepository.GetAll(false).FirstOrDefaultAsync(e => e.OrderId == orderId);
            while (invoice == null)
            {
                await Task.Delay(1000);
                invoice = await _invoicesRepository.GetAll(false).FirstOrDefaultAsync(e => e.OrderId == orderId);
            }
            _logger.LogInformation("Paying invoice {invoice}", invoice.Id);
            invoice.Pay();
            await _invoicesRepository.UnitOfWork.CommitAsync();
        }

        private void AddProductToBasket(Basket basket, Product product)
        {
            _logger.LogInformation("Adding {name} to basket", product.Info.Name);
            basket.AddItem(new OrderProduct(product.Id, product.Info.Name, product.Price));
        }

        private async Task GenerateProductsAsync()
        {
            _logger.LogInformation("Generating Product Catalogue");
            await _productsRepository.InsertAsync(Product.Create(ProductInfo.Create("SQL Server", "SQL Server relational database"), "0001", 100, Picture.Create("SQL Server", "https://ddmart.com/products/0001")));
            await _productsRepository.InsertAsync(Product.Create(ProductInfo.Create("MongoDB Cluster", "MongoDB database cluster"), "0002", 200, Picture.Create("MongoDB Cluster", "https://ddmart.com/products/0002")));
            await _productsRepository.InsertAsync(Product.Create(ProductInfo.Create("MongoDB Node", "MongoDB database node"), "0003", 80, Picture.Create("MongoDB Node", "https://ddmart.com/products/0003")));
            await _productsRepository.InsertAsync(Product.Create(ProductInfo.Create("Web Server", "Web server"), "0004", 400, Picture.Create("Web Server", "https://ddmart.com/products/0004")));
            await _productsRepository.UnitOfWork.CommitAsync();
        }
    }
}
