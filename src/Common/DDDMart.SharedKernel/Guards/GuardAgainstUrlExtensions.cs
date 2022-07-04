
namespace DDDMart.SharedKernel.Guards
{
    public static partial class GuardClauseExtensions
    {
        public static string InvalidUrl(this IGuardClause guardClause, string url, string parameterName = "URL", string message = null)
        {
            try
            {
                _ = new Uri(url);
            }
            catch
            {
                Error(message ?? $"Must have a valid {parameterName}.");
            }
            return url;
        }
    }
}
