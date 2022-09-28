
namespace DDDMart.SharedKernel.Guards
{
    public static partial class GuardClauseExtensions
    {
        public static string LengthGreaterThan(this IGuardClause guardClause, string input, int maxLength, string parameterName = "Value", string message = null)
        {
            if (input.Length > maxLength)
            {
                Error(message ?? $"'{parameterName}' length must be less than or equal to {maxLength}.");
            }
            return input;
        }
    }
}
