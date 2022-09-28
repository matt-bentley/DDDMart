
namespace DDDMart.SharedKernel.Guards
{
    public static partial class GuardClauseExtensions
    {
        public static decimal LessThan(this IGuardClause guardClause, decimal input, int minValue, string parameterName = "Amount", string message = null)
        {
            if (input < minValue)
            {
                Error(message ?? $"'{parameterName}' must be greater than or equal to {minValue}.");
            }
            return input;
        }

        public static decimal LessThanZero(this IGuardClause guardClause, decimal input, string parameterName = "Amount", string message = null)
        {
            return guardClause.LessThan(input, 0, parameterName, message);
        }

        public static decimal GreaterThan(this IGuardClause guardClause, decimal input, int maxValue, string parameterName = "Amount", string message = null)
        {
            if (input > maxValue)
            {
                Error(message ?? $"'{parameterName}' must be less than or equal to {maxValue}.");
            }
            return input;
        }
    }
}
