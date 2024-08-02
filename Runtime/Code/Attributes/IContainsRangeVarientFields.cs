//using System.Reflection;
using UnityEngine;
//using Runtime.Shared;

namespace Runtime.Environment.Weather
{
    public interface IContainsRangeVarientFields
    {
        public static float Random (Vector2 range) => UnityEngine.Random.Range(range.x, range.y);
#if UNITY_EDITOR
        public static void InitRange (ref Vector2 range, float value, float offset)
        {
            // Prefered way by using the attribute
            //MinMaxSliderAttribute slider = typeof(Vector2).GetProperty("Name").GetCustomAttribute<MinMaxSliderAttribute>();
            //range = new Vector2(Mathf.Max(value - offset, slider.Min), Mathf.Min(value + offset, slider.Max));

            range = new Vector2(value - offset, value + offset);
        }
#endif
    }
}