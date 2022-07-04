
namespace DDDMart.SharedKernel.Guards
{
    public static partial class GuardClauseExtensions
    {
        public static decimal LessThanZero(this IGuardClause guardClause, decimal input, string parameterName = "Amount", string message = null)
        {
            if (input < 0)
            {
                Error(message ?? $"'{parameterName}' must be greater than or equal to 0.");
            }
            return input;
        }
    }
}
