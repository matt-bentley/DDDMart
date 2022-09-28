using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Guards;

namespace DDDMart.Catalogue.Core.Reviews.ValueObjects
{
    public class Rating : ValueObject<Rating>
    {
        private Rating(int rating)
        {
            Value = rating;
        }

        public static Rating Create(int rating)
        {
            Guard.Against.LessThan(rating, 1, "Rating");
            Guard.Against.GreaterThan(rating, 5, "Rating");
            return new Rating(rating);
        }

        public int Value { get; private set; }

        protected override int GetValueHashCode()
        {
            return Value.GetHashCode();
        }

        protected override bool ValueEquals(Rating other)
        {
            return Value.Equals(other.Value);
        }
    }
}
