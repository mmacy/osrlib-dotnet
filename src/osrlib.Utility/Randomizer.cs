using System;

namespace osrlib.Utility
{
    /// <summary>
    /// Utility class providing helper methods for working with random numbers.
    /// </summary>
    public static class Randomizer
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Gets a random number between the minimum and maximum value (lower and upper inclusive).
        /// </summary>
        /// <param name="minVal">The minimum value for the random range (inclusive).</param>
        /// <param name="maxVal">The maximum value for the random range (inclusive).</param>
        public static int GetRandomInt(int minVal, int maxVal)
        {
            return _random.Next(minVal, maxVal + 1); //Lower bound is inclusive but upper is exclusive, so add 1
        }
    }
}
