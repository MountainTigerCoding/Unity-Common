using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Shared
{
    // Needs replacing with an enumeration that uses flags instead
    public enum Axis : int
    {
        Everything = 0,
        X = 1,
        Y = 2,
        Z = 3,
    }

    public static class Utils
    {
    #region Extentions
        /// <summary>
        /// Creates and returns a clone of any given scriptable object.
        /// </summary>
        public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
        {
            if (scriptableObject == null)
            {
                Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
                return (T)ScriptableObject.CreateInstance(typeof(T));
            }

            T instance = UnityEngine.Object.Instantiate(scriptableObject);
            instance.name = scriptableObject.name; // remove (Clone) from name
            return instance;
        }
    #endregion Extentions

    #region Mathematics
        /// <summary>
        /// Rounds a float with a maxiumum number of decimal places
        /// </summary>
        /// <param name="value"></param>
        /// <param name="places">The number of placed after the decimal point</param>
        /// <returns></returns>
        public static float Round (float value, int places)
        {
            float mult = Mathf.Pow(10, places);
            return Mathf.Round(value * mult) / mult;
        }

        /// <summary>
        /// Rounds a double with a maxiumum number of decimal places
        /// </summary>
        /// <param name="value"></param>
        /// <param name="places">The number of placed after the decimal point</param>
        /// <returns></returns>
        public static double Round (double value, int places)
        {
            float mult = Mathf.Pow(10, places);
            return Math.Round(value * mult) / mult;
        }

        /// <summary>
        /// This node returns the posterized (also known as quantized) value of the input In into an amount of values specified by the number of Steps
        /// </summary>
        /// <param name="value"></param>
        /// <param name="step">The number of steps applied to the value</param>
        /// <returns>Returns the posterized value</returns>
        public static int Posterize (int value, int step)
        {
            return Mathf.FloorToInt(value / (1 / step)) * (1 / step);
        }

        /// <summary>
        /// This node returns the posterized (also known as quantized) value of the input In into an amount of values specified by the number of Steps
        /// </summary>
        /// <param name="value"></param>
        /// <param name="step">The number of steps applied to the value</param>
        /// <returns>Returns the posterized value</returns>
        public static float Posterize (float value, float step)
        {
            return Mathf.Floor(value / (1 / step)) * (1 / step);
        }

        /// <summary>
        /// Linearly interpolates floats from a, b, c based on t
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="t">Float for blending between colors a, b and c.</param>
        /// <returns></returns>
        public static float Lerp (float a, float b, float c, float t)
        {
            if (t <= 0.5f) return Mathf.Lerp(a, b, t * 2);
            else return Mathf.Lerp(b, c, (t * 2) - 1);
        }

        /// <summary>
        /// Linearly interpolates positions or directions from a, b, c based on t
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="t">Float for blending between colors a, b and c.</param>
        /// <returns></returns>
        public static Vector3 Lerp (Vector3 a, Vector3 b, Vector3 c, float t)
        {
            if (t <= 0.5f) return Vector3.Lerp(a, b, t * 2);
            else return Vector3.Lerp(b, c, (t * 2) - 1);
        }

        /// <summary>
        /// Linearly interpolates colors from a, b, c based on t
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="t"></param>
        /// <returns>Float for blending between colors a, b and c.</returns>
        public static Color Lerp (Color a, Color b, Color c, float t)
        {
            if (t <= 0.5f) return Color.Lerp(a, b, t * 2);
            else return Color.Lerp(b, c, (t * 2) - 1);
        }

        /// <summary>
        /// Linearly interpolates angles in degrees from a, b, c based on t
        /// </summary>
        /// <param name="a">First degree</param>
        /// <param name="b">Middle degree</param>
        /// <param name="c">Last degree</param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float LerpAngle (float a, float b, float c, float t)
        {
            if (t <= 0.5f) return Mathf.LerpAngle(a, b, t * 2);
            else return Mathf.LerpAngle(b, c, (t * 2) - 1);
        }

        /// <summary>
        /// Clamps a Range/MinMax vector2 so that the x and y don't overlap
        /// </summary>
        /// <param name="values">Should be a vector 2 with the MinMaxRange attribute</param>
        /// <param name="minLimit"></param>
        /// <param name="maxLimit"></param>
        /// <returns></returns>
        public static Vector2 ValidateMinMaxFloats (Vector2 values, float minLimit, float maxLimit)
        { 
            values.x = Mathf.Clamp(values.x, minLimit, values.y);
            values.y = Mathf.Clamp(values.y, values.x, maxLimit);
            return values;
        }

        /// <summary>
        /// Converts an enumeration's members into a string representation intended for UI purposes.
        /// </summary>
        /// <param name="enum">The enumeration being cconverted</param>
        /// <returns></returns>
        public static string[] ToArray (this Enum @enum)
        {
            return Enum.GetNames(@enum.GetType());
        }
    #endregion Mathematics

    #region Angles
        /// <summary>
        /// Removes negative angles or ones greater than 360, by looping it around
        /// </summary>
        /// <param name="angles">The angle in degrees</param>
        /// <returns>A positive ranging from 0 to 360</returns>
        public static Vector3 LoopAngleDegrees (Vector3 eulerAngles)
        {
            return new Vector3(LoopAngleDegrees(eulerAngles.x), LoopAngleDegrees(eulerAngles.y), LoopAngleDegrees(eulerAngles.z));
        }

        /// <summary>
        /// Removes negative angles or ones greater than 360, by looping it
        /// </summary>
        /// <param name="angle">The angle in degrees</param>
        /// <returns>A positive angle that ranges from 0 to 360</returns>
        public static float LoopAngleDegrees (float angleDegrees)
        {
            if (angleDegrees < 0) return 360 + angleDegrees;
            return angleDegrees %= 360;
        }

        /// <summary>
        /// Calculates the angle from the origin to the target
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns>The angle in degrees</returns>
        public static float PointTowards (Vector2 origin, Vector2 target)
        {
            Vector2 delta = target - origin;
            float angle;
            if (delta.y == 0) {
                if (delta.x < 0) {
                    return 270;
                } else {
                    return 90;
                }
            } else {
                if (delta.y < 0f) {
                    angle = 180 + Mathf.Atan(delta.x / delta.y) * Mathf.Rad2Deg;
                    return LoopAngleDegrees(angle);
                } else {
                    angle = Mathf.Atan(delta.x / delta.y) * Mathf.Rad2Deg;
                    return LoopAngleDegrees(angle);
                }
            }
        }
    #endregion Angles

    #region Vectors
        /// <summary>
        /// Sophisicated version of Quaternion.AngleAxis
        /// </summary>
        public static Vector3 RotateDirection (Vector3 direction, float angle, Vector3 up)
        {
            return Quaternion.AngleAxis(angle, up) * direction;
        }

        /// <summary>
        /// Clamps a vector3 so it's individual components will never be outside the inputted range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minMagnitude">has to be less than the max magnitude</param>
        /// <param name="maxMagnitude">Has to be larger than the min magnitude</param>
        /// <returns></returns>
        public static Vector3 ClampVector (Vector3 value, float minMagnitude, float maxMagnitude)
        {
            return new
            (
                value.x = Mathf.Clamp(value.x, minMagnitude, maxMagnitude),
                value.y = Mathf.Clamp(value.y, minMagnitude, maxMagnitude),
                value.z = Mathf.Clamp(value.z, minMagnitude, maxMagnitude)
            );
        }

        /// <summary>
        /// Clamps a vector2 so it's individual components will never be outside the inputted range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minMagnitude">has to be less than the max magnitude</param>
        /// <param name="maxMagnitude">Has to be larger than the min magnitude</param>
        /// <returns></returns>
        public static Vector2 ClampVector (Vector2 value, float minMagnitude, float maxMagnitude)
        {
            return new
            (
                value.x = Mathf.Clamp(value.x, minMagnitude, maxMagnitude),
                value.y = Mathf.Clamp(value.y, minMagnitude, maxMagnitude)
            );
        }

        public static Vector3 SelectReplaceVector (Vector3 vector, Axis keepAxis, Vector3 overrideVector)
        {
            return keepAxis switch
            {
                Axis.X => new(vector.x, overrideVector.y, overrideVector.z),
                Axis.Y => new(overrideVector.x, vector.y, overrideVector.z),
                Axis.Z => new(overrideVector.x, overrideVector.y, vector.z),
                _ => vector,
            };
        }

        public static float ComponentFromVector (Axis axis, Vector3 vector)
        {
            return axis switch {
                Axis.X => vector.x,
                Axis.Y => vector.y,
                Axis.Z => vector.z,
                _ => -1f,
            };
        }

        public static Vector3 ReplaceVectorComponent (Axis axis, Vector3 vector, Vector3 replacement)
        {
            return ReplaceVectorComponent(axis, vector, ComponentFromVector(axis, replacement));
        }

        /// <summary>
        /// Replaces specific component of a vector
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vector"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static Vector3 ReplaceVectorComponent (Axis axis, Vector3 vector, float replacement)
        {
            switch (axis) {
                case Axis.X:
                    vector.x = replacement;
                    break;

                case Axis.Y:
                    vector.y = replacement;
                    break;

                case Axis.Z:
                    vector.z = replacement;
                    break;
            }
            return vector; 
        }

        /// <summary>
        /// Moves the 3D vector in a direction by a distance
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="direction">The direction of translation</param>
        /// <param name="distance">the distance of the translation</param>
        /// <returns>The translated vector</returns>
        public static Vector3 TranslateVector (Vector3 vector, Vector3 direction, float distance)
        {
            return vector + direction.normalized * distance;
        }

        /// <summary>
        /// Moves the 3D vector on a 2D plane along the X and Z axes in a direction by a distance
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="direction">The direction of translation</param>
        /// <param name="distance">the distance of the translation</param>
        /// <returns>The translated vector</returns>
        public static Vector3 TranslateVector (Vector3 vector, Vector2 direction, float distance)
        {
            direction.Normalize();
            return vector + new Vector3(direction.x * distance, 0, direction.y * distance);
        }

        /// <summary>
        /// Moves the 3D vector in a direction by a distance
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="direction">The direction of translation</param>
        /// <param name="distance">the distance of the translation</param>
        /// <returns>The translated vector</returns>
        public static Vector2 TranslateVector (Vector2 vector, Vector2 direction, float distance)
        {
            return vector + direction.normalized * distance;
        }
    #endregion Vectors

    #region Unity Components
        /// <summary>
        /// Finds the first layer of children parented to a transform
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>All GameObjects found in the first layer</returns>
        public static Transform[] GetChildren (Transform parent)
        {
            List<Transform> transformList = new();
            foreach (Transform child in parent) transformList.Add(child);
            return transformList.ToArray();
        }

        /// <summary>
        /// Finds all children and parents under a transform
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="transformList"></param>
        /// <returns>All GameObjects under the transform</returns>
        public static Transform[] GetAllChildren (Transform parent, List<Transform> transformList = null)
        {
            transformList ??= new List<Transform>();
            foreach (Transform child in parent)
            {
                transformList.Add(child);
                GetAllChildren(child, transformList);
            }
            return transformList.ToArray();
        }

        /// <summary>
        /// Finds all components (of a single type) in a List of GameObjects
        /// </summary>
        /// <typeparam name="T">The component type</typeparam>
        /// <param name="transforms">the transformed being checked</param>
        /// <returns>All found components</returns>
        public static T[] TryGetMassComponents <T>(List<Transform> transforms) where T : MonoBehaviour
        {
            return TryGetMassComponents<T>(transforms.ToArray());
        }

        /// <summary>
        /// Finds all components (of a single type) in an array of GameObjects
        /// </summary>
        /// <typeparam name="T">The component type</typeparam>
        /// <param name="transforms">the transformed being checked</param>
        /// <returns>All found components</returns>
        public static T[] TryGetMassComponents <T>(Transform[] transforms) where T : MonoBehaviour
        {
            List<T> components = new();
            foreach (Transform transform in transforms)
            {
                if (transform.TryGetComponent(out T component)) components.Add(component);
            }
            return components.ToArray();
        }
    #endregion Unity Components

    #region Meshes
        /// <summary>
        /// Calculates the volumes of a mesh
        /// Adapted from https://stackoverflow.com/questions/57236085/get-volume-of-an-object-in-unity3d
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns>The volume in the same measurement unit as the mesh</returns>
        public static float VolumeOfMesh (Mesh mesh)
        {
            float volume = 0;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];
                volume += VolumeOfTriangle(p1, p2, p3);
            }
            return Mathf.Abs(volume);
        }

        /// <summary>
        /// Calculates the Signed volume of a triangle,
        /// Adapated from https://stackoverflow.com/questions/57236085/get-volume-of-an-object-in-unity3d
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns>The volume in the same measurement unit as the triangle</returns>
        private static float VolumeOfTriangle (Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float v321 = p3.x * p2.y * p1.z;
            float v231 = p2.x * p3.y * p1.z;
            float v312 = p3.x * p1.y * p2.z;
            float v132 = p1.x * p3.y * p2.z;
            float v213 = p2.x * p1.y * p3.z;
            float v123 = p1.x * p2.y * p3.z;
            return 1 / 6 * (-v321 + v231 + v312 - v132 - v213 + v123);
        }
    #endregion Meshes
    }
}