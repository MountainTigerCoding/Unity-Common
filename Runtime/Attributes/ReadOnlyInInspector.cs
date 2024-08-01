using UnityEngine;

namespace Runtime.Shared
{
    /// <summary>
    /// Makes a serializable field unchangedable via the inspector
    /// </summary>
    public sealed class ReadOnlyInInspectorAttribute : PropertyAttribute {}
}