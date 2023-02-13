using System;
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
            // Use a regular expression to match the required format
            Regex regex = new Regex(@"^[0-9]*d[0-9]+$");

            // Convert the input to lowercase
            diceNotation = diceNotation.ToLowerInvariant();

            // Remove any invalid characters and whitespace from the string
            diceNotation = Regex.Replace(diceNotation, @"[^0-9d]", string.Empty);
            diceNotation = diceNotation.Trim();

            // If the input is just "dN", add a "1" to the beginning to make it "1dN"
            if (diceNotation.StartsWith("d"))
            {
                diceNotation = "1" + diceNotation;
            }

            // Check that the string matches the required format
            if (!regex.IsMatch(diceNotation))
            {
                throw new ArgumentException(
                    "Incorrect dice notation format. Use NdN, where N is first the number of dice and then the number of sides.");
            }

            return diceNotation;
        }
    }
}
