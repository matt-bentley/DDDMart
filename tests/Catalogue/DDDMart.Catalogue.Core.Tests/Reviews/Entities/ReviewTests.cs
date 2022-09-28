using DDDMart.Catalogue.Core.Reviews.DomainEvents;
using DDDMart.Catalogue.Core.Reviews.ValueObjects;
using DDDMart.Catalogue.Core.Tests.Builders;
using DDDMart.Core.Tests;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Catalogue.Core.Tests.Reviews.Entities
{
    [TestClass]
    public class ReviewTests
    {
        [TestMethod]
        public void GivenReview_WhenCreateValid_ThenCreate()
        {
            var comment = Comment.Create("Test comment.");
            var review = new ReviewBuilder()
                            .WithComment(comment.Value)
                            .Build();

            review.Comment.Should().Be(comment);
            review.DomainEvents.Where(e => e is ReviewCreatedDomainEvent).Should().HaveCount(1);
        }

        [TestMethod]
        public void GivenReview_WhenUpdate_ThenUpdateRatingAndComment()
        {
            var comment = Comment.Create("Test comment.");
            var rating = Rating.Create(5);
            var review = new ReviewBuilder()
                            .Build();
            review.Update(rating, comment);

            review.Comment.Should().Be(comment);
            review.Rating.Should().Be(rating);
            review.DomainEvents.Where(e => e is ReviewUpdatedDomainEvent).Should().HaveCount(1);
        }

        [TestMethod]
        public void GivenReview_WhenWhitespaceAfterComment_ThenTrimComment()
        {
            var review = new ReviewBuilder()
                            .WithComment("Whitespace after comment   ")
                            .Build();

            review.Comment.Value.Should().Be("Whitespace after comment");
        }

        [TestMethod]
        public void GivenReview_WhenCommentTooLong_ThenError()
        {
            var review = new ReviewBuilder()
                            .WithComment(StringGenerator.WithLength(201));

            Action action = () => review.Build();

            action.Should().Throw<DomainException>().WithMessage("'Comment' length must be less than or equal to 200.");
        }

        [TestMethod]
        public void GivenReview_WhenCommentNull_ThenError()
        {
            var review = new ReviewBuilder()
                            .WithComment(null);

            Action action = () => review.Build();

            action.Should().Throw<DomainException>().WithMessage("Required input 'Comment' is missing.");
        }

        [TestMethod]
        public void GivenReview_WhenRatingTooHigh_ThenError()
        {
            var review = new ReviewBuilder()
                            .WithRating(6);

            Action action = () => review.Build();

            action.Should().Throw<DomainException>().WithMessage("'Rating' must be less than or equal to 5.");
        }

        [TestMethod]
        public void GivenReview_WhenRatingTooLow_ThenError()
        {
            var review = new ReviewBuilder()
                            .WithRating(0);

            Action action = () => review.Build();

            action.Should().Throw<DomainException>().WithMessage("'Rating' must be greater than or equal to 1.");
        }

        [TestMethod]
        public void GivenReview_WhenRespond_ThenAddResponse()
        {
            var review = new ReviewBuilder()
                            .Build();

            var comment = Comment.Create("response");
            review.Respond(GetResponder(), comment, DateTime.UtcNow);

            review.Responses.Should().HaveCount(1);
            review.DomainEvents.Where(e => e is ReviewResponseCreatedDomainEvent).Should().HaveCount(1);
        }

        [TestMethod]
        public void GivenReview_WhenDeleteResponse_ThenRemove()
        {
            var review = new ReviewBuilder()
                            .Build();

            var responder = GetResponder();
            var comment = Comment.Create("response");
            review.Respond(responder, comment, DateTime.UtcNow);
            review.DeleteResponse(review.Responses.First().Id, responder, DateTime.UtcNow);

            review.Responses.Should().HaveCount(0);
        }

        [TestMethod]
        public void GivenReview_WhenUpdateResponse_ThenUpdate()
        {
            var review = new ReviewBuilder()
                            .Build();

            var responder = GetResponder();
            var comment = Comment.Create("response");
            var updatedComment = Comment.Create("updated response");
            review.Respond(responder, comment, DateTime.UtcNow);
            review.EditResponse(review.Responses.First().Id, responder, updatedComment, DateTime.UtcNow);

            review.Responses.Single().Comment.Should().Be(updatedComment);
        }

        [TestMethod]
        public void GivenReview_WhenDeleteUnknownResponse_ThenNotFound()
        {
            var review = new ReviewBuilder()
                            .Build();

            var responder = GetResponder();
            Action action = () => review.DeleteResponse(Guid.NewGuid(), responder, DateTime.UtcNow);

            action.Should().Throw<NotFoundException>();
        }

        [TestMethod]
        public void GivenReview_WhenDeleteWrongCustomerResponse_ThenUnauthorized()
        {
            var review = new ReviewBuilder()
                            .Build();

            var responder1 = GetResponder("responder 1");
            var responder2 = GetResponder("responder 2");
            var comment = Comment.Create("response");
            review.Respond(responder1, comment, DateTime.UtcNow);
            Action action = () => review.DeleteResponse(review.Responses.First().Id, responder2, DateTime.UtcNow);

            action.Should().Throw<UnauthorizedAccessException>();
        }

        [TestMethod]
        public void GivenReview_WhenCreateResponseOnRatingOver6MonthsOld_ThenError()
        {
            var review = new ReviewBuilder()
                            .Build();

            var comment = Comment.Create("response");
            Action action = () => review.Respond(GetResponder(), comment, DateTime.UtcNow.AddMonths(7));

            action.Should().Throw<DomainException>();
        }

        private static Customer GetResponder(string name = "Responder 1")
        {
            return new Customer(Guid.NewGuid(), name);
        }
    }
}
