using DDDMart.Catalogue.Core.Reviews.ValueObjects;
using DDDMart.SharedKernel;

namespace DDDMart.Catalogue.Core.Reviews.Entities
{
    public class ReviewResponse : Entity
    {
        private ReviewResponse(Customer customer, Comment comment)
        {
            Customer = customer;
            Comment = comment;
        }

        internal static ReviewResponse Create(Customer customer, Comment comment)
        {
            return new ReviewResponse(customer, comment);
        }

        public Guid ReviewId { get; private set; }
        public Customer Customer { get; private set; }
        public Comment Comment { get; private set; }

        internal void Update(Comment comment)
        {
            Comment = comment;
        }
    }
}
