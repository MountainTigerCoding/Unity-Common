// Taken from: https://gist.github.com/aarthificial/f2dbb58e4dbafd0a93713a380b9612af
// Adapted from: https://www.youtube.com/watch?v=uZmWgQ7cLNI&list=WL&index=28
using UnityEngine;

namespace Runtime.Shared
{
    /// <summary>
    /// A container class which can be enabled and disabled via code or the inspector 
    /// </summary>
    /// <typeparam name="T">data type must always have [System.Serializable] otherwise null reference errors will occur</typeparam>
    [System.Serializable]
    public class Optional <T>
    {
    #region Fields
        public bool Enabled;
        [SerializeField] private T _value;
    #endregion Fields

    #region Properties
        public T Value
        {
            get => _value;
            set {
                if (!Enabled) {
                    Debug.LogError("Cannot set Optional<>.Value while it's disabled");
                    return;
                }
                _value = value;
            }
        }
    #endregion Properties

        public Optional (bool enabled, T value)
        {
            Enabled = enabled;
            _value = value;
        }

        /// <summary>
        /// Sets the value while ignoring whether it is enable or not
        /// </summary>
        /// <param name="value">The value being set</param>
        public void SetValue_IgnoreEnabled (T value)
        {
            _value = value;
        }
    }
}