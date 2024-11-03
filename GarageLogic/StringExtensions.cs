using System.Text.RegularExpressions;

namespace GarageLogic
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string input)
        {
            return Regex.Replace(
                input,
                "(\\B[A-Z])",
                " $1"
            );
        }
    }
}
