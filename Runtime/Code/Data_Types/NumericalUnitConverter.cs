using UnityEngine;

namespace Runtime.Shared
{
    /// <summary>
    /// Utility functions used to convert between numerical units
    /// </summary>
    public static class NumericalUnitConverter
    {
        /// <summary>
        /// A dynamic conversion from one unit to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Returns the converted value. If no conversion was possible then the original value is returned</returns>
        public static float Convert (float value, NumericalUnit from, NumericalUnit to)
        {
            if (from == NumericalUnit.MSPerSecond && to == NumericalUnit.KMPerSecond) return MS_To_KMH(value);
            else return value;
        }

    #region MS To KMH
        private const float _MS_To_KMHFactor = 3.6f;
        public static float MS_To_KMH (int velocity)
        {
            return velocity * _MS_To_KMHFactor;
        }

        public static float MS_To_KMH (float velocity)
        {
            return velocity * _MS_To_KMHFactor;
        }

        public static Vector3 MS_To_KMH (Vector3 velocity)
        {
            return velocity * _MS_To_KMHFactor;
        }
    #endregion MS To KMH
    }
}