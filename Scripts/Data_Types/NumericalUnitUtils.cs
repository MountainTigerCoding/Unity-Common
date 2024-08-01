using System;
using UnityEngine;

namespace Runtime.Shared
{
    public enum NumericalUnit : int
    {
        [Tooltip("Eg: 10")] Generic     = 0,
        [Tooltip("Eg: 10%")] Percentage = 1,
        [Tooltip("Eg: $10")] Money      = 2,

        Kilometres  = 10,
        Metres      = 11,
        Centimetres = 12,

        [Tooltip("Written as m/s")] MSPerSecond  = 20,
        [Tooltip("Written as Km/s")] KMPerSecond = 21,
        [Tooltip("Written as Kn")] Knots         = 22,
    }

    public static class NumericalUnitUtils
    {
    #region Append Numerical Suffix
        public static string AppendNumericalSuffix (string value, NumericalUnit type)
        {
            return AddNumericalSuffix(value, type);
        }

        public static string AppendNumericalSuffix (int value, NumericalUnit type)
        {
            return AddNumericalSuffix(value, type);
        }

        public static string AppendNumericalSuffix (float value, NumericalUnit type)
        {
            return AddNumericalSuffix(value, type);
        }

        public static string AppendNumericalSuffix (double value, NumericalUnit type)
        {
            return AddNumericalSuffix(value, type);
        }

        /// <summary>
        /// Appends a suffix to a value, generally a number
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="value">The value with the suffix being appended too</param>
        /// <param name="type">The type of value</param>
        /// <returns>Returns a string which </returns>
        private static string AddNumericalSuffix <TInput>(TInput value, NumericalUnit type) where TInput : IComparable
        {
            string suffix = type switch
            {
                NumericalUnit.Percentage => "%",
                NumericalUnit.Money => "$",
                _ => "",
            };
            
            return Convert.ToString(value) + suffix;
        }
    #endregion Append Numerical Suffix
    }
}