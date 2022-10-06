using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using RichPackage.FunctionalProgramming;

using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

namespace RichPackage
{
    /// <summary>
    /// Random helpers
    /// </summary>
    public static class RichRandom
    {
        /// <summary>
        /// Has <paramref name="n"/>% probability of returning <see langword="true"/>.
        /// </summary>
        /// <param name="n">[0, 1]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(float n) => Random.Range(float.Epsilon, 1f) <= n;

        /// <summary>
        /// Has an <paramref name="n"/> : 100 probability of returning <see langword="true"/>.
        /// </summary>
        /// <param name="n">[0, 100]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(int n) => Random.Range(1, 101) <= n;

        /// <param name="dice">[0, inf)</param>
        /// <param name="sides">[1, inf)</param>
        /// <param name="mod">(whatever, whatever)</param>
        /// <returns>e.g. 2d6 + mod</returns>
        public static int RollDice(int dice, int sides, int mod = 0)
        {   // validate
            Debug.Assert(dice >= 0 && sides > 0, 
                $"[Utility] Invalid RollDice input: {dice} {sides}.");

            int result = mod; // return value
            int upperRandomLimit = sides + 1; // because upper bound of random is exclusive
            for (; dice > 0; --dice)
                result += Random.Range(1, upperRandomLimit);
            return result;
        }

        /// <summary>
        /// Simulate a roll of a <paramref name="sides"/>-sided die.
        /// </summary>
        /// <param name="sides"></param>
        /// <returns>[1, <paramref name="sides"/>]</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RollDie(int sides) => Random.Range(1, sides + 1);

        /// <summary>
        /// Standard normal random. 0 is average, and the stdev is 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
        {
            return EvaluateRandomGaussian(minValue, maxValue, Random.value, Random.value);
        }

        // https://answers.unity.com/questions/421968/normal-distribution-random.html
        /// <summary>
        /// Standard normal random. 0 is average, and the stdev is 1.
        /// </summary>
        /// <param name="a">a random value [0, 1]</param>
        /// <param name="b">a random value [0, 1]</param>
        public static float EvaluateRandomGaussian(
            float minValue, float maxValue, float a, float b)
        {
            float u, v, S;

            do
            {
                u = 2 * a - 1;
                v = 2 * b - 1;
                S = u * u + v * v;
            }
            while (S >= 1 || S == 0);

            // Standard Normal Distribution
            float std = u * Math.Sqrt(-2.0f * Math.Log(S) / S).ToFloat();

            // Normal Distribution centered between the min and max value
            // and clamped following the "three-sigma rule"
            float mean = (minValue + maxValue) / 2.0f;
            float sigma = (maxValue - mean) / 3.0f;
            return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
        }
    }
}
