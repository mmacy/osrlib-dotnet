/*************************************************************************
 * File:        Randomizer.cs
 * Author(s):   Marshall Macy II (marshallmacy@gmail.com)
 * Copyright (c) 2018 Marshall Macy II
 *************************************************************************/

using System;

namespace tbrpg.Utility
{
    /// <summary>
    /// Utility class providing helper methods for working with random numbers.
    /// </summary>
    public static class Randomizer
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public static int GetRandomInt(int minVal, int maxVal)
        {
            return _random.Next(minVal, maxVal + 1); //Lower bound is inclusive but upper is exclusive, so add 1
        }
    }
}
