using DDDMart.Catalogue.Core.Reviews.DomainEvents;
using DDDMart.Catalogue.Core.Reviews.ValueObjects;
using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Catalogue.Core.Reviews.Entities
{
    public class Review : AggregateRoot
    {
        private Review(Guid productId, Rating rating, Comment comment,
            Customer customer, Guid orderId)
        {
            ProductId = productId;
            Rating = rating;
            Comment = comment;
            Customer = customer;
            OrderId = orderId;
        }

        public static Review Create(Guid productId, Rating rating, Comment comment,
            Customer customer, Guid orderId)
        {
            var review = new Review(productId, rating, comment, customer, orderId);
            review.PublishCreated();
            return review;
        }

        public Guid ProductId { get; private set; }
        public Rating Rating { get; private set; }
        public Comment Comment { get; private set; }
        public Customer Customer { get; private set; }
        public Guid OrderId { get; private set; }
        public const int ResponseDeadlineMonths = 6;

        private readonly List<ReviewResponse> _responses = new List<ReviewResponse>();
        public IReadOnlyCollection<ReviewResponse> Responses => _responses.AsReadOnly();

        private void PublishCreated()
        {
            AddDomainEvent(new ReviewCreatedDomainEvent(Id, Customer, OrderId, Rating, Comment));
        }

        public void Update(Rating rating, Comment comment)
        {
            Rating = rating;
            Comment = comment;
            AddDomainEvent(new ReviewUpdatedDomainEvent(Id, Customer, OrderId, Rating, Comment));
        }

        public void Respond(Customer customer, Comment comment, DateTime responseDate)
        {
            CheckIfResponseDeadlinePassed(responseDate);
            var response = ReviewResponse.Create(customer, comment);
            _responses.Add(response);
            AddDomainEvent(new ReviewResponseCreatedDomainEvent(Id, response.Id, customer, comment));
        }

        public void EditResponse(Guid responseId, Customer customer, Comment comment, DateTime responseDate)
        {
            var response = GetCustomerResponse(responseId, customer, responseDate);
            response.Update(comment);
        }

        public void DeleteResponse(Guid responseId, Customer customer, DateTime responseDate)
        {
            var response = GetCustomerResponse(responseId, customer, responseDate);
            _responses.Remove(response);
        }

        private ReviewResponse GetCustomerResponse(Guid responseId, Customer customer, DateTime responseDate)
        {
            CheckIfResponseDeadlinePassed(responseDate);
            var response = _responses.FirstOrDefault(e => e.Id == responseId);
            if (response == null)
            {
                throw new NotFoundException($"Response not found: {responseId}");
            }
            if (!response.Customer.Equals(customer))
            {
                throw new UnauthorizedAccessException();
            }
            return response;
        }

        private void CheckIfResponseDeadlinePassed(DateTime responseDate)
        {
            if (responseDate > CreatedDate.AddMonths(ResponseDeadlineMonths))
            {
                throw new DomainException($"Cannot respond to review that is over {ResponseDeadlineMonths} months old");
            }
        }
    }
}
