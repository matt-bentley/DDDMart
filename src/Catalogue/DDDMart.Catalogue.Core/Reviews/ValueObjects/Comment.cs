using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Guards;

namespace DDDMart.Catalogue.Core.Reviews.ValueObjects
{
    public class Comment : ValueObject<Comment>
    {
        private Comment(string comment)
        {
            Value = comment;
        }

        public static Comment Create(string comment)
        {
            comment = (comment ?? string.Empty).Trim();
            Guard.Against.NullOrEmpty(comment, "Comment");
            Guard.Against.LengthGreaterThan(comment, 200, "Comment");
            return new Comment(comment);
        }

        public string Value { get; private set; }

        protected override int GetValueHashCode()
        {
            return Value.GetHashCode();
        }

        protected override bool ValueEquals(Comment other)
        {
            return Value.Equals(other.Value);
        }
    }
}
