using DDDMart.Catalogue.Core.Reviews.Entities;
using DDDMart.Catalogue.Core.Reviews.ValueObjects;

namespace DDDMart.Catalogue.Core.Tests.Builders
{
    public class ReviewBuilder
    {
        private Guid _productId = Guid.NewGuid();
        private Guid _orderId = Guid.NewGuid();
        private Guid _customerId = Guid.NewGuid();
        private string _customerName = "Test User";
        private int _rating = 4;
        private string _comment = "This is a test comment.";

        public Review Build()
        {
            var rating = Rating.Create(_rating);
            var comment = Comment.Create(_comment);
            var customer = new Customer(_customerId, _customerName);
            var review = Review.Create(_productId, rating, comment, customer, _orderId);
            return review;
        }

        public ReviewBuilder WithRating(int rating)
        {
            _rating = rating;
            return this;
        }

        public ReviewBuilder WithComment(string comment)
        {
            _comment = comment;
            return this;
        }
    }
}
