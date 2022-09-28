
namespace DDDMart.Core.Tests
{
    public static class StringGenerator
    {
        public static string WithLength(int length, char character = 'x')
        {
            return string.Join("", Enumerable.Range(0, length).Select(e => character).ToArray());
        }
    }
}
