using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.SharedKernel.Guards
{
    public static partial class GuardClauseExtensions
    {
        private static void Error(string message)
        {
            throw new DomainException(message);
        }
    }
}
