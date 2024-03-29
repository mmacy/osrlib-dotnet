using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;

namespace osrlib.Dice
{
    /// <summary>
    /// Provides utility methods for working with dice notation strings.
    /// </summary>
    public static class DiceUtility
    {
        /// <summary>
        /// Sanitizes the input string to ensure it is in the correct format "NdN" or "dN".
        /// </summary>
        /// <param name="diceNotation">The input string to sanitize.</param>
        /// <returns>The sanitized input string in the standardized "NdN" format, even if the original input was in the "dN" format.</returns>
        /// <example>
        /// The following example demonstrates the use of the SanitizeDiceNotation method:
        /// <code>
        /// string diceNotation = "2d6";
        /// diceNotation = osrlib.Dice.DiceUtility.SanitizeDiceNotation(diceNotation);
        /// </code>
        /// The following example demonstrates the use of the SanitizeDiceNotation method for the "dN" format:
        /// <code>
        /// string diceNotation = "d6";
        /// diceNotation = osrlib.Dice.DiceUtility.SanitizeDiceNotation(diceNotation);
        /// </code>
        /// </example>
        public static string SanitizeDiceNotation(string diceNotation)
        {
            // Check for null or empty input
            if (string.IsNullOrEmpty(diceNotation))
            {
                throw new ArgumentException("Dice notation cannot be null or empty.");
            }

            // Regular expression matching the required NdN format
            Regex regex = new Regex(@"^([1-9]\d{0,2})d([1-9]\d{0,1})$", RegexOptions.IgnoreCase);

            // If the input is just "dN", add a "1" to the beginning to make it "1dN"
            if (diceNotation.StartsWith("d", StringComparison.OrdinalIgnoreCase))
            {
                diceNotation = "1" + diceNotation;
            }

            // Remove any invalid characters and whitespace from the string
            diceNotation = Regex.Replace(diceNotation, @"[^0-9dD]", string.Empty);
            diceNotation = diceNotation.Trim();

            // Check that the string matches the required format
            if (!regex.IsMatch(diceNotation))
            {
                throw new ArgumentException(
                    ErrorConstants.DiceNotationInvalid);
            }

            // Get the number of sides in the dice notation
            int sides = int.Parse(diceNotation.Split('d')[1]);

            // Check that the number of sides is one of the values in the DieType enum
            if (!Enum.IsDefined(typeof(DieType), sides))
            {
                throw new ArgumentException(ErrorConstants.DiceCountInvalid);
            }

            // Convert the notation to lowercase for consistency
            diceNotation = diceNotation.ToLowerInvariant();

            return diceNotation;
        }

    }
}
