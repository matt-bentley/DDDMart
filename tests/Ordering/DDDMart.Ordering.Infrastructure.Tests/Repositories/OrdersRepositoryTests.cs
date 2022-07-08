using Autofac;
using DDDMart.Ordering.Application.Services;
using DDDMart.Ordering.Core.Orders.Entities;
using DDDMart.Ordering.Core.Orders.Repositories;
using DDDMart.Ordering.Core.Tests.Builders;
using DDDMart.Ordering.Infrastructure.AutofacModules;
using DDDMart.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DDDMart.Ordering.Infrastructure.Tests.Repositories
{
    [TestClass]
    public class OrdersRepositoryTests
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly IContainer _container;

        public OrdersRepositoryTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new OrderingInfrastructureModule());
            builder.RegisterInstance(new OrderingIntegrationEventMapper()).AsImplementedInterfaces();
            builder.RegisterInstance(_mediator.Object);
            _container = builder.Build();
            _ordersRepository = _container.Resolve<IOrdersRepository>();
        }

        [TestMethod]
        public async Task GivenOrdersRepository_WhenInsert_ThenCanGetById()
        {
            var order = await GenerateAsync();
            var inserted = await _ordersRepository.GetByIdAsync(order.Id);
            inserted.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GivenOrdersRepository_WhenHasEvent_ThenAddToOutbox()
        {
            var order = await GenerateAsync();
            order.Cancel();
            await _ordersRepository.UnitOfWork.CommitAsync();
            _mediator.Verify(e => e.Publish<DomainEvent>(It.IsAny<DomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);

            var context = _container.Resolve<OrderingContext>();
            context.OutboxIntegrationEvents.Count().Should().Be(1);
        }

        [TestMethod]
        public async Task GivenOrdersRepository_WhenInsert_ThenCanGet()
        {
            var order = await GenerateAsync();
            var inserted = await _ordersRepository.GetAll().FirstOrDefaultAsync(e => e.Id == order.Id);
            inserted.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GivenOrdersRepository_WhenInsert_ThenCanGetWithTracking()
        {
            var order = await GenerateAsync();
            var inserted = await _ordersRepository.GetAll(false).FirstOrDefaultAsync(e => e.Id == order.Id);
            inserted.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GivenOrdersRepository_WhenDelete_ThenCantGet()
        {
            var order = await GenerateAsync();
            _ordersRepository.Delete(order);
            await _ordersRepository.UnitOfWork.CommitAsync();
            await AssertDoesNotExist(order.Id);
        }

        [TestMethod]
        public async Task GivenOrdersRepository_WhenRemove_ThenCantGet()
        {
            var order = await GenerateAsync();
            _ordersRepository.Remove(new List<Order>() { order });
            await _ordersRepository.UnitOfWork.CommitAsync();
            await AssertDoesNotExist(order.Id);
        }

        private async Task<Order> GenerateAsync()
        {
            var order = new OrderBuilder().Build();
            await _ordersRepository.InsertAsync(order);
            await _ordersRepository.UnitOfWork.CommitAsync();
            return order;
        }

        private async Task AssertDoesNotExist(Guid id)
        {
            var order = await _ordersRepository.GetByIdAsync(id);
            order.Should().BeNull();
        }
    }
}
